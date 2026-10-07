using System.ComponentModel.DataAnnotations;
using Domain.Books;
namespace Api.Dtos;


public record BookDto(Guid Id, string Title, string Author, int PublishedYear,
    Genre Genre,string Isbn, bool IsAvailable,DateTime CreatedAt)
{
    public static BookDto FromDomainModel(Book book) 
        => new(book.Id, book.Title, book.Author, book.PublishedYear,  book.Genre, book.Isbn, book.IsAvailable,book.CreatedAt);
}

public record CreateBookDto(
    [Required, MaxLength(255)] string Title,
    [Required, MaxLength(255)] string Author,
    [Range(0, 3000)] int PublishedYear,
    [EnumDataType(typeof(Genre))] Genre Genre, 
    [Required, MaxLength(20)] string Isbn,
    bool IsAvailable);
    
public record UpdateBookDto(
    [Required, MaxLength(255)] string Title,
    [Required, MaxLength(255)] string Author,
    [Range(0, 3000)] int PublishedYear,
    [EnumDataType(typeof(Genre))] Genre Genre, 
    [Required, MaxLength(20)] string Isbn,
    bool IsAvailable);