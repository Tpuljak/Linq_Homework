//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TheRealDeal.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Remark
    {
        public int Id { get; set; }
        public string QuestionIndex { get; set; }
        public string Explanation { get; set; }
        public int GradeId { get; set; }
    
        public virtual Grade Grade { get; set; }
    }
}
