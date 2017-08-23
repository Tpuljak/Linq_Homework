using System.Collections.Generic;
using System.Linq;
using TheRealDeal.Data;
using TheRealDeal.Domain.DTO;

namespace TheRealDeal.Domain.Repositories
{
    public class ApplicationsRepository
    {
        public ApplicationsRepository()
        {
            _context = new SzstEntities();
        }

        private readonly SzstEntities _context;

        public int NumberOfValidNotSubmitted()
        {
            return _context.Applications.Count(application => application.IsValid && !application.IsSubmitted);
        }

        public double SumOfMoneyRequested()
        {
            return _context.Answers
                .Where(answer => answer.QuestionIndex == "4.1")
                .Where(answer => answer.Value != null)
                .ToList()
                .Select(answer => double.Parse(answer.Value.Replace(",", "")))
                .ConvertToDouble()
                .Sum();
        }

        public double SumOfMoneyNeeded()
        {
            return _context.Answers
                .Where(answer => answer.QuestionIndex == "4.3")
                .Where(answer => answer.Value != null)
                .ToList()
                .Select(answer => double.Parse(answer.Value.Replace(",", "")))
                .ConvertToDouble()
                .Sum();
        }

        public double PercentageRequestedFromNeeded()
        {
            var answers = _context.Answers
                            .Where(answer => answer.QuestionIndex == "4.1" || answer.QuestionIndex == "4.3")
                            .Where(answer => answer.Value != null)
                            .ToList()
                            .Select(answer => new MoneyRequestAnswersDTO {
                                QuestionIndex = answer.QuestionIndex,
                                Value = double.Parse(answer.Value.Replace(",", "")) });

            return answers.GetSumOfMoney("4.1") / answers.GetSumOfMoney("4.3") * 100;
        }

        public List<ApplicationIdAndNameDTO> GetGradedApplications()
        {
            return _context.Applications
                .GroupWithGradesAndAnswers(_context)
                .Where(application => application.Grades.ToList().Count == 4)
                .Select(application => new ApplicationIdAndNameDTO() {
                    ApplicationId = application.ApplicationId,
                    ProjectName = application.Answers.FirstOrDefault(answer => answer.QuestionIndex == "1.1").Value })
                .ToList();
        }

        public List<MoneyApplicationsDTO> GetRewardedApplications()
        {
            return _context.Applications
                 .GroupWithGradesAndAnswers(_context)
                 .ConvertToGraded()
                 .OrderByDescending(application => application.Grade)
                 .DistributeMoney()
                 .ToList();
        }
    }
}
