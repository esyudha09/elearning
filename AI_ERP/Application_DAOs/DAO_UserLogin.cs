using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;

namespace AI_ERP.Application_DAOs
{
    public static class DAO_UserLogin
    {
        public const string SP_INSERT = "USR_INSERT";
        public const string SP_UPDATE = "USR_UPDATE";
        public const string SP_DELETE = "USR_DELETE";
        public const string SP_SELECT_ALL = "USR_SELECT_ALL";
        public const string SP_SELECT_LOGIN = "USR_SELECT_LOGIN";
        public const string SP_SELECT_LOGIN_ALL_USER = "ALLUSER_SELECT_LOGIN";
        public const string SP_SELECT_LOGIN_NON_PWD = "USR_SELECT_LOGIN_NON_PWD";
        public const string SP_SELECT_DET_LOGIN = "USR_DET_SELECT_LOGIN";
        public const string SP_SELECT_BY_NIP = "USR_SELECT_BY_NIP";

        public static bool Insert(UserLogin m)
        {
            bool hasil = false;
            SqlConnection conn = Application_Libs.Libs.GetConnection_Person();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;

                comm.CommandText = SP_INSERT;
                comm.Parameters.Clear();
                comm.Parameters.Add(new SqlParameter("@UserID", m.UserID));
                comm.Parameters.Add(new SqlParameter("@Password", m.Password));
                comm.Parameters.Add(new SqlParameter("@NoInduk", m.NoInduk));
                comm.ExecuteNonQuery();

                transaction.Commit();
                hasil = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return hasil;
        }

        public static bool Update(UserLogin m)
        {
            bool hasil = false;
            SqlConnection conn = Application_Libs.Libs.GetConnection_Person();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;

                comm.CommandText = SP_UPDATE;
                comm.Parameters.Clear();
                comm.Parameters.Add(new SqlParameter("@UserID", m.UserID));
                comm.Parameters.Add(new SqlParameter("@Pwd", m.Password));
                comm.Parameters.Add(new SqlParameter("@NIP", m.NoInduk));
                comm.ExecuteNonQuery();

                transaction.Commit();
                hasil = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return hasil;
        }

        public static bool Delete(string userid)
        {
            bool hasil = false;
            SqlConnection conn = Application_Libs.Libs.GetConnection_Person();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;

                comm.CommandText = SP_DELETE;
                comm.Parameters.Clear();
                comm.Parameters.Add(new SqlParameter("@UserID", userid));
                comm.ExecuteNonQuery();

                transaction.Commit();
                hasil = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return hasil;
        }

        public static DataTable SelectAll()
        {
            DataTable hasil = null;

            SqlConnection conn = Application_Libs.Libs.GetConnection_Person();
            SqlCommand comm = conn.CreateCommand();
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;

                comm.CommandText = SP_SELECT_ALL;
                comm.Parameters.Clear();
                comm.ExecuteNonQuery();

                SqlDataAdapter sda = new SqlDataAdapter(comm);
                sda.Fill(hasil);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        public static UserLogin SelectLogin(string userid, string password)
        {
            UserLogin hasil = null;

            SqlConnection conn = Application_Libs.Libs.GetConnection_Person();
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand(SP_SELECT_LOGIN, conn);
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@password", Application_Libs.Libs.Encryptdata(password));

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    hasil = new UserLogin();
                    hasil.UserID = sdr["UserID"].ToString();
                    hasil.Password = sdr["Pwd"].ToString();
                    hasil.NoInduk = sdr["NIP"].ToString();
                    hasil.JenisUser = JenisUser.Karyawan;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        //public static UserLogin SelectLoginAllUsers(string userid, string password)
        //{
        //    UserLogin hasil = null;

        //    SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand(SP_SELECT_LOGIN_ALL_USER, conn);
        //        conn.Open();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Clear();
        //        cmd.Parameters.AddWithValue("@UserID", userid);
        //        cmd.Parameters.AddWithValue("@Password", Application_Libs.Libs.Encryptdata(password));

        //        SqlDataReader sdr = cmd.ExecuteReader();
        //        while (sdr.Read())
        //        {
        //            hasil = new UserLogin();
        //            hasil.UserID = sdr["UserID"].ToString();
        //            hasil.Password = "";
        //            hasil.NoInduk = sdr["NoInduk"].ToString();
        //            hasil.JenisUser = (JenisUser)sdr["JenisUser"];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message.ToString());
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //    return hasil;
        //}

        public static UserLogin GetByNIP(string nip)
        {
            UserLogin hasil = null;

            SqlConnection conn = Application_Libs.Libs.GetConnection_Person();
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand(SP_SELECT_BY_NIP, conn);
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@NIP", nip);
                
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    hasil = new UserLogin();
                    hasil.UserID = sdr["UserID"].ToString();
                    hasil.Password = sdr["Pwd"].ToString();
                    hasil.NoInduk = sdr["NIP"].ToString();
                    hasil.JenisUser = JenisUser.Karyawan;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        public static UserLogin SelectLoginAllUsers(string userid, string password)
        {
            UserLogin hasil = null;

            SqlConnection conn = Application_Libs.Libs.GetConnection_Person();
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand(SP_SELECT_LOGIN, conn);
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@password", Application_Libs.Libs.Encryptdata(password));

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    hasil = new UserLogin();
                    hasil.UserID = sdr["UserID"].ToString();
                    hasil.Password = sdr["Pwd"].ToString();
                    hasil.NoInduk = sdr["NIP"].ToString();
                    hasil.JenisUser = JenisUser.Karyawan;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }
    }
}