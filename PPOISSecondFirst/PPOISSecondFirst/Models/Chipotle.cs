using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Chipotle : Institution<FastFood, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override FastFood Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly ChipotleService _service;

        public Chipotle(IEnumerable<Food> menu, Adress adress, Meneger meneger, ChipotleService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class ChipotleService
    {
        private Chipotle _chipotle;
        public ChipotleService(Chipotle chipotle) => _chipotle = chipotle;
        public void AddBurrito(Food burrito) => _chipotle.Menu = _chipotle.Menu.Append(burrito);
        public bool HasGuacamole() => _chipotle.Menu.Any(f => f.Name.Contains("Guacamole"));
    }

}
