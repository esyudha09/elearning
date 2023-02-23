using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.KB;

namespace AI_ERP.Application_DAOs.Elearning.KB
{
    public static class DAO_Rapor_NilaiSiswa
    {
        public const string SP_SELECT_ALL = "KB_Rapor_NilaiSiswa_SELECT_ALL";
        public const string SP_SELECT_BY_HEADER = "KB_Rapor_NilaiSiswa_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_ID = "KB_Rapor_NilaiSiswa_SELECT_BY_ID";

        public const string SP_INSERT = "KB_Rapor_NilaiSiswa_INSERT";

        public const string SP_UPDATE = "KB_Rapor_NilaiSiswa_UPDATE";
        public const string SP_UPDATE_POSTED_BY_KELASDET_BY_TAHUNAJARAN_BY_SEMESTER = "KB_Rapor_NilaiSiswa_UPDATE_POSTED_BY_KELASDET_BY_TAHUNAJARAN_BY_SEMESTER";

        public const string SP_DELETE = "KB_Rapor_NilaiSiswa_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_Nilai = "Rel_Rapor_Nilai";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string BeratBadan = "BeratBadan";
            public const string TinggiBadan = "TinggiBadan";
            public const string LingkarKepala = "LingkarKepala";
            public const string IsPosted = "IsPosted";
            public const string IsLocked = "IsLocked";
            public const string Usia = "Usia";
        }

        private static Rapor_NilaiSiswa GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_NilaiSiswa
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_Nilai = new Guid(row[NamaField.Rel_Rapor_Nilai].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                BeratBadan = row[NamaField.BeratBadan].ToString(),
                TinggiBadan = row[NamaField.TinggiBadan].ToString(),
                LingkarKepala = row[NamaField.LingkarKepala].ToString(),
                Usia = row[NamaField.Usia].ToString(),
                IsPosted = (row[NamaField.IsPosted] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsPosted])),
                IsLocked = (row[NamaField.IsLocked] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsLocked]))
            };
        }

        public static List<Rapor_NilaiSiswa> GetAll_Entity()
        {
            List<Rapor_NilaiSiswa> hasil = new List<Rapor_NilaiSiswa>();
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

        public static List<Rapor_NilaiSiswa> GetByHeader_Entity(string rel_rapornilai)
        {
            List<Rapor_NilaiSiswa> hasil = new List<Rapor_NilaiSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (rel_rapornilai.Trim() == "") return hasil;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_Nilai, rel_rapornilai);

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

        public static Rapor_NilaiSiswa GetByID_Entity(string kode)
        {
            Rapor_NilaiSiswa hasil = new Rapor_NilaiSiswa();
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

        public static void Insert(Rapor_NilaiSiswa m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BeratBadan, m.BeratBadan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TinggiBadan, m.TinggiBadan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LingkarKepala, m.LingkarKepala));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsPosted, m.IsPosted));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsLocked, m.IsLocked));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Usia, m.Usia));
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

        public static void Update(Rapor_NilaiSiswa m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BeratBadan, m.BeratBadan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TinggiBadan, m.TinggiBadan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LingkarKepala, m.LingkarKepala));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Usia, m.Usia));
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

        public static void UpdatePostedByKelasByTahunAjaranBySemester
            (string rel_kelas, string tahun_ajaran, string semester, bool is_posted)
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
                comm.CommandText = SP_UPDATE_POSTED_BY_KELASDET_BY_TAHUNAJARAN_BY_SEMESTER;

                comm.Parameters.Add(new SqlParameter("@Rel_KelasDet", rel_kelas));
                comm.Parameters.Add(new SqlParameter("@TahunAjaran", tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@Semester", semester));
                comm.Parameters.Add(new SqlParameter("@IsPosted", is_posted));
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