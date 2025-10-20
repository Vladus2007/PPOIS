using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public abstract class Institution<T,K>
    {
        
        public abstract string Description { get; set; }

        public abstract T Type { get; set; }

        public abstract double Mark { get; set; }

        public abstract IEnumerable<K> Menu { get; set; }

        public abstract Adress Adress { get; set; }

        


    }
}
