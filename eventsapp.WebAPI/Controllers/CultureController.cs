using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace eventsapp.WebAPI.Controllers
{
    [RoutePrefix("Culture")]
    public class CultureController : ApiController
    {
        public CultureController()
        {
        }

        [Route("SetCulture")]
        public IHttpActionResult SetCulture(SaveModel model)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(model.value);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(model.value);

            return Ok();
        }

    }

    public class SaveModel
    {
        public string value { get; set; }
    }

}
