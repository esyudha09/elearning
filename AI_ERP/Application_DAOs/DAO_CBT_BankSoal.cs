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
    public static class DAO_CBT_BankSoal
    {
        public const string SP_SELECT_ALL_BY_MAPEL = "CBT_BANK_SOAL_SELECT_ALL_BY_MAPEL";
        public const string SP_SELECT_ALL_BY_MAPEL_FOR_SEARCH = "CBT_BANK_SOAL_SELECT_ALL_BY_MAPEL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "CBT_BANK_SOAL_SELECT_BY_ID";
        
        public const string SP_SELECT_BY_GURU_BY_TA = "Mapel_SELECT_BY_GURU_BY_TA";
        public const string SP_SELECT_BY_GURU_BY_TA_FOR_SEARCH = "Mapel_SELECT_BY_GURU_BY_TA_FOR_SEARCH";

        //public const string SP_SELECT_ALL = "CBT_BankSoal_SELECT_ALL";       
        //public const string SP_SELECT_ALL_FOR_SEARCH = "CBT_BankSoal_SELECT_ALL_FOR_SEARCH";
        //public const string SP_SELECT_ALL_BY_UNIT_FOR_SEARCH = "CBT_BankSoal_SELECT_ALL_BY_UNIT_FOR_SEARCH";
        //public const string SP_SELECT_BY_SEKOLAH = "CBT_BankSoal_SELECT_BY_SEKOLAH";
        //public const string SP_SELECT_BY_STRUKTUR_NILAI = "CBT_BankSoal_SELECT_BY_STRUKTUR_NILAI_SD";
        //public const string SP_SELECT_BY_GURU = "CBT_BankSoal_SELECT_BY_GURU";
        //public const string SP_SELECT_BY_GURU_BY_TA = "CBT_BankSoal_SELECT_BY_GURU_BY_TA";
        //public const string SP_SELECT_BY_GURU_BY_TA_FOR_SEARCH = "CBT_BankSoal_SELECT_BY_GURU_BY_TA_FOR_SEARCH";        
        //public const string SP_SELECT_DISTINCT_ABSEN_BY_TA_BY_UNIT_BY_SEMESTER = "CBT_BankSoal_SELECT_DISTINCT_ABSEN_BY_TA_BY_UNIT_BY_SEMESTER";

        public const string SP_INSERT = "CBT_BANK_SOAL_INSERT";

        public const string SP_UPDATE = "CBT_BANK_SOAL_UPDATE";

        public const string SP_DELETE = "CBT_BANK_SOAL_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_Guru = "Rel_Guru";
            public const string Soal = "Soal";
            public const string Jenis = "Jenis";
            public const string JwbEssay = "JwbEssay";
            public const string JwbGanda = "JwbGanda";
            public const string JwbGanda1 = "JwbGanda1";
            public const string JwbGanda2 = "JwbGanda2";
            public const string JwbGanda3 = "JwbGanda3";
            public const string JwbGanda4 = "JwbGanda4";
            public const string JwbGanda5 = "JwbGanda5";
            public const string id_output = "id_output";
        }

        public static CBT_BankSoal GetEntityFromDataRow(DataRow row)
        {
            return new CBT_BankSoal
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),              
                Soal = row[NamaField.Soal].ToString(),
                Jenis = row[NamaField.Jenis].ToString(),
                JwbEssay = row[NamaField.JwbEssay].ToString(),
                JwbGanda1 = row[NamaField.JwbGanda1].ToString(),
                JwbGanda2 = row[NamaField.JwbGanda2].ToString(),
                JwbGanda3 = row[NamaField.JwbGanda3].ToString(),
                JwbGanda4 = row[NamaField.JwbGanda4].ToString(),
                JwbGanda5 = row[NamaField.JwbGanda5].ToString(),
                JwbGanda = row[NamaField.JwbGanda].ToString()
            };
        }

        

        public static CBT_BankSoal GetByID_Entity(string kode)
        {
            CBT_BankSoal hasil = new CBT_BankSoal();
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

        public static void Delete(string Kode, string user_id)
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
                comm.CommandText = SP_DELETE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, Kode));
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
        public static void Insert(CBT_BankSoal BankSoal, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, BankSoal.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, BankSoal.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, BankSoal.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Soal, BankSoal.Soal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, BankSoal.Jenis));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbEssay, BankSoal.JwbEssay));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbGanda, BankSoal.JwbGanda));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbGanda1, BankSoal.JwbGanda1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbGanda2, BankSoal.JwbGanda2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbGanda3, BankSoal.JwbGanda3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbGanda4, BankSoal.JwbGanda4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbGanda5, BankSoal.JwbGanda5));
                

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

        public static void Update(CBT_BankSoal BankSoal, string user_id)
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
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, BankSoal.Kode));
                //comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, BankSoal.Rel_Mapel));
                //comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, BankSoal.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Soal, BankSoal.Soal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, BankSoal.Jenis));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbEssay, BankSoal.JwbEssay));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbGanda, BankSoal.JwbGanda));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbGanda1, BankSoal.JwbGanda1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbGanda2, BankSoal.JwbGanda2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbGanda3, BankSoal.JwbGanda3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbGanda4, BankSoal.JwbGanda4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbGanda5, BankSoal.JwbGanda5));               
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

        

       
    }
}