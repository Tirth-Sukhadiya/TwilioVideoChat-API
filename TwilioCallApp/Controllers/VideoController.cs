using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TwilioCallApp.Services.Abstractions;
using TwilioCallApp.Shared.ViewModels;
using TwilioCallApp.Shared.ViewModels.VideoViewModels;

namespace TwilioCallApp.Controllers
{
    [ApiController, Route("api/video")]
    public class VideoController : Controller
    {
        readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
            => _videoService = videoService;

        
        [HttpGet("token")]
        public IActionResult GetToken()
        {
            return new JsonResult(new { token = _videoService.GetTwilioJwt(User.Identity.Name) });
        }


        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms()
            => new JsonResult(await _videoService.GetAllRoomsAsync());


        [HttpGet("room")]
        public IActionResult GetRoom()
        {
            try
            {
                var response = _videoService.GetRoomToJoin("owner");
                return Ok(response.data);
            }
            catch (System.Exception)
            {
                return StatusCode(500, new ErrorViewModel(1001, StatusCodes.Status500InternalServerError, "Server error."));
            }
        }

        [HttpGet("room/{roomid}")]
        public IActionResult GetRoom(string roomid)
        {
            try
            {
                var response = _videoService.JoinRoom(roomid, "user");
                return Ok(response.data);
            }
            catch (System.Exception)
            {
                return StatusCode(500, new ErrorViewModel(1001, StatusCodes.Status500InternalServerError, "Server error."));
            }
        }

        public IActionResult WebHook(WebhookViewModel requestdata)
        {
            try
            {
                if (requestdata.ParticipantSid == "owner" && requestdata.StatusCallbackEvent == "participant-disconnected")
                {
                    var response = _videoService.CompleteVideoRoom(requestdata.RoomSid);
                    return Ok(new { status = "success" });
                }
                else
                {
                    return Ok(new { status = "success" });
                }
            }
            catch (System.Exception)
            {
                return StatusCode(500, new ErrorViewModel(1001, StatusCodes.Status500InternalServerError, "Server error."));
            }
        }
    }
}
