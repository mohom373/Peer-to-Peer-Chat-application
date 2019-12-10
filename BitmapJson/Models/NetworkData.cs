using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapJson.Models
{
    public class NetworkData
    {
        #region Properties
        public string Name { get; private set; }

        public string Data { get; private set; }

        public bool ImageMessage { get; set; }

        #endregion

        public NetworkData(string name, string data, bool imageMessage)
        {
            Name = name;
            Data = data;
            ImageMessage = imageMessage;
            
        }
    }
}
