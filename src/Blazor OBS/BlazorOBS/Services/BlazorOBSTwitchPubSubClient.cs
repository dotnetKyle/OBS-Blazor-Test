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
            client._pubSubClient.OnFollow += client._pubSubClient_OnFollow;


            client._pubSubClient.Connect();


            return client;
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
