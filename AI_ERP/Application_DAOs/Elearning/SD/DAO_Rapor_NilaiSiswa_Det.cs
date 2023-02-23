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
    public static class DAO_Rapor_NilaiSiswa_Det
    {
        public const string SP_SELECT_BY_HEADER = "SD_Rapor_NilaiSiswa_Det_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_KP_BY_SISWA = "SD_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_KP_BY_SISWA";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS_DET_FOR_LTS = "SD_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_KELAS_DET_FOR_LTS";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS_DET_BY_MAPEL_BY_SISWA_FOR_LTS = "SD_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_KELAS_DET_BY_MAPEL_BY_SISWA_FOR_LTS";        
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET = "SD_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_KELASDET";
        public const string SP_SELECT_BY_TA_BY_SM = "SD_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM";
        public const string SP_SELECT_NILAI_BY_HEADER_BY_SISWA_BY_AP_BY_KD_BY_KP = "SD_Rapor_NilaiSiswa_Det_SELECT_NILAI_BY_HEADER_BY_SISWA_BY_AP_BY_KD_BY_KP";

        public const string SP_INSERT = "SD_Rapor_NilaiSiswa_Det_INSERT";

        public const string SP_UPDATE = "SD_Rapor_NilaiSiswa_Det_UPDATE";

        public const string SP_DELETE = "SD_Rapor_NilaiSiswa_Det_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_NilaiSiswa = "Rel_Rapor_NilaiSiswa";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_Rapor_StrukturNilai_AP = "Rel_Rapor_StrukturNilai_AP";
            public const string Rel_Rapor_StrukturNilai_KD = "Rel_Rapor_StrukturNilai_KD";
            public const string Rel_Rapor_StrukturNilai_KP = "Rel_Rapor_StrukturNilai_KP";
            public const string Rel_Rapor_KomponenPenilaian = "Rel_Rapor_KomponenPenilaian";
            public const string Nilai = "Nilai";
        }

        public class Rapor_NilaiSiswa_Det_EXT : Rapor_NilaiSiswa_Det
        {
            public string Rel_Mapel { get; set; }
            public string Rel_Rapor_KomponenPenilaian { get; set; }
            public string Rel_KelasDet { get; set; }
        }

        private static Rapor_NilaiSiswa_Det GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_NilaiSiswa_Det
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_NilaiSiswa = new Guid(row[NamaField.Rel_Rapor_NilaiSiswa].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_Rapor_StrukturNilai_AP = row[NamaField.Rel_Rapor_StrukturNilai_AP].ToString(),
                Rel_Rapor_StrukturNilai_KD = row[NamaField.Rel_Rapor_StrukturNilai_KD].ToString(),
                Rel_Rapor_StrukturNilai_KP = row[NamaField.Rel_Rapor_StrukturNilai_KP].ToString(),
                Nilai = row[NamaField.Nilai].ToString()
            };
        }

        private static Rapor_NilaiSiswa_Det_EXT GetEntityFromDataRowExt(DataRow row)
        {
            return new Rapor_NilaiSiswa_Det_EXT
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_NilaiSiswa = new Guid(row[NamaField.Rel_Rapor_NilaiSiswa].ToString()),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Rapor_StrukturNilai_AP = row[NamaField.Rel_Rapor_StrukturNilai_AP].ToString(),
                Rel_Rapor_StrukturNilai_KD = row[NamaField.Rel_Rapor_StrukturNilai_KD].ToString(),
                Rel_Rapor_StrukturNilai_KP = row[NamaField.Rel_Rapor_StrukturNilai_KP].ToString(),
                Rel_Rapor_KomponenPenilaian = row[NamaField.Rel_Rapor_KomponenPenilaian].ToString(),
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

        public static void Insert(Rapor_NilaiSiswa_Det m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_NilaiSiswa, m.Rel_Rapor_NilaiSiswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_AP, m.Rel_Rapor_StrukturNilai_AP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KD, m.Rel_Rapor_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KP, m.Rel_Rapor_StrukturNilai_KP));
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

        public static void Update(Rapor_NilaiSiswa_Det m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_NilaiSiswa, m.Rel_Rapor_NilaiSiswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_AP, m.Rel_Rapor_StrukturNilai_AP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KD, m.Rel_Rapor_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KP, m.Rel_Rapor_StrukturNilai_KP));
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

        public static List<Rapor_NilaiSiswa_Det> GetAllByHeader_Entity(
                string rel_rapor_nilai
            )
        {
            List<Rapor_NilaiSiswa_Det> hasil = new List<Rapor_NilaiSiswa_Det>();
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

        public static List<Rapor_NilaiSiswa_Det> GetAllByTABySMByKelasDetByMapelByAPByKDByKPBySiswa_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_mapel,
                string rel_sn_ap,
                string rel_sn_kd,
                string rel_sn_kp,
                string rel_siswa
            )
        {
            List<Rapor_NilaiSiswa_Det> hasil = new List<Rapor_NilaiSiswa_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_KP_BY_SISWA;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_AP", rel_sn_ap);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_KD", rel_sn_kd);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_KP", rel_sn_kp);
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

        public static string GetByHeaderBySiswaByAPByKDByKP_Entity(
                string rel_rapor_nilai,
                string rel_siswa,
                string rel_sn_ap,
                string rel_sn_kd,
                string rel_sn_kp                
            )
        {
            string hasil = "";
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_NILAI_BY_HEADER_BY_SISWA_BY_AP_BY_KD_BY_KP;
                comm.Parameters.AddWithValue("@Rel_Rapor_Nilai", rel_rapor_nilai);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_AP", rel_sn_ap);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_KD", rel_sn_kd);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_KP", rel_sn_kp);                

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = row[0].ToString();
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

        public static List<Rapor_NilaiSiswa_Det> GetAllByTABySMByKelasDetForLTS_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet
            )
        {
            List<Rapor_NilaiSiswa_Det> hasil = new List<Rapor_NilaiSiswa_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS_DET_FOR_LTS;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);

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

        public static List<Rapor_NilaiSiswa_Det> GetAllByTABySMByKelasDetByMapelBySiswaForLTS_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_mapel,
                string rel_siswa
            )
        {
            List<Rapor_NilaiSiswa_Det> hasil = new List<Rapor_NilaiSiswa_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS_DET_BY_MAPEL_BY_SISWA_FOR_LTS;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
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

        public static List<Rapor_NilaiSiswa_Det_EXT> GetAllByTABySMByKelasDet_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet
            )
        {
            List<Rapor_NilaiSiswa_Det_EXT> hasil = new List<Rapor_NilaiSiswa_Det_EXT>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRowExt(row));
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

        public static List<Rapor_NilaiSiswa_Det_EXT> GetAllByTABySM_Entity(
                string tahun_ajaran,
                string semester
            )
        {
            List<Rapor_NilaiSiswa_Det_EXT> hasil = new List<Rapor_NilaiSiswa_Det_EXT>();
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
                    hasil.Add(GetEntityFromDataRowExt(row));
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