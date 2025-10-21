using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Wendys : Institution<FastFood, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override FastFood Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly WendysService _service;

        public Wendys(IEnumerable<Food> menu, Adress adress, Meneger meneger, WendysService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class WendysService
    {
        private Wendys _wendys;
        public WendysService(Wendys wendys) => _wendys = wendys;
        public void AddFrosty(Food frosty) => _wendys.Menu = _wendys.Menu.Append(frosty);
        public bool HasSquareBurgers() => true; 
    }
}
