using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public abstract class Staff
    {


        public   abstract  decimal Salary { get; }
        public abstract string PhoneNumber { get; set; }

        public abstract decimal Balanse { get; set; }
        public abstract string Name { get; set; }

        public abstract string Surname { get; set; }


        public abstract string Description { get; set; }

        public abstract int YearsOld { get; set; }
    }
}
