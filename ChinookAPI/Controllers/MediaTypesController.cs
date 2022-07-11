namespace ChinookAPI;

[Route("api/[controller]")]
[ApiController]
public class MediaTypesController : Controller {
    private readonly ChinookContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<MediaTypesController> _logger;

    public MediaTypesController(ChinookContext context, IMapper mapper, ILogger<MediaTypesController> logger) {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: api/MediaTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MediaTypeReadDto>>> GetMediaTypes() {
        try {
            var mediaType = await _context.MediaTypes
                .ProjectTo<MediaTypeReadDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(mediaType);
        } catch (Exception ex) {
            _logger.LogError($"GetMediaTypes() threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // GET: api/MediaTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MediaTypeReadDto>> GetMediaTypes(int id) {
        try {
            var mediaType = await _context.MediaTypes
                .ProjectTo<MediaTypeReadDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.MediaTypeId == id);

            return Ok(mediaType);
        } catch (Exception ex) {
            _logger.LogError($"GetMediaType(id) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
