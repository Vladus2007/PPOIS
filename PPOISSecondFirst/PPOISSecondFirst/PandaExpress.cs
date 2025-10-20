using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class PandaExpress : Institution<FastFood, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override FastFood Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly PandaExpressService _service;

        public PandaExpress(IEnumerable<Food> menu, Adress adress, Meneger meneger, PandaExpressService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class PandaExpressService
    {
        private PandaExpress _panda;
        public PandaExpressService(PandaExpress panda) => _panda = panda;
        public void AddChineseFood(Food food) => _panda.Menu = _panda.Menu.Append(food);
        public IEnumerable<Food> GetOrangeChicken() => _panda.Menu.Where(f => f.Name.Contains("Orange Chicken"));
    }
}
