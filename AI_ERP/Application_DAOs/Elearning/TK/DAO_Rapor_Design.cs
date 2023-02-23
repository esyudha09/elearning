using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.TK;

namespace AI_ERP.Application_DAOs.Elearning.TK
{
    public static class DAO_Rapor_Design
    {
        public const string SP_SELECT_ALL = "TK_Rapor_Design_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "TK_Rapor_Design_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_TIPE_RAPOR = "TK_Rapor_Design_SELECT_ALL_BY_TIPE_RAPOR";
        public const string SP_SELECT_ALL_BY_TIPE_RAPOR_FOR_SEARCH = "TK_Rapor_Design_SELECT_ALL_BY_TIPE_RAPOR_FOR_SEARCH";

        public const string SP_SELECT_BY_KELAS_BY_TAHUNAJARAN_BY_GURU_BY_MAPEL = "TK_Rapor_Design_SELECT_BY_KELAS_BY_TAHUNAJARAN_BY_GURU_BY_MAPEL";
        public const string SP_SELECT_BY_KELAS_BY_TAHUNAJARAN_BY_GURU_BY_MAPEL_FOR_SEARCH = "TK_Rapor_Design_SELECT_BY_KELAS_BY_TAHUNAJARAN_BY_GURU_BY_MAPEL_FOR_SEARCH";
        public const string SP_SELECT_BY_TIPE_RAPOR_BY_KELAS_BY_TAHUNAJARAN_BY_GURU_BY_MAPEL = "TK_Rapor_Design_SELECT_BY_TIPE_RAPOR_BY_KELAS_BY_TAHUNAJARAN_BY_GURU_BY_MAPEL";
        public const string SP_SELECT_BY_TIPE_RAPOR_BY_KELAS_BY_TAHUNAJARAN_BY_GURU_BY_MAPEL_FOR_SEARCH = "TK_Rapor_Design_SELECT_BY_TIPE_RAPOR_BY_KELAS_BY_TAHUNAJARAN_BY_GURU_BY_MAPEL_FOR_SEARCH";

        public const string SP_SELECT_BY_KELASDET_BY_TAHUNAJARAN = "TK_Rapor_Design_SELECT_BY_KELASDET_BY_TAHUNAJARAN";
        public const string SP_SELECT_BY_KELASDET_BY_TAHUNAJARAN_FOR_SEARCH = "TK_Rapor_Design_SELECT_BY_KELASDET_BY_TAHUNAJARAN_FOR_SEARCH";
        public const string SP_SELECT_BY_TIPE_RAPOR_BY_KELASDET_BY_TAHUNAJARAN = "TK_Rapor_Design_SELECT_BY_TIPE_RAPOR_BY_KELASDET_BY_TAHUNAJARAN";
        public const string SP_SELECT_BY_TIPE_RAPOR_BY_KELASDET_BY_TAHUNAJARAN_FOR_SEARCH = "TK_Rapor_Design_BY_TIPE_RAPOR_SELECT_BY_KELASDET_BY_TAHUNAJARAN_FOR_SEARCH";

        public const string SP_SELECT_BY_TAHUNAJARAN_BY_SEMESTER = "TK_Rapor_Design_SELECT_BY_TAHUNAJARAN_BY_SEMESTER";
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_BY_TIPERAPOR = "TK_Rapor_Design_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_BY_TIPERAPOR";
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_FOR_SEARCH = "TK_Rapor_Design_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_FOR_SEARCH";
        public const string SP_SELECT_BY_TIPE_RAPOR_BY_TAHUNAJARAN_BY_SEMESTER = "TK_Rapor_Design_SELECT_BY_TIPE_RAPOR_BY_TAHUNAJARAN_BY_SEMESTER";
        public const string SP_SELECT_BY_TIPE_RAPOR_BY_TAHUNAJARAN_BY_SEMESTER_FOR_SEARCH = "TK_Rapor_Design_SELECT_BY_TIPE_RAPOR_BY_TAHUNAJARAN_BY_SEMESTER_FOR_SEARCH";

        public const string SP_SELECT_NILAI_MAPEL_EKSKUL_BY_TAHUNAJARAN_BY_SEMESTER = "TK_Rapor_Design_SELECT_NILAI_MAPEL_EKSKUL_BY_TAHUNAJARAN_BY_SEMESTER";
        public const string SP_SELECT_NILAI_MAPEL_EKSKUL_BY_TAHUNAJARAN_BY_SEMESTER_FOR_SEARCH = "TK_Rapor_Design_SELECT_NILAI_MAPEL_EKSKUL_BY_TAHUNAJARAN_BY_SEMESTER_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "TK_Rapor_Design_SELECT_BY_ID";

        public const string SP_INSERT = "TK_Rapor_Design_INSERT";

        public const string SP_UPDATE = "TK_Rapor_Design_UPDATE";
        public const string SP_UPDATE_LOCKED = "TK_Rapor_Design_UPDATE_LOCKED";
        
        public const string SP_DELETE = "TK_Rapor_Design_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string IsLocked = "IsLocked";
            public const string TipeRapor = "TipeRapor";
        }

        private static Rapor_Design GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_Design
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Kelas = new Guid(row[NamaField.Rel_Kelas].ToString()),
                IsLocked = (row[NamaField.IsLocked] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsLocked])),
                TipeRapor = row[NamaField.TipeRapor].ToString()
            };
        }

        public static List<Rapor_Design> GetAll_Entity()
        {
            List<Rapor_Design> hasil = new List<Rapor_Design>();
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

        public static Rapor_Design GetByID_Entity(string kode)
        {
            Rapor_Design hasil = new Rapor_Design();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
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

        public static void Delete(string Kode, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));
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

        public static void Insert(Rapor_Design m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TipeRapor, m.TipeRapor));
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));
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

        public static void Update(Rapor_Design m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TipeRapor, m.TipeRapor));
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));
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

        public static void UpdatePosting(string kode, bool is_locked)
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
                comm.CommandText = SP_UPDATE_LOCKED;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsLocked, is_locked));
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