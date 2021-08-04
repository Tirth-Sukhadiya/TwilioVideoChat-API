using System.Collections.Generic;
using System.Threading.Tasks;
using TwilioCallApp.Shared.ViewModels;

namespace TwilioCallApp.Services.Abstractions
{
    public interface IVideoService
    {
        string GetTwilioJwt(string identity);
        BaseViewModel GetRoomToJoin(string identity);
        Task<IEnumerable<RoomDetails>> GetAllRoomsAsync();
        BaseViewModel JoinRoom(string RoomSid, string identity);
        BaseViewModel CompleteVideoRoom(string RoomSid);
    }
}
