using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.TK;

namespace AI_ERP.Application_DAOs.Elearning.TK
{
    public static class DAO_Rapor_NilaiSiswa
    {
        public const string SP_SELECT_ALL = "TK_Rapor_NilaiSiswa_SELECT_ALL";
        public const string SP_SELECT_BY_HEADER = "TK_Rapor_NilaiSiswa_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_ID = "TK_Rapor_NilaiSiswa_SELECT_BY_ID";
        public const string SP_SELECT_EKSKUL = "TK_Rapor_NilaiSiswa_SELECT_EKSKUL";

        public const string SP_INSERT = "TK_Rapor_NilaiSiswa_INSERT";

        public const string SP_UPDATE = "TK_Rapor_NilaiSiswa_UPDATE";
        public const string SP_UPDATE_POSTED_BY_KELASDET_BY_TAHUNAJARAN_BY_SEMESTER = "TK_Rapor_NilaiSiswa_UPDATE_POSTED_BY_KELASDET_BY_TAHUNAJARAN_BY_SEMESTER";

        public const string SP_DELETE = "TK_Rapor_NilaiSiswa_DELETE";

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
            public const string Mapel = "Mapel";

            public const string LTS_CK_KEHADIRAN = "LTS_CK_KEHADIRAN";
            public const string LTS_CK_KETEPATAN_WKT = "LTS_CK_KETEPATAN_WKT";
            public const string LTS_CK_PENGGUNAAN_SRGM = "LTS_CK_PENGGUNAAN_SRGM";
            public const string LTS_CK_PENGGUNAAN_KMR = "LTS_CK_PENGGUNAAN_KMR";

            public const string SM_CK_KEHADIRAN = "SM_CK_KEHADIRAN";
            public const string SM_CK_KETEPATAN_WKT = "SM_CK_KETEPATAN_WKT";
            public const string SM_CK_PENGGUNAAN_SRGM = "SM_CK_PENGGUNAAN_SRGM";
            public const string SM_CK_PENGGUNAAN_KMR = "SM_CK_PENGGUNAAN_KMR";
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
                IsPosted = (row[NamaField.IsPosted] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsPosted])),
                IsLocked = (row[NamaField.IsLocked] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsLocked])),

                LTS_CK_KEHADIRAN = row[NamaField.LTS_CK_KEHADIRAN].ToString(),
                LTS_CK_KETEPATAN_WKT = row[NamaField.LTS_CK_KETEPATAN_WKT].ToString(),
                LTS_CK_PENGGUNAAN_SRGM = row[NamaField.LTS_CK_PENGGUNAAN_SRGM].ToString(),
                LTS_CK_PENGGUNAAN_KMR = row[NamaField.LTS_CK_PENGGUNAAN_KMR].ToString(),

                SM_CK_KEHADIRAN = row[NamaField.SM_CK_KEHADIRAN].ToString(),
                SM_CK_KETEPATAN_WKT = row[NamaField.SM_CK_KETEPATAN_WKT].ToString(),
                SM_CK_PENGGUNAAN_SRGM = row[NamaField.SM_CK_PENGGUNAAN_SRGM].ToString(),
                SM_CK_PENGGUNAAN_KMR = row[NamaField.SM_CK_PENGGUNAAN_KMR].ToString()
            };
        }

        private static RaporEkskulSiswa GetEntityFromDataRowEKskul(DataRow row)
        {
            return new RaporEkskulSiswa
            {
                Mapel = row[NamaField.Mapel].ToString(),
                LTS_CK_KEHADIRAN = row[NamaField.LTS_CK_KEHADIRAN].ToString()
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
        
        public static List<RaporEkskulSiswa> GetEkskul_Entity(string tahun_ajaran, string semester, string rel_kelasdet, string rel_siswa)
        {
            List<RaporEkskulSiswa> hasil = new List<RaporEkskulSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_EKSKUL;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(new RaporEkskulSiswa {
                        Mapel = row["Mapel"].ToString(),
                        LTS_CK_KEHADIRAN = row["LTS_CK_KEHADIRAN"].ToString()
                    });
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_KEHADIRAN, (m.LTS_CK_KEHADIRAN == null ? "" : m.LTS_CK_KEHADIRAN)));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_KETEPATAN_WKT, (m.LTS_CK_KETEPATAN_WKT == null ? "" : m.LTS_CK_KETEPATAN_WKT)));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_PENGGUNAAN_SRGM, (m.LTS_CK_PENGGUNAAN_SRGM == null ? "" : m.LTS_CK_PENGGUNAAN_SRGM)));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_PENGGUNAAN_KMR, (m.LTS_CK_PENGGUNAAN_KMR == null ? "" : m.LTS_CK_PENGGUNAAN_KMR)));

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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LingkarKepala, m.LingkarKepala));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TinggiBadan, m.TinggiBadan));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_KEHADIRAN, (m.LTS_CK_KEHADIRAN == null ? "" : m.LTS_CK_KEHADIRAN)));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_KETEPATAN_WKT, (m.LTS_CK_KETEPATAN_WKT == null ? "" : m.LTS_CK_KETEPATAN_WKT)));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_PENGGUNAAN_SRGM, (m.LTS_CK_PENGGUNAAN_SRGM == null ? "" : m.LTS_CK_PENGGUNAAN_SRGM)));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_PENGGUNAAN_KMR, (m.LTS_CK_PENGGUNAAN_KMR == null ? "" : m.LTS_CK_PENGGUNAAN_KMR)));

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