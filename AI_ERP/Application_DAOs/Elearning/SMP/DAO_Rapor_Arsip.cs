using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.SMP;

namespace AI_ERP.Application_DAOs.Elearning.SMP
{
    public static class DAO_Rapor_Arsip
    {
        public const string SP_SELECT_ALL = "SMP_Rapor_Arsip_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SMP_Rapor_Arsip_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "SMP_Rapor_Arsip_SELECT_BY_ID";

        public const string SP_INSERT = "SMP_Rapor_Arsip_INSERT";

        public const string SP_UPDATE = "SMP_Rapor_Arsip_UPDATE";

        public const string SP_DELETE = "SMP_Rapor_Arsip_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string JenisRapor = "JenisRapor";
            public const string TanggalAwalAbsen = "TanggalAwalAbsen";
            public const string TanggalAkhirAbsen = "TanggalAkhirAbsen";
            public const string TanggalClosing = "TanggalClosing";
            public const string KepalaSekolah = "KepalaSekolah";
            public const string TanggalRapor = "TanggalRapor";
            public const string Keterangan = "Keterangan";
            public const string IsArsip = "IsArsip";
        }

        private static Rapor_Arsip GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_Arsip
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                JenisRapor = row[NamaField.JenisRapor].ToString(),
                TanggalAwalAbsen = Convert.ToDateTime(row[NamaField.TanggalAwalAbsen]),
                TanggalAkhirAbsen = Convert.ToDateTime(row[NamaField.TanggalAkhirAbsen]),
                TanggalClosing = Convert.ToDateTime(row[NamaField.TanggalClosing]),
                KepalaSekolah = row[NamaField.KepalaSekolah].ToString(),
                TanggalRapor = Convert.ToDateTime((row[NamaField.TanggalRapor] == DBNull.Value ? DateTime.Now : row[NamaField.TanggalRapor])),
                Keterangan = row[NamaField.Keterangan].ToString(),
                IsArsip = (row[NamaField.IsArsip] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsArsip]))
            };
        }

        public static List<Rapor_Arsip> GetAll_Entity()
        {
            List<Rapor_Arsip> hasil = new List<Rapor_Arsip>();
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

        public static Rapor_Arsip GetByID_Entity(string kode)
        {
            Rapor_Arsip hasil = new Rapor_Arsip();
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

        public static void Insert(Rapor_Arsip m)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisRapor, m.JenisRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalAwalAbsen, m.TanggalAwalAbsen));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalAkhirAbsen, m.TanggalAkhirAbsen));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalClosing, m.TanggalClosing));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KepalaSekolah, m.KepalaSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalRapor, m.TanggalRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsArsip, m.IsArsip));
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

        public static void Update(Rapor_Arsip m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisRapor, m.JenisRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalAwalAbsen, m.TanggalAwalAbsen));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalAkhirAbsen, m.TanggalAkhirAbsen));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalClosing, m.TanggalClosing));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KepalaSekolah, m.KepalaSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalRapor, m.TanggalRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsArsip, m.IsArsip));
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