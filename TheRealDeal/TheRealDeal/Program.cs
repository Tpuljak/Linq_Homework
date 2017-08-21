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
                        applicationsRepository.GetGradedApplications().ForEach(application => Console.WriteLine(application.ProjectName));
                        break;
                    case 6:
                        applicationsRepository.GetRewardedApplications().ForEach(application => Console.WriteLine(application.ProjectName + " " + application.MoneyWon));
                        break;
                }
            } while (selection != 0);
        }
    }
}
