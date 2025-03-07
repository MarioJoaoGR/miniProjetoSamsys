namespace DDDNetCore.Domain.Books
{
    public class BookMapper
    {
        public static BookDto toDto(Book obj, string authorNIF)
        {
            return new BookDto(
                obj.Id.AsGuid(),
                obj.Isbn.isbn,
                obj.Title.title,
                authorNIF,
                obj.Value.value,
                obj.bookStatus.ToString()
            );
        }
    }
}
