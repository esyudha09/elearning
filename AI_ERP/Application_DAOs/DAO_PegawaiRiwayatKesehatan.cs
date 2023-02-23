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
    public static class DAO_PegawaiRiwayatKesehatan
    {
        public const string SP_SELECT_BY_HEADER = "PegawaiRiwayatKesehatan_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_ID = "PegawaiRiwayatKesehatan_SELECT_BY_ID";

        public const string SP_INSERT = "PegawaiRiwayatKesehatan_INSERT";

        public const string SP_UPDATE = "PegawaiRiwayatKesehatan_UPDATE";

        public const string SP_DELETE = "PegawaiRiwayatKesehatan_DELETE";
        public const string SP_DELETE_BY_HEADER = "PegawaiRiwayatKesehatan_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Pegawai = "Rel_Pegawai";
            public const string DariTanggal = "DariTanggal";
            public const string SampaiTanggal = "SampaiTanggal";
            public const string IsIzin = "IsIzin";
            public const string NamaPenyakit = "NamaPenyakit";
            public const string RSKlinik = "RSKlinik";
            public const string Dokter = "Dokter";
            public const string Keterangan = "Keterangan";
            public const string Urutan = "Urutan";
        }

        public static PegawaiRiwayatKesehatan GetEntityFromDataRow(DataRow row)
        {
            return new PegawaiRiwayatKesehatan
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Pegawai = row[NamaField.Rel_Pegawai].ToString(),
                DariTanggal = Convert.ToDateTime(row[NamaField.DariTanggal]),
                SampaiTanggal = Convert.ToDateTime(row[NamaField.SampaiTanggal]),
                IsIzin = Convert.ToBoolean(row[NamaField.IsIzin]),
                NamaPenyakit = row[NamaField.NamaPenyakit].ToString(),
                RSKlinik = row[NamaField.RSKlinik].ToString(),
                Dokter = row[NamaField.Dokter].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                Urutan = Convert.ToInt16(row[NamaField.Urutan])
            };
        }

        public static List<PegawaiRiwayatKesehatan> GetAllByHeader_Entity(string rel_pegawai)
        {
            List<PegawaiRiwayatKesehatan> hasil = new List<PegawaiRiwayatKesehatan>();
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

        public static PegawaiRiwayatKesehatan GetByID_Entity(string kode)
        {
            PegawaiRiwayatKesehatan hasil = new PegawaiRiwayatKesehatan();
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

        public static void Insert(PegawaiRiwayatKesehatan m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DariTanggal, m.DariTanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SampaiTanggal, m.SampaiTanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsIzin, m.IsIzin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaPenyakit, m.NamaPenyakit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RSKlinik, m.RSKlinik));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Dokter, m.Dokter));
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

        public static void Update(PegawaiRiwayatKesehatan m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DariTanggal, m.DariTanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SampaiTanggal, m.SampaiTanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsIzin, m.IsIzin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaPenyakit, m.NamaPenyakit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RSKlinik, m.RSKlinik));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Dokter, m.Dokter));
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