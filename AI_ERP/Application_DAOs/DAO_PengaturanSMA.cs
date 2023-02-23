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
    public static class DAO_PengaturanSMA
    {
        public const string SP_SELECT_ALL = "PengaturanSMA_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "PengaturanSMA_SELECT_BY_ID";

        public const string SP_INSERT = "PengaturanSMA_INSERT";

        public const string SP_UPDATE = "PengaturanSMA_UPDATE";

        public const string SP_DELETE = "PengaturanSMA_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string HeaderLogo = "HeaderLogo";
            public const string HeaderKop = "HeaderKop";
            public const string HeaderAlamat = "HeaderAlamat";
            public const string IsTestEmail = "IsTestEmail";
            public const string TestEmail = "TestEmail";
            public const string TeksLinkLTS = "TeksLinkLTS";
            public const string ExpiredLinkLTSHari = "ExpiredLinkLTSHari";
            public const string ExpiredLinkLTSJam = "ExpiredLinkLTSJam";
            public const string ExpiredLinkLTSMenit = "ExpiredLinkLTSMenit";
            public const string TemplateHTMLLinkExpired = "TemplateHTMLLinkExpired";
            public const string JenisFileRapor = "JenisFileRapor";
        }

        public static PengaturanSMA GetEntityFromDataRow(DataRow row)
        {
            return new PengaturanSMA
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                HeaderLogo = row[NamaField.HeaderLogo].ToString(),
                HeaderKop = row[NamaField.HeaderKop].ToString(),
                HeaderAlamat = row[NamaField.HeaderAlamat].ToString(),
                IsTestEmail = (row[NamaField.IsTestEmail] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTestEmail])),
                TestEmail = row[NamaField.TestEmail].ToString(),
                TeksLinkLTS = row[NamaField.TeksLinkLTS].ToString(),
                ExpiredLinkLTSHari = Convert.ToInt16((row[NamaField.ExpiredLinkLTSHari] == DBNull.Value ? 0 : row[NamaField.ExpiredLinkLTSHari])),
                ExpiredLinkLTSJam = Convert.ToInt16((row[NamaField.ExpiredLinkLTSJam] == DBNull.Value ? 0 : row[NamaField.ExpiredLinkLTSJam])),
                ExpiredLinkLTSMenit = Convert.ToInt16((row[NamaField.ExpiredLinkLTSMenit] == DBNull.Value ? 0 : row[NamaField.ExpiredLinkLTSMenit])),
                TemplateHTMLLinkExpired = row[NamaField.TemplateHTMLLinkExpired].ToString(),
                JenisFileRapor = row[NamaField.JenisFileRapor].ToString()
            };
        }

        public static List<PengaturanSMA> GetAll_Entity()
        {
            List<PengaturanSMA> hasil = new List<PengaturanSMA>();
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

        public static PengaturanSMA GetByID_Entity(string kode)
        {
            PengaturanSMA hasil = new PengaturanSMA();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            if (kode.Trim() == "") return hasil;
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

        public static void Insert(PengaturanSMA m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HeaderLogo, m.HeaderLogo));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HeaderKop, m.HeaderKop));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HeaderAlamat, m.HeaderAlamat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTestEmail, m.IsTestEmail));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TestEmail, m.TestEmail));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TeksLinkLTS, m.TeksLinkLTS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ExpiredLinkLTSHari, m.ExpiredLinkLTSHari));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ExpiredLinkLTSJam, m.ExpiredLinkLTSJam));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ExpiredLinkLTSMenit, m.ExpiredLinkLTSMenit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TemplateHTMLLinkExpired, m.TemplateHTMLLinkExpired));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisFileRapor, m.JenisFileRapor));
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

        public static void Update(PengaturanSMA m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HeaderLogo, m.HeaderLogo));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HeaderKop, m.HeaderKop));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HeaderAlamat, m.HeaderAlamat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTestEmail, m.IsTestEmail));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TestEmail, m.TestEmail));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TeksLinkLTS, m.TeksLinkLTS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ExpiredLinkLTSHari, m.ExpiredLinkLTSHari));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ExpiredLinkLTSJam, m.ExpiredLinkLTSJam));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ExpiredLinkLTSMenit, m.ExpiredLinkLTSMenit));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TemplateHTMLLinkExpired, m.TemplateHTMLLinkExpired));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisFileRapor, m.JenisFileRapor));
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

        public static void SaveData(PengaturanSMA m)
        {
            if (GetAll_Entity().Count > 0)
            {
                Update(m);
            }
            else
            {
                Insert(m);
            }
        }
    }
}