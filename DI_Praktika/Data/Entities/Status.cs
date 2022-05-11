﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Data.Entities
{
    public class Status
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public ICollection<Query> Queries { get; set; }
    }
}
