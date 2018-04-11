////-----------------------------------------------------------------------
//// <copyright file="OperationResult.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Model.Util
{
    /// <summary>
    /// Operation Result
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult" /> class.
        /// </summary>
        /// <param name="operationSuccess">Indicates whether operation was executed with success</param>
        public OperationResult(bool operationSuccess)
        {
            this.Success = operationSuccess;
            this.Message = null;
            this.Data = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult" /> class.
        /// </summary>
        /// <param name="operationSuccess">Indicates whether operation was executed with success</param>
        /// <param name="message">Result message</param>
        public OperationResult(bool operationSuccess, string message)
        {
            this.Success = operationSuccess;
            this.Message = message;
            this.Data = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult" /> class.
        /// </summary>
        /// <param name="operationSuccess">Indicates whether operation was executed with success</param>
        /// <param name="message">Result message</param>
        /// <param name="data">Object returned by operation</param>
        public OperationResult(bool operationSuccess, string message, object data)
        {
            this.Success = operationSuccess;
            this.Message = message;
            this.Data = data;
        }

        /// <summary> Gets or sets a value indicating whether operation was success </summary>
        public bool Success { get; set; }

        /// <summary> Gets or sets the message of the operation </summary>
        public string Message { get; set; }

        /// <summary> Gets or sets the data object of the operation </summary>
        public object Data { get; set; }
    }
}
