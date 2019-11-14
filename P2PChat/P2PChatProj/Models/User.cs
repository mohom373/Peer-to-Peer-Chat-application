using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Model
{
    public class User : INotifyPropertyChanged
    {
        
        private string userName;
        private int portNumber;
        public User()
        {
            userName = "Kimmo SID";
            portNumber = 1337;
        }

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

        public int PortNumber 
        { 
            get
            {
                return portNumber;
            }
            
            set
            {
                portNumber = value;
                RaisePropertyChanged("PortNumber");
                RaisePropertyChanged("InfoString");
            }
        }

        public string InfoString 
        { 
            get
            {
                return "User: " + userName + "  | Port: " + portNumber.ToString();
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
