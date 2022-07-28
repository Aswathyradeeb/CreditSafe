using eventsapp.WebAPI.Models;
using Eventsapp.Services.Services.Interface;
using EventsApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace eventsapp.WebAPI
{
    [Authorize]
    [RoutePrefix("api/Athlete")]
    public class AthleteController : ApiController
    {
        private IAthleteService athleteService;
        private ICurrentUser user;
      
        public AthleteController(IAthleteService athleteService, ICurrentUser user)
        {
            this.athleteService = athleteService;
            this.user = user;
        }

        [Route("GetAthletes")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAthletes(RequestFilter request)
        {
            try
            {
                var returnValue = await this.athleteService.GetAllAthletes(request.FilterParams, request.Page, request.PageSize,request.Searchtext);
                var pagedRecord = new PagedList
                {
                    TotalRecords = returnValue.athletesCount,
                    Content = returnValue.athletes,
                    CurrentPage = request.Page,
                    PageSize = request.PageSize
                };
                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [Route("GetGuests")]
        [HttpPost]
        public async Task<IHttpActionResult> GetGuests(RequestFilter request)
        {
            try
            {
                var returnValue = await this.athleteService.GetGuests(request.FilterParams, request.Page, request.PageSize, request.Searchtext);
                var pagedRecord = new PagedList
                {
                    TotalRecords = returnValue.athletesCount,
                    Content = returnValue.athletes,
                    CurrentPage = request.Page,
                    PageSize = request.PageSize
                };
                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [Route("GetGuestsOfAthlete")]
        [HttpPost]
        public async Task<IHttpActionResult> GetGuestsOfAthlete(RequestFilter request)
        {
            try
            {
                var returnValue = await this.athleteService.GetGuestsOfAthlete(request.FilterParams, request.Page, request.PageSize,Convert.ToInt32(request.Searchtext), request.SortBy);
                var pagedRecord = new PagedList
                {
                    TotalRecords = returnValue.athletesCount,
                    Content = returnValue.athletes,
                    CurrentPage = request.Page,
                    PageSize = request.PageSize
                };
                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [Route("GetAthleteVoucher")]
         [HttpPost]
        public async Task<IHttpActionResult> GetAthleteVoucher(RequestFilter request)
        {
            try
            {
                var returnValue = await this.athleteService.GetAthleteVoucher(request.FilterParams, request.Page, request.PageSize);
                var pagedRecord = new PagedList
                {
                    TotalRecords = returnValue.voucherCount,
                    Content = returnValue.voucher,
                    CurrentPage = request.Page,
                    PageSize = request.PageSize
                };
                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        [Route("GetAllVouchers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllVouchers()
        {
            try
            {
                var _vouchersTypes = await this.athleteService.GetAllVouchers();
               return Ok(_vouchersTypes);
            }
            catch
            {
                return InternalServerError();
            }
        }

        
        [Route("GetVoucherShareCount")]
        [HttpGet]
        public async Task<IHttpActionResult> GetVoucherShareCount(int voucherId)
        {
            try
            {
                var _vouchersTypes = await this.athleteService.GetVoucherShareCount(voucherId);
                return Ok(_vouchersTypes);
            }
            catch
            {
                return InternalServerError();
            }
        }

        
        [Route("GetAllAthletesGuest")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAthletesGuest()
        {
            try
            {
                var _guests =  this.athleteService.GetAllAthletesGuest();
                return Ok(_guests);
            }
            catch
            {
                return InternalServerError();
            }
        }

        
        [Route("ShareGuestVoucher")]
        [HttpGet]
        public async Task<IHttpActionResult> ShareGuestVoucher(int guestId,int voucherId)
        {
            try
            {
                var _guests = this.athleteService.ShareGuestVoucher(guestId, voucherId);
                return Ok(_guests);
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}
