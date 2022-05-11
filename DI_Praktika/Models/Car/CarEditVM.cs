using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Models.Car
{
    public class CarEditVM
    {
        public string ID { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string CarModel { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int PassangersCount { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double PriceForRentForDay { get; set; }
        [Required]
        public string Photo { get; set; }
    }
}
