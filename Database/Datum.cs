﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bioscoop_Website_2021.Database
{
    public class Datum
    {
        public int id { get; set; }

        public DateTime datumtijd { get; set; }

        public int film_id { get; set; }

        public string voorraad { get; set; }


    }
}
