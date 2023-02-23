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
    public static class DAO_Rapor_NilaiEkskulSiswa
    {
        public const string SP_SELECT_BY_TA_BY_SM_BY_MAPEL = "SMA_Rapor_NilaiEkskulSiswa_SELECT_BY_TA_BY_SM_BY_MAPEL";
        public const string SP_SELECT_BY_TA_BY_SM_BY_MAPEL_BY_SISWA = "SMA_Rapor_NilaiEkskulSiswa_SELECT_BY_TA_BY_SM_BY_MAPEL_BY_SISWA";        
        public const string SP_SELECT_BY_ID = "SMA_Rapor_NilaiEkskul_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER_BY_SISWA = "SMA_Rapor_NilaiEkskulSiswa_SELECT_BY_HEADER_BY_SISWA";
        
        public const string SP_INSERT = "SMA_Rapor_NilaiEkskulSiswa_INSERT";

        public const string SP_UPDATE = "SMA_Rapor_NilaiEkskulSiswa_UPDATE";

        public const string SP_SAVE_BY_TA_BY_SM_BY_MAPEL_BY_SISWA = "SMA_Rapor_NilaiEkskulSiswa_SAVE_BY_TA_BY_SM_BY_MAPEL_BY_SISWA";
        public const string SP_SAVE_LTSHD_BY_TA_BY_SM_BY_MAPEL_BY_SISWA = "SMA_Rapor_NilaiEkskulSiswa_SAVE_LTSHD_BY_TA_BY_SM_BY_MAPEL_BY_SISWA";
        public const string SP_SAVE_SAKIT_BY_TA_BY_SM_BY_MAPEL_BY_SISWA = "SMA_Rapor_NilaiEkskulSiswa_SAVE_SAKIT_BY_TA_BY_SM_BY_MAPEL_BY_SISWA";
        public const string SP_SAVE_IZIN_BY_TA_BY_SM_BY_MAPEL_BY_SISWA = "SMA_Rapor_NilaiEkskulSiswa_SAVE_IZIN_BY_TA_BY_SM_BY_MAPEL_BY_SISWA";
        public const string SP_SAVE_ALPA_BY_TA_BY_SM_BY_MAPEL_BY_SISWA = "SMA_Rapor_NilaiEkskulSiswa_SAVE_ALPA_BY_TA_BY_SM_BY_MAPEL_BY_SISWA";

        public const string SP_DELETE = "SMA_Rapor_NilaiEkskulSiswa_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_NilaiEkskul = "Rel_Rapor_NilaiEkskul";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Nilai = "Nilai";
            public const string LTS_HD = "LTS_HD";
            public const string Sakit = "Sakit";
            public const string Izin = "Izin";
            public const string Alpa = "Alpa";
        }

        private static Rapor_NilaiEkskulSiswa GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_NilaiEkskulSiswa
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_NilaiEkskul = row[NamaField.Rel_Rapor_NilaiEkskul].ToString(),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Nilai = row[NamaField.Nilai].ToString(),
                LTS_HD = row[NamaField.LTS_HD].ToString(),
                Sakit = row[NamaField.Sakit].ToString(),
                Izin = row[NamaField.Izin].ToString(),
                Alpa = row[NamaField.Alpa].ToString()
            };
        }

        public static List<Rapor_NilaiEkskulSiswa> GetByTABySMByMapel_Entity(string tahun_ajaran, string semester, string rel_mapel)
        {
            List<Rapor_NilaiEkskulSiswa> hasil = new List<Rapor_NilaiEkskulSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_MAPEL;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
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

        public static List<Rapor_NilaiEkskulSiswa> GetByTABySMByMapelBySiswa_Entity(string tahun_ajaran, string semester, string rel_mapel, string rel_siswa)
        {
            List<Rapor_NilaiEkskulSiswa> hasil = new List<Rapor_NilaiEkskulSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_MAPEL_BY_SISWA;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
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

        public static List<Rapor_NilaiEkskulSiswa> GetByHeaderBySiswa_Entity(string rel_rapor_nilaiekskul, string rel_siswa)
        {
            List<Rapor_NilaiEkskulSiswa> hasil = new List<Rapor_NilaiEkskulSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER_BY_SISWA;
                comm.Parameters.AddWithValue("@Rel_Rapor_NilaiEkskul", rel_rapor_nilaiekskul);
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

        public static Rapor_NilaiEkskulSiswa GetByID_Entity(string kode)
        {
            Rapor_NilaiEkskulSiswa hasil = new Rapor_NilaiEkskulSiswa();
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

        public static void Insert(Rapor_NilaiEkskulSiswa m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_NilaiEkskul, m.Rel_Rapor_NilaiEkskul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai, m.Nilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_HD, m.LTS_HD));
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

        public static void SaveNilaiEkskul(
                string tahun_ajaran,
                string semester,
                string rel_mapel,
                string rel_siswa,
                string nilai,
                string lts_hd,
                string sakit,
                string izin,
                string alpa
            )
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
                comm.CommandText = SP_SAVE_BY_TA_BY_SM_BY_MAPEL_BY_SISWA;

                comm.Parameters.Add(new SqlParameter("@TahunAjaran", tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@Semester", semester));
                comm.Parameters.Add(new SqlParameter("@Rel_Mapel", rel_mapel));
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", rel_siswa));
                comm.Parameters.Add(new SqlParameter("@Nilai", nilai));
                comm.Parameters.Add(new SqlParameter("@LTS_HD", lts_hd));
                comm.Parameters.Add(new SqlParameter("@Sakit", sakit));
                comm.Parameters.Add(new SqlParameter("@Izin", izin));
                comm.Parameters.Add(new SqlParameter("@Alpa", alpa));
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

        public static void SaveLTSHDEkskul(
                string tahun_ajaran,
                string semester,
                string rel_mapel,
                string rel_siswa,
                string nilai
            )
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
                comm.CommandText = SP_SAVE_LTSHD_BY_TA_BY_SM_BY_MAPEL_BY_SISWA;

                comm.Parameters.Add(new SqlParameter("@TahunAjaran", tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@Semester", semester));
                comm.Parameters.Add(new SqlParameter("@Rel_Mapel", rel_mapel));
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", rel_siswa));
                comm.Parameters.Add(new SqlParameter("@Nilai", nilai));
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

        public static void SaveSakitEkskul(
                string tahun_ajaran,
                string semester,
                string rel_mapel,
                string rel_siswa,
                string nilai
            )
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
                comm.CommandText = SP_SAVE_SAKIT_BY_TA_BY_SM_BY_MAPEL_BY_SISWA;

                comm.Parameters.Add(new SqlParameter("@TahunAjaran", tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@Semester", semester));
                comm.Parameters.Add(new SqlParameter("@Rel_Mapel", rel_mapel));
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", rel_siswa));
                comm.Parameters.Add(new SqlParameter("@Nilai", nilai));
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

        public static void SaveIzinEkskul(
               string tahun_ajaran,
               string semester,
               string rel_mapel,
               string rel_siswa,
               string nilai
           )
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
                comm.CommandText = SP_SAVE_IZIN_BY_TA_BY_SM_BY_MAPEL_BY_SISWA;

                comm.Parameters.Add(new SqlParameter("@TahunAjaran", tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@Semester", semester));
                comm.Parameters.Add(new SqlParameter("@Rel_Mapel", rel_mapel));
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", rel_siswa));
                comm.Parameters.Add(new SqlParameter("@Nilai", nilai));
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

        public static void SaveAlpaEkskul(
               string tahun_ajaran,
               string semester,
               string rel_mapel,
               string rel_siswa,
               string nilai
           )
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
                comm.CommandText = SP_SAVE_ALPA_BY_TA_BY_SM_BY_MAPEL_BY_SISWA;

                comm.Parameters.Add(new SqlParameter("@TahunAjaran", tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@Semester", semester));
                comm.Parameters.Add(new SqlParameter("@Rel_Mapel", rel_mapel));
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", rel_siswa));
                comm.Parameters.Add(new SqlParameter("@Nilai", nilai));
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

        public static void Update(Rapor_NilaiEkskulSiswa m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_NilaiEkskul, m.Rel_Rapor_NilaiEkskul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nilai, m.Nilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_HD, m.LTS_HD));
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
    }
}