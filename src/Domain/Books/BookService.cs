using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Domain.Authors;
using DDDSample1.Domain.Shared;

namespace DDDNetCore.Domain.Books
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IUnitOfWork unitOfWork, IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _unitOfWork = unitOfWork;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }


        public async Task<BookDto> AddAsync(CreatingBookDto dto)
        {
          bool isbnIsUnique = await validateIsbnIsUnique(dto.Isbn);
            if (!isbnIsUnique)
            {
                throw new BusinessRuleValidationException("Isbn already exists");
            }
            var authorName = dto.AuthorNIF;

            await checkAuthorByNIFAsync(dto.AuthorNIF, dto);

            var book = new Book(dto.Isbn, dto.Title, dto.AuthorNIF, dto.Value);

            await _bookRepository.AddAsync(book);
            await _unitOfWork.CommitAsync();

            return BookMapper.toDto(book, authorName);

        }

        public async Task<BookDto> GetByIdAsync(BookId id)
        {

            var book = await _bookRepository.GetByIdAsync(id);
            var authorNIF = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.NIF.nif;

            return book == null ? null : BookMapper.toDto(book, authorNIF);
        }


        public async Task<Author> checkAuthorByNIFAsync(string name, CreatingBookDto book)
        {
            try
            {
                var author = await _authorRepository.GetByNIFAsync(name);
                if (author == null)
                {
                    throw new BusinessRuleValidationException("Author not found");
                }
                book.AuthorNIF = author.Id.AsString();
                return author;
            }
            catch (Exception e)
            {
                throw new BusinessRuleValidationException("Author not Found");
            }
        }

       


        public async Task<bool> validateIsbnIsUnique(string isbn)
        {
            var existingBook = await _bookRepository.GetByIsbnAsync(isbn);
            if (existingBook != null)
            {
                return false;
            }
            return true;

        }

        public async Task<List<BookDto>> GetAllAsync()
        {
            var list = await _bookRepository.GetAllAsync();

            List<BookDto> listDto = new List<BookDto>();

            foreach (Book book in list)
            {
                var authorNIF = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.NIF.nif;
                listDto.Add(BookMapper.toDto(book, authorNIF));
            }
            return listDto;

        }
    }
}


