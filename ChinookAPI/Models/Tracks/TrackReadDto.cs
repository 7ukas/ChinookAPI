namespace ChinookAPI;

public class TrackReadDto {
    public int TrackId { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public string Composer { get; set; }
    public string Genre { get; set; }
    public string Price { get; set; }
    public string Duration { get; set; }
    public string FileSize { get; set; }
    public string FileFormat { get; set; }
}
