using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class McDonalds : Institution<FastFood, Food>
    {
        public Meneger _meneger { get; set; }

        public override string Description { get; set; }

        public override FastFood Type { get; set; }

        public override double Mark { get; set; }

        private readonly McDonaldService _service;

        public double countOfMetteng { get; set; }

        public override IEnumerable<Food> Menu { get; set; }

        public McDonalds(IEnumerable<Food> _Menu, Adress adress, Meneger meneger,McDonaldService service)
        {
            _service = service;
            Menu = _Menu;
            Adress = adress;
            _meneger = meneger;
        }

        public override Adress Adress { get; set; }


    }
}
