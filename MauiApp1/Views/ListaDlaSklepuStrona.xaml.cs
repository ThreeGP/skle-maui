using MauiApp1.Models;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiApp1.Views
{
    public partial class ListaDlaSklepuStrona : ContentPage
    {
        private ObservableCollection<Kategoria> kategorie;
        private string nazwaSklep;
        private PlikSerwis plikSerwis;

        public ListaDlaSklepuStrona(
            ObservableCollection<Kategoria> kat,
            string sklep,
            PlikSerwis serwis)
        {
            InitializeComponent();

            kategorie = kat;
            nazwaSklep = sklep;
            plikSerwis = serwis;

            TytulLabel.Text = $"Lista: {sklep}";
            OdswiezListe();
        }

        private void OdswiezListe()
        {
            ListaProduktow.Children.Clear();

            // zebrać produkty dla tego sklepu
            var produktyDlaSklepu = new List<(Produkt produkt, string nazwaKategorii)>();

            foreach (var kategoria in kategorie)
            {
                foreach (var produkt in kategoria.Produkty)
                {
                    if (produkt.Sklep == nazwaSklep)
                    {
                        produktyDlaSklepu.Add((produkt, kategoria.Nazwa));
                    }
                }
            }

            // sortować po nazwie kategorii i statusie kupienia
            var posortowane = produktyDlaSklepu
                .OrderBy(x => x.nazwaKategorii)
                .ThenBy(x => x.produkt.CzyKupiony)
                .ToList();

            string ostatniaKategoria = "";

            foreach (var item in posortowane)
            {
                if (item.nazwaKategorii != ostatniaKategoria)
                {
                    var naglowek = new Label
                    {
                        Text = item.nazwaKategorii,
                        FontSize = 18,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(5, 10, 5, 5),
                        TextColor = Colors.DarkGreen
                    };

                    ListaProduktow.Children.Add(naglowek);
                    ostatniaKategoria = item.nazwaKategorii;
                }

                var produktView = new ProduktView
                {
                    Produkt = item.produkt
                };

                produktView.ProduktZmieniony += async (s, p) =>
                {
                    await plikSerwis.ZapiszKategorieAsync(kategorie);
                    OdswiezListe();
                };

                produktView.ProduktUsuniety += async (s, p) =>
                {
                    var kat = kategorie.FirstOrDefault(k => k.Id == p.IdKategorii);
                    if (kat != null)
                    {
                        kat.Produkty.Remove(p);
                        await plikSerwis.ZapiszKategorieAsync(kategorie);
                        OdswiezListe();
                    }
                };

                ListaProduktow.Children.Add(produktView);
            }

            if (posortowane.Count == 0)
            {
                ListaProduktow.Children.Add(new Label
                {
                    Text = $"Brak produktów dla sklepu: {nazwaSklep}",
                    FontSize = 18,
                    Margin = new Thickness(20)
                });
            }
        }

        private async void OnPowrotClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
