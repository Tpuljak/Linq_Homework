using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRealDeal.Domain.Repositories;

namespace TheRealDeal
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationsRepository applicationsRepository = new ApplicationsRepository();

            int selection;

            Console.WriteLine("Izaberi task: (0 za izac)");
            Console.WriteLine("(1) Broj validnih koji nisu submitani");
            Console.WriteLine("(2) Zatrazene pare");
            Console.WriteLine("(3) Predvideni troskovi");
            Console.WriteLine("(4) Postotak zatrazenih od predvidenih troskova");
            Console.WriteLine("(5) Svi do kraja ocijenjeni projekti");
            Console.WriteLine("(6) Nagradeni projekti");

            do
            {
                selection = int.Parse(Console.ReadLine());

                switch (selection)
                {
                    case 1:
                        Console.WriteLine(applicationsRepository.NumberOfValidNotSubmitted());
                        break;
                    case 2:
                        Console.WriteLine(applicationsRepository.SumOfMoneyRequested());
                        break;
                    case 3:
                        Console.WriteLine(applicationsRepository.SumOfMoneyNeeded());
                        break;
                    case 4:
                        Console.WriteLine(applicationsRepository.PercentageRequestedFromNeeded());
                        break;
                    case 5:
                        var i = 1;
                        applicationsRepository.GetGradedApplications().ForEach(application => Console.WriteLine((i++).ToString() + ". " + application.ProjectName));
                        break;
                    case 6:
                        var j = 1;
                        applicationsRepository.GetRewardedApplications().ForEach(application => Console.WriteLine((j++).ToString() + ". " + application.ProjectName + " " + application.MoneyWon));
                        break;
                }
            } while (selection != 0);

            Console.WriteLine("Pomalo HE HE");
        }
    }
}
