using System;
using DDDSample1.Domain.Shared;

namespace DDDNetCore.Domain.Authors
{
    public class FullName : IValueObject
    {
        public string fullName { get; private set; }

        public FullName(string fullName)
        {
            validateFullName(fullName);
            fullName.Trim();
            this.fullName = fullName;
        }

        private void validateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentNullException("Invalid name");
            }
            
            if(fullName.Length > 100 || fullName.Length < 3)
            {
                throw new BusinessRuleValidationException("Invalid name length");
            }
        }


        // Método para obter o primeiro nome
        public string getFirstName()
        {
            var names = fullName.Split(' ');
            return names[0]; // O primeiro elemento da lista é o primeiro nome
        }

        // Método para obter o último nome
        public string getLastName()
        {
            var names = fullName.Split(' ');
            return names[names.Length - 1]; // O último elemento da lista é o último nome
        }

        public static implicit operator string(FullName v)
        {
            return new FullName(v);
        }
    }
}