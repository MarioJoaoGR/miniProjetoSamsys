using Microsoft.EntityFrameworkCore;


using DDDNetCore.Domain.Books;
using DDDNetCore.Domain.Authors;
using DDDNetCore.Infrastructure.Authors;
using DDDNetCore.Infrastructure.Books;


namespace DDDSample1.Infrastructure
{
    public class DDDSample1DbContext : DbContext
    {
        /*public DbSet<Category> Categories { get; set; }*/


        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        public DDDSample1DbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.ApplyConfiguration(new BookEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorEntityTypeConfiguration());




            base.OnModelCreating(modelBuilder);



        }
    }
}