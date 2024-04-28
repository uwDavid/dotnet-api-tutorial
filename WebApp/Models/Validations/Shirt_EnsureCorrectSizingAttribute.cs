using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Validations;

public class Shirt_EnsureCorrectSizingAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var shirt = validationContext.ObjectInstance as Shirt;
        // we are validating a property of Shirt class
        //thus we know that the validationContext is a Shirt class

        if (shirt is not null && !string.IsNullOrEmpty(shirt.Gender))
        {
            if (shirt.Gender.Equals("men", StringComparison.OrdinalIgnoreCase) && shirt.Size < 8)
            {
                return new ValidationResult("For men's shirt, size must be greater or equal to 8.");
            }

            else if (shirt.Gender.Equals("women", StringComparison.OrdinalIgnoreCase) && shirt.Size < 6)
            {
                return new ValidationResult("For women's shirt, size must be greater or equal to 6.");
            }
        }

        return ValidationResult.Success;
    }
}