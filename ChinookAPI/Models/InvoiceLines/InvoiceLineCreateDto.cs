namespace ChinookAPI; 

public class InvoiceLineCreateDto {
    [Required]
    public int TrackId { get; set; }

    [Required]
    public int Quantity { get; set; }
}
