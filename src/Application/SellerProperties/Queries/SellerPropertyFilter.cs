namespace ReProServices.Application.SellerProperties.Queries
{
    public class SellerPropertyFilter
    {
        public string AddressPremises { get; set; }
        public string PAN { get; set; }
        public int PropertyId { get; set; } = 0;
        public string SellerName { get; set; }
    }
}
