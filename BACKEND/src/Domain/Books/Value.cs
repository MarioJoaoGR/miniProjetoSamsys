using DDDSample1.Domain.Shared;

namespace DDDNetCore.Domain.Books
{
    public class Value : IValueObject
    {
        public string value { get; private set; }

        public Value(string value)
        {
            ValidateValue(value);
            this.value = value;
        }

        public void ValidateValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new System.ArgumentException("Price cannot be null or empty.");
            }

            if (!decimal.TryParse(value, out decimal decimalValue))
            {
                throw new System.ArgumentException("Price must be a valid decimal number.");
            }

            if (decimalValue < 0)
            {
                throw new System.ArgumentException("Price cannot be negative.");
            }

            if (decimal.Round(decimalValue, 2) != decimalValue)
            {
                throw new System.ArgumentException("Price must have two decimal places.");
            }
        }
    }
}

