﻿namespace YunusExpress_MVC.ViewModels
{
    public class ReceiverUpdateVm
    {
        public int Id { get; set; }

        public string ClientCode { get; set; }
        public string ReceiverName { get; set; }
        public string? ReceiverAddress { get; set; }
        public string? ReceiverPhoneNum { get; set; }
        public int? ReceiverDiscount { get; set; }
        public DateTime ContractDate { get; set; }
        public bool IsEDV { get; set; } = false;

        public string? BankName { get; set; }
        public string? BankCode { get; set; }
        public string? BankVoen { get; set; }
        public string? Swift { get; set; }
        public string? Voen { get; set; }
        public string? Iban { get; set; }
        public string? Mh { get; set; }
        public bool QiymetVar { get; set; }
    }
}
