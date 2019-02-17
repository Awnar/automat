using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace automat
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            int n;
            do
            {
                Console.WriteLine("Podaj ilość stanów");
                input = Console.ReadLine();
                n = int.Parse(input);
            } while (n < 1);

            int fs;
            do
            {
                Console.WriteLine("Podaj ilość stanów akceptujących (końcowych)");
                input = Console.ReadLine();
                fs = int.Parse(input);
            } while (fs > n || fs < 1);

            int al;
            do
            {
                Console.WriteLine("Podaj ilość znaków w alfabecie");
                input = Console.ReadLine();
                al = int.Parse(input);
            } while (al < 1);

            double X;
            do
            {
                Console.WriteLine("Szansa na przejście do innego stanu, 1>=X>=0");
                input = Console.ReadLine();
                X = double.Parse(input);
            } while (0 > X || X > 1);

            //tablica z stanami przejść 
            int[,] tab = new int[n, al + 1];

            Random rand = new Random();

            //losowanie stanów końcowych
            int f;
            do
            {
                f = 0;
                for (int i = 0; i < n; i++)
                {
                    f += tab[i, 0] = rand.Next(2);
                }
            } while (f != fs);

            //losowanie przejść; liczba większa od n oznacza brak przejścia
            for (int i = 1; i < al + 1; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (rand.NextDouble() <= X)
                        tab[j, i] = rand.Next(n);
                    else tab[j, i] = n + 1;
                }
            }

            //wyświetlanie tabeli
            Console.Write("\nStany\tfinal\t");
            for (int i = 'a'; i < 'a' + al; i++)
                Console.Write((char) i + "\t");
            Console.WriteLine();
            for (int i = 0; i < n; i++)
            {
                Console.Write("q" + i + "\t" + tab[i, 0] + "\t");
                for (int j = 1; j < al + 1; j++)
                {
                    if (tab[i, j] < n)
                        Console.Write("q" + tab[i, j] + "\t");
                    else
                        Console.Write("-\t");
                }

                Console.WriteLine();
            }

            //generowanie słów zgodnie z automatem 
            Console.WriteLine("Ile słów wygenerować?");
            input = Console.ReadLine();
            var s = int.Parse(input);

            string[] slowa = new string[s];

            //generowanie słów odbywa się poprzez wylosowanie stanu końcowego i losowanie potencjalnych stanów pośrednich (zgodnie z przejściami w automacie) aż do osiągnięcia stanu startowego (q0)
            for (int i = 0; i < s; i++)
            {
                var start = 0;
                do
                {
                    //losowanie stanu końcowego
                    start = rand.Next(n);
                } while (tab[start, 0] != 1);

                do
                {
                    var next = 0;
                    bool t = true;
                    int tmp = 0;
                    do
                    {
                        //losowanie znaku z alfabetu i sprawdzanie czy jest jakieś przejście do obecnego stanu
                        next = rand.Next(al) + 1;
                        if (rand.Next(1) == 1)
                        {
                            for (int j = 0; j < n; j++)
                                if (tab[j, next] == start)
                                {
                                    start = j;
                                    t = false;
                                    break;
                                }
                        }
                        else
                        {
                            for (int j = n-1; j >= 0; j--)
                                if (tab[j, next] == start)
                                {
                                    start = j;
                                    t = false;
                                    break;
                                }
                        }

                        if (tmp++ > 2 * n)//jeśli nie wylosuje pasującego przejścia reset słowa
                        {
                            next = 0;
                            tmp = 0;
                        }
                    } while (t);

                    //uzupełnianie słowa
                    slowa[i] = (char) ('a' + next - 1) + slowa[i];
                } while (start != 0); //półki nie jest to stan 0 (startowy)
            }

            //wyświetlanie słów
            foreach (var item in slowa)
                Console.WriteLine(item);

            //oczekiwanie na zakończenie programu
            Console.ReadKey();
        }
    }
}