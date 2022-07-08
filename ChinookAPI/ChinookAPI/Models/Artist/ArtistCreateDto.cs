namespace ChinookAPI; 

public class ArtistCreateDto {
    [Required]
    [StringLength(120)]
    public string Name { get; set; }
}
