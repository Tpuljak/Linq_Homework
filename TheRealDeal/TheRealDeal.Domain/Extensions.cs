using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRealDeal.Domain
{
    public static class Extensions
    {
        public static List<double> ConvertToDouble(this IEnumerable<double> list)
        {
            return list
                .Select(number => number.ToString().Contains('.') && number.ToString().Split('.')[1].Length > 2 ? double.Parse(number.ToString().Replace(".", "")) : number)
                .ToList();

        }
    }
}
