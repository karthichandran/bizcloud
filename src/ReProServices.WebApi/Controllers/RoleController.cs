using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.Roles;
using ReProServices.Application.Roles.Command;
using ReProServices.Application.Roles.Queries;

namespace WebApi.Controllers
{
    [Authorize]
    public class RoleController :  ApiController
    {
        [HttpGet]
        public async Task<List<RolesDto>> GetRoles()
        {
            var result = await Mediator.Send(new GetRoleListQuery { });
            return result;
        }

        [HttpGet("RoleById/{roleId}")]
        public async Task<RolesDto> GetRoleById(int roleId)
        {
            var result = await Mediator.Send(new GetRoleByIdQuery { RoleID=roleId });
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateRoleCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        [HttpPut()]
        public async Task<ActionResult<int>> Update(UpdateRoleCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }
    }
}
