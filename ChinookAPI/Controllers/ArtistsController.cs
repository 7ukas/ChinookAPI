namespace ChinookAPI;

[Route("api/[controller]")]
[ApiController]
public class ArtistsController : Controller {
    private readonly ChinookContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ArtistsController> _logger;

    public ArtistsController(ChinookContext context, IMapper mapper, ILogger<ArtistsController> logger) {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: api/Artists/page=5
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArtistsReadDto>>> GetArtists(int page) {
        try {
            var artists = await _context.Artists
            .ProjectTo<ArtistsReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

            // Set page
            if (page < 1 || page > ((artists.Count - 1) / Page.Rows) + 1) {
                page = 1;
            }

            int index = (page - 1) * Page.Rows;
            int count = (page * Page.Rows) > artists.Count ?
                (artists.Count % Page.Rows) : Page.Rows;

            artists = artists.OrderBy(x => x.ArtistId).ToList().GetRange(index, count);

            return Ok(artists);
        } catch (Exception ex) {
            _logger.LogError($"GetArtists(page) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
        
    }

    // GET: api/Artists/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ArtistReadDto>> GetArtist(int id) {
        try {
            var artist = await _context.Artists
            .ProjectTo<ArtistReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.ArtistId == id);

            return Ok(artist);
        } catch (Exception ex) {
            _logger.LogError($"GetArtist(id) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
