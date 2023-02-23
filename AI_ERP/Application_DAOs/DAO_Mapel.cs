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
    public static class DAO_Mapel
    {
        public const string SP_SELECT_ALL = "Mapel_SELECT_ALL";
        public const string SP_SELECT_ALL_BY_UNIT = "Mapel_SELECT_ALL_BY_UNIT";
        public const string SP_SELECT_ALL_FOR_SEARCH = "Mapel_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_UNIT_FOR_SEARCH = "Mapel_SELECT_ALL_BY_UNIT_FOR_SEARCH";
        public const string SP_SELECT_BY_SEKOLAH = "Mapel_SELECT_BY_SEKOLAH";
        public const string SP_SELECT_BY_STRUKTUR_NILAI = "Mapel_SELECT_BY_STRUKTUR_NILAI_SD";
        public const string SP_SELECT_BY_GURU = "Mapel_SELECT_BY_GURU";       
        public const string SP_SELECT_BY_ID = "Mapel_SELECT_BY_ID";
        public const string SP_SELECT_DISTINCT_ABSEN_BY_TA_BY_UNIT_BY_SEMESTER = "Mapel_SELECT_DISTINCT_ABSEN_BY_TA_BY_UNIT_BY_SEMESTER";

        public const string SP_INSERT = "Mapel_INSERT";

        public const string SP_UPDATE = "Mapel_UPDATE";

        public const string SP_DELETE = "Mapel_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Nama = "Nama";
            public const string Alias = "Alias";
            public const string Jenis = "Jenis";
            public const string Keterangan = "Keterangan";
            public const string Rel_Sekolah = "Rel_Sekolah";
        }

        public static Mapel GetEntityFromDataRow(DataRow row)
        {
            return new Mapel
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Nama = row[NamaField.Nama].ToString(),
                Alias = row[NamaField.Alias].ToString(),
                Jenis = DAO_Mapel.GetJenisMapelByJenis(row[NamaField.Jenis].ToString()), //row[NamaField.Jenis].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),                
                Rel_Sekolah = row[NamaField.Rel_Sekolah].ToString()
            };
        }

        public static List<Mapel> GetAll_Entity()
        {
            List<Mapel> hasil = new List<Mapel>();
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

        public static List<Mapel> GetAllBySekolah_Entity(string rel_sekolah)
        {
            List<Mapel> hasil = new List<Mapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_SEKOLAH;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);

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

        public static List<Mapel> GetDistinctByTABySMBySekolah_Entity(string tahun_ajaran, string semester, string rel_sekolah)
        {
            List<Mapel> hasil = new List<Mapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_ABSEN_BY_TA_BY_UNIT_BY_SEMESTER;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);

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
        

        public static List<Mapel> GetAllByStrukturNilai_Entity(string tahun_ajaran, string semester, string rel_kelas)
        {
            List<Mapel> hasil = new List<Mapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_STRUKTUR_NILAI;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);

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

        public static List<Mapel> GetAllByGuru_Entity(string rel_guru, string rel_sekolah)
        {
            List<Mapel> hasil = new List<Mapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_GURU;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@Rel_Sekolah", rel_sekolah);

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

        public static Mapel GetByID_Entity(string kode)
        {
            Mapel hasil = new Mapel();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            if (kode.Length <= 10) return hasil;
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
        public static void Insert(Mapel mapel, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, Guid.NewGuid()));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, mapel.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alias, mapel.Alias));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, mapel.Jenis));                
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, mapel.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, mapel.Rel_Sekolah));
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

        public static void Update(Mapel mapel, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, mapel.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, mapel.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alias, mapel.Alias));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, mapel.Jenis));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, mapel.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, mapel.Rel_Sekolah));
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

        public static string GetJenisMapel(string kode)
        {
            Mapel m = DAO_Mapel.GetByID_Entity(kode);

            if (m != null)
            {
                if (m.Nama != null)
                {
                    return GetJenisMapelByJenis(m.Jenis);

                }
            }

            return "";
        }

        public static string GetJenisMapelByJenis(string jenis)
        {
            if (jenis == Application_Libs.Libs.JENIS_MAPEL.KHUSUS)
                return Application_Libs.Libs.JENIS_MAPEL.KHUSUS;

            if (jenis == Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER || jenis == Application_Libs.Libs.JENIS_MAPEL.EKSKUL)
                return Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER;

            if (jenis == Application_Libs.Libs.JENIS_MAPEL.WAJIB)
                return Application_Libs.Libs.JENIS_MAPEL.WAJIB;

            if (jenis == Application_Libs.Libs.JENIS_MAPEL.WAJIB_A)
                return Application_Libs.Libs.JENIS_MAPEL.WAJIB_A;

            if (jenis == Application_Libs.Libs.JENIS_MAPEL.WAJIB_B)
                return Application_Libs.Libs.JENIS_MAPEL.WAJIB_B;

            if (jenis == Application_Libs.Libs.JENIS_MAPEL.WAJIB_B_PILIHAN)
                return Application_Libs.Libs.JENIS_MAPEL.WAJIB_B_PILIHAN;

            if (jenis == Application_Libs.Libs.JENIS_MAPEL.PEMINATAN)
                return Application_Libs.Libs.JENIS_MAPEL.PEMINATAN;

            if (jenis == Application_Libs.Libs.JENIS_MAPEL.LINTAS_MINAT)
                return Application_Libs.Libs.JENIS_MAPEL.LINTAS_MINAT;

            if (jenis == Application_Libs.Libs.JENIS_MAPEL.SIKAP)
                return Application_Libs.Libs.JENIS_MAPEL.SIKAP;

            if (jenis == Application_Libs.Libs.JENIS_MAPEL.VOLUNTEER)
                return Application_Libs.Libs.JENIS_MAPEL.VOLUNTEER;

            if (jenis == Application_Libs.Libs.JENIS_MAPEL.PILIHAN)
                return Application_Libs.Libs.JENIS_MAPEL.PILIHAN;

            return "";
        }
    }
}