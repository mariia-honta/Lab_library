using Application.Books.Services.Abstract;
using Application.Common.Interfaces;
using Domain.Books;

namespace Application.Books.Services.Implementation;

public class BookService(IBookRepository bookRepository) : IBookService
{

    public async Task<IReadOnlyList<Book>> GetBooks(string? author, Genre? genre, bool? isAvailable, CancellationToken cancellationToken)
    {
        return await bookRepository.GetAll(author, genre, isAvailable, cancellationToken);
    }

    public async Task<Book?> GetBook(Guid id, CancellationToken cancellationToken)
    {
        return await bookRepository.GetById(id, cancellationToken);
    }
    
    public async Task<Book> Add(string title, string author, int publishedYear, Genre genre, string isbn,
        bool isAvailable,CancellationToken cancellationToken)
    {
        var existingBook = await bookRepository.GetByIsbn(isbn, cancellationToken);
        if(existingBook is not null)
            throw new ArgumentException($"Book with Isbn '{isbn}' already exists");
        var book = Book.New(Guid.NewGuid(), title, author, publishedYear, genre, isbn,isAvailable);
        return await bookRepository.Add(book, cancellationToken);
    }

    public async Task<Book?> Update(Guid id, string title, string author, int publishedYear, Genre genre,string isbn,
        bool isAvailable,CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetById(id, cancellationToken);
        if (book is null)
            return null;
        var bookWithSameIsbn = await bookRepository.GetByIsbn(isbn, cancellationToken);
        if(bookWithSameIsbn is not null && bookWithSameIsbn.Id != id)
            throw new ArgumentException($"Book with Isbn '{isbn}' already exists");
        book.UpdateDetails(title,author,publishedYear,genre,isbn,isAvailable);
        return await bookRepository.Update(book, cancellationToken);
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetById(id, cancellationToken);
        if (book is null)
            return false;
        await bookRepository.Delete(book, cancellationToken);
        return true;
    }
}