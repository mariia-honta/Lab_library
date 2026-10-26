using Application.Common.Interfaces.Repositories;
using MediatR;
using Application.Books.Exceptions;
using Domain.Books;
using LanguageExt;

namespace Application.Books.Commands;

public record DeleteBookCommand : IRequest<Either<BookException, Book>>
{
    public required Guid BookId { get; init; }
}

public class DeleteBookCommandHandler(IBookRepository bookRepository)
    : IRequestHandler<DeleteBookCommand, Either<BookException, Book>>
{
    public async Task<Either<BookException, Book>> Handle(
        DeleteBookCommand request,
        CancellationToken cancellationToken)
    {
        var bookId = new BookId(request.BookId);
        var book = await bookRepository.GetById(bookId, cancellationToken);

        return await book.MatchAsync(
            b => DeleteEntity(b, cancellationToken),
            () => new BookNotFoundException(bookId));
    }

    private async Task<Either<BookException, Book>> DeleteEntity(
        Book book,
        CancellationToken cancellationToken)
    {
        try
        {
            await bookRepository.Delete(book, cancellationToken);
            return book;
        }
        catch (Exception exception)
        {
            return new BookUnhandledException(book.Id, exception);
        }
    }
}