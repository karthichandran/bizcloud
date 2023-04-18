using AutoMapper;
using ReProServices.Application.Common.Mappings;
using ReProServices.Domain.Entities;
namespace ReProServices.Application.NatureOfPayments.Queries
{
    public class NatureOfPaymentDto : IMapFrom<NatureOfPayment>
    {
        public int NatureOfPaymentID { get; set; }
        public string NatureOfPaymentText { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap< NatureOfPayment, NatureOfPaymentDto>();
        }
    }
}
