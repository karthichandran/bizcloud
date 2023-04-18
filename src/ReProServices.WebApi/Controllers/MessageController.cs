using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.Message.Commands;
using ReProServices.Application.Message;
using ReProServices.Application.Message.Queries;

namespace WebApi.Controllers
{
    public class MessageController :  ApiController
    {        

        [HttpPost("lane/1")]
        public async Task<MessageDto> AddMessage(CreateMessageCommand command)
        {
            command.laneNo = 1;
            return await Mediator.Send(command);
        }
        [HttpPost("lane/2")]
        public async Task<MessageDto> AddMessageLane2(CreateMessageCommand command)
        {
            command.laneNo = 2;
            return await Mediator.Send(command);
        }
        [HttpPost("lane/3")]
        public async Task<MessageDto> AddMessageLane3(CreateMessageCommand command)
        {
            command.laneNo = 3;
            return await Mediator.Send(command);
        }
        [HttpPost("lane/4")]
        public async Task<MessageDto> AddMessageLane4(CreateMessageCommand command)
        {
            command.laneNo = 4;
            return await Mediator.Send(command);
        }
        [HttpPost("lane/5")]
        public async Task<MessageDto> AddMessageLane5(CreateMessageCommand command)
        {
            command.laneNo = 5;
            return await Mediator.Send(command);
        }
        [HttpPost("lane/6")]
        public async Task<MessageDto> AddMessageLane6(CreateMessageCommand command)
        {
            command.laneNo = 6;
            return await Mediator.Send(command);
        }

        [HttpGet("{lane}")]
        public async Task<MessageDto> GetMEssahe(int lane)
        {           
            return await Mediator.Send(new GetMessageByLaneQuery { laneNo=lane});
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await Mediator.Send(new DeleteMessageCommand { laneNo = id });
        }
    }
}
