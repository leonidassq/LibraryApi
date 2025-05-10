using LibraryApi.Data;
using LibraryApi.Dtos;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthorsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync();
            return Ok(authors.Select(a => new AuthorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var a = await _context.Authors.FindAsync(id);
            if (a == null) return NotFound();
            return Ok(new AuthorDto { Id = a.Id, FirstName = a.FirstName, LastName = a.LastName });
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorDto dto)
        {
            var author = new Author { FirstName = dto.FirstName, LastName = dto.LastName };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            dto.Id = author.Id;
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorDto>> UpdateAuthor(int id, AuthorDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();
            author.FirstName = dto.FirstName;
            author.LastName = dto.LastName;
            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

