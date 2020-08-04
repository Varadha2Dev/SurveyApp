using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyCreation.Models
{
    public class UserSurvey
    {
        public int UserSurveyId { get; set; }

        public int SurveyId { get; set; }
        public int UserId { get; set; }

        public List<SurveyResponse> SurveyResponses { get; set; }
    }
}
