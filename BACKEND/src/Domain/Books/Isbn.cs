using System.Linq;
using DDDSample1.Domain.Shared;

namespace DDDNetCore.Domain.Books
{
    public class Isbn : IValueObject
    {
        public string isbn { get; private set; }

        public Isbn(string isbn)
        {
            ValidateIsbn(isbn);
            this.isbn = isbn;
        }

        public void ValidateIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new BusinessRuleValidationException("ISBN cannot be empty.");

            isbn = isbn.Replace("-", "").Replace(" ", ""); // Remove traços e espaços

            if (isbn.Length == 10)
            {
                if (!IsValidIsbn10(isbn))
                    throw new BusinessRuleValidationException("Invalid ISBN-10 format.");
            }
            else if (isbn.Length == 13)
            {
                if (!IsValidIsbn13(isbn))
                    throw new BusinessRuleValidationException("Invalid ISBN-13 format.");
            }
            else
            {
                throw new BusinessRuleValidationException("ISBN must be either 10 or 13 characters long.");
            }
        }

        private bool IsValidIsbn10(string isbn)
        {
            if (!isbn.Substring(0, 9).All(char.IsDigit) ||
                (!char.IsDigit(isbn[9]) && isbn[9] != 'X'))
                return false;

            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += (isbn[i] - '0') * (10 - i);

            int checkDigit = (isbn[9] == 'X') ? 10 : isbn[9] - '0';
            sum += checkDigit;

            return sum % 11 == 0;
        }

        private bool IsValidIsbn13(string isbn)
        {
            if (!isbn.All(char.IsDigit))
                return false;

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = isbn[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == (isbn[12] - '0');
        }
    }
}
