using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Models
{
    /// <summary>
    /// Datatype used for sending data over the network
    /// </summary>
    public class NetworkData
    {
        #region Properties
        public User User { get; private set; }

        public NetworkDataType DataType { get; private set; }

        public string Message { get; private set; }

        public ResponseType ResponseType { get; private set; }

        public string Date { get; private set; }
        #endregion

        /// <summary>
        /// NetworkData constructor
        /// </summary>
        /// <param name="user">User info</param>
        /// <param name="dataType">Type of network data</param>
        /// <param name="message">A message containing data</param>
        /// <param name="date">A date in string form</param>
        public NetworkData(User user, NetworkDataType dataType, string message = "", 
            ResponseType responseType = ResponseType.None, string date = "")
        {
            User = user;
            DataType = dataType;
            Message = message;
            ResponseType = responseType;
            
            if (date == "")
            {
                Date = DateTime.Now.ToString();
            }
        }
    }
}
