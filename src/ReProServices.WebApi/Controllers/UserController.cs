using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.User;
using ReProServices.Application.User.Command;
using ReProServices.Application.User.Queries;

namespace WebApi.Controllers
{
    //[Authorize]
    public class UserController : ApiController
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<List<UserDto>> GetUsers([FromQuery] UserFilter userFilter)
        {
            var result = await Mediator.Send(new GetUsersQuery {Filter=userFilter });
            return result;
        }
       
        [HttpGet("UserByLoginName/{loginName}")]
        public async Task<UserDto> GetByLoginName(string loginName)
        {
            var result = await Mediator.Send(new GetUserByLoginNameQuery { LoginName = loginName });
            return result;
        }

        [HttpGet("UserProfile/{loginName}")]
        public async Task<UserProfileDto> GetProfileByLoginName(string loginName)
        {
            var result = await Mediator.Send(new GetUserProfileQuery { LoginName = loginName });
            return result;
        }

        [HttpGet("UserById/{userID}")]
        public async Task<UserVM> GetById(int userID)
        {
            var result = await Mediator.Send(new GetUserByIdQuery { UserID = userID });
            return result;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateUserCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }
        [Authorize(Roles = "Admin")]
        [HttpPut()]
        public async Task<ActionResult<int>> Update(UpdateUserCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }
    }
}
