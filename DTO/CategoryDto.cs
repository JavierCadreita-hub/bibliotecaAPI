using System.Text.Json.Serialization;

namespace BibliotecaAPI.DTO
{

    /*
     n DTO es el JSON de la API escrito como clase de C#. 
    Lleva lo que entra y sale por la API, con los nombres del JSON, y las validaciones de formato. No lleva relaciones
     */
    public class CategoryDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        // El nombre no se puede repetir.
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

    }
}
