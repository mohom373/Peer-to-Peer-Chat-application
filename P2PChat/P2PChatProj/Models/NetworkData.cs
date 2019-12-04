using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Models
{
    public abstract class NetworkData
    {
        public string UserName { get; set; }

        public string IpAddress { get; set; }

        public int PortNumber { get; set; }

    }
}
