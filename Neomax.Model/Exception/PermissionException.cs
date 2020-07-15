////-----------------------------------------------------------------------
//// <copyright file="PermissionException.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Exception
{
    using System;

    /// <summary>
    /// Permission Exception definition
    /// </summary>
    public class PermissionException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionException" /> class.
        /// </summary>
        public PermissionException() : base()
        {
        }
    }
}
