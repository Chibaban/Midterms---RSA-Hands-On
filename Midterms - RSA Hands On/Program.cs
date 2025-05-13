using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midterms___RSA_Hands_On
{
    internal class Program
    {
        // List of prime numbers between 0 and 255
        static List<int> primes = new List<int> {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29,
            31, 37, 41, 43, 47, 53, 59, 61, 67,
            71, 73, 79, 83, 89, 97, 101, 103, 107,
            109, 113, 127, 131, 137, 139, 149, 151,
            157, 163, 167, 173, 179, 181, 191, 193,
            197, 199, 211, 223, 227, 229, 233, 239,
            241, 251
        };

        static void Main(string[] args)
        {
            Random rand = new Random();

            // Step 1: Pick two different random primes
            int p = primes[rand.Next(primes.Count)];
            int q = 0;
            do { 
                q = primes[rand.Next(primes.Count)]; 
            } while (q == p);

            // Step 2: Compute n and t
            int n = p * q;
            int t = (p - 1) * (q - 1);

            // Step 3: Choose public exponent e (must be coprime with t)
            int e = 0;
            do { 
                e = rand.Next(2, t); 
            } while (GCD(e, t) != 1);

            // Step 4: Compute private key d (modular inverse of e mod phi)
            int d = ModInverse(e, t);

            // Show the generated keys
            Console.WriteLine("RSA Key Details:\n");
            Console.WriteLine($"- Modulus (n): {n}");
            Console.WriteLine($"- Public exponent (e): {e}");
            Console.WriteLine($"- Private exponent (d): {d}\n");

            // Step 5: Get message from user
            int message = 0;
            while (true)
            {
                Console.Write("Enter a message (integer less than n): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out message))
                {
                    if (message < n)
                        break;
                    Console.WriteLine("Message must be less than n.\n");
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a number.\n");
                }
            }

            // Step 6: Encrypt and Decrypt
            int encrypted = ModPow(message, e, n);
            int decrypted = ModPow(encrypted, d, n);

            Console.WriteLine($"\nEncrypted message: {encrypted}");
            Console.WriteLine($"Decrypted message: {decrypted}");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // Greatest Common Divisor
        static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        // Modular inverse using Extended Euclidean Algorithm
        static int ModInverse(int a, int m)
        {
            int m0 = m, x0 = 0, x1 = 1;

            while (a > 1)
            {
                int q = a / m;
                (a, m) = (m, a % m);
                (x0, x1) = (x1 - q * x0, x0);
            }

            return x1 < 0 ? x1 + m0 : x1;
        }

        // Modular exponentiation: (base^exponent) % mod
        static int ModPow(int baseNum, int exponent, int mod)
        {
            int result = 1;
            baseNum %= mod;

            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                    result = (result * baseNum) % mod;

                exponent >>= 1;
                baseNum = (baseNum * baseNum) % mod;
            }

            return result;
        }
    }
}
