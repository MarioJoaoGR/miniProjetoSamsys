using System;
using DDDSample1.Domain.Shared;

namespace DDDNetCore.Domain.Authors
{
    public class Author : Entity<AuthorId>, IAggregateRoot
    {
        public FullName FullName { get; private set; }
        public NIF NIF { get; private set; }


        private Author() { }

        public Author(string fullName, string nif)
        {
            this.Id = new AuthorId(Guid.NewGuid());
            this.FullName = new FullName(fullName);
            this.NIF = new NIF(nif);

        }

        public void ChangeFullName(string fullName)
        {
            this.FullName = new FullName(fullName);
        }

        public void ChangeNIF(string nif)
        {
            this.NIF = new NIF(nif);
        }

    }
}
