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

        public List<ChatTextMessage> UserMessages { get; set; }

        public List<ChatTextMessage> RemoteMessages { get; set; }

        public string Date { get; set; }

        #endregion

        public ChatData(User localUser, User remoteUser, List<ChatTextMessage> userMessages, 
                        List<ChatTextMessage> remoteMessages, string date)
        {
            LocalUser = localUser;
            RemoteUser = remoteUser;
            UserMessages = userMessages;
            RemoteMessages = remoteMessages;
            Date = date;
        }
        
    }
}
