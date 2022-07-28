using Eventsapp.Repositories;
using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace eventsapp.WebAPI.Controllers
{
    [RoutePrefix("api/Payment")]
    public class PaymentController : ApiController
    {
        private readonly IPaymentService _PaymentService;

        public PaymentController(IPaymentService _PaymentService)
        {
            this._PaymentService = _PaymentService;
        }
        [HttpGet]
        [Route("MakePayment")]
        public async Task<IHttpActionResult> MakePayment(int subscriptionId)
        {
            try
            {
                var responseURL = await this._PaymentService.MakePayment(subscriptionId);
                return Ok(responseURL);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("ReceivePayment")] 
        public async Task<HttpResponseMessage> ReceivePayment(string paymentRef)
        {
            try
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Moved);
                var id = await _PaymentService.ReceivePayment(paymentRef);
                response.Headers.Location =  new System.Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/#/app/viewPayment/" + id);
                return response; 

            }
            catch (Exception ex)
            { 
                return null;
            }
        }

        [HttpGet]
        [Route("GetTransaction")]
        public async Task<IHttpActionResult> GetTransaction(int transactionId)
        {
            try
            {
                var returnObj = await _PaymentService.GetTransaction(transactionId);
                return Ok(returnObj);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        

    }
}