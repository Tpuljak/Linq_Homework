using System;
using System.Collections.Generic;
using System.Linq;

namespace Warmup
{
    class MainClass
    {
        public static bool IsPrime(int number)
        {
            if (number == 1 || number == 0) return false;
            for (int i = 2; i <= number / 2.0; i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        public static List<int> GetPrimes(int n)
        {
            var OneToN = Enumerable.Range(1, n);

            return OneToN.Where(number => IsPrime(number)).ToList();
        }

        public static int NumberOfDivisors(int number) {
            var OneToN = Enumerable.Range(1, number);

            return OneToN.Where(num => number % num == 0).ToList().Count;
        }

        public static int GetWeakness(int number) {
			var OneToN = Enumerable.Range(1, number);

            return OneToN.Where(num => NumberOfDivisors(num) > NumberOfDivisors(number)).ToList().Count;
		}

        public static int[] WeakNumbers(int number) {
			var OneToN = Enumerable.Range(1, number);
            OneToN = OneToN.Select(num => GetWeakness(num)).ToList();

            var result = new int[] { 0, 0 };

            result[0] = OneToN.Max();
            result[1] = OneToN.Count(num => num == result[0]);

            return result;
        }

        public static void Main(string[] args)
        {
            int selection;

            do
            {
                Console.WriteLine("Izaberi zadatak (1 ili 2):");
                Console.WriteLine("1 => Prosti brojevi 1-n");
                Console.WriteLine("2 => Codewars");
                selection = int.Parse(Console.ReadLine());
            } while (selection != 1 && selection != 2);

			int n = int.Parse(Console.ReadLine());

			switch (selection)
            {
                case 1:
					GetPrimes(n).ForEach(Console.WriteLine);
                    break;
                case 2:
                    WeakNumbers(n).ToList().ForEach(Console.WriteLine);
                    break;
            }
        }
    }
}
