﻿namespace Core.Mappers {
    using AutoMapper;
    using Core.Controllers;
    using DataAccess.EFCore.DigitalClaimModels;
    using Services.ViewModels;
    using Core.ViewModels;
    using System;
    using System.Globalization;
    using DataAccess.EFCore.RvpOfficeModels;
    using Core.Models;
    using Services.Models;

    public class DataMapperProfile : Profile {
        public DataMapperProfile() {           
            CreateMap<ApprovalViewModel, IclaimApproval>()
                .ForMember(m => m.SumReqMoney, opt => opt.MapFrom(src => src.SumMoney))
                .ForMember(m => m.CureMoney, opt => opt.MapFrom(src => src.SumMoney));
            CreateMap<BankViewModel, InputBankViewModel>();
            CreateMap<VictimViewModel, VictimtViewModel>();
            CreateMap<BillViewModel, CheckDuplicateInvoiceViewModel>()
                .ForMember(m => m.AccNo, opt => opt.MapFrom(src => src.accNo))
                .ForMember(m => m.VictimNo, opt => opt.MapFrom(src => src.victimNo))
                .ForMember(m => m.ReqNo, opt => opt.MapFrom(src => src.reqNo))
                .ForMember(m => m.BillId, opt => opt.MapFrom(src => src.billNo))
                .ForMember(m => m.BookNo, opt => opt.MapFrom(src => src.bookNo))
                .ForMember(m => m.ReceiptNo, opt => opt.MapFrom(src => src.bill_no))
                .ForMember(m => m.HosId, opt => opt.MapFrom(src => src.selectHospitalId))
                .ForMember(m => m.IsCansel, opt => opt.MapFrom(src => src.isCancel));
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
            CreateMap<ECM, DocumentDetail>()
                .ForMember(m => m.SystemId, opt => opt.MapFrom(src => src.SystemId))
                .ForMember(m => m.TemplateId, opt => opt.MapFrom(src => src.TemplateId))
                .ForMember(m => m.DocumentId, opt => opt.MapFrom(src => src.DocID))
                .ForMember(m => m.RefId, opt => opt.MapFrom(src => src.RefNo))
                .ForMember(m => m.CreateDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(m => m.StatusDoc, opt => opt.MapFrom(src => "A"));
            CreateMap<BillViewModel, UpdateInvoiceViewModel>();
            CreateMap<BankViewModel, UpdateBankViewModel>();
            CreateMap<ReqEkyc, EkycReqBody>()
                .ForMember(m => m.IdentityImage, opt => opt.MapFrom(src => src.IdCardBase64))
                .ForMember(m => m.FaceImage, opt => opt.MapFrom(src => src.FaceBase64));
            CreateMap<AccidentInput, HosAccident>()
                .ForMember(m => m.DateRec, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(m => m.TimeRec, opt => opt.MapFrom(src => DateTime.Now.ToString("HH:mm")))
                .ForMember(m => m.DateAcc, opt => opt.MapFrom(src => DateTime.ParseExact(src.AccDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                .ForMember(m => m.TimeAcc, opt => opt.MapFrom(src => src.AccTime))
                .ForMember(m => m.AccPlace, opt => opt.MapFrom(src => src.AccPlace))
                .ForMember(m => m.AccProv, opt => opt.MapFrom(src => src.AccProv))
                .ForMember(m => m.AccType, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.AccTypeCase, opt => opt.MapFrom(src => "1"))
                .ForMember(m => m.AccNature, opt => opt.MapFrom(src => src.AccDetail))
                .ForMember(m => m.Status, opt => opt.MapFrom(src => "V"))
                .ForMember(m => m.LastUpdate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(m => m.SendStatus, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.HospitalId, opt => opt.MapFrom(src => "VTM" + src.AccBranchId))
                .ForMember(m => m.Regisdate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(m => m.AccDist, opt => opt.MapFrom(src => src.AccDist))
                .ForMember(m => m.AccSubDist, opt => opt.MapFrom(src => src.AccSubDist))
                .ForMember(m => m.Survey, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.Claim30Day, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.ChangeCarLicense, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.ClaimDuplicate, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.ChangeVictim, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.ChangeSumCase, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.ChangeAccident, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.OtherCase, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.GetRecordStatus, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.BranchId, opt => opt.MapFrom(src => src.AccBranchId))
                .ForMember(m => m.InputBy, opt => opt.MapFrom(src => "C"))
                .ForMember(m => m.FalseInform, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.Surveyed, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.OutSurveyed, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.InsureCover, opt => opt.MapFrom(src => "I"));
            CreateMap<AccidentCarInput, HosCarAccident>()
                .ForMember(m => m.TpNo, opt => opt.MapFrom(src => 1))
                .ForMember(m => m.CushiNo, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.AccCarTankNo) ? src.AccCarTankNo : null))
                .ForMember(m => m.CarLicense, opt => opt.MapFrom(src => src.AccCarLicense))
                .ForMember(m => m.CarProv, opt => opt.MapFrom(src => src.AccCarLicenseProv))
                .ForMember(m => m.DrvPrefix, opt => opt.MapFrom(src => src.AccCarDrivePrefixName))
                .ForMember(m => m.DrvFname, opt => opt.MapFrom(src => src.AccCarDriveFirstname))
                .ForMember(m => m.DrvLname, opt => opt.MapFrom(src => src.AccCarDriveLastname))
                .ForMember(m => m.DrvTelNo, opt => opt.MapFrom(src => src.AccCarDriveTelNo))
                .ForMember(m => m.CarType, opt => opt.MapFrom(src => "0"))
                .ForMember(m => m.LastUpdate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(m => m.SendStatus, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.Regisdate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(m => m.StatusCanCelCar, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.SearchStatus, opt => opt.MapFrom(src => "1"))
                .ForMember(m => m.FoundPolicyNo, opt => opt.MapFrom(src => src.AccCarPolicyNo))
                .ForMember(m => m.FoundEffDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.AccCarDriveProtectStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(m => m.FoundExpDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.AccCarDriveProtectEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(m => m.FoundInsId, opt => opt.MapFrom(src => "RVP"))
                .ForMember(m => m.InputBy, opt => opt.MapFrom(src => "C"))
                .ForMember(m => m.FoundCarLicense, opt => opt.MapFrom(src => src.AccCarLicense))
                .ForMember(m => m.FoundCarProv, opt => opt.MapFrom(src => src.AccCarLicenseProv))
                .ForMember(m => m.FoundChassisNo, opt => opt.MapFrom(src => src.AccCarTankNo))
                .ForMember(m => m.GetRecordStatus, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.FoundEnginesize, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.AccCarEngineSize) ? src.AccCarEngineSize : null))
                .ForMember(m => m.FoundCarType, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.AccCarTypeCarId) ? src.AccCarTypeCarId : null))
                .ForMember(m => m.TypeMotor, opt => opt.MapFrom(src => "1.30"))
                .ForMember(m => m.NeedToBillOic, opt => opt.MapFrom(src => "N"));
            CreateMap<AccidentVictimInput, HosVicTimAccident>()
                .ForMember(m => m.VictimNo, opt => opt.MapFrom(src => 1))
                .ForMember(m => m.Prefix, opt => opt.MapFrom(src => src.AccVicPrefixName))
                .ForMember(m => m.Fname, opt => opt.MapFrom(src => src.AccVicFirstname))
                .ForMember(m => m.Lname, opt => opt.MapFrom(src => src.AccVicLastname))
                .ForMember(m => m.DrvSocNo, opt => opt.MapFrom(src => src.AccVicIdCardNo))
                .ForMember(m => m.HomeId, opt => opt.MapFrom(src => src.AccVicCurrentHomeId))
                .ForMember(m => m.Moo, opt => opt.MapFrom(src => src.AccVicCurrentMoo))
                .ForMember(m => m.Soi, opt => opt.MapFrom(src => src.AccVicCurrentSoi))
                .ForMember(m => m.Road, opt => opt.MapFrom(src => src.AccVicCurrentRoad))
                .ForMember(m => m.Tumbol, opt => opt.MapFrom(src => src.AccVicCurrentSubDist))
                .ForMember(m => m.District, opt => opt.MapFrom(src => src.AccVicCurrentDist))
                .ForMember(m => m.Province, opt => opt.MapFrom(src => src.AccVicCurrentProv))
                .ForMember(m => m.TelNo, opt => opt.MapFrom(src => src.AccVicTelNo))
                .ForMember(m => m.VicTimIs, opt => opt.MapFrom(src => "ผขป"))
                .ForMember(m => m.VictimType, opt => opt.MapFrom(src => src.AccVicCaseType))
                .ForMember(m => m.CarNo, opt => opt.MapFrom(src => 0))
                .ForMember(m => m.Broken, opt => opt.MapFrom(src => 1))
                .ForMember(m => m.DetailBroken, opt => opt.MapFrom(src => src.AccVicBrokenDetail))
                .ForMember(m => m.SendStatus, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.LastUpdate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(m => m.Regisdate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(m => m.StatusCanCelVtm, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.InputBy, opt => opt.MapFrom(src => "C"))
                .ForMember(m => m.GetRecordStatus, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(m => m.UserId, opt => opt.MapFrom(src => src.AccVicUserLineId))
                .ForMember(m => m.VCf014, opt => opt.MapFrom(src => "Y"))
                .ForMember(m => m.InsurerPaid, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.GetMoneyCarOwner, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.TypeCard, opt => opt.MapFrom(src => "C"))
                .ForMember(m => m.RiskAlgohol, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.RiskMobile, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.RiskSafetyBelt, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.RiskHelmet, opt => opt.MapFrom(src => "N"))
                .ForMember(m => m.InsureType, opt => opt.MapFrom(src => "I"))
                .ForMember(m => m.BirthDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.AccVicDateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(m => m.NationalityId, opt => opt.MapFrom(src => 159))
                .ForMember(m => m.AlterHomeId, opt => opt.MapFrom(src => src.AccVicCurrentHomeId))
                .ForMember(m => m.AlterMoo, opt => opt.MapFrom(src => src.AccVicCurrentMoo))
                .ForMember(m => m.AlterSoi, opt => opt.MapFrom(src => src.AccVicCurrentSoi))
                .ForMember(m => m.AlterRoad, opt => opt.MapFrom(src => src.AccVicCurrentRoad))
                .ForMember(m => m.AlterTumbol, opt => opt.MapFrom(src => src.AccVicCurrentSubDist))
                .ForMember(m => m.AlterDistrict, opt => opt.MapFrom(src => src.AccVicCurrentDist))
                .ForMember(m => m.AlterProvince, opt => opt.MapFrom(src => src.AccVicCurrentProv))
                ;


        }
    }
}
