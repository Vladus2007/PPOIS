using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Basket
    {
        public decimal price { get; private set; }
        public IEnumerable<Food> BasketOfFood { get; private set; }

        public Basket()
        {

        }
        public void AddItem(Food food)
        {
            BasketOfFood.Append(food);
            price += food.Price;
        }
        public void AddItem(IEnumerable<Food> items)
        {

            foreach (Food item in items)
            {
                BasketOfFood.Append(item);
                price += item.Price;   
            }


        }


        


    }
}
