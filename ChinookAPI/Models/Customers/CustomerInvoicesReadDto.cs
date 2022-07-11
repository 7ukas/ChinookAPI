namespace ChinookAPI;

public class CustomerInvoicesReadDto {
    public int CustomerId { get; set; }
    public IEnumerable<InvoiceReadDto> Invoices { get; set; }
}
