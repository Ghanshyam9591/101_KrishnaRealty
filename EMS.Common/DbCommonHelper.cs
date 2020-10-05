using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Common
{
    public class DbCommonHelper
    {
        string strcon = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ToString();

        List<RecordException> ERROR_LIST = new List<RecordException>();
        NpgsqlTransaction trans;

        string ACTIVITY_NUMBER, ACTIVITY_NAME = string.Empty;
        int BATCH_NUMBER = 0;
        DateTime? BATCH_NUMBER_DATE, ACTIVITY_DATE;

        public string Save(string InsertQuery, string selectQuery, string updateQuery, NpgsqlParameter[] paramenters, NpgsqlParameter[] SelectParam, NpgsqlParameter[] updateParam)
        {
            DataTable dt = new DataTable();
            string Response = string.Empty;
            using (NpgsqlConnection conn = new NpgsqlConnection(strcon))
            {
                try
                {
                    conn.Open();
                    using (NpgsqlCommand cmdSelect = new NpgsqlCommand(selectQuery))
                    {
                        cmdSelect.Connection = conn;
                        if (paramenters != null)
                            cmdSelect.Parameters.AddRange(SelectParam);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmdSelect))
                        {
                            SqDA.Fill(dt);
                        }
                    }
                    if (dt.Rows.Count == 0)
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand(InsertQuery))
                        {
                            cmd.Connection = conn;
                            if (paramenters != null)
                                cmd.Parameters.AddRange(paramenters);
                            cmd.ExecuteNonQuery();
                            Response = MessageHelper.Success;
                        }
                    }
                    else
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand(updateQuery))
                        {
                            cmd.Connection = conn;
                            if (paramenters != null)
                                cmd.Parameters.AddRange(updateParam);
                            cmd.ExecuteNonQuery();
                            Response = MessageHelper.Update;
                        }
                        // Response = MessageHelper.DuplicateRecord;
                    }
                }
                catch (Exception ex)
                {
                    Helper.WriteLog("ERROR IN Save fun : " + ex.Message);
                    Response = MessageHelper.Fail;
                }
                return Response;
            }
        }
        public string Save(string InsertQuery, NpgsqlParameter[] paramenters)
        {
            DataTable dt = new DataTable();
            string Response = string.Empty;
            using (NpgsqlConnection conn = new NpgsqlConnection(strcon))
            {
                conn.Open();
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand(InsertQuery))
                    {
                        cmd.Connection = conn;
                        if (paramenters != null)
                            cmd.Parameters.AddRange(paramenters);
                        cmd.ExecuteNonQuery();
                        Response = MessageHelper.Success;
                    }

                }
                catch (Exception ex)
                {
                    Response = MessageHelper.Fail;
                }
                return Response;
            }
        }

        public string ExecuteProcedure(string procedure_name, NpgsqlParameter[] paramenters)
        {
            DataTable dt = new DataTable();
            string Response = string.Empty;
            object obj = new object();
            using (NpgsqlConnection conn = new NpgsqlConnection(strcon))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(procedure_name,conn))
                {
                    try
                    {
                        conn.Open();
                        if (paramenters != null)
                            cmd.Parameters.AddRange(paramenters);
                       // cmd.CommandType = CommandType.StoredProcedure;
                        obj = cmd.ExecuteScalar();

                        if (obj != null)
                            return obj.ToString();
                        else
                            return "";
                    }
                    catch (Exception exx)
                    {
                        Helper.WriteLog("ExecuteProcedure error :"+exx.Message);
                        return "";
                    }
                }
            }
        }

        public string Save2(string InsertQuery, string update_or_deleteQuery, NpgsqlParameter[] paramenters, NpgsqlParameter[] update_or_deletepara, int rowcount)
        {
            DataTable dt = new DataTable();
            string Response = string.Empty;
            using (NpgsqlConnection conn = new NpgsqlConnection(strcon))
            {
                try
                {
                    conn.Open();
                    if (rowcount == 1)
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand(update_or_deleteQuery))
                        {
                            cmd.Connection = conn;
                            if (paramenters != null)
                                cmd.Parameters.AddRange(update_or_deletepara);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    using (NpgsqlCommand cmd = new NpgsqlCommand(InsertQuery))
                    {
                        cmd.Connection = conn;
                        if (paramenters != null)
                            cmd.Parameters.AddRange(paramenters);
                        cmd.ExecuteNonQuery();
                        Response = MessageHelper.Success;
                    }
                    // Response = MessageHelper.DuplicateRecord;

                }
                catch (Exception ex)
                {
                    Helper.WriteLog("ERROR IN Save2 fun : " + ex.Message);
                    Response = MessageHelper.Fail;
                }
                return Response;
            }
        }


        public List<RecordException> PushToTBLPaymentDone(List<ReleasePaymentModel> releaseModel)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(strcon))
            {
                con.Open();
                trans = con.BeginTransaction();
                try
                {

                    foreach (ReleasePaymentModel RModel in releaseModel.Where(item => item.SELECT_STATUS == true))
                    {

                        string UpdateQuery = @"update ems_tbl_release_for_payment set payment_done='Yes', payment_date=@payment_date where seqid=@seqid";


                        using (NpgsqlCommand cmdupdate = new NpgsqlCommand(UpdateQuery, con, trans))
                        {
                            cmdupdate.Parameters.AddWithValue("@payment_date", RModel.ACTIVITY_DATE);
                            cmdupdate.Parameters.AddWithValue("@seqid", RModel.ID);
                            cmdupdate.ExecuteNonQuery();
                        }
                        string InsertQuery = @"INSERT INTO ems_tbl_payment_done(
                                                           emp_code,emp_name,claim_amt,
                                                           batch_number,batch_number_date,data_upload_date,created_date,releaseforpayment_seqid
                                                         )
                                                 VALUES(
                                                          @EMP_CODE,@EMP_NAME,@CLAIM_AMT,
                                                          @BATCH_NUMBER, @BATCH_NUMBER_DATE, @DATA_UPLOAD_DATE, @CREATED_DATE,@RELEASEFORPAYMENT_SEQID
                                                        )";


                        using (NpgsqlCommand cmd = new NpgsqlCommand(InsertQuery, con, trans))
                        {

                            cmd.Parameters.AddWithValue("@EMP_CODE", RModel.EMP_CODE);
                            cmd.Parameters.AddWithValue("@EMP_NAME", RModel.EMP_NAME);
                            cmd.Parameters.AddWithValue("@CLAIM_AMT", RModel.CLAIM_AMT);
                            cmd.Parameters.AddWithValue("@BATCH_NUMBER", RModel.BATCH_NUMBER);
                            cmd.Parameters.AddWithValue("@BATCH_NUMBER_DATE", RModel.BATCH_NUMBER_DATE);
                            cmd.Parameters.AddWithValue("@DATA_UPLOAD_DATE", DateTime.Now);
                            cmd.Parameters.AddWithValue("@CREATED_DATE", DateTime.Now);
                            cmd.Parameters.AddWithValue("@RELEASEFORPAYMENT_SEQID", RModel.ID);
                            cmd.ExecuteNonQuery();
                        }

                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    RecordException RException2 = new RecordException();

                    RException2.ERR_MSG = ex.Message;
                    ERROR_LIST.Add(RException2);
                    trans.Rollback();
                    //EmailActivity.SendEmail(ERROR_LIST, "INSERT ERROR");
                }
            }
            return ERROR_LIST;
        }



        public List<DDLModel> FILE_DROPDOWN_LIST(string cQuery, NpgsqlParameter[] paramenters)
        {

            List<DDLModel> ddlList = new List<DDLModel>();
            DataTable dt = new DataTable();
            using (NpgsqlConnection conn = new NpgsqlConnection(strcon))
            {
                try
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(cQuery))
                    {
                        cmd.Connection = conn;
                        if (paramenters != null)
                            cmd.Parameters.AddRange(paramenters);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                        foreach (DataRow drow in dt.Rows)
                        {
                            DDLModel ddlobj = new DDLModel();
                            ddlobj.SeqID = string.IsNullOrEmpty(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"]);
                            ddlobj.Name = string.IsNullOrEmpty(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            ddlList.Add(ddlobj);
                        }
                        return ddlList;
                    }
                }
                catch (Exception ex)
                {
                    Helper.WriteLog("ERROR [ExecuteParaQuery] : " + ex.Message);
                    return null;
                }
            }
        }

        public List<DDLModel> FILE_DROPDOWNLIST_WITH_SEQID(string cQuery, string ColumnNm, NpgsqlParameter[] paramenters)
        {

            List<DDLModel> ddlList = new List<DDLModel>();
            DataTable dt = new DataTable();
            using (NpgsqlConnection conn = new NpgsqlConnection(strcon))
            {
                try
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(cQuery))
                    {
                        cmd.Connection = conn;
                        if (paramenters != null)
                            cmd.Parameters.AddRange(paramenters);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                        foreach (DataRow drow in dt.Rows)
                        {
                            DDLModel ddlobj = new DDLModel();
                            ddlobj.SeqID = string.IsNullOrEmpty(drow["seqid"].ToString()) ? 0 : Convert.ToInt32(drow["seqid"].ToString());
                            ddlobj.Name = string.IsNullOrEmpty(drow[ColumnNm].ToString()) ? "" : drow[ColumnNm].ToString();
                            ddlList.Add(ddlobj);
                        }
                        return ddlList;
                    }
                }
                catch (Exception ex)
                {
                    Helper.WriteLog("ERROR [ExecuteParaQuery] : " + ex.Message);
                    return null;
                }
            }
        }

        public List<DDLModel2> FEEL_DROPDOWNLIST_FOR_EMP(string cQuery, string ColumnNm, NpgsqlParameter[] paramenters)
        {

            List<DDLModel2> ddlList2 = new List<DDLModel2>();
            DataTable dt = new DataTable();
            using (NpgsqlConnection conn = new NpgsqlConnection(strcon))
            {
                try
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(cQuery))
                    {
                        cmd.Connection = conn;
                        if (paramenters != null)
                            cmd.Parameters.AddRange(paramenters);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                        foreach (DataRow drow in dt.Rows)
                        {
                            DDLModel2 ddlobj = new DDLModel2();
                            ddlobj.emp_code = string.IsNullOrEmpty(drow["seqid"].ToString()) ? 0 : Convert.ToInt32(drow["seqid"].ToString());
                            ddlobj.EmpName = string.IsNullOrEmpty(drow[ColumnNm].ToString()) ? "" : drow[ColumnNm].ToString();
                            ddlList2.Add(ddlobj);
                        }
                        return ddlList2;
                    }
                }
                catch (Exception ex)
                {
                    Helper.WriteLog("ERROR [ExecuteParaQuery] : " + ex.Message);
                    return null;
                }
            }
        }
        public string Update(string UpdateQuery, NpgsqlParameter[] paramenters)
        {
            DataTable dt = new DataTable();
            string Response = string.Empty;
            using (NpgsqlConnection conn = new NpgsqlConnection(strcon))
            {
                conn.Open();
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand(UpdateQuery))
                    {
                        cmd.Connection = conn;
                        if (paramenters != null)
                            cmd.Parameters.AddRange(paramenters);
                        cmd.ExecuteNonQuery();
                        Response = MessageHelper.Update;
                    }

                }
                catch (Exception ex)
                {
                    Response = MessageHelper.Fail;
                }
                return Response;
            }
        }

        public DataTable ExecuteParaQuery(string cQuery, NpgsqlParameter[] paramenters)
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection conn = new NpgsqlConnection(strcon))
            {
                try
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(cQuery))
                    {
                        cmd.Connection = conn;
                        if (paramenters != null)
                            cmd.Parameters.AddRange(paramenters);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    Helper.WriteLog("ERROR [ExecuteParaQuery] : " + ex.Message);
                    return null;
                }
            }
        }
    }
}
