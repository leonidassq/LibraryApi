using LibraryApi.Data;
using LibraryApi.Dtos;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BooksController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] int? authorId)
        {
            var query = _context.Books.Include(b => b.Author).AsQueryable();
            if (authorId.HasValue)
                query = query.Where(b => b.AuthorId == authorId.Value);

            var books = await query.ToListAsync();
            return Ok(books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Year = b.Year,
                Author = new AuthorDto
                {
                    Id = b.Author.Id,
                    FirstName = b.Author.FirstName,
                    LastName = b.Author.LastName
                }
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var b = await _context.Books.Include(bk => bk.Author)
                                        .FirstOrDefaultAsync(bk => bk.Id == id);
            if (b == null) return NotFound();
            return Ok(new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Year = b.Year,
                Author = new AuthorDto
                {
                    Id = b.Author.Id,
                    FirstName = b.Author.FirstName,
                    LastName = b.Author.LastName
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] BookDto dto)
        {
            var authorId = dto.Author.Id;
            var author = await _context.Authors.FindAsync(authorId);
            if (author == null) return NotFound($"Author {authorId} not found");

            var book = new Book { Title = dto.Title, Year = dto.Year, AuthorId = authorId };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            dto.Id = book.Id;
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> UpdateBook(int id, BookDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            var newAuthor = await _context.Authors.FindAsync(dto.Author.Id);
            if (newAuthor == null) return NotFound($"Author {dto.Author.Id} not found");

            book.Title = dto.Title;
            book.Year = dto.Year;
            book.AuthorId = dto.Author.Id;
            await _context.SaveChangesAsync();

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var b = await _context.Books.FindAsync(id);
            if (b == null) return NotFound();
            _context.Books.Remove(b);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
