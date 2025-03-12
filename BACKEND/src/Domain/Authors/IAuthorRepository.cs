using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace DDDNetCore.Domain.Authors
{
    public interface IAuthorRepository : IRepository<Author, AuthorId>
    {
        Task<Author> GetByNIFAsync(string nif);
        Task<List<Author>> FilterByNameAsync(string name);
    }
}
