using DDDSample1.Domain.Shared;
using System;
using System.Text.RegularExpressions;

namespace DDDNetCore.Domain.Authors
{
    public class NIF : IValueObject
    {
        public string nif { get; private set; }


        public NIF(string nif)
        {
           validateNIF(nif);
           this.nif = nif;
        }

        private void validateNIF(string nif)
        {
            if (string.IsNullOrWhiteSpace(nif))
                throw new BusinessRuleValidationException("O NIF não pode estar vazio.");

            if (!Regex.IsMatch(nif, @"^\d{9}$"))
                throw new BusinessRuleValidationException("O NIF deve conter exatamente 9 dígitos.");

            if (!IsValidCheckDigit(nif))
                throw new BusinessRuleValidationException("O NIF é inválido.");
        }

        private bool IsValidCheckDigit(string nif)
        {
            int[] pesos = { 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;

            for (int i = 0; i < 8; i++)
            {
                soma += (nif[i] - '0') * pesos[i];
            }

            int digitoVerificacao = 11 - (soma % 11);
            if (digitoVerificacao == 10) digitoVerificacao = 0;

            return digitoVerificacao == (nif[8] - '0');
        }
    }

}
