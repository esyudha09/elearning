using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.SD;

namespace AI_ERP.Application_DAOs.Elearning.SD
{
    public static class DAO_Rapor_StrukturNilai_KURTILAS_KPKelompok
    {
        public const string SP_SELECT_ALL = "SD_Rapor_StrukturNilai_KURTILAS_KPKelompok_SELECT_ALL";
        public const string SP_SELECT_ALL_BY_HEADER = "SD_Rapor_StrukturNilai_KURTILAS_KPKelompok_SELECT_ALL_BY_HEADER";
        public const string SP_SELECT_BY_ID = "SD_Rapor_StrukturNilai_KURTILAS_KPKelompok_SELECT_BY_ID";

        public const string SP_INSERT = "SD_Rapor_StrukturNilai_KURTILAS_KPKelompok_INSERT";

        public const string SP_UPDATE = "SD_Rapor_StrukturNilai_KURTILAS_KPKelompok_UPDATE";

        public const string SP_DELETE = "SD_Rapor_StrukturNilai_KURTILAS_KPKelompok_DELETE";
        public const string SP_DELETE_BY_HEADER = "SD_Rapor_StrukturNilai_KURTILAS_KPKelompok_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_StrukturNilai = "Rel_Rapor_StrukturNilai";
            public const string Rel_Rapor_KomponenPenilaian = "Rel_Rapor_KomponenPenilaian";
            public const string Bobot = "Bobot";
            public const string Urutan = "Urutan";
        }

        private static Rapor_StrukturNilai_KURTILAS_KPKelompok GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_StrukturNilai_KURTILAS_KPKelompok
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_StrukturNilai = new Guid(row[NamaField.Rel_Rapor_StrukturNilai].ToString()),
                Rel_Rapor_KomponenPenilaian = new Guid(row[NamaField.Rel_Rapor_KomponenPenilaian].ToString()),
                Bobot = Convert.ToDecimal(row[NamaField.Bobot]),
                Urutan = Convert.ToInt16(row[NamaField.Urutan])
            };
        }

        public static List<Rapor_StrukturNilai_KURTILAS_KPKelompok> GetAll_Entity()
        {
            List<Rapor_StrukturNilai_KURTILAS_KPKelompok> hasil = new List<Rapor_StrukturNilai_KURTILAS_KPKelompok>();
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

        public static List<Rapor_StrukturNilai_KURTILAS_KPKelompok> GetAllByHeader_Entity(string rel_rapor_StrukturNilai)
        {
            List<Rapor_StrukturNilai_KURTILAS_KPKelompok> hasil = new List<Rapor_StrukturNilai_KURTILAS_KPKelompok>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_StrukturNilai, rel_rapor_StrukturNilai);

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

        public static Rapor_StrukturNilai_KURTILAS_KPKelompok GetByID_Entity(string kode)
        {
            Rapor_StrukturNilai_KURTILAS_KPKelompok hasil = new Rapor_StrukturNilai_KURTILAS_KPKelompok();
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

        public static void DeleteByHeader(string rel_StrukturNilai)
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
                comm.CommandText = SP_DELETE_BY_HEADER;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai, rel_StrukturNilai));
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

        public static void Insert(Rapor_StrukturNilai_KURTILAS_KPKelompok m)
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

                if (m.Kode.ToString() == Application_Libs.Constantas.GUID_NOL) m.Kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai, m.Rel_Rapor_StrukturNilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KomponenPenilaian, m.Rel_Rapor_KomponenPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Bobot, m.Bobot));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urutan, m.Urutan));
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

        public static void Update(Rapor_StrukturNilai_KURTILAS_KPKelompok m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai, m.Rel_Rapor_StrukturNilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KomponenPenilaian, m.Rel_Rapor_KomponenPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Bobot, m.Bobot));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urutan, m.Urutan));
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