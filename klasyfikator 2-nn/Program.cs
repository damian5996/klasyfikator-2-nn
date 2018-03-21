using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace klasyfikator_2_nn
{
    class Program
    {
        static string TablicaDoString<T>(T[][] tab)
        {
            string wynik = "";
            for (int i = 0; i < tab.Length; i++)
            {
                for (int j = 0; j < tab[i].Length; j++)
                {
                    wynik += tab[i][j].ToString() + " ";
                }
                wynik = wynik.Trim() + Environment.NewLine;
            }

            return wynik;
        }

        static double StringToDouble(string liczba)
        {
            double wynik; liczba = liczba.Trim();
            if (!double.TryParse(liczba.Replace(',', '.'), out wynik) && !double.TryParse(liczba.Replace('.', ','), out wynik))
                throw new Exception("Nie udało się skonwertować liczby do double");

            return wynik;
        }


        static int StringToInt(string liczba)
        {
            int wynik;
            if (!int.TryParse(liczba.Trim(), out wynik))
                throw new Exception("Nie udało się skonwertować liczby do int");

            return wynik;
        }

        static string[][] StringToTablica(string sciezkaDoPliku)
        {
            string trescPliku = System.IO.File.ReadAllText(sciezkaDoPliku); // wczytujemy treść pliku do zmiennej
            string[] wiersze = trescPliku.Trim().Split(new char[] { '\n' }); // treść pliku dzielimy wg znaku końca linii, dzięki czemu otrzymamy każdy wiersz w oddzielnej komórce tablicy
            string[][] wczytaneDane = new string[wiersze.Length][];   // Tworzymy zmienną, która będzie przechowywała wczytane dane. Tablica będzie miała tyle wierszy ile wierszy było z wczytanego poliku

            for (int i = 0; i < wiersze.Length; i++)
            {
                string wiersz = wiersze[i].Trim();     // przypisuję i-ty element tablicy do zmiennej wiersz
                string[] cyfry = wiersz.Split(new char[] { ' ' });   // dzielimy wiersz po znaku spacji, dzięki czemu otrzymamy tablicę cyfry, w której każda oddzielna komórka to czyfra z wiersza
                wczytaneDane[i] = new string[cyfry.Length];    // Do tablicy w której będą dane finalne dokładamy wiersz w postaci tablicy integerów tak długą jak długa jest tablica cyfry, czyli tyle ile było cyfr w jednym wierszu
                for (int j = 0; j < cyfry.Length; j++)
                {
                    string cyfra = cyfry[j].Trim(); // przypisuję j-tą cyfrę do zmiennej cyfra
                    wczytaneDane[i][j] = cyfra;
                }
            }
            return wczytaneDane;
        }

        static void Main(string[] args)
        {
            string sciezkaDoSystemuTestowego = @"SystemTestowy.txt";
            string sciezkaDoSystemuTreningowego = @"SystemTreningowy.txt";

            string[][] systemTestowy = StringToTablica(sciezkaDoSystemuTestowego);
            string[][] systemTreningowy = StringToTablica(sciezkaDoSystemuTreningowego);

            Console.WriteLine("Dane systemu testowego");
            string wynikTestowy = TablicaDoString(systemTestowy);
            Console.Write(wynikTestowy);

            Console.WriteLine("");
            Console.WriteLine("Dane systemu treningowego");

            string wynikTreningowy = TablicaDoString(systemTreningowy);
            Console.Write(wynikTreningowy);

            string liczba;
            // Przykład konwertowania string do double
            liczba = "1.4";
            double dliczba = StringToDouble(liczba);


            // Przykład konwertowania string do int
            liczba = "1";
            int iLiczba = StringToInt(liczba);

            /****************** Miejsce na rozwiązanie *********************************/
            while (true)
            {


                double obiektPoprawnieSklasyfikowany1 = 0;
                double obiektPoprawnieSklasyfikowany2 = 0;
                double chwycony1 = 0;
                double chwycony2 = 0;
                double acc1 = 0;
                double acc2 = 0;
                double cov1 = 0;
                double cov2 = 0;
                double liczbaObiektow1 = 0;
                double liczbaObiektow2 = 0;
                double accGlobal = 0;
                double covGlobal = 0;
                double blednieSklasyfikowany1 = 0;
                double blednieSklasyfikowany2 = 0;
                double TPR1 = 0;
                double TPR2 = 0;

                List<string> klasy = new List<string>();
                List<string> klasyPowtorzenia = new List<string>();
                List<string> klasyPowtorzeniaSystemTestowy = new List<string>();
                Console.WriteLine("\nWybierz numer metryki, której chcesz zobaczyć wyniki :)");
                Console.WriteLine("1. Metryka Euklidesowa");
                Console.WriteLine("2. Metryka Manhattan");
                Console.WriteLine("3. Metryka Canberra");
                Console.WriteLine("4. Metryka Czebyszewa");
                Console.WriteLine("5. Wyjście");

                int metryka = Convert.ToInt32(Console.ReadLine());

                for (int wierszY = 0; wierszY < systemTreningowy.Length; wierszY++)
                {
                    for (int atrybutY = systemTreningowy[wierszY].Length - 1;
                        atrybutY < systemTreningowy[wierszY].Length;
                        atrybutY++)
                    {
                        klasyPowtorzenia.Add(systemTreningowy[wierszY][atrybutY]);
                        if (!klasy.Contains(systemTreningowy[wierszY][atrybutY]))
                        {
                            klasy.Add(systemTreningowy[wierszY][atrybutY]);
                        }
                    }
                }
                for (int wierszY = 0; wierszY < systemTestowy.Length; wierszY++)
                {
                    for (int atrybutY = systemTestowy[wierszY].Length - 1;
                        atrybutY < systemTestowy[wierszY].Length;
                        atrybutY++)
                    {
                        klasyPowtorzeniaSystemTestowy.Add(systemTestowy[wierszY][atrybutY]);
                    }
                }

                for (int wierszX = 0; wierszX < systemTestowy.Length; wierszX++)
                {
                    string c = klasyPowtorzeniaSystemTestowy[wierszX];
                    List<double> mocKlas = new List<double>();
                    List<double> listaDecyzje = new List<double>();
                    Console.WriteLine("\n\nDla x{0}, ", wierszX + 1);
                    Console.WriteLine($"c = {c}");
                    for (int wierszY = 0; wierszY < systemTreningowy.Length; wierszY++)
                    {
                        double decyzja = 0;
                        List<double> decyzjeCzeryszewa = new List<double>();
                        for (int atrybutXiY = 0; atrybutXiY < systemTreningowy[wierszY].Length; atrybutXiY++)
                        {
                            if (atrybutXiY < systemTreningowy[wierszY].Length - 1)
                            {
                                switch (metryka)
                                {
                                    case 1:
                                        decyzja += Math.Pow(
                                        StringToDouble(systemTestowy[wierszX][atrybutXiY]) -
                                        StringToDouble(systemTreningowy[wierszY][atrybutXiY]), 2);
                                        break;

                                    case 2:
                                        decyzja += Math.Abs(
                                            StringToDouble(systemTestowy[wierszX][atrybutXiY]) -
                                            StringToDouble(systemTreningowy[wierszY][atrybutXiY]));
                                        break;
                                    case 3:
                                        decyzja += Math.Abs(
                                            (StringToDouble(systemTestowy[wierszX][atrybutXiY]) -
                                            StringToDouble(systemTreningowy[wierszY][atrybutXiY])) /
                                            (StringToDouble(systemTestowy[wierszX][atrybutXiY]) +
                                            StringToDouble(systemTreningowy[wierszY][atrybutXiY])));
                                        break;
                                    case 4:
                                        decyzjeCzeryszewa.Add(Math.Abs(
                                            StringToDouble(systemTestowy[wierszX][atrybutXiY]) -
                                            StringToDouble(systemTreningowy[wierszY][atrybutXiY])));
                                        break;
                                    case 5:
                                        Environment.Exit(0);
                                        break;
                                }

                            }
                        }
                        switch (metryka)
                        {
                            case 1:
                                decyzja = Math.Sqrt(decyzja);
                                break;
                            case 4:
                                decyzja = decyzjeCzeryszewa.Max();
                                break;
                        }
                        listaDecyzje.Add(decyzja);
                        Console.WriteLine("d(x{0},y{1}) = {2}", wierszX + 1, wierszY + 1, decyzja);
                    }

                    foreach (string klasa in klasy)
                    {
                        List<int> indeksyKlas = new List<int>();
                        List<double> decyzjeWKlasie = new List<double>();
                        for (int i = 0; i < klasyPowtorzenia.Count; i++)
                        {
                            if (klasyPowtorzenia[i] == klasa)
                            {
                                indeksyKlas.Add(i);
                            }
                        }
                        foreach (int indeks in indeksyKlas)
                            decyzjeWKlasie.Add(listaDecyzje[indeks]);

                        double najmniejsza = decyzjeWKlasie.Min();
                        decyzjeWKlasie.Remove(najmniejsza);
                        double najmniejsza2 = decyzjeWKlasie.Min();
                        mocKlas.Add(najmniejsza + najmniejsza2);
                        Console.WriteLine($"Klasa {klasa} głosuje z mocą {najmniejsza} + {najmniejsza2} = {najmniejsza + najmniejsza2}");
                    }

                    for (int i = 0; i < mocKlas.Count; i++)
                    {
                        if (i + 1 < mocKlas.Count)
                        {
                            if (mocKlas[i] < mocKlas[i + 1])
                            {
                                Console.WriteLine($"{mocKlas[i]} < {mocKlas[i + 1]}");
                                Console.Write($"Obiekt x{wierszX + 1} otrzymuje decyzję {klasy[i]}, ");
                                if (klasy[i] == c)
                                {
                                    Console.WriteLine("jest poprawnie sklasyfikowany.");
                                    obiektPoprawnieSklasyfikowany1++;
                                    chwycony1++;
                                }
                                else
                                {
                                    Console.WriteLine("jest błędnie sklasyfikowany.");
                                    chwycony2++;
                                    blednieSklasyfikowany2++;
                                }

                            }
                            else if (mocKlas[i + 1] < mocKlas[i])
                            {
                                Console.WriteLine($"{mocKlas[i + 1]} < {mocKlas[i]}");
                                Console.Write($"Obiekt x{wierszX + 1} otrzymuje decyzję {klasy[i + 1]}, ");
                                if (klasy[i + 1] == c)
                                {
                                    Console.WriteLine("jest poprawnie sklasyfikowany.");
                                    obiektPoprawnieSklasyfikowany2++;
                                    chwycony2++;
                                }
                                else
                                {
                                    Console.WriteLine("jest błędnie sklasyfikowany.");
                                    chwycony1++;
                                    blednieSklasyfikowany1++;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"{mocKlas[i]} = {mocKlas[i + 1]}");
                                Console.WriteLine($"Obiekt x{wierszX + 1} nie jest chwytany");
                            }
                            if (wierszX == 0)
                            {
                                foreach (string klasa in klasyPowtorzeniaSystemTestowy)
                                {
                                    if (klasa == klasy[i])
                                        liczbaObiektow1++;
                                    if (klasa == klasy[i + 1])
                                        liczbaObiektow2++;
                                }
                            }
                        }
                    }
                }
                acc1 = obiektPoprawnieSklasyfikowany1 / chwycony1;
                acc2 = obiektPoprawnieSklasyfikowany2 / chwycony2;
                accGlobal = (obiektPoprawnieSklasyfikowany1 + obiektPoprawnieSklasyfikowany2) / (chwycony1 + chwycony2);
                cov1 = chwycony1 / liczbaObiektow1;
                cov2 = chwycony2 / liczbaObiektow2;
                covGlobal = (chwycony1 + chwycony2) / (liczbaObiektow1 + liczbaObiektow2);
                TPR1 = obiektPoprawnieSklasyfikowany1 / (obiektPoprawnieSklasyfikowany1 + blednieSklasyfikowany2);
                TPR2 = obiektPoprawnieSklasyfikowany2 / (obiektPoprawnieSklasyfikowany2 + blednieSklasyfikowany1);
                Console.WriteLine("\n=====================================================================");
                Console.WriteLine("{0,5} {1,5} {2,5} {3,16} {4,16} {5,16}", " ", klasy[0], klasy[1], "No. of obj.", "Accuracy", "Coverage");
                Console.WriteLine("{0,5} {1,5} {2,5} {3,16} {4,16} {5,16}", klasy[0], obiektPoprawnieSklasyfikowany1, blednieSklasyfikowany1, liczbaObiektow1, Math.Round(acc1, 2), Math.Round(cov1, 2));
                Console.WriteLine("{0,5} {1,5} {2,5} {3,16} {4,16} {5,16}", klasy[1], blednieSklasyfikowany2, obiektPoprawnieSklasyfikowany2, liczbaObiektow2, Math.Round(acc2, 2), Math.Round(cov2, 2));
                Console.WriteLine("---------------------------------------------------------------------");

                Console.WriteLine("{0,5} {1,5} {2,5} {3,16} {4,16} {5,16}", "TPR", Math.Round(TPR1, 2), Math.Round(TPR2, 2), " ", Math.Round(accGlobal, 2), Math.Round(covGlobal, 2));

                Console.WriteLine("=====================================================================");
            }
            /****************** Koniec miejsca na rozwiązanie ********************************/
            //Console.ReadKey();
        }
    }
}

