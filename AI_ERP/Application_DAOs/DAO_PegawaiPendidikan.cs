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
    public static class DAO_PegawaiPendidikan
    {
        public const string SP_SELECT_BY_HEADER = "PegawaiPendidikan_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_ID = "PegawaiPendidikan_SELECT_BY_ID";

        public const string SP_INSERT = "PegawaiPendidikan_INSERT";

        public const string SP_UPDATE = "PegawaiPendidikan_UPDATE";

        public const string SP_DELETE = "PegawaiPendidikan_DELETE";
        public const string SP_DELETE_BY_HEADER = "PegawaiPendidikan_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Pegawai = "Rel_Pegawai";
            public const string JenisPendidikan = "JenisPendidikan";
            public const string Rel_Lembaga = "Rel_Lembaga";
            public const string Lembaga = "Lembaga";
            public const string DariTahun = "DariTahun";
            public const string SampaiTahun = "SampaiTahun";
            public const string NilaiAkhir = "NilaiAkhir";
            public const string Rel_Jurusan = "Rel_Jurusan";
            public const string Jurusan = "Jurusan";
            public const string Keterangan = "Keterangan";
            public const string Urutan = "Urutan";
        }

        public static PegawaiPendidikan GetEntityFromDataRow(DataRow row)
        {
            return new PegawaiPendidikan
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Pegawai = row[NamaField.Rel_Pegawai].ToString(),
                JenisPendidikan = row[NamaField.JenisPendidikan].ToString(),
                Rel_Lembaga = row[NamaField.Rel_Lembaga].ToString(),
                Lembaga = row[NamaField.Lembaga].ToString(),
                DariTahun = row[NamaField.DariTahun].ToString(),
                SampaiTahun = row[NamaField.SampaiTahun].ToString(),
                NilaiAkhir = row[NamaField.NilaiAkhir].ToString(),
                Rel_Jurusan = row[NamaField.Rel_Jurusan].ToString(),
                Jurusan = row[NamaField.Jurusan].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                Urutan = Convert.ToInt16(row[NamaField.Urutan])
            };
        }

        public static List<PegawaiPendidikan> GetAllByHeader_Entity(string rel_pegawai)
        {
            List<PegawaiPendidikan> hasil = new List<PegawaiPendidikan>();
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

        public static PegawaiPendidikan GetByID_Entity(string kode)
        {
            PegawaiPendidikan hasil = new PegawaiPendidikan();
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

        public static void Insert(PegawaiPendidikan m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPendidikan, m.JenisPendidikan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Lembaga, m.Rel_Lembaga));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DariTahun, m.DariTahun));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SampaiTahun, m.SampaiTahun));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NilaiAkhir, m.NilaiAkhir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Jurusan, m.Rel_Jurusan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
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

        public static void Update(PegawaiPendidikan m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPendidikan, m.JenisPendidikan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Lembaga, m.Rel_Lembaga));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DariTahun, m.DariTahun));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SampaiTahun, m.SampaiTahun));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NilaiAkhir, m.NilaiAkhir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Jurusan, m.Rel_Jurusan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
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