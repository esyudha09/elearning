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
    public static class DAO_Rapor_StrukturNilai_Deskripsi
    {
        public const string SP_SELECT_BY_ID = "SMA_Rapor_StrukturNilai_Deskripsi_SELECT_BY_ID";
        public const string SP_SELECT_BY_TA_BY_SM_BY_Kelas = "SMA_Rapor_StrukturNilai_Deskripsi_SELECT_BY_TA_BY_SM_BY_Kelas";
        public const string SP_SELECT_BY_TA_BY_SM_BY_Kelas_BY_Mapel = "SMA_Rapor_StrukturNilai_Deskripsi_SELECT_BY_TA_BY_SM_BY_Kelas_BY_Mapel";

        public const string SP_INSERT = "SMA_Rapor_StrukturNilai_Deskripsi_INSERT";

        public const string SP_UPDATE = "SMA_Rapor_StrukturNilai_Deskripsi_UPDATE";

        public const string SP_SAVE = "SMA_Rapor_StrukturNilai_Deskripsi_SAVE";

        public const string SP_DELETE = "SMA_Rapor_StrukturNilai_Deskripsi_DELETE";
        public const string SP_DELETE_BY_StrukturNilai_BY_KelasDet = "SMA_Rapor_StrukturNilai_Deskripsi_DELETE_BY_StrukturNilai_BY_KelasDet";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_StrukturNilai = "Rel_StrukturNilai";
            public const string Rel_StrukturNilai_AP = "Rel_StrukturNilai_AP";
            public const string Rel_StrukturNilai_KD = "Rel_StrukturNilai_KD";
            public const string Rel_StrukturNilai_KP = "Rel_StrukturNilai_KP";
            public const string Deskripsi = "Deskripsi";
        }

        private static Rapor_StrukturNilai_Deskripsi GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_StrukturNilai_Deskripsi
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_StrukturNilai = row[NamaField.Rel_StrukturNilai].ToString(),
                Rel_StrukturNilai_AP = row[NamaField.Rel_StrukturNilai_AP].ToString(),
                Rel_StrukturNilai_KD = row[NamaField.Rel_StrukturNilai_KD].ToString(),
                Rel_StrukturNilai_KP = row[NamaField.Rel_StrukturNilai_KP].ToString(),
                Deskripsi = row[NamaField.Deskripsi].ToString()
            };
        }

        public static void Save(Rapor_StrukturNilai_Deskripsi m)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai, m.Rel_StrukturNilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_AP, m.Rel_StrukturNilai_AP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_KD, m.Rel_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_KP, m.Rel_StrukturNilai_KP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, m.Deskripsi));
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

        public static void Insert(Rapor_StrukturNilai_Deskripsi m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai, m.Rel_StrukturNilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_AP, m.Rel_StrukturNilai_AP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_KD, m.Rel_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_KP, m.Rel_StrukturNilai_KP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, m.Deskripsi));
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

        public static void Update(Rapor_StrukturNilai_Deskripsi m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai, m.Rel_StrukturNilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_AP, m.Rel_StrukturNilai_AP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_KD, m.Rel_StrukturNilai_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_StrukturNilai_KP, m.Rel_StrukturNilai_KP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, m.Deskripsi));
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
        
        public static Rapor_StrukturNilai_Deskripsi GetByID_Entity(string kode)
        {
            Rapor_StrukturNilai_Deskripsi hasil = new Rapor_StrukturNilai_Deskripsi();
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

        public static List<Rapor_StrukturNilai_Deskripsi> GetAllByTABySMByKelas_Entity(
                string tahun_ajaran, string semester, string rel_kelas
            )
        {
            List<Rapor_StrukturNilai_Deskripsi> hasil = new List<Rapor_StrukturNilai_Deskripsi>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_Kelas;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));
                comm.Parameters.Add(new SqlParameter("@Rel_Kelas", rel_kelas));

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

        public static List<Rapor_StrukturNilai_Deskripsi> GetAllByTABySMByKelasByMapel_Entity(
                string tahun_ajaran, string semester, string rel_kelas, string rel_mapel
            )
        {
            List<Rapor_StrukturNilai_Deskripsi> hasil = new List<Rapor_StrukturNilai_Deskripsi>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_Kelas_BY_Mapel;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));
                comm.Parameters.Add(new SqlParameter("@Rel_Kelas", rel_kelas));
                comm.Parameters.Add(new SqlParameter("@Rel_Mapel", rel_mapel));

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
    }
}