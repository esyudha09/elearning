using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.SD;

namespace AI_ERP.Application_DAOs.Elearning.SD
{
    public static class DAO_Rapor_LTS_Sikap_Det
    {
        public const string SP_SELECT_ALL = "SD_Rapor_LTS_Sikap_Det_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "SD_Rapor_LTS_Sikap_Det_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "SD_Rapor_LTS_Sikap_Det_SELECT_BY_HEADER";        

        public const string SP_INSERT = "SD_Rapor_LTS_Sikap_Det_INSERT";

        public const string SP_UPDATE = "SD_Rapor_LTS_Sikap_Det_UPDATE";

        public const string SP_DELETE = "SD_Rapor_LTS_Sikap_Det_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_LTS = "Rel_Rapor_LTS";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_KD_Nilai1 = "Rel_KD_Nilai1";
            public const string Nilai1 = "Nilai1";
            public const string Rel_KD_Nilai2 = "Rel_KD_Nilai2";
            public const string Nilai2 = "Nilai2";
            public const string Rel_KD_Nilai3 = "Rel_KD_Nilai3";
            public const string Nilai3 = "Nilai3";
            public const string Rel_KD_Nilai4 = "Rel_KD_Nilai4";
            public const string Nilai4 = "Nilai4";
            public const string Rel_KD_Nilai5 = "Rel_KD_Nilai5";
            public const string Nilai5 = "Nilai5";
            public const string Rel_KD_Nilai6 = "Rel_KD_Nilai6";
            public const string Nilai6 = "Nilai6";
            public const string Rel_KD_Nilai7 = "Rel_KD_Nilai7";
            public const string Nilai7 = "Nilai7";
            public const string Rel_KD_Nilai8 = "Rel_KD_Nilai8";
            public const string Nilai8 = "Nilai8";
            public const string Rel_KD_Nilai9 = "Rel_KD_Nilai9";
            public const string Nilai9 = "Nilai9";
            public const string Rel_KD_Nilai10 = "Rel_KD_Nilai10";
            public const string Nilai10 = "Nilai10";

        }

        private static Rapor_LTS_Sikap_Det GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_LTS_Sikap_Det
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_LTS = new Guid(row[NamaField.Rel_Rapor_LTS].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_KD_Nilai1 = row[NamaField.Rel_KD_Nilai1].ToString(),
                Nilai1 = row[NamaField.Nilai1].ToString(),
                Rel_KD_Nilai2 = row[NamaField.Rel_KD_Nilai2].ToString(),
                Nilai2 = row[NamaField.Nilai2].ToString(),
                Rel_KD_Nilai3 = row[NamaField.Rel_KD_Nilai3].ToString(),
                Nilai3 = row[NamaField.Nilai3].ToString(),
                Rel_KD_Nilai4 = row[NamaField.Rel_KD_Nilai4].ToString(),
                Nilai4 = row[NamaField.Nilai4].ToString(),
                Rel_KD_Nilai5 = row[NamaField.Rel_KD_Nilai5].ToString(),
                Nilai5 = row[NamaField.Nilai5].ToString(),
                Rel_KD_Nilai6 = row[NamaField.Rel_KD_Nilai6].ToString(),
                Nilai6 = row[NamaField.Nilai6].ToString(),
                Rel_KD_Nilai7 = row[NamaField.Rel_KD_Nilai7].ToString(),
                Nilai7 = row[NamaField.Nilai7].ToString(),
                Rel_KD_Nilai8 = row[NamaField.Rel_KD_Nilai8].ToString(),
                Nilai8 = row[NamaField.Nilai8].ToString(),
                Rel_KD_Nilai9 = row[NamaField.Rel_KD_Nilai9].ToString(),
                Nilai9 = row[NamaField.Nilai9].ToString(),
                Rel_KD_Nilai10 = row[NamaField.Rel_KD_Nilai10].ToString(),
                Nilai10 = row[NamaField.Nilai10].ToString()
            };
        }

        public static void Delete(string Kode)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
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

        public static void Insert(Rapor_LTS_Sikap_Det m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_INSERT;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_LTS, m.Rel_Rapor_LTS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai1, m.Rel_KD_Nilai1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai1, m.Nilai1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai2, m.Rel_KD_Nilai2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai2, m.Nilai2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai3, m.Rel_KD_Nilai3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai3, m.Nilai3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai4, m.Rel_KD_Nilai4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai4, m.Nilai4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai5, m.Rel_KD_Nilai5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai5, m.Nilai5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai6, m.Rel_KD_Nilai6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai6, m.Nilai6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai7, m.Rel_KD_Nilai7));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai7, m.Nilai7));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai8, m.Rel_KD_Nilai8));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai8, m.Nilai8));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai9, m.Rel_KD_Nilai9));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai9, m.Nilai9));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai10, m.Rel_KD_Nilai10));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai10, m.Nilai10));
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

        public static void Update(Rapor_LTS_Sikap_Det m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_LTS, m.Rel_Rapor_LTS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai1, m.Rel_KD_Nilai1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai1, m.Nilai1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai2, m.Rel_KD_Nilai2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai2, m.Nilai2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai3, m.Rel_KD_Nilai3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai3, m.Nilai3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai4, m.Rel_KD_Nilai4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai4, m.Nilai4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai5, m.Rel_KD_Nilai5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai5, m.Nilai5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai6, m.Rel_KD_Nilai6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai6, m.Nilai6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai7, m.Rel_KD_Nilai7));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai7, m.Nilai7));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai8, m.Rel_KD_Nilai8));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai8, m.Nilai8));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai9, m.Rel_KD_Nilai9));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai9, m.Nilai9));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KD_Nilai10, m.Rel_KD_Nilai10));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai10, m.Nilai10));
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

        public static List<Rapor_LTS_Sikap_Det> GetAllByHeader_Entity(
                string rel_rapor_lts
            )
        {
            List<Rapor_LTS_Sikap_Det> hasil = new List<Rapor_LTS_Sikap_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_LTS, rel_rapor_lts);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow(row));
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
    }
}