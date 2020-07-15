////-----------------------------------------------------------------------
//// <copyright file="PropertyValidator.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Business.Util
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Validate different kinds of properties according its specification
    /// </summary>
    public static class PropertyValidator
    {
        /// <summary>
        /// Check if a text string contains only numbers (used for password checking)
        /// </summary>
        /// <param name="text">text string</param>
        /// <returns>Boolean result</returns>
        public static bool ValidateOnlyNumberText(string text)
        {
            Regex onlyNumberRegex = new Regex(@"^[0-9]*$");

            //return "true" if text is valid (contains only numbers)
            return onlyNumberRegex.IsMatch(text);
        }

        /// <summary>
        /// Check if a text string contains at least a letter and a number (used for password checking)
        /// </summary>
        /// <param name="text">text string</param>
        /// <returns>Boolean result</returns>
        public static bool ValidateAtLeastLetterNumberText(string text)
        {
            //return "true" if text is valid (contains at least a letter and a number)
            return (text.Any(char.IsLetter) && text.Any(char.IsNumber));            
        }

        /// <summary>
        /// Check if e-mail format is valid
        /// </summary>
        /// <param name="email">E-mail</param>
        /// <returns>Boolean result</returns>
        public static bool ValidateEmail(string email)
        {
            Regex emailRegex = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");

            //return "true" if email is valid
            return emailRegex.IsMatch(email);
        }

        /// <summary>
        /// Check if CPF is valid according to its digits
        /// </summary>
        /// <param name="cpf">CPF</param>
        /// <returns>Boolean result</returns>
        public static bool ValidateCpf(string cpf)
        {
            int[] multiplier1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digit;
            int sum;
            int rest;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11 || cpf.Equals("00000000000") || cpf.Equals("11111111111") || cpf.Equals("22222222222")
                || cpf.Equals("33333333333") || cpf.Equals("44444444444") || cpf.Equals("55555555555")
                || cpf.Equals("66666666666") || cpf.Equals("77777777777") || cpf.Equals("88888888888")
                || cpf.Equals("99999999999"))
            {
                return false;
            }

            tempCpf = cpf.Substring(0, 9);
            sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];
            rest = sum % 11;
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;
            digit = rest.ToString();
            tempCpf = tempCpf + digit;
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];
            rest = sum % 11;
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;
            digit = digit + rest.ToString();
            return cpf.EndsWith(digit);
        }

        /// <summary>
        /// Check if CNPJ is valid according to its digits
        /// </summary>
        /// <param name="cnpj">CNPJ</param>
        /// <returns>Boolean result</returns>
        public static bool ValidateCNPJ(string cnpj)
        {
            int[] multiplier = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum;
            int rest;
            string digit;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);
            sum = 0;
            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier[i];
            rest = (sum % 11);
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit = rest.ToString();
            tempCnpj = tempCnpj + digit;
            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];
            rest = (sum % 11);
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;
            digit = digit + rest.ToString();

            return cnpj.EndsWith(digit);
        }
    }
}
