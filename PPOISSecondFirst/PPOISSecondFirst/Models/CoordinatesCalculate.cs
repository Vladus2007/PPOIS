using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class CoordinatesCalculate
    {
        public CoordinatesCalculate() { }

        public Coordinates? Nearest(IEnumerable<User> users, Coordinates coor)
        {
            try
            {
                if(users==null)  throw new ArgumentNullException($"{users}");
                if(coor==null)  throw new ArgumentNullException($"{coor}");
                double min = 0;
                Coordinates results = null;
                foreach (User user in users)
                {
                    if (min >= (Math.Abs(Sqrt(user.adress.coordinates) - Sqrt(coor))))
                    {
                        min = Math.Abs(Sqrt(user.adress.coordinates) - Sqrt(coor));
                        results = user.adress.coordinates;

                    }



                }

                return results;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        
        }
        private double Sqrt(Coordinates coordinates)
        {
            return Math.Sqrt(coordinates.x)+Math.Sqrt(coordinates.y);
        }
        private double Min(List<double> list)
        {
            double min = 0;   
            foreach(double value in list)
            {
                min = Math.Min(min, value);

            }
            return min;
        }
    }
}
