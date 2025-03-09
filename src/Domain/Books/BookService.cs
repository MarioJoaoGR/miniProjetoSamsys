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
            var authorNIF = dto.AuthorNIF;
            var authorName = _authorRepository.GetByNIFAsync(dto.AuthorNIF).Result.FullName.fullName;

            await checkAuthorByNIFAsync(dto.AuthorNIF, dto);




            var book = new Book(dto.Isbn, dto.Title, dto.AuthorNIF, dto.Value);



            await _bookRepository.AddAsync(book);
            await _unitOfWork.CommitAsync();

            return BookMapper.toDto(book, authorNIF, authorName);

        }


        public async Task<BookDto> UpdateAsync(EditingBookDto dto)
        {
            var book = await _bookRepository.GetByIdAsync(new BookId(dto.Id));

            if (book == null)
            {
                throw new BusinessRuleValidationException("Book not found");
            }




            if (!string.IsNullOrWhiteSpace(dto.Title) && !book.Title.title.Equals(dto.Title))
            {
                book.ChangeTitle(dto.Title);

            }

            if (!string.IsNullOrWhiteSpace(dto.Isbn) && !book.Isbn.isbn.Equals(dto.Isbn))
            {
                bool isbnIsUnique = await validateIsbnIsUnique(dto.Isbn);
                if (!isbnIsUnique && !book.Isbn.isbn.Equals(dto.Isbn))
                {
                    throw new BusinessRuleValidationException("Isbn already exists");
                }
                book.ChangeIsbn(dto.Isbn);

            }

            if (!string.IsNullOrWhiteSpace(dto.Value) && !book.Value.value.Equals(dto.Value))
            {
                book.ChangeValue(dto.Value);
            }


            if (!string.IsNullOrWhiteSpace(dto.AuthorNIF))
            {

                await checkAuthorByNIFForEditingAsync(dto.AuthorNIF, dto);
                if (!book.AuthorId.Equals(dto.AuthorNIF))
                {
                    book.ChangeAuthor(dto.AuthorNIF);
                }
            }

            var authorNIF = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.NIF.nif;
            var authorName = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.FullName.fullName;







            await _unitOfWork.CommitAsync();



            return BookMapper.toDto(book, authorNIF, authorName);



        }


        public async Task<BookDto> DeleteAsync(BookId id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            var authorNIF = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.NIF.nif;
            var authorName = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.FullName.fullName;

            if (book == null)
            {
                throw new BusinessRuleValidationException("Book not found");
            }

            if (book.bookStatus == BookStatus.Inactive)
            {
                throw new BusinessRuleValidationException("Book is already inactive");
            }


            book.Deactivate();

            await _unitOfWork.CommitAsync();


            return BookMapper.toDto(book, authorNIF, authorName);
        }



        public async Task<BookDto> GetByIdAsync(BookId id)
        {

            var book = await _bookRepository.GetByIdAsync(id);
            var authorNIF = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.NIF.nif;
            var authorName = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.FullName.fullName;

            return book == null ? null : BookMapper.toDto(book, authorNIF, authorName);
        }


        public async Task<Author> checkAuthorByNIFAsync(string nif, CreatingBookDto book)
        {
            try
            {
                var author = await _authorRepository.GetByNIFAsync(nif);
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

        public async Task<Author> checkAuthorByNIFForEditingAsync(string name, EditingBookDto book)
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
                var authorName = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.FullName.fullName;
                listDto.Add(BookMapper.toDto(book, authorNIF, authorName));
            }
            return listDto;

        }
    }

        



}


