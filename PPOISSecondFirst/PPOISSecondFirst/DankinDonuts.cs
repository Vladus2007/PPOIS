using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class DunkinDonuts : Institution<Cafe, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override Cafe Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        private readonly DunkinDonutsService _service;

        public DunkinDonuts(IEnumerable<Food> menu, Adress adress, Meneger meneger, DunkinDonutsService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class DunkinDonutsService
    {
        private DunkinDonuts _dunkin;
        public DunkinDonutsService(DunkinDonuts dunkin) => _dunkin = dunkin;
        public void AddDonut(Food donut) => _dunkin.Menu = _dunkin.Menu.Append(donut);
        public int CountDonuts() => _dunkin.Menu.Count(f => f.Name.Contains("Donut"));
    }

}
