namespace ChinookAPI; 

public class CustomerUpdateDto {
    [StringLength(40)]
    [RegexValidation(Match = ValidationFlag.Name)]
    public string FirstName { get; set; }

    [StringLength(20)]
    [RegexValidation(Match = ValidationFlag.Name)]
    public string LastName { get; set; }

    [StringLength(80)]
    public string? Company { get; set; }

    [StringLength(70)]
    [RegexValidation(Match = ValidationFlag.Address)]
    public string? Address { get; set; }

    [StringLength(40)]
    [RegexValidation(Match = ValidationFlag.City)]
    public string? City { get; set; }

    [StringLength(40)]
    [RegexValidation(Match = ValidationFlag.State)]
    public string? State { get; set; }

    [StringLength(40)]
    [RegexValidation(Match = ValidationFlag.Country)]
    public string? Country { get; set; }

    [StringLength(10)]
    [RegexValidation(Match = ValidationFlag.PostalCode)]
    public string? PostalCode { get; set; }

    [StringLength(24)]
    [RegexValidation(Match = ValidationFlag.Phone)]
    public string? Phone { get; set; }

    [StringLength(24)]
    [RegexValidation(Match = ValidationFlag.Fax)]
    public string? Fax { get; set; }

    [RegexValidation(Match = ValidationFlag.Email)]
    public string Email { get; set; }
}
