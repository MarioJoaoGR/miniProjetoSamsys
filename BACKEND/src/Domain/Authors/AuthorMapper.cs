
namespace DDDNetCore.Domain.Authors
{
    public class AuthorMapper
    {
        public static AuthorDto toDto(Author obj)
        {
            return new AuthorDto(
                obj.Id.AsGuid(),
                obj.FullName.fullName,
                obj.NIF.nif
            );
        }
    }
}
