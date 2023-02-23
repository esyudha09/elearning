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
    public static class DAO_Rapor_StrukturNilai_KP
    {
        public const string SP_SELECT_ALL = "SMP_Rapor_StrukturNilai_KP_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SMP_Rapor_StrukturNilai_KP_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_HEADER = "SMP_Rapor_StrukturNilai_KP_SELECT_ALL_BY_HEADER";
        public const string SP_SELECT_ALL_BY_TA_BY_SM_BY_KELAS = "SMP_Rapor_StrukturNilai_KP_SELECT_ALL_BY_TA_BY_SM_BY_KELAS";
        public const string SP_SELECT_BY_ID = "SMP_Rapor_StrukturNilai_KP_SELECT_BY_ID";

        public const string SP_INSERT = "SMP_Rapor_StrukturNilai_KP_INSERT";
        public const string SP_INSERT_LENGKAP = "SMP_Rapor_StrukturNilai_KP_INSERT_LENGKAP";

        public const string SP_UPDATE = "SMP_Rapor_StrukturNilai_KP_UPDATE";

        public const string SP_DELETE = "SMP_Rapor_StrukturNilai_KP_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_StrukturNilai_KD = "Rel_Rapor_StrukturNilai_KD";
            public const string Rel_Rapor_KomponenPenilaian = "Rel_Rapor_KomponenPenilaian";
            public const string BobotNK = "BobotNK";
            public const string Jenis = "Jenis";
            public const string Urutan = "Urutan";
            public const string IsAdaPB = "IsAdaPB";
            public const string IsLTS = "IsLTS";
            public const string Materi = "Materi";
            public const string Deskripsi = "Deskripsi";
            public const string KodeKD = "KodeKD";
        }

        private static Rapor_StrukturNilai_KP GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_StrukturNilai_KP
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_StrukturNilai_KD = new Guid(row[NamaField.Rel_Rapor_StrukturNilai_KD].ToString()),
                Rel_Rapor_KomponenPenilaian = new Guid(row[NamaField.Rel_Rapor_KomponenPenilaian].ToString()),
                BobotNK = Convert.ToDecimal(row[NamaField.BobotNK]),
                Jenis = row[NamaField.Jenis].ToString(),
                Urutan = Convert.ToInt16(row[NamaField.Urutan]),
                IsLTS = (row[NamaField.IsLTS] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsLTS])),
                IsAdaPB = (row[NamaField.IsAdaPB] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsAdaPB])),
                Materi = row[NamaField.Materi].ToString(),
                Deskripsi = row[NamaField.Deskripsi].ToString(),
                KodeKD = row[NamaField.KodeKD].ToString()
            };
        }

        public static List<Rapor_StrukturNilai_KP> GetAll_Entity()
        {
            List<Rapor_StrukturNilai_KP> hasil = new List<Rapor_StrukturNilai_KP>();
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

        public static List<Rapor_StrukturNilai_KP> GetAllByHeader_Entity(string rel_rapor_strukturnilai_kd)
        {
            List<Rapor_StrukturNilai_KP> hasil = new List<Rapor_StrukturNilai_KP>();
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

        public static List<Rapor_StrukturNilai_KP> GetAllByTABySMByKelas_Entity(string tahun_ajaran, string semester, string rel_kelas)
        {
            List<Rapor_StrukturNilai_KP> hasil = new List<Rapor_StrukturNilai_KP>();
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

        public static Rapor_StrukturNilai_KP GetByID_Entity(string kode)
        {
            Rapor_StrukturNilai_KP hasil = new Rapor_StrukturNilai_KP();
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

        public static void Insert(Rapor_StrukturNilai_KP m, string user_id)
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

                if (m.Kode.ToString() == Application_Libs.Constantas.GUID_NOL) m.Kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KD, m.Rel_Rapor_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KomponenPenilaian, m.Rel_Rapor_KomponenPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotNK, m.BobotNK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, m.Jenis));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsAdaPB, m.IsAdaPB));
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

        public static void InsertLengkap(Rapor_StrukturNilai_KP m, string user_id)
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

                if (m.Kode.ToString() == Application_Libs.Constantas.GUID_NOL) m.Kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KD, m.Rel_Rapor_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KomponenPenilaian, m.Rel_Rapor_KomponenPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotNK, m.BobotNK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, m.Jenis));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Materi, m.Materi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KodeKD, m.KodeKD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, m.Deskripsi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsAdaPB, m.IsAdaPB));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsLTS, m.IsLTS));
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

        public static void Update(Rapor_StrukturNilai_KP m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsAdaPB, m.IsAdaPB));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsLTS, m.IsLTS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Materi, m.Materi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, m.Deskripsi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KodeKD, m.KodeKD));
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