using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_DAOs
{
    public static class DAO_MailJobs
    {
        public const string SP_SELECT_ALL = "MailJobs_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "MailJobs_SELECT_BY_ID";
        public const string SP_SELECT_BY_IS_DONE = "MailJobs_SELECT_BY_IS_DONE";
        public const string SP_SELECT_BY_APP = "MailJobs_SELECT_BY_APP";
        public const string SP_SELECT_BY_SMTP_BY_SENDER_BY_TANGGAL = "MailJobs_SELECT_BY_SMTP_BY_SENDER_BY_TANGGAL";
        public const string SP_SELECT_SUKSES = "MailJobs_SELECT_SUKSES";
        public const string SP_SELECT_GAGAL = "MailJobs_SELECT_GAGAL";
        public const string SP_SELECT_PENGUMUMAN_PSB_BY_NO_SELEKSI = "MailJobs_SELECT_PENGUMUMAN_PSB_BY_NO_SELEKSI";
        public const string SP_SELECT_PENGUMUMAN_LTS_BY_SISWA = "MailJobs_SELECT_PENGUMUMAN_LTS_BY_SISWA";
        public const string SP_SELECT_PENGUMUMAN_RAPOR_BY_SISWA = "MailJobs_SELECT_PENGUMUMAN_RAPOR_BY_SISWA";

        public const string SP_INSERT = "MailJobs_INSERT";
        public const string SP_INSERT_LENGKAP = "MailJobs_INSERT_LENGKAP";

        public const string SP_UPDATE_IS_DONE = "MailJobs_UPDATE_IS_DONE";
        public const string SP_UPDATE_STATUS = "MailJobs_UPDATE_STATUS";
        public const string SP_UPDATE_SMTP = "MailJobs_UPDATE_SMTP";

        public const string MSG_EMAIL_TERKIRIM = "Terkirim";
        public const string MSG_STATUS_MENGIRIM = "Mengirim Email...";

        public static class NamaField
        {
            public const string KODE = "Kode";
            public const string REL_APLIKASI = "Rel_Aplikasi";
            public const string KETERANGAN = "Keterangan";
            public const string SENDER = "Sender";
            public const string SMTP = "SMTP";
            public const string PORT = "Port";
            public const string PWD = "Pwd";
            public const string SENDER_NAME = "SenderName";
            public const string TUJUAN = "Tujuan";
            public const string TO_PERSON = "ToPerson";
            public const string SUBJEK = "Subjek";
            public const string MESSAGE = "Message";
            public const string TANGGAL = "Tanggal";
            public const string STATUS = "Status";
            public const string IS_DONE = "IsDone";
            public const string USER_ID_SENDER = "UserIDSender";
            public const string LINK_EXPIRED_DATE = "LinkExpiredDate";
        }

        private static MailJobs GetEntityFromDataRow(DataRow row)
        {
            return new MailJobs
            {
                Kode = new Guid(row[NamaField.KODE].ToString()),
                Rel_Aplikasi = row[NamaField.REL_APLIKASI].ToString(),
                Keterangan = row[NamaField.KETERANGAN].ToString(),
                Sender = row[NamaField.SENDER].ToString(),
                SMTP = row[NamaField.SMTP].ToString(),
                Port = row[NamaField.PORT].ToString(),
                PWD = row[NamaField.PWD].ToString(),
                SenderName = row[NamaField.SENDER_NAME].ToString(),
                Tujuan = row[NamaField.TUJUAN].ToString(),
                ToPerson = row[NamaField.TO_PERSON].ToString(),
                Subjek = row[NamaField.SUBJEK].ToString(),
                Message = row[NamaField.MESSAGE].ToString(),
                Tanggal = Convert.ToDateTime(row[NamaField.TANGGAL]),
                Status = row[NamaField.STATUS].ToString(),
                IsDone = Convert.ToBoolean(row[NamaField.IS_DONE]),
                UserIDSender = row[NamaField.USER_ID_SENDER].ToString(),
                LinkExpiredDate = Convert.ToDateTime((row[NamaField.LINK_EXPIRED_DATE] == DBNull.Value ? DateTime.MinValue : row[NamaField.LINK_EXPIRED_DATE]))
            };
        }

        public static List<MailJobs> GetData_Entity()
        {
            List<MailJobs> lst = new List<MailJobs>();
            SqlConnection conn = Libs.GetConnection_Mailer();
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
                lst.Clear();
                foreach (DataRow row in dtResult.Rows)
                {
                    lst.Add(GetEntityFromDataRow(row));
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

            return lst;
        }

        public static List<MailJobs> GetDataByIsDone_Entity(bool is_done)
        {
            List<MailJobs> lst = new List<MailJobs>();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_IS_DONE;
                comm.Parameters.AddWithValue("@" + NamaField.IS_DONE, is_done);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                lst.Clear();
                foreach (DataRow row in dtResult.Rows)
                {
                    lst.Add(GetEntityFromDataRow(row));
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

            return lst;
        }

        public static MailJobs GetDataByID_Entity(string kode)
        {
            MailJobs hasil = new MailJobs();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_ID;
                comm.Parameters.AddWithValue("@" + NamaField.KODE, kode);

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

        public static List<MailJobs> GetDataByApp_Entity(string rel_aplikasi)
        {
            List<MailJobs> hasil = new List<MailJobs>();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_APP;
                comm.Parameters.AddWithValue("@" + NamaField.REL_APLIKASI, rel_aplikasi);

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

        public static List<MailJobs> GetDataBySMTPBySenderByTanggal_Entity(string smtp, string sender, DateTime tanggal)
        {
            List<MailJobs> hasil = new List<MailJobs>();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_SMTP_BY_SENDER_BY_TANGGAL;
                comm.Parameters.AddWithValue("@" + NamaField.SMTP, smtp);
                comm.Parameters.AddWithValue("@" + NamaField.SENDER, sender);
                comm.Parameters.AddWithValue("@" + NamaField.TANGGAL, tanggal);

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

        public static List<MailJobs> GetPengumumanPSBByNoSeleksi_Entity(string no_seleksi)
        {
            List<MailJobs> hasil = new List<MailJobs>();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_PENGUMUMAN_PSB_BY_NO_SELEKSI;
                comm.Parameters.AddWithValue("@NoSeleksi", no_seleksi);

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

        public static List<MailJobs> GetPengumumanLTSBySiswa_Entity(string rel_siswa)
        {
            List<MailJobs> hasil = new List<MailJobs>();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_PENGUMUMAN_LTS_BY_SISWA;
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);

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

        public static List<MailJobs> GetPengumumanRaporBySiswa_Entity(string rel_siswa)
        {
            List<MailJobs> hasil = new List<MailJobs>();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_PENGUMUMAN_RAPOR_BY_SISWA;
                comm.Parameters.AddWithValue("@Rel_Siswa", rel_siswa);

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

        public static List<MailJobs> GetDataTerkirim_Entity()
        {
            List<MailJobs> hasil = new List<MailJobs>();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SUKSES;

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

        public static List<MailJobs> GetDataGagalKirim_Entity()
        {
            List<MailJobs> hasil = new List<MailJobs>();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_GAGAL;

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

        public static DateTime GetNow()
        {
            DateTime hasil = new DateTime();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "GETNOW";

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);

                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = Convert.ToDateTime(row[0]);
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

        public static void Insert(MailJobs m)
        {
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_INSERT;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_APLIKASI, m.Rel_Aplikasi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KETERANGAN, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SENDER, m.Sender));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SMTP, m.SMTP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PORT, m.Port));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PWD, m.PWD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SENDER_NAME, m.SenderName));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TUJUAN, m.Tujuan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TO_PERSON, m.ToPerson));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SUBJEK, m.Subjek));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.MESSAGE, m.Message));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TANGGAL, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.STATUS, m.Status));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_DONE, m.IsDone));

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

        public static void InsertLengkap(MailJobs m)
        {
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_INSERT_LENGKAP;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_APLIKASI, m.Rel_Aplikasi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KETERANGAN, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SENDER, m.Sender));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SMTP, m.SMTP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PORT, m.Port));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PWD, m.PWD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SENDER_NAME, m.SenderName));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TUJUAN, m.Tujuan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TO_PERSON, m.ToPerson));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SUBJEK, m.Subjek));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.MESSAGE, m.Message));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TANGGAL, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.STATUS, m.Status));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_DONE, m.IsDone));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.USER_ID_SENDER, m.UserIDSender));
                if (m.LinkExpiredDate == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.LINK_EXPIRED_DATE, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.LINK_EXPIRED_DATE, m.LinkExpiredDate));
                }

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

        public static void UpdateIsDone(Guid kode, bool isdone)
        {
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE_IS_DONE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_DONE, isdone));

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

        public static void UpdateStatus(Guid kode, string status)
        {
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE_STATUS;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.STATUS, status));

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

        public static void UpdateSMTP(Guid kode, string sender, string smtp, string port, string pwd, string sender_name)
        {
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE_SMTP;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SENDER, sender));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SMTP, smtp));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PORT, port));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PWD, pwd));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SENDER_NAME, sender_name));

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