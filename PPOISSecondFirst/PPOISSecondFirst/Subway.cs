using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Subway : Institution<FastFood, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override FastFood Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly SubwayService _service;

        public Subway(IEnumerable<Food> menu, Adress adress, Meneger meneger, SubwayService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class SubwayService
    {
        private Subway _subway;
        public SubwayService(Subway subway) => _subway = subway;
        public void CreateCustomSandwich(List<Food> ingredients) => _subway.Menu = _subway.Menu.Concat(ingredients);
        public int CountSandwiches() => _subway.Menu.Count(f => f.Name.Contains("Sandwich"));
    }
}
