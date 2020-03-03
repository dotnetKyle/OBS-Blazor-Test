using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorOBS
{
    public class UserState
    {
        public event EventHandler<HighlightedMessage> OnHighlightMessage;

        public void ShowHighlightedMessage(object sender, HighlightedMessage highlightedMessage)
        {
            var list = OnHighlightMessage.GetInvocationList();

            foreach(var func in list)
            {
                func.DynamicInvoke(sender, highlightedMessage);
            }

            //OnHighlightMessage?.Invoke(sender, highlightedMessage);
        }
    }

    public class HighlightedMessage
    {
        public string Username { get; set; }
        public string Message { get; set; }
    }
}
