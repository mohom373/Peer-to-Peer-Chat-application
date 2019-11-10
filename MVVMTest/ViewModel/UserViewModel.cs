using MVVMTest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMTest.ViewModel
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            LoadUsers();
        }

        public ObservableCollection<User> Users
        {
            get;
            set;
        }

        public void LoadUsers()
        {
            ObservableCollection<User> users = new ObservableCollection<User>();

            users.Add(new User { UserName = "Victor", Port = 1030 });
            users.Add(new User { UserName = "Moe", Port = 1040 });
            users.Add(new User { UserName = "Rune", Port = 1337 });

            Users = users;
        }
    }
}
