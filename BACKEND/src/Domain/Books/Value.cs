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
                throw new BusinessRuleValidationException("Price cannot be null or empty.");
            }

            if (!decimal.TryParse(value, out decimal decimalValue))
            {
                throw new BusinessRuleValidationException("Price must be a valid decimal number.");
            }

            if (decimalValue < 0)
            {
                throw new BusinessRuleValidationException("Price cannot be negative.");
            }

            // Verifica se o preço tem exatamente duas casas decimais
            var parts = value.Split(',');
            if (parts.Length > 2) // Se houver mais de um ponto, é um formato inválido
            {
                throw new BusinessRuleValidationException("Price must be a valid decimal number.");
            }

            // Se tiver parte decimal, valida se tem exatamente 2 casas
            if (parts.Length == 2 && parts[1].Length != 2)
            {
                throw new BusinessRuleValidationException("Price must have exactly two decimal places.");
            }

            // Se o valor não tiver parte decimal, garante que é um número inteiro sem ponto
            if (parts.Length == 1)
            {
                value = value + ",00"; // Adiciona as duas casas decimais
            }
        }
    }
}
