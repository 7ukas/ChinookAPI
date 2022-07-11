namespace ChinookAPI;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : Controller {
    private readonly ChinookContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ChinookContext context, IMapper mapper, ILogger<CustomersController> logger) {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // POST: api/Customers
    [HttpPost]
    public async Task<ActionResult<CustomerCreateDto>> PostCustomer([FromBody] CustomerCreateDto customerDto) {
        try {
            if (customerDto == null) {
                _logger.LogWarning("\"customerDto\" object sent from the client is null");
                return BadRequest("Customer object is null");
            }

            if (!ModelState.IsValid) {
                _logger.LogWarning("\"customerDto\" object sent from the client is invalid");
                return BadRequest("Model object is invalid");
            }

            var customer = _mapper.Map<Customer>(customerDto);

            // Set Id for 'Customer'
            customer.CustomerId = _context.Customers.Count() > 0 ?
                _context.Customers.Max(x => x.CustomerId) + 1 : 1;

            // Adding customer
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"POST: Customer (ID: {customer.CustomerId}) - {customer.FirstName} {customer.LastName}");

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        } catch (Exception ex) {
            _logger.LogError($"PostCustomer(customerDto) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // POST: api/Customers/5/invoices
    [HttpPost("{customerId}/invoices")]
    public async Task<ActionResult<InvoiceCreateDto>> PostCustomerInvoice(int customerId, [FromBody] InvoiceCreateDto invoiceDto) {
        try {
            // Looking for customer
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null) {
                _logger.LogWarning($"Can not find \"customer\" object (ID: {customerId}) in database");
                return BadRequest($"Customer (ID: {customerId}) does not exist");
            }

            if (invoiceDto == null) {
                _logger.LogWarning("\"invoiceDto\" object sent from the client is null");
                return BadRequest("Invoice object is null");
            }

            if (!ModelState.IsValid) {
                _logger.LogWarning("\"invoiceDto\" object sent from the client is invalid");
                return BadRequest("Model object is invalid");
            }

            // Map needed columns from 'Customer' to 'Invoice' and set up 'InvoiceLines'
            var invoice = _mapper.Map<Invoice>(invoiceDto);
            ICollection<InvoiceLine> invoiceLines = invoice.InvoiceLines;

            foreach (var invoiceLine in invoiceLines) {
                var track = await _context.Tracks.FindAsync(invoiceLine.TrackId);

                if (invoiceLine.Track == null || invoiceLine.Quantity < 1) {
                    _logger.LogWarning("\"invoiceLine\" object sent from the client is invalid");
                    return BadRequest("InvoiceLine object is invalid");
                }

                // Set Id for 'InvoiceLine'
                invoiceLine.InvoiceLineId = _context.InvoiceLines.Count() > 0 ?
                _context.InvoiceLines.Max(x => x.InvoiceLineId) + 1 : 1;

                invoiceLine.InvoiceId = invoice.InvoiceId;
                invoiceLine.Track = track;
                invoiceLine.UnitPrice = invoiceLine.Track.UnitPrice;

                // Adding invoice lines
                await _context.InvoiceLines.AddAsync(invoiceLine);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"POST: InvoiceLine (ID: {invoiceLine.InvoiceLineId})");
            }

            invoice = _mapper.Map<Invoice>(customer);
            invoice.InvoiceLines = invoiceLines;

            // Set Id for 'Invoice'
            invoice.InvoiceId = _context.Invoices.Count() > 0 ?
                _context.Invoices.Max(x => x.InvoiceId) + 1 : 1;

            invoice.InvoiceDate = DateTime.Now;
            invoice.Total = invoice.InvoiceLines.Sum(x => x.UnitPrice * x.Quantity);

            // Adding invoice
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"POST: Invoice (ID: {invoice.InvoiceId}) - " +
                $"{invoice.Customer.FirstName} {invoice.Customer.LastName} ");

            return CreatedAtAction(nameof(GetCustomer), new { id = customerId }, customer);
        } catch (Exception ex) {
            _logger.LogError($"PostCustomerInvoice(id, invoiceDto) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // GET: api/Customers/page=5
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomersReadDto>>> GetCustomers(int page) {
        try {
            var customers = await _context.Customers
            .ProjectTo<CustomersReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

            // Set page
            if (page < 1 || page > ((customers.Count - 1) / Page.Rows) + 1) {
                page = 1;
            }

            int index = (page - 1) * Page.Rows;
            int count = (page * Page.Rows) > customers.Count ?
                (customers.Count % Page.Rows) : Page.Rows;

            customers = customers.OrderBy(x => x.CustomerId).ToList().GetRange(index, count);

            return Ok(customers);
        } catch (Exception ex) {
            _logger.LogError($"GetCustomers(page) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomersReadDto>> GetCustomer(int id) {
        try {
            var customer = await _context.Customers
                .ProjectTo<CustomersReadDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.CustomerId == id);

            return Ok(customer);
        } catch (Exception ex) {
            _logger.LogError($"GetCustomer(id) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // GET: api/Customers/5/invoices
    [HttpGet("{customerId}/invoices")]
    public async Task<ActionResult<CustomerInvoicesReadDto>> GetCustomerInvoices(int customerId, int id) {
        try {
            var customer = await _context.Customers
                .ProjectTo<CustomerInvoicesReadDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.CustomerId == customerId);

            // Get Invoice by InvoiceId
            if (id > 0) {
                var invoice = await _context.Invoices.FindAsync(id);

                if (invoice != null) {
                    customer.Invoices = customer.Invoices
                        .Where(x => x.InvoiceId == id).Select(x => x).ToList();
                }
            }

            return Ok(customer);
        } catch (Exception ex) {
            _logger.LogError($"GetCustomerInvoices(customerId, id) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // PUT: api/Customer/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(int id, [FromBody] CustomerUpdateDto customerDto) {
        try {
            // Looking for customer
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null) {
                _logger.LogWarning("\"customerDto\" object sent from the client is null");
                return BadRequest($"Customer (ID: {id}) does not exist");
            }

            if (!ModelState.IsValid) {
                _logger.LogWarning("\"customerDto\" object sent from the client is invalid");
                return BadRequest("Model object is invalid");
            }

            _mapper.Map(customerDto, customer);

            // Modifying customer
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"PUT: Customer (ID: {id})");

            return CreatedAtAction(nameof(GetCustomer), new { id = id }, customer);
        } catch (Exception ex) {
            _logger.LogError($"PutCustomer(id, customerDto) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // DELETE: api/Customer/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id) {
        try {
            // Looking for customer
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null) {
                _logger.LogWarning($"Can not find \"customer\" object (ID: {id}) in database");
                return BadRequest($"Customer (ID: {id}) does not exist");
            }

            // Removing all customer's invoices
            await DeleteCustomerInvoices(id, 0);

            // Removing customer
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"DELETE: Customer (ID: {id})");

            return NoContent();
        } catch (Exception ex) {
            _logger.LogError($"DeleteCustomer(id) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // DELETE: api/Customers/5/invoices
    [HttpDelete("{customerId}/invoices")]
    public async Task<IActionResult> DeleteCustomerInvoices(int customerId, int id) {
        try {
            // Looking for customer
            var customer = await _context.Customers
                .ProjectTo<CustomerInvoicesReadDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.CustomerId == customerId);

            if (customer == null) {
                _logger.LogWarning($"Can not find \"customer\" object (ID: {customerId}) in database");
                return BadRequest($"Customer (ID: {customerId}) does not exist");
            }

            // ID - invalid
            if (id < 0) {
                _logger.LogWarning($"Specified ID: {id} is invalid");
                return BadRequest($"Specified ID: {id} is invalid");
            } 
            
            // ID - specified - Removing invoice and all it's invoice lines
            else if (id != 0) {
                var invoice = await _context.Invoices.FindAsync(id);

                if (invoice == null) {
                    _logger.LogWarning($"Can not find \"invoice\" object (ID: {id}) in database");
                    return BadRequest($"Invoice (ID: {id}) does not exist");
                }

                var invoiceLines = await _context.InvoiceLines
                    .Where(x => x.InvoiceId == id).ToListAsync();

                // Removing all invoice lines
                foreach (var invoiceLine in invoiceLines) {
                    _context.InvoiceLines.Remove(invoiceLine);
                    _logger.LogInformation($"DELETE: InvoiceLine (ID: {invoiceLine.InvoiceLineId})");
                }

                // Removing invoice
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"DELETE: Invoice (ID: {id})");
            }

            // ID - not specified - Removing all invoices and invoice lines within them
            else {
                foreach (var _invoice in customer.Invoices) {
                    // Looking for invoice
                    int invoiceId = _invoice.InvoiceId;
                    var invoice = await _context.Invoices.FindAsync(invoiceId);

                    if (invoice == null) {
                        _logger.LogWarning($"Can not find \"invoice\" object (ID: {invoiceId}) in database");
                        return BadRequest($"Invoice (ID: {invoiceId}) does not exist");
                    }

                    var invoiceLines = await _context.InvoiceLines
                        .Where(x => x.InvoiceId == invoiceId).ToListAsync();

                    // Removing all invoice lines
                    foreach (var invoiceLine in invoiceLines) {
                        _context.InvoiceLines.Remove(invoiceLine);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"DELETE: InvoiceLine (ID: {invoiceLine.InvoiceLineId})");
                    }

                    // Removing invoice
                    _context.Invoices.Remove(invoice);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"DELETE: Invoice (ID: {invoiceId})");
                }
            }

            return NoContent();
        } catch (Exception ex) {
            _logger.LogError($"DeleteCustomerInvoices(customerId, id) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
