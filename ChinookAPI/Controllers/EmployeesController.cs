namespace ChinookAPI;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : Controller {
    private readonly ChinookContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(ChinookContext context, IMapper mapper, ILogger<EmployeesController> logger) {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: api/Employees
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeesReadDto>>> GetEmployees() {
        try {
            var employees = await _context.Employees
                .ProjectTo<EmployeesReadDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(employees);
        } catch (Exception ex) {
            _logger.LogError($"GetEmployees() threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // GET: api/Employees/5
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeesReadDto>> GetEmployee(int id) {
        try {
            var employee = await _context.Employees
                .ProjectTo<EmployeesReadDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.EmployeeId == id);

            return Ok(employee);
        } catch (Exception ex) {
            _logger.LogError($"GetEmployee(id) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
