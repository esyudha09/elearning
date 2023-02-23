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
    public static class DAO_PegawaiPengalamanKerjaLuar
    {
        public const string SP_SELECT_BY_HEADER = "PegawaiPengalamanKerjaLuar_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_ID = "PegawaiPengalamanKerjaLuar_SELECT_BY_ID";

        public const string SP_INSERT = "PegawaiPengalamanKerjaLuar_INSERT";

        public const string SP_UPDATE = "PegawaiPengalamanKerjaLuar_UPDATE";

        public const string SP_DELETE = "PegawaiPengalamanKerjaLuar_DELETE";
        public const string SP_DELETE_BY_HEADER = "PegawaiPengalamanKerjaLuar_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Pegawai = "Rel_Pegawai";
            public const string NamaPerusahaan = "NamaPerusahaan";
            public const string Dari = "Dari";
            public const string Sampai = "Sampai";
            public const string Rel_Jabatan = "Rel_Jabatan";
            public const string Jabatan = "Jabatan";
            public const string Urutan = "Urutan";
        }

        public static PegawaiPengalamanKerjaLuar GetEntityFromDataRow(DataRow row)
        {
            return new PegawaiPengalamanKerjaLuar
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Pegawai = row[NamaField.Rel_Pegawai].ToString(),
                NamaPerusahaan = row[NamaField.NamaPerusahaan].ToString(),
                Dari = row[NamaField.Dari].ToString(),
                Sampai = row[NamaField.Sampai].ToString(),
                Rel_Jabatan = row[NamaField.Rel_Jabatan].ToString(),
                Jabatan = row[NamaField.Jabatan].ToString(),
                Urutan = Convert.ToInt16(row[NamaField.Urutan])
            };
        }

        public static List<PegawaiPengalamanKerjaLuar> GetAllByHeader_Entity(string rel_pegawai)
        {
            List<PegawaiPengalamanKerjaLuar> hasil = new List<PegawaiPengalamanKerjaLuar>();
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

        public static PegawaiPengalamanKerjaLuar GetByID_Entity(string kode)
        {
            PegawaiPengalamanKerjaLuar hasil = new PegawaiPengalamanKerjaLuar();
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

        public static void Insert(PegawaiPengalamanKerjaLuar m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaPerusahaan, m.NamaPerusahaan));
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

        public static void Update(PegawaiPengalamanKerjaLuar m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaPerusahaan, m.NamaPerusahaan));
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