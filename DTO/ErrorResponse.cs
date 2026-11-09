using System.Text.Json.Serialization;

namespace BibliotecaAPI.DTO
{
    public class ErrorResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }


        public ErrorResponse(int code, string description)
        {
            this.Code = code;
            this.Description = description;
        }




    }
}
