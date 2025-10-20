using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Meneger:Staff
    {
        public override string PhoneNumber { get; set; } = null!;

        public override decimal Salary { get;  }
        public override decimal Balanse { get; set; }
        public Meneger(StaffInformation inf)
        {
            PhoneNumber = inf.phoneNumber;
            Name= inf.name;
            Surname = inf.surname;
            Description = inf.description;
            YearsOld = inf.yearsOld;
        }
        public override string Name { get; set; } = null!;

        public override string Surname { get; set; } = null!;


        public override string Description { get; set; } = null!;

        public override int YearsOld { get; set; }








    }
}
