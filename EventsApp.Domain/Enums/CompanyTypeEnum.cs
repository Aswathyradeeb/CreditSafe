namespace EventsApp.Domain.Enums
{
    public enum CompanyTypeEnum
    {
        Sponser = 1,
        Exhibitor = 2,
        Partner = 3
    }
    public enum EventTypeEnum
    {
        Seminar = 1,
        Meeting = 2,
        Associations = 3,
        AwardCeremony = 4,
        Banquet = 5,
        BrandSale = 6,
        Concert = 7,
        Conference = 8,
        Convention = 9,
        Exhibition = 10,
        FashionShow = 11,
        Graduation = 12,
        ProductLaunch = 13,
        Sports = 14,
        Theatre = 15,
        Wedding = 16,
        Workshop = 17
    }
    public enum PersonTypeEnum
    {
        Speaker = 1,
        VIP = 2
    }
    public enum RegistrationTypeEnum
    {
        Athlete = 1,     
        Guest = 3,
        Restaurant = 2,
        Company = 4,
        Official = 5,
        Partner = 6,
        Exhibitor=8,
        Sponsor=9
    }
    public enum SponsorTypeEnum
    {
        Gold = 1,
        Silver = 2,
        Platenium = 3
    }

    public enum UserActionsEnum
    {
        SendSMS = 1,
        SendEmail = 2,
        LockUser = 3,
        UnlockUser = 4
    }
    public enum RolesEnum
    {
        SuperAdministrator = 1,
        User = 2,
        Administrator = 3,
    }
    public enum PaymentStatusEnum
    {
        Pending = 1,
        InProgress = 2,
        Paid = 3
    }

    public enum EventPackageEnum
    {
        Hachling = 1,
        Baby = 2,
        Business = 3,
        BusinessPro = 4,
        Flexible = 5
    }

    public enum ParticipantsRegistrationTypeEnum
    {
        EventBased = 1,
        AgendaBased = 2
    }
}
