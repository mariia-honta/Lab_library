using Domain.Books;

namespace Application.Books.Services.Abstract;

public interface IBookService
{
    Task<IReadOnlyList<Book>> GetBooks(string? author, Genre? genre, bool? isAvailable, CancellationToken cancellationToken);
    Task<Book?> GetBook(Guid id, CancellationToken cancellationToken);
    Task<Book> Add(string title, string author, int publishedYear, Genre genre, string isbn,bool isAvailable,CancellationToken cancellationToken);
    Task<Book?> Update(Guid id, string title, string author, int publishedYear, Genre genre, string isbn,bool isAvailable, CancellationToken cancellationToken);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken);
}