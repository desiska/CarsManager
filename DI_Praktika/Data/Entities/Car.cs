using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Data.Entities
{
    public class Car
    {
        public string ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int PassangersCount { get; set; }
        public string Description { get; set; }
        public double PriceForRentForDay { get; set; }
        public string Photo { get; set; }
        public ICollection<Query> Queries { get; set; }

        public override string ToString()
        {
            return $"{Brand} {Model} {Year}";
        }
    }
}
