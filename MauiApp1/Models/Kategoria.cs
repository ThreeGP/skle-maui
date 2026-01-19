using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class Kategoria
    {
        public string Id { get; set; }
        public string Nazwa { get; set; }
        public ObservableCollection<Produkt> Produkty { get; set; }
        public bool CzyRozwinieta { get; set; }

        public Kategoria()
        {
            Id = Guid.NewGuid().ToString();
            Nazwa = "";
            Produkty = new ObservableCollection<Produkt>();
            CzyRozwinieta = false;
        }
    }
}
