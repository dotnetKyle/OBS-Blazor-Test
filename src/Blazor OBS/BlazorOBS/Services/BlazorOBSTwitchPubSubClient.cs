using TwitchLib.PubSub;
using TwitchLib;
using TwitchLib.PubSub.Models;
using TwitchLib.PubSub.Events;
using System;

namespace BlazorOBS.Services
{
    public class BlazorOBSTwitchPubSubClient
    {
        TwitchPubSub _pubSubClient;
        BlazorOBSUserSettings _userSettings;

        public static BlazorOBSTwitchPubSubClient Create(BlazorOBSUserSettings userSettings)
        {
            var client = new BlazorOBSTwitchPubSubClient();

            client._userSettings = userSettings;

            client._pubSubClient = new TwitchPubSub();

            client._pubSubClient.OnPubSubServiceConnected += client._pubSubClient_OnPubSubServiceConnected;
            client._pubSubClient.OnListenResponse += client._pubSubClient_OnListenResponse;

            client._pubSubClient.OnFollow += client._pubSubClient_OnFollow;
            client._pubSubClient.OnHost += client._pubSubClient_OnHost;
            client._pubSubClient.OnMessageDeleted += client._pubSubClient_OnMessageDeleted;
            client._pubSubClient.OnPubSubServiceError += client._pubSubClient_OnPubSubServiceError;

            client._pubSubClient.ListenToFollows("dotnetKyle");
            client._pubSubClient.ListenToVideoPlayback("dotnetKyle");
            client._pubSubClient.ListenToChatModeratorActions("dotnetKyle", "dotnetKyle");
            client._pubSubClient.Connect();

            return client;
        }

        private void _pubSubClient_OnListenResponse(object sender, OnListenResponseArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Response from topic:" + e.Topic);
            System.Diagnostics.Debug.WriteLine("Nonce: " + e.Response.Nonce);

            if(!e.Successful)
            {
                System.Diagnostics.Debug.WriteLine("Successful: " + e.Response.Successful);
                System.Diagnostics.Debug.WriteLine("Error: " + e.Response.Error);
            }
        }

        private void _pubSubClient_OnPubSubServiceError(object sender, OnPubSubServiceErrorArgs e)
        {
            System.Diagnostics.Debug.WriteLine("PubSub Service Error:");
            System.Diagnostics.Debug.WriteLine(e.Exception.ToString());
        }

        private void _pubSubClient_OnMessageDeleted(object sender, OnMessageDeletedArgs e)
        {
            var pubEvent = new TwitchPublishEvent(PublishEventType.MessageDeleted)
            {
                Message = e.Message,
                Username = e.TargetUser
            };

            OnPublishEvent?.Invoke(pubEvent);
        }

        private void _pubSubClient_OnHost(object sender, OnHostArgs e)
        {
            
        }

        private void _pubSubClient_OnPubSubServiceConnected(object sender, EventArgs e)
        {
            _pubSubClient.SendTopics(_userSettings.AccessToken);
        }

        public Action<TwitchPublishEvent> OnPublishEvent { get; set; }

        private void _pubSubClient_OnFollow(object sender, OnFollowArgs e)
        {
            var pubEvent = new TwitchPublishEvent(PublishEventType.NewFollow)
            {
                FollowedChannelId = e.FollowedChannelId,
                DisplayName = e.DisplayName,
                Username = e.Username,
                UserId = e.UserId
            };

            OnPublishEvent?.Invoke(pubEvent);
        }
    }
}
