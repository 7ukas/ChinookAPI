namespace ChinookAPI;

[Route("api/[controller]")]
[ApiController]
public class PlaylistsController : Controller {
    private readonly ChinookContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<PlaylistsController> _logger;

    public PlaylistsController(ChinookContext context, IMapper mapper, ILogger<PlaylistsController> logger) {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: api/Playlists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlaylistsReadDto>>> GetPlaylists() {
        try {
            var playlist = await _context.Playlists
                .ProjectTo<PlaylistsReadDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(playlist);
        } catch (Exception ex) {
            _logger.LogError($"GetPlaylists() threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // GET: api/Playlists/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PlaylistReadDto>> GetPlaylist(int id) {
        try {
            var playlist = await _context.Playlists
                .ProjectTo<PlaylistReadDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.PlaylistId == id);

            return Ok(playlist);
        } catch (Exception ex) {
            _logger.LogError($"GetPlaylist(id) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}