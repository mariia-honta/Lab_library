using System.ComponentModel.DataAnnotations;
using Domain.Books;
namespace Api.Dtos;


public record BookDto(Guid Id, string Title, string Author, int PublishedYear,
    Genre Genre,string Isbn, bool IsAvailable,DateTime CreatedAt)
{
    public static BookDto FromDomainModel(Book book) 
        => new(book.Id.Value, book.Title, book.Author, book.PublishedYear,  book.Genre, book.Isbn, book.IsAvailable,book.CreatedAt);
}

public record CreateBookDto(string Title, string Author, int PublishedYear,
    Genre Genre, string Isbn, bool IsAvailable);

public record UpdateBookDto(string Title, string Author, int PublishedYear,
    Genre Genre, string Isbn, bool IsAvailable);