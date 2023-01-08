using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Popunjavanje(long[] Rezultati, string[] Resenja)
        {
            string[] probna;
            Rezultati[0] = 1; Rezultati[1] = 0; Rezultati[2] = 1;
            for (long p = 3; p <= Resenja.Length; p++)
            {
                probna = Resenja[p].Split(new string[] { ": " }, StringSplitOptions.None);
                Rezultati[p] = long.Parse(probna[1]);
            }
        }
        static BigInteger Faktorijal(long N)//N!
        {
            if (N == 0) { return 1; }
            else if (N == 1) { return 1; }
            BigInteger U = new BigInteger(1);
            for (long a = N; a >= 2; a--)
            {
                U *= a;
            }
            return U;
        }
        static BigInteger nCx(long n, long x)//nCx
        {
            BigInteger dole = Faktorijal(x) * Faktorijal(n - x);
            return Faktorijal(n) / dole;
        }
        static BigInteger KombIsteOsobe(BigInteger[] Rezultati, long p, long i)//i je u prvoj iteriaciji UVEK p, rekurzivna func
        {
            if (i == 1)
            {
                return p * Rezultati[p - i];
            }
            return nCx(p, i) * Rezultati[p - i] + KombIsteOsobe(Rezultati, p, i - 1);
        }
        static BigInteger Kombinacije(BigInteger[] Rezultati, long p)//Ukupne kombinacije
        {
            if (p == 0 || p == 2) { return 1; }
            else if (p == 1) { return 0; }
            return Faktorijal(p) - KombIsteOsobe(Rezultati, p, p);
        }
        static void ProcenatInt(string U, StreamWriter procenat, long nastavak, long i)
        {
            if (U == "100000") { procenat.WriteLine((nastavak + i) + ": 100%"); return; }
            else if (U == "0") { procenat.WriteLine((nastavak + i) + ": 0%"); return; }
            try
            {
                procenat.WriteLine((nastavak + i) + ": " + U[0] + U[1] + "," + U[2] + U[3] + "%");
            }
            catch (Exception)
            {
                try { procenat.WriteLine((nastavak + i) + ": " + U[0] + U[1] + "," + U[2] + "%"); }
                catch (Exception)
                {
                    try { procenat.WriteLine((nastavak + i) + ": " + U[0] + U[1] + "%"); }
                    catch (Exception) { Console.WriteLine("GREŠKA"); }
                }
            }
        }
        static void Main(string[] args)
        {
            BigInteger[] Rezultati = new BigInteger[1000000];
            var Resenja = File.ReadAllLines(@"Rezultati.txt");//Uzima ostala rešenja
            using (StreamWriter izlaz = new StreamWriter(@"Rezultati.txt"))//Ukljucen izlaz
            {
                using (StreamWriter procenat = new StreamWriter(@"Procenti.txt"))
                {
                    string[] probna;
                    foreach (string Line in Resenja)//Piše ostala rešenja
                    {
                        izlaz.WriteLine(Line);
                    }
                    for (long z = 0; z < Resenja.Length; z++)//Piše rezultate
                    {
                        probna = Resenja[z].Split(new string[] { ": " }, StringSplitOptions.None);
                        Rezultati[z] = long.Parse(probna[1]);
                    }
                    //Pocetak
                    long N = long.Parse(Console.ReadLine());
                    long nastavak = Resenja.Length;
                    string U;
                    for (long i = 0; i < N; i++)
                    {
                        Rezultati[nastavak + i] = Kombinacije(Rezultati, nastavak + i);
                        izlaz.WriteLine("Kombinacije za " + (nastavak + i) + " osobe: " + Rezultati[nastavak + i]);
                        U = (Rezultati[nastavak + i] * 100000 / Faktorijal(nastavak + i)).ToString();
                        ProcenatInt(U, procenat, nastavak, i);//namerno prvo mnozim da ne ide prvo u decimale (ovo su veliki brojevi)
                        Console.WriteLine((i + 1) + "/" + N + " ZAVRŠENO");
                    }
                }
            }//kraj
            Console.WriteLine("Završeno!");
            Console.WriteLine();
            Resenja = File.ReadAllLines(@"Rezultati.txt");
            using (StreamWriter backup = new StreamWriter(@"Backup.txt")) { foreach (string Line in Resenja) { backup.WriteLine(Line); } }//Zapisuje rezultate direktno u backup file (ako se ne desi crash)
            Console.ReadLine();
        }
    }
}
