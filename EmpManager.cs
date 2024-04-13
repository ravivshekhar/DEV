using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Entity;
using System.Collections;


namespace DataAccess
{
    public class EmpManager
    {

        public static string connection = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        SqlConnection con = new SqlConnection(connection);
        SqlCommand cmd = new SqlCommand();

        EmpRegistration empRegister = new EmpRegistration();
        Leads lead = new Leads();
        StatusLog sl = new StatusLog();
        Reminder rm = new Reminder();

        #region Check Employee Registratione
        public bool CheckEmployee(string emailID)
        {
            try
            {
                cmd = new SqlCommand("SpCheckEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@emailID", SqlDbType.NVarChar).Value = emailID;
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Check Employee EmailID on ForgetPassword Page
        public bool CheckEmailID(string emailID)
        {
            try
            {
                cmd = new SqlCommand("SpCheckEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@emailID", SqlDbType.NVarChar).Value = emailID;
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Register Employee
        public bool RegisterMgr(EmpRegistration empRegister)
        {
            try
            {
                cmd = new SqlCommand("SpInsertEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ProfileID", SqlDbType.Int).Value = empRegister.ProfileID;
                cmd.Parameters.Add("@DepID", SqlDbType.Int).Value = empRegister.DepartmentID;
                cmd.Parameters.Add("@HeadID", SqlDbType.Int).Value = empRegister.HeadID;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = empRegister.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = empRegister.LastName;
                cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar).Value = empRegister.EmailID;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = empRegister.Password;
                cmd.Parameters.Add("@PhoneNo", SqlDbType.NVarChar).Value = empRegister.Phone;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Select All Profile By PfleID
        public DataTable GetProfileByID(int profileID)
        {
            cmd = new SqlCommand("SpSelectPfleByPfID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@profileID", SqlDbType.Int).Value = profileID;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Select All Profile
        public DataTable GetProfileInReport(int empID)
        {
            cmd = new SqlCommand("SpSelectProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Select Employee By Profile
        public DataTable GetEmployee(int empID)
        {
            cmd = new SqlCommand("SpGetEmployeeToAsd", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Select Employee For Report
        public DataTable GetEmpInReport(int empID, int profileID)
        {
            cmd = new SqlCommand("GetEmpInReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            cmd.Parameters.Add("@profileID", SqlDbType.Int).Value = profileID;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Get ProfileId From Employee Table
        public int GetPfleIDByEmp(int empID)
        {
            try
            {
                int profileid = 0;
                cmd = new SqlCommand("SpSelectPfleIDByEmp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    profileid = Convert.ToInt32(sdr["ProfileID"].ToString());
                }
                return profileid;
            }
            catch
            {
                return 0;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Select Member Name By Employee Table
        public DataTable GetMemberName(int DepID)
        {
            cmd = new SqlCommand("SpSelectMemberName", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DepID", SqlDbType.Int).Value = DepID;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region update Profile
        public bool UpdateProfile(int profileID, string profileName)
        {
            try
            {
                cmd = new SqlCommand("SpUpdateProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@profileID", SqlDbType.Int).Value = profileID;
                cmd.Parameters.Add("@profileName", SqlDbType.NVarChar).Value = profileName;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@profileID", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.ReturnValue;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Login Employee
        public EmpRegistration LoginEmp(string userid, string pwd, string strIP)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("PROCEMPLOGDTLS", con);
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userid;
                cmd.Parameters.Add("@Pwd", SqlDbType.NVarChar).Value = pwd;
                cmd.Parameters.Add("@IpAddress", SqlDbType.NVarChar).Value = strIP;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                   // empRegister.Flag = (int)dr["Flag"];
                    empRegister.EmailID = (string)dr["EmailID"];
                    empRegister.FirstName = (string)dr["FirstName"];
                    empRegister.LastName = (string)dr["LastName"];
                    empRegister.Password = (string)dr["Password"];
                    empRegister.EmpID = (int)dr["EmpID"];
                    empRegister.ProfileID = (int)dr["profileID"];
                    empRegister.DepartmentID = (int)dr["DepartmentID"];
                    empRegister.ProfileName = (string)dr["ProfileName"];
                }

                return empRegister;
            }
            catch
            {
                return new EmpRegistration();
            }
            finally
            {
                con.Close();
            }

        }
        #endregion

        #region select ManagerList
        public DataTable selectMgr()
        {
            cmd = new SqlCommand("SpSelectMgr", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Select TlList
        public DataTable selectTl(int empid)
        {
            cmd = new SqlCommand("SpSelectTl", con);
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empid;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Count Tl
        public bool CountTl(int empid)
        {
            try
            {
                cmd = new SqlCommand("SELECT * FROM Employee  WHERE ProfileID=3 AND HeadID=@empId", con);
                cmd.Parameters.Add("@empId", SqlDbType.Int).Value = empid;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }


        }
        #endregion

        #region Count Exe
        public bool CountExe(int empid)
        {
            try
            {
                cmd = new SqlCommand("SELECT * FROM Employee WHERE ProfileID=4 AND HeadID=@empId", con);
                cmd.Parameters.Add("@empId", SqlDbType.Int).Value = empid;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }


        }
        #endregion

        public DataTable GetExe(int empId)
        {
            cmd = new SqlCommand("SpSelectExe", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empId", SqlDbType.Int).Value = empId;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        #region Check Status
        public bool CheckStatus(string status)
        {
            try
            {
                cmd = new SqlCommand("SpCheckStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = status;
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Insert Status
        public bool InsertStatus(string Status)
        {
            try
            {
                cmd = new SqlCommand("SpInsertStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Select Status
        public DataTable GetStatus()
        {
            cmd = new SqlCommand("SpSelectStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Update Status
        public bool UpdateStatus(int status_id, string status)
        {
            try
            {
                cmd = new SqlCommand("SpUpdateStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@status_id", SqlDbType.Int).Value = status_id;
                cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = status;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@status_id", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.ReturnValue;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Delete Status
        public bool DeleteStatus(int statusID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SpDeleteStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@statusID", SqlDbType.Int).Value = statusID;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Multiple Delete Status
        public void DeleteMultipleStatus(ArrayList idCollection)
        {
            foreach (int id in idCollection)
            {
                cmd = new SqlCommand("delete from Status where StatusID=@id", con);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            }
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string msg = "Deletion Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region COUNT Status
        public DataTable CountStatus(int empID)
        {
            cmd = new SqlCommand("Sp_COUNT_STATUS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Insert Source Type
        public bool InsertSource(string SourceType)
        {
            try
            {
                cmd = new SqlCommand("SpInsertSource", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SourceType", SqlDbType.NVarChar).Value = SourceType;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Select Source Type
        public DataTable GetSource()
        {
            cmd = new SqlCommand("SpSelectSource", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Update Source Type
        public bool UpdateSource(int source_id, string source_type)
        {
            try
            {
                cmd = new SqlCommand("SpUpdateSource", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@source_id", SqlDbType.Int).Value = source_id;
                cmd.Parameters.Add("@source_type", SqlDbType.NVarChar).Value = source_type;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@source_id", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.ReturnValue;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Delete Source Type
        public bool DeleteSource(int source_id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SpDeleteSource", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@source_id", SqlDbType.Int).Value = source_id;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Multiple Delete Source
        public void DeleteMultipleSource(ArrayList idCollection)
        {
            foreach (int id in idCollection)
            {
                cmd = new SqlCommand("delete from Source where SourceID=@id", con);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            }
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string msg = "Deletion Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Select Site Url
        public object GetSiteUrl()
        {
            cmd = new SqlCommand("SpSelectSiteUrl", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Check Source
        public bool CheckSource(string source)
        {
            try
            {
                cmd = new SqlCommand("SpCheckSource", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@source", SqlDbType.NVarChar).Value = source;
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Insert Site Url
        public bool InsertSiteUrl(string urlName, int sourceID, string url)
        {
            try
            {
                cmd = new SqlCommand("SpInsertSiteUrl", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@urlName", SqlDbType.NVarChar).Value = urlName;
                cmd.Parameters.Add("@sourceID", SqlDbType.Int).Value = sourceID;
                cmd.Parameters.Add("@url", SqlDbType.NVarChar).Value = url;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Update siteUrl
        public bool UpdateSiteUrl(int rowIndex, string urlName, int sourceID, string urlType)
        {
            try
            {
                cmd = new SqlCommand("SpUpdateSiteUrl", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@rowIndex", SqlDbType.Int).Value = rowIndex;
                cmd.Parameters.Add("@urlName", SqlDbType.NVarChar).Value = urlName;
                cmd.Parameters.Add("@sourceID", SqlDbType.Int).Value = sourceID;
                cmd.Parameters.Add("@urlType", SqlDbType.NVarChar).Value = urlType;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@rowIndex", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.ReturnValue;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Delete SiteUrl
        public bool DeleteSiteUrl(int urlID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SpDeleteSiteUrl", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@urlID", SqlDbType.Int).Value = urlID;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Insert Assigner
        public bool InsertAssigner(int empID, string StartTime, string EndTime, string Switch)
        {
            try
            {
                cmd = new SqlCommand("SpInsertAssigner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                cmd.Parameters.Add("@StartTime", SqlDbType.NVarChar).Value = StartTime;
                cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndTime;
                cmd.Parameters.Add("@Switch", SqlDbType.NVarChar).Value = Switch;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Insert Assigner By Domain
        public bool InsertAsnrOnDomain(int empID, string domain)
        {
            try
            {
                cmd = new SqlCommand("SpInsertAsnrByDomain", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                cmd.Parameters.Add("@domain", SqlDbType.NVarChar).Value = domain;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Update Assigner Details
        public bool UpdateAssigner(int rowIndex, int empID, string StartTime, string EndTime, string Switch)
        {
            try
            {
                cmd = new SqlCommand("SpUpdateAssigner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@rowIndex", SqlDbType.Int).Value = rowIndex;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                cmd.Parameters.Add("@startTime", SqlDbType.NVarChar).Value = StartTime;
                cmd.Parameters.Add("@endTime", SqlDbType.NVarChar).Value = EndTime;
                cmd.Parameters.Add("@Switch", SqlDbType.NVarChar).Value = Switch;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@rowIndex", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.ReturnValue;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Update Asnr By Domain Details
        public bool UpdateAsnrByDomain(int rowIndex, int empID, string domain)
        {
            try
            {
                cmd = new SqlCommand("SpUpdateAsneByDomain", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@rowIndex", SqlDbType.Int).Value = rowIndex;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                cmd.Parameters.Add("@domain", SqlDbType.NVarChar).Value = domain;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@rowIndex", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.ReturnValue;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Select Assigner Details
        public DataTable GetAssignerEmpID()
        {
            cmd = new SqlCommand("SpGetAssigner", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Select AsnrBy Domain Details
        public DataTable GetAsnrByDomainEmpID()
        {
            cmd = new SqlCommand("SpGetAsnrByDomain", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion
        //remove after assigner implementation
        #region Select AsnrDomain EmpID
        //public List<Assigner> GetAsnrDomainEmpID(string urlName)
        //{
        //    try
        //    {
        //        List<Assigner> lstAsnr = new List<Assigner>();
        //        cmd = new SqlCommand("SpGetAsnrDomainEmpID", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@urlName", SqlDbType.NVarChar).Value = urlName;
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            Assigner asnr = new Assigner();
        //            asnr.EmpID = (int)dr["EmpID"];
        //            asnr.StartTime = dr["StartTime"].ToString();
        //            asnr.EndTime = dr["EndTime"].ToString();
        //            asnr.Switch = dr["Switch"].ToString();
        //            lstAsnr.Add(asnr);
        //        }
        //        return lstAsnr;
        //    }
        //    catch
        //    {
        //        return new List<Assigner>();
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        #endregion

        #region Get Assigner EmpID
        public int GetAsnrEmpID(string Domain, string Country)
        {
            int empID = 0;
            try
            {
                cmd = new SqlCommand("SP_GetAsnrEmpID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Domain", SqlDbType.VarChar).Value = Domain;
                cmd.Parameters.Add("@Country", SqlDbType.VarChar).Value = Country;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    empID = (int)dr["EmpID"];
                }
                return empID;
            }
            catch
            {
                return empID;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region GetUrlByUrlID
        //public string GetUrlByUrlID(int urlID)
        //{
        //    string urlName = "";
        //    try
        //    {
        //        cmd = new SqlCommand("SpGetUrlByUrlID", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@urlID", SqlDbType.Int).Value = urlID;
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            urlName = dr["UrlName"].ToString();
        //        }
        //        return urlName;
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        #endregion
        //remove after assigner implementation
        #region GetAsnrDomainName
        //public string GetAsnrDomainName(string urlName)
        //{
        //    string domainName = "";
        //    try
        //    {
        //        cmd = new SqlCommand("SpGetAsnrDomainName", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@urlName", SqlDbType.NVarChar).Value = urlName;
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            domainName = dr["Domain"].ToString();
        //        }
        //        return domainName;
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        #endregion
        //remove after assigner implementation
        #region Read Assigner Details
        //public List<Assigner> GetAssignerDtls()
        //{
        //    try
        //    {
        //        List<Assigner> lstAsnr = new List<Assigner>();
        //        cmd = new SqlCommand("SpGetAssignerDtls", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            Assigner asnr = new Assigner();
        //            //asnr.FullName = dr["FullName"].ToString();
        //            asnr.EmpID = (int)dr["EmpID"];
        //            asnr.StartTime = dr["StartTime"].ToString();
        //            asnr.EndTime = dr["EndTime"].ToString();
        //            asnr.Switch = dr["Switch"].ToString();
        //            lstAsnr.Add(asnr);
        //        }
        //        return lstAsnr;
        //    }
        //    catch
        //    {
        //        return new List<Assigner>();
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        #endregion

        #region Get Assigner Employee and Domain in Search Area
        public DataSet GetAsnrInSearchArea()
        {
            try
            {
                cmd = new SqlCommand("SP_GetAsnrInSearchArea", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                return ds;
            }
            catch
            {
                return new DataSet();
            }
        }
        #endregion

        #region Delete AssignerLeads Details
        public bool DeleteAssigner(int rowID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SpDeleteAssigner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@rowID", SqlDbType.Int).Value = rowID;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Delete AsnrBy Domain Details
        public bool DeleteAsnrByDomain(int rowID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SpDeleteAsnrByDomain", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@rowID", SqlDbType.Int).Value = rowID;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        //Only for hello travel
        public bool InsertLead1(LeadDtls ldDtls)
        {
            cmd = new SqlCommand("insert into lead(Cname,EmailId,Country,Phone,Requirements,SessionValue) values(@Cname,@EmailId,@Country,@Phone,@Requirements,@SessionValue) ", con);
            cmd.Parameters.Add("@Cname", SqlDbType.VarChar).Value = ldDtls.Name;
            cmd.Parameters.Add("@EmailID", SqlDbType.VarChar).Value = ldDtls.EmailID;
            cmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = ldDtls.Phone;
            cmd.Parameters.Add("@Country", SqlDbType.VarChar).Value = ldDtls.Country;
            cmd.Parameters.Add("@Requirements", SqlDbType.NVarChar).Value = ldDtls.Requirements;
            cmd.Parameters.Add("@SessionValue", SqlDbType.VarChar).Value = ldDtls.SessionValue;
            con.Open();
            int count = cmd.ExecuteNonQuery();
            con.Close();
            if (count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Insert lead code
        /// </summary>
        /// <param name="ldDtls"></param>
        /// <param name="empID"></param>
        /// <returns></returns>
        public bool InsertLead(LeadDtls ldDtls, UrlDtls urlDtls)
        {
            try
            {
                cmd = new SqlCommand("Sp_InsertLead", con);
                cmd.CommandType = CommandType.StoredProcedure;
                /*************************COMMON PARAMETERS**********************************************/
                cmd.Parameters.Add("@EmpID", SqlDbType.Int).Value = ldDtls.EmpID;
                cmd.Parameters.Add("@Tag", SqlDbType.Char).Value = ldDtls.Tag;
                cmd.Parameters.Add("@IpAddress", SqlDbType.VarChar).Value = ldDtls.IpAddress;
                cmd.Parameters.Add("@EnquirePage", SqlDbType.NVarChar).Value = ldDtls.EnquirePage;
                cmd.Parameters.Add("@ReferralUrl", SqlDbType.NVarChar).Value = ldDtls.ReferralUrl;
                cmd.Parameters.Add("@SessionValue", SqlDbType.VarChar).Value = ldDtls.SessionValue;
                if (ldDtls.AssignedTo == 0)
                {
                    cmd.Parameters.Add("@AssignedTo", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@AssignedTo", SqlDbType.Int).Value = ldDtls.AssignedTo;
                }
                if (ldDtls.AssignedBy == 0)
                {
                    cmd.Parameters.Add("@AssignedBy", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@AssignedBy", SqlDbType.Int).Value = ldDtls.AssignedBy;
                }
                cmd.Parameters.Add("@AssignedDateTime", SqlDbType.DateTime).Value = ldDtls.AssignedDateTime == Convert.ToDateTime("01/01/0001") ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(ldDtls.AssignedDateTime);
                cmd.Parameters.Add("@SourceID", SqlDbType.Int).Value = ldDtls.SourceID;
                cmd.Parameters.Add("@UrlID", SqlDbType.Int).Value = ldDtls.UrlID;
                cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = ldDtls.StatusID;
                cmd.Parameters.Add("@Cname", SqlDbType.VarChar).Value = ldDtls.Name;
                cmd.Parameters.Add("@EmailID", SqlDbType.VarChar).Value = ldDtls.EmailID;
                cmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = ldDtls.Phone;
                cmd.Parameters.Add("@Country", SqlDbType.VarChar).Value = ldDtls.Country;
                cmd.Parameters.Add("@Requirements", SqlDbType.NVarChar).Value = ldDtls.Requirements;
                /**************************************************************************************************/
                cmd.Parameters.Add("@CheckInDate", SqlDbType.DateTime).Value = ldDtls.CheckInDate;
                cmd.Parameters.Add("@CheckOutDate", SqlDbType.DateTime).Value = ldDtls.CheckOutDate;
                /*************************TOUR PARAMETERS**********************************************/
                if (urlDtls.UrlType == "Tour")
                {
                    cmd.Parameters.Add("@TourName", SqlDbType.VarChar).Value = ldDtls.TourName;
                    cmd.Parameters.Add("@TravelDate", SqlDbType.DateTime).Value = ldDtls.DateOfTravel;// == Convert.ToDateTime("01/01/0001") ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(ldDtls.DateOfTravel);
                    cmd.Parameters.Add("@Duration", SqlDbType.Int).Value = ldDtls.DurationOfTrip;
                    cmd.Parameters.Add("@HotelCateogry", SqlDbType.VarChar).Value = ldDtls.HotelCateogry;
                    cmd.Parameters.Add("@NoOfGuest", SqlDbType.Int).Value = ldDtls.NoOfGuest;
                }
                /*************************HOTEL PARAMETERS**********************************************/
                else if (urlDtls.UrlType == "Hotel")
                {
                    cmd.Parameters.Add("@HotelName", SqlDbType.VarChar).Value = ldDtls.HotelName;
                    cmd.Parameters.Add("@SingleRoom", SqlDbType.Int).Value = ldDtls.SingleRoom;
                    cmd.Parameters.Add("@DoubleRoom", SqlDbType.Int).Value = ldDtls.DoubleRoom;
                    cmd.Parameters.Add("@TripleRoom", SqlDbType.Int).Value = ldDtls.TripleRoom;
                }
                /*************************WEDDING PARAMETERS**********************************************/
                else if (urlDtls.UrlType == "Wedding")
                {
                    cmd.Parameters.Add("@WeddingType", SqlDbType.VarChar).Value = ldDtls.WeddingType;
                    cmd.Parameters.Add("@WeddingVanue", SqlDbType.VarChar).Value = ldDtls.WeddingVanue;
                    cmd.Parameters.Add("@WeddingDate", SqlDbType.DateTime).Value = ldDtls.WeddingDate;// == Convert.ToDateTime("01/01/0001") ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(ldDtls.WeddingDate);
                    cmd.Parameters.Add("@WeddingPlace", SqlDbType.VarChar).Value = ldDtls.WeddingPlace;
                    cmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = ldDtls.Address;
                    cmd.Parameters.Add("@Budget", SqlDbType.Money).Value = ldDtls.Budget;
                    cmd.Parameters.Add("@Guest", SqlDbType.VarChar).Value = ldDtls.Guest;
                }
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        /// <summary>
        /// end code
        /// </summary>

        #region Delete Multiple SiteUrl
        public void DeleteMultipleUrl(ArrayList urlIDToDelete)
        {
            foreach (int id in urlIDToDelete)
            {
                cmd = new SqlCommand("delete from SiteUrl where UrlID=@id", con);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            }
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string msg = "Deletion Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Select UnassignedLeads
        public DataSet UnassignedLeads(int index, string searchItem, int rowsPerPage, int pageNum, string dateFrom, string dateTo)
        {
            cmd = new SqlCommand("SpUnassignedLeads", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@rowsPerPage", SqlDbType.Int).Value = rowsPerPage;
            cmd.Parameters.Add("@pageNum", SqlDbType.Int).Value = pageNum;
            cmd.Parameters.Add("@dateFrom", SqlDbType.NVarChar).Value = dateFrom;
            cmd.Parameters.Add("@dateTo", SqlDbType.NVarChar).Value = dateTo;
            cmd.Parameters.Add("@t_Name", SqlDbType.NVarChar).Value = index == 1 ? searchItem : "";
            cmd.Parameters.Add("@h_Name", SqlDbType.NVarChar).Value = index == 1 ? searchItem : "";
            cmd.Parameters.Add("@t_EmailID", SqlDbType.NVarChar).Value = index == 2 ? searchItem : "";
            cmd.Parameters.Add("@h_EmailID", SqlDbType.NVarChar).Value = index == 2 ? searchItem : "";
            cmd.Parameters.Add("@t_Country", SqlDbType.NVarChar).Value = index == 3 ? searchItem : "";
            cmd.Parameters.Add("@h_Country", SqlDbType.NVarChar).Value = index == 3 ? searchItem : "";
            cmd.Parameters.Add("@t_Phone", SqlDbType.NVarChar).Value = index == 4 ? searchItem : "";
            cmd.Parameters.Add("@h_Phone", SqlDbType.NVarChar).Value = index == 4 ? searchItem : "";
            cmd.Parameters.Add("@urlName", SqlDbType.NVarChar).Value = index == 5 ? searchItem : "";
            cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = index == 6 ? searchItem : "";
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
        #endregion

        #region Select All Leads IN QUERY BANK
        public DataSet GetAllLeads(int rowsPerPage, int pageNum)
        {
            cmd = new SqlCommand("SpSelectAllLeads1", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@rowsPerPage", SqlDbType.Int).Value = rowsPerPage;
            cmd.Parameters.Add("@pageNum", SqlDbType.Int).Value = pageNum;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
        #endregion

        #region Select Rest Leads
        public int GetRestLeads(int assigner, int empID)
        {
            try
            {
                cmd = new SqlCommand("SpAssignRestLeads", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assigner", SqlDbType.Int).Value = assigner;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                cmd.Parameters.Add("@assignBy", SqlDbType.Int).Value = System.DBNull.Value;
                cmd.Parameters.Add("@assignTo", SqlDbType.Int).Value = System.DBNull.Value;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);

                if (count >= 1)
                {
                    return count;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;

            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Get Duplicate Leads
        public DataSet GetDuplicateLeads(int rowsPerPage, int pageNum)
        {
            cmd = new SqlCommand("SpGetDuplicateLeads", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@rowsPerPage", SqlDbType.Int).Value = rowsPerPage;
            cmd.Parameters.Add("@pageNum", SqlDbType.Int).Value = pageNum;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
        #endregion

        #region Get Trash Leads
        public DataSet GetTrashLeads(int rowsPerPage, int pageNum)
        {
            cmd = new SqlCommand("SpGetTrashLeads", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@rowsPerPage", SqlDbType.Int).Value = rowsPerPage;
            cmd.Parameters.Add("@pageNum", SqlDbType.Int).Value = pageNum;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
        #endregion

        #region Get Reports By MGR,TL & EXE.
        public DataTable GetReport(int LoginEmpID, int Profileid, int EmpId, int StatusID, string UrlType, int SourceID, int UrlID, string DateFrom, string HF, string MF, string DateTo, string HT, string MT)
        {
            cmd = new SqlCommand("GETREPORTBYMTE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LoginEmpID", SqlDbType.Int).Value = LoginEmpID;
            cmd.Parameters.Add("@Profileid", SqlDbType.Int).Value = Profileid;
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = StatusID;
            cmd.Parameters.Add("@UrlType", SqlDbType.NVarChar).Value = UrlType;
            cmd.Parameters.Add("@SourceID", SqlDbType.Int).Value = SourceID;
            cmd.Parameters.Add("@UrlID", SqlDbType.Int).Value = UrlID;
            cmd.Parameters.Add("@DateFrom", SqlDbType.NVarChar).Value = DateFrom;
            cmd.Parameters.Add("@HF", SqlDbType.NVarChar).Value = HF;
            cmd.Parameters.Add("@MF", SqlDbType.NVarChar).Value = MF;
            cmd.Parameters.Add("@DateTo", SqlDbType.NVarChar).Value = DateTo;
            cmd.Parameters.Add("@HT", SqlDbType.NVarChar).Value = HT;
            cmd.Parameters.Add("@MT", SqlDbType.NVarChar).Value = MT;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Get Reports By Admin
        public DataTable GetReport1(string flag, int Profileid, int EmpId, int Criteria, int StatusID, string UrlType, int SourceID, int UrlID, string Country, string SessionVal, string PpcUrl, string DateFrom, string HF, string MF, string DateTo, string HT, string MT)
        {
            cmd = new SqlCommand("GETREPORTBYADMIN", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Flag", SqlDbType.Char).Value = flag;
            cmd.Parameters.Add("@Profileid", SqlDbType.Int).Value = Profileid;
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@Criteria", SqlDbType.Int).Value = Criteria;
            cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = StatusID;
            cmd.Parameters.Add("@UrlType", SqlDbType.NVarChar).Value = UrlType;
            cmd.Parameters.Add("@SourceID", SqlDbType.Int).Value = SourceID;
            cmd.Parameters.Add("@UrlID", SqlDbType.Int).Value = UrlID;
            cmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = Country;
            cmd.Parameters.Add("@SessionVal", SqlDbType.NVarChar).Value = SessionVal;
            cmd.Parameters.Add("@PpcUrl", SqlDbType.NVarChar).Value = PpcUrl;
            cmd.Parameters.Add("@DateFrom", SqlDbType.NVarChar).Value = DateFrom;
            cmd.Parameters.Add("@HF", SqlDbType.NVarChar).Value = HF;
            cmd.Parameters.Add("@MF", SqlDbType.NVarChar).Value = MF;
            cmd.Parameters.Add("@DateTo", SqlDbType.NVarChar).Value = DateTo;
            cmd.Parameters.Add("@HT", SqlDbType.NVarChar).Value = HT;
            cmd.Parameters.Add("@MT", SqlDbType.NVarChar).Value = MT;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region All Manager Leads Report
        //public DataTable GetAllMgrReports(DateTime dateFrom, DateTime dateTo)
        //{

        //    //if (dateFrom != Convert.ToDateTime("1/1/1753 12:00:00 AM") && dateTo != Convert.ToDateTime("1/1/1753 12:00:00 AM"))
        //    //{
        //    //    cmd = new SqlCommand("SpAllMgrReport", con);
        //    //    cmd.CommandType = CommandType.StoredProcedure;
        //    //    cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = dateFrom;
        //    //    cmd.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = dateTo;
        //    //}
        //    //else
        //    //{
        //        cmd = new SqlCommand("SpAllMgrReport", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = dateFrom;
        //        cmd.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = dateTo;
        //    //}
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    sda.Fill(dt);
        //    return dt;
        //}
        #endregion

        #region Tl Leads Report
        public DataTable GetReports(DateTime dateFrom, DateTime dateTo, int empID)
        {

            //if (dateFrom != Convert.ToDateTime("1/1/1753 12:00:00 AM") && dateTo != Convert.ToDateTime("1/1/1753 12:00:00 AM"))
            //{
            //    cmd = new SqlCommand("SpTlReport", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = dateFrom;
            //    cmd.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = dateTo;
            //}
            //else
            //{
            cmd = new SqlCommand("SpReports", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = dateFrom;
            cmd.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = dateTo;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            // }
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Tl/Exe Leads Report
        public DataTable GetLeadsReport(DateTime dateFrom, DateTime dateTo, int empID)
        {

            if (dateFrom != Convert.ToDateTime("1/1/1753 12:00:00 AM") && dateTo != Convert.ToDateTime("1/1/1753 12:00:00 AM"))
            {
                cmd = new SqlCommand("SpLeadsReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = dateTo;
                cmd.Parameters.Add("@employeeID", SqlDbType.Int).Value = empID;
            }
            else
            {
                cmd = new SqlCommand("SpLeadsReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = dateTo;
                cmd.Parameters.Add("@employeeID", SqlDbType.Int).Value = empID;
            }
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Exe Leads Report
        public DataTable GetExeLeadsReport(DateTime dateFrom, DateTime dateTo)
        {

            if (dateFrom != Convert.ToDateTime("1/1/1753 12:00:00 AM") && dateTo != Convert.ToDateTime("1/1/1753 12:00:00 AM"))
            {
                cmd = new SqlCommand("SpExeLeadsReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = dateTo;
            }
            else
            {
                cmd = new SqlCommand("SpExeLeadsReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = dateTo;
            }
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Select Mgr/Tl Leads
        public DataSet GetLeads(int index, string searchItem, int rowsPerPage, int pageNum, string dateFrom, string dateTo, int empID)
        {
            cmd = new SqlCommand("SpGetLeads1", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@rowsPerPage", SqlDbType.Int).Value = rowsPerPage;
            cmd.Parameters.Add("@pageNum", SqlDbType.Int).Value = pageNum;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            cmd.Parameters.Add("@dateFrom", SqlDbType.NVarChar).Value = dateFrom;
            cmd.Parameters.Add("@dateTo", SqlDbType.NVarChar).Value = dateTo;
            cmd.Parameters.Add("@t_Name", SqlDbType.NVarChar).Value = index == 1 ? searchItem : "";
            cmd.Parameters.Add("@h_Name", SqlDbType.NVarChar).Value = index == 1 ? searchItem : "";
            cmd.Parameters.Add("@t_EmailID", SqlDbType.NVarChar).Value = index == 2 ? searchItem : "";
            cmd.Parameters.Add("@h_EmailID", SqlDbType.NVarChar).Value = index == 2 ? searchItem : "";
            cmd.Parameters.Add("@t_Country", SqlDbType.NVarChar).Value = index == 3 ? searchItem : "";
            cmd.Parameters.Add("@h_Country", SqlDbType.NVarChar).Value = index == 3 ? searchItem : "";
            cmd.Parameters.Add("@t_Phone", SqlDbType.NVarChar).Value = index == 4 ? searchItem : "";
            cmd.Parameters.Add("@h_Phone", SqlDbType.NVarChar).Value = index == 4 ? searchItem : "";
            cmd.Parameters.Add("@urlName", SqlDbType.NVarChar).Value = index == 5 ? searchItem : "";
            cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = index == 6 ? searchItem : "";
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
        #endregion

        #region Get Mgr Trash Leads
        public DataSet GetMgrTrLeads(int rowsPerPage, int pageNum, int empID)
        {

            cmd = new SqlCommand("SpMgrTrLeads", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            cmd.Parameters.Add("@rowsPerPage", SqlDbType.Int).Value = rowsPerPage;
            cmd.Parameters.Add("@pageNum", SqlDbType.Int).Value = pageNum;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
        #endregion

        #region Select Self Assigned Mgr/Tl Leads
        public DataSet GetSelfLeads(int index, string searchItem, int rowsPerPage, int pageNum, string dateFrom, string dateTo, int empID)
        {

            cmd = new SqlCommand("SpGetSelfAssignedLeads1", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@rowsPerPage", SqlDbType.Int).Value = rowsPerPage;
            cmd.Parameters.Add("@pageNum", SqlDbType.Int).Value = pageNum;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            cmd.Parameters.Add("@t_Name", SqlDbType.NVarChar).Value = index == 1 ? searchItem : "";
            cmd.Parameters.Add("@h_Name", SqlDbType.NVarChar).Value = index == 1 ? searchItem : ""; ;
            cmd.Parameters.Add("@t_EmailID", SqlDbType.NVarChar).Value = index == 2 ? searchItem : ""; ;
            cmd.Parameters.Add("@h_EmailID", SqlDbType.NVarChar).Value = index == 2 ? searchItem : ""; ;
            cmd.Parameters.Add("@t_Country", SqlDbType.NVarChar).Value = index == 3 ? searchItem : ""; ;
            cmd.Parameters.Add("@h_Country", SqlDbType.NVarChar).Value = index == 3 ? searchItem : ""; ;
            cmd.Parameters.Add("@t_Phone", SqlDbType.NVarChar).Value = index == 4 ? searchItem : ""; ;
            cmd.Parameters.Add("@h_Phone", SqlDbType.NVarChar).Value = index == 4 ? searchItem : ""; ;
            cmd.Parameters.Add("@urlName", SqlDbType.NVarChar).Value = index == 5 ? searchItem : ""; ;
            cmd.Parameters.Add("@dateFrom", SqlDbType.NVarChar).Value = dateFrom;
            cmd.Parameters.Add("@dateTo", SqlDbType.NVarChar).Value = dateTo;
            cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = index == 6 ? searchItem : ""; ;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
        #endregion

        #region Get Assigned Leads Details By Admin,Mgr,Tl & Asd to Exe
        public DataSet GetAsssignLeads(int index, string searchItem, int rowsPerPage, int pageNum, string dateFrom, string dateTo, int empID)
        {

            cmd = new SqlCommand("SpGetAssignedLeads1", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@rowsPerPage", SqlDbType.Int).Value = rowsPerPage;
            cmd.Parameters.Add("@pageNum", SqlDbType.Int).Value = pageNum;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            cmd.Parameters.Add("@dateFrom", SqlDbType.NVarChar).Value = dateFrom;
            cmd.Parameters.Add("@dateTo", SqlDbType.NVarChar).Value = dateTo;
            cmd.Parameters.Add("@t_Name", SqlDbType.NVarChar).Value = index == 1 ? searchItem : "";
            cmd.Parameters.Add("@h_Name", SqlDbType.NVarChar).Value = index == 1 ? searchItem : "";
            cmd.Parameters.Add("@t_EmailID", SqlDbType.NVarChar).Value = index == 2 ? searchItem : "";
            cmd.Parameters.Add("@h_EmailID", SqlDbType.NVarChar).Value = index == 2 ? searchItem : "";
            cmd.Parameters.Add("@t_Country", SqlDbType.NVarChar).Value = index == 3 ? searchItem : "";
            cmd.Parameters.Add("@h_Country", SqlDbType.NVarChar).Value = index == 3 ? searchItem : "";
            cmd.Parameters.Add("@t_Phone", SqlDbType.NVarChar).Value = index == 4 ? searchItem : "";
            cmd.Parameters.Add("@h_Phone", SqlDbType.NVarChar).Value = index == 4 ? searchItem : "";
            cmd.Parameters.Add("@urlName", SqlDbType.NVarChar).Value = index == 5 ? searchItem : "";
            cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = index == 6 ? searchItem : "";
            cmd.Parameters.Add("@employee", SqlDbType.NVarChar).Value = index == 7 ? searchItem : "";
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
        #endregion

        #region Select Lead Details
        public LeadDtls GetLeadDetails(long LeadID)
        {
            LeadDtls comnLdDtls = new LeadDtls();
            try
            {
                cmd = new SqlCommand("SP_GET_COMMON_LDDTLS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LeadID", SqlDbType.BigInt).Value = LeadID;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        comnLdDtls.LeadID = (long)dr["LeadID"];
                        comnLdDtls.IpAddress = (dr["IpAddress"] == System.DBNull.Value ? "" : (string)dr["IpAddress"]);
                        comnLdDtls.EnquirePage = (dr["EnquirePage"] == System.DBNull.Value ? "" : (string)dr["EnquirePage"]);
                        comnLdDtls.ReferralUrl = (dr["ReferralUrl"] == System.DBNull.Value ? "" : (string)dr["ReferralUrl"]);
                        comnLdDtls.EnquireBy = (dr["EnquireEmp"] == System.DBNull.Value ? "" : (string)dr["EnquireEmp"]);
                        comnLdDtls.DomainName = (dr["UrlName"] == System.DBNull.Value ? "" : (string)dr["UrlName"]);
                        comnLdDtls.SourceType = (dr["SourceType"] == System.DBNull.Value ? "" : (string)dr["SourceType"]);
                        comnLdDtls.SourceID = (dr["SourceID"] == System.DBNull.Value ? 0 : (int)dr["SourceID"]);
                        comnLdDtls.SessionValue = (dr["SessionValue"] == System.DBNull.Value ? "" : (string)dr["SessionValue"]);
                        comnLdDtls.StatusID = (dr["StatusID"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["StatusID"]));
                        comnLdDtls.StatusName = (dr["Status"] == System.DBNull.Value ? "" : (string)dr["Status"]);
                        comnLdDtls.LeadDate = (dr["QueryDate"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["QueryDate"]);
                        comnLdDtls.Remark = (dr["Remark"] == System.DBNull.Value ? "" : (string)dr["Remark"]);
                        comnLdDtls.ReminderDate = (dr["ReminderDate"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["ReminderDate"]);
                        comnLdDtls.ReminderTime = (dr["ReminderTime"] == System.DBNull.Value ? "00:00" : (string)dr["ReminderTime"]);
                        comnLdDtls.ReminderMessage = (dr["ReminderMessage"] == System.DBNull.Value ? "" : (string)dr["ReminderMessage"]);
                        comnLdDtls.Name = (dr["Cname"] == System.DBNull.Value ? "" : (string)dr["Cname"]);
                        comnLdDtls.EmailID = (dr["EmailID"] == System.DBNull.Value ? "" : (string)dr["EmailID"]);
                        comnLdDtls.Phone = (dr["Phone"] == System.DBNull.Value ? "0" : (string)dr["Phone"]);
                        comnLdDtls.Country = (dr["Country"] == System.DBNull.Value ? "0" : (string)dr["Country"]);
                        comnLdDtls.Requirements = (dr["Requirements"] == System.DBNull.Value ? "" : (string)dr["Requirements"]);
                        comnLdDtls.UrlType = (dr["UrlType"] == System.DBNull.Value ? "" : (string)dr["UrlType"]);

                        if (comnLdDtls.UrlType == "Tour")
                        {
                            comnLdDtls.TourName = (dr["TourName"] == System.DBNull.Value ? "" : (string)dr["TourName"]);
                            comnLdDtls.DateOfTravel = (dr["TravelDate"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["TravelDate"]);
                            comnLdDtls.DurationOfTrip = (dr["Duration"] == System.DBNull.Value ? 0 : (int)dr["Duration"]);
                            comnLdDtls.NoOfGuest = (dr["NoOfGuest"] == System.DBNull.Value ? 0 : (int)dr["NoOfGuest"]);
                            comnLdDtls.HotelCateogry = (dr["HotelCateogry"] == System.DBNull.Value ? "0" : (string)dr["HotelCateogry"]);
                        }
                        else if (comnLdDtls.UrlType == "Hotel")
                        {
                            comnLdDtls.HotelName = (dr["HOTELNAME"] == System.DBNull.Value ? "" : (string)dr["HOTELNAME"]);
                            comnLdDtls.CheckInDate = (dr["CHECKINDATE"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["CHECKINDATE"]);
                            comnLdDtls.CheckOutDate = (dr["CHECKOUTDATE"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["CHECKOUTDATE"]);
                            comnLdDtls.SingleRoom = (dr["SINGLEROOM"] == System.DBNull.Value ? 0 : (int)dr["SINGLEROOM"]);
                            comnLdDtls.DoubleRoom = (dr["DOUBLEROOM"] == System.DBNull.Value ? 0 : (int)dr["DOUBLEROOM"]);
                            comnLdDtls.TripleRoom = (dr["TRIPLEROOM"] == System.DBNull.Value ? 0 : (int)dr["TRIPLEROOM"]);
                        }
                        else if (comnLdDtls.UrlType == "Wedding")
                        {
                            comnLdDtls.WeddingType = dr["WeddingType"].ToString();
                            comnLdDtls.WeddingVanue = dr["WeddingVanue"].ToString();
                            comnLdDtls.WeddingDate = (DateTime)dr["WeddingDate"];
                            comnLdDtls.WeddingPlace = dr["WeddingPlace"].ToString();
                            comnLdDtls.Guest = dr["Guest"].ToString();
                            comnLdDtls.Budget = (decimal)dr["Budget"];
                            comnLdDtls.CheckInDate = (DateTime)dr["WCheckInDate"];
                            comnLdDtls.CheckOutDate = (DateTime)dr["WCheckOutDate"];
                            comnLdDtls.Address = dr["Address"].ToString();
                        }
                    }
                }
            }
            catch
            {
                return new LeadDtls();
            }
            finally
            {
                con.Close();
            }
            return comnLdDtls;
        }
        #endregion

        #region Select Lead Details for Sending Mail
        public LeadDtls GetLeadDtlsForSendMail(long LeadID)
        {
            LeadDtls mailldDtls = new LeadDtls();
            try
            {
                cmd = new SqlCommand("SP_GetldDtlsForMail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LeadID", SqlDbType.BigInt).Value = LeadID;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mailldDtls.LeadDate = (dr["QueryDate"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["QueryDate"]);
                        mailldDtls.Name = (dr["Cname"] == System.DBNull.Value ? "" : (string)dr["Cname"]);
                        mailldDtls.EmailID = (dr["EmailID"] == System.DBNull.Value ? "" : (string)dr["EmailID"]);
                        mailldDtls.Phone = (dr["Phone"] == System.DBNull.Value ? "0" : (string)dr["Phone"]);
                        mailldDtls.Country = (dr["Country"] == System.DBNull.Value ? "0" : (string)dr["Country"]);
                        mailldDtls.Requirements = (dr["Requirements"] == System.DBNull.Value ? "" : (string)dr["Requirements"]);
                        mailldDtls.UrlType = (dr["UrlType"] == System.DBNull.Value ? "" : (string)dr["UrlType"]);

                        if (mailldDtls.UrlType == "Tour")
                        {
                            mailldDtls.TourName = (dr["TourName"] == System.DBNull.Value ? "" : (string)dr["TourName"]);
                            mailldDtls.DateOfTravel = (dr["TravelDate"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["TravelDate"]);
                            mailldDtls.DurationOfTrip = (dr["Duration"] == System.DBNull.Value ? 0 : (int)dr["Duration"]);
                            mailldDtls.NoOfGuest = (dr["NoOfGuest"] == System.DBNull.Value ? 0 : (int)dr["NoOfGuest"]);
                            mailldDtls.HotelCateogry = (dr["HotelCateogry"] == System.DBNull.Value ? "0" : (string)dr["HotelCateogry"]);
                        }
                        else if (mailldDtls.UrlType == "Hotel")
                        {
                            mailldDtls.HotelName = (dr["HOTELNAME"] == System.DBNull.Value ? "" : (string)dr["HOTELNAME"]);
                            mailldDtls.CheckInDate = (dr["CHECKINDATE"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["CHECKINDATE"]);
                            mailldDtls.CheckOutDate = (dr["CHECKOUTDATE"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["CHECKOUTDATE"]);
                            mailldDtls.SingleRoom = (dr["SINGLEROOM"] == System.DBNull.Value ? 0 : (int)dr["SINGLEROOM"]);
                            mailldDtls.DoubleRoom = (dr["DOUBLEROOM"] == System.DBNull.Value ? 0 : (int)dr["DOUBLEROOM"]);
                            mailldDtls.TripleRoom = (dr["TRIPLEROOM"] == System.DBNull.Value ? 0 : (int)dr["TRIPLEROOM"]);
                        }
                        else if (mailldDtls.UrlType == "Wedding")
                        {
                            mailldDtls.WeddingType = dr["WeddingType"].ToString();
                            mailldDtls.WeddingVanue = dr["WeddingVanue"].ToString();
                            mailldDtls.WeddingDate = (DateTime)dr["WeddingDate"];
                            mailldDtls.WeddingPlace = dr["WeddingPlace"].ToString();
                            mailldDtls.Guest = dr["Guest"].ToString();
                            mailldDtls.Budget = (decimal)dr["Budget"];
                            mailldDtls.CheckInDate = (DateTime)dr["WCheckInDate"];
                            mailldDtls.CheckOutDate = (DateTime)dr["WCheckOutDate"];
                            mailldDtls.Address = dr["Address"].ToString();
                        }
                    }
                }
            }
            catch
            {
                return new LeadDtls();
            }
            finally
            {
                con.Close();
            }
            return mailldDtls;
        }
        #endregion

        public UrlDtls checkUrl(int urlID)
        {
            try
            {
                UrlDtls urlDtls = new UrlDtls();
                cmd = new SqlCommand("select UrlType,UrlName,SourceID from SiteUrl where UrlID=@urlID", con);
                cmd.Parameters.Add("@urlID", SqlDbType.Int).Value = urlID;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    urlDtls.UrlType = (string)dr["UrlType"];
                    urlDtls.DomainName = (string)dr["UrlName"];
                    urlDtls.SourceID = (int)dr["SourceID"];
                }
                return urlDtls;
            }
            catch
            {
                return new UrlDtls();
            }
            finally
            {
                con.Close();
            }
        }

        #region Delete Lead
        public bool DeleteLead(long leadID)
        {
            cmd = new SqlCommand("Delete FROM Leads WHERE LeadID=@leadID;Delete FROM StatusLog WHERE LeadID=@leadID;Delete FROM Reminder WHERE LeadID=@leadID;", con);
            cmd.Parameters.Add("@leadID", SqlDbType.Int).Value = leadID;
            try
            {
                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteNonQuery());
                if (count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                string msg = "Deletion Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Delete Leads
        public void DeleteLeads(ArrayList LeadToDelete)
        {
            foreach (var leadID in LeadToDelete)
            {
                cmd = new SqlCommand("SpDeleteLeads", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = leadID;
            }
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string msg = "Deletion Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Get All Employee
        public DataTable GetAllEmployee()
        {
            cmd = new SqlCommand("SpSelectAllEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Get Assigner
        public DataTable GetAssigner()
        {
            cmd = new SqlCommand("SpGetAssignerToFetch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Get Employee To Assign
        public DataTable GetEmployeeToAsd(int empID)
        {
            cmd = new SqlCommand("SpGetEmployeeToAsd", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Assigned Lead
        public void AssignLeads(ArrayList LeadToAssign, int assignedID, int empID)
        {
            try
            {
                DateTime datetime = new DateTime();
                datetime = DateTime.Now;
                foreach (var leadID in LeadToAssign)
                {
                    cmd = new SqlCommand("SpAssignLeads", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = leadID;
                    cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                    cmd.Parameters.Add("@assignedID", SqlDbType.Int).Value = assignedID;
                    cmd.Parameters.Add("@datetime", SqlDbType.DateTime).Value = datetime;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                string msg = "Asigned Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
        }
        #endregion

        #region Assigned Lead through Leadstatus page
        public void AsgnLds(long leadID, int assignedID, int empID)
        {
            try
            {
                DateTime datetime = new DateTime();
                datetime = DateTime.Now;
                cmd = new SqlCommand("SpAssignLeads", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = leadID;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                cmd.Parameters.Add("@assignedID", SqlDbType.Int).Value = assignedID;
                cmd.Parameters.Add("@datetime", SqlDbType.DateTime).Value = datetime;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                string msg = "Asigned Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
        }
        #endregion

        #region Check Duplicate Leads
        public void CheckDupLeads(ArrayList chkDupLeads)
        {
            try
            {
                foreach (var leadID in chkDupLeads)
                {
                    cmd = new SqlCommand("SpChkDupLeads", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = leadID;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string msg = "Check Duplicate Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
        }
        #endregion

        #region Move Leads In Trash
        public void MoveInTrash(ArrayList trleads)
        {
            try
            {
                foreach (var leadID in trleads)
                {
                    cmd = new SqlCommand("SpTrashLeads", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = leadID;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string msg = "Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
        }
        #endregion

        #region Restore Trash Leads
        public void ReTrLeads(ArrayList trleads)
        {
            try
            {
                foreach (var leadID in trleads)
                {
                    cmd = new SqlCommand("SpReTrLeads", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = leadID;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                string msg = "Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
        }
        #endregion

        #region Mark Duplicate Lead
        public void MarkDupLeads(ArrayList MDuplicate)
        {
            try
            {
                foreach (var leadID in MDuplicate)
                {
                    cmd = new SqlCommand("SpMarkDuplicateLeads", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = leadID;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                string msg = "Mark Duplicate Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }

        }
        #endregion

        #region UnMarkDupLeads From Leads Table
        public void UnMarkDupLeads(ArrayList UnMDuplicate)
        {
            try
            {
                foreach (var leadID in UnMDuplicate)
                {
                    cmd = new SqlCommand("SpUnMarkDupLeads", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = leadID;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                string msg = "Asigned Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }

        }
        #endregion

        #region Update Tour Lead
        public bool UpdateTourLead(Leads lead)
        {
            try
            {
                cmd = new SqlCommand("SpUpdateTourLead", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = lead.LeadID;
                cmd.Parameters.Add("@t_TourName", SqlDbType.NVarChar).Value = lead.T_TourName;
                cmd.Parameters.Add("@t_TravelDate", SqlDbType.DateTime).Value = lead.T_DoTravel;
                cmd.Parameters.Add("@t_Duration", SqlDbType.Int).Value = lead.T_DurTrip;
                cmd.Parameters.Add("@t_NoOfGuest", SqlDbType.Int).Value = lead.T_NoGuest;
                cmd.Parameters.Add("@t_HotelCateogry", SqlDbType.NVarChar).Value = lead.T_HotelCateogry;
                cmd.Parameters.Add("@t_Name", SqlDbType.NVarChar).Value = lead.T_Name;
                cmd.Parameters.Add("@t_EmailID", SqlDbType.NVarChar).Value = lead.T_EmailID;
                cmd.Parameters.Add("@t_Phone", SqlDbType.NVarChar).Value = lead.T_PhoneNo;
                cmd.Parameters.Add("@t_Country", SqlDbType.NVarChar).Value = lead.T_Country;
                cmd.Parameters.Add("@t_Requirements", SqlDbType.NVarChar).Value = lead.T_Requirements;
                //Added By Ashok Kumar on 11/11/2011
                cmd.Parameters.Add("@sourcetId", SqlDbType.BigInt).Value = lead.SourceTypeID;
                //End of Addition
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@leadID", SqlDbType.BigInt);
                returnValue.Direction = ParameterDirection.ReturnValue;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Update Hotel Lead
        public bool UpdateHotelLead(Leads lead)
        {
            try
            {
                cmd = new SqlCommand("SpUpdateHotelLead", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = lead.LeadID;
                cmd.Parameters.Add("@h_HotelName", SqlDbType.NVarChar).Value = lead.H_HotelName;
                cmd.Parameters.Add("@h_CheckInDate", SqlDbType.DateTime).Value = lead.H_CheckInDate;
                cmd.Parameters.Add("@h_CheckOutDate", SqlDbType.DateTime).Value = lead.H_CheckOutDate;
                cmd.Parameters.Add("@h_SingleRoom", SqlDbType.Int).Value = lead.H_SingleRoom;
                cmd.Parameters.Add("@h_DoubleRoom", SqlDbType.Int).Value = lead.H_DoubleRoom;
                cmd.Parameters.Add("@h_TripleRoom", SqlDbType.Int).Value = lead.H_TripleRoom;
                cmd.Parameters.Add("@h_Name", SqlDbType.NVarChar).Value = lead.H_Name;
                cmd.Parameters.Add("@h_EmailID", SqlDbType.NVarChar).Value = lead.H_EmailID;
                cmd.Parameters.Add("@h_Phone", SqlDbType.NVarChar).Value = lead.H_PhoneNo;
                cmd.Parameters.Add("@h_Country", SqlDbType.NVarChar).Value = lead.H_Country;
                cmd.Parameters.Add("@h_Requirements", SqlDbType.NVarChar).Value = lead.H_Requirements;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@leadID", SqlDbType.BigInt);
                returnValue.Direction = ParameterDirection.ReturnValue;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        public List<Reminder> GetReminder(long empid, string pagevalue)
        {
            try
            {
                List<Reminder> rmdr = new List<Reminder>();
                cmd = new SqlCommand("SPGETREMINDER", con);
                cmd.Parameters.AddWithValue("@EmpId", empid);
                cmd.Parameters.AddWithValue("@PageValue", pagevalue);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Reminder rmd = new Reminder();
                    rmd.LeadID = (long)(dr["LeadID"]);
                    rmd.ReminderID = (long)(dr["ReminderID"]);
                    rmd.ReminderDate = (DateTime)(dr["ReminderDate"]);
                    rmd.ReminderTime = (string)(dr["ReminderTime"]);
                    rmd.ReminderMessage = dr["ReminderMessage"].ToString();
                    rmdr.Add(rmd);

                }
                return rmdr;
            }
            catch
            {
                return new List<Reminder>();
            }
            finally
            {
                con.Close();
            }

        }

        #region Update Reminder when click on snooze
        public string newupdatereminder(string strvalue)
        {
            try
            {
                string timeAdd = "";
                string flag = "T";

                StringBuilder lstrXML = new StringBuilder();
                lstrXML.Append("<root>");
                string[] RemvalA = strvalue.Split(',');
                string strReminderid = string.Empty;
                string strRemdrid = string.Empty;
                string strTimevalue = string.Empty;
                for (int i = 0; i < RemvalA.Length; i++)
                {
                    string[] value = RemvalA[i].Split('^');
                    strReminderid = value[0].ToString();
                    strTimevalue = value[1].ToString();
                    string strXMLData = string.Empty;
                    if (strTimevalue == "1")
                    {
                        timeAdd = DateTime.Now.AddMinutes(5).ToString("HH:mm");
                    }
                    if (strTimevalue == "2")
                    {
                        timeAdd = DateTime.Now.AddMinutes(10).ToString("HH:mm");
                    }
                    if (strTimevalue == "3")
                    {
                        timeAdd = DateTime.Now.AddMinutes(30).ToString("HH:mm");
                    }
                    if (strTimevalue == "4")
                    {
                        timeAdd = DateTime.Now.AddHours(1).ToString("HH:mm");
                    }
                    if (strTimevalue == "5")
                    {
                        timeAdd = DateTime.Now.AddHours(2).ToString("HH:mm");
                    }
                    if (strTimevalue == "6")
                    {
                        timeAdd = DateTime.Now.AddDays(1).ToShortDateString();
                        flag = "D";
                    }
                    if (strTimevalue == "7")
                    {
                        timeAdd = DateTime.Now.AddDays(7).ToShortDateString();
                        flag = "D";
                    }
                    if (strTimevalue == "8")
                    {
                        timeAdd = DateTime.Now.AddMonths(1).ToShortDateString();
                        flag = "D";
                    }

                    if (flag == "T")
                    {
                        strXMLData = " UPDATE REMINDER SET REMINDERTIME='" + timeAdd + "' WHERE REMINDERID=" + strReminderid + "";
                    }
                    if (flag == "D")
                    {
                        strXMLData = " UPDATE REMINDER SET REMINDERDATE='" + timeAdd + "' WHERE REMINDERID=" + strReminderid + "";
                    }
                    lstrXML.Append("<row Query=\"" + strXMLData + "\"/>");

                    // code for sending the reminders id for aspx.cs file
                    if (strRemdrid == "")
                    {
                        strRemdrid = value[0].ToString();
                    }
                    else
                    {
                        strRemdrid = strRemdrid + "," + value[0].ToString();
                    }
                }
                lstrXML.Append("</root>");
                cmd = new SqlCommand("UpdateReminder", con);
                cmd.Parameters.AddWithValue("@strXmldata", lstrXML.ToString());
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                return strRemdrid;
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion Update Reminder when click on snooze

        #region Get Reninder Details in leadstatus page
        public Reminder GetReminderDetails(long reminderID)
        {
            try
            {
                cmd = new SqlCommand("GetReminderDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@reminderID", SqlDbType.BigInt).Value = reminderID;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rm.ReminderDate = (DateTime)(dr["ReminderDate"]);
                    rm.ReminderTime = (string)(dr["ReminderTime"]);
                    rm.ReminderMessage = dr["ReminderMessage"].ToString();
                }
                return rm;
            }
            catch
            {
                return new Reminder();
            }
            finally
            {
                con.Close();
            }

        }
        #endregion

        #region Update Reminder
        public bool UpdateReminder(Reminder rm)
        {
            try
            {
                cmd = new SqlCommand("SpUpdateReminder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@reminderID", SqlDbType.BigInt).Value = rm.ReminderID;
                cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = rm.LeadID;
                cmd.Parameters.Add("@reminderDate", SqlDbType.DateTime).Value = rm.ReminderDate;
                cmd.Parameters.Add("@reminderTime", SqlDbType.NVarChar).Value = rm.ReminderTime;
                cmd.Parameters.Add("@reminderMessage", SqlDbType.NVarChar).Value = rm.ReminderMessage;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region  Insert StatusLog By Welcome Page
        public bool InsertStatusLogByWelcome(StatusLog sl)
        {
            try
            {
                cmd = new SqlCommand("InsertStatusLogByWelcome", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = sl.LeadID;
                cmd.Parameters.Add("@statusID", SqlDbType.Int).Value = sl.StatusID;
                cmd.Parameters.Add("@statusLogID", SqlDbType.BigInt).Value = sl.StatusLogID;
                cmd.Parameters.Add("@changeStatusID", SqlDbType.Int).Value = sl.ChangeStatusID;
                cmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = sl.Remark;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = sl.EmpID;
                cmd.Parameters.Add("@reminderDate", SqlDbType.DateTime).Value = sl.ReminderDate;
                cmd.Parameters.Add("@reminderTime", SqlDbType.NVarChar).Value = sl.ReminderTime;
                cmd.Parameters.Add("@reminderMessage", SqlDbType.NVarChar).Value = sl.ReminderMessage;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);

                if (count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region  Insert StatusLog By LeadManagement Page
        public bool InsertStatusLogByLeadMgmt(StatusLog sl)
        {
            try
            {
                cmd = new SqlCommand("SpInsertSatusLogByLeadMgmt", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = sl.LeadID;
                cmd.Parameters.Add("@previousstatusID", SqlDbType.Int).Value = sl.StatusID;
                cmd.Parameters.Add("@statusLogID", SqlDbType.BigInt).Value = sl.StatusLogID;
                cmd.Parameters.Add("@changeStatusID", SqlDbType.Int).Value = sl.ChangeStatusID;
                cmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = sl.Remark;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = sl.EmpID;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);

                if (count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Get Status Log Details
        public List<StatusLog> GetStatusLog(long leadID)
        {
            try
            {
                List<StatusLog> stLog = new List<StatusLog>();
                cmd = new SqlCommand("SpSelectStatusLog", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = leadID;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    StatusLog sl = new StatusLog();
                    sl.LeadID = (long)(dr["LeadID"]);
                    sl.Status = (string)(dr["PreviousStatus"]);
                    sl.ChangeStatusID = (int)(dr["ChangeStatusID"]);
                    sl.StatusID = (int)(dr["LeadStatusID"]);
                    sl.ChangeStatus = (string)(dr["ChangeStatus"]);
                    sl.Remark = dr["Remark"].ToString();
                    sl.FullName = (string)(dr["FullName"]);
                    sl.RemarkDate = (DateTime)(dr["RemarkDate"]);
                    stLog.Add(sl);

                }
                return stLog;
            }
            catch
            {
                return new List<StatusLog>();
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Leadmanagement Searching Area
        public List<string> GetLeadItemByUser(string prefixText, int count, string contextKey)
        {
            try
            {
                cmd = new SqlCommand("SpSelectSearchItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchText", prefixText);
                cmd.Parameters.AddWithValue("@contextKey", contextKey);
                cmd.Connection = con;
                con.Open();
                List<string> LeadItem = new List<string>();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    if (contextKey == "1")
                    {
                        LeadItem.Add(sdr["T_Name"].ToString());
                        LeadItem.Add(sdr["H_Name"].ToString());
                    }
                    else if (contextKey == "2")
                    {
                        LeadItem.Add(sdr["T_EmailID"].ToString());
                        LeadItem.Add(sdr["H_EmailID"].ToString());
                    }
                    else if (contextKey == "3")
                    {
                        LeadItem.Add(sdr["Country"].ToString());
                        //LeadItem.Add(sdr["H_Country"].ToString());
                    }
                    else if (contextKey == "4")
                    {
                        LeadItem.Add(sdr["T_Phone"].ToString());
                        LeadItem.Add(sdr["H_Phone"].ToString());
                    }
                    else if (contextKey == "5")
                    {
                        LeadItem.Add(sdr["UrlName"].ToString());
                    }
                    else if (contextKey == "6")
                    {
                        LeadItem.Add(sdr["Status"].ToString());
                    }
                    else if (contextKey == "7")
                    {
                        LeadItem.Add(sdr["Employee"].ToString());
                    }
                }
                return LeadItem;
            }
            catch
            {
                return new List<string>();
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Count Leads
        public DataTable CountLead(int empID)
        {
            cmd = new SqlCommand("Sp_CountLead", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region GetReminderDetails From Reminder Table
        public DataTable GetReminders(int empID)
        {
            cmd = new SqlCommand("SpGetReminders", con);
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Get LeadUrl and Status
        public Leads GetReminder_Status(long reminderID, long leadID)
        {
            try
            {
                cmd = new SqlCommand("Select l.LeadID,l.StatusID,l.StatusLogID,r.reminderID,r.ReminderDate,r.ReminderTime,r.ReminderMessage,su.UrlType from Leads l LEFT JOIN SiteUrl su ON l.UrlID=su.UrlID LEFT JOIN Reminder r On l.LeadID=r.LeadID where r.ReminderID=@reminderID", con);
                //cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = leadID;
                cmd.Parameters.Add("@reminderID", SqlDbType.BigInt).Value = reminderID;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lead.LeadID = (long)(dr["LeadID"]);
                    lead.StatusLogID = (long)(dr["StatusLogID"]);
                    lead.ReminderID = (long)(dr["ReminderID"]);
                    lead.StatusID = (int)(dr["StatusID"]);
                    lead.UrlType = (string)(dr["UrlType"]);
                    lead.ReminderDate = (DateTime)(dr["ReminderDate"]);
                    lead.ReminderTime = (string)(dr["ReminderTime"]);
                    lead.ReminderMessage = (string)(dr["ReminderMessage"]);
                    //sl.ChangeStatusID = (dr["ChangeStatusID"] == System.DBNull.Value) ? 0 : (int)dr["ChangeStatusID"];
                    //sl.ChangeStatus = (dr["ChangeStatus"] == System.DBNull.Value)?"":(string)dr["ChangeStatus"];
                    //sl.Remark = dr["Remark"] == System.DBNull.Value ? "" : dr["Remark"].ToString();
                    //sl.FullName = dr["FullName"] == System.DBNull.Value ? "" : dr["FullName"].ToString();
                    //sl.RemarkDate = dr["RemarkDate"] == System.DBNull.Value ? Convert.ToDateTime("1/1/1753 12:00:00 AM") : Convert.ToDateTime(dr["RemarkDate"]);
                    //sl.ReminderDate = dr["ReminderDate"] == System.DBNull.Value ? Convert.ToDateTime("1/1/1753 12:00:00 AM") : Convert.ToDateTime(dr["ReminderDate"]);
                    //sl.ReminderTime = dr["ReminderTime"] == System.DBNull.Value ? "" : dr["ReminderTime"].ToString();
                    //sl.ReminderMessage = dr["ReminderMessage"] == System.DBNull.Value ? "" : dr["ReminderMessage"].ToString();

                }
                return lead;
            }
            catch
            {
                return new Leads();
            }
            finally
            {
                con.Close();
            }

        }
        #endregion

        public bool UpdateStatusLogForReminder(StatusLog sl)
        {
            try
            {
                cmd = new SqlCommand("UPDATE StatusLog SET LeadID=@leadID,StatusID=@statusID,ChangeStatusID=@changeStatusID,EmpID=@empID,ReminderDate=@reminderDate,ReminderTime=@reminderTime,ReminderMessage=@reminderMessage Where LeadID=@leadID", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = sl.LeadID;
                cmd.Parameters.Add("@statusID", SqlDbType.Int).Value = sl.StatusID;
                cmd.Parameters.Add("@changeStatusID", SqlDbType.Int).Value = sl.ChangeStatusID;
                //cmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = sl.Remark;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = sl.EmpID;
                cmd.Parameters.Add("@reminderDate", SqlDbType.DateTime).Value = sl.ReminderDate;
                cmd.Parameters.Add("@reminderTime", SqlDbType.NVarChar).Value = sl.ReminderTime;
                cmd.Parameters.Add("@reminderMessage", SqlDbType.NVarChar).Value = sl.ReminderMessage;
                //SqlParameter returnValue = new SqlParameter();
                //returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                //returnValue.Direction = ParameterDirection.Output;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                //int count = Convert.ToInt32(returnValue.Value);

                if (count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }


        public void InsertReminder(Reminder rm, int empID)
        {
            try
            {
                cmd = new SqlCommand("SpInsertReminder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = rm.LeadID;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                cmd.Parameters.Add("@reminderID", SqlDbType.BigInt).Value = rm.ReminderID;
                cmd.Parameters.Add("@reminderDate", SqlDbType.DateTime).Value = rm.ReminderDate;
                cmd.Parameters.Add("@reminderTime", SqlDbType.NVarChar).Value = rm.ReminderTime;
                cmd.Parameters.Add("@reminderMessage", SqlDbType.NVarChar).Value = rm.ReminderMessage;
                con.Open();
                cmd.ExecuteNonQuery();

            }
            //catch
            //{ 

            //}
            finally
            {
                con.Close();
            }
        }

        public bool DeleteReminder(long reminderID, long leadID)
        {
            try
            {
                cmd = new SqlCommand("SpDeleteReminder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@reminderID", SqlDbType.BigInt).Value = reminderID;
                cmd.Parameters.Add("@leadID", SqlDbType.BigInt).Value = leadID;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        #region Get Employee Details For Edit
        public EmpRegistration GetEmpDetails(int empID)
        {
            try
            {
                cmd = new SqlCommand("SpGetEmpDtlsByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    empRegister.ProfileName = dr["ProfileName"].ToString();
                    empRegister.ProfileID = (int)dr["ProfileID"];
                    empRegister.HeadID = (int)dr["HeadID"];
                    empRegister.EmpID = (int)dr["EmpID"];
                    empRegister.FirstName = dr["FirstName"].ToString();
                    empRegister.LastName = dr["LastName"].ToString();
                    empRegister.EmailID = dr["EmailID"].ToString();
                    empRegister.Phone = dr["PhoneNo"].ToString();
                }
                return empRegister;
            }
            catch
            {
                return new EmpRegistration();
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        public DataTable GetMemberByEmpID(int empID)
        {
            cmd = new SqlCommand("SpGetMemberByEmpID", con);
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataSet GetMembers(int empID)
        {
            cmd = new SqlCommand("SpGetMembers", con);
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }

        public DataTable GetProfileByEmpID(int empID)
        {
            cmd = new SqlCommand("SpGetPfleName", con);
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        public EmpRegistration GetProfile(int empID)
        {
            try
            {
                cmd = new SqlCommand("SpGetPfleName", con);
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    empRegister.ProfileID = (int)dr["ProfileID"];
                }
                return empRegister;
            }
            catch
            {
                return new EmpRegistration();
            }
            finally
            {
                con.Close();
            }
        }
        #region Update Employee
        public bool UpdateEmployee(EmpRegistration empRegister)
        {
            try
            {
                cmd = new SqlCommand("SpUpdateEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empRegister.EmpID;
                cmd.Parameters.Add("@profileID", SqlDbType.Int).Value = empRegister.ProfileID;
                cmd.Parameters.Add("@headID", SqlDbType.Int).Value = empRegister.HeadID;
                cmd.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = empRegister.FirstName;
                cmd.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = empRegister.LastName;
                cmd.Parameters.Add("@emailID", SqlDbType.NVarChar).Value = empRegister.EmailID;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = empRegister.Password;
                cmd.Parameters.Add("@phoneNo", SqlDbType.NVarChar).Value = empRegister.Phone;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Char, 1);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                char count = Convert.ToChar(returnValue.Value);
                if (count == 'U')
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region GetDomain
        public DataSet GetDomain(int strtype, string urltype)
        {
            cmd = new SqlCommand("SpGetDomain", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@strStype", SqlDbType.Int).Value = strtype;
            cmd.Parameters.Add("@urltype", SqlDbType.NVarChar).Value = urltype;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region GetCountry
        public DataSet GetCountry()
        {
            cmd = new SqlCommand("SpGetCountry", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region GetStatus
        public DataSet GetStatusName()
        {
            cmd = new SqlCommand("SpGetStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region GetSource
        public DataSet GetSourceName()
        {
            cmd = new SqlCommand("SpGetSource", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            sda.Fill(dt);
            return dt;
        }
        #endregion


        #region Get Advanced Search Leads
        public DataSet GetSearchLeads(Leads lead, string sessionVal, int employee, int empID, string page, string dateFrom, string dateTo, int rowsPerPage, int pageNum)
        {
            cmd = new SqlCommand("SpSearchLeads", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@dateFrom", SqlDbType.NVarChar).Value = dateFrom;
            cmd.Parameters.Add("@dateTo", SqlDbType.NVarChar).Value = dateTo;
            cmd.Parameters.Add("@t_Name", SqlDbType.NVarChar).Value = lead.T_Name;
            cmd.Parameters.Add("@t_EmailID", SqlDbType.NVarChar).Value = lead.T_EmailID;
            cmd.Parameters.Add("@t_Country", SqlDbType.NVarChar).Value = lead.T_Country;
            cmd.Parameters.Add("@urlType", SqlDbType.NVarChar).Value = lead.UrlType;
            cmd.Parameters.Add("@urlName", SqlDbType.Int).Value = Convert.ToInt32(lead.UrlName);
            cmd.Parameters.Add("@status", SqlDbType.Int).Value = Convert.ToInt32(lead.Status);
            cmd.Parameters.Add("@source", SqlDbType.Int).Value = Convert.ToInt32(lead.SourceType);
            cmd.Parameters.Add("@employee", SqlDbType.Int).Value = employee;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            cmd.Parameters.Add("@page", SqlDbType.NVarChar).Value = page;
            cmd.Parameters.Add("@SessionVal", SqlDbType.NVarChar).Value = sessionVal;
            cmd.Parameters.Add("@rowsPerPage", SqlDbType.Int).Value = rowsPerPage;
            cmd.Parameters.Add("@pageNum", SqlDbType.Int).Value = pageNum;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
        #endregion

        public bool DeleteEmployee(int empid)
        {
            try
            {
                cmd = new SqlCommand("SpDeleteEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empid;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public string GetAdminEmailID()
        {
            try
            {
                string emailID = "";
                cmd = new SqlCommand("SpGetAdminEmailID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    emailID = dr["EmailID"].ToString();
                }
                return emailID;
            }
            finally
            {
                con.Close();
            }
        }

        public string GetEmpPwd(string emailID)
        {
            try
            {
                string pwd = "";
                cmd = new SqlCommand("SpGetEmpPwd", con);
                cmd.Parameters.Add("@emailID", SqlDbType.NVarChar).Value = emailID;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    pwd = dr["Password"].ToString();
                }
                return pwd;
            }
            finally
            {
                con.Close();
            }
        }

        #region Employee Update Password
        public bool UpdatePassword(string pwd, int empID)
        {
            try
            {
                cmd = new SqlCommand("update Employee set Password=@pwd where EmpID=@empID", con);
                cmd.Parameters.Add("@pwd", SqlDbType.NVarChar).Value = pwd;
                cmd.Parameters.Add("@empID", SqlDbType.BigInt).Value = empID;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        //Added By Ashok Kumar on 24-11-200 for cheking valid time and date for comprision
        #region function for checking the valid time and date
        public DateTime validdate(DateTime start, DateTime end)
        {
            string flag = "";
            DateTime dtend = Convert.ToDateTime("1/1/1753 12:00:00 AM");
            DateTime currentDT = Convert.ToDateTime(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            if (start < end)
            {
                dtend = end;
            }
            else
            {
                //checking leap year
                if ((end.Year % 4 == 0) || (end.Year % 100 == 0) || (end.Year % 400 == 0))
                {
                    flag = "l";
                }
                if (end.Month == 2 && flag == "l")
                {
                    if (end.Day == 29)
                    {
                        dtend = end.AddDays(1);
                    }
                    else
                    {
                        dtend = end.AddDays(1);
                    }

                }
                else if (end.Month == 2 && flag != "l")
                {
                    if (end.Day == 28)
                    {
                        dtend = end.AddDays(1);
                    }
                    else
                    {
                        dtend = end.AddDays(1);
                    }
                }

                if (end.Month == 4 || end.Month == 6 || end.Month == 9 || end.Month == 11)
                {
                    if (end.Day == 30)
                    {
                        dtend = end.AddDays(1);
                    }
                    else
                    {
                        dtend = end.AddDays(1);
                    }
                }
                if (end.Month == 1 || end.Month == 3 || end.Month == 5 || end.Month == 7 || end.Month == 8 || end.Month == 10 || end.Month == 12)
                {
                    if (end.Month == 12)
                    {
                        if (end.Day == 31)
                        {
                            dtend = end.AddDays(1);
                        }
                        else
                        {
                            dtend = end.AddDays(1);
                        }
                    }
                    else if (end.Day == 31)
                    {
                        dtend = end.AddDays(1);
                    }
                    else
                    {
                        dtend = end.AddDays(1);
                    }
                }
            }
            return dtend;
        }
        #endregion

        #region for getting the assign lead for page of getsholidays.com
        public int Getleadid(string start, string end)
        {
            try
            {
                int leadid = 0;
                cmd = new SqlCommand("SpGetleadIdforholdys", con);
                cmd.Parameters.Add("@startt", SqlDbType.NVarChar).Value = start;
                cmd.Parameters.Add("@endt", SqlDbType.NVarChar).Value = end;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    leadid = (int)dr["AssignerId"];
                }
                return leadid;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Function for Reminder
        public DataTable reminder(string start, int empids)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("GETREMDETLS", con);
            da.SelectCommand.Parameters.AddWithValue("@EmpId", empids);
            da.SelectCommand.Parameters.AddWithValue("@Starttm", start);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.Fill(dt);
            return dt;
        }
        #endregion

        public void disable(int i)
        {
            DateTime cudttime = DateTime.Now.AddMinutes(30);
            string updttime = cudttime.ToString("H:mm");

            try
            {
                cmd = new SqlCommand("Update reminder set ReminderTime='" + updttime + "',Tag='F' where Reminderid='" + i + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch 
            {

            }
            finally
            {
                con.Close();
            }

        }

        #region Function for getting value if leadstatus based on leadid after redirection from message popup
        public DataTable getleadsdata(int leadid)
        {
            cmd = new SqlCommand("Getleadsdtls", con);
            cmd.Parameters.Add("@leadid", SqlDbType.Int).Value = leadid;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Insert EmpId In Display Table For Hide Query Bank From User Account
        public bool InsertEmpInDisplay(int empid)
        {
            try
            {
                cmd = new SqlCommand("SpInsertEmpInDisplay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empid", SqlDbType.Int).Value = empid;
                SqlParameter returnValue = new SqlParameter();
                returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(returnValue.Value);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Get EmpId From Display Table For Hide/Show Query Bank From User Account
        public List<int> GetEmpIDFromDisplay()
        {
            List<int> employee = new List<int>();
            int empid = 0;
            try
            {
                cmd = new SqlCommand("SpGetEmpIDFromDisplay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    empid = Convert.ToInt32(dr["EmpID"]);
                    employee.Add(empid);
                }
                return employee;
            }
            catch
            {
                return new List<int>();
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Get Hide QueryBank User List
        public DataTable GetHideQBankUserList()
        {
            cmd = new SqlCommand("SpGetHideQBankUserList", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Delete Hide QueryBank User List
        public bool DeleteHideQBankUserList(int empid)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE  FROM DISPLAY WHERE EMPID=@empid", con);
                cmd.Parameters.Add("@empid", SqlDbType.Int).Value = empid;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region GetLogDetails
        public DataTable GetLogDetails()
        {
            cmd = new SqlCommand("SpGetLogDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            sda.Fill(table);
            return table;
        }
        #endregion

        #region Get Lead Type
        public Leads GetLeadType(long leadid)
        {
            try
            {
                cmd = new SqlCommand("select st.UrlType,ld.ReminderID,ld.StatusID from leads ld left outer join siteurl st on ld.urlid=st.urlid where ld.LeadID=@leadid", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@leadid", SqlDbType.BigInt).Value = leadid;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lead.UrlType = (string)dr["UrlType"];
                    lead.ReminderID = (dr["ReminderID"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["ReminderID"]);
                    lead.StatusID = (dr["StatusID"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["StatusID"]);
                }
                return lead;
            }
            catch
            {
                return lead;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        public void ChangeLdFlds(ArrayList ldfieldlist, int empID, string Flag)
        {
            int PRIORITY = 1;
            foreach (string LdFlds in ldfieldlist)
            {
                cmd = new SqlCommand("ChangeLdFlds", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LdFlds", SqlDbType.NVarChar).Value = LdFlds;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                cmd.Parameters.Add("@Flag", SqlDbType.Char).Value = Flag;
                cmd.Parameters.Add("@PRIORITY", SqlDbType.Int).Value = PRIORITY;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                PRIORITY++;
            }
        }

        public DataTable GetListboxItem(int empID)
        {
            cmd = new SqlCommand("GetLeadFields", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            sda.Fill(table);
            return table;
        }
     
        #region Insert Country To Assign
        public bool InstCntryToAsn(string country, int empID)
        {
            cmd = new SqlCommand("INSERT INTO ASNLDBYCNTRY (EMPID,COUNTRY) VALUES (@empID,@country)", con);
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            cmd.Parameters.Add("@country", SqlDbType.NVarChar).Value = country;
            con.Open();
            int count = Convert.ToInt32(cmd.ExecuteNonQuery());
            con.Close();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region Get AsnLdByCountry Details For Bind in Table
        public DataTable GetCntryToAsn()
        {
            cmd = new SqlCommand("SELECT ASN.*,EMP.FIRSTNAME+' '+EMP.LASTNAME+' ('+PRF.PROFILENAME+')' AS EMPLOYEE FROM ASNLDBYCNTRY ASN " +
                                 "LEFT OUTER JOIN EMPLOYEE EMP ON ASN.EMPID=EMP.EMPID LEFT OUTER JOIN PROFILE PRF ON EMP.PROFILEID= " +
                                 "PRF.PROFILEID", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Get CountryList To Update
        public DataTable GetCountryList(int EmpID)
        {
            cmd = new SqlCommand("SELECT ALC.COUNTRY,ALC.EmpID,EMP.FIRSTNAME+' '+EMP.LASTNAME+' ('+PRF.PROFILENAME+')' AS EMPLOYEE FROM ASNLDBYCNTRY ALC LEFT OUTER JOIN EMPLOYEE EMP ON ALC.EMPID=EMP.EMPID LEFT OUTER JOIN PROFILE PRF ON EMP.PROFILEID=PRF.PROFILEID WHERE ALC.EMPID=@EmpID", con);
            cmd.Parameters.Add("@EmpID", SqlDbType.Int).Value = EmpID;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Check Employee in AsnLdByCountry
        public bool CheckEmpToAsn(int empID)
        {
            try
            {
                cmd = new SqlCommand("SELECT * FROM ASNLDBYCNTRY WHERE EMPID=@empID", con);
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                con.Open();
                int COUNT = Convert.ToInt32(cmd.ExecuteScalar());
                if (COUNT > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Delete Employee From AsnLdByCountry
        public bool DeleteAsnByCountry(int asnLdByCID)
        {
            try
            {
                cmd = new SqlCommand("DELETE FROM ASNLDBYCNTRY WHERE EMPID=@asnLdByCID", con);
                cmd.Parameters.Add("@asnLdByCID", SqlDbType.Int).Value = asnLdByCID;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion
        
        public bool UpdtCntryToAsn(string strval, int empID)
        {
            cmd = new SqlCommand("UPDATE_ASN_LD_BY_CNTRY", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            cmd.Parameters.Add("@strval", SqlDbType.NVarChar).Value = strval;
            con.Open();
            int count = Convert.ToInt32(cmd.ExecuteNonQuery());
            con.Close();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region COUNT SOURCE REPORT WITH URL
        public DataSet ChangeSrcReport(string DateFrom, string DateTo)
        {
            cmd = new SqlCommand("SP_COUNT_SOURCE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DateFrom", SqlDbType.NVarChar).Value = DateFrom;
            cmd.Parameters.Add("@DateTo", SqlDbType.NVarChar).Value = DateTo;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
        #endregion

        #region Store GetQueryBankEmpID
        public int GetQueryBankEmp(int empID)
        {
            try
            {
                cmd = new SqlCommand("Select EmpID from Display where EmpID=@empID", con);
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    empID = (int)dr["EmpID"];
                }
                return empID;
            }
            catch
            {
                return 0;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        public List<Employee> GetEmpPersonalInfo(int empID, int profileID)
        {
            try
            {
                List<Employee> listempPi = new List<Employee>();
                cmd = new SqlCommand("SP_GetPersonalInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                cmd.Parameters.Add("@profileID", SqlDbType.Int).Value = profileID;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Employee empPi = new Employee();
                        empPi.FirstName = dr["FirstName"].ToString();
                        empPi.LastName = dr["LastName"].ToString();
                        empPi.EmailID = dr["EmailID"].ToString();
                        empPi.Phone = dr["PhoneNo"].ToString();
                        empPi.Profile = dr["Profilename"].ToString();
                        if (profileID != 1 && profileID!=2)
                        {
                            empPi.Head = dr["Head"].ToString();
                        }
                        listempPi.Add(empPi);
                    }
                }
                return listempPi;
            }
            catch
            {
                return new List<Employee>();
            }
            finally
            {
                con.Close();
            }
        }

        #region Get Department
        public DataTable GetDepartment()
        {
            try
            {
                cmd = new SqlCommand("SELECT * FROM DEPARTMENT", con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch
            {
                return new DataTable();
            }
        }
        #endregion

        public string GetIPCountry(long IP)
        {
            string Country = "";
            try
            {
                cmd = new SqlCommand("SP_GetIPCountry",con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IP", SqlDbType.BigInt).Value = IP;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Country = dr["CountryName"].ToString();
                }
            }
            catch(SqlException ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }
             return Country;
        }

        //////////////////////////////////////////////////////////////////////////
        #region All Operation Employee List
        public DataTable GeOpeEmpToAsd()
        {
            cmd = new SqlCommand("Sp_GetOpeToAsd", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Operation Employee List By EmpID
        public DataTable GetFileAsdToEmp(int EmpID)
        {
            cmd = new SqlCommand("Sp_FileAsdToEmp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@EmpID", SqlDbType.Int).Value = EmpID;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        #endregion

        #region Get File Details
        public FileDtls GetFileDetails(long LeadID, long FileID)
        {
            FileDtls fileInfo = new FileDtls();
            try
            {
                cmd = new SqlCommand("SP_GetFileDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = FileID;
                cmd.Parameters.Add("@LeadID", SqlDbType.BigInt).Value = LeadID;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        fileInfo.FileID = dr["FileID"] == System.DBNull.Value ? 0 : Convert.ToInt64(dr["FileID"]);
                        fileInfo.IpAddress = (dr["IpAddress"] == System.DBNull.Value ? "" : (string)dr["IpAddress"]);
                        fileInfo.EnquirePage = (dr["EnquirePage"] == System.DBNull.Value ? "" : (string)dr["EnquirePage"]);
                        fileInfo.ReferralUrl = (dr["ReferralUrl"] == System.DBNull.Value ? "" : (string)dr["ReferralUrl"]);
                        fileInfo.EnquireBy = (dr["EnquireEmp"] == System.DBNull.Value ? "" : (string)dr["EnquireEmp"]);
                        fileInfo.DomainName = (dr["UrlName"] == System.DBNull.Value ? "" : (string)dr["UrlName"]);
                        fileInfo.SourceType = (dr["SourceType"] == System.DBNull.Value ? "" : (string)dr["SourceType"]);
                        fileInfo.SessionValue = (dr["SessionValue"] == System.DBNull.Value ? "" : (string)dr["SessionValue"]);
                        fileInfo.StatusName = (dr["Status"] == System.DBNull.Value ? "" : (string)dr["Status"]);
                        fileInfo.LeadDate = (dr["QueryDate"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["QueryDate"]);
                        fileInfo.Name = (dr["Name"] == System.DBNull.Value ? "" : (string)dr["Name"]);
                        fileInfo.EmailID = (dr["EmailID"] == System.DBNull.Value ? "" : (string)dr["EmailID"]);
                        fileInfo.Phone = (dr["Phone"] == System.DBNull.Value ? "0" : (string)dr["Phone"]);
                        fileInfo.Country = (dr["Country"] == System.DBNull.Value ? "0" : (string)dr["Country"]);
                        fileInfo.Requirements = (dr["Requirements"] == System.DBNull.Value ? "" : (string)dr["Requirements"]);
                        fileInfo.UrlType = (dr["UrlType"] == System.DBNull.Value ? "" : (string)dr["UrlType"]);

                        if (fileInfo.UrlType == "Tour")
                        {
                            fileInfo.TourName = (dr["TourName"] == System.DBNull.Value ? "" : (string)dr["TourName"]);
                            fileInfo.DateOfTravel = (dr["TravelDate"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["TravelDate"]);
                            fileInfo.DurationOfTrip = (dr["Duration"] == System.DBNull.Value ? 0 : (int)dr["Duration"]);
                            fileInfo.NoOfGuest = (dr["NoOfGuest"] == System.DBNull.Value ? 0 : (int)dr["NoOfGuest"]);
                            fileInfo.HotelCateogry = (dr["HotelCateogry"] == System.DBNull.Value ? "0" : (string)dr["HotelCateogry"]);
                        }
                        else if (fileInfo.UrlType == "Hotel")
                        {
                            fileInfo.HotelName = (dr["HOTELNAME"] == System.DBNull.Value ? "" : (string)dr["HOTELNAME"]);
                            fileInfo.CheckInDate = (dr["CHECKINDATE"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["CHECKINDATE"]);
                            fileInfo.CheckOutDate = (dr["CHECKOUTDATE"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["CHECKOUTDATE"]);
                            fileInfo.SingleRoom = (dr["SINGLEROOM"] == System.DBNull.Value ? 0 : (int)dr["SINGLEROOM"]);
                            fileInfo.DoubleRoom = (dr["DOUBLEROOM"] == System.DBNull.Value ? 0 : (int)dr["DOUBLEROOM"]);
                            fileInfo.TripleRoom = (dr["TRIPLEROOM"] == System.DBNull.Value ? 0 : (int)dr["TRIPLEROOM"]);
                        }
                        else if (fileInfo.UrlType == "Wedding")
                        {
                            fileInfo.WeddingType = dr["WeddingType"].ToString();
                            fileInfo.WeddingVanue = dr["WeddingVanue"].ToString();
                            fileInfo.WeddingDate = (DateTime)dr["WeddingDate"];
                            fileInfo.WeddingPlace = dr["WeddingPlace"].ToString();
                            fileInfo.Guest = dr["Guest"].ToString();
                            fileInfo.Budget = (decimal)dr["Budget"];
                            fileInfo.CheckInDate = (DateTime)dr["WCheckInDate"];
                            fileInfo.CheckOutDate = (DateTime)dr["WCheckOutDate"];
                            fileInfo.Address = dr["Address"].ToString();
                        }
                        if (fileInfo.FileID != 0)
                        {
                            fileInfo.ArrivalCity = dr["ArrivalCity"].ToString();
                            fileInfo.DepartureCity = dr["DepartureCity"].ToString();
                            fileInfo.ArrivalDate = dr["ArrivalDate"].ToString();
                            fileInfo.DepartureDate = dr["DepartureDate"].ToString();
                            fileInfo.CurrencyType = dr["CurrencyType"].ToString();
                            fileInfo.TotalAmount = dr["TotalAmount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(dr["TotalAmount"]);
                            fileInfo.DepositRecived = dr["DepositRecived"] == System.DBNull.Value ? "" : dr["DepositRecived"].ToString();
                            fileInfo.DepositAmount = dr["DepositAmount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(dr["DepositAmount"]);
                            fileInfo.DepositCurrencyType = dr["DepositCurrencyType"].ToString();
                            fileInfo.DepositMethod = dr["DepositMethodType"].ToString();
                            fileInfo.Balance = dr["Balance"] == System.DBNull.Value ? "" : dr["Balance"].ToString();
                            fileInfo.BalanceAmount = dr["BalanceAmount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(dr["BalanceAmount"]);
                            fileInfo.BalanceDate = dr["BalanceDate"].ToString();
                            fileInfo.BalanceMethod = dr["BalanceMethodType"].ToString();
                            fileInfo.TransportRequired = dr["TransportRequired"] == System.DBNull.Value ? "" : dr["TransportRequired"].ToString();
                            fileInfo.TrptUser = dr["TrptUser"] == System.DBNull.Value ? "" : dr["TrptUser"].ToString();
                            fileInfo.HotelRequired = dr["HotelRequired"] == System.DBNull.Value ? "" : dr["HotelRequired"].ToString();
                            fileInfo.SpecialServices = dr["SpecialServices"].ToString();
                            fileInfo.ArrivalDetails = dr["ArrivalDetails"].ToString();
                            fileInfo.DepartureDetails = dr["DepartureDetails"].ToString();
                            fileInfo.StatusSent = dr["StatusSent"] == System.DBNull.Value ? "" : dr["StatusSent"].ToString();
                            fileInfo.HotelConfirmed = dr[("HotelConfirmed")] == System.DBNull.Value ? "" : dr["HotelConfirmed"].ToString();
                            fileInfo.GuideConfirmed = dr[("GuideConfirmed")] == System.DBNull.Value ? "" : dr["GuideConfirmed"].ToString();
                            fileInfo.OperatorInfoSend = dr[("OperatorInfoSend")] == System.DBNull.Value ? "" : dr["OperatorInfoSend"].ToString();
                            fileInfo.HotelReconfirmed = dr[("HotelReconfirmed")] == System.DBNull.Value ? "" : dr["HotelReconfirmed"].ToString();
                            fileInfo.GuideReconfirmed = dr[("GuideReconfirmed")] == System.DBNull.Value ? "" : dr["GuideReconfirmed"].ToString();
                            fileInfo.OperatorReconfirmed = dr[("OperatorReconfirmed")] == System.DBNull.Value ? "" : dr["OperatorReconfirmed"].ToString();
                            fileInfo.HotelPaymentSent = dr[("HotelPaymentSent")] == System.DBNull.Value ? "" : dr["HotelPaymentSent"].ToString();
                            fileInfo.KitMade = dr[("KitMade")] == System.DBNull.Value ? "" : dr["KitMade"].ToString();
                            fileInfo.KitSent = dr[("KitSent")] == System.DBNull.Value ? "" : dr["KitSent"].ToString();
                            fileInfo.BalancePayRcvd = dr[("BalanceRecived")] == System.DBNull.Value ? "" : dr["BalanceRecived"].ToString();
                            fileInfo.ReminderDate = (dr["ReminderDate"] == System.DBNull.Value ? Convert.ToDateTime("01/01/1900") : (DateTime)dr["ReminderDate"]);
                            fileInfo.ReminderTime = (dr["ReminderTime"] == System.DBNull.Value ? "00:00" : (string)dr["ReminderTime"]);
                            fileInfo.ReminderMessage = (dr["ReminderMessage"] == System.DBNull.Value ? "" : (string)dr["ReminderMessage"]);
                        }
                    }
                }
            }
            catch
            {
                return new FileDtls();
            }
            finally
            {
                con.Close();
            }
            return fileInfo;
        }
        #endregion

        #region Guide Section
        public bool InsertUpdateGuide(Guide guide)
        {
            try
            {
                cmd = new SqlCommand("Sp_IUGuide", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = guide.FileID;
                cmd.Parameters.Add("@GuideID", SqlDbType.BigInt).Value = guide.GuideID;
                cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = guide.City;
                cmd.Parameters.Add("@Language", SqlDbType.VarChar).Value = guide.Language;
                con.Open();
                int counbt = cmd.ExecuteNonQuery();
                if (counbt > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public DataSet GetGuide(long fileid)
        {
            cmd = new SqlCommand("Sp_GetGuide", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = fileid;
            cmd.Parameters.Add("@PageNum", SqlDbType.Int).Value = 0;
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sdr.Fill(ds);
            return ds;
        }

        public DataSet GuideList(long fileid, int pagenum)
        {
            try
            {
                List<Guide> listGuide = new List<Guide>();
                cmd = new SqlCommand("Sp_GetGuide", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = fileid;
                cmd.Parameters.Add("@PageNum", SqlDbType.Int).Value = pagenum;
                SqlDataAdapter sdr = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sdr.Fill(ds);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public bool GuideDelete(long GuideID)
        {
            try
            {
                cmd = new SqlCommand("Delete from Guide where GuideID=@GuideID", con);
                cmd.Parameters.Add("@GuideID", SqlDbType.BigInt).Value = GuideID;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region TicketInfo Section
        public DataSet GetTicketInfo(long FileID,string Tag)
        {
            cmd = new SqlCommand("Sp_GetTicketInfo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = FileID;
            cmd.Parameters.Add("@PageNum", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@Tag", SqlDbType.Char).Value = Tag;
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sdr.Fill(ds);
            return ds;
        }

        public DataSet GetTicketList(long fileid, int pagenum, string Tag)
        {
            try
            {
                cmd = new SqlCommand("Sp_GetTicketInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = fileid;
                cmd.Parameters.Add("@PageNum", SqlDbType.Int).Value = pagenum;
                cmd.Parameters.Add("@Tag", SqlDbType.Char).Value = Tag;
                SqlDataAdapter sdr = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sdr.Fill(ds);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public bool InsertUpdateTicket(Ticket ticket)
        {
            try
            {
                cmd = new SqlCommand("Sp_IUTicket", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = ticket.FileID;
                cmd.Parameters.Add("@Tag", SqlDbType.Char).Value = ticket.Tag;
                cmd.Parameters.Add("@TicketID", SqlDbType.BigInt).Value = ticket.TicketID;
                cmd.Parameters.Add("@TravelDate", SqlDbType.DateTime).Value = ticket.TravelDate;
                cmd.Parameters.Add("@Sector", SqlDbType.VarChar).Value = ticket.Sector;
                cmd.Parameters.Add("@FTBNameNo", SqlDbType.VarChar).Value = ticket.FTBNameNo;
                cmd.Parameters.Add("@Done", SqlDbType.VarChar).Value = ticket.Done;
                con.Open();
                int counbt = cmd.ExecuteNonQuery();
                if (counbt > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool TicketDelete(long TicketID)
        {
            try
            {
                cmd = new SqlCommand("Delete from TicketInfo where TicketInfoID=@TicketID", con);
                cmd.Parameters.Add("@TicketID", SqlDbType.BigInt).Value = TicketID;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Transport Section

        public bool InsertUpdateTrpt(Transport trpt)
        {
            try
            {
                cmd = new SqlCommand("Sp_IUTrpt", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = trpt.FileID;
                cmd.Parameters.Add("@TrptID", SqlDbType.BigInt).Value = trpt.TrptID;
                cmd.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = trpt.Vendor;
                cmd.Parameters.Add("@TrptAmount", SqlDbType.Decimal).Value = trpt.TrptAmount;
                cmd.Parameters.Add("@TrptType", SqlDbType.VarChar).Value = trpt.TrptType;
                cmd.Parameters.Add("@DriverName", SqlDbType.VarChar).Value = trpt.DriverName;
                cmd.Parameters.Add("@DriverPhoneNo", SqlDbType.VarChar).Value = trpt.DriverPhoneNo;
                cmd.Parameters.Add("@TrptDone", SqlDbType.VarChar).Value = trpt.TrptDone;
                con.Open();
                int counbt = cmd.ExecuteNonQuery();
                if (counbt > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public DataSet GetTrptInfo(long FileID)
        {
            cmd = new SqlCommand("Sp_GetTrptInfo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = FileID;
            cmd.Parameters.Add("@PageNum", SqlDbType.Int).Value = 0;
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sdr.Fill(ds);
            return ds;
        }

        public DataSet GetTrptList(long fileid, int pagenum)
        {
            try
            {
                cmd = new SqlCommand("Sp_GetTrptInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = fileid;
                cmd.Parameters.Add("@PageNum", SqlDbType.Int).Value = pagenum;
                SqlDataAdapter sdr = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sdr.Fill(ds);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public bool TrptDelete(long TrptID)
        {
            try
            {
                cmd = new SqlCommand("Delete from TransportInfo where TrptID=@TrptID", con);
                cmd.Parameters.Add("@TrptID", SqlDbType.BigInt).Value = TrptID;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Get File Column
        public DataTable GetClosedListboxItem(int empID)
        {
            cmd = new SqlCommand("GetFileFields", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            sda.Fill(table);
            return table;
        }
        #endregion

        #region Insert or Update File Column
        public void ChangeClosedFields(ArrayList ldfieldlist, int empID, string Flag)
        {
            int PRIORITY = 1;
            foreach (string ClosedFlds in ldfieldlist)
            {
                cmd = new SqlCommand("SP_ChangeFileFields", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CloseFlds", SqlDbType.NVarChar).Value = ClosedFlds;
                cmd.Parameters.Add("@empID", SqlDbType.Int).Value = empID;
                cmd.Parameters.Add("@Flag", SqlDbType.Char).Value = Flag;
                cmd.Parameters.Add("@Priority", SqlDbType.Int).Value = PRIORITY;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                PRIORITY++;
            }
        }
        #endregion


        public DataTable GetAllTrptUser()
        {
            try
            {
                cmd = new SqlCommand("Sp_GetAllTrptUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch(SqlException ex)
            {
                throw ex;
            }

        }

        #region insert/Update Doc
        public bool IUDoc(int Tag,string DocName, string DocType, byte[] DocData,long FileID)
        {
            try
            {
                cmd = new SqlCommand("SP_IUDoc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Tag", SqlDbType.Int).Value = Tag;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = FileID;
                cmd.Parameters.Add("@DocName", SqlDbType.VarChar).Value = DocName;
                cmd.Parameters.Add("@DocType", SqlDbType.VarChar).Value = DocType;
                cmd.Parameters.Add("@DocData", SqlDbType.VarBinary).Value = DocData;
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Get Doc for open
        public List<FileDoc> GetDoc(long FileID)
        {
            try
            {
                List<FileDoc> listDoc = new List<FileDoc>();
                cmd = new SqlCommand("SELECT FileID,ItineraryName,ItineraryType,ItineraryData,InclusionName,InclusionType,InclusionData,CostSheetName,CostSheetType,CostShettData FROM Doc WHERE FileID=@FileID", con);
               // cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = FileID;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    FileDoc doc = new FileDoc();
                    doc.FileID = (long)dr["FileID"];
                    doc.ITDocName = (dr["ItineraryName"] == System.DBNull.Value) ? string.Empty : (string)dr["ItineraryName"];
                    doc.ITDocType = (dr["ItineraryType"] == System.DBNull.Value) ? string.Empty : (string)dr["ItineraryType"];
                    doc.ITDocContent = (dr["ItineraryData"] == System.DBNull.Value) ?System.Text.Encoding.UTF8.GetBytes(String.Empty) : (Byte[])dr["ItineraryData"];
                    doc.INDocName = (dr["InclusionName"] == System.DBNull.Value) ? string.Empty : (string)dr["InclusionName"];
                    doc.INDocType = (dr["InclusionType"] == System.DBNull.Value) ? string.Empty : (string)dr["InclusionType"];
                    doc.INDocContent = (dr["InclusionData"] == System.DBNull.Value) ? System.Text.Encoding.UTF8.GetBytes(String.Empty) : (Byte[])dr["InclusionData"]; 
                    doc.CSDocName = (dr["CostSheetName"] == System.DBNull.Value) ? string.Empty : (string)dr["CostSheetName"];
                    doc.CSDocType = (dr["CostSheetType"] == System.DBNull.Value) ? string.Empty : (string)dr["CostSheetType"];
                    doc.CSDocContent = (dr["CostShettData"] == System.DBNull.Value) ? System.Text.Encoding.UTF8.GetBytes(String.Empty) : (Byte[])dr["CostShettData"];
                    listDoc.Add(doc);
                }
                return listDoc;
            }
            catch
            {
                return new List<FileDoc>();
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Get Doc for open
        public FileDoc GetDocByTag(long FileID,int Tag)
        {
            try
            {
                FileDoc doc = new FileDoc();
                cmd = new SqlCommand("SP_GetDoc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = FileID;
                cmd.Parameters.Add("@Tag", SqlDbType.Int).Value = Tag;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    doc.FileID = (long)dr["FileID"];
                    if (Tag == 1)
                    {
                        doc.ITDocName = (dr["ItineraryName"] == System.DBNull.Value) ? string.Empty : (string)dr["ItineraryName"];
                        doc.ITDocType = (dr["ItineraryType"] == System.DBNull.Value) ? string.Empty : (string)dr["ItineraryType"];
                        doc.ITDocContent = (dr["ItineraryData"] == System.DBNull.Value) ? System.Text.Encoding.UTF8.GetBytes(String.Empty) : (Byte[])dr["ItineraryData"];
                    }
                    else if (Tag == 2)
                    {
                        doc.INDocName = (dr["InclusionName"] == System.DBNull.Value) ? string.Empty : (string)dr["InclusionName"];
                        doc.INDocType = (dr["InclusionType"] == System.DBNull.Value) ? string.Empty : (string)dr["InclusionType"];
                        doc.INDocContent = (dr["InclusionData"] == System.DBNull.Value) ? System.Text.Encoding.UTF8.GetBytes(String.Empty) : (Byte[])dr["InclusionData"];
                    }
                    else if (Tag == 3)
                    {
                        doc.CSDocName = (dr["CostSheetName"] == System.DBNull.Value) ? string.Empty : (string)dr["CostSheetName"];
                        doc.CSDocType = (dr["CostSheetType"] == System.DBNull.Value) ? string.Empty : (string)dr["CostSheetType"];
                        doc.CSDocContent = (dr["CostShettData"] == System.DBNull.Value) ? System.Text.Encoding.UTF8.GetBytes(String.Empty) : (Byte[])dr["CostShettData"];
                    }
                }
                return doc;
            }
            catch
            {
                return new FileDoc();
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        //public DataTable GetStatusRpt(ArrayList items, DateTime DateFrom, DateTime DateTo)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        foreach (var item in items)
        //        {
        //            cmd = new SqlCommand("SP_GetStatusRpt", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("EmpID", SqlDbType.VarChar).Value = item;
        //            cmd.Parameters.Add("DateFrom", SqlDbType.DateTime).Value = DateFrom;
        //            cmd.Parameters.Add("DateTo", SqlDbType.DateTime).Value = DateTo;
        //            SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //            sda.Fill(dt);
        //        }
        //        return dt;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}


    }
    ////////////////////////////////////////////////////////////////////////////

}
