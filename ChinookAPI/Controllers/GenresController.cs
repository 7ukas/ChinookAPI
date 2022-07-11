namespace ChinookAPI;

[Route("api/[controller]")]
[ApiController]
public class GenresController : Controller {
    private readonly ChinookContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GenresController> _logger;

    public GenresController(ChinookContext context, IMapper mapper, ILogger<GenresController> logger) {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: api/Genres
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreReadDto>>> GetGenres() {
        try {
            var genre = await _context.Genres
                .ProjectTo<GenreReadDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(genre);
        } catch (Exception ex) {
            _logger.LogError($"GetGenres() threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // GET: api/Genres/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GenreReadDto>> GetGenre(int id) {
        try {
            var genre = await _context.Genres
                .ProjectTo<GenreReadDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.GenreId == id);

            return Ok(genre);
        } catch (Exception ex) {
            _logger.LogError($"GetGenre(id) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
