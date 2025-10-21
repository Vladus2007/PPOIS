using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public interface IUserService
    {

        public void BuyFood(Basket basket, IEnumerable<Food> ListOfFoods, decimal balance);
       
        public void PayAnything(decimal price, decimal balance);


        public void GiveMoney(decimal money, ref decimal balance);
        
    }
}
