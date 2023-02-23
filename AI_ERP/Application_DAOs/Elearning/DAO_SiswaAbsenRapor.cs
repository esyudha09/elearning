using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;

namespace AI_ERP.Application_DAOs.Elearning
{
    public static class DAO_SiswaAbsenRapor
    {
        public const string SP_SELECT_BY_ID = "SiswaAbsenRapor_SELECT_BY_ID";
        public const string SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_DET = "SiswaAbsenRapor_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_DET";
        public const string SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_DET_BY_SISWA = "SiswaAbsenRapor_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_DET_BY_SISWA";

        public const string SP_INSERT = "SiswaAbsenRapor_INSERT";

        public const string SP_SAVE_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_DET_BY_SISWA = "SiswaAbsenRapor_SAVE_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_DET_BY_SISWA";

        public const string SP_UPDATE = "SiswaAbsenRapor_UPDATE";

        public const string SP_DELETE = "SiswaAbsenRapor_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Sakit = "Sakit";
            public const string Izin = "Izin";
            public const string Alpa = "Alpa";
        }

        private static SiswaAbsenRapor GetEntityFromDataRow(DataRow row)
        {
            return new SiswaAbsenRapor
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Sakit = Convert.ToInt16(row[NamaField.Sakit].ToString()),
                Izin = Convert.ToInt16(row[NamaField.Izin].ToString()),
                Alpa = Convert.ToInt16(row[NamaField.Alpa].ToString())
            };
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

        public static void Insert(SiswaAbsenRapor m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Sakit, m.Sakit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Izin, m.Izin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alpa, m.Alpa));
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

        public static void Save_ByTABySMByKelasDetBySiswa(SiswaAbsenRapor m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SAVE_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_DET_BY_SISWA;

                Guid kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Sakit, m.Sakit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Izin, m.Izin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alpa, m.Alpa));
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

        public static void Update(SiswaAbsenRapor m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Sakit, m.Sakit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Izin, m.Izin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alpa, m.Alpa));
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

        public static List<SiswaAbsenRapor> GetAllByTABySMByKelasDetBySiswa_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas_det,
                string rel_siswa
            )
        {
            List<SiswaAbsenRapor> hasil = new List<SiswaAbsenRapor>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_DET_BY_SISWA;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelas_det);
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

        public static List<SiswaAbsenRapor> GetAllByTABySMByKelasDet_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas_det
            )
        {
            List<SiswaAbsenRapor> hasil = new List<SiswaAbsenRapor>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_DET;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelas_det);

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