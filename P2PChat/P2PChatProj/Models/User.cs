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
        private int portNumber;

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
