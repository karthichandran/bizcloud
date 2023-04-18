namespace ReProServices.Application.SellerProperties
{
    public class SellerPropertyVM
    {
        public int PropertyId { get; set; }

        public string AddressPremises { get; set; }
        
        public string SellerNames { get; set; }

        public string PanNumbers { get; set; }

        public string PropertyShortName { get; set; }

        public bool? IsActive { get; set; }
    }
}
