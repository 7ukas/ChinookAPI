namespace ChinookAPI; 

public class AlbumReadDto {
    public int AlbumId { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Genre { get; set; }
    public string Price { get; set; }
    public string Duration { get; set; }
    public string FileSize { get; set; }
    public string FileFormat { get; set; }
    public IEnumerable<string> Tracklist { get; set; }
}
