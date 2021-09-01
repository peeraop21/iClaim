namespace Core.Mappers {
    using AutoMapper;
    using Core.Controllers;
    using DataAccess.EFCore.DigitalClaimModels;
    using Services.ViewModels;
    using System;
    using System.Linq;

    public class DataMapperProfile : Profile {
        public DataMapperProfile() {
            CreateMap<vwApproval, HosApproval>();
            CreateMap<vwBankData, InputBankViewModel>();
        }
    }
}
