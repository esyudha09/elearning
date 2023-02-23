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
    public static class DAO_PegawaiPengalamanKerjaDalam
    {
        public const string SP_SELECT_BY_HEADER = "PegawaiPengalamanKerjaDalam_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_ID = "PegawaiPengalamanKerjaDalam_SELECT_BY_ID";

        public const string SP_INSERT = "PegawaiPengalamanKerjaDalam_INSERT";

        public const string SP_UPDATE = "PegawaiPengalamanKerjaDalam_UPDATE";

        public const string SP_DELETE = "PegawaiPengalamanKerjaDalam_DELETE";
        public const string SP_DELETE_BY_HEADER = "PegawaiPengalamanKerjaDalam_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Pegawai = "Rel_Pegawai";
            public const string Rel_Divisi = "Rel_Divisi";
            public const string Divisi = "Divisi";
            public const string Rel_Unit = "Rel_Unit";
            public const string Unit = "Unit";
            public const string Dari = "Dari";
            public const string Sampai = "Sampai";
            public const string Rel_Jabatan = "Rel_Jabatan";
            public const string Jabatan = "Jabatan";
            public const string Urutan = "Urutan";
        }

        public static PegawaiPengalamanKerjaDalam GetEntityFromDataRow(DataRow row)
        {
            return new PegawaiPengalamanKerjaDalam
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Pegawai = row[NamaField.Rel_Pegawai].ToString(),
                Rel_Divisi = row[NamaField.Rel_Divisi].ToString(),
                Divisi = row[NamaField.Divisi].ToString(),
                Rel_Unit = row[NamaField.Rel_Unit].ToString(),
                Unit = row[NamaField.Unit].ToString(),
                Dari = row[NamaField.Dari].ToString(),
                Sampai = row[NamaField.Sampai].ToString(),
                Rel_Jabatan = row[NamaField.Rel_Jabatan].ToString(),
                Jabatan = row[NamaField.Jabatan].ToString(),
                Urutan = Convert.ToInt16(row[NamaField.Urutan])
            };
        }

        public static List<PegawaiPengalamanKerjaDalam> GetAllByHeader_Entity(string rel_pegawai)
        {
            List<PegawaiPengalamanKerjaDalam> hasil = new List<PegawaiPengalamanKerjaDalam>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Pegawai, rel_pegawai);

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

        public static PegawaiPengalamanKerjaDalam GetByID_Entity(string kode)
        {
            PegawaiPengalamanKerjaDalam hasil = new PegawaiPengalamanKerjaDalam();
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

        public static void Delete(string Kode)
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

        public static void DeleteByHeader(string rel_pegawai)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pegawai, rel_pegawai));
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

        public static void Insert(PegawaiPengalamanKerjaDalam m)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pegawai, m.Rel_Pegawai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Divisi, m.Rel_Divisi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Unit, m.Rel_Unit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Dari, m.Dari));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Sampai, m.Sampai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Jabatan, m.Rel_Jabatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urutan, m.Urutan));

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

        public static void Update(PegawaiPengalamanKerjaDalam m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pegawai, m.Rel_Pegawai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Divisi, m.Rel_Divisi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Unit, m.Rel_Unit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Dari, m.Dari));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Sampai, m.Sampai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Jabatan, m.Rel_Jabatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urutan, m.Urutan));

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