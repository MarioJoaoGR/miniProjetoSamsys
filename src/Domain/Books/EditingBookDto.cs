using System;

namespace DDDNetCore.Domain.Books
{
    public class EditingBookDto
    {
        public required Guid Id { get; set; }
        public string? Isbn { get; set; }
        public string? Title { get; set; }
        public string? AuthorNIF { get; set; }
        public string? Value { get; set; }
        

        public EditingBookDto(Guid id, string isbn, string title, string authorNIF, string value)
        {
            this.Id = id;
            this.Isbn = isbn;
            this.Title = title;
            this.AuthorNIF = authorNIF;
            this.Value = value;
         
        }
    }
}
