using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SD;

namespace AI_ERP.Application_DAOs.Elearning.SD
{
    public static class DAO_Rapor_Desain
    {
        public const string SP_SELECT_ALL = "SD_Rapor_Desain_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SD_Rapor_Desain_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_JENISRAPOR = "SD_Rapor_Desain_SELECT_ALL_BY_JENISRAPOR";
        public const string SP_SELECT_ALL_FOR_SEARCH_BY_JENISRAPOR = "SD_Rapor_Desain_SELECT_ALL_FOR_SEARCH_BY_JENISRAPOR";
        public const string SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS = "SD_Rapor_Desain_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS";
        public const string SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER = "SD_Rapor_Desain_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER";
        public const string SP_SELECT_BY_ID = "SD_Rapor_Desain_SELECT_BY_ID";

        public const string SP_INSERT = "SD_Rapor_Desain_INSERT";

        public const string SP_UPDATE = "SD_Rapor_Desain_UPDATE";

        public const string SP_DELETE = "SD_Rapor_Desain_DELETE";

        public static class JenisRapor
        {
            public const string LTS = "LTS";
            public const string Semester = "Semester";
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string JenisRapor = "JenisRapor";
        }

        private static Rapor_Desain GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_Desain
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Kelas = row[NamaField.Rel_Kelas].ToString(),
                JenisRapor = row[NamaField.JenisRapor].ToString()
            };
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

        public static void Insert(Rapor_Desain m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisRapor, m.JenisRapor));
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

        public static Rapor_Desain GetByID_Entity(
                string kode
            )
        {
            Rapor_Desain hasil = new Rapor_Desain();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
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

        public static List<Rapor_Desain> GetByTABySMByKelas_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas_det,
                string jenis_rapor
            )
        {
            List<Rapor_Desain> hasil = new List<Rapor_Desain>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {

                        conn.Open();
                        comm.CommandTimeout = 1200;
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS;
                        comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                        comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                        comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, m_kelas_det.Rel_Kelas.ToString());

                        DataTable dtResult = new DataTable();
                        sqlDA = new SqlDataAdapter(comm);
                        sqlDA.Fill(dtResult);
                        foreach (DataRow row in dtResult.Rows)
                        {
                            hasil.Add(GetEntityFromDataRow(row));
                        }

                        hasil = hasil.FindAll(m => m.JenisRapor == jenis_rapor).ToList();

                    }
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

        public static List<Rapor_Desain> GetByTABySM_Entity(
                string tahun_ajaran,
                string semester
            )
        {
            List<Rapor_Desain> hasil = new List<Rapor_Desain>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);

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

        public static void Update(Rapor_Desain m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisRapor, m.JenisRapor));
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