namespace BlazorOBS.Services
{
    public enum TwitchChatEventType
    {
        Log,
        LogError,
        UserJoined,
        UserLeft,
        HostingStarted,
        HostingStopped,
        RaidStarted,
        MessageRecieved,
        CommandRecieved,
    }
}
