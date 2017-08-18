using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRealDeal.Data;
using TheRealDeal.Domain.DTO;

namespace TheRealDeal.Domain.Repositories
{
    class ApplicationsRepository
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

        public int SumOfMoneyRequested()
        {
            return _context.Answers
                .Where(answer => answer.QuestionIndex == "4.1")
                .Select(answer => int.Parse(answer.Value))
                .Sum();
        }

        public int SumOfMoneyNeeded()
        {
            return _context.Answers
                .Where(answer => answer.QuestionIndex == "4.3")
                .Select(answer => int.Parse(answer.Value))
                .Sum();
        }

        public double PercentageRequestedFromNeeded()
        {
            var answers = _context.Answers
                            .Where(answer => answer.QuestionIndex == "4.1" || answer.QuestionIndex == "4.3")
                            .Select(answer => new { QuestionIndex = answer.QuestionIndex, Value = int.Parse(answer.Value) });

            return answers.Where(answer => answer.QuestionIndex == "4.1").Select(answer => answer.Value).Sum() /
                   answers.Where(answer => answer.QuestionIndex == "4.3").Select(answer => answer.Value).Sum() * 100;
        }

        public List<GradedApplicationDTO> GetGradedApplications()
        {
            return _context.Applications
                .GroupJoin(_context.Grades, application => application.Id, grade => grade.ApplicationId, (application, grades) => new { ApplicationId = application.Id, Grades = grades })
                .GroupJoin(_context.Answers, application => application.ApplicationId, answer => answer.ApplicationId, (application, answers) => new { ApplicationId = application.ApplicationId, Grades = application.Grades, Answers = answers })
                .Where(application => application.Grades.ToList().Count == 4)
                .Select(application => new GradedApplicationDTO(){ ApplicationId = application.ApplicationId, ProjectName = application.Answers.First(answer => answer.QuestionIndex == "1.1").Value })
                .ToList();
        }  
    }
}
