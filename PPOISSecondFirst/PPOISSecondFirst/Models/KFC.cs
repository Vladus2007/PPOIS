using System;
using System.Collections.Generic;
using System.Linq;

namespace PPOISSecondFirst
{
    
    public class KFC : Institution<FastFood, Food>
    {
        public Meneger _meneger { get; set; }
        public override string Description { get; set; }
        public override FastFood Type { get; set; }
        public override double Mark { get; set; }
        public override IEnumerable<Food> Menu { get; set; }
        public override Adress Adress { get; set; }
        public double countOfMetteng { get; set; }
        private readonly KFCService _service;

        public KFC(IEnumerable<Food> menu, Adress adress, Meneger meneger, KFCService service)
        {
            Menu = menu;
            Adress = adress;
            _meneger = meneger;
            _service = service;
        }
    }

    public class KFCService
    {
        private KFC _kfc;
        public KFCService(KFC kfc) => _kfc = kfc;
        public void AddChickenBucket(Food chicken) => _kfc.Menu = _kfc.Menu.Append(chicken);
        public double CalculateColonelRating() => _kfc.Mark * 1.1;
    }

    
    


   
}