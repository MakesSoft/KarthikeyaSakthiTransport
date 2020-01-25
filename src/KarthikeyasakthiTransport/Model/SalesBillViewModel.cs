namespace KarthikeyasakthiTransport.Model
{
    public class SalesBillViewModel : SalesBill
    {
        public string ConsignorName { get; set; }
        public string ConsignorGst { get; set; }
        public string ConsignorAddress { get; set; }
        public string ConsignorPhone { get; set; }

        public string ConsigneeName { get; set; }
        public string ConsigneeGst { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ConsigneePhone { get; set; }

        public string ValueInText { get; set; }
    }
}