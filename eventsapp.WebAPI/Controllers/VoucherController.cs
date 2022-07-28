using eventsapp.WebAPI.Models;
using Eventsapp.Services;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Athlete;
using EventsApp.Domain.DTOs.Core;
using EventsApp.Domain.DTOs.ReturnObjects;
using EventsApp.Domain.DTOs.Subscription;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace eventsapp.WebAPI.Controllers
{
    //[Authorize] 
    [RoutePrefix("api/ClaimedVoucher")]
    public class VoucherController : ApiController
    {
        private readonly IVoucherService _VoucherService;
        private readonly IKeyedRepository<ClaimedVoucher, int> ClaimedVoucherRepository;

        public VoucherController(IVoucherService VoucherService, IKeyedRepository<ClaimedVoucher, int> _ClaimedVoucherRepository)
        {
            _VoucherService = VoucherService;
            ClaimedVoucherRepository = _ClaimedVoucherRepository;
        }

        [HttpGet]
        [Route("ResetVouchersCount")]
        public async Task<IHttpActionResult> ResetVouchersCount(bool IsReset)
        {
            try
            {
                var data = await this._VoucherService.ResetVouchersCount(IsReset);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("GetClaimedVouchers")]
        public async Task<IHttpActionResult> GetClaimedVouchers(bool IsASuperAdmin)
        {
            try
            {
                var dataList = await this._VoucherService.GetClaimedVouchers(IsASuperAdmin);
                return Ok(dataList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("GetClaimedVouchers")]
        public async Task<IHttpActionResult> GetClaimedVouchers(ReportsRequestFilter request)
        {
            try
            {
                IEnumerable<ClaimedVoucherDto> ClaimedVouchers =
                    await _VoucherService.GetPagedClaimedVoucher(request.FilterParams, request.Page, request.PageSize,
                    (request.SortDirection == "asc"), request.Searchtext, request.SortBy);

                var pagedRecord = new PagedList
                {
                    TotalRecords = await _VoucherService.GetNumbersOfRecords(request.FilterParams, request.Searchtext),
                    Content = ClaimedVouchers.ToList(),
                    CurrentPage = request.Page,
                    PageSize = request.PageSize,
                    TotalCount = await this.ClaimedVoucherRepository.GetCountAsync(x => x.Id > 0)
                };

                return Ok(pagedRecord);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}