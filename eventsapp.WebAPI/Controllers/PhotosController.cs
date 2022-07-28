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
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using Eventsapp.Services;
using Eventsapp.Repositories;
using System.Threading.Tasks;

namespace eventsapp.WebAPI.Controllers
{
    [RoutePrefix("api/Photo")]
    public class PhotosController : ApiController
    {
        private IPhotoService photoService;
        private readonly IPhotoRepository photoRepository;

        public PhotosController(IPhotoService photoService, IPhotoRepository _photoRepository)
        {
            this.photoService = photoService;
            this.photoRepository = _photoRepository;
        }

        // GET: api/Photos
        [Route("GetPhotos")]
        public async Task<IHttpActionResult> GetPhotos()
        {
            try
            {
                var photoDtos = await photoService.GetAllPhotos();

                return Ok(photoDtos);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("GetPhotosByEventId")]
        public async Task<IHttpActionResult> GetPhotosByEventId(int eventId)
        {
            try
            {
                List<PhotoReverseDto> photoList = await photoService.GetPhotosByEventId(eventId);

                return Ok(photoList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // POST: api/Photos
        [Route("CreatePhoto")]
        [HttpPost]
        public IHttpActionResult PostPhoto(List<PhotoDto> _photosDto)
        {


            try
            {
                List<PhotoDto> photoList = photoService.CreatePhoto(_photosDto);
                return Ok(photoList);
            }
            catch
            {
                return InternalServerError();
            }

        }
    }
}