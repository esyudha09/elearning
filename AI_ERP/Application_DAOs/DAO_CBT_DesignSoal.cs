using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;

namespace AI_ERP.Application_DAOs
{
    public static class DAO_CBT_DesignSoal
    {
        
       
        
        
        public const string SP_SELECT_BY_ID = "CBT_DESIGN_SOAL_SELECT_BY_ID";
        public const string SP_SELECT_BY_RS = "CBT_DESIGN_SOAL_SELECT_BY_RS";
        public const string SP_SELECT_JWB_BY_DS_SISWA = "CBT_DESIGN_SOAL_JWB_SELECT_BY_RS_BY_SISWA";
        public const string SP_SELECT_JWB_BY_RS_SISWA = "CBT_JWB_BY_RS_BY_SISWA";
                
        public const string SP_INSERT = "CBT_DESIGN_SOAL_INSERT";
        public const string SP_JWB_INSERT = "CBT_JWB_INSERT";

        public const string SP_UPDATE = "CBT_DESIGN_SOAL_UPDATE";

        public const string SP_DELETE = "CBT_DESIGN_SOAL_DELETE";
        public const string SP_JWB_DELETE = "CBT_JWB_DELETE";

        public const string SP_UPDATE_SKOR = "CBT_DESIGN_SOAL_UPDATE_SKOR";
        public const string SP_UPDATE_URUT = "CBT_DESIGN_SOAL_UPDATE_URUT";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_RumahSoal = "Rel_RumahSoal";
            public const string Rel_BankSoal = "Rel_BankSoal";          
            public const string Skor = "Skor";
            public const string Urut = "Urut";
            public const string JwbEssay = "JwbEssay";
            public const string Rel_JwbGanda = "Rel_JwbGanda";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rel_Jwb = "Rel_Jwb";
            public const string Rel_DesignSoal = "Rel_DesignSoal";

        }

        public static CBT_DesignSoal GetEntityFromDataRow(DataRow row)
        {
            return new CBT_DesignSoal
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_RumahSoal = row[NamaField.Rel_RumahSoal].ToString(),
                Rel_BankSoal = row[NamaField.Rel_BankSoal].ToString(),        
                Skor = (int)row[NamaField.Skor]
                            
            };
        }
        
        public static CBT_Jwb GetEntityJwbFromDataRow(DataRow row)
        {
            return new CBT_Jwb
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_RumahSoal = row[NamaField.Rel_RumahSoal].ToString(),
                Rel_DesignSoal = row[NamaField.Rel_DesignSoal].ToString(),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_JwbGanda = row[NamaField.Rel_JwbGanda].ToString(),
                JwbEssay = row[NamaField.JwbEssay].ToString(),
            };
        }
        
        
        //public static CBT_DesignSoalJwb GetEntityJwbFromDataRow(DataRow row)
        //{
        //    return new CBT_DesignSoalJwb
        //    {
        //        Kode = new Guid(row[NamaField.Kode].ToString()),
        //        Rel_RumahSoal = row[NamaField.Rel_RumahSoal].ToString(),
        //        Rel_BankSoal = row[NamaField.Rel_BankSoal].ToString(),
        //        Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
        //        Rel_Jwb = row[NamaField.Rel_Jwb].ToString(),
        //    };
        //}
        
               

        public static CBT_DesignSoal GetByRumahSoal_Entity(string Rel_RumahSoal)
        {
            CBT_DesignSoal hasil = new CBT_DesignSoal();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (Rel_RumahSoal == null) return hasil;
            if (Rel_RumahSoal.Length <= 10) return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_RS;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_RumahSoal, Rel_RumahSoal);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = GetEntityFromDataRow(row);
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }
        
       

        public static CBT_DesignSoal GetByID_Entity(string kode)
        {
            CBT_DesignSoal hasil = new CBT_DesignSoal();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            if (kode.Length <= 10) return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_ID;
                comm.Parameters.AddWithValue("@" + NamaField.Kode, kode);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = GetEntityFromDataRow(row);
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        //public static void Delete(string Kode, string user_id)
        //{
        //    SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
        //    SqlCommand comm = conn.CreateCommand();
        //    SqlTransaction transaction = null;
        //    try
        //    {
        //        conn.Open();
        //        transaction = conn.BeginTransaction();
        //        comm.Transaction = transaction;
        //        comm.CommandType = CommandType.StoredProcedure;
        //        comm.CommandText = SP_DELETE;

        //        comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, Kode));
        //        comm.Parameters.Add(new SqlParameter("@user_id", user_id));
        //        comm.ExecuteNonQuery();
        //        transaction.Commit();
        //    }
        //    catch (Exception ec)
        //    {
        //        transaction.Rollback();
        //        throw new Exception(ec.Message.ToString());
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        public static void Insert(CBT_DesignSoal DesignSoal, string user_id)
        {
            
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_INSERT;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, Guid.NewGuid()));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_RumahSoal, DesignSoal.Rel_RumahSoal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_BankSoal, DesignSoal.Rel_BankSoal));              
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Skor, DesignSoal.Skor));
                
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));

                comm.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static void UpdateSkor(CBT_DesignSoal DesignSoal, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE_SKOR;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, DesignSoal.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Skor, DesignSoal.Skor));

                comm.Parameters.Add(new SqlParameter("@user_id", user_id));

                comm.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static void UpdateUrut(CBT_DesignSoal DesignSoal, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE_URUT;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, DesignSoal.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urut, DesignSoal.Urut));

                comm.Parameters.Add(new SqlParameter("@user_id", user_id));

                comm.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }


        public static void InsertJwb(CBT_Jwb JwbSoal)
        {

            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_JWB_INSERT;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, Guid.NewGuid()));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_RumahSoal, JwbSoal.Rel_RumahSoal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_DesignSoal, JwbSoal.Rel_DesignSoal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_BankSoal, JwbSoal.Rel_BankSoal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, JwbSoal.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_JwbGanda, JwbSoal.Rel_JwbGanda));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbEssay, JwbSoal.JwbEssay));


                comm.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static void DeleteJwb(string Rel_DesignSoal)
        {

            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_JWB_DELETE;

                
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_DesignSoal, Rel_DesignSoal));
               

                comm.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }





    }
}