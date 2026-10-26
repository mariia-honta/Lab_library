using Domain.Books;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IBookQueries
{
    Task<IReadOnlyList<Book>> GetAll(string? author, Genre? genre, bool? isAvailable, CancellationToken cancellationToken);
    Task<Option<Book>> GetById(BookId id, CancellationToken cancellationToken);
}