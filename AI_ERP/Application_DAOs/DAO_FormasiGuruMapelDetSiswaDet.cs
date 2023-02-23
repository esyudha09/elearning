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
    public static class DAO_FormasiGuruMapelDetSiswaDet
    {
        public const string SP_SELECT_ALL = "FormasiGuruMapelDetSiswaDet_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "FormasiGuruMapelDetSiswaDet_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "FormasiGuruMapelDetSiswaDet_SELECT_BY_HEADER";
        public const string SP_SELECT_SISWA_BY_TA_BY_SM_BY_MAPEL_BY_KELAS_DET = "FormasiGuruMapelDetSiswaDet_SELECT_SISWA_BY_TA_BY_SM_BY_MAPEL_BY_KELAS_DET";
        public const string SP_SELECT_SISWA_BY_TA_BY_SM_BY_MAPEL_BY_KELAS_DET_FOR_SEARCH = "FormasiGuruMapelDetSiswaDet_SELECT_SISWA_BY_TA_BY_SM_BY_MAPEL_BY_KELAS_DET_FOR_SEARCH";

        public const string SP_INSERT = "FormasiGuruMapelDetSiswaDet_INSERT";

        public const string SP_UPDATE = "FormasiGuruMapelDetSiswaDet_UPDATE";

        public const string SP_DELETE = "FormasiGuruMapelDetSiswaDet_DELETE";
        public const string SP_DELETE_BY_HEADER = "FormasiGuruMapelDetSiswaDet_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_FormasiGuruMapelDet = "Rel_FormasiGuruMapelDet";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Urutan = "Urutan";
        }

        public static FormasiGuruMapelDetSiswaDet GetEntityFromDataRow(DataRow row)
        {
            return new FormasiGuruMapelDetSiswaDet
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_FormasiGuruMapelDet = row[NamaField.Rel_FormasiGuruMapelDet].ToString(),
                Urutan = Application_Libs.Libs.GetStringToInteger(row[NamaField.Urutan].ToString())
            };
        }

        public static List<FormasiGuruMapelDetSiswaDet> GetAll_Entity()
        {
            List<FormasiGuruMapelDetSiswaDet> hasil = new List<FormasiGuruMapelDetSiswaDet>();
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

        public static FormasiGuruMapelDetSiswaDet GetByID_Entity(string kode)
        {
            FormasiGuruMapelDetSiswaDet hasil = new FormasiGuruMapelDetSiswaDet();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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

        public static void DeleteByHeader(string Kode_header)
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
                comm.CommandText = SP_DELETE_BY_HEADER;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_FormasiGuruMapelDet, Kode_header));
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

        public static void Insert(FormasiGuruMapelDetSiswaDet m)
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

                if (m.Kode.ToString() == Application_Libs.Constantas.GUID_NOL) m.Kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_FormasiGuruMapelDet, m.Rel_FormasiGuruMapelDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
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

        public static void Update(FormasiGuruMapelDetSiswaDet m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_FormasiGuruMapelDet, m.Rel_FormasiGuruMapelDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));

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

        public static List<FormasiGuruMapelDetSiswaDet> GetByHeader_Entity(string kode)
        {
            List<FormasiGuruMapelDetSiswaDet> hasil = new List<FormasiGuruMapelDetSiswaDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@Rel_FormasiGuruMapelDet", kode);

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

        public static List<Siswa> GetSiswaByTABySMByMapelByKelasDet_Entity(string tahun_ajaran, string semester, string rel_mapel, string rel_kelas_det)
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SISWA_BY_TA_BY_SM_BY_MAPEL_BY_KELAS_DET;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelas_det);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(DAO_Siswa.GetEntityFromDataRow(row));
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