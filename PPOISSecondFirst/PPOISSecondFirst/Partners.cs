using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class Partners<T> where T : Institution<Types, Food>
    {
        public HashSet<T> Partner { get; private set; }

        public Partners(List<T> list)
        {
            foreach(T item in list)
            {
                Add(item);
            }
        }
        public Partners(IEnumerable<T> list)
        {
            foreach(var item in list)
            {
                Add(item);
            }
        }

        private void Add(T item)
        {
            try
            {
                Partner.Add(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Request");
            }
        }
        public void Remove(T item)
        {
            try
            {
                Partner.Remove(item);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void Rename(T start,T end)
        {
      
                Partner.Remove(start);
                Partner.Add(end);
            
        }
    }
}
