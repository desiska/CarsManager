using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Models.Query
{
    public class QueryVM
    {
        public string ID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Car { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
    }
}
