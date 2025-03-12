
using DDDSample1.Infrastructure.Shared;
using DDDSample1.Infrastructure;
using DDDNetCore.Domain.Books;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using SendGrid.Helpers.Mail;

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


        public async Task<List<Book>> GetByFiltersAsync(string isbn, string title)
        {
            var query = this.context.Books.AsQueryable();

            // Filtro por ISBN - Deve começar com a string fornecida
            if (!string.IsNullOrWhiteSpace(isbn))
            {
                query = query.Where(b => b.Isbn.isbn.StartsWith(isbn));
            }

            // Filtro por título - Deve conter a string fornecida (ignorar maiúsculas/minúsculas)
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(b => b.Title.title.ToLower().Contains(title.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<List<Book>> GetAllActiveAsync()
        {
            return await this.context.Set<Book>()
                .Where(b => b.bookStatus == BookStatus.Active)
                .ToListAsync();
        }

        public async Task<List<Book>> GetAllInactiveAsync()
        {
            return await this.context.Set<Book>()
                .Where(b => b.bookStatus == BookStatus.Inactive)
                .ToListAsync();
        }





    }
}
