using Eventsapp.Repositories;
using Eventsapp.Services.Services.Interface;
using EventsApp.Domain.DTOs;
using EventsApp.Domain.DTOs.Athlete;
using EventsApp.Domain.DTOs.ReturnObjects;
using EventsApp.Domain.Entities;
using EventsApp.Domain.Enums;
using EventsApp.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventsapp.Services.Services.Implementation
{
    public class AthleteService : IAthleteService
    {
        private readonly ICurrentUser user;
        private readonly IUserRepository userRepository;
        private readonly IKeyedRepository<Voucher, int> voucherRepository;
        private readonly IKeyedRepository<AthleteVoucher, int> athletevoucherRepository;
        private readonly IKeyedRepository<GuestVoucher, int> guestVoucherRepository;

        public AthleteService(ICurrentUser user, IUserRepository userRepository,
            IKeyedRepository<Voucher, int> voucherRepository,
            IKeyedRepository<AthleteVoucher, int> athletevoucherRepository,
            IKeyedRepository<GuestVoucher, int> guestVoucherRepository)
        {

            this.user = user;
            this.userRepository = userRepository;
            this.voucherRepository = voucherRepository;
            this.athletevoucherRepository = athletevoucherRepository;
            this.guestVoucherRepository = guestVoucherRepository;
        }

        public async Task<ReturnAthleteVoucherDto> GetAthleteVoucher(FilterParams filterParams, int page, int pageSize)
        {

            var voucherIds = this.athletevoucherRepository.Query(x => x.UserId == this.user.UserInfo.Id).ToList().Select(x => x.VoucherId);
            var vouchers = await this.voucherRepository.GetAllAsync();
            var selecteVoucher = vouchers.Where(t => voucherIds.Contains(t.Id));


            var selecteVouchersDtoLst = MapperHelper.Map<List<VoucherDto>>(selecteVoucher.Skip((page - 1) * pageSize)
                   .Take(pageSize).ToList());
            return new ReturnAthleteVoucherDto { voucher = selecteVouchersDtoLst, voucherCount = selecteVouchersDtoLst.Count };

        }

        public async Task<ReturnAthletesDto> GetAllAthletes(FilterParams filterParams, int page, int pageSize, string searchText)
        {
            List<User> athletes = new List<User>();

            athletes = (await this.userRepository.QueryAsync(x => x.IsActive != null && x.CompanyId == null && x.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete || x.RegistrationTypeId == (int?)RegistrationTypeEnum.Official)).OrderByDescending(y => y.CreatedOn).ToList();
            if (!string.IsNullOrEmpty(searchText))
            {
                List<User> filteredAthletes = new List<User>();
                filteredAthletes = athletes.Where(o => o.FirstName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                o.LastName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                o.Email.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                var athleteDtoLst = MapperHelper.Map<List<UserDto>>(filteredAthletes.Skip((page - 1) * pageSize)
                       .Take(pageSize).ToList());

                foreach (var item in athleteDtoLst)
                {
                    if (item.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete)
                    {
                        item.GuestCount = (await this.userRepository.QueryAsync(x => x.GuestOf == item.Id)).Count();
                    }
                }
                return new ReturnAthletesDto { athletes = athleteDtoLst, athletesCount = filteredAthletes.Count };
            }
            else
            {
                var athletesDtoLst = MapperHelper.Map<List<UserDto>>(athletes.Skip((page - 1) * pageSize)
                                 .Take(pageSize).ToList());

                foreach (var item in athletesDtoLst)
                {
                    if (item.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete)
                    {
                        item.GuestCount = (await this.userRepository.QueryAsync(x => x.GuestOf == item.Id)).Count();
                    }
                }
                return new ReturnAthletesDto { athletes = athletesDtoLst, athletesCount = athletes.Count };

            }

        }

        public async Task<ReturnAthletesDto> GetGuests(FilterParams filterParams, int page, int pageSize, string searchText)
        {
            List<User> athletes = new List<User>();

            athletes = (await this.userRepository.QueryAsync(x => x.IsActive != null && x.CompanyId == null && x.RegistrationTypeId == (int?)RegistrationTypeEnum.Guest)).OrderByDescending(y => y.CreatedOn).ToList();
            if (!string.IsNullOrEmpty(searchText))
            {
                List<User> filteredAthletes = new List<User>();
                filteredAthletes = athletes.Where(o => o.FirstName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                o.LastName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                o.Email.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                var athleteDtoLst = MapperHelper.Map<List<UserDto>>(filteredAthletes.Skip((page - 1) * pageSize)
                       .Take(pageSize).ToList());

                foreach (var item in athleteDtoLst)
                {
                    if (item.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete)
                    {
                        item.GuestCount = (await this.userRepository.QueryAsync(x => x.GuestOf == item.Id)).Count();
                    }
                }
                return new ReturnAthletesDto { athletes = athleteDtoLst, athletesCount = filteredAthletes.Count };

            }
            else
            {
                var athletesDtoLst = MapperHelper.Map<List<UserDto>>(athletes.Skip((page - 1) * pageSize)
                                 .Take(pageSize).ToList());

                foreach (var item in athletesDtoLst)
                {
                    if (item.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete)
                    {
                        item.GuestCount = (await this.userRepository.QueryAsync(x => x.GuestOf == item.Id)).Count();
                    }
                }
                return new ReturnAthletesDto { athletes = athletesDtoLst, athletesCount = athletes.Count };

            }

        }

        public async Task<ReturnAthletesDto> GetGuestsOfAthlete(FilterParams filterParams, int page, int pageSize, int athleteId, string searchText)
        {
            List<User> guests = new List<User>();

            guests = (await this.userRepository.QueryAsync(x => x.IsActive != null && x.RegistrationTypeId == (int?)RegistrationTypeEnum.Guest && x.GuestOf == athleteId)).OrderByDescending(y => y.CreatedOn).ToList();
            if (!string.IsNullOrEmpty(searchText))
            {
                List<User> filteredGuests = new List<User>();
                filteredGuests = guests.Where(o => searchText.Contains(o.FirstName) || searchText.Contains(o.LastName) || searchText.Contains(o.Email)).ToList();
                var athleteDtoLst = MapperHelper.Map<List<UserDto>>(filteredGuests.Skip((page - 1) * pageSize)
                                  .Take(pageSize).ToList());
                return new ReturnAthletesDto { athletes = athleteDtoLst, athletesCount = filteredGuests.Count };
            }
            else
            {
                var athletesDtoLst = MapperHelper.Map<List<UserDto>>(guests.Skip((page - 1) * pageSize)
                                 .Take(pageSize).ToList());
                return new ReturnAthletesDto { athletes = athletesDtoLst, athletesCount = guests.Count };
            }


        }

        public async Task<List<VoucherDto>> GetAllVouchers()
        {
            var vouchers = await this.voucherRepository.GetAllAsync();
            var vouchersDtos = MapperHelper.Map<List<VoucherDto>>(vouchers);

            return vouchersDtos;

        }

        public List<UserDto> GetAllAthletesGuest()
        {
            var userIds = this.guestVoucherRepository.Query(x => x.sharedBy == this.user.UserInfo.Id).ToList().Select(x => x.userId);
            var guests = this.userRepository.GetAll().Where(t => userIds.Contains(t.Id));

            return MapperHelper.Map<List<UserDto>>(guests);

        }

        public async Task<int> GetVoucherShareCount(int voucherId)
        {
            var vouchers = await this.guestVoucherRepository.QueryAsync(x => x.sharedBy == this.user.UserInfo.Id && x.voucherId == voucherId);

            return vouchers.Count;

        }

        public async Task<int> ShareGuestVoucher(int guestId, int voucherId)
        {
            var alreadyShared = this.guestVoucherRepository.Query(x => x.userId == guestId && x.voucherId == voucherId).ToList().Count;
            if (alreadyShared == 0)
            {
                GuestVoucher guestEntity = new GuestVoucher();
                guestEntity.userId = guestId;
                guestEntity.voucherId = voucherId;
                guestEntity.sharedBy = this.user.UserInfo.Id;
                this.guestVoucherRepository.Insert(guestEntity);
                this.guestVoucherRepository.Commit();
                return 1;
            }
            else
                return 0;

        }
    }
}
