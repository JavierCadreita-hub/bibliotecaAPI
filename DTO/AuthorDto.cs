using System.Text.Json.Serialization;
using BibliotecaAPI.Validations;

namespace BibliotecaAPI.DTO
{
    public class AuthorDto
    {
        /*
     n DTO es el JSON de la API escrito como clase de C#. 
    Lleva lo que entra y sale por la API, con los nombres del JSON, y las validaciones de formato. No lleva relaciones
     */

        [JsonPropertyName("id")]
        public int Id { get; set; }


        [JsonPropertyName("name")]

        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("nationality")]

        public string Nationality { get; set; } = string.Empty;

        [JsonPropertyName("birth_year")]
        [NotFutureYear(ErrorMessage = "25|El año de nacimiento del autor no puede ser futuro")]
        public int BirthYear { get; set; }



    }
}
