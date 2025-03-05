
using DDDSample1.Infrastructure.Shared;
using DDDSample1.Infrastructure;
using DDDNetCore.Domain.Books;

namespace DDDNetCore.Infraestructure.Books
{
    public class BookRepository : BaseRepository<Book, BookId>, IBookRepository
    {
        private readonly DDDSample1DbContext context;



        public BookRepository(DDDSample1DbContext context) : base(context.Books)
        {
            this.context = context;
        }



    }
}
