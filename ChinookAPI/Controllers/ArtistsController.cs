namespace ChinookAPI;

[Route("api/[controller]")]
[ApiController]
public class ArtistsController : Controller {
    private readonly ChinookContext _context;
    private readonly IMapper _mapper;

    public ArtistsController(ChinookContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    // POST: api/Artists
    [HttpPost]
    public async Task<ActionResult<ArtistCreateDto>> PostArtist(ArtistCreateDto artistDto) {
        var artist = _mapper.Map<Artist>(artistDto);
        await _context.Artists.AddAsync(artist);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetArtist), new { id = artist.ArtistId }, artist);
    }

    // GET: api/Artists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArtistReadDto>>> GetArtists() {
        var artists = await _context.Artists.ToListAsync();

        var artistsDto = _mapper.Map<IEnumerable<ArtistReadDto>>(artists);
        return Ok(artistsDto);
    }

    // GET: api/Artists/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ArtistReadDto>> GetArtist(int id) {
        var artist = await _context.Artists.FindAsync(id);
        if (artist == null) return NotFound();

        var artistDto = _mapper.Map<ArtistReadDto>(artist);
        return Ok(artistDto);
    }

    // PUT: api/Artists/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutArtist(int id, ArtistUpdateDto artistDto) {
        if (id != artistDto.ArtistId) return BadRequest();

        var artist = await _context.Artists.FindAsync(id);
        if (artist == null) return NotFound();

        _mapper.Map(artistDto, artist);
        _context.Entry(artist).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if (!_ArtistExists(id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

    // DELETE: api/Artists/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtist(int? id)
    {
        var artist = await _context.Artists.FindAsync(id);
        if (artist == null) return NotFound();

        _context.Artists.Remove(artist);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool _ArtistExists(int id) {
        return (_context.Artists?.Any(e => e.ArtistId == id)).GetValueOrDefault();
    }
}
