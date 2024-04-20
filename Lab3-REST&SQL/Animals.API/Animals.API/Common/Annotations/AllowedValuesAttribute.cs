using System.ComponentModel.DataAnnotations;

namespace Animals.API.Common.Annotations;

public class AllowedValuesAttribute<T> : ValidationAttribute
{
    private readonly T[] _allowedValues;

    public AllowedValuesAttribute(params T[] allowedValues)
    {
        _allowedValues = allowedValues;
        ErrorMessage = $"The value must be one of the following: {string.Join(", ", allowedValues)}.";
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || !_allowedValues.Contains((T)value))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success!;
    }
}