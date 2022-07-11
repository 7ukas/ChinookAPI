namespace ChinookAPI;

[Route("api/[controller]")]
[ApiController]
public class AlbumsController : Controller {
    private readonly ChinookContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AlbumsController> _logger;

    public AlbumsController(ChinookContext context, IMapper mapper, ILogger<AlbumsController> logger) {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: api/Albums/page=5&artistId=5&genreId=5
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlbumsReadDto>>> GetAlbums(int page, int artistId, int genreId) {
        try {
            var albums = await _context.Albums
            .ProjectTo<AlbumsReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

            // Get albums by Artist
            if (artistId > 0) {
                var artist = _context.Artists.Find(artistId);

                if (artist != null) {
                    var artistAlbums = albums.Where(x => x.Artist == artist.Name).ToList();
                    albums = new List<AlbumsReadDto>(artistAlbums);
                }
            }

            // Get albums by Genre
            if (genreId > 0) {
                var genre = _context.Genres.Find(genreId);

                if (genre != null) {
                    var genreAlbums = albums.Where(x => x.Genre == genre.Name).ToList();
                    albums = new List<AlbumsReadDto>(genreAlbums);
                }
            }

            // Set page
            if (page < 1 || page > ((albums.Count - 1) / Page.Rows) + 1) {
                page = 1;
            }

            int index = (page - 1) * Page.Rows;
            int count = (page * Page.Rows) > albums.Count ?
                (albums.Count % Page.Rows) : Page.Rows;

            albums = albums.OrderBy(x => x.AlbumId).ToList().GetRange(index, count);

            return Ok(albums);
        } catch (Exception ex) {
            _logger.LogError($"GetAlbums(page, artistId, genreId) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    // GET: api/Albums/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AlbumReadDto>> GetAlbum(int id) {
        try {
            var album = await _context.Albums
            .ProjectTo<AlbumReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.AlbumId == id);

            return Ok(album);
        } catch (Exception ex) {
            _logger.LogError($"GetAlbum(id) threw an exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
