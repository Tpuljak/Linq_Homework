using System;
using System.Collections.Generic;
using System.Linq;

namespace Warmup
{
    class MainClass
    {
        public static bool IsPrime (int number) {
            if (number == 1 || number == 0) return false;
            for (int i = 2; i <= number / 2.0; i++) {
                if (number % i == 0) return false;
            }
            return true;
        }

        public static List<int> GetPrimes (int n) {
            var OneToN = Enumerable.Range(1, n);

            return OneToN.Where(number => IsPrime(number)).ToList();
        }

        public static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            GetPrimes(n).ForEach(number => Console.WriteLine(number));
        }
    }
}
