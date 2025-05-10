using System.Text.Json.Serialization;

namespace LibraryApi.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = null!;

        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = null!;
    }
}
