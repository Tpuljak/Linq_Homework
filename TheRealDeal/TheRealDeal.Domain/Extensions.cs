using System.Collections.Generic;
using System.Linq;
using TheRealDeal.Data;
using TheRealDeal.Domain.DTO;

namespace TheRealDeal.Domain
{
    public static class Extensions
    {
        public static Dictionary<string, string> Categories { get; set; } = new Dictionary<string, string>()
        {
            {"1", "Radionice, predavanja i tribine"},
            {"2", "Studentski mediji i kulturna događanja"},
            {"3", "Znanstveno-istraživački rad"},
            {"4", "Inovativno-tehnološki projekti"},
            {"5", "Šport"}
        };

        public static List<double> ConvertToDouble(this IEnumerable<double> list)
        {
            return list
                .Select(number => number.ToString().Contains('.') && number.ToString().Split('.')[1].Length > 2 ? double.Parse(number.ToString().Replace(".", "")) : number)
                .ToList();
        }

        public static double GetSumOfMoney(this IEnumerable<MoneyRequestAnswersDTO> list, string questionIndex)
        {
            return list.Where(answer => answer.QuestionIndex == questionIndex).Select(answer => answer.Value).ConvertToDouble().Sum();
        }

        public static IEnumerable<EntityGroupDTO> GroupWithGradesAndAnswers(this IEnumerable<Application> list, SzstEntities _context)
        {
            return list
                .GroupJoin(_context.Grades,
                    application => application.Id,
                    grade => grade.ApplicationId,
                    (application, grades) => new {
                        ApplicationId = application.Id,
                        Grades = grades
                    })
                .GroupJoin(_context.Answers,
                    application => application.ApplicationId,
                    answer => answer.ApplicationId,
                    (application, answers) => new EntityGroupDTO() {
                        ApplicationId = application.ApplicationId,
                        Grades = application.Grades.ToList(),
                        Answers = answers.ToList()
                    });
        }

        public static int AddGrades(this IEnumerable<Grade> list)
        {
            return list.Sum(grade => grade.Sustainability) + 
                list.Sum(grade => grade.Innovation) + 
                list.Sum(grade => grade.ImportanceForOtherStudents) + 
                list.Sum(grade => grade.SubjectiveOpinion) + 
                list.Sum(grade => grade.SzstPromotion) + 
                list.Sum(grade => grade.Workflow) + 
                list.Sum(grade => grade.SupportFromOtherOrganizations);
        }

        public static IEnumerable<GradedApplicationDTO> ConvertToGraded(this IEnumerable<EntityGroupDTO> list)
        {
            return list
                .Select(application => new GradedApplicationDTO() {
                    ApplicationId = application.ApplicationId,
                    ProjectName = application.Answers.Where(answer => answer.QuestionIndex == "1.1").DefaultIfEmpty(null).First().Value,
                    Grade = application.Grades.AddGrades(),
                    Category = application.Answers.Where(answer => answer.QuestionIndex == "1.2" && answer.Value != null).DefaultIfEmpty(null).First() != null ?
                                  Categories[application.Answers.FirstOrDefault(answer => answer.QuestionIndex == "1.2").Value] : "No category"
                });
        }

        public static IEnumerable<MoneyApplicationsDTO> DistributeMoney(this IEnumerable<GradedApplicationDTO> list)
        {
            var moneyApps = new List<MoneyApplicationsDTO>();
            var maximumGrade = list.Max(application => application.Grade);

            moneyApps.AddRange(list
                .TakeWhile(application => application.Grade == maximumGrade)
                .Select(application => new MoneyApplicationsDTO() {
                    ApplicationId = application.ApplicationId,
                    ProjectName = application.ProjectName,
                    MoneyWon = 200000.0 / list.Count(app => app.Grade == maximumGrade) }));

            var skipIndex = 0;

            list = list
                .Where(app => app.Category != "No category")
                .OrderBy(app => app.Category)
                .ThenByDescending(app => app.Grade)
                .ToList();

            for (int i = 0; i < 5; i++) {
                var currMaxGrade = list.ToList()[skipIndex].Grade;

                var applications = list
                    .Skip(skipIndex)
                    .TakeWhile(application => application.Grade == currMaxGrade)
                    .ToList();

                if (applications.Count == 1)
                {
                    if (moneyApps.Where(app => app.ApplicationId == applications[0].ApplicationId).DefaultIfEmpty(null).First() != null)
                        moneyApps.First(app => app.ApplicationId == applications[0].ApplicationId).MoneyWon += 30000;
                    else
                        moneyApps.Add(new MoneyApplicationsDTO() { ProjectName = applications[0].ProjectName, ApplicationId = applications[0].ApplicationId, MoneyWon = 30000 });
                }
                else
                {
                    for (int j = 0; i < applications.Count; i++)
                    {
                        if (moneyApps.Where(app => app.ApplicationId == applications[j].ApplicationId).DefaultIfEmpty(null).First() != null)
                            moneyApps.First(app => app.ApplicationId == applications[j].ApplicationId).MoneyWon += 30000.0 / applications.Count;
                        else
                            moneyApps.Add(new MoneyApplicationsDTO() { ProjectName = applications[j].ProjectName, ApplicationId = applications[j].ApplicationId, MoneyWon = 30000 / applications.Count });
                    }
                }

                skipIndex += list.Select(app => app.Category).Count(category => category == applications[0].Category);
            }

            return moneyApps;
        }
    }
}
