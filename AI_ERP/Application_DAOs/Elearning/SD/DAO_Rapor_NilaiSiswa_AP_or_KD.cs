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
    public static class DAO_Rapor_NilaiSiswa_AP_or_KD
    {
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_MAPEL_BY_SISWA_BY_AP_or_KD = 
            "SD_Rapor_NilaiSiswa_AP_or_KD_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_MAPEL_BY_SISWA_BY_AP_or_KD";

        public const string SP_INSERT = "SD_Rapor_NilaiSiswa_AP_or_KD_INSERT";

        public const string SP_UPDATE = "SD_Rapor_NilaiSiswa_AP_or_KD_UPDATE";

        public const string SP_DELETE = "SD_Rapor_NilaiSiswa_AP_or_KD_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_Nilai = "Rel_Rapor_Nilai";
            public const string Rel_StrukturNilai_AP = "Rel_StrukturNilai_AP";
            public const string Rel_StrukturNilai_KD = "Rel_StrukturNilai_KD";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Nilai = "Nilai";
        }

        private static Rapor_NilaiSiswa_AP_or_KD GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_NilaiSiswa_AP_or_KD
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_Nilai = row[NamaField.Rel_Rapor_Nilai].ToString(),
                Rel_StrukturNilai_AP = row[NamaField.Rel_StrukturNilai_AP].ToString(),
                Rel_StrukturNilai_KD = row[NamaField.Rel_StrukturNilai_KD].ToString(),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
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

        public static void Insert(Rapor_NilaiSiswa_AP_or_KD m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Nilai, m.Rel_Rapor_Nilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_AP, m.Rel_StrukturNilai_AP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_KD, m.Rel_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai, m.Nilai));
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

        public static void Update(Rapor_NilaiSiswa_AP_or_KD m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Nilai, m.Rel_Rapor_Nilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_AP, m.Rel_StrukturNilai_AP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_KD, m.Rel_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai, m.Nilai));
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

        public static List<Rapor_NilaiSiswa_AP_or_KD> GetAllByTABySMByKelasByMapelBySiswaByStruktur_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_siswa,
                string rel_mapel,
                string rel_strukturnilai_ap_or_kd
            )
        {
            List<Rapor_NilaiSiswa_AP_or_KD> hasil = new List<Rapor_NilaiSiswa_AP_or_KD>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_MAPEL_BY_SISWA_BY_AP_or_KD;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);
                comm.Parameters.AddWithValue("@Rel_StrukturNilai_AP_or_KD", rel_strukturnilai_ap_or_kd);

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