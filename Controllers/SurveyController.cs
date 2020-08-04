using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurveyCreationAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : Controller
    {
        SurveyDBDataContext dc = new SurveyDBDataContext();

        [Route("SaveSurvey")]
        [HttpPost]
        public ValidationResult SaveSurvey(/*[FromBody]*/ Survey survey)
        {
            ValidationResult result = new ValidationResult();
            try
            {
                bool isSucess = false;

                //isSucess = dc.InsertTODB(survey);

                if (survey.SurveyId == 0)
                {
                    isSucess = dc.InsertTODB(survey);
                }
                else
                {
                    isSucess = dc.UpdateTODB(survey);
                }

                if (isSucess)
                {
                    result.IsSuccess = true;
                    result.ErrorMessage = new List<string> { "successfull" };

                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = new List<string> { "Error while saving User." };

                    return result;
                }
            }
            catch (Exception ex)
            {


                result.IsSuccess = false;
                result.ErrorMessage = new List<string> { ex.Message };

                return result;
            }
        }


        [Route("SaveSurveyResponse")]
        [HttpPost]
        public ValidationResult SaveSurveyResponse(/*[FromBody]*/ Survey survey)
        {

            ValidationResult result = new ValidationResult();
            try
            {
                bool isSucess = false;

                isSucess = dc.InsertSurveyResponse(survey);

                //isSucess = dc.InsertTODB(survey);

                //if (survey.SurveyId == 0)
                //{
                //    isSucess = dc.InsertTODB(survey);
                //}
                //else
                //{
                //    isSucess = dc.UpdateTODB(survey);
                //}
                //test
                if (isSucess)
                {
                    result.IsSuccess = true;
                    result.ErrorMessage = new List<string> { "successfull" };

                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = new List<string> { "Error while saving User." };

                    return result;
                }

                //test line
            }
            catch (Exception ex)
            {


                result.IsSuccess = false;
                result.ErrorMessage = new List<string> { ex.Message };

                return result;
            }
        }

        [Route("GetSurvey")]
        [HttpGet]
        public ActionResult SurveyDetail([FromQuery] int SurveyId)
        {
            Survey survey = dc.GetSurvey(SurveyId);
            return Ok(survey);
        }

        [Route("GetSurveyResponse")]
        [HttpGet]
        public ActionResult SurveyResponseDetail([FromQuery] int SurveyId)
        {
            Survey survey = dc.GetSurveyResponse(SurveyId);
            return Ok(survey);
        }
        [Route("GetSurveyDetails")]
        [HttpGet]
        public ActionResult GetSurveyDetails()
        {
            List<Survey> survey = dc.GetSurveyDeatails();
            return Ok(survey);
        }




    }

}
