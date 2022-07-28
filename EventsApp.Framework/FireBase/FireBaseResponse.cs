using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework.FireBase
{
    class FireBaseResponse
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="FireBaseResponse"/> class  
        /// </summary>  
        public FireBaseResponse()
        {
        }

        /// <summary>  
        /// Initializes a new instance of the <see cref="FireBaseResponse"/> class  
        /// </summary>  
        /// <param name="success">If Success</param>  
        /// <param name="errorMessage">Error Message</param>  
        /// <param name="httpResponse">HTTP Response</param>  
        /// <param name="jsonContent">JSON Content</param>  
        public FireBaseResponse(bool success, string errorMessage, HttpResponseMessage httpResponse = null, string jsonContent = null)
        {
            this.Success = success;
            this.JSONContent = jsonContent;
            this.ErrorMessage = errorMessage;
            this.HttpResponse = httpResponse;
        }

        /// <summary>  
        /// Gets or sets Boolean status of Success  
        /// </summary>  
        public bool Success { get; set; }

        /// <summary>  
        /// Gets or sets JSON content returned by the Request  
        /// </summary>  
        public string JSONContent { get; set; }

        /// <summary>  
        /// Gets or sets Error Message if Any  
        /// </summary>  
        public string ErrorMessage { get; set; }

        /// <summary>  
        /// Gets or sets full Http Response  
        /// </summary>  
        public HttpResponseMessage HttpResponse { get; set; }
    }
}
