using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Models.Query
{
    public class QueryIndexVM
    {
        public ICollection<QueryVM> Items { get; set; }
    }
}
