using System.Collections.Generic;

namespace TwilioCallApp.Shared.ViewModels
{
    public class RoomDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string token { get; set; }
        public int ParticipantCount { get; set; }
        public int MaxParticipants { get; set; }
    }
    public class RoomList
    {
        public List<RoomDetails> list { get; set; }
        public RoomList()
        {
            list = new List<RoomDetails>();
        }
    }
}
