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
    public static class DAO_BiayaDet
    {
        public const string SP_SELECT_ALL = "BiayaDet_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "BiayaDet_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "BiayaDet_SELECT_BY_ID";
        public const string SP_SELECT_BY_BIAYA = "BiayaDet_SELECT_BY_BIAYA";

        public const string SP_INSERT = "BiayaDet_INSERT";

        public const string SP_UPDATE = "BiayaDet_UPDATE";

        public const string SP_DELETE = "BiayaDet_DELETE";
        public const string SP_DELETE_BY_BIAYA = "BiayaDet_DELETE_BY_BIAYA";

        public static class NamaField
        {
            public const string KODE = "Kode";
            public const string REL_BIAYA = "Rel_Biaya";
            public const string REL_ITEMBIAYA = "Rel_ItemBiaya";
            public const string JUMLAH = "Jumlah";
            public const string KETERANGAN = "Keterangan";
            public const string IS_SISWA_DALAM = "IsSiswaDalam";
            public const string IS_SISWA_LUAR = "IsSiswaLuar";
            public const string IS_LAKI_LAKI = "IsLakiLaki";
            public const string IS_PEREMPUAN = "IsPerempuan";
            public const string URUTAN_BIAYA = "UrutanBiaya";
        }

        public static BiayaDet GetEntityFromDataRow(DataRow row)
        {
            return new BiayaDet
            {
                Kode = new Guid(row[NamaField.KODE].ToString()),
                Rel_Biaya = new Guid(row[NamaField.REL_BIAYA].ToString()),
                Rel_ItemBiaya = new Guid(row[NamaField.REL_ITEMBIAYA].ToString()),
                Jumlah = Convert.ToDecimal(row[NamaField.JUMLAH]),
                Keterangan = row[NamaField.KETERANGAN].ToString(),
                IsSiswaDalam = Convert.ToBoolean(row[NamaField.IS_SISWA_DALAM]),
                IsSiswaLuar = Convert.ToBoolean(row[NamaField.IS_SISWA_LUAR]),
                IsLakiLaki = Convert.ToBoolean(row[NamaField.IS_LAKI_LAKI]),
                IsPerempuan = Convert.ToBoolean(row[NamaField.IS_PEREMPUAN]),
                UrutanBiaya = Convert.ToInt16(row[NamaField.URUTAN_BIAYA])
            };
        }

        public static List<BiayaDet> GetByBiaya_Entity(string rel_biaya)
        {
            List<BiayaDet> hasil = new List<BiayaDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_BIAYA;
                comm.Parameters.AddWithValue("@Rel_Biaya", rel_biaya);

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

        public static List<BiayaDet> GetAll_Entity()
        {
            List<BiayaDet> hasil = new List<BiayaDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
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

        public static BiayaDet GetByID_Entity(string kode)
        {
            BiayaDet hasil = new BiayaDet();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
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

        public static void Delete(string Kode)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_DELETE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, Kode));
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
        public static void DeleteByBiaya(string rel_biaya)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_DELETE_BY_BIAYA;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_BIAYA, rel_biaya));
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

        public static void Insert(BiayaDet biaya_det)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_INSERT;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, biaya_det.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_BIAYA, biaya_det.Rel_Biaya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_ITEMBIAYA, biaya_det.Rel_ItemBiaya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JUMLAH, biaya_det.Jumlah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KETERANGAN, biaya_det.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_SISWA_DALAM, biaya_det.IsSiswaDalam));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_SISWA_LUAR, biaya_det.IsSiswaLuar));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_LAKI_LAKI, biaya_det.IsLakiLaki));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_PEREMPUAN, biaya_det.IsPerempuan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.URUTAN_BIAYA, biaya_det.UrutanBiaya));
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

        public static void Update(BiayaDet biaya_det)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, biaya_det.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_BIAYA, biaya_det.Rel_Biaya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_ITEMBIAYA, biaya_det.Rel_ItemBiaya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JUMLAH, biaya_det.Jumlah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KETERANGAN, biaya_det.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_SISWA_DALAM, biaya_det.IsSiswaDalam));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_SISWA_LUAR, biaya_det.IsSiswaLuar));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_LAKI_LAKI, biaya_det.IsLakiLaki));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_PEREMPUAN, biaya_det.IsPerempuan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.URUTAN_BIAYA, biaya_det.UrutanBiaya));
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