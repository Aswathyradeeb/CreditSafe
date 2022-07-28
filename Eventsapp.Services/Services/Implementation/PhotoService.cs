using Eventsapp.Repositories;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace Eventsapp.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository photoRepository;

        public PhotoService(IPhotoRepository _photoRepository)
        {
            this.photoRepository = _photoRepository;
        }


        public List<PhotoDto> CreatePhoto(List<PhotoDto> _photosDto)
        {
            foreach (PhotoDto photo in _photosDto)
            {
                if (!string.IsNullOrEmpty(photo.PhotoName))
                {
                    String path = HttpContext.Current.Server.MapPath("/Uploads/Attachment/"); //Path

                    //Check if directory exist
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                    }

                    photo.PhotoName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";

                    //set the image path
                    string imgPath = Path.Combine(path, photo.PhotoName);

                    byte[] imageBytes = Convert.FromBase64String(photo.PhotoName);

                    File.WriteAllBytes(imgPath, imageBytes);
                    photo.PhotoName = "";
                }

                Photo photoMap = MapperHelper.Map<Photo>(photo);
                photoMap.Event = null;
                photoMap.Approved = true; 
                this.photoRepository.Insert(photoMap);

            }
            return _photosDto;
        }

        public async Task<List<PhotoDto>> GetAllPhotos()
        {
            var photos = await this.photoRepository.GetAllAsync();
            var photosDtos = MapperHelper.Map<List<PhotoDto>>(photos); 
            return photosDtos;
        }

        public async Task<List<PhotoReverseDto>> GetPhotosByEventId(int eventId)
        {
            var photos = await this.photoRepository.QueryAsync(x => x.EventId == eventId); 
            var photosDtos = MapperHelper.Map<List<PhotoReverseDto>>(photos); 
            return photosDtos;
        }


    }
}
