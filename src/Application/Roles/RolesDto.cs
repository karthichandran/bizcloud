using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ReProServices.Application.Common.Mappings;

namespace ReProServices.Application.Roles
{
    public class RolesDto : IMapFrom<Domain.Entities.Roles>
    {
        public int RoleID { get; set; }
        public string Name { get; set; }
        public string ReportingTo { get; set; }
        public bool IsOrganizationRole { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Roles, RolesDto>();
        }
    }
}
