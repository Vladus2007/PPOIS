using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISFirstSecond
{
    public class WordPair
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Values { get; set; }
    }
}
