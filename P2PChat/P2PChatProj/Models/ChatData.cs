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

        public ObservableCollection<ChatMessage> UserMessages { get; set; }

        public ObservableCollection<ChatMessage> RemoteMessages { get; set; }

        public string Date { get; set; }

        public ChatData(User localUser, User remoteUser, ObservableCollection<ChatMessage> userMessages, 
                        ObservableCollection<ChatMessage> remoteMessages, string date)
        {
            LocalUser = localUser;
            RemoteUser = remoteUser;
            UserMessages = userMessages;
            RemoteMessages = remoteMessages;
            Date = date;
        }
        #endregion



    }
}
