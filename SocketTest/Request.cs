using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketTest
{
    class Request
    {
        private IPAddress ip;
        private int port;
        private string name;

        public IPAddress Ip 
        { 
            get { return ip; } 
            set { ip = value; } 
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}
