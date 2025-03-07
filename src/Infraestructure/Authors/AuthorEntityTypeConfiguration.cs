using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDNetCore.Domain.Authors;

namespace DDDNetCore.Infrastructure.Authors
{
    internal class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(a => a.Id);
            builder.OwnsOne(a => a.FullName, fullName =>
            {
                fullName.Property(f => f.fullName).IsRequired();
            });
            builder.OwnsOne(a => a.NIF, nif =>
            {
                nif.Property(n => n.nif).IsRequired();
                nif.HasIndex(n => n.nif).IsUnique();
            });
        }
    }
}

