using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Domain.DTOs.Core
{
    public interface IResponseType<TType>
    {
        TType Data { get; set; }

        string Message { get; set; }

        bool Successed { get; set; }

        string ErrorMessage { get; set; }

        string ErrorCode { get; set; }

    }
}
