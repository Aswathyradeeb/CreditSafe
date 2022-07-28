using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Lookups;
using EventsApp.Domain.Entities;
using EventsApp.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace sponsersapp.WebAPI.Controllers
{
    [RoutePrefix("api/Lookup")]
    public class LookupController : ApiController
    {
        private readonly IKeyedRepository<PaymentStatus, int> _paymentStatusRepository;
        private readonly IKeyedRepository<Language, int> _languageRepository;
        private readonly IKeyedRepository<EventPackage, int> _eventPackageRepository;
        private readonly IKeyedRepository<BackgroundTheme, int> _bgRepository;
        private readonly IKeyedRepository<ParticipantsRegistrationType, int> _ParticipantsRegistrationTypeRepository;

        public LookupController(IKeyedRepository<PaymentStatus, int> paymentStatusRepository,
            IKeyedRepository<EventPackage, int> eventPackageRepository, IKeyedRepository<BackgroundTheme, int> bgRepository,
            IKeyedRepository<ParticipantsRegistrationType, int> ParticipantsRegistrationTypeRepository,
            IKeyedRepository<Language, int> _languageRepository)
        {
            this._paymentStatusRepository = paymentStatusRepository;
            this._eventPackageRepository = eventPackageRepository;
            this._bgRepository = bgRepository;
            this._languageRepository = _languageRepository;
            this._ParticipantsRegistrationTypeRepository = ParticipantsRegistrationTypeRepository;
        }

        [Route("GetLanguages")]
        public async Task<IHttpActionResult> GetLanguages()
        {
            try
            {
                var data = await this._languageRepository.GetAllAsync();
                return Ok(MapperHelper.Map<List<LanguageDto>>(data));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("GetPaymentStatus")]
        public async Task<IHttpActionResult> GetPaymentStatus()
        {
            try
            {
                var _paymentStatuses = await this._paymentStatusRepository.GetAllAsync();
                var _paymentStatusesDtos = MapperHelper.Map<List<LookupDto>>(_paymentStatuses);
                return Ok(_paymentStatusesDtos);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("GetEventPackages")]
        public async Task<IHttpActionResult> GetEventPackages()
        {
            try
            {
                var eventPackages = await this._eventPackageRepository.GetAllAsync();
                var eventPackagesDtos = MapperHelper.Map<List<EventPackageDto>>(eventPackages);
                return Ok(eventPackagesDtos);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("GetBackgrounds")]
        public async Task<IHttpActionResult> GetBackgrounds()
        {
            try
            {
                var bgLayouts = await this._bgRepository.GetAllAsync();
                var bgLayoutDTO = MapperHelper.Map<List<BackgroundThemeDTO>>(bgLayouts);
                return Ok(bgLayoutDTO);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("GetBackgroundById/{id}")]
        public async Task<IHttpActionResult> GetBackgroundById(int id)
        {
            try
            {
                var bgLayouts = await this._bgRepository.QueryAsync(x => x.Id == id);
                if (bgLayouts.Count() > 0)
                    return Ok(bgLayouts.FirstOrDefault().URL);
                else
                    return null;
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("GetParticipantsRegistrationType")]
        public async Task<IHttpActionResult> GetParticipantsRegistrationType()
        {
            try
            {
                var data = await this._ParticipantsRegistrationTypeRepository.GetAllAsync();
                return Ok(MapperHelper.Map<List<ParticipantsRegistrationTypeDto>>(data));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}