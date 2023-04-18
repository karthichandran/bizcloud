using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ReProServices.Application.Common.Mappings;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.User
{
   public class UserDto:IMapFrom<Domain.Entities.Users>
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string UserPassword { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public int? GenderID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IsActive { get; set; }
        public bool? IsAgent { get; set; }
        public string ISD { get; set; }
        public bool IsAdmin { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Users, UserDto>();
        }
    }
}
