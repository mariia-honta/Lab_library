using Api.Dtos;
using Application.Books.Services.Abstract;
using Domain.Books;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("books")]
[ApiController]
public class BooksController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookDto>>> GetBooks(
        [FromQuery] string? author,
        [FromQuery] Genre? genre,
        [FromQuery] bool? isAvailable,
        CancellationToken cancellationToken)
    {
        var books = await bookService.GetBooks(author, genre, isAvailable, cancellationToken);
        return books.Select(BookDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookDto>> GetBook(Guid id, CancellationToken cancellationToken)
    {
        var book = await bookService.GetBook(id, cancellationToken);
        if (book is null)
        {
            return NotFound();
        }

        return BookDto.FromDomainModel(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook(
        [FromBody] CreateBookDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var book = await bookService.Add(request.Title, request.Author, request.PublishedYear, request.Genre, 
                request.Isbn, request.IsAvailable,cancellationToken);
            var dto = BookDto.FromDomainModel(book);

            return CreatedAtAction(nameof(GetBook), new { id = dto.Id }, dto);
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookDto>> UpdateBook(
        Guid id,
        [FromBody] UpdateBookDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var book = await bookService.Update(id, request.Title, request.Author, request.PublishedYear, request.Genre,
                request.Isbn, request.IsAvailable,cancellationToken);
            if (book is null)
            {
                return NotFound();
            }

            return BookDto.FromDomainModel(book);
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await bookService.Delete(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
    
}