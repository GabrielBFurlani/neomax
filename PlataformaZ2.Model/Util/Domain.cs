////-----------------------------------------------------------------------
//// <copyright file="Domain.cs" company="Zetacorp">
////  (R) Registrado 2018 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Model.Util
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
    public enum Weekday
    {
        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Domingo")]
        Domingo = 0,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Segunda-Feira")]
        SegundaFeira = 1,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Terça-Feira")]
        TercaFeira = 2,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Quarta-Feira")]
        QuartaFeira = 3,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Quinta-Feira")]
        QuintaFeira = 4,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Sexta-Feira")]
        SextaFeira = 5,

        /// <summary> Indicates a day of week </summary>
        [DescriptionAttribute("Sabado")]
        Sabado = 6
    }

    /// <summary>
    /// Enumeration representing all the permissions
    /// </summary>
    public enum Permissions
    {
        // <summary> Indicates a permission </summary>
        [Description("Carregar Minha Sessão de Usuário")]
        LoadMyUserSession = 1,

        /// <summary> Indicates a permission </summary>
        [Description("Editar Meus Dados")]
        EditMyUser = 2,

        // <summary> Indicates a permission </summary>
        [Description("Alterar Minha Senha")]
        ChangeMyPassword = 3,

        /// <summary> Indicates a permission </summary>
        [Description("Pesquisar/Visualizar Usuários")]
        ReadUsers = 4,

        /// <summary> Indicates a permission </summary>
        [Description("Cadastrar/Editar/Excluir Usuário")]
        EditUser = 5,

        /// <summary> Indicates a permission </summary>
        [Description("Gerenciar Todos os Usuários")]
        ManageAllUsers = 6,

        // <summary> Indicates a permission </summary>
        [Description("Pesquisar/Visualizar Perfis de Usuário")]
        ReadProfiles = 7
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
