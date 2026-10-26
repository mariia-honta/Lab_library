using FluentValidation;

namespace Application.Books.Commands;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(255);
        RuleFor(x => x.Author).NotEmpty().MinimumLength(3).MaximumLength(255);
        RuleFor(x => x.PublishedYear).InclusiveBetween(0, 3000);
        RuleFor(x => x.Genre).IsInEnum();
        RuleFor(x => x.Isbn).NotEmpty().MinimumLength(10).MaximumLength(20);
    }
}