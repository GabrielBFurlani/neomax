////-----------------------------------------------------------------------
//// <copyright file="PropertyFormatter.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Business.Util
{
    using System;

    /// <summary>
    /// Formats different kinds of properties according its specification
    /// </summary>
    public static class PropertyFormatter
    {
        /// <summary>
        /// Formats a CPF in its known format
        /// </summary>
        /// <param name="cpf">CPF without formatting</param>
        /// <returns>Formatted CPF</returns>
        public static string FormatCpf(string cpf)
        {
            UInt64 convertedCpf;

            if (UInt64.TryParse(cpf, out convertedCpf))
            {
                return convertedCpf.ToString(@"000\.000\.000\-00");
            }

            return string.Empty;
        }

        /// <summary>
        /// Formats a CNPJ in its known format
        /// </summary>
        /// <param name="cnpj">CNPJ without formatting</param>
        /// <returns>Formatted CNPJ</returns>
        public static string FormatCnpj(string cnpj)
        {
            UInt64 convertedCnpj;

            if (UInt64.TryParse(cnpj, out convertedCnpj))
            {
                return convertedCnpj.ToString(@"00\.000\.000\/0000\-00");
            }

            return string.Empty;
        }
    }
}
