using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.Segment;
using ReProServices.Application.Segment.Command;
using ReProServices.Application.Segment.Queries;

namespace WebApi.Controllers
{
    [Authorize]
    public class RolePermissionsController : ApiController
    {
        [HttpGet]
        public async Task<List<SegmentPermissionsDto>> GetSegment()
        {
            var result = await Mediator.Send(new GetSegmentQuery { });
            return result;
        }

        [HttpGet("RoleBy/{roleID}")]
        public async Task<List<SegmentPermissionsDto>> GetSegment(int roleID)
        {
            var result = await Mediator.Send(new GetSegmentByRoleIDQuery { RoleID=roleID});
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateRolePermissionsCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        [HttpPut()]
        public async Task<ActionResult<int>> Update(UpdateRolePermissionsCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }
    }
}
