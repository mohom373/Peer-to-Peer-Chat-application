using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Exceptions
{
    public class HistoryNotFoundException : Exception
    {
        public HistoryNotFoundException()
        {
        }

        public HistoryNotFoundException(string message)
            : base(message)
        {
        }

        public HistoryNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
