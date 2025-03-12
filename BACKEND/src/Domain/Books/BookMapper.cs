namespace DDDNetCore.Domain.Books
{
    public class BookMapper
    {
        public static BookDto toDto(Book obj, string authorNIF, string authorName)
        {
            return new BookDto(
                obj.Id.AsGuid(),
                obj.Isbn.isbn,
                obj.Title.title,
                authorNIF,
                authorName,
                obj.Value.value,
                obj.bookStatus.ToString()
            );
        }
    }
}
