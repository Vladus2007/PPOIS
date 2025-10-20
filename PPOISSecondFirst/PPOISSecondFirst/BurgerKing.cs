using PPOISSecondFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{

    public class BurgerKing : Institution<FastFood, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override FastFood Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly BurgerKingService _service;

        public BurgerKing(IEnumerable<Food> menu, Adress adress, Meneger meneger, BurgerKingService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class BurgerKingService
{
    private BurgerKing _bk;
    public BurgerKingService(BurgerKing bk) => _bk = bk;
    public void AddWhopper(Food whopper) => _bk.Menu = _bk.Menu.Append(whopper);
    public bool HasWhopper() => _bk.Menu.Any(f => f.Name.Contains("Whopper"));
}

}
