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
    public static class DAO_Rapor_DesignDetEkskul
    {
        public const string SP_SELECT_ALL = "KB_Rapor_DesignDetEkskul_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "KB_Rapor_DesignDetEkskul_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "KB_Rapor_DesignDetEkskul_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_HEADER_BY_SISWA = "KB_Rapor_DesignDetEkskul_SELECT_BY_HEADER_BY_SISWA";
        public const string SP_SELECT_BY_HEADER_BY_SISWA_FOR_DESIGN = "KB_Rapor_DesignDetEkskul_SELECT_BY_HEADER_BY_SISWA_FOR_DESIGN";
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_BY_KELAS_BY_MAPEL = "KB_Rapor_DesignDetEkskul_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_BY_KELAS_BY_MAPEL";
        public const string SP_SELECT_BY_RAPOR_DESIGN_BY_MAPEL = "KB_Rapor_DesignDetEkskul_SELECT_BY_RAPOR_DESIGN_BY_MAPEL";

        public const string SP_INSERT = "KB_Rapor_DesignDetEkskul_INSERT";

        public const string SP_UPDATE = "KB_Rapor_DesignDetEkskul_UPDATE";
        public const string SP_UPDATE_URUT = "KB_Rapor_DesignDetEkskul_UPDATE_URUT";

        public const string SP_DELETE = "KB_Rapor_DesignDetEkskul_DELETE";
        public const string SP_DELETE_BY_HEADER = "KB_Rapor_DesignDetEkskul_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_Design = "Rel_Rapor_Design";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Poin = "Poin";
            public const string Rel_KomponenRapor = "Rel_KomponenRapor";
            public const string NamaKomponen = "NamaKomponen";
            public const string JenisKomponen = "JenisKomponen";
            public const string Urut = "Urut";
        }

        private static Rapor_DesignDetEkskul GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_DesignDetEkskul
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_Design = new Guid(row[NamaField.Rel_Rapor_Design].ToString()),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Poin = row[NamaField.Poin].ToString(),
                Rel_KomponenRapor = new Guid(row[NamaField.Rel_KomponenRapor].ToString()),
                JenisKomponen = (JenisKomponenRapor)Convert.ToInt16(row[NamaField.JenisKomponen]),
                NamaKomponen = row[NamaField.NamaKomponen].ToString(),
                Urut = Convert.ToInt16(row[NamaField.Urut])
            };
        }

        public static List<Rapor_DesignDetEkskul> GetAll_Entity()
        {
            List<Rapor_DesignDetEkskul> hasil = new List<Rapor_DesignDetEkskul>();
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

        public static List<Rapor_DesignDetEkskul> GetByHeader_Entity(string Rel_Rapor_Design)
        {
            List<Rapor_DesignDetEkskul> hasil = new List<Rapor_DesignDetEkskul>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_Design, Rel_Rapor_Design);

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

        public static List<Rapor_DesignDetEkskul> GetByHeaderBySiswaForDesign_Entity(string rel_rapor_design, string rel_siswa)
        {
            List<Rapor_DesignDetEkskul> hasil = new List<Rapor_DesignDetEkskul>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER_BY_SISWA_FOR_DESIGN;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_Design, rel_rapor_design);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Siswa, rel_siswa);

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

        public static List<Rapor_DesignDetEkskul> GetByTABySMByKelasByMapel_Entity(string tahun_ajaran, string semester, string rel_kelas, string rel_mapel)
        {
            List<Rapor_DesignDetEkskul> hasil = new List<Rapor_DesignDetEkskul>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_BY_KELAS_BY_MAPEL;
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

        public static List<Rapor_DesignDetEkskul> GetByTAByRaporDesignByMapel_Entity(string rel_rapor_design, string rel_mapel)
        {
            List<Rapor_DesignDetEkskul> hasil = new List<Rapor_DesignDetEkskul>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_RAPOR_DESIGN_BY_MAPEL;
                comm.Parameters.AddWithValue("@Rel_Rapor_Design", rel_rapor_design);
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

        public static Rapor_DesignDetEkskul GetByID_Entity(string kode)
        {
            Rapor_DesignDetEkskul hasil = new Rapor_DesignDetEkskul();
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

        public static void DeleteByRaporDesign(string Rel_Rapor_Design, string user_id)
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
                comm.CommandText = SP_DELETE_BY_HEADER;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Design, Rel_Rapor_Design));
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

        public static void Insert(Rapor_DesignDetEkskul m, string user_id)
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
                if (m.Kode != new Guid(Application_Libs.Constantas.GUID_NOL))
                {
                    kode = m.Kode;
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Design, m.Rel_Rapor_Design));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Poin, m.Poin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KomponenRapor, m.Rel_KomponenRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKomponen, (int)m.JenisKomponen));
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

        public static void Update(Rapor_DesignDetEkskul m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Design, m.Rel_Rapor_Design));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Poin, m.Poin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KomponenRapor, m.Rel_KomponenRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKomponen, (int)m.JenisKomponen));
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

        public static void UpdateUrut(string kode, int urut, string user_id)
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
                comm.CommandText = SP_UPDATE_URUT;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urut, urut));
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
    }
}