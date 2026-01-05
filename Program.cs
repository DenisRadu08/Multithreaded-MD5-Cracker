using System;
using System.Text;
using System.Security.Cryptography; // Aici se află uneltele pentru MD5
using System.Threading;

namespace GeneratorParole
{
    class Program
    {
        private const string alfabet = "abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
        private static volatile bool gasit = false;
        private static string Md5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes=Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }

        private static void Backtracking(int k, int lungime, char[] buffer, string targetHash)
        {
            if (gasit) return;
            if (k == lungime)
            {
                string candidat = new string(buffer);
                string hashCandidat=Md5Hash(candidat);
                if (hashCandidat == targetHash)
                {
                    Console.WriteLine("Am gasit parola: "+candidat);
                    gasit = true;
                }

                return;

            }

            for (int i = 0; i < alfabet.Length; i++)
            {
                if (gasit) break;
                buffer[k] = alfabet[i];
                Backtracking(k+1, lungime, buffer, targetHash);
            }
        }


        private static void Worker(int start, int stop, string targetHash, int lungime)
        {
            char[] buffer = new char[lungime];

            for (int i = start; i < stop; i++)
            {
                if (gasit) return;
                
                buffer[0]=alfabet[i];
                Backtracking(1,lungime,buffer,targetHash);
            }
        }
        
            
            
        public static void Main(string[] args)
        {
            Console.WriteLine("=== SPARGATOR DE PAROLE INTERACTIV ===");
            
            string targetHash1 = "56aed7e7485ff03d5605b885b86e947e";
            string targetHash2 = "e26026b73cdc3b59012c318ba26b5518";
            string targetHash3 = "9de37a0627c25684fdd519ca84073e34";
            string targetHashv1 = "6f8f57715090da2632453988d9a1501b"; //admin
            string targetHashv2 = "59417166c47f7e6b70d1a6701bf4efdd"; //motor
            
            Console.WriteLine("\nVrei sa introduci un Hash nou pentru spart? (In caz de nu, vei primi unul predefinit).");
            Console.WriteLine("'d' -> DA, 'n' -> NU: ");
            
            string raspuns=Console.ReadLine().ToLower();

            if (raspuns == "d")
            {
                Console.WriteLine("\nTe rog introdu Hash-ul MD5: ");
                string input=Console.ReadLine().Trim().ToLower();

                if (!string.IsNullOrEmpty(input))
                {
                    targetHashv2 = input;
                    Console.WriteLine("[INFO] Hash-ul a fost actualizat.");
                }
                else
                {
                    Console.WriteLine("[ATENTIE] Nu ai introdus nimic. Raman la hash-ul default");
                }
            }
            else
            {
                Console.WriteLine("\n[INFO] Se va folosi hash-ul default.");
            }
            
            Console.WriteLine("\nIntrodu lungimea parolei (ex: 4, 5, 6...): ");

            if (!int.TryParse(Console.ReadLine(), out int lungimeCautata))
            {
                lungimeCautata = 5;
                Console.WriteLine("Ai introdus gresit. Folosesc lungimea 5 (DEFAULT)");
            }
            
            Console.WriteLine("Introdu numarul de fire de executie (ex: 4, 8, 12): ");
            if (!int.TryParse(Console.ReadLine(), out int nThreads))
            {
                nThreads = 4;
                Console.WriteLine("Ai introdus gresit. Folosesc 4 fire. [DEFAULT]");
            }
            
            Thread[] threads = new Thread[nThreads];
            int cat=alfabet.Length/nThreads;
            gasit = false;
            
            Console.WriteLine($"\n[START] Pornesc {nThreads} fire pentru a sparge hash-ul...");
            
            var watch = System.Diagnostics.Stopwatch.StartNew();

            int startCurrent = 0;

            for (int i = 0; i < nThreads; i++)
            {
                int stopCurrent=startCurrent+cat;
                if (i == nThreads - 1)
                {
                    stopCurrent=alfabet.Length;
                }

                int inc = startCurrent;
                int fin = stopCurrent;
                
                threads[i] = new Thread(() => Worker(inc,fin,targetHashv2,lungimeCautata));
                threads[i].Start();
                
                startCurrent = stopCurrent;
            }

            for (int i = 0; i < nThreads; i++)
            {
                threads[i].Join();
            }
            
            watch.Stop();
            Console.WriteLine($"\n[STOP] Proces finalizat!");
            Console.WriteLine($"Timp scurs: {watch.ElapsedMilliseconds} ms ({watch.Elapsed.TotalSeconds:F2} secunde).");
            
            Console.WriteLine("\nApasa orice tasta pentru a inchide...");
            Console.ReadKey();
        }
    }
}