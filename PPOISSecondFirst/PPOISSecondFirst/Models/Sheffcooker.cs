using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Sheffcooker:Staff
    {
        public override string PhoneNumber { get; set; } = null!;

        public override decimal Salary { get; }
        public override decimal Balanse { get; set; }
        public override string Name { get; set; } = null!;

        public override string Surname { get; set; } = null!;


        public override string Description { get; set; } = null!;

        public override int YearsOld { get; set; }

        public int countStarMischlen { get; private set; }

        public int countOfExpirience { get; private set; }

        
        public Sheffcooker(StaffInformation information,int countStarMishlen,int countOfExpirirence)
        {
            PhoneNumber = information.phoneNumber;
            Name = information.name;
            Surname = information.surname;
            Description = information.description;
            YearsOld = information.yearsOld;
            this.countOfExpirience = countOfExpirirence;
            this.countStarMischlen= countStarMishlen;

        }

    }
}
