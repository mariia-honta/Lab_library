namespace Domain.Books;

public record BookId(Guid Value)
{
    public static BookId Empty() => new(Guid.Empty);
    public static BookId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}