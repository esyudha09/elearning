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
    public static class DAO_Rapor_PilihEkstrakurikuler
    {
        public const string SP_SELECT_ALL = "SD_Rapor_PilihEkstrakurikuler_SELECT_ALL";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET = "SD_Rapor_PilihEkstrakurikuler_SELECT_BY_TA_BY_SM_BY_KELASDET";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_SISWA = "SD_Rapor_PilihEkstrakurikuler_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_SISWA";
        public const string SP_SELECT_BY_TA_BY_SM_BY_MAPEL_BY_KELAS = "SD_Rapor_PilihEkstrakurikuler_SELECT_BY_TA_BY_SM_BY_MAPEL_BY_KELAS";
        public const string SP_SELECT_BY_ID = "SD_Rapor_PilihEkstrakurikuler_SELECT_BY_ID";

        public const string SP_INSERT = "SD_Rapor_PilihEkstrakurikuler_INSERT";

        public const string SP_UPDATE = "SD_Rapor_PilihEkstrakurikuler_UPDATE";

        public const string SP_DELETE = "SD_Rapor_PilihEkstrakurikuler_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rel_Mapel1 = "Rel_Mapel1";
            public const string Rel_Mapel2 = "Rel_Mapel2";
            public const string Rel_Mapel3 = "Rel_Mapel3";
            public const string Rel_Mapel4 = "Rel_Mapel4";
            public const string Rel_Mapel5 = "Rel_Mapel5";
        }

        private static Rapor_PilihEkstrakurikuler GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_PilihEkstrakurikuler
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_Mapel1 = row[NamaField.Rel_Mapel1].ToString(),
                Rel_Mapel2 = row[NamaField.Rel_Mapel2].ToString(),
                Rel_Mapel3 = row[NamaField.Rel_Mapel3].ToString(),
                Rel_Mapel4 = row[NamaField.Rel_Mapel4].ToString(),
                Rel_Mapel5 = row[NamaField.Rel_Mapel5].ToString()
            };
        }

        public static List<Rapor_PilihEkstrakurikuler> GetAll_Entity()
        {
            List<Rapor_PilihEkstrakurikuler> hasil = new List<Rapor_PilihEkstrakurikuler>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL;

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

        public static Rapor_PilihEkstrakurikuler GetByID_Entity(string kode)
        {
            Rapor_PilihEkstrakurikuler hasil = new Rapor_PilihEkstrakurikuler();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
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

        public static List<Rapor_PilihEkstrakurikuler> GetByTABySMByKelasDet_Entity(string tahun_ajaran, string semester, string rel_kelasdet)
        {
            List<Rapor_PilihEkstrakurikuler> hasil = new List<Rapor_PilihEkstrakurikuler>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);

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

        public static List<Rapor_PilihEkstrakurikuler> GetByTABySMByKelasDetBySiswa_Entity(string tahun_ajaran, string semester, string rel_kelasdet, string rel_siswa)
        {
            List<Rapor_PilihEkstrakurikuler> hasil = new List<Rapor_PilihEkstrakurikuler>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_SISWA;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
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

        public static List<Rapor_PilihEkstrakurikuler> GetByTABySMByMapelByKelas_Entity(string tahun_ajaran, string semester, string rel_mapel, string rel_kelas)
        {
            List<Rapor_PilihEkstrakurikuler> hasil = new List<Rapor_PilihEkstrakurikuler>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_MAPEL_BY_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);

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

        public static void Insert(Rapor_PilihEkstrakurikuler m)
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

                Guid kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel1, m.Rel_Mapel1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel2, m.Rel_Mapel2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel3, m.Rel_Mapel3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel4, m.Rel_Mapel4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel5, m.Rel_Mapel5));
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

        public static void Update(Rapor_PilihEkstrakurikuler m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel1, m.Rel_Mapel1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel2, m.Rel_Mapel2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel3, m.Rel_Mapel3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel4, m.Rel_Mapel4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel5, m.Rel_Mapel5));
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