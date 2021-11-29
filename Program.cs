using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AlgorytmyPlecakowe {
    class ProblemPlecakowy {
        static int potega(int a, int b) {
            int wynik = 1;
            for (int i = 0; i < b; i++) {
                wynik *= a;
            }
            return wynik;
        }
        static int max(int a, int b) {
            if (a > b) {
                return a;
            }
            return b;
        }
        public struct Przedmiot {
            public int waga;
            public int wartosc;
        }
        public int pojemnoscPlecaka;
        public List<Przedmiot> przedmioty;
        public ProblemPlecakowy(int pojemnoscPlecaka) {
            this.pojemnoscPlecaka = pojemnoscPlecaka;
            przedmioty = new List<Przedmiot>();
        }
        public void DodajPrzedmiot(int waga, int wartosc) {
            Przedmiot przedmiot = new Przedmiot();
            przedmiot.waga = waga;
            przedmiot.wartosc = wartosc;
            przedmioty.Add(przedmiot);
        }
        public List<bool> algorytmSilowy() {
            List<bool> wynik = new List<bool>();
            int najwiekszaWartosc = 0;
            int rozwiazanie = 0;
            int m = potega(2, przedmioty.Count);
            for (int i = 0; i < m; i++) {
                int b = i;
                int waga = 0;
                int wartosc = 0;
                for (int j = przedmioty.Count - 1; j >= 0; j--) {
                    if (b % 2 == 1) {
                        waga += przedmioty[j].waga;
                        wartosc += przedmioty[j].wartosc;
                    }
                    b /= 2;
                }
                if (waga <= pojemnoscPlecaka && wartosc >= najwiekszaWartosc) {
                    najwiekszaWartosc = wartosc;
                    rozwiazanie = i;
                }
            }
            for (int i = przedmioty.Count - 1; i >= 0; i--) {
                if (rozwiazanie % 2 == 1) {
                    wynik.Insert(0, true);
                }
                else {
                    wynik.Insert(0, false);
                }
                rozwiazanie /= 2;
            }
            return wynik;
        }
        public List<bool> algorytmRekurencyjny() {
            int wartosc = 0;
            return krokRekurencyjny(0, pojemnoscPlecaka, ref wartosc);
        }
        protected List<bool> krokRekurencyjny(int element, int pozostalaPojemnosc, ref int wartosc) {
            if (element >= przedmioty.Count) {
                wartosc = 0;
                return new List<bool>();
            }
            if (przedmioty[element].waga > pozostalaPojemnosc) {
                List<bool> wynik = krokRekurencyjny(element + 1, pozostalaPojemnosc, ref wartosc);
                wynik.Insert(0, false);
                return wynik;
            }
            int wartosc1 = 0, wartosc2 = 0;
            List<bool> wynik1 = krokRekurencyjny(element + 1, pozostalaPojemnosc, ref wartosc1);
            List<bool> wynik2 = krokRekurencyjny(element + 1, pozostalaPojemnosc - przedmioty[element].waga, ref wartosc2);
            wartosc2 += przedmioty[element].wartosc;
            if (wartosc1 > wartosc2) {
                wynik1.Insert(0, false);
                wartosc = wartosc1;
                return wynik1;
            }
            wynik2.Insert(0, true);
            wartosc = wartosc2;
            return wynik2;
        }
        public List<bool> algorytmPD() {
            List<bool> wynik = new List<bool>();
            int[,] v = new int[przedmioty.Count + 1, pojemnoscPlecaka + 1];
            for (int i = 0; i <= przedmioty.Count; i++) {
                for (int j = 0; j <= pojemnoscPlecaka; j++) {
                    if (i == 0 || j == 0) {
                        v[i, j] = 0;
                        continue;
                    }
                    if (j < przedmioty[i - 1].waga) {
                        v[i, j] = v[i - 1, j];
                        continue;
                    }
                    v[i, j] = max(v[i - 1, j], v[i - 1, j - przedmioty[i - 1].waga] + przedmioty[i - 1].wartosc);
                }
            }
            for(int i = przedmioty.Count, j = pojemnoscPlecaka; i > 0; i--) {
                if (v[i, j] == v[i - 1, j]) {
                    wynik.Insert(0, false);
                    continue;
                }
                wynik.Insert(0, true);
                j -= przedmioty[i - 1].waga;
            }
            return wynik;
        }
    }
    class Program {
        static void Main(string[] args) {
            Random random = new Random();
            Stopwatch stopwatch = new Stopwatch();
            for (int i = 1; i <= 10; i++) {
                Console.Write(i + "\n");
                for(int t = 0; t < 10; t++) {
                    ProblemPlecakowy problem = new ProblemPlecakowy(i * 100000);
                    for(int j = 0; j < 14 + i; j++) {
                        problem.DodajPrzedmiot(random.Next(1, i * 2000), random.Next(1, i * 2000));
                    }
                    stopwatch.Restart();
                    problem.algorytmSilowy();
                    Console.Write(stopwatch.ElapsedMilliseconds + " ");
                    stopwatch.Restart();
                    problem.algorytmRekurencyjny();
                    Console.Write(stopwatch.ElapsedMilliseconds + " ");
                    stopwatch.Restart();
                    problem.algorytmPD();
                    Console.Write(stopwatch.ElapsedMilliseconds + "\n");
                }
            }
        }
    }
}
