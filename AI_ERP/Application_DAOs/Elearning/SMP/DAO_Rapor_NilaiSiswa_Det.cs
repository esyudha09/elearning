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
    public static class DAO_Rapor_NilaiSiswa_Det
    {
        public const string SP_SELECT_BY_HEADER = "SMP_Rapor_NilaiSiswa_Det_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_STRUKTUR_NILAI = "SMP_Rapor_NilaiSiswa_Det_SELECT_BY_STRUKTUR_NILAI";
        public const string SP_SELECT_BY_TA_BY_SM = "SMP_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM";
        public const string SP_SELECT_BY_TA_BY_SM_BY_Kelas_BY_MAPEL = "SMP_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_Kelas_BY_MAPEL";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL = "SMP_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL";        
        public const string SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_SISWA = "SMP_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_SISWA";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_SISWA = "SMP_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_SISWA";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_SISWA = "SMP_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_SISWA";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_KP_BY_SISWA = "SMP_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_KP_BY_SISWA";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET = "SMP_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_KELASDET";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET_EXTEND = "SMP_Rapor_NilaiSiswa_Det_SELECT_BY_TA_BY_SM_BY_KelasDet_Extend";

        public const string SP_INSERT = "SMP_Rapor_NilaiSiswa_Det_INSERT";

        public const string SP_UPDATE = "SMP_Rapor_NilaiSiswa_Det_UPDATE";
        public const string SP_UPDATE_PB = "SMP_Rapor_NilaiSiswa_Det_UPDATE_PB";

        public const string SP_DELETE = "SMP_Rapor_NilaiSiswa_Det_DELETE";
        public const string SP_DELETE_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_KP_BY_SISWA = "SMP_Rapor_NilaiSiswa_Det_DELETE_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_KP_BY_SISWA";

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
            public const string Rel_Rapor_AspekPenilaian = "Rel_Rapor_AspekPenilaian";
            public const string Rel_Rapor_KompetensiDasar = "Rel_Rapor_KompetensiDasar";
            public const string Rel_Rapor_KomponenPenilaian = "Rel_Rapor_KomponenPenilaian";
            public const string NamaKD = "NamaKD";
            public const string MateriKP = "MateriKP";
            public const string DeskripsiKP = "DeskripsiKP";
            public const string UrutanAP = "UrutanAP";
            public const string UrutanKD = "UrutanKD";
            public const string UrutanKP = "UrutanKP";
            public const string Nilai = "Nilai";
            public const string PB = "PB";
            public const string LTS_HD = "LTS_HD";
            public const string LTS_MAX_HD = "LTS_MAX_HD";
        }

        public class Rapor_NilaiSiswa_Det_EXT : Rapor_NilaiSiswa_Det
        {
            public string Rel_Mapel { get; set; }
            public string Rel_Rapor_KomponenPenilaian { get; set; }
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
                Nilai = (
                    Application_Libs.Libs.IsAngka(row[NamaField.Nilai].ToString().Replace("=", ""))
                    ? row[NamaField.Nilai].ToString().Replace("=", "")
                    : row[NamaField.Nilai].ToString()
                ),
                PB = (
                    Application_Libs.Libs.IsAngka(row[NamaField.PB].ToString().Replace("=", ""))
                    ? row[NamaField.PB].ToString().Replace("=", "")
                    : row[NamaField.PB].ToString()
                )
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
                Rel_Rapor_StrukturNilai_AP = row[NamaField.Rel_Rapor_StrukturNilai_AP].ToString(),
                Rel_Rapor_StrukturNilai_KD = row[NamaField.Rel_Rapor_StrukturNilai_KD].ToString(),
                Rel_Rapor_StrukturNilai_KP = row[NamaField.Rel_Rapor_StrukturNilai_KP].ToString(),
                Rel_Rapor_KomponenPenilaian = row[NamaField.Rel_Rapor_KomponenPenilaian].ToString(),
                Nilai = (
                    Application_Libs.Libs.IsAngka(row[NamaField.Nilai].ToString().Replace("=", ""))
                    ? row[NamaField.Nilai].ToString().Replace("=", "")
                    : row[NamaField.Nilai].ToString()
                ),
                PB = (
                    Application_Libs.Libs.IsAngka(row[NamaField.PB].ToString().Replace("=", ""))
                    ? row[NamaField.PB].ToString().Replace("=", "")
                    : row[NamaField.PB].ToString()
                )
            };
        }

        private static Rapor_NilaiSiswa_Det_Extend GetEntityFromDataRow_Extend(DataRow row)
        {
            return new Rapor_NilaiSiswa_Det_Extend
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_NilaiSiswa = new Guid(row[NamaField.Rel_Rapor_NilaiSiswa].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Rapor_StrukturNilai_AP = row[NamaField.Rel_Rapor_StrukturNilai_AP].ToString(),
                Rel_Rapor_StrukturNilai_KD = row[NamaField.Rel_Rapor_StrukturNilai_KD].ToString(),
                Rel_Rapor_StrukturNilai_KP = row[NamaField.Rel_Rapor_StrukturNilai_KP].ToString(),
                Rel_Rapor_KompetensiDasar = row[NamaField.Rel_Rapor_KompetensiDasar].ToString(),
                Rel_Rapor_KomponenPenilaian = row[NamaField.Rel_Rapor_KomponenPenilaian].ToString(),
                NamaKD = row[NamaField.NamaKD].ToString(),
                MateriKP = row[NamaField.MateriKP].ToString(),
                DeskripsiKP = row[NamaField.DeskripsiKP].ToString(),
                UrutanAP = Convert.ToInt16(row[NamaField.UrutanAP]),
                UrutanKD = Convert.ToInt16(row[NamaField.UrutanKD]),
                UrutanKP = Convert.ToInt16(row[NamaField.UrutanKP]),
                Nilai = row[NamaField.Nilai].ToString(),
                PB = (row[NamaField.PB] == DBNull.Value ? "" : row[NamaField.PB].ToString()),
                LTS_HD = row[NamaField.LTS_HD].ToString(),
                LTS_MAX_HD = row[NamaField.LTS_MAX_HD].ToString()
            };
        }

        public static void DeleteByTABySMByKelasDetByMapelByAPByKDByKPBySiswa_Entity(
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
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_DELETE_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_KP_BY_SISWA;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_AP", rel_sn_ap);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_KD", rel_sn_kd);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_KP", rel_sn_kp);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);
                comm.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
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

        public static void UpdatePB(Rapor_NilaiSiswa_Det m, string user_id)
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
                comm.CommandText = SP_UPDATE_PB;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_NilaiSiswa, m.Rel_Rapor_NilaiSiswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_AP, m.Rel_Rapor_StrukturNilai_AP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KD, m.Rel_Rapor_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KP, m.Rel_Rapor_StrukturNilai_KP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PB, m.PB));

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

        public static List<Rapor_NilaiSiswa_Det> GetAllByTABySMByKelasDetByMapelByAPByKDBySiswa_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_mapel,
                string rel_sn_ap,
                string rel_sn_kd,
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

        public static List<Rapor_NilaiSiswa_Det> GetAllByTABySMByKelasDetByMapelByAPBySiswa_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_mapel,
                string rel_sn_ap,
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
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_SISWA;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_AP", rel_sn_ap);
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

        public static List<Rapor_NilaiSiswa_Det_Extend> GetAllByTABySMByKelasDetByMapel_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_mapel
            )
        {
            List<Rapor_NilaiSiswa_Det_Extend> hasil = new List<Rapor_NilaiSiswa_Det_Extend>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                
                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_Extend(row));
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

        public static List<Rapor_NilaiSiswa_Det_Extend> GetAllByTABySM_Entity(
                string tahun_ajaran,
                string semester
            )
        {
            List<Rapor_NilaiSiswa_Det_Extend> hasil = new List<Rapor_NilaiSiswa_Det_Extend>();
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
                    hasil.Add(GetEntityFromDataRow_Extend(row));
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

        public static List<Rapor_NilaiSiswa_Det_Extend> GetAllByTABySMByKelasDetByMapelBySiswa_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_mapel,
                string rel_siswa
            )
        {
            List<Rapor_NilaiSiswa_Det_Extend> hasil = new List<Rapor_NilaiSiswa_Det_Extend>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_SISWA;
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
                    hasil.Add(GetEntityFromDataRow_Extend(row));
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
        
        public static List<Rapor_NilaiSiswa_Det_Extend> GetAllByTABySMByKelasDet_Extend_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet
            )
        {
            List<Rapor_NilaiSiswa_Det_Extend> hasil = new List<Rapor_NilaiSiswa_Det_Extend>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET_EXTEND;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_Extend(row));
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

        public static List<Rapor_NilaiSiswa_Det> GetAllByStrukturNilai_Entity(
                string rel_sn_ap,
                string rel_sn_kd,
                string rel_sn_kp
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
                comm.CommandText = SP_SELECT_BY_STRUKTUR_NILAI;
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_AP", rel_sn_ap);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_KD", rel_sn_kd);
                comm.Parameters.AddWithValue("@Rel_Rapor_StrukturNilai_KP", rel_sn_kp);

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

        public static List<Rapor_NilaiSiswa_Det> GetAllByTABySMByKelasByMapel_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas,
                string rel_mapel
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
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_Kelas_BY_MAPEL;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);

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
    }
}