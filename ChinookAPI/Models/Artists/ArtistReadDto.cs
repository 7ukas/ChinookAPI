namespace ChinookAPI; 

public class ArtistReadDto {
    public int ArtistId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<string> Albums { get; set; }
}
