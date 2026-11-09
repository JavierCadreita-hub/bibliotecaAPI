using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BibliotecaAPI.Validations;

namespace BibliotecaAPI.DTO
{
    public class BookDto
    {

        /*
     Un DTO es el JSON de la API escrito como clase de C#.
    Lleva lo que entra y sale por la API, con los nombres del JSON, y las validaciones de formato. No lleva relaciones
     */
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        [Required(ErrorMessage = "20|El título del libro es obligatorio")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("isbn")]
        [Required(ErrorMessage = "21|El ISBN debe tener exactamente 13 dígitos")]
        [RegularExpression(@"^[0-9]{13}$", ErrorMessage = "21|El ISBN debe tener exactamente 13 dígitos")]
        public string Isbn { get; set; } = string.Empty;

        [JsonPropertyName("pages")]
        [Range(1, 2000, ErrorMessage = "22|El número de páginas debe estar entre 1 y 2000")]
        public int Pages { get; set; }

        [JsonPropertyName("publication_year")]
        [NotFutureYear(ErrorMessage = "23|El año de publicación no puede ser futuro")]
        public int PublicationYear { get; set; }

        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        // Los autores llegan como lista de ids: "authors_id": [4, 5]
        [JsonPropertyName("authors_id")]
        [MinLength(1, ErrorMessage = "24|El libro debe tener al menos un autor")]
        public List<int> AuthorsId { get; set; } = new List<int>();
    }
}
