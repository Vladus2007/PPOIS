using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class FiveGuys : Institution<FastFood, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override FastFood Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly FiveGuysService _service;

        public FiveGuys(IEnumerable<Food> menu, Adress adress, Meneger meneger, FiveGuysService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class FiveGuysService
    {
        private FiveGuys _fiveGuys;
        public FiveGuysService(FiveGuys fiveGuys) => _fiveGuys = fiveGuys;
        public void AddBurger(Food burger) => _fiveGuys.Menu = _fiveGuys.Menu.Append(burger);
        public int CountFreeToppings() => 15; // Five Guys известен бесплатными добавками
    }
}
