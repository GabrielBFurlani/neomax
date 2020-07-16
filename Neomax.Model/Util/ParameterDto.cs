////-----------------------------------------------------------------------
//// <copyright file="ParameterDto.cs" company="GabrielFurlani">
////  (R) Registrado 2020 Gabriel Furlani.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Util
{

    /// <summary>
    /// Transitional model representing Parameter (for example, Enum)
    /// </summary>
    public class ParameterDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterDto" /> class.
        /// </summary>
        public ParameterDto()
        {
        }

        /// <summary> Gets or sets the parameter </summary>
        public int Parameter { get; set; }

        /// <summary> Gets or sets the parameter name </summary>
        public string Name { get; set; }
    }
}
