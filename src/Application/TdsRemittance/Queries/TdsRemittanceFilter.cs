namespace ReProServices.Application.TdsRemittance.Queries
{
    public class TdsRemittanceFilter
    {
        public int UnitNo { get; set; }
        public int FromUnitNo { get; set; }
        public int ToUnitNo { get; set; }
        public string PropertyPremises { get; set; }
        public int LotNo { get; set; }
        public string CustomerName { get; set; }
        public int? RemittanceStatusID { get; set; }
        public int TdsAmount { get; set; }
    }
}
