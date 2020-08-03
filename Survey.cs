using System;
using System.Collections.Generic;

namespace SurveyCreationAPI.Model
{
    public class Survey
    {
        public int SurveyId { get; set; }

        public int UserId { get; set; }

        public string SurveyTitle { get; set; }
        public string SurveyDesc { get; set; }



        public List<SurveyQuestion> SurveyQuestions { get; set; }

    }
}


