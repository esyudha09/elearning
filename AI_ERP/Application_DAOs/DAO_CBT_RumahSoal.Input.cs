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
    public static class DAO_CBT_RumahSoal_Input
    {
        
       
        
        
        public const string SP_SELECT_BY_ID = "CBT_RUMAH_SOAL_SELECT_BY_ID";
        public const string SP_SELECT_BY_KP = "CBT_RUMAH_SOAL_SELECT_BY_KP";
                
        public const string SP_INSERT = "CBT_RUMAH_SOAL_INSERT";

        public const string SP_UPDATE = "CBT_RUMAH_SOAL_UPDATE";

        public const string SP_DELETE = "CBT_RUMAH_SOAL_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_StrukturNilai_KP = "Rel_Rapor_StrukturNilai_KP";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_Guru = "Rel_Guru";
            public const string Kurikulum = "Kurikulum";
            public const string Nama = "Nama";
            public const string Deskripsi = "Deskripsi";
            public const string StartDatetime = "StartDatetime";
            public const string EndDatetime = "EndDatetime";
            public const string LimitTime = "LimitTime";
            public const string LimitSatuan = "LimitSatuan";
            
        }

        public static CBT_RumahSoal GetEntityFromDataRow(DataRow row)
        {
            return new CBT_RumahSoal
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_StrukturNilai_KP = row[NamaField.Rel_Rapor_StrukturNilai_KP].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),              
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                Kurikulum = row[NamaField.Kurikulum].ToString(),
                Nama = row[NamaField.Nama].ToString(),
                Deskripsi = row[NamaField.Deskripsi].ToString(),
                StartDatetime = Convert.ToDateTime(row[NamaField.StartDatetime]),
                EndDatetime = Convert.ToDateTime(row[NamaField.EndDatetime]),
                LimitTime = (int)row[NamaField.LimitTime],
                LimitSatuan = row[NamaField.LimitSatuan].ToString()              
            };
        }

        

        public static CBT_RumahSoal GetByKP_Entity(string Rel_Rapor_StrukturNilai_KP)
        {
            CBT_RumahSoal hasil = new CBT_RumahSoal();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (Rel_Rapor_StrukturNilai_KP == null) return hasil;
            if (Rel_Rapor_StrukturNilai_KP.Length <= 10) return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_KP;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_StrukturNilai_KP, Rel_Rapor_StrukturNilai_KP);

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

        public static CBT_RumahSoal GetByID_Entity(string kode)
        {
            CBT_RumahSoal hasil = new CBT_RumahSoal();
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

        public static void Insert(CBT_RumahSoal RumahSoal, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, RumahSoal.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, RumahSoal.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KP, RumahSoal.Rel_Rapor_StrukturNilai_KP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kurikulum, RumahSoal.Kurikulum));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, RumahSoal.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, RumahSoal.Deskripsi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StartDatetime, RumahSoal.StartDatetime));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EndDatetime, RumahSoal.EndDatetime));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LimitSatuan, RumahSoal.LimitSatuan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LimitTime, RumahSoal.LimitTime));
                

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

        public static void Update(CBT_RumahSoal RumahSoal, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, RumahSoal.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, RumahSoal.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, RumahSoal.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KP, RumahSoal.Rel_Rapor_StrukturNilai_KP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kurikulum, RumahSoal.Kurikulum));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, RumahSoal.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, RumahSoal.Deskripsi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StartDatetime, RumahSoal.StartDatetime));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EndDatetime, RumahSoal.EndDatetime));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LimitSatuan, RumahSoal.LimitSatuan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LimitTime, RumahSoal.LimitTime));
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