using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;

namespace Prism.Module1.Messages
{
    public class ShowUserListViewMessage
    {
        public ShowUserListViewMessage()
        {
            Reload = false;
        }
        public bool Reload { get; set; }
    }

    public class ShowUserListViewEvent : CompositePresentationEvent<ShowUserListViewMessage>{}
}
