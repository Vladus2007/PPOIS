using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Mamuka:Institution<FastFood,Food>
    {
        public Meneger _meneger { get; set; }

        public Sheffcooker sheffcooker { get; set; }

        public override string Description { get; set; }

        public override FastFood Type { get; set; }

        public override double Mark { get; set; }



        public double countOfMetteng { get; set; }

        public override IEnumerable<Food> Menu { get; set; } = Enumerable.Empty<Food>();

        public override Adress Adress { get; set; }

        public Mamuka(IEnumerable<Food> _Menu, Adress adress, 
            Meneger meneger, 
            Sheffcooker sheffcooker,
            string Description,
            MamukaService service
            )
        {
            Menu = _Menu;
            Adress = adress;
            _meneger = meneger;
            this.sheffcooker = sheffcooker;

            this.sheffcooker = sheffcooker;
            this.Description = Description;
        }


    }
}
