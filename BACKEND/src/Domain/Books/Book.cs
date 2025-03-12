using System;
using DDDNetCore.Domain.Authors;
using DDDSample1.Domain.Shared;

namespace DDDNetCore.Domain.Books
{
    public class Book : Entity<BookId>, IAggregateRoot
    {
        public Isbn Isbn { get; private set; }
        public Title Title { get; private set; }
        public String AuthorId { get; private set; }
        public Value Value { get; private set; }
        public BookStatus bookStatus { get; private set; }

        private Book() { }

        public Book(string isbn, string title, String authorId, string price)
        {
            this.Id = new BookId(Guid.NewGuid());
            this.Isbn = new Isbn(isbn);
            this.Title = new Title(title);
            this.AuthorId = authorId;
            this.Value = new Value(price);
            this.bookStatus = BookStatus.Active;
        }

        public void ChangeIsbn(string isbn)
        {
            this.Isbn = new Isbn(isbn);
        }

        public void ChangeTitle(string title)
        {
            this.Title = new Title(title);
        }

        public void ChangeAuthor(String authorId)
        {
            this.AuthorId = new AuthorId(authorId).Value;
        }

        public void ChangeValue(string value)
        {
            this.Value = new Value(value);
        }

        public void Deactivate()
        {
            this.bookStatus = BookStatus.Inactive;
        }
        public void Activate()
        {
            this.bookStatus = BookStatus.Active;
        }
    }
}
