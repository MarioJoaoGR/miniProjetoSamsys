using System;
using System.Collections;
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

        public async Task<List<BookDto>> SearchAsync(BookFilterDto dto)
        {
            
            var books = new List<Book>();

            // Se nenhum filtro for fornecido, retorna todos os livros
            if (string.IsNullOrWhiteSpace(dto.Isbn) &&
                string.IsNullOrWhiteSpace(dto.Title) &&
                string.IsNullOrWhiteSpace(dto.AuthorName) &&
                string.IsNullOrWhiteSpace(dto.ValueOrder))
            {
                books = await _bookRepository.GetAllActiveAsync();
            }
            else
            {
        
                // Filtragem inicial pelos filtros de ISBN e título
                books = await _bookRepository.GetByFiltersAsync(dto.Isbn, dto.Title);

                // Se um nome de autor foi fornecido, filtramos os livros pelos autores encontrados
                if (!string.IsNullOrWhiteSpace(dto.AuthorName))
                {
                    var authorList = await _authorRepository.FilterByNameAsync(dto.AuthorName);
                    var authorIds = authorList.Select(a => a.Id.Value).ToList();

                    // Filtra os livros que têm um authorId que corresponde aos autores encontrados
                    books = books.Where(b => authorIds.Contains(b.AuthorId)).ToList();
                }
            }

            // Se houver ordenação por preço, aplicamos a ordenação no final
            if (!string.IsNullOrWhiteSpace(dto.ValueOrder))
            {
                books = OrderByValue(books, dto.ValueOrder);
            }


            // Convert the list of string authorIds to a list of AuthorId objects
            var authorIdsToLoad = books.Select(b => new AuthorId(b.AuthorId)).Distinct().ToList();
            var authors = await _authorRepository.GetByIdsAsync(authorIdsToLoad);
            var authorDictionary = authors.ToDictionary(a => a.Id.Value, a => a);

            // Mapeando os livros para DTOs
            var result = new List<BookDto>();

            foreach (var book in books)
            {
                // Recupera o autor diretamente do dicionário
                var author = authorDictionary[book.AuthorId];

                // Mapeando o livro para DTO
                var bookDto = BookMapper.toDto(book, author.NIF.nif, author.FullName.fullName);

                result.Add(bookDto);
            }

            return result;
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

        public async Task<List<BookDto>> GetAllActiveAsync()
        {
            var list = await _bookRepository.GetAllActiveAsync();

            List<BookDto> listDto = new List<BookDto>();

            foreach (Book book in list)
            {
                var authorNIF = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.NIF.nif;
                var authorName = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.FullName.fullName;
                listDto.Add(BookMapper.toDto(book, authorNIF, authorName));
            }
            return listDto;

        }

        public async Task<List<BookDto>> GetAllInactiveAsync()
        {
            var list = await _bookRepository.GetAllInactiveAsync();

            List<BookDto> listDto = new List<BookDto>();

            foreach (Book book in list)
            {
                var authorNIF = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.NIF.nif;
                var authorName = _authorRepository.GetByIdAsync(new AuthorId(book.AuthorId)).Result.FullName.fullName;
                listDto.Add(BookMapper.toDto(book, authorNIF, authorName));
            }
            return listDto;

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

            return books; // Retorna sem ordenação se o parâmetro for inválido.
        }



    }

}


