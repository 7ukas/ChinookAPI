namespace ChinookAPI;

public class PlaylistReadDto {
    public int PlaylistId { get; set; }
    public string Name { get; set; }
    public IEnumerable<string> Tracklist { get; set; }
}
