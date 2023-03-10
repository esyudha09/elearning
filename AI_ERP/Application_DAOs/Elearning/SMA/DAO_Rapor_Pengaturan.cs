using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SMA;

namespace AI_ERP.Application_DAOs.Elearning.SMA
{
    public static class DAO_Rapor_Pengaturan
    {
        public const string SP_SELECT_ALL = "SMA_Rapor_Pengaturan_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SMA_Rapor_Pengaturan_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "SMA_Rapor_Pengaturan_SELECT_BY_ID";

        public const string SP_INSERT = "SMA_Rapor_Pengaturan_INSERT";

        public const string SP_UPDATE = "SMA_Rapor_Pengaturan_UPDATE";
        public const string SP_UPDATE_EMAIL_LTS = "SMA_Rapor_Pengaturan_UPDATE_EMAIL_LTS";
        public const string SP_UPDATE_EMAIL_RAPOR = "SMA_Rapor_Pengaturan_UPDATE_EMAIL_RAPOR";

        public const string SP_SAVE = "SMA_Rapor_Pengaturan_SAVE";

        public const string SP_DELETE = "SMA_Rapor_Pengaturan_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string KepalaSekolah = "KepalaSekolah";
            public const string KurikulumRaporLevel10 = "KurikulumRaporLevel10";
            public const string KurikulumRaporLevel11 = "KurikulumRaporLevel11";
            public const string KurikulumRaporLevel12 = "KurikulumRaporLevel12";
            public const string TemplateEmailLTS = "TemplateEmailLTS";
            public const string TemplateEmailRapor = "TemplateEmailRapor";
            public const string TanggalBukaLinkRapor = "TanggalBukaLinkRapor";
            public const string TanggalBukaLinkLTS = "TanggalBukaLinkLTS";
        }

        private static Rapor_Pengaturan GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_Pengaturan
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                KepalaSekolah = row[NamaField.KepalaSekolah].ToString(),
                KurikulumRaporLevel10 = row[NamaField.KurikulumRaporLevel10].ToString(),
                KurikulumRaporLevel11 = row[NamaField.KurikulumRaporLevel11].ToString(),
                KurikulumRaporLevel12 = row[NamaField.KurikulumRaporLevel12].ToString(),
                TemplateEmailLTS = row[NamaField.TemplateEmailLTS].ToString(),
                TemplateEmailRapor = row[NamaField.TemplateEmailRapor].ToString(),
                TanggalBukaLinkRapor = (
                        row[NamaField.TanggalBukaLinkRapor] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(row[NamaField.TanggalBukaLinkRapor])
                    ),
                TanggalBukaLinkLTS = (
                        row[NamaField.TanggalBukaLinkLTS] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(row[NamaField.TanggalBukaLinkLTS])
                    )
            };
        }

        public static void Save(Rapor_Pengaturan m)
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
                comm.CommandText = SP_SAVE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KepalaSekolah, m.KepalaSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KurikulumRaporLevel10, m.KurikulumRaporLevel10));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KurikulumRaporLevel11, m.KurikulumRaporLevel11));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KurikulumRaporLevel12, m.KurikulumRaporLevel12));
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

        public static void Insert(Rapor_Pengaturan m)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KepalaSekolah, m.KepalaSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KurikulumRaporLevel10, m.KurikulumRaporLevel10));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KurikulumRaporLevel11, m.KurikulumRaporLevel11));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KurikulumRaporLevel12, m.KurikulumRaporLevel12));
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

        public static void Update(Rapor_Pengaturan m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KepalaSekolah, m.KepalaSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KurikulumRaporLevel10, m.KurikulumRaporLevel10));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KurikulumRaporLevel11, m.KurikulumRaporLevel11));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KurikulumRaporLevel12, m.KurikulumRaporLevel12));
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

        public static void UpdateEmailLTS(string kode, string template_email, DateTime tanggal_buka_link_lts)
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
                comm.CommandText = SP_UPDATE_EMAIL_LTS;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TemplateEmailLTS, template_email));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalBukaLinkLTS, tanggal_buka_link_lts));
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

        public static void UpdateEmailRapor(string kode, string template_email, DateTime tanggal_buka_link_rapor)
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
                comm.CommandText = SP_UPDATE_EMAIL_RAPOR;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TemplateEmailRapor, template_email));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalBukaLinkRapor, tanggal_buka_link_rapor));
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

        public static Rapor_Pengaturan Get_Entity()
        {
            Rapor_Pengaturan hasil = new Rapor_Pengaturan();
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

        public static Rapor_Pengaturan GetByID_Entity(string kode)
        {
            Rapor_Pengaturan hasil = new Rapor_Pengaturan();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_ID;
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));

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

        public static List<Rapor_Pengaturan> GetAll_Entity()
        {
            List<Rapor_Pengaturan> hasil = new List<Rapor_Pengaturan>();
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

        public static Rapor_Pengaturan GetByTABySM_Entity(string tahun_ajaran, string semester)
        {
            Rapor_Pengaturan hasil = GetAll_Entity().FindAll(
                    m0 => m0.TahunAjaran == tahun_ajaran &&
                          m0.Semester == semester
                ).ToList().FirstOrDefault();

            return hasil;
        }
    }
}