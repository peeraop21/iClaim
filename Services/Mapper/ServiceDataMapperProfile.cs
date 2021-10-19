namespace Core.Mappers {
    using AutoMapper;
    using DataAccess.EFCore.iPolicyModels;
    using DataAccess.EFCore.RvpSystemModels;
    using Services.ViewModels;


    public class ServiceDataMapperProfile : Profile {
        public ServiceDataMapperProfile() {
            CreateMap<DirectPolicyKycViewModel, DirectPolicyKyc>();
            CreateMap<EdocDetailViewModel, EDocDetail>();
        }
    }
}
