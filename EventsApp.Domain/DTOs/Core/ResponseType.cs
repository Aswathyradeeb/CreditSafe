using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.Core
{
    public class ResponseType<TType>
    {
        public TType Data { get; set; }

        public string Message { get; set; }

        public bool Successed { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

        public dynamic ModelState { get; set; }

        public string MessageAr { get; set; }
        public string ErrorMessageAr { get; set; }
        public static ResponseType<T> PerformSuccessed<T>(dynamic data)
        {
            return new ResponseType<T>
            {
                Successed = true,
                Data = data,
            };
        }
        public static ResponseType<T> PerformError<T>(string errorCode, string errorMessage, string errorMessageAr = null)
        {
            return new ResponseType<T>
            {
                Successed = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode,
                ErrorMessageAr = errorMessageAr
            };
        }
    }
}
