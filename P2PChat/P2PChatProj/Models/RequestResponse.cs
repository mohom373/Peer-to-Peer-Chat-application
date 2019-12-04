using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Models
{
    public class RequestResponse : NetworkData
    {
        public RequestResponse(ResponseType type, string userName, string ipAddress, int portNumber)
        {
            ResponseType = type;
            UserName = userName;
            IpAddress = ipAddress;
            PortNumber = portNumber;
        }

        public ResponseType ResponseType { get; set; }
    }
}
