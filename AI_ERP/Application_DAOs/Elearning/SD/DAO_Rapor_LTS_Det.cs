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
    public static class DAO_Rapor_LTS_Det
    {
        public const string SP_SELECT_ALL = "SD_Rapor_LTS_Det_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "SD_Rapor_LTS_Det_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "SD_Rapor_LTS_Det_SELECT_BY_HEADER";

        public const string SP_INSERT = "SD_Rapor_LTS_Det_INSERT";

        public const string SP_UPDATE = "SD_Rapor_LTS_Det_UPDATE";

        public const string SP_DELETE = "SD_Rapor_LTS_Det_DELETE";
        public const string SP_DELETE_BY_HEADER = "SD_Rapor_LTS_Det_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_LTS = "Rel_Rapor_LTS";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Nilai1 = "Nilai1";
            public const string Nilai2 = "Nilai2";
            public const string Nilai3 = "Nilai3";
            public const string Nilai4 = "Nilai4";
            public const string Nilai5 = "Nilai5";
            public const string Nilai6 = "Nilai6";
            public const string Nilai7 = "Nilai7";
            public const string Nilai8 = "Nilai8";
            public const string Nilai9 = "Nilai9";
            public const string Nilai10 = "Nilai10";
            public const string Nilai11 = "Nilai11";
            public const string Nilai12 = "Nilai12";
            public const string Nilai13 = "Nilai13";
            public const string Nilai14 = "Nilai14";
            public const string Nilai15 = "Nilai15";
            public const string Nilai16 = "Nilai16";
            public const string Nilai17 = "Nilai17";
            public const string Nilai18 = "Nilai18";
            public const string Nilai19 = "Nilai19";
            public const string Nilai20 = "Nilai20";

        }

        private static Rapor_LTS_Det GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_LTS_Det
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_LTS = new Guid(row[NamaField.Rel_Rapor_LTS].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Nilai1 = row[NamaField.Nilai1].ToString(),
                Nilai2 = row[NamaField.Nilai2].ToString(),
                Nilai3 = row[NamaField.Nilai3].ToString(),
                Nilai4 = row[NamaField.Nilai4].ToString(),
                Nilai5 = row[NamaField.Nilai5].ToString(),
                Nilai6 = row[NamaField.Nilai6].ToString(),
                Nilai7 = row[NamaField.Nilai7].ToString(),
                Nilai8 = row[NamaField.Nilai8].ToString(),
                Nilai9 = row[NamaField.Nilai9].ToString(),
                Nilai10 = row[NamaField.Nilai10].ToString(),
                Nilai11 = row[NamaField.Nilai11].ToString(),
                Nilai12 = row[NamaField.Nilai12].ToString(),
                Nilai13 = row[NamaField.Nilai13].ToString(),
                Nilai14 = row[NamaField.Nilai14].ToString(),
                Nilai15 = row[NamaField.Nilai15].ToString(),
                Nilai16 = row[NamaField.Nilai16].ToString(),
                Nilai17 = row[NamaField.Nilai17].ToString(),
                Nilai18 = row[NamaField.Nilai18].ToString(),
                Nilai19 = row[NamaField.Nilai19].ToString(),
                Nilai20 = row[NamaField.Nilai20].ToString()
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

        public static void DeleteByHeader(string Kode)
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
                comm.CommandText = SP_DELETE_BY_HEADER;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_LTS, Kode));
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

        public static void Insert(Rapor_LTS_Det m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai1, m.Nilai1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai2, m.Nilai2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai3, m.Nilai3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai4, m.Nilai4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai5, m.Nilai5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai6, m.Nilai6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai7, m.Nilai7));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai8, m.Nilai8));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai9, m.Nilai9));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai10, m.Nilai10));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai11, m.Nilai11));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai12, m.Nilai12));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai13, m.Nilai13));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai14, m.Nilai14));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai15, m.Nilai15));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai16, m.Nilai16));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai17, m.Nilai17));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai18, m.Nilai18));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai19, m.Nilai19));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai20, m.Nilai20));
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

        public static void Update(Rapor_LTS_Det m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai1, m.Nilai1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai2, m.Nilai2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai3, m.Nilai3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai4, m.Nilai4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai5, m.Nilai5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai6, m.Nilai6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai7, m.Nilai7));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai8, m.Nilai8));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai9, m.Nilai9));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai10, m.Nilai10));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai11, m.Nilai11));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai12, m.Nilai12));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai13, m.Nilai13));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai14, m.Nilai14));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai15, m.Nilai15));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai16, m.Nilai16));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai17, m.Nilai17));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai18, m.Nilai18));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai19, m.Nilai19));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai20, m.Nilai20));
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

        public static List<Rapor_LTS_Det> GetAllByHeader_Entity(
                string rel_rapor_lts
            )
        {
            List<Rapor_LTS_Det> hasil = new List<Rapor_LTS_Det>();
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