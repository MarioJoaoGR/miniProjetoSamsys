using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<MessagingHelper<BookDto>> AddAsync(CreatingBookDto dto)
        {
            bool isbnIsUnique = await validateIsbnIsUnique(dto.Isbn);
            if (!isbnIsUnique)
            {
                return MessagingHelper<BookDto>.ErrorMessage("Isbn already exists", ErrorType.DataHasChanged);
            }

            var author = await checkAuthorByNIFAsync(dto.AuthorNIF, dto);
            if (author == null)
            {
                return MessagingHelper<BookDto>.ErrorMessage("Author not found", ErrorType.NotFound);
            }

            var book = new Book(dto.Isbn, dto.Title, dto.AuthorNIF, dto.Value);
            await _bookRepository.AddAsync(book);
            await _unitOfWork.CommitAsync();

            var bookDto = BookMapper.toDto(book, author.NIF.nif, author.FullName.fullName);
            return MessagingHelper<BookDto>.SuccessMessage(bookDto);
        }

        public async Task<MessagingHelper<BookDto>> UpdateAsync(EditingBookDto dto)
        {
            var book = await _bookRepository.GetByIdAsync(new BookId(dto.Id));
            if (book == null)
            {
                return MessagingHelper<BookDto>.ErrorMessage("Book not found", ErrorType.NotFound);
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
                    return MessagingHelper<BookDto>.ErrorMessage("Isbn already exists", ErrorType.DataHasChanged);
                }
                book.ChangeIsbn(dto.Isbn);
            }

            if (!string.IsNullOrWhiteSpace(dto.Value) && !book.Value.value.Equals(dto.Value))
            {
                book.ChangeValue(dto.Value);
            }

            if (!string.IsNullOrWhiteSpace(dto.AuthorNIF))
            {
                var author = await checkAuthorByNIFForEditingAsync(dto.AuthorNIF, dto);
                if (author == null)
                {
                    return MessagingHelper<BookDto>.ErrorMessage("Author not found", ErrorType.NotFound);
                }

                if (!book.AuthorId.Equals(dto.AuthorNIF))
                {
                    book.ChangeAuthor(dto.AuthorNIF);
                }
            }

            await _unitOfWork.CommitAsync();

            var authorData = await _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId));
            var bookDto = BookMapper.toDto(book, authorData.NIF.nif, authorData.FullName.fullName);

            return MessagingHelper<BookDto>.SuccessMessage(bookDto);
        }

        public async Task<MessagingHelper<BookDto>> DeleteAsync(BookId id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return MessagingHelper<BookDto>.ErrorMessage("Book not found", ErrorType.NotFound);
            }

            if (book.bookStatus == BookStatus.Inactive)
            {
                return MessagingHelper<BookDto>.ErrorMessage("Book is already inactive", ErrorType.DataHasChanged);
            }

            var authorData = await _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId));
            book.Deactivate();
            await _unitOfWork.CommitAsync();

            var bookDto = BookMapper.toDto(book, authorData.NIF.nif, authorData.FullName.fullName);
            return MessagingHelper<BookDto>.SuccessMessage(bookDto);
        }

        public async Task<MessagingHelper<List<BookDto>>> SearchAsync(BookFilterDto dto)
        {
            var books = new List<Book>();

            if (string.IsNullOrWhiteSpace(dto.Isbn) && string.IsNullOrWhiteSpace(dto.Title) && string.IsNullOrWhiteSpace(dto.AuthorName) && string.IsNullOrWhiteSpace(dto.ValueOrder))
            {
                books = await _bookRepository.GetAllActiveAsync();
            }
            else
            {
                books = await _bookRepository.GetByFiltersAsync(dto.Isbn, dto.Title);
                if (!string.IsNullOrWhiteSpace(dto.AuthorName))
                {
                    var authorList = await _authorRepository.FilterByNameAsync(dto.AuthorName);
                    var authorIds = authorList.Select(a => a.Id.Value).ToList();
                    books = books.Where(b => authorIds.Contains(b.AuthorId)).ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(dto.ValueOrder))
            {
                books = OrderByValue(books, dto.ValueOrder);
            }

            var authorIdsToLoad = books.Select(b => new AuthorId(b.AuthorId)).Distinct().ToList();
            var authors = await _authorRepository.GetByIdsAsync(authorIdsToLoad);
            var authorDictionary = authors.ToDictionary(a => a.Id.Value, a => a);

            var result = books.Select(book =>
            {
                var author = authorDictionary[book.AuthorId];
                return BookMapper.toDto(book, author.NIF.nif, author.FullName.fullName);
            }).ToList();

            return MessagingHelper<List<BookDto>>.SuccessMessage(result);
        }

        public async Task<MessagingHelper<List<BookDto>>> GetBooksByAuthorAsync(string authorId)
        {
            var books = await _bookRepository.GetBooksByAuthorAsync(authorId);

            if (books == null || books.Count == 0)
            {
                return MessagingHelper<List<BookDto>>.ErrorMessage("No books found for the given author", ErrorType.NotFound);
            }

            var bookDtos = await Task.WhenAll(books.Select(async book =>
            {
                var authorData = await _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId));
                return BookMapper.toDto(book, authorData.NIF.nif, authorData.FullName.fullName);
            }));

            return MessagingHelper<List<BookDto>>.SuccessMessage(bookDtos.ToList());
        }


        public async Task<MessagingHelper<BookDto>> GetByIdAsync(BookId id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return MessagingHelper<BookDto>.ErrorMessage("Book not found", ErrorType.NotFound);
            }

            var authorData = await _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId));
            var bookDto = BookMapper.toDto(book, authorData.NIF.nif, authorData.FullName.fullName);

            return MessagingHelper<BookDto>.SuccessMessage(bookDto);
        }

        public async Task<MessagingHelper<List<BookDto>>> GetAllAsync()
        {
            var list = await _bookRepository.GetAllAsync();
            if (list == null || list.Count == 0)
            {
                return MessagingHelper<List<BookDto>>.ErrorMessage("No books found", ErrorType.NotFound);
            }

            var listDto = await Task.WhenAll(list.Select(async book =>
            {
                var authorData = await _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId));
                return BookMapper.toDto(book, authorData.NIF.nif, authorData.FullName.fullName);
            }));

            return MessagingHelper<List<BookDto>>.SuccessMessage(listDto.ToList());
        }

        // Método para obter todos os livros ativos
        public async Task<MessagingHelper<List<BookDto>>> GetAllActiveAsync()
        {
            var books = await _bookRepository.GetAllActiveAsync();
            if (books == null || books.Count == 0)
            {
                return MessagingHelper<List<BookDto>>.ErrorMessage("No active books found", ErrorType.NotFound);
            }

            var bookDtos = await Task.WhenAll(books.Select(async book =>
            {
                var authorData = await _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId));
                return BookMapper.toDto(book, authorData.NIF.nif, authorData.FullName.fullName);
            }));

            return MessagingHelper<List<BookDto>>.SuccessMessage(bookDtos.ToList());
        }

        // Método para obter todos os livros inativos
        public async Task<MessagingHelper<List<BookDto>>> GetAllInactiveAsync()
        {
            var books = await _bookRepository.GetAllInactiveAsync();
            if (books == null || books.Count == 0)
            {
                return MessagingHelper<List<BookDto>>.ErrorMessage("No inactive books found", ErrorType.NotFound);
            }

            var bookDtos = await Task.WhenAll(books.Select(async book =>
            {
                var authorData = await _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId));
                return BookMapper.toDto(book, authorData.NIF.nif, authorData.FullName.fullName);
            }));

            return MessagingHelper<List<BookDto>>.SuccessMessage(bookDtos.ToList());
        }

        private async Task<Author> checkAuthorByNIFAsync(string nif, CreatingBookDto book)
        {
            var author = await _authorRepository.GetByNIFAsync(nif);
            if (author == null)
            {
                throw new BusinessRuleValidationException("Author not found");
            }
            book.AuthorNIF = author.Id.AsString();
            return author;
        }

        private async Task<Author> checkAuthorByNIFForEditingAsync(string nif, EditingBookDto book)
        {
            var author = await _authorRepository.GetByNIFAsync(nif);
            if (author == null)
            {
                throw new BusinessRuleValidationException("Author not found");
            }
            book.AuthorNIF = author.Id.AsString();
            return author;
        }

        public async Task<bool> validateIsbnIsUnique(string isbn)
        {
            var existingBook = await _bookRepository.GetByIsbnAsync(isbn);
            return existingBook == null;
        }

        public List<Book> OrderByValue(List<Book> books, string valueOrder)
        {
            if (valueOrder.ToLower() == "increasing")
            {
                return books.OrderBy(b => decimal.Parse(b.Value.value)).ToList();
            }
            else if (valueOrder.ToLower() == "decreasing")
            {
                return books.OrderByDescending(b => decimal.Parse(b.Value.value)).ToList();
            }
            return books; // Return without sorting if the parameter is invalid
        }
    }
}
