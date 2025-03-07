namespace DDDNetCore.Domain.Books
{
    public class CreatingBookDto
    {
        public required string Isbn { get; set; }
        public required string Title { get; set; }
        public required string AuthorNIF { get; set; }
        public required string Value { get; set; }

    }
}
