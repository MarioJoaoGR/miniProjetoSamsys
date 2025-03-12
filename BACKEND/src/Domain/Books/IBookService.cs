using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDNetCore.Domain.Books
{
    public interface IBookService
    {
        Task<BookDto> AddAsync(CreatingBookDto dto);
        Task<BookDto> DeleteAsync(BookId id);
        Task<BookDto> UpdateAsync(EditingBookDto dto);
        Task<List<BookDto>> SearchAsync(BookFilterDto dto);
        Task<BookDto> GetByIdAsync(BookId id);
        Task<List<BookDto>> GetAllAsync();
        Task<List<BookDto>> GetAllActiveAsync();
        Task<List<BookDto>> GetAllInactiveAsync();

    }
}
