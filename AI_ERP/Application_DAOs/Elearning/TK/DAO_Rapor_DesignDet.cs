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
    public static class DAO_Rapor_DesignDet
    {
        public const string SP_SELECT_ALL = "TK_Rapor_DesignDet_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "TK_Rapor_DesignDet_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "TK_Rapor_DesignDet_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_HEADER_DENGAN_NILAI = "TK_Rapor_DesignDet_SELECT_BY_HEADER_DENGAN_NILAI";

        public const string SP_INSERT = "TK_Rapor_DesignDet_INSERT";
        public const string SP_INSERT_LENGKAP = "TK_Rapor_DesignDet_INSERT_LENGKAP";

        public const string SP_UPDATE = "TK_Rapor_DesignDet_UPDATE";
        public const string SP_UPDATE_URUT = "TK_Rapor_DesignDet_UPDATE_URUT";
        public const string SP_UPDATE_PENGATURAN = "TK_Rapor_DesignDet_UPDATE_PENGATURAN";        

        public const string SP_DELETE = "TK_Rapor_DesignDet_DELETE";
        public const string SP_DELETE_BY_HEADER = "TK_Rapor_DesignDet_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_Design = "Rel_Rapor_Design";
            public const string Poin = "Poin";
            public const string Rel_KomponenRapor = "Rel_KomponenRapor";
            public const string NamaKomponen = "NamaKomponen";
            public const string JenisKomponen = "JenisKomponen";
            public const string IsNewPage = "IsNewPage";
            public const string IsLockGuruKelas = "IsLockGuruKelas";
            public const string Urut = "Urut";
        }

        private static Rapor_DesignDet GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_DesignDet
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_Design = new Guid(row[NamaField.Rel_Rapor_Design].ToString()),
                Poin = row[NamaField.Poin].ToString(),
                Rel_KomponenRapor = new Guid(row[NamaField.Rel_KomponenRapor].ToString()),
                JenisKomponen = (JenisKomponenRapor)Convert.ToInt16(row[NamaField.JenisKomponen]),
                NamaKomponen = row[NamaField.NamaKomponen].ToString(),
                IsNewPage = (row[NamaField.IsNewPage] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsNewPage])),
                IsLockGuruKelas = (row[NamaField.IsLockGuruKelas] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsLockGuruKelas])),
                Urut = Convert.ToInt16(row[NamaField.Urut])
            };
        }

        public static List<Rapor_DesignDet> GetAll_Entity()
        {
            List<Rapor_DesignDet> hasil = new List<Rapor_DesignDet>();
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

        public static List<Rapor_DesignDet> GetByHeader_Entity(string Rel_Rapor_Design)
        {
            List<Rapor_DesignDet> hasil = new List<Rapor_DesignDet>();
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

        public static Rapor_DesignDet GetByID_Entity(string kode)
        {
            Rapor_DesignDet hasil = new Rapor_DesignDet();
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

        public static void Insert(Rapor_DesignDet m, string user_id)
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

        public static void InsertLengkap(Rapor_DesignDet m, string user_id)
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

                Guid kode = Guid.NewGuid();
                if (m.Kode != new Guid(Application_Libs.Constantas.GUID_NOL))
                {
                    kode = m.Kode;
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Design, m.Rel_Rapor_Design));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Poin, m.Poin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KomponenRapor, m.Rel_KomponenRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKomponen, (int)m.JenisKomponen));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urut, (int)m.Urut));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNewPage, m.IsNewPage));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsLockGuruKelas, m.IsLockGuruKelas));
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

        public static void Update(Rapor_DesignDet m, string user_id)
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

        public static void UpdatePengaturan(string kode, bool is_new_page, bool is_lock_guru_kelas)
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
                comm.CommandText = SP_UPDATE_PENGATURAN;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNewPage, is_new_page));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsLockGuruKelas, is_lock_guru_kelas));
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