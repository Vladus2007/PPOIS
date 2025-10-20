using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class PizzaHut : Institution<FastFood, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override FastFood Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly PizzaHutService _service;

        public PizzaHut(IEnumerable<Food> menu, Adress adress, Meneger meneger, PizzaHutService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class PizzaHutService
    {
        private PizzaHut _pizzaHut;
        public PizzaHutService(PizzaHut pizzaHut) => _pizzaHut = pizzaHut;
        public void AddPizza(Food pizza) => _pizzaHut.Menu = _pizzaHut.Menu.Append(pizza);
        public IEnumerable<Food> GetPizzas() => _pizzaHut.Menu.Where(f => f.Name.Contains("Pizza"));
    }

}
