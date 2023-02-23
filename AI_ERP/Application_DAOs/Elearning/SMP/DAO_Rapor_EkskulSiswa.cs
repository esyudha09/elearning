using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.SMP;

namespace AI_ERP.Application_DAOs.Elearning.SMP
{
    public static class DAO_Rapor_EkskulSiswa
    {
        public const string SP_SELECT_BY_HEADER = "SMP_Rapor_EkskulSiswa_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_HEADER_BY_SISWA = "SMP_Rapor_EkskulSiswa_SELECT_BY_HEADER_BY_SISWA";

        public const string SP_INSERT = "SMP_Rapor_EkskulSiswa_INSERT";

        public const string SP_UPDATE = "SMP_Rapor_EkskulSiswa_UPDATE";

        public const string SP_DELETE = "SMP_Rapor_EkskulSiswa_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_Ekskul = "Rel_Rapor_Ekskul";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rapor = "Rapor";
            public const string Sakit = "Sakit";
            public const string Izin = "Izin";
            public const string Alpa = "Alpa";
        }

        private static Rapor_EkskulSiswa GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_EkskulSiswa
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_Ekskul = new Guid(row[NamaField.Rel_Rapor_Ekskul].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rapor = row[NamaField.Rapor].ToString(),
                Sakit = row[NamaField.Sakit].ToString(),
                Izin = row[NamaField.Izin].ToString(),
                Alpa = row[NamaField.Alpa].ToString()
            };
        }

        public static void Delete(string Kode, string user_id)
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

        public static void Insert(Rapor_EkskulSiswa m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Ekskul, m.Rel_Rapor_Ekskul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rapor, m.Rapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Sakit, m.Sakit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Izin, m.Izin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alpa, m.Alpa));

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

        public static void Update(Rapor_EkskulSiswa m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Ekskul, m.Rel_Rapor_Ekskul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rapor, m.Rapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Sakit, m.Sakit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Izin, m.Izin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alpa, m.Alpa));

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

        public static List<Rapor_EkskulSiswa> GetAllByHeader_Entity(
                string rel_rapor_sikap
            )
        {
            List<Rapor_EkskulSiswa> hasil = new List<Rapor_EkskulSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_Ekskul, rel_rapor_sikap);

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

        public static List<Rapor_EkskulSiswa> GetAllByHeaderBySiswa_Entity(
                string rel_rapor_sikap,
                string rel_siswa
            )
        {
            List<Rapor_EkskulSiswa> hasil = new List<Rapor_EkskulSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER_BY_SISWA;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_Ekskul, rel_rapor_sikap);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Siswa, rel_siswa);

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