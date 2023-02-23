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
    public static class DAO_Rapor_SikapSemesterSiswa_Det
    {
        public const string SP_SELECT_BY_HEADER = "SD_Rapor_SikapSemesterSiswa_Det_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_KP_BY_SISWA =
                            "SD_Rapor_SikapSemesterSiswa_Det_SELECT_BY_TA_BY_SM_BY_KelasDet_BY_MAPEL_BY_AP_BY_KD_BY_KP_BY_SISWA";

        public const string SP_INSERT = "SD_Rapor_SikapSemesterSiswa_Det_INSERT";

        public const string SP_UPDATE = "SD_Rapor_SikapSemesterSiswa_Det_UPDATE";

        public const string SP_DELETE = "SD_Rapor_SikapSemesterSiswa_Det_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_SikapSemesterSiswa = "Rel_Rapor_SikapSiswa";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rel_Rapor_StrukturNilai_AP = "Rel_Rapor_StrukturNilai_AP";
            public const string Rel_Rapor_StrukturNilai_KD = "Rel_Rapor_StrukturNilai_KD";
            public const string Rel_Rapor_StrukturNilai_KP = "Rel_Rapor_StrukturNilai_KP";
            public const string Nilai = "Nilai";
        }

        private static Rapor_SikapSemesterSiswa_Det GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_SikapSemesterSiswa_Det
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_SikapSiswa = new Guid(row[NamaField.Rel_Rapor_SikapSemesterSiswa].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_Rapor_StrukturNilai_AP = row[NamaField.Rel_Rapor_StrukturNilai_AP].ToString(),
                Rel_Rapor_StrukturNilai_KD = row[NamaField.Rel_Rapor_StrukturNilai_KD].ToString(),
                Rel_Rapor_StrukturNilai_KP = row[NamaField.Rel_Rapor_StrukturNilai_KP].ToString(),
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

        public static void Insert(Rapor_SikapSemesterSiswa_Det m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@Rel_Rapor_SikapSemesterSiswa", m.Rel_Rapor_SikapSiswa));
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

        public static void Update(Rapor_SikapSemesterSiswa_Det m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@Rel_Rapor_SikapSemesterSiswa", m.Rel_Rapor_SikapSiswa));
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

        public static List<Rapor_SikapSemesterSiswa_Det> GetAllByHeader_Entity(
                string rel_rapor_nilai
            )
        {
            List<Rapor_SikapSemesterSiswa_Det> hasil = new List<Rapor_SikapSemesterSiswa_Det>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@Rel_Rapor_SikapSemester", rel_rapor_nilai);

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

        public static List<Rapor_SikapSemesterSiswa_Det> GetAllByTABySMByKelasDetByMapelByAPByKDByKPBySiswa_Entity(
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
            List<Rapor_SikapSemesterSiswa_Det> hasil = new List<Rapor_SikapSemesterSiswa_Det>();
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
    }
}