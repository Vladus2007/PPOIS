using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class User:UserModel
    {
        private readonly UserService _userService;
        public User(UserModel userModel,UserService userService)
        {
            userModel.balance = balance;
            userModel.BirthDay = BirthDay;
            userModel.Surname = Surname;
            userModel.Name = Name;
            userModel.adress = adress;
            _userService = userService; 

        }
        

      
    }
}
