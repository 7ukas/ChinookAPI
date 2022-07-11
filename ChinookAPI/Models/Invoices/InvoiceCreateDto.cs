namespace ChinookAPI; 

public class InvoiceCreateDto {
    public IEnumerable<InvoiceLineCreateDto> InvoiceLines { get; set; }
}
