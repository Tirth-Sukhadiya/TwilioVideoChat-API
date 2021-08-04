using System;

namespace TwilioCallApp.Shared.ViewModels.VideoViewModels
{
    public class WebhookViewModel
    {
        public string MediaUri { get; set; }
        public string RoomSid { get; set; }
        public string RoomName { get; set; }
        public string SourceSid { get; set; }
        public string ParticipantIdentity { get; set; }
        public long Size { get; set; }
        public string RecordingSid { get; set; }
        public double Duration { get; set; }
        public string StatusCallbackEvent { get; set; }
        public DateTime Timestamp { get; set; }
        public string AccountSid { get; set; }
        public string RecordingUri { get; set; }
        public string Codec { get; set; }
        public string Container { get; set; }
        public string TrackName { get; set; }
        public string ParticipantSid { get; set; }
    }
}
