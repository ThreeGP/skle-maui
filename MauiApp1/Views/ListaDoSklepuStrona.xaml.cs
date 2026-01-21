using MauiApp1.Models;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiApp1.Views
{
    public partial class ListaDoSklepuStrona : ContentPage
    {
        private ObservableCollection<Kategoria> kategorie;
        private PlikSerwis plikSerwis;

        public ListaDoSklepuStrona(ObservableCollection<Kategoria> kat, PlikSerwis serwis)
        {
            InitializeComponent();
            kategorie = kat;
            plikSerwis = serwis;
            OdswiezListe();
        }

        private void OdswiezListe()
        {
            ListaProduktow.Children.Clear();

            // zebrac wszystkie niekupione produkty
            var wszystkieProdukty = new List<(Produkt produkt, string nazwaKategorii)>();

            foreach (var kategoria in kategorie)
            {
                foreach (var produkt in kategoria.Produkty)
                {
                    if (!produkt.CzyKupiony)
                    {
                        wszystkieProdukty.Add((produkt, kategoria.Nazwa));
                    }
                }
            }

            // sortowac po nazwie kategorii
            var posortowane = wszystkieProdukty.OrderBy(x => x.nazwaKategorii).ToList();

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
                        TextColor = Colors.DarkBlue
                    };
                    ListaProduktow.Children.Add(naglowek);
                    ostatniaKategoria = item.nazwaKategorii;
                }

                var produktView = new ProduktView { Produkt = item.produkt };
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
                    Text = "Brak produktow do kupienia!",
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
