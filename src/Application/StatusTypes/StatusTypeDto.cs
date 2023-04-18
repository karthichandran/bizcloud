using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ReProServices.Application.Common.Mappings;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.StatusTypes
{
    public class StatusTypeDto : IMapFrom<StatusType>
    {
        [Key]
        public int StatusTypeID { get; set; }
        public string Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<StatusType, StatusTypeDto>();
        }
    }
}
