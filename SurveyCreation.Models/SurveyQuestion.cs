using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyCreation.Models
{
    public class SurveyQuestion
    {
        public int QuestionId { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionTitle { get; set; }


        public List<QuestionChoiceDetail> QuestionChoices { get; set; }


        
    }
}
