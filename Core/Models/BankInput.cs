

namespace Core.Models
{

    public class BankInput
    {
        public string accountName { get; set; }
        public string accountNumber { get; set; }
        public string accountBankName { get; set; }
        public string bankId { get; set; }
        public string bankBase64String { get; set; }
        public string bankFilename { get; set; }
        public bool isEditBankImage { get; set; }
    }
}
