using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISFirstFirst
{
    public class DefaultLengthCalculator : ILength
    {
        public double CalculateLength(double x, double y, double z)
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }
    }
}
