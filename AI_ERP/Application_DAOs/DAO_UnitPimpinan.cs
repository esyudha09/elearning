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
    public static class DAO_UnitPimpinan
    {
        public const string SP_SELECT_ALL = "UnitPimpinan_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "UnitPimpinan_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "UnitPimpinan_SELECT_BY_ID";
        public const string SP_SELECT_BY_UNIT = "UnitPimpinan_SELECT_BY_UNIT";

        public const string SP_INSERT = "UnitPimpinan_INSERT";

        public const string SP_UPDATE = "UnitPimpinan_UPDATE";

        public const string SP_DELETE = "UnitPimpinan_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Unit = "Rel_Unit";
            public const string Rel_Pegawai = "Rel_Pegawai";
            public const string UrutanLevel = "UrutanLevel";
            public const string Rel_Jabatan = "Rel_Jabatan";
        }

        public static UnitPimpinan GetEntityFromDataRow(DataRow row)
        {
            return new UnitPimpinan
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Unit = new Guid(row[NamaField.Rel_Unit].ToString()),
                Rel_Pegawai = row[NamaField.Rel_Pegawai].ToString(),
                UrutanLevel = Convert.ToInt16(row[NamaField.UrutanLevel]),
                Rel_Jabatan = new Guid(row[NamaField.Rel_Jabatan].ToString())
            };
        }

        public static List<UnitPimpinan> GetByDivisi_Entity(string rel_divisi)
        {
            List<UnitPimpinan> hasil = new List<UnitPimpinan>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_UNIT;
                comm.Parameters.AddWithValue("@Rel_Divisi", rel_divisi);

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

        public static List<UnitPimpinan> GetAll_Entity()
        {
            List<UnitPimpinan> hasil = new List<UnitPimpinan>();
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

        public static UnitPimpinan GetByID_Entity(string kode)
        {
            UnitPimpinan hasil = new UnitPimpinan();
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

        public static void Insert(UnitPimpinan divisi_pimpinan, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Unit, divisi_pimpinan.Rel_Unit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UrutanLevel, divisi_pimpinan.UrutanLevel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Jabatan, divisi_pimpinan.Rel_Jabatan));
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

        public static void Update(UnitPimpinan divisi_pimpinan, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, divisi_pimpinan.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Unit, divisi_pimpinan.Rel_Unit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UrutanLevel, divisi_pimpinan.UrutanLevel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Jabatan, divisi_pimpinan.Rel_Jabatan));
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