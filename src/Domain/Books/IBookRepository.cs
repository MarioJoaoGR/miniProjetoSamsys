using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace DDDNetCore.Domain.Books
{
    public interface IBookRepository : IRepository<Book, BookId>
    {
        Task<Book> GetByIsbnAsync(string isbn);
        Task<List<Book>> GetByFiltersAsync(string isbn, string title);
    }
}
