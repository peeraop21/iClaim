namespace Core.Mappers {
    using AutoMapper;
    using Core.Controllers;
    using DataAccess.EFCore.DigitalClaimModels;
    using Services.ViewModels;
    using Core.ViewModels;
    using System;
    using System.Globalization;

    public class DataMapperProfile : Profile {
        public DataMapperProfile() {           
            CreateMap<ApprovalViewModel, IclaimApproval>();
            CreateMap<BankViewModel, InputBankViewModel>();
            CreateMap<VictimViewModel, VictimtViewModel>();
            CreateMap<BillViewModel, Invoicehd>()
                .ForMember(m => m.Takendate, opt => opt.MapFrom(src => DateTime.ParseExact(src.hospitalized_date, "yyyy-MM-dd", CultureInfo.InvariantCulture) ))
                .ForMember(m => m.Takentime, opt => opt.MapFrom(src => src.hospitalized_time.Replace(":",".")))
                .ForMember(m => m.Dispensedate, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.out_hospital_date) ? DateTime.ParseExact(src.hospitalized_date, "yyyy-MM-dd", CultureInfo.InvariantCulture): DateTime.ParseExact(src.out_hospital_date, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                .ForMember(m => m.Dispensetime, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.out_hospital_time) ? "00.00" : src.out_hospital_time.Replace(":", ".")))
                .ForMember(m => m.ReceiptNo, opt => opt.MapFrom(src => src.bill_no))
                .ForMember(m => m.VictimType, opt => opt.MapFrom(src => src.typePatient))
                .ForMember(m => m.Hosid, opt => opt.MapFrom(src => src.selectHospitalId))
                .ForMember(m => m.Mainconsider, opt => opt.MapFrom(src => src.injuriId.ToString()))
                .ForMember(m => m.BookNo, opt => opt.MapFrom(src => src.bookNo))
                .ForMember(m => m.Suminv, opt => opt.MapFrom(src => double.Parse(src.money)));
            CreateMap<ECMViewModel, EdocDetailViewModel>()
                .ForMember(m => m.SystemId, opt => opt.MapFrom(src => src.SystemId))
                .ForMember(m => m.TemplateId, opt => opt.MapFrom(src => src.TemplateId))
                .ForMember(m => m.DocumentId, opt => opt.MapFrom(src => src.DocID))
                .ForMember(m => m.RefId, opt => opt.MapFrom(src => src.RefNo))
                .ForMember(m => m.CreateDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(m => m.StatusDoc, opt => opt.MapFrom(src => "A"));
            CreateMap<BillViewModel, UpdateInvoiceViewModel>();
            CreateMap<BankViewModel, UpdateBankViewModel>();

        }
    }
}
