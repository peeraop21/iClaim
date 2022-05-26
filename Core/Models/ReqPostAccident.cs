

namespace Core.Models
{

    public class ReqPostAccident
    {
        public AccidentInput AccidentInput { get; set; }
        public AccidentCarInput AccidentCarInput { get; set; }
        public AccidentVictimInput AccidentVictimInput { get; set; }
    }

    public class AccidentInput
    {
        public string AccDate { get; set; }
        public string AccTime { get; set; }
        public string AccProv { get; set; }
        public string AccDist { get; set; }
        public string AccSubDist { get; set; }
        public string AccPlace { get; set; }
        public string AccDetail { get; set; }
        public string AccBranchId { get; set; }

    }
    public class AccidentCarInput
    {
        public string AccCarPolicyNo { get; set; }
        public string AccCarLicense { get; set; }
        public string AccCarLicenseProv { get; set; }
        public string AccCarTankNo { get; set; }
        public string AccCarEngineSize { get; set; }
        public string AccCarTypeCarId { get; set; }
        public bool AccCarIsRedCarLicense { get; set; }
        public string AccCarDrivePrefixName { get; set; }
        public string AccCarDriveFirstname { get; set; }
        public string AccCarDriveLastname { get; set; }
        public string AccCarDriveTelNo { get; set; }
        public string AccCarDriveProtectStartDate { get; set; }
        public string AccCarDriveProtectStartTime { get; set; }
        public string AccCarDriveProtectEndDate { get; set; }
        public string AccCarDriveProtectEndTime { get; set; }
    }
    public class AccidentVictimInput
    {
        public string AccVicCurrentHomeId { get; set; }
        public string AccVicCurrentMoo { get; set; }
        public string AccVicCurrentSoi { get; set; }
        public string AccVicCurrentRoad { get; set; }
        public string AccVicCurrentProv { get; set; }
        public string AccVicCurrentDist { get; set; }
        public string AccVicCurrentSubDist { get; set; }
        public string AccVicCaseType { get; set; }
        public string AccVicBrokenDetail { get; set; }
        public string AccVicUserLineId { get; set; }
        public string AccVicIdCardNo { get; set; }
        public string AccVicPrefixName { get; set; }
        public string AccVicFirstname { get; set; }
        public string AccVicLastname { get; set; }
        public string AccVicTelNo { get; set; }
        public string AccVicDateOfBirth { get; set; }
    }
}
