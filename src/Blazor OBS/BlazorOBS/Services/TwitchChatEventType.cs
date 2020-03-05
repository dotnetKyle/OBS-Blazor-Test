namespace BlazorOBS.Services
{
    public enum TwitchChatEventType
    {
        LogError,
        UserJoined,
        UserLeft,
        HostingStarted,
        HostingStopped,
        RaidStarted,
        MessageRecieved,
        CommandRecieved,
        PublishNotification
    }
}
