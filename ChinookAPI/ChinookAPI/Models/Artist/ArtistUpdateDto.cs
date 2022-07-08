namespace ChinookAPI; 

public class ArtistUpdateDto {
    public int ArtistId { get; set; }

    [Required]
    [StringLength(120)]
    public string Name { get; set; }
}
