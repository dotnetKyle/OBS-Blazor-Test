﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;
using System.IdentityModel.Tokens.Jwt;

namespace BlazorOBS.Services
{

    public class BlazorOBSTwitchClient
    {
        TwitchClient _client;

        public string Username { get; private set; }

        public static BlazorOBSTwitchClient Create(BlazorOBSUserSettings userSettings)
        {
            var blazorObsTwitchClient = new BlazorOBSTwitchClient();

            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = null;
            if (jwtHandler.CanReadToken(userSettings.IdToken))
            {

                token = jwtHandler.ReadJwtToken(userSettings.IdToken);

                var prefUsernameClaim = token.Claims.FirstOrDefault(c => c.Type == "preferred_username");

                blazorObsTwitchClient.Username = prefUsernameClaim.Value;
            }

            if (blazorObsTwitchClient.Username == null)
                throw new Exception("Username is null");

            var credentials = new ConnectionCredentials(blazorObsTwitchClient.Username, userSettings.AccessToken);
            var clientOptions = new TwitchLib.Communication.Models.ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            var webSocketClient = new TwitchLib.Communication.Clients.WebSocketClient(clientOptions);
            var client = new TwitchClient(webSocketClient);
            client.Initialize(credentials, blazorObsTwitchClient.Username);

            client.OnLog += blazorObsTwitchClient.Client_OnLog;
            client.OnMessageReceived += blazorObsTwitchClient.Client_OnMessageReceived;
            //client.OnChatCommandReceived += blazorObsTwitchClient.Client_OnChatCommandReceived;
            client.OnUserJoined += blazorObsTwitchClient.Client_OnUserJoined;
            client.OnUserLeft += blazorObsTwitchClient.Client_OnUserLeft;
            client.OnExistingUsersDetected += blazorObsTwitchClient.Client_OnExistingUsersDetected;
            client.OnHostingStarted += blazorObsTwitchClient.Client_OnHostingStarted;
            client.OnHostingStopped += blazorObsTwitchClient.Client_OnHostingStopped;
            //client.OnBeingHosted += blazorObsTwitchClient.Client_OnBeingHosted;
            client.OnRaidNotification += blazorObsTwitchClient.Client_OnRaidNotification;
            //client.OnChannelStateChanged += blazorObsTwitchClient.Client_OnChannelStateChanged;
            //client.OnChatCleared += blazorObsTwitchClient.Client_OnChatCleared;
            client.OnIncorrectLogin += blazorObsTwitchClient.Client_OnIncorrectLogin;
            client.OnNoPermissionError += blazorObsTwitchClient.Client_OnNoPermissionError;
            client.OnError += blazorObsTwitchClient.Client_OnError;
            client.OnUnaccountedFor += blazorObsTwitchClient.Client_OnUnaccountedFor;


            client.Connect();

            blazorObsTwitchClient._client = client;
            return blazorObsTwitchClient;
        }

        private void Client_OnUnaccountedFor(object sender, OnUnaccountedForArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Unaccounted For Event: {e.Location} - Raw: {e.RawIRC}");
        }

        private void Client_OnError(object sender, TwitchLib.Communication.Events.OnErrorEventArgs e)
        {
            OnChatEvent?.Invoke(new TwitchChatEvent(TwitchChatEventType.LogError, e.Exception.ToString()));
        }

        private void Client_OnNoPermissionError(object sender, EventArgs e)
        {
            OnChatEvent?.Invoke(new TwitchChatEvent(TwitchChatEventType.LogError, "No Permission Error"));
        }

        private void Client_OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            OnChatEvent?.Invoke(new TwitchChatEvent(TwitchChatEventType.LogError, 
                e.Exception.ToString()));
        }

        public Action<TwitchChatEvent> OnChatEvent { get; set; }

        private void Client_OnRaidNotification(object sender, OnRaidNotificationArgs e)
        {
            OnChatEvent?.Invoke(new TwitchChatEvent(TwitchChatEventType.RaidStarted, 
                $"RAID! DisplayName:{e.RaidNotification.DisplayName}, Login:{e.RaidNotification.Login}, ViewerCount:{e.RaidNotification.MsgParamViewerCount}"));
        }

        private void Client_OnUserLeft(object sender, OnUserLeftArgs e)
        {
            var chatEvent = new TwitchChatEvent(TwitchChatEventType.UserLeft);
            chatEvent.Username = e.Username;
            chatEvent.Channel = e.Channel;

            OnChatEvent?.Invoke(chatEvent);
        }

        private void Client_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            var chatEvent = new TwitchChatEvent(TwitchChatEventType.UserJoined);
            chatEvent.Username = e.Username;
            chatEvent.Channel = e.Channel;

            OnChatEvent?.Invoke(chatEvent);
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var chatEvent = new TwitchChatEvent(TwitchChatEventType.MessageRecieved, e.ChatMessage.Message);
            chatEvent.Username = e.ChatMessage.Username;

            OnChatEvent?.Invoke(chatEvent);
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Username:{e.BotUsername}: {e.Data}");
        }

        private void Client_OnHostingStopped(object sender, OnHostingStoppedArgs e)
        {
            OnChatEvent?.Invoke(new TwitchChatEvent(TwitchChatEventType.HostingStopped, "Hosting Stopped."));
        }
        private void Client_OnHostingStarted(object sender, OnHostingStartedArgs e)
        {
            OnChatEvent?.Invoke(new TwitchChatEvent(TwitchChatEventType.HostingStarted, 
                $"{e.HostingStarted.HostingChannel} is hosting {e.HostingStarted.TargetChannel} with {e.HostingStarted.Viewers} viewers."));
        }

        private void Client_OnExistingUsersDetected(object sender, OnExistingUsersDetectedArgs e)
        {
            var users = e.Users;
            var channel = e.Channel;
        }

        private void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            var chatEvent = new TwitchChatEvent(TwitchChatEventType.CommandRecieved, e.Command.CommandText);
            chatEvent.Username = e.Command.ChatMessage.Username;

            OnChatEvent?.Invoke(chatEvent);
        }

        private void Client_OnChatCleared(object sender, OnChatClearedArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_OnChannelStateChanged(object sender, OnChannelStateChangedArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_OnBeingHosted(object sender, OnBeingHostedArgs e)
        {
            var hostedBy = e.BeingHostedNotification.HostedByChannel;
            var isAutoHost = e.BeingHostedNotification.IsAutoHosted;
            var viewers = e.BeingHostedNotification.Viewers;

            throw new NotImplementedException();
        }

        public void GetFollowers()
        {
        }
    }
}
