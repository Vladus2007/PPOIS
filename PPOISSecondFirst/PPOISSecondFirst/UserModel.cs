using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class UserModel
    {
       public decimal balance { get; set; }


        private int id;

        public string Name { get; set; }    
        public string Surname { get; set; }

        public DateTime BirthDay { get;  set; }

        public Adress adress { get;  set; }
        public IEnumerable<Food> ListOfFoods { get;  set; }

        


        

    }
}
