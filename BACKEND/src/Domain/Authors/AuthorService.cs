using DDDNetCore.Domain.Books;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDNetCore.Infraestructure.Books;

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

        public async Task<MessagingHelper<List<AuthorDto>>> GetAllAsync()
        {
            var list = await _authorRepository.GetAllAsync();

            if (list == null || list.Count == 0)
            {
                return new MessagingHelper<List<AuthorDto>>
                {
                    Success = false,
                    Message = "Nenhum autor encontrado.",
                    ErrorType = ErrorType.NotFound
                };
            }

            List<AuthorDto> listDto = list.ConvertAll(AuthorMapper.toDto);

            return new MessagingHelper<List<AuthorDto>>
            {
                Success = true,
                Message = "Lista de autores obtida com sucesso.",
                Obj = listDto
            };
        }

        public async Task<MessagingHelper<AuthorDto>> GetByIdAsync(AuthorId id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return new MessagingHelper<AuthorDto>
                {
                    Success = false,
                    Message = "Autor não encontrado.",
                    ErrorType = ErrorType.NotFound
                };
            }

            return new MessagingHelper<AuthorDto>
            {
                Success = true,
                Message = "Autor encontrado com sucesso.",
                Obj = AuthorMapper.toDto(author)
            };
        }
    }
}
