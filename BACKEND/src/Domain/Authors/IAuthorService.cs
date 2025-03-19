using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDNetCore.Domain.Authors
{
    public interface IAuthorService
    {
        Task<MessagingHelper<List<AuthorDto>>> GetAllAsync();
        Task<MessagingHelper<AuthorDto>> GetByIdAsync(AuthorId id);
    }
}
