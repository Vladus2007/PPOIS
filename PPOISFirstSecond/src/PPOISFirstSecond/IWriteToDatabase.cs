using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISFirstSecond
{
    public interface IWriteToDatabase
    {
        void WriteDatabase(WordPair pair);
    }
}
