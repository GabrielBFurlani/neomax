////-----------------------------------------------------------------------
//// <copyright file="Domain.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Util
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Enumeration representing days of week
    /// </summary>
    public enum ContactDay
    {
        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Segunda-Feira")]
        Monday = 1,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Terça-Feira")]
        Tuesday = 2,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Quarta-Feira")]
        Wednesday = 3,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Quinta-Feira")]
        Thursday = 4,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Sexta-Feira")]
        Friday = 5
    }

    public enum ContactTime
    {
        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("09:00h até as 10:00h")]
        Time09to10 = 0,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("10:00h até as 11:00h")]
        Time10to11 = 1,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("11:00h até as 12:00h")]
        Time11to12 = 2,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("12:00h até as 13:00h")]
        Time12to13 = 3,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("13:00h até as 14:00h")]
        Time13to14 = 4,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("14:00h até as 15:00h")]
        Time14to15 = 5,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("15:00h até as 16:00h")]
        Time15to16 = 6,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("16:00h até as 17:00h")]
        Time16to17 = 7,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("17:00h até as 18:00h")]
        Time17to18 = 8
    }

    public enum Gender
    {
        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Masculino")]
        Male = 0,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Feminino")]
        Feminine = 1,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Não informado/Indiferente")]
        Uninformed = 2
    }

    public enum TypeNoteEmited
    {
        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Nota de Produto")]
        ProductNote = 0,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Nota de Serviço")]
        ServiceNote = 1,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Nota de Produto e Serviço")]
        ProductServiceNote = 2
    }

    public enum AnnualBilling
    {
        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Até R$ 1 milhão")]
        ToOne = 0,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("De R$ 1 milhão até R$ 2 milhões")]
        OneToTwo = 1,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("De R$ 2 milhão até R$ 4 milhões")]
        TwoToFour = 2,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Mais que R$ 4 milhões")]
        FourPlus = 3
    }

    public enum CompanyNatureTypes
    {
        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Empresário Individual (EI)")]
        EI = 0,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Microempresa (ME)")]
        ME = 1,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Microempresa Individual (MEU)")]
        MEU = 2,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Empresário Individual de Responsabilidade Limitada (EIRELI)")]
        EIRELI = 3,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Empresa de Pequeno Porte (EPP)")]
        EPP = 4,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Empresa de Responsabilidade Limitada (LTDA)")]
        LTDA = 5,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Sociedade Anônima (S/A)")]
        Time15to16 = 6,
    }

    public enum SolicitationStatus
    {
        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Aguardando Aprovação")]
        WaitingApprove = 0,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Necessita de Revisão")]
        RevisionSolicited = 1,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Aprovado")]
        Approved = 2,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Reprovado")]
        Disapproved = 3,
    }

    public enum TelephoneType
    {
        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Fixo")]
        Telephone = 0,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Celular")]
        Celphone = 1,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Comercial")]
        Commerce = 2
    }



    /// <summary>
    /// Enumeration representing all the permissions
    /// </summary>
    public enum Permissions
    {

    }

    /// <summary>
    /// Responsible for domain management
    /// </summary>
    public static class Domain
    {
        /// <summary>
        /// Gets attribute "Description" (or name) of enumeration.
        /// </summary>
        /// <param name="value">enumeration value.</param>
        /// <returns>Attribute "Description" or text value</returns>
        public static string TextValueFrom(Enum value)
        {
            FieldInfo fieldInfo;
            DescriptionAttribute[] attributes;

            if (value != null)
            {
                fieldInfo = value.GetType().GetField(value.ToString());
                attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
                else
                {
                    return value.ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets enumeration value based on parameter text.
        /// </summary>
        /// <param name="value">Text to return the enumeration value.</param>
        /// <param name="enumType">Enumeration type.</param>
        /// <returns>Attribute "Description" or text value</returns>
        public static object EnumValueFrom(string value, Type enumType)
        {
            string[] names = Enum.GetNames(enumType);
            foreach (string name in names)
            {
                if (TextValueFrom((Enum)Enum.Parse(enumType, name)).Equals(value))
                {
                    return Enum.Parse(enumType, name);
                }

                if (name.Equals(value))
                {
                    return Enum.Parse(enumType, name);
                }
            }

            throw new ArgumentException("O texto não é uma descrição ou valor do enum especificado.");
        }

        /// <summary>
        /// Gets a list based in enumeration parameter.
        /// </summary>
        /// <param name="enumType">Enumeration type.</param>
        /// <returns>List of enumeration values and descriptions</returns>
        public static IDictionary<int, string> GetEnumList(Type enumType)
        {
            Dictionary<int, string> list = new Dictionary<int, string>();

            foreach (string s in Enum.GetNames(enumType))
            {
                object value = Enum.Parse(enumType, s);
                int i = (int)value;
                string description = TextValueFrom((Enum)value);

                list.Add(i, description);
            }

            return list;
        }
    }
}
