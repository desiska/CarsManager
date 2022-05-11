using DI_Praktika.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Models.Query
{
    public class QueryCreateVM
    {
        private DateTime start = DateTime.Now;
        private DateTime end = DateTime.Now;

        [Required]
        public DateTime Start 
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        [Required]
        public DateTime End 
        {
            get
            {
                return end;
            }
            set
            {
                if (value < this.Start)
                {
                    throw new Exception();
                }
                else
                {
                    end = value;
                }
            }
        }
        [Required]
        public string CarID { get; set; }
        public ICollection<Data.Entities.Car> Cars { get; set; }
        public string User { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Status { get; set; }
        public ICollection<Status> StatusObject { get; set; }
        public string Message { get; set; }
        public bool IsFirstTime { get; set; }
    }
}
