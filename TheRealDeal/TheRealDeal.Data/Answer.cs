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
    
    public partial class Answer
    {
        public int Id { get; set; }
        public string QuestionIndex { get; set; }
        public string Value { get; set; }
        public string Explanation { get; set; }
        public int ApplicationId { get; set; }
    
        public virtual Application Application { get; set; }
    }
}