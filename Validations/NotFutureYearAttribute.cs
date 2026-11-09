using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Validations
{
    /*
     Validación propia: comprueba que un año (int) no sea posterior al año actual.
     Se usa poniendo [NotFutureYear(ErrorMessage = "código|mensaje")] encima de la propiedad.
     La usan dos DTOs: BookDto.PublicationYear (error 23) y AuthorDto.BirthYear (error 25).
     */
    public class NotFutureYearAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is int year)
            {
                return year <= DateTime.Now.Year;   // true = válido, false = error
            }

            return true;   // si no es un int, no es cosa de esta validación
        }
    }
}
