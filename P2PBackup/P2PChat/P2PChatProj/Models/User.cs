using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Models
{
    public class User : INotifyPropertyChanged
    {
        
        private string userName;
        private string ipAddress;
        private int portNumber;

        public User(string userName, string ipAddress, int portNumber)
        {
            this.userName = userName;
            this.ipAddress = ipAddress;
            this.portNumber = portNumber;
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
            } 
        }

        public string IpAddress
        {
            get
            {
                return ipAddress;
            }

            set
            {
                ipAddress = value;
                RaisePropertyChanged("IpAddress");
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
