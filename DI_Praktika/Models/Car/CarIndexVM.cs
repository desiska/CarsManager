using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Models.Car
{
    public class CarIndexVM
    {
        public ICollection<Data.Entities.Car> Items { get; set; }
    }
}
