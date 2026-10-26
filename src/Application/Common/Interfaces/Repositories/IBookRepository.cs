using Domain.Books;
using LanguageExt;
namespace Application.Common.Interfaces.Repositories;

public interface IBookRepository
{
    Task<Option<Book>> GetById(BookId id, CancellationToken cancellationToken);
    Task<Option<Book>> GetByIsbn(string isbn, CancellationToken cancellationToken);
    Task<Book> Add(Book book, CancellationToken cancellationToken);
    Task<Book> Update(Book book, CancellationToken cancellationToken);
    Task Delete(Book book, CancellationToken cancellationToken);
}