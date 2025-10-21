using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Validation:IProgrammerValidation
    {
        
        public string SetTypeDepartment(string typeDepartment)
        {
            string TypeDepartment="";
            if (typeDepartment == null) throw new ArgumentNullException("type");

            if (typeDepartment == "Mobile") TypeDepartment = typeDepartment;
            if (typeDepartment == "Backend") TypeDepartment = typeDepartment;
            if (typeDepartment == "Frontend") TypeDepartment = typeDepartment;
            if (typeDepartment == "Desktop") TypeDepartment = typeDepartment;


            return TypeDepartment;
        }

    }
}
