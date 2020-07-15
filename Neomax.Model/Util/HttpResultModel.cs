////-----------------------------------------------------------------------
//// <copyright file="HttpResultModel.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Model.Util
{
    /// <summary>
    /// Http Result model
    /// </summary>
    public class HttpResultModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResultModel" /> class.
        /// </summary>
        /// <param name="message">Result message</param>
        public HttpResultModel(string message)
        {
            this.ResultMessage = message;
            this.ResultData = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResultModel" /> class.
        /// </summary>
        /// <param name="message">Result message</param>
        /// <param name="data">Object returned by operation</param>
        public HttpResultModel(string message, object data)
        {
            this.ResultMessage = message;
            this.ResultData = data;
        }
        
        /// <summary> Gets or sets the message of the Http Result </summary>
        public string ResultMessage { get; set; }

        /// <summary> Gets or sets the data object of the Http Result </summary>
        public object ResultData { get; set; }
    }
}
