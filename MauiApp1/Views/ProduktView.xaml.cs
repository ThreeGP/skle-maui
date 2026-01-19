using MauiApp1.Models;
using System.ComponentModel;

namespace MauiApp1.Views
{
    public partial class ProduktView : ContentView, INotifyPropertyChanged
    {
        public event EventHandler<Produkt> ProduktUsuniety;
        public event EventHandler<Produkt> ProduktZmieniony;

        private Produkt produkt;

        public Produkt Produkt
        {
            get => produkt;
            set
            {
                produkt = value;
                OdswiezWyglad();
            }
        }

        public string Nazwa => Produkt?.Nazwa ?? "";
        public string Jednostka => Produkt?.Jednostka ?? "";
        public int Ilosc => Produkt?.Ilosc ?? 0;
        public bool CzyKupiony => Produkt?.CzyKupiony ?? false;
        public bool CzyOpcjonalny => Produkt?.CzyOpcjonalny ?? false;
        public string SlepInfo => "Sklep: " + (Produkt?.Sklep ?? "");
        public bool CzySklep => !string.IsNullOrEmpty(Produkt?.Sklep) && Produkt?.Sklep != "Dowolny";

        public Color KolorTla
        {
            get
            {
                if (CzyKupiony) return Colors.LightGray;
                return Colors.White;
            }
        }

        public TextDecorations Dekoracja
        {
            get
            {
                if (CzyKupiony) return TextDecorations.Strikethrough;
                return TextDecorations.None;
            }
        }

        public ProduktView()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void OnPlusClicked(object sender, EventArgs e)
        {
            if (Produkt != null)
            {
                Produkt.Ilosc++;
                OdswiezWyglad();
                ProduktZmieniony?.Invoke(this, Produkt);
            }
        }

        private void OnMinusClicked(object sender, EventArgs e)
        {
            if (Produkt != null && Produkt.Ilosc > 0)
            {
                Produkt.Ilosc--;
                OdswiezWyglad();
                ProduktZmieniony?.Invoke(this, Produkt);
            }
        }

        private void OnIloscChanged(object sender, TextChangedEventArgs e)
        {
            if (Produkt != null && int.TryParse(e.NewTextValue, out int nowaIlosc))
            {
                if (nowaIlosc >= 0)
                {
                    Produkt.Ilosc = nowaIlosc;
                    ProduktZmieniony?.Invoke(this, Produkt);
                }
            }
        }

        private void OnKupionyChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Produkt != null)
            {
                Produkt.CzyKupiony = e.Value;
                OdswiezWyglad();
                ProduktZmieniony?.Invoke(this, Produkt);
            }
        }

        private void OnUsunClicked(object sender, EventArgs e)
        {
            if (Produkt != null)
            {
                ProduktUsuniety?.Invoke(this, Produkt);
            }
        }

        private void OdswiezWyglad()
        {
            OnPropertyChanged(nameof(Nazwa));
            OnPropertyChanged(nameof(Jednostka));
            OnPropertyChanged(nameof(Ilosc));
            OnPropertyChanged(nameof(CzyKupiony));
            OnPropertyChanged(nameof(CzyOpcjonalny));
            OnPropertyChanged(nameof(SlepInfo));
            OnPropertyChanged(nameof(CzySklep));
            OnPropertyChanged(nameof(KolorTla));
            OnPropertyChanged(nameof(Dekoracja));
        }
    }
}
