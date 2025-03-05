using Microsoft.EntityFrameworkCore;

using DDDSample1.Domain.Patients;
using DDDSample1.Infrastructure.Patients;
using DDDSample1.Domain.StaffMembers;
using DDDSample1.Infrastructure.StaffMembers;
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
        public DbSet<Staff> StaffMembers { get; set; }

        public DbSet<Patient> Patients { get; set; }


        public DDDSample1DbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.ApplyConfiguration(new PatientEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StaffEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BookEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorEntityTypeConfiguration());




            base.OnModelCreating(modelBuilder);



        }
    }
}