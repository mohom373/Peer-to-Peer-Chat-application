using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

        public string Data { get; private set; }

        public ResponseType ResponseType { get; private set; }

        public string Date { get; private set; }

        //public BitmapImage Picture { get; set; }
        #endregion

        /// <summary>
        /// NetworkData constructor
        /// </summary>
        /// <param name="user">User info</param>
        /// <param name="dataType">Type of network data</param>
        /// <param name="data">A message containing data</param>
        /// <param name="date">A date in string form</param>
        public NetworkData(User user, NetworkDataType dataType, string data = "", 
            ResponseType responseType = ResponseType.None, string date = ""/*, BitmapImage picture = null*/)
        {
            User = user;
            DataType = dataType;
            Data = data;
            ResponseType = responseType;
            
            if (date == "")
            {
                Date = DateTime.Now.ToString();
            }
            else
            {
                Date = date;
            }

            //if (picture == null)
            //{
            //    Picture = new BitmapImage();
            //}
            //else
            //{
            //    Picture = picture;
            //}

        }
    }
}
