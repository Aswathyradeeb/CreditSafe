using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web;
using System.Data.Entity.Validation;
using Eventsapp.Services;
using System.Threading.Tasks;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Eventsapp.Repositories;
using eventsapp.WebAPI.Models;

namespace agendasapp.WebAPI.Controllers
{
    // [Authorize]
    [RoutePrefix("api/Email")]
    public class EmailController : ApiController
    {
        private IEmailService emailService;

        public EmailController(IEmailService _emailService)
        {
            this.emailService = _emailService;
        }

        [Route("SendContactUsEmail")]
        [HttpPost]
        public async Task<IHttpActionResult> SendContactUsEmail(ContactUsDto ContactUsModel)
        {
            try
            {
                var data = await emailService.SendContactUsEmail(ContactUsModel);
                return Ok("Contact Us Email Sent");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [Route("SendContactUsSuccesResponseEmail")]
        [HttpPost]
        public async Task<IHttpActionResult> SendContactUsSuccesResponseEmail(ContactUsDto ContactUsModel)
        {
            try
            {
                var data = await emailService.SendContactUsSuccesResponseEmail(ContactUsModel);
                return Ok("Contact Us Email Response Sent");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}