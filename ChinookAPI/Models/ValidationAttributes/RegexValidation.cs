using System.Text.RegularExpressions;

namespace ChinookAPI;

public sealed class RegexValidation : ValidationAttribute {
    public ValidationFlag Match { get; set; }
    
    protected override ValidationResult IsValid(object obj, ValidationContext validationContext) {
        string value = (string)obj;
        if (value == null) return ValidationResult.Success;
        else if (value.Length == 0) return ValidationResult.Success;

        bool match = false;
        switch (Match) {
            case ValidationFlag.Name:
                match = new Regex(@"[a-zA-Z]").IsMatch(value);
                if (!match) return new ValidationResult("Name must contain only letters!");
                break;
            case ValidationFlag.Address:
                match = new Regex(@"[A-Za-z0-9'\.\-\s\,]").IsMatch(value);
                if (!match) return new ValidationResult("Street address is not valid!");
                break;
            case ValidationFlag.City:
                match = new Regex(@"^([a-zA-Z\u0080-\u024F]+(?:. |-| |'))*[a-zA-Z\u0080-\u024F]*$").IsMatch(value);
                if (!match) return new ValidationResult("City name is not valid!");
                break;
            case ValidationFlag.State:
                match = new Regex(@"^([a-zA-Z\u0080-\u024F]+(?:. |-| |'))*[a-zA-Z\u0080-\u024F]*$").IsMatch(value);
                if (!match) return new ValidationResult("State name is not valid!");
                break;
            case ValidationFlag.Country:
                match = new Regex(@"[a-zA-Z\xC0-\uFFFF]{2,}").IsMatch(value);
                if (!match) return new ValidationResult("Country name is not valid!");
                break;
            case ValidationFlag.PostalCode:
                match = new Regex(@"(?i)^[a-z0-9][a-z0-9\- ]{0,10}[a-z0-9]$").IsMatch(value);
                if (!match) return new ValidationResult("Postal code is not valid!");
                break;
            case ValidationFlag.Phone:
                match = new Regex(@"[0-9\s\+()-]+").IsMatch(value);
                if (!match) return new ValidationResult("Phone number is not valid!");
                break;
            case ValidationFlag.Fax:
                match = new Regex(@"[0-9\s\+()-]+").IsMatch(value);
                if (!match) return new ValidationResult("Fax number is not valid!");
                break;
            case ValidationFlag.Email:
                match = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(value);
                if (!match) return new ValidationResult("E-mail address is not valid!");
                break;
        }

        return ValidationResult.Success;
    }
}
