using Domain.Books;
namespace Application.Books.Exceptions;

public abstract class BookException(BookId bookId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public BookId BookId { get; } = bookId;
}

public class BookAlreadyExistsException(BookId bookId)
    : BookException(bookId, $"Book already exists under id {bookId}");

public class BookNotFoundException(BookId bookId)
    : BookException(bookId, $"Book not found under id {bookId}");

public class BookUnhandledException(BookId bookId, Exception? innerException = null)
    : BookException(bookId, $"Unhandled exception under id {bookId}", innerException);