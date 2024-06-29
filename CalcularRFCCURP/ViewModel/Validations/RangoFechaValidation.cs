using System.ComponentModel.DataAnnotations;

namespace CalcularCURPyRFC.ViewModel.Validations;

public class RangoFechaValidation : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        DateTime date;

        if (DateTime.TryParse((string?)value, out date))
            return true;
        else
            return false;

    }
}
