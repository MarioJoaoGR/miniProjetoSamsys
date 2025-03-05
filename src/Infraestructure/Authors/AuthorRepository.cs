using DDDNetCore.Domain.Authors;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.Shared;

namespace DDDNetCore.Infraestructure.Authors
{
    public class AuthorRepository : BaseRepository<Author, AuthorId>, IAuthorRepository
    {
        private readonly DDDSample1DbContext context;


        public AuthorRepository(DDDSample1DbContext context) : base(context.Authors)
        {
            this.context = context;
        }



    }
}
