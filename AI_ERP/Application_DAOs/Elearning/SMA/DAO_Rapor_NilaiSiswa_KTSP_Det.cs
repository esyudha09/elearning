using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.SMA;

namespace AI_ERP.Application_DAOs.Elearning.SMA
{
    public static class DAO_Rapor_NilaiSiswa_KTSP_Det
    {
        public const string SP_SELECT_BY_HEADER = "SMA_Rapor_NilaiSiswa_KTSP_Det_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_TA_BY_SM = "SMA_Rapor_NilaiSiswa_KTSP_Det_SELECT_BY_TA_BY_SM";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS_DET = "SMA_Rapor_NilaiSiswa_KTSP_Det_SELECT_BY_TA_BY_SM_BY_KelasDet";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_KD_BY_KP_BY_SISWA = "SMA_Rapor_NilaiSiswa_KTSP_Det_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_KD_BY_KP_BY_SISWA";

        public const string SP_INSERT = "SMA_Rapor_NilaiSiswa_KTSP_Det_INSERT";

        public const string SP_UPDATE = "SMA_Rapor_NilaiSiswa_KTSP_Det_UPDATE";

        public const string SP_DELETE = "SMA_Rapor_NilaiSiswa_KTSP_Det_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_NilaiSiswa_KTSP = "Rel_Rapor_NilaiSiswa_KTSP";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Rapor_StrukturNilai_KTSP_KD = "Rel_Rapor_StrukturNilai_KTSP_KD";
            public const string Rel_Rapor_StrukturNilai_KTSP_KP = "Rel_Rapor_StrukturNilai_KTSP_KP";
            public const string Nilai = "Nilai";
        }

        private static Rapor_NilaiSiswa_KTSP_Det GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_NilaiSiswa_KTSP_Det
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_NilaiSiswa_KTSP = new Guid(row[NamaField.Rel_Rapor_NilaiSiswa_KTSP].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_Rapor_StrukturNilai_KTSP_KD = row[NamaField.Rel_Rapor_StrukturNilai_KTSP_KD].ToString(),
                Rel_Rapor_StrukturNilai_KTSP_KP = row[NamaField.Rel_Rapor_StrukturNilai_KTSP_KP].ToString(),
                Nilai = row[NamaField.Nilai].ToString()
            };
        }

        private static Rapor_NilaiSiswa_KTSP_Det_Lengkap GetEntityFromDataRow_Lengkap(DataRow row)
        {
            return new Rapor_NilaiSiswa_KTSP_Det_Lengkap
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_NilaiSiswa_KTSP = new Guid(row[NamaField.Rel_Rapor_NilaiSiswa_KTSP].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Rapor_StrukturNilai_KTSP_KD = row[NamaField.Rel_Rapor_StrukturNilai_KTSP_KD].ToString(),
                Rel_Rapor_StrukturNilai_KTSP_KP = row[NamaField.Rel_Rapor_StrukturNilai_KTSP_KP].ToString(),                
                Nilai = row[NamaField.Nilai].ToString()
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

        public static void Insert(Rapor_NilaiSiswa_KTSP_Det m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_NilaiSiswa_KTSP, m.Rel_Rapor_NilaiSiswa_KTSP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KTSP_KD, m.Rel_Rapor_StrukturNilai_KTSP_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KTSP_KP, m.Rel_Rapor_StrukturNilai_KTSP_KP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai, m.Nilai));

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

        public static void Update(Rapor_NilaiSiswa_KTSP_Det m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_NilaiSiswa_KTSP, m.Rel_Rapor_NilaiSiswa_KTSP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KTSP_KD, m.Rel_Rapor_StrukturNilai_KTSP_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KTSP_KP, m.Rel_Rapor_StrukturNilai_KTSP_KP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai, m.Nilai));

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

        public static List<Rapor_NilaiSiswa_KTSP_Det> GetAllByTABySMByKelasDet_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas_det
            )
        {
            List<Rapor_NilaiSiswa_KTSP_Det> hasil = new List<Rapor_NilaiSiswa_KTSP_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS_DET;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelas_det);

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

        public static List<Rapor_NilaiSiswa_KTSP_Det_Lengkap> GetAllByTABySM_Entity(
                string tahun_ajaran,
                string semester
            )
        {
            List<Rapor_NilaiSiswa_KTSP_Det_Lengkap> hasil = new List<Rapor_NilaiSiswa_KTSP_Det_Lengkap>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_Lengkap(row));
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

        public static List<Rapor_NilaiSiswa_KTSP_Det> GetAllByHeader_Entity(
                string rel_rapor_nilai
            )
        {
            List<Rapor_NilaiSiswa_KTSP_Det> hasil = new List<Rapor_NilaiSiswa_KTSP_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@Rel_Rapor_Nilai", rel_rapor_nilai);

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

        public static List<Rapor_NilaiSiswa_KTSP_Det> GetAllByTABySMByKelasDetByMapelByKDByKPBySiswa_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_mapel,
                string rel_sn_kd,
                string rel_sn_kp,
                string rel_siswa
            )
        {
            List<Rapor_NilaiSiswa_KTSP_Det> hasil = new List<Rapor_NilaiSiswa_KTSP_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_KD_BY_KP_BY_SISWA;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_KTSP_KD", rel_sn_kd);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_KTSP_KP", rel_sn_kp);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);

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