# Lista Zakupow - Aplikacja MAUI

## Opis
Aplikacja do zarzadzania lista zakupow napisana w .NET MAUI (.NET 9).

## Zrealizowane wymagania

### Ocena dopuszczajaca ?
- Aplikacja musi przechowywac dane na temat listy zakupow w formie plikow (nie .TXT) - ? JSON
- Aplikacja musi byc zgodna z tematem zadania tj. "Lista zakupow" - ?
- Projekt powinien byc podzielony na widoki i modele danych - ?
- Niedopuszczalny jest ponglisz (dopuszczalne sa teksty do wyswietlania w jezyku polskim) - ?
- Produkty powinny byc reprezentowane na interface za pomoca ContentView, ktory posiada nastepujace wlasciwosci:
  - Nazwa produktu - ?
  - Jednostka w jakiej mierzymy produkt (szt. l. kg. itp.) - ?
  - Mozliwosc zaznaczenia elementu kupionego. Taki element nie zostaje usuniety. Powinien on zostac w jakis sposob zaznaczony (wyszarzenie, skreslenie) i umieszczony na koncu listy kategorii - ?
  - Mozliwosc okreslenia ilosci poprzez wpisanie wartosci liczbowej z klawiatury lub klikniecie w "+" lub "-" aby zwiekszyc/zmniejszyc o jeden ilosc produktu - ?
  - Mozliwosc calkowitego usuniecia produktu - ?

### Ocena dostateczna ?
- Aplikacja ma dawac mozliwosc tworzenia nowych kategorii produktow (nabial, warzywa, elektronika, agd, etc.) kilka z nich moze byc predefiniowanych juz w aplikacji - ?
- Kazda kategoria powinna byc zaprezentowana w formie ContentView i miec mozliwosc rozwiniecia kategorii w celu zobaczenia produktow z tej kategorii - ?
- Kazdy produkt musi byc przypisany do jakiejs kategorii juz istniajacej - ?

### Ocena bardzo dobra ? (4 z 6 dodatkowych wymagan)

#### ? 1. Mozliwosc generowania/przelaczania widoku "Listy do sklepu"
W ktorej beda pokazane tylko produkty nieoznaczone jako kupione oraz bez podzialu na kategorie, ale po kategoriach posortowane. W momencie zaznaczenia elementu jako kupiony powinien on zniknac z tego widoku a na liscie z kategoriami powinien byc zaznaczony jako kupiony.
- **Status:** ZREALIZOWANE
- Widok: `ListaDoSklepuStrona.xaml`
- Funkcja: Filtrowanie niekupionych produktow, sortowanie po kategoriach, synchronizacja stanu

#### ? 2. Mozliwosc eksportu pliku do podzielnia sie lista zakupowa z innym uzytkownikiem
Funkcjonalnosc exportu i importu.
- **Status:** ZREALIZOWANE
- Funkcje: `EksportujListe()`, `ImportujListe()` w `GlownaStrona.xaml.cs`
- Format: JSON

#### ? 3. Mozliwosc oznaczenia sklepu w jakim dany produkt uzytkownik chce kupic
Biedronka, Selgros itp. i generowania widoku listy tylko dla tego konkretnego sklepu.
- **Status:** ZREALIZOWANE
- Widok: `ListaDlaSklepuStrona.xaml`
- Funkcja: Wybor sklepu dla produktu, filtrowanie po sklepie, generowanie listy dla sklepu

#### ? 4. Mozliwosc oznaczenia w czytelny sposob, ze dany produkt w liscie zakupowej jest opcjonalny
- **Status:** ZREALIZOWANE
- Pole: `CzyOpcjonalny` w modelu `Produkt`
- Wyswietlanie: Etykieta [OPCJONALNY] w `ProduktView`

#### ? 5. Zakladka z przepisami
W ktorej mozemy importowac z przepisu potrzebne skladniki do listy zakupow (Wraz z mozliwoscia dodawania nowych przepisow, i przynajmniej dwoma predefiniowanymi przepisami).
- **Status:** NIE ZREALIZOWANE

#### ? 6. Mozliwosc sortowania wyboru sortowania produktow
Po kategoriach (w przypadku list juz wygenerowanych do konkretnego sklepu lub listy produktow niekupionych), po nazwie, po ilosci.
- **Status:** NIE ZREALIZOWANE

## Struktura projektu

```
MauiApp1/
??? Models/
?   ??? Produkt.cs       - Model produktu
?   ??? Kategoria.cs        - Model kategorii
??? Services/
?   ??? PlikSerwis.cs       - Obsluga zapisu/odczytu JSON
??? Views/
? ??? ProduktView.xaml     - Widok pojedynczego produktu
?   ??? KategoriaView.xaml         - Widok kategorii z produktami
?   ??? GlownaStrona.xaml          - Glowna strona z kategoriami
?   ??? ListaDoSklepuStrona.xaml   - Lista niekupionych produktow
?   ??? ListaDlaSklepuStrona.xaml  - Lista dla konkretnego sklepu
??? AppShell.xaml    - Shell nawigacji
```

## Funkcjonalnosci

### Glowna strona
- Wyswietlanie wszystkich kategorii
- Dodawanie nowych kategorii
- Rozwijanie/zwijanie kategorii
- Menu z dodatkowymi opcjami

### Zarzadzanie produktami
- Dodawanie produktow do kategorii
- Ustawianie nazwy, jednostki, ilosci
- Wybor sklepu
- Oznaczanie jako opcjonalny
- Zaznaczanie jako kupiony
- Usuwanie produktow

### Dodatkowe widoki
- **Lista do Sklepu** - szybki przeglad produktow do kupienia
- **Lista dla Sklepu** - produkty filtrowane po wybranym sklepie

### Import/Export
- Eksport listy do pliku JSON
- Import listy z pliku JSON

## Jak uzywac

1. Uruchom aplikacje
2. Dodaj kategorie (lub uzyj predefiniowanych)
3. Rozwin kategorie i dodaj produkty
4. Ustaw parametry produktu (ilosc, sklep, opcjonalny)
5. Uzyj "Lista do Sklepu" aby zobaczyc co kupic
6. Zaznaczaj produkty jako kupione podczas zakupow
7. Uzyj Menu aby eksportowac/importowac liste

## Technologie
- .NET 9
- .NET MAUI
- System.Text.Json (zapis/odczyt danych)
