using MauiApp1.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MauiApp1.Views
{
    public partial class KategoriaView : ContentView, INotifyPropertyChanged
    {
        public event EventHandler KategoriaZmieniona;

        private Kategoria kategoria;

        public Kategoria Kategoria
        {
            get => kategoria;
            set
            {
                kategoria = value;
                OdswiezWidok();
            }
        }

        public string NazwaKategorii => Kategoria?.Nazwa ?? "";
        public bool CzyRozwinieta => Kategoria?.CzyRozwinieta ?? false;
        public string TekstPrzycisku => CzyRozwinieta ? "-" : "+";

        public KategoriaView()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void OnRozwinClicked(object sender, EventArgs e)
        {
            if (Kategoria != null)
            {
                Kategoria.CzyRozwinieta = !Kategoria.CzyRozwinieta;
                OdswiezWidok();
                KategoriaZmieniona?.Invoke(this, EventArgs.Empty);
            }
        }

        private async void OnDodajProduktClicked(object sender, EventArgs e)
        {
            if (Kategoria == null) return;

            string nazwa = await Application.Current.MainPage.DisplayPromptAsync(
           "Nowy produkt", "Podaj nazwe produktu:");

            if (string.IsNullOrWhiteSpace(nazwa)) return;

            string jednostka = await Application.Current.MainPage.DisplayPromptAsync(
            "Jednostka", "Podaj jednostke (szt., kg, l):", initialValue: "szt.");

            string iloscStr = await Application.Current.MainPage.DisplayPromptAsync(
            "Ilosc", "Podaj ilosc:", initialValue: "1", keyboard: Keyboard.Numeric);

            int ilosc = 1;
            if (!string.IsNullOrWhiteSpace(iloscStr))
            {
                int.TryParse(iloscStr, out ilosc);
            }

            var sklepy = new string[] { "Dowolny", "Biedronka", "Lidl", "Kaufland", "Auchan", "Carrefour", "Zabka" };
            string sklep = await Application.Current.MainPage.DisplayActionSheet(
     "Wybierz sklep:", "Anuluj", null, sklepy);
            if (sklep == "Anuluj" || string.IsNullOrEmpty(sklep)) sklep = "Dowolny";

            bool opcjonalny = await Application.Current.MainPage.DisplayAlert(
         "Opcjonalny?", "Czy produkt jest opcjonalny?", "Tak", "Nie");

            var produkt = new Produkt
            {
                Nazwa = nazwa,
                Jednostka = jednostka ?? "szt.",
                Ilosc = ilosc,
                CzyKupiony = false,
                Sklep = sklep,
                CzyOpcjonalny = opcjonalny,
                IdKategorii = Kategoria.Id
            };

            Kategoria.Produkty.Add(produkt);
            OdswiezWidok();
            KategoriaZmieniona?.Invoke(this, EventArgs.Empty);
        }

        private void OdswiezWidok()
        {
            OnPropertyChanged(nameof(NazwaKategorii));
            OnPropertyChanged(nameof(CzyRozwinieta));
            OnPropertyChanged(nameof(TekstPrzycisku));

            ListaProduktow.Children.Clear();

            if (Kategoria != null && Kategoria.CzyRozwinieta)
            {
                // sortowac - najpierw niekupione, potem kupione
                var posortowane = Kategoria.Produkty.OrderBy(p => p.CzyKupiony).ToList();

                foreach (var produkt in posortowane)
                {
                    var produktView = new ProduktView { Produkt = produkt };
                    produktView.ProduktUsuniety += (s, p) =>
                      {
                          Kategoria.Produkty.Remove(p);
                          OdswiezWidok();
                          KategoriaZmieniona?.Invoke(this, EventArgs.Empty);
                      };
                    produktView.ProduktZmieniony += (s, p) =>
                      {
                          OdswiezWidok();
                          KategoriaZmieniona?.Invoke(this, EventArgs.Empty);
                      };
                    ListaProduktow.Children.Add(produktView);
                }
            }
        }
    }
}
