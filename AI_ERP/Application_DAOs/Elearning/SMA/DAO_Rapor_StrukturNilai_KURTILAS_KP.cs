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
    public static class DAO_Rapor_StrukturNilai_KURTILAS_KP
    {
        public const string SP_SELECT_ALL = "SMA_Rapor_StrukturNilai_KURTILAS_KP_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SMA_Rapor_StrukturNilai_KURTILAS_KP_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_HEADER = "SMA_Rapor_StrukturNilai_KURTILAS_KP_SELECT_ALL_BY_HEADER";
        public const string SP_SELECT_ALL_BY_TA_BY_SM = "SMA_Rapor_StrukturNilai_KURTILAS_KP_SELECT_ALL_BY_TA_BY_SM";
        public const string SP_SELECT_ALL_BY_TA_BY_SM_BY_KELAS = "SMA_Rapor_StrukturNilai_KURTILAS_KP_SELECT_ALL_BY_TA_BY_SM_BY_KELAS";
        public const string SP_SELECT_BY_ID = "SMA_Rapor_StrukturNilai_KURTILAS_KP_SELECT_BY_ID";

        public const string SP_INSERT = "SMA_Rapor_StrukturNilai_KURTILAS_KP_INSERT";
        public const string SP_INSERT_LENGKAP = "SMA_Rapor_StrukturNilai_KURTILAS_KP_INSERT_LENGKAP";

        public const string SP_UPDATE = "SMA_Rapor_StrukturNilai_KURTILAS_KP_UPDATE";
        public const string SP_UPDATE_DESKRIPSI = "SMA_Rapor_StrukturNilai_KURTILAS_KP_UPDATE_DESKRIPSI";
        public const string SP_UPDATE_DESKRIPSI_ITEM_KP = "SMA_Rapor_StrukturNilai_KURTILAS_KP_UPDATE_DESKRIPSI_ITEM_KP";

        public const string SP_DELETE = "SMA_Rapor_StrukturNilai_KURTILAS_KP_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_StrukturNilai_KD = "Rel_Rapor_StrukturNilai_KD";
            public const string Rel_Rapor_KomponenPenilaian = "Rel_Rapor_KomponenPenilaian";
            public const string BobotNK = "BobotNK";
            public const string Jenis = "Jenis";
            public const string Urutan = "Urutan";
            public const string DeskripsiRapor = "DeskripsiRapor";
            public const string Deskripsi = "Deskripsi";
            public const string IsKomponenRapor = "IsKomponenRapor";
        }

        private static Rapor_StrukturNilai_KURTILAS_KP GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_StrukturNilai_KURTILAS_KP
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_StrukturNilai_KD = new Guid(row[NamaField.Rel_Rapor_StrukturNilai_KD].ToString()),
                Rel_Rapor_KomponenPenilaian = new Guid(row[NamaField.Rel_Rapor_KomponenPenilaian].ToString()),
                BobotNK = Convert.ToDecimal(row[NamaField.BobotNK]),
                Jenis = row[NamaField.Jenis].ToString(),
                Urutan = Convert.ToInt16(row[NamaField.Urutan]),
                DeskripsiRapor = row[NamaField.DeskripsiRapor].ToString(),
                Deskripsi = row[NamaField.Deskripsi].ToString(),
                IsKomponenRapor = Convert.ToBoolean(row[NamaField.IsKomponenRapor] == DBNull.Value ? false : row[NamaField.IsKomponenRapor])
            };
        }

        public static List<Rapor_StrukturNilai_KURTILAS_KP> GetAll_Entity()
        {
            List<Rapor_StrukturNilai_KURTILAS_KP> hasil = new List<Rapor_StrukturNilai_KURTILAS_KP>();
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

        public static List<Rapor_StrukturNilai_KURTILAS_KP> GetAllByHeader_Entity(string rel_rapor_strukturnilai_kd)
        {
            List<Rapor_StrukturNilai_KURTILAS_KP> hasil = new List<Rapor_StrukturNilai_KURTILAS_KP>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_StrukturNilai_KD, rel_rapor_strukturnilai_kd);

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

        public static List<Rapor_StrukturNilai_KURTILAS_KP> GetAllByTABySM_Entity(string tahun_ajaran, string semester)
        {
            List<Rapor_StrukturNilai_KURTILAS_KP> hasil = new List<Rapor_StrukturNilai_KURTILAS_KP>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_BY_TA_BY_SM;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);

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

        public static List<Rapor_StrukturNilai_KURTILAS_KP> GetAllByTABySMByKelas_Entity(string tahun_ajaran, string semester, string rel_kelas)
        {
            List<Rapor_StrukturNilai_KURTILAS_KP> hasil = new List<Rapor_StrukturNilai_KURTILAS_KP>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_BY_TA_BY_SM_BY_KELAS;
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

        public static Rapor_StrukturNilai_KURTILAS_KP GetByID_Entity(string kode)
        {
            Rapor_StrukturNilai_KURTILAS_KP hasil = new Rapor_StrukturNilai_KURTILAS_KP();
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

        public static void Insert(Rapor_StrukturNilai_KURTILAS_KP m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KD, m.Rel_Rapor_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KomponenPenilaian, m.Rel_Rapor_KomponenPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotNK, m.BobotNK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, m.Jenis));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKomponenRapor, m.IsKomponenRapor));
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

        public static void InsertLengkap(Rapor_StrukturNilai_KURTILAS_KP m, string user_id)
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
                comm.CommandText = SP_INSERT_LENGKAP;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KD, m.Rel_Rapor_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KomponenPenilaian, m.Rel_Rapor_KomponenPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotNK, m.BobotNK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, m.Jenis));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKomponenRapor, m.IsKomponenRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, m.Deskripsi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiRapor, m.DeskripsiRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urutan, m.Urutan));
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

        public static void Update(Rapor_StrukturNilai_KURTILAS_KP m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KD, m.Rel_Rapor_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KomponenPenilaian, m.Rel_Rapor_KomponenPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotNK, m.BobotNK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, m.Jenis));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKomponenRapor, m.IsKomponenRapor));
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

        public static void UpdateDeskripsiRapor(Guid kode, string deskripsi_rapor, string user_id)
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
                comm.CommandText = SP_UPDATE_DESKRIPSI;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiRapor, deskripsi_rapor));
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

        public static void UpdateDeskripsi(Guid kode, string deskripsi, string user_id)
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
                comm.CommandText = SP_UPDATE_DESKRIPSI_ITEM_KP;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, deskripsi));
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