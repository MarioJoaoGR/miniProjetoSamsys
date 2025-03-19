using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDNetCore.Domain.Books
{
    public interface IBookService
    {
        Task<MessagingHelper<BookDto>> AddAsync(CreatingBookDto dto);
        Task<MessagingHelper<BookDto>> DeleteAsync(BookId id);
        Task<MessagingHelper<BookDto>> UpdateAsync(EditingBookDto dto);
        Task<MessagingHelper<List<BookDto>>> SearchAsync(BookFilterDto dto);
        Task<MessagingHelper<BookDto>> GetByIdAsync(BookId id);
        Task<MessagingHelper<List<BookDto>>> GetAllAsync();
        Task<MessagingHelper<List<BookDto>>> GetAllActiveAsync();
        Task<MessagingHelper<List<BookDto>>> GetAllInactiveAsync();
        Task<MessagingHelper<List<BookDto>>> GetBooksByAuthorAsync(string authorId);
    }
}
