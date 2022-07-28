using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace eventsapp.WebAPI.Results
{
    public class CustomHttpResult : IHttpActionResult
    {
        MemoryStream memoryStream;
        string FileName;
        HttpRequestMessage httpRequestMessage;
        HttpResponseMessage httpResponseMessage;
        public CustomHttpResult(MemoryStream _memoryStream, HttpRequestMessage _httpRequestMessage, string _FileName)
        {
            memoryStream = _memoryStream;
            httpRequestMessage = _httpRequestMessage;
            FileName = _FileName;
        }
        public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(memoryStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
        }
    }
}