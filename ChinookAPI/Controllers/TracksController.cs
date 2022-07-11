namespace ChinookAPI;

[Route("api/[controller]")]
[ApiController]
public class TracksController : Controller {
    private readonly ChinookContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TracksController> _logger;

    public TracksController(ChinookContext context, IMapper mapper, ILogger<TracksController> logger) {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: api/Tracks/page=5&genreId=5&artistId=5
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TracksReadDto>>> GetTracks(int page, int artistId, int genreId) {
        try {
            var tracks = await _context.Tracks
                .ProjectTo<TracksReadDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            // Get Tracks by Artist
            if (artistId > 0) {
                var artist = _context.Artists.Find(artistId);

                if (artist != null) {
                    var artistTracks = tracks.Where(x => x.Artist == artist.Name).ToList();
                    tracks = new List<TracksReadDto>(artistTracks);
                }
            }

            // Get Tracks by Genre
            if (genreId > 0) {
                var genre = _context.Genres.Find(genreId);

                if (genre != null) {
                    var genreTracks = tracks.Where(x => x.Genre == genre.Name).ToList();
                    tracks = new List<TracksReadDto>(genreTracks);
                }
            }

            // Set page
            if (page < 1 || page > ((tracks.Count - 1) / Page.Rows) + 1) {
                page = 1;
            }

            int index = (page - 1) * Page.Rows;
            int count = (page * Page.Rows) > tracks.Count ?
                (tracks.Count % Page.Rows) : Page.Rows;

            tracks = tracks.OrderBy(x => x.TrackId).ToList().GetRange(index, count);

            return Ok(tracks);
        } catch (Exception ex) {
            _logger.LogError($"GetTracks(page, artistId, genreId) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // GET: api/Tracks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TrackReadDto>> GetTrack(int id) {
        try {
            var track = await _context.Tracks
                .ProjectTo<TrackReadDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.TrackId == id);

            return Ok(track);
        } catch (Exception ex) {
            _logger.LogError($"GetTrack(id) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
