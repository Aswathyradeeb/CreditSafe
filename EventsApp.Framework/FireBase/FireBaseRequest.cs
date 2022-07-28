using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework.FireBase
{
    class FireBaseRequest
    {
        private const string JSON_SUFFIX = ".json";

        /// <summary>  
        /// Initializes a new instance of the <see cref="FirebaseRequest"/> class  
        /// </summary>  
        /// <param name="method">HTTP Method</param>  
        /// <param name="uri">URI of resource</param>  
        /// <param name="jsonString">JSON string</param>  
        public FirebaseRequest(HttpMethod method, string uri, string jsonString = null)
        {
            this.Method = method;
            this.JSON = jsonString;
            if (uri.Replace("/", string.Empty).EndsWith("firebaseio.com"))
            {
                this.Uri = uri + '/' + JSON_SUFFIX;
            }
            else
            {
                this.Uri = uri + JSON_SUFFIX;
            }
        }

        /// <summary>  
        /// Gets or sets HTTP Method  
        /// </summary>  
        private HttpMethod Method { get; set; }

        /// <summary>  
        /// Gets or sets JSON string  
        /// </summary>  
        private string JSON { get; set; }

        /// <summary>  
        /// Gets or sets URI  
        /// </summary>  
        private string Uri { get; set; }

        /// <summary>  
        /// Executes a HTTP requests  
        /// </summary>  
        /// <returns>Firebase Response</returns>  
        public FireBaseResponse Execute()
        {
            Uri requestURI;
            if (FireBaseUtility.ValidateURI(this.Uri))
            {
                requestURI = new Uri(this.Uri);
            }
            else
            {
                return new FireBaseResponse(false, "Proided Firebase path is not a valid HTTP/S URL");
            }

            string json = null;
            if (this.JSON != null)
            {
                if (!FireBaseUtility.TryParseJSON(this.JSON, out json))
                {
                    return new FireBaseResponse(false, string.Format("Invalid JSON : {0}", json));
                }
            }

            var response = FireBaseUtility.RequestHelper(this.Method, requestURI, json);
            response.Wait();
            var result = response.Result;

            var FireBaseResponse = new FireBaseResponse()
            {
                HttpResponse = result,
                ErrorMessage = result.StatusCode.ToString() + " : " + result.ReasonPhrase,
                Success = response.Result.IsSuccessStatusCode
            };

            if (this.Method.Equals(HttpMethod.Get))
            {
                var content = result.Content.ReadAsStringAsync();
                content.Wait();
                FireBaseResponse.JSONContent = content.Result;
            }

            return FireBaseResponse;
        }
    }
}
