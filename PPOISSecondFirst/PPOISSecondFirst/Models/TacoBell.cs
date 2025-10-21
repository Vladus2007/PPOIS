using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class TacoBell : Institution<FastFood, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override FastFood Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly TacoBellService _service;

        public TacoBell(IEnumerable<Food> menu, Adress adress, Meneger meneger, TacoBellService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class TacoBellService
    {
        private TacoBell _tacoBell;
        public TacoBellService(TacoBell tacoBell) => _tacoBell = tacoBell;
        public void AddTaco(Food taco) => _tacoBell.Menu = _tacoBell.Menu.Append(taco);
        public bool IsSpicyMenuAvailable() => _tacoBell.Menu.Any(f => f.Name.Contains("Spicy"));
    }

}
