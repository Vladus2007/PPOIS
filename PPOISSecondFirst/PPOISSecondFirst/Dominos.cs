using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Dominos : Institution<FastFood, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override FastFood Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly DominosService _service;

        public Dominos(IEnumerable<Food> menu, Adress adress, Meneger meneger, DominosService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class DominosService
    {
        private Dominos _dominos;
        public DominosService(Dominos dominos) => _dominos = dominos;
        public void AddPizza(Food pizza) => _dominos.Menu = _dominos.Menu.Append(pizza);
        public TimeSpan CalculateDeliveryTime() => TimeSpan.FromMinutes(30);
    }

}
