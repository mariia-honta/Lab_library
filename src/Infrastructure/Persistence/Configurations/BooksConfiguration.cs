using Domain.Books;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BooksConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.Author)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.PublishedYear)
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(x => x.Genre)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(x => x.Isbn)
            .HasColumnType("varchar(20)")
            .IsRequired();

        builder.HasIndex(x => x.Isbn)
            .IsUnique();

        builder.Property(x => x.IsAvailable)
            .HasColumnType("boolean")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();
    }
}