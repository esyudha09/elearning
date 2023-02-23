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
    public static class DAO_Rapor_DesignEkskulDet
    {
        public const string SP_SELECT_ALL = "KB_Rapor_DesignEkskulDet_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "KB_Rapor_DesignEkskulDet_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "KB_Rapor_DesignEkskulDet_SELECT_BY_HEADER";

        public const string SP_INSERT = "KB_Rapor_DesignEkskulDet_INSERT";
        public const string SP_INSERT_LENGKAP = "KB_Rapor_DesignEkskulDet_INSERT_LENGKAP";

        public const string SP_UPDATE = "KB_Rapor_DesignEkskulDet_UPDATE";
        public const string SP_UPDATE_URUT = "KB_Rapor_DesignEkskulDet_UPDATE_URUT";

        public const string SP_DELETE = "KB_Rapor_DesignEkskulDet_DELETE";
        public const string SP_DELETE_BY_HEADER = "KB_Rapor_DesignEkskulDet_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_DesignEkskul = "Rel_Rapor_DesignEkskul";
            public const string Poin = "Poin";
            public const string Rel_KomponenRapor = "Rel_KomponenRapor";
            public const string JenisKomponen = "JenisKomponen";
            public const string NamaKomponen = "NamaKomponen";
            public const string Urut = "Urut";
        }

        private static Rapor_DesignEkskulDet GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_DesignEkskulDet
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_DesignEkskul = new Guid(row[NamaField.Rel_Rapor_DesignEkskul].ToString()),
                Poin = row[NamaField.Poin].ToString(),
                Rel_KomponenRapor = new Guid(row[NamaField.Rel_KomponenRapor].ToString()),
                JenisKomponen = (JenisKomponenRapor)row[NamaField.JenisKomponen],
                NamaKomponen = row[NamaField.NamaKomponen].ToString(),
                Urut = Convert.ToInt16(row[NamaField.Urut].ToString())
            };
        }

        public static List<Rapor_DesignEkskulDet> GetAll_Entity()
        {
            List<Rapor_DesignEkskulDet> hasil = new List<Rapor_DesignEkskulDet>();
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

        public static Rapor_DesignEkskulDet GetByID_Entity(string kode)
        {
            Rapor_DesignEkskulDet hasil = new Rapor_DesignEkskulDet();
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

        public static List<Rapor_DesignEkskulDet> GetByHeader_Entity(string rel_rapor_designekskul)
        {
            List<Rapor_DesignEkskulDet> hasil = new List<Rapor_DesignEkskulDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_DesignEkskul, rel_rapor_designekskul);

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

        public static void Insert(Rapor_DesignEkskulDet m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_DesignEkskul, m.Rel_Rapor_DesignEkskul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Poin, m.Poin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KomponenRapor, m.Rel_KomponenRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKomponen, m.JenisKomponen));
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

        public static void InsertLengkap(Rapor_DesignEkskulDet m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_DesignEkskul, m.Rel_Rapor_DesignEkskul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Poin, m.Poin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KomponenRapor, m.Rel_KomponenRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKomponen, (int)m.JenisKomponen));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urut, m.Urut));
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

        public static void Update(Rapor_DesignEkskulDet m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_DesignEkskul, m.Rel_Rapor_DesignEkskul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Poin, m.Poin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KomponenRapor, m.Rel_KomponenRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKomponen, m.JenisKomponen));
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

        public static void UpdateUrut(Rapor_DesignEkskulDet m, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urut, m.Urut));
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