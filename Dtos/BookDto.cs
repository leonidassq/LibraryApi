namespace LibraryApi.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public AuthorDto Author { get; set; } = null!;
    }
}
