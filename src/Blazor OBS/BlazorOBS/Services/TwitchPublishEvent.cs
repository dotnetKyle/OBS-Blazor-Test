using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorOBS.Services
{
    public enum PublishEventType
    {
        NewFollow,
        MessageDeleted
    }
    public class TwitchPublishEvent
    {
        public TwitchPublishEvent(PublishEventType type)
        {
            Type = type;
        }
        
        public PublishEventType Type { get; private set; }

        public string FollowedChannelId { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
    }
}
