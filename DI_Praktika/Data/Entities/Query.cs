using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Data.Entities
{
    public class Query
    {
        public string ID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string carID { get; set; }
        public Car Car { get; set; }
        public string userID { get; set; }
        public User User { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public Status StatusObject { get; set; }
    }
}
