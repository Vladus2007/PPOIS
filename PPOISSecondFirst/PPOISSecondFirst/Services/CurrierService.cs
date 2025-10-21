using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class StaffService<T> where T : Staff
    {

        public void WorkSucces(T staff)
        {
            staff.Balanse += staff.Salary;
        }


    }
}
