using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class BaskinRobbins : Institution<IceCreamShop, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override IceCreamShop Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly BaskinRobbinsService _service;

        public BaskinRobbins(IEnumerable<Food> menu, Adress adress, Meneger meneger, BaskinRobbinsService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class BaskinRobbinsService
    {
        private BaskinRobbins _baskin;
        public BaskinRobbinsService(BaskinRobbins baskin) => _baskin = baskin;
        public void AddIceCream(Food iceCream) => _baskin.Menu = _baskin.Menu.Append(iceCream);
        public int CountFlavors() => _baskin.Menu.Count(f => f.Name.Contains("Ice Cream"));
    }

}
