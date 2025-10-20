using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class ChickfilA : Institution<FastFood, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override FastFood Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly ChickfilAService _service;

        public ChickfilA(IEnumerable<Food> menu, Adress adress, Meneger meneger, ChickfilAService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class ChickfilAService
    {
        private ChickfilA _chickfilA;
        public ChickfilAService(ChickfilA chickfilA) => _chickfilA = chickfilA;
        public void AddChickenSandwich(Food sandwich) => _chickfilA.Menu = _chickfilA.Menu.Append(sandwich);
        public bool IsSundayClosed() => true; 
    }
}
