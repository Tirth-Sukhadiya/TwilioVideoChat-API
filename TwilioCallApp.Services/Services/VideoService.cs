using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwilioCallApp.Services.Abstractions;
using TwilioCallApp.Services.Options;
using TwilioCallApp.Shared.ViewModels;
using Twilio;
using Twilio.Base;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Insights.V1.Room;
using ParticipantStatus = Twilio.Rest.Video.V1.Room.ParticipantResource.StatusEnum;

namespace TwilioCallApp.Services.Services
{
    public class VideoService : IVideoService
    {
        BaseViewModel response;
        public VideoService()
        {
            response = new BaseViewModel();
            TwilioClient.Init(TwilioSettings.ApiKey, TwilioSettings.ApiSecret);
        }

        public string GetTwilioJwt(string identity)
            => new Token(TwilioSettings.AccountSid,
                         TwilioSettings.ApiKey,
                         TwilioSettings.ApiSecret,
                         identity ?? Guid.NewGuid().ToString(),
                         grants: new HashSet<IGrant> { new VideoGrant() }).ToJwt();

        public BaseViewModel GetRoomToJoin(string identity)
        {
            RoomDetails room = new RoomDetails();
            try
            {
                var roomResource = RoomResource.Create(new CreateRoomOptions { UniqueName = Guid.NewGuid().ToString(), MaxParticipants = 2, Type = RoomResource.RoomTypeEnum.PeerToPeer });
                room.Id = roomResource.Sid;
                room.Name = roomResource.UniqueName;
                room.MaxParticipants = roomResource.MaxParticipants ?? 0;
                room.token = GetTwilioJwt(identity);
                var roomList = new RoomList();
                roomList.list.Add(room);
                response.data = roomList;
                response.success = 1;
            }
            catch (Exception)
            {
                response.success = -1;
            }
            return response;
        }

        public async Task<IEnumerable<RoomDetails>> GetAllRoomsAsync()
        {
            var rooms = await RoomResource.ReadAsync();
            var tasks = rooms.Select(
                room => GetRoomDetailsAsync(
                    room,
                    Twilio.Rest.Video.V1.Room.ParticipantResource.ReadAsync(
                        room.Sid,
                        Twilio.Rest.Video.V1.Room.ParticipantResource.StatusEnum.Connected)));

            return await Task.WhenAll(tasks);

            static async Task<RoomDetails> GetRoomDetailsAsync(
                RoomResource room,
                Task<ResourceSet<Twilio.Rest.Video.V1.Room.ParticipantResource>> participantTask)
            {
                var participants = await participantTask;
                var roomDetails = new RoomDetails();
                roomDetails.Id = room.Sid;
                roomDetails.Name = room.UniqueName;
                roomDetails.ParticipantCount = participants.Count();
                roomDetails.MaxParticipants = 2;
                return roomDetails;
            }
        }


        public BaseViewModel JoinRoom(string RoomSid, string identity)
        {
            try
            {
                RoomDetails room = new RoomDetails();
                var roomResource = RoomResource.Fetch((RoomSid ?? string.Empty));
                if (roomResource != null && roomResource.Status == RoomResource.RoomStatusEnum.InProgress)
                {
                    bool InProgress = false;
                    var participants = ParticipantResource.Read(RoomSid);
                    foreach (var participant in participants)
                    {
                        if (participant.ParticipantIdentity.Contains("owner"))
                            InProgress = true;
                    }
                    if (InProgress)
                    {
                        room.Id = roomResource.Sid;
                        room.Name = roomResource.UniqueName;
                        room.MaxParticipants = roomResource.MaxParticipants ?? 0;
                        room.token = GetTwilioJwt(identity);
                        response.data = room;
                        response.success = 1;
                    }
                    else
                    {
                        response.success = -2;
                    }
                }
                else
                {
                    response.success = -2;
                }
            }
            catch (Exception)
            {
                response.success = -1;
            }
            return response;
        }

        public BaseViewModel CompleteVideoRoom(string RoomSid)
        {
            try
            {
                RoomResource.Update(pathSid: RoomSid, status: RoomResource.RoomStatusEnum.Completed);
                response.success = 1;
            }
            catch (Exception)
            {
                response.success = -1;
            }
            return response;
        }
    }
}
