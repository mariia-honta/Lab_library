namespace Domain.Books;

public class Book
{
    public BookId Id { get; } 
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int PublishedYear { get; private set; }
    public Genre Genre { get; private set; }
    public string Isbn { get; private set; }
    public bool IsAvailable { get; private set; }
    public DateTime CreatedAt { get; }

    private Book(BookId id, string title, string author, int publishedYear, Genre genre, string isbn,bool isAvailable, DateTime createdAt) 
        => (Id, Title, Author, PublishedYear, Genre, Isbn, IsAvailable, CreatedAt) = (id, title, author, publishedYear, genre, isbn,isAvailable, createdAt);
    
    public static Book New(BookId id, string title, string author, int publishedYear, Genre genre, string isbn,bool isAvailable=true)
        => new(id, title, author, publishedYear, genre, isbn,isAvailable ,DateTime.UtcNow);
    
    public void UpdateDetails(string title, string author, int publishedYear, Genre genre, string isbn,bool isAvailable)
        => (Title, Author, PublishedYear, Genre, Isbn,IsAvailable) = (title, author, publishedYear, genre, isbn,isAvailable);

}