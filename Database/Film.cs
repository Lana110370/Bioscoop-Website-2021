using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bioscoop_Website_2021.Database
{
    public class Film
    {
        public int id { get; set; }

        public string Titel { get; set; }

        public int Leeftijdsgrens { get; set; }

        public string Beschrijving { get; set; }

        public string Img { get; set; }

        public string Trailer { get; set; }

        public int Genre { get; set; }

        public int Tijdsduur { get; set; }

    }
}
