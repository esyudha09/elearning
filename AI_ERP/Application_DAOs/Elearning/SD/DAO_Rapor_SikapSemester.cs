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
    public static class DAO_Rapor_SikapSemester
    {
        public const string SP_SELECT_ALL_FOR_SEARCH = "SD_Rapor_SikapSemester_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_MAPEL_BY_MAPEL_SIKAP = "SD_Rapor_SikapSemester_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_MAPEL_BY_MAPEL_SIKAP";
        public const string SP_SELECT_NILAI_SIKAP = "SD_Rapor_SikapSemester_SELECT_NILAI_SIKAP";
        public const string SP_SELECT_NILAI_SIKAP_BY_MAPEL = "SD_Rapor_SikapSemester_SELECT_NILAI_SIKAP_BY_MAPEL";
        public const string SP_SELECT_NILAI_SIKAP_BY_GURU_KELAS = "SD_Rapor_SikapSemester_SELECT_NILAI_SIKAP_BY_GURU_KELAS";

        public const string SP_INSERT = "SD_Rapor_SikapSemester_INSERT";

        public const string SP_UPDATE = "SD_Rapor_SikapSemester_UPDATE";

        public const string SP_DELETE = "SD_Rapor_SikapSemester_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_MapelSikapSemester = "Rel_MapelSikap";
            public const string Kurikulum = "Kurikulum";
        }

        private static Rapor_SikapSemester GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_SikapSemester
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Kelas = row[NamaField.Rel_Kelas].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_MapelSikap = row[NamaField.Rel_MapelSikapSemester].ToString(),
                Kurikulum = row[NamaField.Kurikulum].ToString()
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

        public static void Insert(Rapor_SikapSemester m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@Rel_MapelSikapSemester", m.Rel_MapelSikap));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kurikulum, m.Kurikulum));
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

        public static void Update(Rapor_SikapSemester m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@Rel_MapelSikapSemester", m.Rel_MapelSikap));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kurikulum, m.Kurikulum));
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

        public static List<Rapor_SikapSemester> GetAllByTABySMByKelasDetByMapel_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_mapel,
                string rel_mapel_SikapSemester
            )
        {
            List<Rapor_SikapSemester> hasil = new List<Rapor_SikapSemester>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_MAPEL_BY_MAPEL_SIKAP;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Mapel, rel_mapel);
                comm.Parameters.AddWithValue("@Rel_MapelSikapSemester", rel_mapel_SikapSemester);

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

        public static decimal GetNilaiSikapSemester_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_siswa,
                string rel_strukturnilai_ap,
                string rel_strukturnilai_kd,
                string rel_strukturnilai_kp,
                decimal bobot_guru_kelas,
                decimal bobot_guru_mapel
            )
        {
            decimal hasil = 0;
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_NILAI_SIKAP;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_AP", rel_strukturnilai_ap);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_KD", rel_strukturnilai_kd);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_KP", rel_strukturnilai_kp);
                comm.Parameters.AddWithValue("@BobotSikapSemesterGuruMapel", bobot_guru_mapel);
                comm.Parameters.AddWithValue("@BobotSikapSemesterGuruKelas", bobot_guru_kelas);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = Convert.ToDecimal(row[0]);
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

        public static decimal GetNilaiSikapSemesterByMapel_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_siswa,
                string rel_mapel,
                string rel_mapel_sikap
            )
        {
            decimal hasil = 0;
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_NILAI_SIKAP_BY_MAPEL;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_MapelSikap", rel_mapel_sikap);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = Convert.ToDecimal(row[0]);
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

        public static decimal GetNilaiSikapSemesterByGuruKelas_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_siswa,
                string rel_mapel_sikap
            )
        {
            decimal hasil = 0;
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_NILAI_SIKAP_BY_GURU_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);
                comm.Parameters.AddWithValue("@Rel_MapelSikap", rel_mapel_sikap);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = Convert.ToDecimal(row[0]);
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