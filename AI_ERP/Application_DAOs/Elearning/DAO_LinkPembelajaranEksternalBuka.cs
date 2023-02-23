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
    public static class DAO_LinkPembelajaranEksternalBuka
    {
        public const string SP_SELECT_ALL = "LinkPembelajaranEksternalBuka_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "LinkPembelajaranEksternalBuka_SELECT_BY_ID";
        public const string SP_SELECT_HIT_COUNTER = "LinkPembelajaranEksternalBuka_SELECT_HIT_COUNTER";
        public const string SP_SELECT_HIT_COUNTER_FOR_SEARCH = "LinkPembelajaranEksternalBuka_SELECT_HIT_COUNTER_FOR_SEARCH";
        public const string SP_SELECT_HISTORY_BY_PEGAWAI = "LinkPembelajaranEksternalBuka_SELECT_HISTORY_BY_PEGAWAI";
        public const string SP_SELECT_HISTORY_BY_PEGAWAI_BY_TANGGAL = "LinkPembelajaranEksternalBuka_SELECT_HISTORY_BY_PEGAWAI_BY_TANGGAL";

        public const string SP_INSERT = "LinkPembelajaranEksternalBuka_INSERT";

        public const string SP_UPDATE = "LinkPembelajaranEksternalBuka_UPDATE";

        public const string SP_DELETE = "LinkPembelajaranEksternalBuka_DELETE";

        public class HitCounter
        {
            public Guid Kode { get; set; }
            public string Nama { get; set; }
            public string Kategori { get; set; }
            public string Unit { get; set; }
            public string LinkOwner { get; set; }
            public int JumlahAksesSemua { get; set; }
            public int JumlahAksesGuruYbs { get; set; }
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_LinkPembelajaranEksternal = "Rel_LinkPembelajaranEksternal";
            public const string Tanggal = "Tanggal";
            public const string Rel_Pegawai = "Rel_Pegawai";
            public const string Link = "Link";
            public const string Nama = "Nama";
            public const string Kategori = "Kategori";
            public const string RTG_UNIT = "RTG_UNIT";
            public const string RTG_LEVEL = "RTG_LEVEL";
            public const string RTG_SEMESTER = "RTG_SEMESTER";
            public const string RTG_KELAS = "RTG_KELAS";
            public const string RTG_SUBKELAS = "RTG_SUBKELAS";
        }

        private static LinkPembelajaranEksternalBuka GetEntityFromDataRow(DataRow row)
        {
            return new LinkPembelajaranEksternalBuka
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_LinkPembelajaranEksternal = row[NamaField.Rel_LinkPembelajaranEksternal].ToString(),
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                Rel_Pegawai = row[NamaField.Rel_Pegawai].ToString(),
                Link = row[NamaField.Link].ToString(),
                Nama = row[NamaField.Nama].ToString(),
                Kategori = row[NamaField.Kategori].ToString(),
                RTG_UNIT = row[NamaField.RTG_UNIT].ToString(),
                RTG_LEVEL = row[NamaField.RTG_LEVEL].ToString(),
                RTG_SEMESTER = row[NamaField.RTG_SEMESTER].ToString(),
                RTG_KELAS = row[NamaField.RTG_KELAS].ToString(),
                RTG_SUBKELAS = row[NamaField.RTG_SUBKELAS].ToString()
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

        public static void Insert(LinkPembelajaranEksternalBuka m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_LinkPembelajaranEksternal, m.Rel_LinkPembelajaranEksternal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pegawai, m.Rel_Pegawai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Link, m.Link));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, m.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kategori, m.Kategori));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_UNIT, m.RTG_UNIT));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_LEVEL, m.RTG_LEVEL));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_SEMESTER, m.RTG_SEMESTER));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_KELAS, m.RTG_KELAS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_SUBKELAS, m.RTG_SUBKELAS));

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


        public static void Update(LinkPembelajaranEksternalBuka m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_LinkPembelajaranEksternal, m.Rel_LinkPembelajaranEksternal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pegawai, m.Rel_Pegawai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Link, m.Link));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, m.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kategori, m.Kategori));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_UNIT, m.RTG_UNIT));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_LEVEL, m.RTG_LEVEL));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_SEMESTER, m.RTG_SEMESTER));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_KELAS, m.RTG_KELAS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_SUBKELAS, m.RTG_SUBKELAS));

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

        public static LinkPembelajaranEksternalBuka GetByID_Entity(string kode)
        {
            LinkPembelajaranEksternalBuka hasil = new LinkPembelajaranEksternalBuka();
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

        public static List<LinkPembelajaranEksternalBuka> GetAll_Entity()
        {
            List<LinkPembelajaranEksternalBuka> hasil = new List<LinkPembelajaranEksternalBuka>();
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

        public static List<HitCounter> GetHitCounter_Entity(string rel_pegawai)
        {
            List<HitCounter> hasil = new List<HitCounter>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_HIT_COUNTER;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pegawai, rel_pegawai));

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new HitCounter {
                                Kode = new Guid(row["Kode"].ToString()),
                                Nama = row["Nama"].ToString(),
                                Kategori = row["Kategori"].ToString(),
                                Unit = row["Unit"].ToString(),
                                LinkOwner = row["LinkOwner"].ToString(),
                                JumlahAksesSemua = Libs.GetStringToInteger(row["JumlahAksesSemua"].ToString()),
                                JumlahAksesGuruYbs = Libs.GetStringToInteger(row["JumlahAksesGuruYbs"].ToString())
                            }
                        );
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