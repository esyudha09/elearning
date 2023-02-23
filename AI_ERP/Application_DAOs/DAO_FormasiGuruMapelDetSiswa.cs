using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;

namespace AI_ERP.Application_DAOs
{
    public static class DAO_FormasiGuruMapelDetSiswa
    {
        public const string SP_SELECT_ALL = "FormasiGuruMapelDetSiswa_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "FormasiGuruMapelDetSiswa_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "FormasiGuruMapelDetSiswa_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_HEADER_ORDER_BY_JENJANG_BY_KELAS = "FormasiGuruMapelDetSiswa_SELECT_BY_HEADER_ORDER_BY_JENJANG_BY_KELAS";
        public const string SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL = "FormasiGuruMapelDetSiswa_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL";
        public const string SP_SELECT_DISTINCT_KELAS_BY_HEADER_ORDER_BY_JENJANG_BY_KELAS = "FormasiGuruMapelDetSiswa_SELECT_DISTINCT_KELAS_BY_HEADER_ORDER_BY_JENJANG_BY_KELAS";
        public const string SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER = "FormasiGuruMapelDetSiswa_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER";
        public const string SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL_BY_KELAS = "FormasiGuruMapelDetSiswa_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL_BY_KELAS";
        public const string SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL_BY_KELAS_FOR_SEARCH = "FormasiGuruMapelDetSiswa_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL_BY_KELAS_FOR_SEARCH";
        public const string SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL_BY_KELAS_BY_KELAS_DET = "FormasiGuruMapelDetSiswa_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL_BY_KELAS_BY_KELAS_DET";        
        public const string SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL_BY_KELAS_BY_KELAS_DET_FOR_SEARCH = "FormasiGuruMapelDetSiswa_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL_BY_KELAS_BY_KELAS_DET_FOR_SEARCH";

        public const string SP_INSERT = "FormasiGuruMapelDetSiswa_INSERT";

        public const string SP_UPDATE = "FormasiGuruMapelDetSiswa_UPDATE";

        public const string SP_DELETE = "FormasiGuruMapelDetSiswa_DELETE";
        public const string SP_DELETE_BY_HEADER = "FormasiGuruMapelDetSiswa_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_FormasiGuruMapel = "Rel_FormasiGuruMapel";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Urutan = "Urutan";
        }

        public static FormasiGuruMapelDetSiswa GetEntityFromDataRow(DataRow row)
        {
            return new FormasiGuruMapelDetSiswa
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_FormasiGuruMapel = new Guid(row[NamaField.Rel_FormasiGuruMapel].ToString()),
                Urutan = Application_Libs.Libs.GetStringToInteger(row[NamaField.Urutan].ToString())
            };
        }

        public static List<FormasiGuruMapelDetSiswa> GetAll_Entity()
        {
            List<FormasiGuruMapelDetSiswa> hasil = new List<FormasiGuruMapelDetSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
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

        public static FormasiGuruMapelDetSiswa GetByID_Entity(string kode)
        {
            FormasiGuruMapelDetSiswa hasil = new FormasiGuruMapelDetSiswa();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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

        public static List<FormasiGuruMapelDetSiswa> GetByTAByMapelBySM_Entity(string rel_mapel, string tahun_ajaran, string semester)
        {
            List<FormasiGuruMapelDetSiswa> hasil = new List<FormasiGuruMapelDetSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL;
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

        public static List<Siswa> GetSiswaByTABySMByMapelByKelasByKelasDet_Entity(string tahun_ajaran, string semester, string rel_mapel, string rel_kelas, string rel_kelas_det)
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL_BY_KELAS_BY_KELAS_DET;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelas_det);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(DAO_Siswa.GetEntityFromDataRow(row));
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

        public static List<DAO_Siswa.SiswaByFormasiMapel> GetSiswaByTABySMBy_Entity(string tahun_ajaran, string semester)
        {
            List<DAO_Siswa.SiswaByFormasiMapel> hasil = new List<DAO_Siswa.SiswaByFormasiMapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(new DAO_Siswa.SiswaByFormasiMapel {
                        Kode = new Guid(row["Kode"].ToString()),
                        Nama = row["Nama"].ToString(),
                        NISN = row["NISN"].ToString(),
                        NISSekolah = row["NISSekolah"].ToString(),
                        JenisKelamin = row["JenisKelamin"].ToString(),
                        JenisKelas = row["JenisKelas"].ToString(),
                        Rel_Kelas = row["Rel_Kelas"].ToString(),
                        Rel_KelasDet = row["Rel_KelasDet"].ToString(),
                        Rel_KelasDetJurusan = row["Rel_KelasDetJurusan"].ToString(),
                        Rel_KelasDetSosialisasi = row["Rel_KelasDetSosialisasi"].ToString(),
                        Rel_Mapel = row["Rel_Mapel"].ToString()
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

        public static List<Siswa> GetSiswaByTABySMByMapelByKelas_Entity(string tahun_ajaran, string semester, string rel_mapel, string rel_kelas)
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL_BY_KELAS;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(DAO_Siswa.GetEntityFromDataRow(row));
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

        public static void DeleteByHeader(string Kode_header, string user_id)
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
                comm.CommandText = SP_DELETE_BY_HEADER;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_FormasiGuruMapel, Kode_header));
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

        public static void Insert(FormasiGuruMapelDetSiswa m, string user_id)
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
                comm.CommandText = SP_INSERT;

                if (m.Kode.ToString() == Application_Libs.Constantas.GUID_NOL) m.Kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_FormasiGuruMapel, m.Rel_FormasiGuruMapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
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

        public static void Update(FormasiGuruMapelDetSiswa m, string user_id)
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
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_FormasiGuruMapel, m.Rel_FormasiGuruMapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
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

        public static List<FormasiGuruMapelDetSiswa> GetByHeader_Entity(string kode)
        {
            List<FormasiGuruMapelDetSiswa> hasil = new List<FormasiGuruMapelDetSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@Rel_FormasiGuruMapel", kode);

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

        public static List<FormasiGuruMapelDetSiswa> GetByHeaderOrderByJenjangByKelas_Entity(string kode)
        {
            List<FormasiGuruMapelDetSiswa> hasil = new List<FormasiGuruMapelDetSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER_ORDER_BY_JENJANG_BY_KELAS;
                comm.Parameters.AddWithValue("@Rel_FormasiGuruMapel", kode);

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

        public static List<string> GetByDistinctKelasDetHeader_Entity(string kode)
        {
            List<string> hasil = new List<string>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_KELAS_BY_HEADER_ORDER_BY_JENJANG_BY_KELAS;
                comm.Parameters.AddWithValue("@Rel_FormasiGuruMapel", kode);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(row["Rel_KelasDet"].ToString());
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