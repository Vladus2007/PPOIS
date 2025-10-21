using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Programmer:Staff
    {
        private readonly IProgrammerValidation _programmervalidation;
        public Programmer(StaffInformation information, IProgrammerValidation programmerValidation)
        {
            PhoneNumber = information.phoneNumber;
            Name = information.name;
            Surname = information.surname;
            Description = information.description;
            YearsOld = information.yearsOld;
           this.TypeDepartment= _programmervalidation.SetTypeDepartment(TypeDepartment);
        }

        public override decimal Salary { get;  }
        public override string PhoneNumber { get; set; } = null!;

        public override decimal Balanse { get   ; set; }


        public override string Name { get; set; } = null!;

        public override string Surname { get; set; } = null!;


        public override string Description { get; set; } = null!;

        public override int YearsOld { get; set; }

        private string TypeDepartment { get; set; }

        








    }
}
