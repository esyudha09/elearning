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
    public static class DAO_Rapor_DesignDet_GuruKhusus
    {
        public const string SP_SELECT_ALL = "TK_Rapor_DesignDet_GuruKhusus_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "TK_Rapor_DesignDet_GuruKhusus_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "TK_Rapor_DesignDet_GuruKhusus_SELECT_BY_HEADER";

        public const string SP_INSERT = "TK_Rapor_DesignDet_GuruKhusus_INSERT";

        public const string SP_DELETE = "TK_Rapor_DesignDet_GuruKhusus_DELETE";
        public const string SP_DELETE_BY_HEADER = "TK_Rapor_DesignDet_GuruKhusus_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_DesignDet = "Rel_Rapor_DesignDet";
            public const string Rel_Guru = "Rel_Guru";
            public const string Urut = "Urut";
        }

        private static Rapor_DesignDet_GuruKhusus GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_DesignDet_GuruKhusus
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_DesignDet = new Guid(row[NamaField.Rel_Rapor_DesignDet].ToString()),
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                Urut = Convert.ToInt16(row[NamaField.Urut])
            };
        }

        public static List<Rapor_DesignDet_GuruKhusus> GetAll_Entity()
        {
            List<Rapor_DesignDet_GuruKhusus> hasil = new List<Rapor_DesignDet_GuruKhusus>();
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

        public static List<Rapor_DesignDet_GuruKhusus> GetByHeader_Entity(string rel_rapor_design_det)
        {
            List<Rapor_DesignDet_GuruKhusus> hasil = new List<Rapor_DesignDet_GuruKhusus>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_DesignDet, rel_rapor_design_det);

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

        public static Rapor_DesignDet_GuruKhusus GetByID_Entity(string kode)
        {
            Rapor_DesignDet_GuruKhusus hasil = new Rapor_DesignDet_GuruKhusus();
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

        public static void DeleteByHeader(string rel_rapor_design_det)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_DesignDet, rel_rapor_design_det));
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

        public static void Insert(Rapor_DesignDet_GuruKhusus m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_DesignDet, m.Rel_Rapor_DesignDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, m.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urut, m.Urut));
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