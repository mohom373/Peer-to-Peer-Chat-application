using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Models
{
    public class MessageData : NetworkData
    {
        public MessageData(string message, string date, string userName, string ipAddress, int portNumber)
        {
            Message = message;
            Date = date;
            UserName = userName;
            IpAddress = ipAddress;
            PortNumber = portNumber;
        }

        public string Message { get; set; }

        public string Date { get; set; }
    }
}
