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
        public TwitchChatEvent(TwitchChatEventType type)
        {
            Type = type;
            DTGUtc = DateTime.UtcNow;
        }
        public TwitchChatEventType Type { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public string Channel { get; set; }
        public string Command { get; set; }
        public int Viewers { get; set; }
        public DateTime DTGUtc { get; set; }
    }
}
