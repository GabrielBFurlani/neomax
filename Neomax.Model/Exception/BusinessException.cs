////-----------------------------------------------------------------------
//// <copyright file="BusinessException.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Exception
{
    using System;

    /// <summary>
    /// Business Exception definition
    /// </summary>
    public class BusinessException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException" /> class.
        /// </summary>
        public BusinessException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public BusinessException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception object</param>
        public BusinessException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
