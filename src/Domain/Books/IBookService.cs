using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDNetCore.Domain.Books
{
    public interface IBookService
    {
        Task<BookDto> AddAsync(CreatingBookDto dto);
        Task<BookDto> UpdateAsync(EditingBookDto dto);
        Task<BookDto> GetByIdAsync(BookId id);
        Task<List<BookDto>> GetAllAsync();
    }
}
