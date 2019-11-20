using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketTest
{
    class Request
    {
        private Socket socket;
        private string name;

        public Request(Socket s, string n)
        {
            socket = s;
            name = n;
        }

        public Socket Socket 
        { 
            get { return socket; } 
            set { socket = value; } 
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}
