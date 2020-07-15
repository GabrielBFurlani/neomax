////-----------------------------------------------------------------------
//// <copyright file="OperationResult.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Util
{
    /// <summary>
    /// Operation Result model
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult" /> class.
        /// </summary>
        /// <param name="message">Result message</param>
        /// <param name="data">Object returned by operation</param>
        public OperationResult(string message, object data)
        {
            this.Message = message;
            this.Data = data;
        }
        
        /// <summary> Gets or sets the message of the operation </summary>
        public string Message { get; set; }

        /// <summary> Gets or sets the data object of the operation </summary>
        public object Data { get; set; }
    }
}
