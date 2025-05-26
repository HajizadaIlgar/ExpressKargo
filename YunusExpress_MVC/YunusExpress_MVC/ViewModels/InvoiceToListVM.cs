namespace YunusExpress_MVC.ViewModels
{
    public class InvoiceToListVM
    {
        public int Id { get; set; }
        public string ClientCode { get; set; }
        public string ReceiverName { get; set; }
        public string? ReceiverAddress { get; set; }
        public string? ReceiverPhoneNum { get; set; }
        public DateTime ContractDate { get; set; }
        public int Orders { get; set; }
        public string? BankName { get; set; }
        public string? BankCode { get; set; }
        public string? BankVoen { get; set; }
        public string? Swift { get; set; }
        public string? Voen { get; set; }
        public string? Iban { get; set; }
        public string? Mh { get; set; }

        public string DaxiliCattirilma { get; set; }
        public string Discount1 { get; set; }
        public string BeynexalqCattirilma { get; set; }
        public string Discount2 { get; set; }
        public string Edv { get; set; }
        public string Total { get; set; }
    }
}
