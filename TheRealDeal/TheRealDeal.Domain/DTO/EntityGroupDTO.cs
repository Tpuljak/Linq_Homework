using System.Collections.Generic;
using TheRealDeal.Data;

namespace TheRealDeal.Domain.DTO
{
    public class EntityGroupDTO
    {
        public int ApplicationId { get; set; }
        public List<Grade> Grades { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
