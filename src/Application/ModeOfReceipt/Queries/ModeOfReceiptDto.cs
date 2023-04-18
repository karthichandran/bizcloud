using AutoMapper;
using ReProServices.Application.Common.Mappings;
using ReProServices.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ReProServices.Application.ModeOfReceipt.Queries
{
    public class ModeOfReceiptDto : IMapFrom<Domain.Entities.ModeOfReceipt>
    {
        [Key]
        public int ModeOfReceiptID { get; set; }
        public string ModeOfReceiptText { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.ModeOfReceipt, ModeOfReceiptDto>();
        }
    }
}
