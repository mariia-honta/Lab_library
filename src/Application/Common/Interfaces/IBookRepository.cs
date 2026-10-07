using Domain.Books;

namespace Application.Common.Interfaces;

public interface IBookRepository
{
    Task<IReadOnlyList<Book>> GetAll(string? author, Genre? genre, bool? isAvailable, CancellationToken cancellationToken);    
    Task<Book?> GetById(Guid id, CancellationToken cancellationToken);
    Task<Book?> GetByIsbn(string isbn, CancellationToken cancellationToken);
    Task<Book> Add(Book book, CancellationToken cancellationToken);
    Task<Book> Update(Book book, CancellationToken cancellationToken);
    Task Delete(Book book, CancellationToken cancellationToken);
}