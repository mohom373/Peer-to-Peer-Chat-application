﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Model
{
    public class User : INotifyPropertyChanged, IDataErrorInfo
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

        #region IDataErrorInfo members
        public string Error
        {
            get;
            private set;
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "UserName")
                {
                    if (String.IsNullOrWhiteSpace(UserName))
                    {
                        Error = "Username can't be null or empty. ";
                    }
                    else
                    {
                        Error = null;
                    }
                }

                //else if (columnName == "PortNumber")
                //{
                //    if (portNumber < 1024 && portNumber > 64000)
                //    {

                //        Error = "Portnumber can't be less than 1024 or exceed 65000";
                //    }
                //    else
                //    {
                //        Error = null;
                //    }
                //}
                return Error;
            }
        }
        #endregion
    }
}
