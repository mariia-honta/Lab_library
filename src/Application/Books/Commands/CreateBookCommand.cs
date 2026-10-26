using Application.Common.Interfaces.Repositories;
using Domain.Books;
using MediatR;
using LanguageExt;
using Application.Books.Exceptions;

namespace Application.Books.Commands;

public record CreateBookCommand : IRequest<Either<BookException, Book>>
{
    public required string Title { get; init; }
    public required string Author { get; init; }
    public required int PublishedYear { get; init; }
    public required Genre Genre { get; init; }
    public required string Isbn { get; init; }
    public required bool IsAvailable { get; init; }
}

public class CreateBookCommandHandler(IBookRepository bookRepository)
    : IRequestHandler<CreateBookCommand, Either<BookException, Book>>
{
    public async Task<Either<BookException, Book>> Handle(
        CreateBookCommand request,
        CancellationToken cancellationToken)
    {
        var existingBook = await bookRepository.GetByIsbn(request.Isbn, cancellationToken);

        return await existingBook.MatchAsync(
            b => new BookAlreadyExistsException(b.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<BookException, Book>> CreateEntity(
        CreateBookCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var book = Book.New(
                BookId.New(),
                request.Title,
                request.Author,
                request.PublishedYear,
                request.Genre,
                request.Isbn,
                request.IsAvailable);

            return await bookRepository.Add(book, cancellationToken);
        }
        catch (Exception exception)
        {
            return new BookUnhandledException(BookId.Empty(), exception);
        }
    }
}