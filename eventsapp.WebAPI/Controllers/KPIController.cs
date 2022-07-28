using System;
using System.Threading.Tasks;
using System.Web.Http; 
using System.Web.Http.Description;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Net.Http.Headers;
using Eventsapp.Services;

namespace eventsapp.WebAPI.Controllers
{
    [Authorize]  
    [RoutePrefix("api/KPI")]
    public class KPIController : ApiController
    {
        private IKPIService kpiService;

        public KPIController(IKPIService _kpiService)
        {
            kpiService = _kpiService;
        } 

        [HttpPost]
        [Route("GetRegisteredUsers")]
        public async Task<IHttpActionResult> GetRegisteredUsers(int eventId)
        {
            try
            {
                //var kpiDtos = await this.kpiService.GetRegisteredUsers(eventId);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("GetSurveyResult")]
        public async Task<IHttpActionResult> GetSurveyResult(int eventId)
        {
            try
            {
                //var kpiDtos = await this.kpiService.GetSurveyResult(eventId);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("GetUsersAttendance")]
        public async Task<IHttpActionResult> GetUsersAttendance(int eventId)
        {
            try
            {
                //var kpiDtos = await this.kpiService.GetUsersAttendance(eventId);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("ExportExcel")]
        public async Task<HttpResponseMessage> ExportExcel(List<GraphDTO> ExcelExportModel)
        {
            try
            {
                HttpResponseMessage result = null;
                // serve the file to the client      
                result = Request.CreateResponse(HttpStatusCode.OK);
                //byte[] array = await this.kpiService.ExportExcel(ExcelExportModel);
                //MemoryStream mem = new MemoryStream(array);
                //result.Content = new StreamContent(mem);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
