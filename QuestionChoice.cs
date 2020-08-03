using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyCreationAPI.Model
{
    public class QuestionChoiceDetail
    {
      
        public int QuestionChoiceId { get; set; }

        public string QuestionChoice { get; set; }

        public string FreetextAnswer { get; set; }

        public int Count { get; set; }


        public bool IsSelected { get; set; }

    }
}
