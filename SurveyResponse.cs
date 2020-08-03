using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyCreationAPI.Model
{
    public class SurveyResponse
    {

        public int SurveyResponseId { get; set; }
        public int UserSurveyId { get; set; }
        public int QuestionId { get; set; }
        public int QuestinChoiceId { get; set; }
        public string FreetextAnswer { get; set; }
        

    }
}
