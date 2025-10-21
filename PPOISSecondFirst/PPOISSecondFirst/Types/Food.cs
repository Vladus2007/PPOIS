using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Food
    {
        public decimal Price { get; set; }

        public string Name { get; set; }

        public bool Milk { get; set; }

        public bool Alhocol { get; set; }

        public int Count { get; set; }

        public Food(decimal price,string name,bool milk,bool alhocol,int count)
        {
            this.Price = price;
            this.Name = name;
            this.Milk = milk;
            this.Alhocol = alhocol;
            this.Count = count;
        }


    }
}
