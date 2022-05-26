﻿namespace Core.Mappers {
    using AutoMapper;
    using DataAccess.EFCore.iPolicyModels;
    using DataAccess.EFCore.RvpSystemModels;
    using Services.Models;
    using Services.ViewModels;


    public class ServiceDataMapperProfile : Profile {
        public ServiceDataMapperProfile() {
            CreateMap<User, DirectPolicyKyc>();
            CreateMap<EdocDetailViewModel, EDocDetail>();
        }
    }
}
