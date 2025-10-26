using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class UserService:IUserService
    {
        public void BuyFood(Basket basket, IEnumerable<Food> ListOfFoods,decimal balance)
        {
            try
            {
                PayAnything(basket.price,balance);

                foreach (Food food in basket.BasketOfFood)
                {
                    ListOfFoods.Append(food);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void PayAnything(decimal price,decimal balance)
        {
            if (balance < price) throw new Exception("Balance lower than price");
            balance -= price;
        }

        public void GiveMoney(decimal money,ref decimal balance)
        {
            if (money < 0) throw new Exception("Money can not be negative");
            balance += money;

        }
    }
}
