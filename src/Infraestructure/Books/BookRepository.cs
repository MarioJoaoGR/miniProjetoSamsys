
using DDDSample1.Infrastructure.Shared;
using DDDSample1.Infrastructure;
using DDDNetCore.Domain.Books;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DDDNetCore.Infraestructure.Books
{
    public class BookRepository : BaseRepository<Book, BookId>, IBookRepository
    {
        private readonly DDDSample1DbContext context;



        public BookRepository(DDDSample1DbContext context) : base(context.Books)
        {
            this.context = context;
        }

        public async Task<Book> GetByIsbnAsync(string isbn)
        {
            return await this.context.Books.FirstOrDefaultAsync(i => i.Isbn.isbn == isbn);
        }




    }
}
