using P2PChatProj.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

        public List<SavedChatMessage> UserMessages { get; set; }

        public List<SavedChatMessage> RemoteMessages { get; set; }

        public string Date { get; set; }

        #endregion

        public ChatData(User localUser, User remoteUser, List<SavedChatMessage> userMessages,
                        List<SavedChatMessage> remoteMessages, string date)
        {
            LocalUser = localUser;
            RemoteUser = remoteUser;
            Date = date;

            UserMessages = userMessages;
            RemoteMessages = remoteMessages;
        }
    }
}
