using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDNetCore.Domain.Authors
{
    public interface IAuthorService
    {
        Task<List<AuthorDto>> GetAllAsync();
        Task<AuthorDto> GetByIdAsync(AuthorId id);
    }
}
