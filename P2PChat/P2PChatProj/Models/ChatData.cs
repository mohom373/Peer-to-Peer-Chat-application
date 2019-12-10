using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Models
{
    public class ChatData
    {
        #region Properties
        public string SearchString
        {
            get
            {
                return RemoteUser.UserName + " " + Date;
            }
        }

        public User LocalUser { get; set; }

        public User RemoteUser { get; set; }

        public List<ChatMessage> UserMessages { get; set; }

        public List<ChatMessage> RemoteMessages { get; set; }

        public string Date { get; set; }

        #endregion

        public ChatData(User localUser, User remoteUser, List<ChatMessage> userMessages, 
                        List<ChatMessage> remoteMessages, string date)
        {
            LocalUser = localUser;
            RemoteUser = remoteUser;
            UserMessages = userMessages;
            RemoteMessages = remoteMessages;
            Date = date;
        }
        
    }
}
