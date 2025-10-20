using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Starbucks : Institution<Cafe, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override Cafe Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly StarbucksService _service;

        public Starbucks(IEnumerable<Food> menu, Adress adress, Meneger meneger, StarbucksService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class StarbucksService
    {
        private Starbucks _starbucks;
        public StarbucksService(Starbucks starbucks) => _starbucks = starbucks;
        public void AddCoffee(Food coffee) => _starbucks.Menu = _starbucks.Menu.Append(coffee);
        public bool HasFrappuccino() => _starbucks.Menu.Any(f => f.Name.Contains("Frappuccino"));
    }
}
