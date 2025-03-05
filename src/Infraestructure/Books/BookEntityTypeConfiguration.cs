using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDNetCore.Domain.Books;

namespace DDDNetCore.Infrastructure.Books
{
    internal class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.OwnsOne(b => b.Isbn, isbn =>
            {
                isbn.Property(i => i.isbn).IsRequired();
                isbn.HasIndex(i => i.isbn).IsUnique();
            });
            builder.OwnsOne(b => b.Title, title =>
            {
                title.Property(t => t.title).IsRequired();
            });
            builder.OwnsOne(b => b.Value, value =>
            {
                value.Property(p => p.value).IsRequired();
            });
        }
    }
}
