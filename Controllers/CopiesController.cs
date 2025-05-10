using LibraryApi.Data;
using LibraryApi.Dtos;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CopiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CopiesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CopyDto>>> GetCopies([FromQuery] int? bookId)
        {
            var query = _context.Copies.AsQueryable();
            if (bookId.HasValue)
                query = query.Where(c => c.BookId == bookId.Value);

            var copies = await query.ToListAsync();
            return Ok(copies.Select(c => new CopyDto { Id = c.Id, BookId = c.BookId }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CopyDto>> GetCopy(int id)
        {
            var c = await _context.Copies.FindAsync(id);
            if (c == null) return NotFound();
            return Ok(new CopyDto { Id = c.Id, BookId = c.BookId });
        }

        [HttpPost]
        public async Task<ActionResult<CopyDto>> CreateCopy(CopyDto dto)
        {
            var book = await _context.Books.FindAsync(dto.BookId);
            if (book == null) return NotFound($"Book {dto.BookId} not found");

            var copy = new Copy { BookId = dto.BookId };
            _context.Copies.Add(copy);
            await _context.SaveChangesAsync();

            dto.Id = copy.Id;
            return CreatedAtAction(nameof(GetCopy), new { id = copy.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCopy(int id, CopyDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var copy = await _context.Copies.FindAsync(id);
            if (copy == null) return NotFound();

            var book = await _context.Books.FindAsync(dto.BookId);
            if (book == null) return NotFound($"Book {dto.BookId} not found");

            copy.BookId = dto.BookId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCopy(int id)
        {
            var c = await _context.Copies.FindAsync(id);
            if (c == null) return NotFound();
            _context.Copies.Remove(c);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

