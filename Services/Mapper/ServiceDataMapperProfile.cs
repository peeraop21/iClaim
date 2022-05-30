namespace Core.Mappers {
    using AutoMapper;
    using DataAccess.EFCore.iPolicyModels;
    using DataAccess.EFCore.RvpSystemModels;
    using Services.Models;


    public class ServiceDataMapperProfile : Profile {
        public ServiceDataMapperProfile() {
            CreateMap<User, DirectPolicyKyc>();
            CreateMap<DocumentDetail, EDocDetail>();
        }
    }
}
