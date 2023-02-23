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
    public static class DAO_ItemBiaya
    {
        public const string SP_SELECT_ALL = "ItemBiaya_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "ItemBiaya_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "ItemBiaya_SELECT_BY_ID";
        public const string SP_SELECT_BY_BIAYA_TERBUKA = "ItemBiaya_SELECT_BY_BIAYA_TERBUKA";

        public const string SP_INSERT = "ItemBiaya_INSERT";

        public const string SP_UPDATE = "ItemBiaya_UPDATE";

        public const string SP_DELETE = "ItemBiaya_DELETE";

        public static class NamaField
        {
            public const string KODE = "Kode";
            public const string NAMA = "Nama";
            public const string KETERANGAN = "Keterangan";
            public const string IS_BULANAN = "IsBulanan";
            public const string IS_BISA_DICICIL = "IsBisaDicicil";
            public const string IS_TOPUP_SMARTCARD = "IsTopUpSmartCard";
            public const string IS_BIAYA_TERBUKA = "IsBiayaTerbuka";
            public const string VA_MANDIRI = "VA_MANDIRI";
            public const string VA_PERMATA = "VA_PERMATA";
            public const string JUMLAH_DEFAULT_MANDIRI = "JumlahDefault_MANDIRI";
            public const string JUMLAH_DEFAULT_PERMATA = "JumlahDefault_PERMATA";
            public const string KODE_TAGIHAN_MANDIRI = "KodeTagihan_MANDIRI";
            public const string KODE_TAGIHAN_PERMATA = "KodeTagihan_PERMATA";
            public const string IS_TAGIH_VA_MANDIRI = "IsTagihVA_MANDIRI";
            public const string IS_TAGIH_VA_PERMATA = "IsTagihVA_PERMATA";

            private static int GetMaxLength(string nama_field)
            {
                return Application_Libs.Libs.GetDbColumnMaxLength
                        (
                            nama_field,
                            SP_SELECT_ALL,
                            new List<SqlParameter>() {
                                new SqlParameter(){
                                    ParameterName = "@" +  NamaField.KODE,
                                    Value = "@_@"
                                }
                            }
                        );
            }

            public static int NAMA__MaxLength { get { return GetMaxLength(NAMA); } }
            public static int KETERANGAN__MaxLength { get { return GetMaxLength(KETERANGAN); } }
            public static int VA_MANDIRI__MaxLength { get { return GetMaxLength(VA_MANDIRI); } }
            public static int VA_PERMATA__MaxLength { get { return GetMaxLength(VA_PERMATA); } }
        }

        private static ItemBiaya GetEntityFromDataRow(DataRow row)
        {
            return new ItemBiaya
            {
                Kode = new Guid(row[NamaField.KODE].ToString()),
                Nama = row[NamaField.NAMA].ToString(),
                Keterangan = row[NamaField.KETERANGAN].ToString(),
                IsBulanan = (row[NamaField.IS_BULANAN] != null && row[NamaField.IS_BULANAN] != DBNull.Value ? Convert.ToBoolean(row[NamaField.IS_BULANAN]) : false),
                IsBisaDicicil = (row[NamaField.IS_BISA_DICICIL] != null && row[NamaField.IS_BISA_DICICIL] != DBNull.Value ? Convert.ToBoolean(row[NamaField.IS_BISA_DICICIL]) : false),
                IsTopUpSmartCard = (row[NamaField.IS_TOPUP_SMARTCARD] != null && row[NamaField.IS_TOPUP_SMARTCARD] != DBNull.Value ? Convert.ToBoolean(row[NamaField.IS_TOPUP_SMARTCARD]) : false),
                IsBiayaTerbuka = (row[NamaField.IS_BIAYA_TERBUKA] != null && row[NamaField.IS_BIAYA_TERBUKA] != DBNull.Value ? Convert.ToBoolean(row[NamaField.IS_BIAYA_TERBUKA]) : false),
                VA_MANDIRI = row[NamaField.VA_MANDIRI].ToString(),
                VA_PERMATA = row[NamaField.VA_PERMATA].ToString(),
                JumlahDefault_MANDIRI = (row[NamaField.JUMLAH_DEFAULT_MANDIRI] != null && row[NamaField.JUMLAH_DEFAULT_MANDIRI] != DBNull.Value ? Convert.ToDecimal(row[NamaField.JUMLAH_DEFAULT_MANDIRI]) : 0),
                JumlahDefault_PERMATA = (row[NamaField.JUMLAH_DEFAULT_PERMATA] != null && row[NamaField.JUMLAH_DEFAULT_PERMATA] != DBNull.Value ? Convert.ToDecimal(row[NamaField.JUMLAH_DEFAULT_PERMATA]) : 0),
                KodeTagihan_MANDIRI = row[NamaField.KODE_TAGIHAN_MANDIRI].ToString(),
                KodeTagihan_PERMATA = row[NamaField.KODE_TAGIHAN_PERMATA].ToString(),
                IsTagihVA_MANDIRI = (row[NamaField.IS_TAGIH_VA_MANDIRI] != null && row[NamaField.IS_TAGIH_VA_MANDIRI] != DBNull.Value ? Convert.ToBoolean(row[NamaField.IS_TAGIH_VA_MANDIRI]) : false),
                IsTagihVA_PERMATA = (row[NamaField.IS_TAGIH_VA_PERMATA] != null && row[NamaField.IS_TAGIH_VA_PERMATA] != DBNull.Value ? Convert.ToBoolean(row[NamaField.IS_TAGIH_VA_PERMATA]) : false)
            };
        }

        public static List<ItemBiaya> GetAll_Entity()
        {
            List<ItemBiaya> hasil = new List<ItemBiaya>();
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

        public static List<ItemBiaya> GetByBiayaTerbuka_Entity(bool isterbuka)
        {
            List<ItemBiaya> hasil = new List<ItemBiaya>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_BIAYA_TERBUKA;
                comm.Parameters.AddWithValue("@" + NamaField.IS_BIAYA_TERBUKA, isterbuka);

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

        public static ItemBiaya GetByID_Entity(string kode)
        {
            ItemBiaya hasil = new ItemBiaya();
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

        public static void Insert(ItemBiaya item_biaya)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NAMA, item_biaya.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KETERANGAN, item_biaya.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_BULANAN, item_biaya.IsBulanan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_BISA_DICICIL, item_biaya.IsBisaDicicil));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_TOPUP_SMARTCARD, item_biaya.IsTopUpSmartCard));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_BIAYA_TERBUKA, item_biaya.IsBiayaTerbuka));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.VA_MANDIRI, item_biaya.VA_MANDIRI));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.VA_PERMATA, item_biaya.VA_PERMATA));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JUMLAH_DEFAULT_MANDIRI, item_biaya.JumlahDefault_MANDIRI));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JUMLAH_DEFAULT_PERMATA, item_biaya.JumlahDefault_PERMATA));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE_TAGIHAN_MANDIRI, item_biaya.KodeTagihan_MANDIRI));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE_TAGIHAN_PERMATA, item_biaya.KodeTagihan_PERMATA));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_TAGIH_VA_MANDIRI, item_biaya.IsTagihVA_MANDIRI));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_TAGIH_VA_PERMATA, item_biaya.IsTagihVA_PERMATA));
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

        public static void Update(ItemBiaya item_biaya)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, item_biaya.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NAMA, item_biaya.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KETERANGAN, item_biaya.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_BULANAN, item_biaya.IsBulanan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_BISA_DICICIL, item_biaya.IsBisaDicicil));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_TOPUP_SMARTCARD, item_biaya.IsTopUpSmartCard));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_BIAYA_TERBUKA, item_biaya.IsBiayaTerbuka));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.VA_MANDIRI, item_biaya.VA_MANDIRI));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.VA_PERMATA, item_biaya.VA_PERMATA));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JUMLAH_DEFAULT_MANDIRI, item_biaya.JumlahDefault_MANDIRI));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JUMLAH_DEFAULT_PERMATA, item_biaya.JumlahDefault_PERMATA));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE_TAGIHAN_MANDIRI, item_biaya.KodeTagihan_MANDIRI));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE_TAGIHAN_PERMATA, item_biaya.KodeTagihan_PERMATA));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_TAGIH_VA_MANDIRI, item_biaya.IsTagihVA_MANDIRI));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_TAGIH_VA_PERMATA, item_biaya.IsTagihVA_PERMATA));
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