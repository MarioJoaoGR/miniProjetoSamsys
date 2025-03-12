using DDDSample1.Domain.Shared;

namespace DDDNetCore.Domain.Books
{
    public class Title : IValueObject
    {
        public string title { get; private set; }

        public Title(string title)
        {
            ValidateTitle(title);
            this.title = title;
        }

        private void ValidateTitle(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new System.ArgumentException("Title cannot be null or empty.");
            }
        }
    }
}
