using DDDNetCore.Domain.Books;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace DDDNetCore.Domain.Authors
{
    public class AuthorService : IAuthorService 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IUnitOfWork unitOfWork, IAuthorRepository authorRepository)
        {
            _unitOfWork = unitOfWork;
            _authorRepository = authorRepository;
        }

        public async Task<List<AuthorDto>> GetAllAsync()
        {
            var list = await _authorRepository.GetAllAsync();

            List<AuthorDto> listDto = list.ConvertAll<AuthorDto>(aut => AuthorMapper.toDto(aut));

            return listDto;

        }
    }
}

