using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.KB;

namespace AI_ERP.Application_DAOs.Elearning.KB
{
    public static class DAO_Rapor_NilaiStandar
    {
        public const string SP_SELECT_ALL = "KB_Rapor_NilaiStandar_SELECT_ALL";
        public const string SP_SELECT_BY_RAPOR_DESIGN_BY_KELAS_DET = "KB_Rapor_NilaiStandar_SELECT_BY_RAPOR_DESIGN_BY_KELAS_DET";
        public const string SP_SELECT_BY_RAPOR_DESIGN_BY_KELAS_DET_BY_MAPEL = "KB_Rapor_NilaiStandar_SELECT_BY_RAPOR_DESIGN_BY_KELAS_DET_BY_MAPEL";
        public const string SP_SELECT_BY_ID = "KB_Rapor_NilaiStandar_SELECT_BY_ID";

        public const string SP_INSERT = "KB_Rapor_NilaiStandar_INSERT";

        public const string SP_UPDATE = "KB_Rapor_NilaiStandar_UPDATE";

        public const string SP_DELETE = "KB_Rapor_NilaiStandar_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_Design = "Rel_Rapor_Design";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Rapor_Kriteria = "Rel_Rapor_Kriteria";
            public const string Rel_Mapel = "Rel_Mapel";
        }

        private static Rapor_NilaiStandar GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_NilaiStandar
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_Design = new Guid(row[NamaField.Rel_Rapor_Design].ToString()),
                Rel_KelasDet = new Guid(row[NamaField.Rel_KelasDet].ToString()),
                Rel_Rapor_Kriteria = new Guid(row[NamaField.Rel_Rapor_Kriteria].ToString()),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString()
            };
        }

        public static List<Rapor_NilaiStandar> GetAll_Entity()
        {
            List<Rapor_NilaiStandar> hasil = new List<Rapor_NilaiStandar>();
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

        public static List<Rapor_NilaiStandar> GetByRaporDesignByKelasDet(string rel_rapordesign, string rel_kelasdet)
        {
            List<Rapor_NilaiStandar> hasil = new List<Rapor_NilaiStandar>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_RAPOR_DESIGN_BY_KELAS_DET;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_Design, rel_rapordesign);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);

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

        public static List<Rapor_NilaiStandar> GetByRaporDesignByKelasDetByMapel(string rel_rapordesign, string rel_kelasdet, string rel_mapel)
        {
            List<Rapor_NilaiStandar> hasil = new List<Rapor_NilaiStandar>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_RAPOR_DESIGN_BY_KELAS_DET_BY_MAPEL;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_Design, rel_rapordesign);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Mapel, rel_mapel);

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

        public static Rapor_NilaiStandar GetByID_Entity(string kode)
        {
            Rapor_NilaiStandar hasil = new Rapor_NilaiStandar();
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

        public static void Insert(Rapor_NilaiStandar m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Design, m.Rel_Rapor_Design));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Kriteria, m.Rel_Rapor_Kriteria));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
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

        public static void Update(Rapor_NilaiStandar m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Design, m.Rel_Rapor_Design));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Kriteria, m.Rel_Rapor_Kriteria));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
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