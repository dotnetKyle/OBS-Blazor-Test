using System;

namespace BlazorOBS.Services
{
    public class TwitchChatEvent
    {
        public TwitchChatEvent(TwitchChatEventType type, string msg)
        {
            Type = type;
            Message = msg;
            DTGUtc = DateTime.UtcNow;
        }
        public TwitchChatEventType Type { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public DateTime DTGUtc { get; set; }
    }
}
