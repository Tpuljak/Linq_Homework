using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                            .Select(answer => new { QuestionIndex = answer.QuestionIndex, Value = double.Parse(answer.Value.Replace(",", "")) });

            return answers.Where(answer => answer.QuestionIndex == "4.1").Select(answer => answer.Value).ConvertToDouble().Sum() /
                   answers.Where(answer => answer.QuestionIndex == "4.3").Select(answer => answer.Value).ConvertToDouble().Sum() * 100;
        }

        public List<ApplicationIdAndNameDTO> GetGradedApplications()
        {
            return _context.Applications
                .GroupJoin(_context.Grades, application => application.Id, grade => grade.ApplicationId, (application, grades) => new { ApplicationId = application.Id, Grades = grades })
                .GroupJoin(_context.Answers, application => application.ApplicationId, answer => answer.ApplicationId, (application, answers) => new { ApplicationId = application.ApplicationId, Grades = application.Grades, Answers = answers })
                .Where(application => application.Grades.ToList().Count == 4)
                .Select(application => new ApplicationIdAndNameDTO() { ApplicationId = application.ApplicationId, ProjectName = application.Answers.FirstOrDefault(answer => answer.QuestionIndex == "1.1").Value })
                .ToList();
        }

        public List<MoneyApplicationsDTO> GetRewardedApplications()
        {
            return _context.Applications
                //.GroupWithGradesAndAnswers(_context)
                 .GroupJoin(_context.Grades, application => application.Id, grade => grade.ApplicationId, (application, grades) => new {
                     ApplicationId = application.Id,
                     Grades = grades })
                 .GroupJoin(_context.Answers, application => application.ApplicationId, answer => answer.ApplicationId, (application, answers) => new EntityGroupDTO() {
                     ApplicationId = application.ApplicationId,
                     Grades = application.Grades.ToList(),
                     Answers = answers.ToList() })
                 .ConvertToGraded()
                 .OrderByDescending(application => application.Grade)
                 .DistributeMoney()
                 .ToList();
        }
    }
}
