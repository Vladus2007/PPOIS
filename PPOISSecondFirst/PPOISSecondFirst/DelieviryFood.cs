using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
   
    public class DelieviryFood
    {
        public IEnumerable<Dispetcher> Dispetchers { get;  set; }

        public IEnumerable<Currier> curriers { get;  set; }
        public static  string name { get; private set; }

        public static DelieviryFood singletone = null;

         static DelieviryFood()
        {
           singletone=new DelieviryFood();
        }

        public void Rename(string Name)
        {
            TextInfo textinfo = CultureInfo.CurrentCulture.TextInfo;
            name = textinfo.ToTitleCase(Name.ToLower());
        }


        public void BuyFoodForUser( ref User user,Basket food)
        {

            user.balance -= food.price;


        }

        
        



    }
}
