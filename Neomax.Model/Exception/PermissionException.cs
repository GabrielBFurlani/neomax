////-----------------------------------------------------------------------
//// <copyright file="PermissionException.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
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
