using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class PlikSerwis
    {
        private string sciezkaPliku;

        public PlikSerwis()
        {
            var folderDanych = FileSystem.AppDataDirectory;
            sciezkaPliku = Path.Combine(folderDanych, "lista_zakupow.json");
        }

        public async Task ZapiszKategorieAsync(ObservableCollection<Kategoria> kategorie)
        {
            try
            {
                var json = JsonSerializer.Serialize(kategorie, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(sciezkaPliku, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Blad zapisu: " + ex.Message);
            }
        }

        public async Task<ObservableCollection<Kategoria>> WczytajKategorieAsync()
        {
            try
            {
                if (File.Exists(sciezkaPliku))
                {
                    var json = await File.ReadAllTextAsync(sciezkaPliku);
                    var kategorie = JsonSerializer.Deserialize<ObservableCollection<Kategoria>>(json);
                    return kategorie ?? new ObservableCollection<Kategoria>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Blad odczytu: " + ex.Message);
            }

            return new ObservableCollection<Kategoria>();
        }

        public async Task EksportujDoPliku(ObservableCollection<Kategoria> kategorie, string sciezkaDoCelu)
        {
            try
            {
                var json = JsonSerializer.Serialize(kategorie, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(sciezkaDoCelu, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Blad eksportu: " + ex.Message);
            }
        }

        public async Task<ObservableCollection<Kategoria>> ImportujZPliku(string sciezkaZPliku)
        {
            try
            {
                if (File.Exists(sciezkaZPliku))
                {
                    var json = await File.ReadAllTextAsync(sciezkaZPliku);
                    var kategorie = JsonSerializer.Deserialize<ObservableCollection<Kategoria>>(json);
                    return kategorie ?? new ObservableCollection<Kategoria>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Blad importu: " + ex.Message);
            }

            return new ObservableCollection<Kategoria>();
        }
    }
}
