using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Models
{
    class RequestResponse
    {

        public RequestResponse(Response res, string un = "")
        {
            ResponseValue = res;
            UserName = un;
        }

        public Response ResponseValue { get; set; }

        public string UserName { get; set; }
    }
}
