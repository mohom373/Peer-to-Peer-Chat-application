using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMTest.Model
{
    public class User : INotifyPropertyChanged
    {
        private string userName;
        private int port;

        public string UserName 
        {
            get
            {
                return userName;
            }

            set 
            {
                userName = value;
                RaisePropertyChanged("UserName");
                RaisePropertyChanged("InfoString");
            } 
        }

        public int Port 
        { 
            get
            {
                return port;
            }
            
            set
            {
                port = value;
                RaisePropertyChanged("Port");
                RaisePropertyChanged("InfoString");
            }
        }

        public string InfoString 
        { 
            get
            {
                return "User: " + userName + "  | Port: " + port.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
