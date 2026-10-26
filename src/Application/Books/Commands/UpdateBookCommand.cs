using Application.Common.Interfaces.Repositories;
using Domain.Books;
using MediatR;
using LanguageExt;
using Application.Books.Exceptions;
using Unit = LanguageExt.Unit;

namespace Application.Books.Commands;

public record UpdateBookCommand : IRequest<Either<BookException, Book>>
{
    public required Guid BookId { get; init; }
    public required string Title { get; init; }
    public required string Author { get; init; }
    public required int PublishedYear { get; init; }
    public required Genre Genre { get; init; }
    public required string Isbn { get; init; }
    public required bool IsAvailable { get; init; }
}

public class UpdateBookCommandHandler(IBookRepository bookRepository)
    : IRequestHandler<UpdateBookCommand, Either<BookException, Book>>
{
    public async Task<Either<BookException, Book>> Handle(
        UpdateBookCommand request,
        CancellationToken cancellationToken)
    {
        var bookId = new BookId(request.BookId);
        var book = await bookRepository.GetById(bookId, cancellationToken);

        return await book.MatchAsync(
            b => CheckDuplicates(b.Id, request.Isbn, cancellationToken)
                .BindAsync(_ => UpdateEntity(b, request, cancellationToken)),
            () => new BookNotFoundException(bookId));
    }

    private async Task<Either<BookException, Unit>> CheckDuplicates(
        BookId currentBookId,
        string isbn,
        CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIsbn(isbn, cancellationToken);

        return book.Match<Either<BookException, Unit>>(
            b => b.Id == currentBookId ? Unit.Default : new BookAlreadyExistsException(b.Id),
            () => Unit.Default);
    }

    private async Task<Either<BookException, Book>> UpdateEntity(
        Book book,
        UpdateBookCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            book.UpdateDetails(
                request.Title,
                request.Author,
                request.PublishedYear,
                request.Genre,
                request.Isbn,
                request.IsAvailable);

            return await bookRepository.Update(book, cancellationToken);
        }
        catch (Exception exception)
        {
            return new BookUnhandledException(book.Id, exception);
        }
    }
}