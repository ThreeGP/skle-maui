using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class Produkt
    {
        public string Nazwa { get; set; }
        public string Jednostka { get; set; }
        public int Ilosc { get; set; }
        public bool CzyKupiony { get; set; }
        public string Sklep { get; set; }
        public bool CzyOpcjonalny { get; set; }
        public string IdKategorii { get; set; }

        public Produkt()
        {
            Nazwa = "";
            Jednostka = "szt.";
            Ilosc = 1;
            CzyKupiony = false;
            Sklep = "Dowolny";
            CzyOpcjonalny = false;
            IdKategorii = "";
        }
    }
}
