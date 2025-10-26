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
    public class StaffServiceException : Exception
    {
    public StaffServiceException() { }
    public StaffServiceException(string message) : base(message) { }
    public StaffServiceException(string message, Exception inner) : base(message, inner) { }
    }
}
