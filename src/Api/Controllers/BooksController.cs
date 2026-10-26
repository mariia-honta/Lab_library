using Api.Dtos;
using Api.Modules.Errors;
using Application.Books.Commands;
using Application.Common.Interfaces.Queries;
using Domain.Books;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("books")]
[ApiController]
public class BooksController(ISender sender, IBookQueries bookQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookDto>>> GetBooks(
        [FromQuery] string? author,
        [FromQuery] Genre? genre,
        [FromQuery] bool? isAvailable,
        CancellationToken cancellationToken)
    {
        var books = await bookQueries.GetAll(author, genre, isAvailable, cancellationToken);

        return books.Select(BookDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookDto>> GetBook(Guid id, CancellationToken cancellationToken)
    {
        var book = await bookQueries.GetById(new BookId(id), cancellationToken);

        return book.Match<ActionResult<BookDto>>(
            b => BookDto.FromDomainModel(b),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook(
        [FromBody] CreateBookDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateBookCommand
        {
            Title = request.Title,
            Author = request.Author,
            PublishedYear = request.PublishedYear,
            Genre = request.Genre,
            Isbn = request.Isbn,
            IsAvailable = request.IsAvailable
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<BookDto>>(
            b => CreatedAtAction(nameof(GetBook), new { id = b.Id.Value }, BookDto.FromDomainModel(b)),
            e => e.ToObjectResult());
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookDto>> UpdateBook(
        Guid id,
        [FromBody] UpdateBookDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateBookCommand
        {
            BookId = id,
            Title = request.Title,
            Author = request.Author,
            PublishedYear = request.PublishedYear,
            Genre = request.Genre,
            Isbn = request.Isbn,
            IsAvailable = request.IsAvailable
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<BookDto>>(
            b => BookDto.FromDomainModel(b),
            e => e.ToObjectResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteBookCommand { BookId = id }, cancellationToken);

        return result.Match<IActionResult>(
            _ => NoContent(),
            e => e.ToObjectResult());
    }
}