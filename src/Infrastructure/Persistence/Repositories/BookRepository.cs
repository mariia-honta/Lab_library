using Application.Common.Interfaces;
using Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BookRepository(ApplicationDbContext context) : IBookRepository
{
    public async Task<IReadOnlyList<Book>> GetAll(string? author, Genre? genre, bool? isAvailable, CancellationToken cancellationToken)
    {
        var query = context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(author))
            query = query.Where(x => x.Author.Contains(author));

        if (genre.HasValue)
            query = query.Where(x => x.Genre.Equals(genre.Value));

        if (isAvailable.HasValue)
            query = query.Where(x => x.IsAvailable == isAvailable.Value);

        return await query.ToListAsync(cancellationToken);
    }
    public async Task<Book?> GetById(Guid id, CancellationToken cancellationToken)
        => await context.Books.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Book?> GetByIsbn(string isbn, CancellationToken cancellationToken)
        => await context.Books.FirstOrDefaultAsync(x => x.Isbn == isbn, cancellationToken);

    public async Task<Book> Add(Book book, CancellationToken cancellationToken)
    {
        await context.Books.AddAsync(book, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return book;
    }

    public async Task<Book> Update(Book book, CancellationToken cancellationToken)
    {
        context.Books.Update(book);
        await context.SaveChangesAsync(cancellationToken);
        return book;
    }

    public async Task Delete(Book book, CancellationToken cancellationToken)
    {
        context.Books.Remove(book);
        await context.SaveChangesAsync(cancellationToken);
    }
}