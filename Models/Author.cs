using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryApi.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}

