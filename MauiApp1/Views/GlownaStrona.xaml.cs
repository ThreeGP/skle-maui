using MauiApp1.Models;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiApp1.Views
{
    public partial class GlownaStrona : ContentPage
    {
        private ObservableCollection<Kategoria> kategorie;
        private PlikSerwis plikSerwis;

        public GlownaStrona()
        {
            InitializeComponent();
            plikSerwis = new PlikSerwis();
            kategorie = new ObservableCollection<Kategoria>();
            Wczytaj();
        }

        private async void Wczytaj()
        {
            kategorie = await plikSerwis.WczytajKategorieAsync();

            if (kategorie.Count == 0)
            {
                // dodac kilka domyslnych kategorii
                DodajDomyslneKategorie();
            }

            OdswiezListeKategorii();
        }

        private void DodajDomyslneKategorie()
        {
            kategorie.Add(new Kategoria { Nazwa = "Nabial" });
            kategorie.Add(new Kategoria { Nazwa = "Warzywa i Owoce" });
            kategorie.Add(new Kategoria { Nazwa = "Mieso i Wedliny" });
            kategorie.Add(new Kategoria { Nazwa = "Pieczywo" });
            kategorie.Add(new Kategoria { Nazwa = "Napoje" });
        }

        private async void OnDodajKategorieClicked(object sender, EventArgs e)
        {
            string nazwa = await DisplayPromptAsync("Nowa kategoria", "Podaj nazwe kategorii:");

            if (!string.IsNullOrWhiteSpace(nazwa))
            {
                var nowaKategoria = new Kategoria { Nazwa = nazwa };
                kategorie.Add(nowaKategoria);
                OdswiezListeKategorii();
                await ZapiszAsync();
            }
        }

        private void OdswiezListeKategorii()
        {
            ListaKategorii.Children.Clear();

            foreach (var kategoria in kategorie)
            {
                var kategoriaView = new KategoriaView { Kategoria = kategoria };
                kategoriaView.KategoriaZmieniona += async (s, e) =>
                {
                    await ZapiszAsync();
                };
                ListaKategorii.Children.Add(kategoriaView);
            }
        }

        private async Task ZapiszAsync()
        {
            await plikSerwis.ZapiszKategorieAsync(kategorie);
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            var akcja = await DisplayActionSheet("Menu", "Anuluj", null,
           "Lista do Sklepu", "Lista dla Sklepu", "Eksportuj", "Importuj", "Zapamietaj");

            if (akcja == "Lista do Sklepu")
            {
                await Navigation.PushAsync(new ListaDoSklepuStrona(kategorie, plikSerwis));
            }
            else if (akcja == "Lista dla Sklepu")
            {
                var sklepy = kategorie.SelectMany(k => k.Produkty).Select(p => p.Sklep).Distinct().ToList();
                var wybrany = await DisplayActionSheet("Wybierz sklep:", "Anuluj", null, sklepy.ToArray());
                if (wybrany != "Anuluj" && !string.IsNullOrEmpty(wybrany))
                {
                    await Navigation.PushAsync(new ListaDlaSklepuStrona(kategorie, wybrany, plikSerwis));
                }
            }
            else if (akcja == "Eksportuj")
            {
                await EksportujListe();
            }
            else if (akcja == "Importuj")
            {
                await ImportujListe();
            }
            else if (akcja == "Zapamietaj")
            {
                await ZapiszAsync();
                await DisplayAlert("Zapisano", "Lista zostala zapisana", "OK");
            }
        }

        private async Task EksportujListe()
        {
            try
            {
                var sciezka = Path.Combine(FileSystem.AppDataDirectory, "eksport_lista.json");
                await plikSerwis.EksportujDoPliku(kategorie, sciezka);
                await DisplayAlert("Eksport", $"Lista wyeksportowana do:\n{sciezka}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Blad", "Nie udalo sie wyeksportowac: " + ex.Message, "OK");
            }
        }

        private async Task ImportujListe()
        {
            try
            {
                var sciezka = Path.Combine(FileSystem.AppDataDirectory, "eksport_lista.json");
                if (File.Exists(sciezka))
                {
                    var zaimportowane = await plikSerwis.ImportujZPliku(sciezka);
                    foreach (var kat in zaimportowane)
                    {
                        kategorie.Add(kat);
                    }
                    OdswiezListeKategorii();
                    await ZapiszAsync();
                    await DisplayAlert("Import", "Lista zaimportowana", "OK");
                }
                else
                {
                    await DisplayAlert("Blad", "Nie znaleziono pliku do importu", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Blad", "Nie udalo sie zaimportowac: " + ex.Message, "OK");
            }
        }
    }
}
