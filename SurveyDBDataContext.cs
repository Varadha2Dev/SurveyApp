using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;


namespace SurveyCreationAPI.Model
{
    public class SurveyDBDataContext
    {
        string conStr;
        SqlConnection con = null;
        SqlDataReader dr;
        private string msg;
        public SurveyDBDataContext()
        {
            var configuration = GetConfiguration();
            conStr = configuration.GetSection("ConnectionString").GetSection("sqlConStr").Value;
        }
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public bool InsertTODB(Survey survey)
        {
            //Survey survey = new Survey();

            int RowsAffected;
            int surveyid;
            try
            {
                con = new SqlConnection(conStr);

                SqlCommand cmd = new SqlCommand("Insert_Survey", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@SurveyId", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@SurveyTitle", survey.SurveyTitle);
                cmd.Parameters.AddWithValue("@SurveyDesc", survey.SurveyDesc);


                RowsAffected = cmd.ExecuteNonQuery();

                surveyid = Convert.ToInt32(cmd.Parameters["@SurveyId"].Value);

                if (con != null && con.State == System.Data.ConnectionState.Open)
                    con.Close();


                for (int i = 0; i <= survey.SurveyQuestions.Count - 1; i++)
                {

                    SqlCommand cmnd = new SqlCommand("Survey_Questions", con);
                    cmnd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    cmnd.Parameters.Clear();

                    cmnd.Parameters.AddWithValue("@SurveyId", surveyid);
                    cmnd.Parameters.Add("@QuestionId", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
                    //cmnd.Parameters.AddWithValue("@QuestionId", survey.SurveyQuestions[i].QuestionId);
                    cmnd.Parameters.AddWithValue("@QuestionTypeId", survey.SurveyQuestions[i].QuestionTypeId);
                    cmnd.Parameters.AddWithValue("@QuestionTitle", survey.SurveyQuestions[i].QuestionTitle);

                    RowsAffected = cmnd.ExecuteNonQuery();

                    int questionid = Convert.ToInt32(cmnd.Parameters["@QuestionId"].Value);


                    if (con != null && con.State == System.Data.ConnectionState.Open)
                        con.Close();

                    if (survey.SurveyQuestions[i].QuestionChoices != null)
                    {
                        for (int j = 0; j <= survey.SurveyQuestions[i].QuestionChoices.Count - 1; j++)
                        {
                            SqlCommand cmnds = new SqlCommand("Survey_QuestionChoice", con);
                            cmnds.CommandType = System.Data.CommandType.StoredProcedure;
                            con.Open();
                            cmnds.Parameters.Clear();


                            cmnds.Parameters.AddWithValue("@QuestionId", questionid);
                            //cmnds.Parameters.AddWithValue("@QuestionChoiceId", survey.SurveyQuestions[i].QuestionChoices[j].QuestionChoiceId);
                            cmnds.Parameters.AddWithValue("@QuestionChoice", survey.SurveyQuestions[i].QuestionChoices[j].QuestionChoice);

                            RowsAffected = cmnds.ExecuteNonQuery();

                            if (con != null && con.State == System.Data.ConnectionState.Open)
                                con.Close();
                        }
                    }

                }

                if (RowsAffected > 0)
                {
                    msg = "Successfull";
                    return true;
                }
                else
                {
                    msg = "Error while saving User. No Rows affected.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            finally
            {
                if (con != null && con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }






        public bool UpdateTODB(Survey survey)
        {


            int RowsAffected;

            try
            {
                con = new SqlConnection(conStr);

                SqlCommand cmd = new SqlCommand("Update_Survey", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SurveyId", survey.SurveyId);
                cmd.Parameters.AddWithValue("@SurveyTitle", survey.SurveyTitle);
                cmd.Parameters.AddWithValue("@SurveyDesc", survey.SurveyDesc);


                RowsAffected = cmd.ExecuteNonQuery();

                if (con != null && con.State == System.Data.ConnectionState.Open)
                    con.Close();


                for (int i = 0; i <= survey.SurveyQuestions.Count - 1; i++)
                {

                    SqlCommand cmnd = new SqlCommand("Update_SurveyQuestion", con);
                    cmnd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    cmnd.Parameters.Clear();

                    cmd.Parameters.AddWithValue("@SurveyId", survey.SurveyId);
                    //cmnd.Parameters.AddWithValue("@QuestionId", survey.SurveyQuestions[i].QuestionId);
                    cmnd.Parameters.AddWithValue("@QuestionTypeId", survey.SurveyQuestions[i].QuestionTypeId);
                    cmnd.Parameters.AddWithValue("@QuestionTitle", survey.SurveyQuestions[i].QuestionTitle);

                    RowsAffected = cmnd.ExecuteNonQuery();

                    if (con != null && con.State == System.Data.ConnectionState.Open)
                        con.Close();

                    for (int j = 0; j <= survey.SurveyQuestions[i].QuestionChoices.Count - 1; j++)
                    {
                        SqlCommand cmnds = new SqlCommand("Update_SurveyQuestionChoice", con);
                        cmnds.CommandType = System.Data.CommandType.StoredProcedure;
                        con.Open();
                        cmnds.Parameters.Clear();


                        cmnds.Parameters.AddWithValue("@QuestionId", survey.SurveyQuestions[i].QuestionId);
                        //cmnds.Parameters.AddWithValue("@QuestionChoiceId", survey.SurveyQuestions[i].QuestionChoices[j].QuestionChoiceId);
                        cmnds.Parameters.AddWithValue("@QuestionChoice", survey.SurveyQuestions[i].QuestionChoices[j].QuestionChoice);

                        RowsAffected = cmnds.ExecuteNonQuery();

                        if (con != null && con.State == System.Data.ConnectionState.Open)
                            con.Close();
                    }

                }

                if (RowsAffected > 0)
                {
                    msg = "Successfull";
                    return true;
                }
                else
                {
                    msg = "Error while saving User. No Rows affected.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            finally
            {
                if (con != null && con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }



        public Survey GetSurvey(int SurveyId)
        {
            Survey survey = new Survey();


            try
            {
                con = new SqlConnection(conStr);

                SqlCommand cmd = new SqlCommand("SelectStatement_Survey", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SurveyId", SurveyId);
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        survey.SurveyId = Convert.ToInt32(dr["SurveyId"]);
                        survey.SurveyTitle = Convert.ToString(dr["SurveyTitle"]);
                        survey.SurveyDesc = Convert.ToString(dr["SurveyDesc"]);

                    }

                }

                dr.NextResult();

                survey.SurveyQuestions = new List<SurveyQuestion>();

                if (dr.HasRows)
                {

                    while (dr.Read())
                    {

                        SurveyQuestion surveyquestion = new SurveyQuestion();

                        surveyquestion.QuestionId = Convert.ToInt32(dr["QuestionId"]);
                        surveyquestion.QuestionTypeId = Convert.ToInt32(dr["QuestionTypeId"]);
                        surveyquestion.QuestionTitle = Convert.ToString(dr["QuestionTitle"]);


                        survey.SurveyQuestions.Add(surveyquestion);




                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        SurveyQuestion question = survey.SurveyQuestions.Where(s => s.QuestionId == Convert.ToInt32(dr["QuestionId"])).FirstOrDefault();

                        if (question.QuestionChoices == null)
                        {
                            question.QuestionChoices = new List<QuestionChoiceDetail>();
                        }

                        QuestionChoiceDetail choice = new QuestionChoiceDetail();
                        choice.QuestionChoiceId = Convert.ToInt32(dr["QuestionChoiceId"]);
                        choice.QuestionChoice = Convert.ToString(dr["QuestionChoice"]);

                        question.QuestionChoices.Add(choice);


                    }

                }


            }
            catch (Exception ex)
            { msg = ex.Message; }

            finally
            {

                if (con != null && con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
            return survey;
        }




        public bool InsertSurveyResponse(Survey survey)
        {

            //UserSurvey usurvey = new UserSurvey();

            int RowsAffected;
            int userSurveyId;
            try
            {
                con = new SqlConnection(conStr);

                SqlCommand cmd = new SqlCommand("Insert_UserSurvey", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@UserSurveyId", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@SurveyId", survey.SurveyId);
                cmd.Parameters.AddWithValue("@UserId", survey.UserId);


                RowsAffected = cmd.ExecuteNonQuery();

                userSurveyId = Convert.ToInt32(cmd.Parameters["@UserSurveyId"].Value);

                if (con != null && con.State == System.Data.ConnectionState.Open)
                    con.Close();


                for (int i = 0; i <= survey.SurveyQuestions.Count - 1; i++)
                {

                    //SqlCommand cmnd = new SqlCommand("Insert_SurveyResponse", con);
                    //cmnd.CommandType = System.Data.CommandType.StoredProcedure;
                    //con.Open();
                    //cmnd.Parameters.Clear();

                    //cmnd.Parameters.AddWithValue("@UserSurveyId", userSurveyId);
                    //cmnd.Parameters.Add("@SurveyResponseId", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
                    //cmnd.Parameters.AddWithValue("@QuestionId", survey.SurveyQuestions[i].QuestionId);

                    if (survey.SurveyQuestions[i].QuestionTypeId == 1)
                    {
                        for (int j = 0; j <= survey.SurveyQuestions[i].QuestionChoices.Count - 1; j++)
                        {


                            SqlCommand cmnd = new SqlCommand("Insert_SurveyResponse", con);
                            cmnd.CommandType = System.Data.CommandType.StoredProcedure;
                            con.Open();
                            cmnd.Parameters.Clear();

                            cmnd.Parameters.AddWithValue("@UserSurveyId", userSurveyId);
                            cmnd.Parameters.Add("@SurveyResponseId", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
                            cmnd.Parameters.AddWithValue("@QuestionId", survey.SurveyQuestions[i].QuestionId);



                            cmnd.Parameters.AddWithValue("@QuestinChoiceId", "");
                            cmnd.Parameters.AddWithValue("@FreetextAnswer", survey.SurveyQuestions[i].QuestionChoices[j].FreetextAnswer);

                            RowsAffected = cmnd.ExecuteNonQuery();

                            if (con != null && con.State == System.Data.ConnectionState.Open)
                                con.Close();
                        }
                    }

                    else
                    {

                        for (int j = 0; j <= survey.SurveyQuestions[i].QuestionChoices.Count - 1; j++)
                        {

                            if (survey.SurveyQuestions[i].QuestionChoices[j].IsSelected == true)
                            {


                                SqlCommand cmnd = new SqlCommand("Insert_SurveyResponse", con);
                                cmnd.CommandType = System.Data.CommandType.StoredProcedure;
                                con.Open();
                                cmnd.Parameters.Clear();

                                cmnd.Parameters.AddWithValue("@UserSurveyId", userSurveyId);
                                cmnd.Parameters.Add("@SurveyResponseId", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
                                cmnd.Parameters.AddWithValue("@QuestionId", survey.SurveyQuestions[i].QuestionId);



                                cmnd.Parameters.AddWithValue("@QuestinChoiceId", survey.SurveyQuestions[i].QuestionChoices[j].QuestionChoiceId);
                                cmnd.Parameters.Add(new SqlParameter("@FreetextAnswer", DBNull.Value));

                                RowsAffected = cmnd.ExecuteNonQuery();

                                if (con != null && con.State == System.Data.ConnectionState.Open)
                                    con.Close();
                            }
                        }
                    }
                }

                if (RowsAffected > 0)
                {
                    msg = "Successfull";
                    return true;
                }
                else
                {
                    msg = "Error while saving User. No Rows affected.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            finally
            {
                if (con != null && con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }



        public Survey GetSurveyResponse(int SurveyId)
        {
            Survey survey = new Survey();

            SurveyQuestion FreetextQuestion = null;

            SurveyQuestion surveyquestion = null;


            try
            {
                con = new SqlConnection(conStr);

                SqlCommand cmd = new SqlCommand("SelectStatement_SurveyResponse", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SurveyId", SurveyId);
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        survey.SurveyId = Convert.ToInt32(dr["SurveyId"]);
                        survey.SurveyTitle = Convert.ToString(dr["SurveyTitle"]);
                        survey.SurveyDesc = Convert.ToString(dr["SurveyDesc"]);

                    }

                }

                dr.NextResult();

                survey.SurveyQuestions = new List<SurveyQuestion>();

                if (dr.HasRows)
                {

                    while (dr.Read())
                    {

                        surveyquestion = new SurveyQuestion();

                        surveyquestion.QuestionId = Convert.ToInt32(dr["QuestionId"]);
                        surveyquestion.QuestionTypeId = Convert.ToInt32(dr["QuestionTypeId"]);
                        surveyquestion.QuestionTitle = Convert.ToString(dr["QuestionTitle"]);

                        if (surveyquestion.QuestionTypeId == 1)
                        {
                            FreetextQuestion = surveyquestion;
                        }

                        survey.SurveyQuestions.Add(surveyquestion);

                    }

                    dr.NextResult();

               

                    while (dr.Read())
                    {
                        SurveyQuestion question = survey.SurveyQuestions.Where(s => s.QuestionId == Convert.ToInt32(dr["QuestionId"])).FirstOrDefault();
                      
                        if (question.QuestionTypeId == 1)
                        {

                            if (question.QuestionChoices == null)
                            {
                                question.QuestionChoices = new List<QuestionChoiceDetail>();
                            }

                            QuestionChoiceDetail choice = new QuestionChoiceDetail();
                           
                            choice.FreetextAnswer = Convert.ToString(dr["FreetextAnswer"]);



                            question.QuestionChoices.Add(choice);
                        }
                        else
                        {
                            if (question.QuestionChoices == null)
                            {
                                question.QuestionChoices = new List<QuestionChoiceDetail>();
                            }

                            QuestionChoiceDetail choice = new QuestionChoiceDetail();
                           
                            choice.QuestionChoiceId = Convert.ToInt32(dr["QuestinChoiceId"]);
                            choice.QuestionChoice = Convert.ToString(dr["QuestionChoice"]);
                            choice.Count = Convert.ToInt32(dr["Count"]);


                            question.QuestionChoices.Add(choice);
                        }


                    }
                    
                }


            }
            catch (Exception ex)
            { msg = ex.Message; }

            finally
            {

                if (con != null && con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
            return survey;
        }

        public List<Survey> GetSurveyDeatails()
        {

            List<Survey> model = new List<Survey>();
            try
            {
                con = new SqlConnection(conStr);

                SqlCommand cmd = new SqlCommand("SurveyDetails", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Survey survey = new Survey();
                        survey.SurveyId = Convert.ToInt32(dr["SurveyId"]);
                        survey.SurveyTitle = Convert.ToString(dr["SurveyTitle"]);
                       


                        model.Add(survey);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;

            }
            finally
            {

                if (con != null && con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }

            return model;
        }


    }
}
