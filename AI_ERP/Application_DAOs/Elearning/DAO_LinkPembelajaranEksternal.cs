using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;

namespace AI_ERP.Application_DAOs.Elearning
{
    public static class DAO_LinkPembelajaranEksternal
    {
        public const string SP_SELECT_ALL = "LinkPembelajaranEksternal_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "LinkPembelajaranEksternal_SELECT_BY_ID";
        public const string SP_SELECT_DISTINCT_KATEGORI = "LinkPembelajaranEksternal_SELECT_DISTINCT_KATEGORI";
        public const string SP_SELECT_DISTINCT_KATEGORI_FOR_SEARCH = "LinkPembelajaranEksternal_SELECT_DISTINCT_KATEGORI_FOR_SEARCH";

        public const string SP_INSERT = "LinkPembelajaranEksternal_INSERT";

        public const string SP_UPDATE = "LinkPembelajaranEksternal_UPDATE";

        public const string SP_DELETE = "LinkPembelajaranEksternal_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Kategori = "Kategori";
            public const string Unit = "Unit";
            public const string Nama = "Nama";
            public const string Link = "Link";
            public const string RTG_UNIT = "RTG_UNIT";
            public const string RTG_LEVEL = "RTG_LEVEL";
            public const string RTG_SEMESTER = "RTG_SEMESTER";
            public const string RTG_KELAS = "RTG_KELAS";
            public const string RTG_SUBKELAS = "RTG_SUBKELAS";
            public const string Rel_Pegawai = "Rel_Pegawai";
            public const string TanggalBuat = "TanggalBuat";
            public const string TanggalUpdate = "TanggalUpdate";
        }

        private static LinkPembelajaranEksternal GetEntityFromDataRow(DataRow row)
        {
            return new LinkPembelajaranEksternal
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Kategori = row[NamaField.Kategori].ToString(),
                Unit = row[NamaField.Unit].ToString(),
                Nama = row[NamaField.Nama].ToString(),
                Link = row[NamaField.Link].ToString(),
                RTG_UNIT = row[NamaField.RTG_UNIT].ToString(),
                RTG_LEVEL = row[NamaField.RTG_LEVEL].ToString(),
                RTG_SEMESTER = row[NamaField.RTG_SEMESTER].ToString(),
                RTG_KELAS = row[NamaField.RTG_KELAS].ToString(),
                RTG_SUBKELAS = row[NamaField.RTG_SUBKELAS].ToString(),
                Rel_Pegawai = row[NamaField.Rel_Pegawai].ToString(),
                TanggalBuat = Convert.ToDateTime(row[NamaField.TanggalBuat]),
                TanggalUpdate = Convert.ToDateTime(row[NamaField.TanggalUpdate])
            };
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

        public static void Insert(LinkPembelajaranEksternal m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kategori, m.Kategori));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Unit, m.Unit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, m.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Link, m.Link));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_UNIT, m.RTG_UNIT));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_LEVEL, m.RTG_LEVEL));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_SEMESTER, m.RTG_SEMESTER));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_KELAS, m.RTG_KELAS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_SUBKELAS, m.RTG_SUBKELAS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pegawai, m.Rel_Pegawai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalBuat, DateTime.Now));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalUpdate, DateTime.Now));

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


        public static void Update(LinkPembelajaranEksternal m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kategori, m.Kategori));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Unit, m.Unit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, m.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Link, m.Link));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_UNIT, m.RTG_UNIT));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_LEVEL, m.RTG_LEVEL));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_SEMESTER, m.RTG_SEMESTER));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_KELAS, m.RTG_KELAS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_SUBKELAS, m.RTG_SUBKELAS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pegawai, m.Rel_Pegawai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalBuat, m.TanggalBuat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalUpdate, DateTime.Now));

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

        public static LinkPembelajaranEksternal GetByID_Entity(string kode)
        {
            LinkPembelajaranEksternal hasil = new LinkPembelajaranEksternal();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
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

        public static List<LinkPembelajaranEksternal> GetAll_Entity()
        {
            List<LinkPembelajaranEksternal> hasil = new List<LinkPembelajaranEksternal>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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

        public static List<string> GetDistinctKategori_Entity()
        {
            List<string> hasil = new List<string>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_KATEGORI;

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(row[NamaField.Kategori].ToString());
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
    }
}