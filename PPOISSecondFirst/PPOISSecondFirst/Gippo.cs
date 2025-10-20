using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Gippo:Institution<Shop,Food>

    {
        private Meneger _meneger { get; set; }

        public override string Description { get; set; }

        public override Shop Type { get; set; }

        public override double Mark { get; set; }



        public double countOfMetteng { get; set; }

        public override IEnumerable<Food> Menu { get; set; }

        public override Adress Adress { get; set; }
        public Food BuyFood(string name)
        {
            var zakaz = Menu.FirstOrDefault(n => n.Name == name);
            if (zakaz == null) throw new Exception("Food is not in store :(");
            zakaz.Count--;
            return zakaz;
        }


        public void GetMark(double markOfUser)
        {
            Mark = (markOfUser + (countOfMetteng * Mark)) / ++countOfMetteng;
            countOfMetteng++;
        }
    }
}
