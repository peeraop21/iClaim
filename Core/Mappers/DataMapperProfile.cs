namespace Core.Mappers {
    using AutoMapper;
    using Core.Controllers;
    using DataAccess.EFCore.DigitalClaimModels;
    using Services.ViewModels;


    public class DataMapperProfile : Profile {
        public DataMapperProfile() {           
            CreateMap<vwApproval, HosApproval>();
            CreateMap<vwBankData, InputBankViewModel>();
            CreateMap<vwVictim, VictimtViewModel>();
        }
    }
}
