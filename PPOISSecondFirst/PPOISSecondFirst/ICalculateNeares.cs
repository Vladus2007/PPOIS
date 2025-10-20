using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public interface ICalculateNeares
    {
        public Coordinates? Nearest(IEnumerable<User> users, Coordinates coor);
    }
}
