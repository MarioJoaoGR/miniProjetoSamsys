using System.Threading.Tasks;
using DDDNetCore.Domain.Authors;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDNetCore.Infraestructure.Authors
{
    public class AuthorRepository : BaseRepository<Author, AuthorId>, IAuthorRepository
    {
        private readonly DDDSample1DbContext context;


        public AuthorRepository(DDDSample1DbContext context) : base(context.Authors)
        {
            this.context = context;
        }

        public async Task<Author> GetByNIFAsync(string nif)
        {
            return await this.context.Authors.FirstOrDefaultAsync(n => n.NIF.nif == nif);
        }



    }
}
