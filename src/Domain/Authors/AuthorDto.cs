using System;

namespace DDDNetCore.Domain.Authors
{
    public class AuthorDto
    {
        public Guid Id { get; }
        public string FullName { get; }
        public string NIF { get; }

        public AuthorDto(Guid id, string fullName, string nif)
        {
            Id = id;
            FullName = fullName;
            NIF = nif;
        }
    }
}
