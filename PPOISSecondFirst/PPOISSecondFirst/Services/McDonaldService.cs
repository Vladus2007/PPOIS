using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class McDonaldService
    {
        public Food BuyFood(string name,IEnumerable<Food> Menu)
        {
            var zakaz = Menu.FirstOrDefault(n => n.Name == name);
            if (zakaz == null) throw new Exception("Food is not in store :(");
            zakaz.Count--;
            return zakaz;
        }


        public void GetMark(double markOfUser, ref double Mark, ref int countOfMetting)
        {
            Mark = (markOfUser + (countOfMetting * Mark)) / ++countOfMetting;
            countOfMetting++;
        }
    }
}

