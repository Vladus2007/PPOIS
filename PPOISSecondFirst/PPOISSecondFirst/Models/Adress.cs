using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Adress
    {
        public string street { get;private set; }
        public string city { get;private set; }
        public string state { get;private set; }

        public string house { get; private set; }

        public int floor { get;private set; }

        public Adress(string street,string city,string state,int floor=1)
        {
            this.street = street;
            this.city = city;
            this.state = state;
            this.floor= floor;  
        }
        public Coordinates coordinates { get; private set; }

        

    }
}
