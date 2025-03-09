using System;

namespace DDDNetCore.Domain.Books
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Isbn { get; private set; }
        public string Title { get; private set; }
        public string AuthorNIF { get; private set; }
        public string AuthorName { get; private set; }
        public string Value { get; private set; }
        public string BookStatus { get; private set; }

        public BookDto(Guid Id, string isbn, string title, string authorNIF, string authorName, string value, string bookStatus) {


            this.Id = Id;
            this.Isbn = isbn;
            this.Title = title;
            this.AuthorNIF = authorNIF;
            this.AuthorName = authorName;
            this.Value = value;
            this.BookStatus = bookStatus;
        }
    }
}
