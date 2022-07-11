namespace ChinookAPI;

public class InvoiceReadDto {
    public int InvoiceId { get; set; }
    public string Date { get; set; }
    public string TotalPrice { get; set; }
    public IEnumerable<string> Orderlist { get; set; }
}
