using AutoMapper;
using ReProServices.Application.Common.Mappings;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.Sellers
{
    public class SellerDto : IMapFrom<Seller>
	{
		public int SellerID { get; set; }
		public string SellerName { get; set; }
		public string AddressPremises { get; set; }
		public string AdressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string City { get; set; }
		public string PinCode { get; set; }
		public string EmailID { get; set; }
		public string PAN { get; set; }
		public string MobileNo { get; set; }
		public int StateID { get; set; }
		public bool IsResident { get; set; }
		public void Mapping(Profile profile)
		{
			profile.CreateMap<Seller, SellerDto>();
		}
		//todo refer https://stackoverflow.com/questions/45275811/automapper-to-map-child-tables
	}
}
