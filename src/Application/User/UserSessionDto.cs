using AutoMapper;
using ReProServices.Application.Common.Mappings;
using ReProServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.User
{
   public class UserSessionDto : IMapFrom<UserSession>
    {
        public int SessionID { get; set; }
        public int UserID { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserSession, UserSessionDto>();
        }
    }
}
