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
    public class SiswaBiayaDanDibayar : SiswaBiaya
    {
        public decimal PersenDenda { get; set; }
        public decimal Dibayar { get; set; }
        public decimal DendaDibayar { get; set; }
    }

    public static class DAO_SiswaBiaya
    {
        public const string SP_SELECT_ALL = "SiswaBiaya_SELECT_ALL";
        public const string SP_SELECT_ALL_AS_TAGIHAN = "SiswaBiaya_SELECT_ALL_AS_TAGIHAN";
        public const string SP_SELECT_ALL_AS_TAGIHAN_SHOW_ALL = "SiswaBiaya_SELECT_ALL_AS_TAGIHAN_SHOW_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SiswaBiaya_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "SiswaBiaya_SELECT_BY_ID";
        public const string SP_SELECT_BY_SISWA_BY_TAHUN_AJARAN = "SiswaBiaya_SELECT_BY_SISWA_BY_TAHUN_AJARAN";
        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN_BY_SISWA = "SiswaBiaya_SELECT_DISTINCT_TAHUN_AJARAN_BY_SISWA";
        public const string SP_SELECT_BY_SISWA_BY_TAHUN_AJARAN_BY_BIAYA = "SiswaBiaya_SELECT_BY_SISWA_BY_TAHUN_AJARAN_BY_BIAYA";
        public const string SP_SELECT_FOR_CREATE = "Biaya_SELECT_FOR_CREATE";

        public const string SP_INSERT = "SiswaBiaya_INSERT";

        public const string SP_UPDATE = "SiswaBiaya_UPDATE";
        public const string SP_UPDATE_DENDA = "SiswaBiaya_UPDATE_DENDA";
        public const string SP_UPDATE_DENDA_BY_SISWA_BY_ITEMBIAYA_BY_TAHUNAJARAN_BY_KETERANGAN = "SiswaBiaya_UPDATE_DENDA_BY_SISWA_BY_ITEMBIAYA_BY_TAHUNAJARAN_BY_KETERANGAN";
        public const string SP_UPDATE_FROM_BIODATA = "SiswaBiaya_UPDATE_FROM_BIODATA";
        public const string SP_UPDATE_BEBAS_BIAYA = "SiswaBiaya_UPDATE_BEBAS_BIAYA";
        public const string SP_UPDATE_BEBAS_BIAYA_DENDA = "SiswaBiaya_UPDATE_BEBAS_BIAYA_DENDA";

        public const string SP_DELETE = "SiswaBiaya_DELETE";

        public const string SP_IS_ADA_BIAYA_BY_TAHUN_AJARAN = "SiswaBiaya_IS_ADA_BIAYA_BY_TAHUN_AJARAN";

        public static class NamaField
        {
            public const string KODE = "Kode";
            public const string TAHUN_AJARAN = "TahunAjaran";
            public const string REL_SEKOLAH = "Rel_Sekolah";
            public const string REL_SISWA = "Rel_Siswa";
            public const string REL_KELAS = "Rel_Kelas";
            public const string REL_KELAS_DET = "Rel_KelasDet";
            public const string REL_ITEM_BIAYA = "Rel_ItemBiaya";
            public const string PERIODE_TAGIH = "PeriodeTagih";
            public const string JUMLAH = "Jumlah";
            public const string DENDA = "Denda";
            public const string PERSEN_DENDA = "PersenDenda";
            public const string DIBAYAR = "Dibayar";
            public const string DENDA_DIBAYAR = "DendaDibayar";
            public const string KETERANGAN = "Keterangan";
            public const string IS_BEBAS = "IsBebas";
            public const string IS_BEBAS_DENDA = "IsBebasDenda";
            public const string URUT = "Urut";

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

            public static int TAHUN_AJARAN__MaxLength { get { return GetMaxLength(TAHUN_AJARAN); } }
            public static int REL_SISWA__MaxLength { get { return GetMaxLength(REL_SISWA); } }
            public static int KETERANGAN__MaxLength { get { return GetMaxLength(KETERANGAN); } }
        }

        private static SiswaBiaya GetEntityFromDataRow(DataRow row)
        {
            SiswaBiaya hasil = new SiswaBiaya();

            hasil.Kode = new Guid(row[NamaField.KODE].ToString());
            hasil.TahunAjaran = row[NamaField.TAHUN_AJARAN].ToString();
            hasil.Rel_Siswa = row[NamaField.REL_SISWA].ToString();
            hasil.Rel_Sekolah = row[NamaField.REL_SEKOLAH].ToString();
            hasil.Rel_Kelas = row[NamaField.REL_KELAS].ToString();
            hasil.Rel_KelasDet = row[NamaField.REL_KELAS_DET].ToString();
            hasil.Rel_ItemBiaya = new Guid(row[NamaField.REL_ITEM_BIAYA].ToString());
            hasil.PeriodeTagih = Convert.ToInt32(row[NamaField.PERIODE_TAGIH]);
            hasil.Jumlah = Convert.ToDecimal(row[NamaField.JUMLAH].ToString());
            hasil.Denda = Convert.ToDecimal((row[NamaField.DENDA].ToString().Trim() == "" ? "0" : row[NamaField.DENDA].ToString()));
            hasil.Keterangan = row[NamaField.KETERANGAN].ToString();
            hasil.IsBebas = (row[NamaField.IS_BEBAS] != DBNull.Value ? Convert.ToBoolean(row[NamaField.IS_BEBAS]) : false);
            hasil.IsBebasDenda = (row[NamaField.IS_BEBAS_DENDA] != DBNull.Value ? Convert.ToBoolean(row[NamaField.IS_BEBAS_DENDA]) : false);
            hasil.Urut = Convert.ToInt16(row[NamaField.URUT].ToString());

            return hasil;
        }

        private static SiswaBiayaDanDibayar GetEntityFromDataRowDibayar(DataRow row)
        {
            SiswaBiayaDanDibayar hasil = new SiswaBiayaDanDibayar();

            hasil.Kode = new Guid(row[NamaField.KODE].ToString());
            hasil.TahunAjaran = row[NamaField.TAHUN_AJARAN].ToString();
            hasil.Rel_Siswa = row[NamaField.REL_SISWA].ToString();
            hasil.Rel_Sekolah = row[NamaField.REL_SEKOLAH].ToString();
            hasil.Rel_Kelas = row[NamaField.REL_KELAS].ToString();
            hasil.Rel_KelasDet = row[NamaField.REL_KELAS_DET].ToString();
            hasil.Rel_ItemBiaya = new Guid(row[NamaField.REL_ITEM_BIAYA].ToString());
            hasil.PeriodeTagih = Convert.ToInt32(row[NamaField.PERIODE_TAGIH]);
            hasil.Jumlah = Convert.ToDecimal(row[NamaField.JUMLAH].ToString());
            hasil.Denda = Convert.ToDecimal((row[NamaField.DENDA].ToString().Trim() == "" ? "0" : row[NamaField.DENDA].ToString()));
            hasil.PersenDenda = Convert.ToDecimal((row[NamaField.PERSEN_DENDA].ToString().Trim() == "" ? "0" : row[NamaField.PERSEN_DENDA].ToString()));
            hasil.Dibayar = Convert.ToDecimal((row[NamaField.DIBAYAR].ToString().Trim() == "" ? "0" : row[NamaField.DIBAYAR].ToString()));
            hasil.DendaDibayar = Convert.ToDecimal((row[NamaField.DENDA_DIBAYAR].ToString().Trim() == "" ? "0" : row[NamaField.DENDA_DIBAYAR].ToString()));
            hasil.Keterangan = row[NamaField.KETERANGAN].ToString();
            hasil.IsBebas = (row[NamaField.IS_BEBAS] != DBNull.Value ? Convert.ToBoolean(row[NamaField.IS_BEBAS]) : false);
            hasil.IsBebasDenda = (row[NamaField.IS_BEBAS_DENDA] != DBNull.Value ? Convert.ToBoolean(row[NamaField.IS_BEBAS_DENDA]) : false);
            hasil.Urut = Convert.ToInt16(row[NamaField.URUT].ToString());

            return hasil;
        }

        public static List<SiswaBiaya> GetAll_Entity()
        {
            List<SiswaBiaya> hasil = new List<SiswaBiaya>();
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

        public static List<SiswaBiayaDanDibayar> GetBySiswaByTahunAjaran_Entity(string rel_siswa, string tahun_ajaran, DateTime tanggal_tagih)
        {
            List<SiswaBiayaDanDibayar> hasil = new List<SiswaBiayaDanDibayar>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_SISWA_BY_TAHUN_AJARAN;
                comm.Parameters.AddWithValue("@" + NamaField.REL_SISWA, rel_siswa);
                comm.Parameters.AddWithValue("@" + NamaField.TAHUN_AJARAN, tahun_ajaran);
                comm.Parameters.AddWithValue("@TanggalTagih", tanggal_tagih);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRowDibayar(row));
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

        public static bool IsAdaBiayaBySiswaByTahunAjaran_Entity(string rel_siswa, string tahun_ajaran)
        {
            bool hasil = false;
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_IS_ADA_BIAYA_BY_TAHUN_AJARAN;
                comm.Parameters.AddWithValue("@" + NamaField.REL_SISWA, rel_siswa);
                comm.Parameters.AddWithValue("@" + NamaField.TAHUN_AJARAN, tahun_ajaran);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = Convert.ToBoolean(row[0]);
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

        public static List<SiswaBiayaDanDibayar> GetAllAsTagihan_Entity(DateTime tanggal_tagih, string rel_siswa)
        {
            List<SiswaBiayaDanDibayar> hasil = new List<SiswaBiayaDanDibayar>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_AS_TAGIHAN;
                comm.Parameters.AddWithValue("@TanggalTagih", tanggal_tagih);
                comm.Parameters.AddWithValue("@" + NamaField.REL_SISWA, rel_siswa);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRowDibayar(row));
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

        public static List<SiswaBiayaDanDibayar> GetAllAsTagihanShowAll_Entity(DateTime tanggal_tagih, string rel_siswa)
        {
            List<SiswaBiayaDanDibayar> hasil = new List<SiswaBiayaDanDibayar>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_AS_TAGIHAN_SHOW_ALL;
                comm.Parameters.AddWithValue("@TanggalTagih", tanggal_tagih);
                comm.Parameters.AddWithValue("@" + NamaField.REL_SISWA, rel_siswa);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRowDibayar(row));
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

        public static List<string> GetDistinctTahunAjaranBySiswa(string rel_siswa)
        {
            List<string> hasil = new List<string>();
            if (rel_siswa == null) return hasil;

            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_TAHUN_AJARAN_BY_SISWA;
                comm.Parameters.AddWithValue("@" + NamaField.REL_SISWA, rel_siswa);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(row["TahunAjaran"].ToString());
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

        public static SiswaBiaya GetByID_Entity(string kode)
        {
            SiswaBiaya hasil = new SiswaBiaya();
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

        public static SiswaBiaya GetBySiswaByTahunAjaranByBiaya_Entity(string rel_siswa, string tahun_ajaran, Guid rel_itembiaya, string keterangan)
        {
            SiswaBiaya hasil = new SiswaBiaya();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_SISWA_BY_TAHUN_AJARAN_BY_BIAYA;
                comm.Parameters.AddWithValue("@" + NamaField.REL_SISWA, rel_siswa);
                comm.Parameters.AddWithValue("@" + NamaField.TAHUN_AJARAN, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.REL_ITEM_BIAYA, rel_itembiaya);
                comm.Parameters.AddWithValue("@" + NamaField.KETERANGAN, keterangan);

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

        public static void Insert(SiswaBiaya siswa_biaya)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.TAHUN_AJARAN, siswa_biaya.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_SISWA, siswa_biaya.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_SEKOLAH, siswa_biaya.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_KELAS, siswa_biaya.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_KELAS_DET, siswa_biaya.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_ITEM_BIAYA, siswa_biaya.Rel_ItemBiaya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PERIODE_TAGIH, siswa_biaya.PeriodeTagih));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JUMLAH, siswa_biaya.Jumlah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DENDA, siswa_biaya.Denda));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KETERANGAN, siswa_biaya.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_BEBAS, siswa_biaya.IsBebas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_BEBAS_DENDA, siswa_biaya.IsBebasDenda));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.URUT, siswa_biaya.Urut));
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

        public static void Update(SiswaBiaya siswa_biaya)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, siswa_biaya.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TAHUN_AJARAN, siswa_biaya.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_SISWA, siswa_biaya.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_SEKOLAH, siswa_biaya.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_KELAS, siswa_biaya.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_KELAS_DET, siswa_biaya.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_ITEM_BIAYA, siswa_biaya.Rel_ItemBiaya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PERIODE_TAGIH, siswa_biaya.PeriodeTagih));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JUMLAH, siswa_biaya.Jumlah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DENDA, siswa_biaya.Denda));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KETERANGAN, siswa_biaya.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_BEBAS, siswa_biaya.IsBebas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_BEBAS_DENDA, siswa_biaya.IsBebasDenda));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.URUT, siswa_biaya.Urut));
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

        public static void UpdateFromBiodata(Guid kode, Guid rel_itembiaya, decimal jumlah, string keterangan)
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
                comm.CommandText = SP_UPDATE_FROM_BIODATA;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_ITEM_BIAYA, rel_itembiaya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JUMLAH, jumlah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KETERANGAN, keterangan));
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

        public static void UpdateBebasBiaya(Guid kode, bool is_bebas)
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
                comm.CommandText = SP_UPDATE_BEBAS_BIAYA;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_BEBAS, is_bebas));
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

        public static void UpdateBebasBiayaDenda(Guid kode, bool is_bebas)
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
                comm.CommandText = SP_UPDATE_BEBAS_BIAYA_DENDA;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IS_BEBAS_DENDA, is_bebas));
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

        public static void UpdateDendaBySiswaByItemBiayaByTahunAjaranByKeterangan(
                string rel_siswa, Guid rel_itembiaya, string tahun_ajaran, string keterangan, decimal denda
            )
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
                comm.CommandText = SP_UPDATE_DENDA_BY_SISWA_BY_ITEMBIAYA_BY_TAHUNAJARAN_BY_KETERANGAN;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_SISWA, rel_siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_ITEM_BIAYA, rel_itembiaya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TAHUN_AJARAN, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KETERANGAN, keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DENDA, denda));
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

        public static void UpdateDenda(
                Guid kode, decimal denda
            )
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
                comm.CommandText = SP_UPDATE_DENDA;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DENDA, denda));
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

        public static string GetKeteranganBiayaBulananFromNamaBiaya(string nama)
        {
            nama = nama.ToLower().Replace("uang ", "u.").Replace("uang", "u.").ToLower();
            return nama;
        }

        public static List<string> GenerateKeteranganBiayaBulanan(Guid kode_biaya)
        {
            List<string> lst = new List<string>();
            lst.Clear();

            ItemBiaya item_biaya = DAO_ItemBiaya.GetByID_Entity(kode_biaya.ToString());
            if (item_biaya != null)
            {
                foreach (string bulan in Application_Libs.Libs.Array_Bulan_Tahun_Ajaran)
                {
                    lst.Add(
                            GetKeteranganBiayaBulananFromNamaBiaya(item_biaya.Nama).Trim() + " " +
                            bulan.ToLower()
                        );
                }
            }

            return lst;
        }

        public static int GetPeriodeTagih(string bulan, string tahun_ajaran)
        {
            int hasil = 0;

            if (tahun_ajaran.Length == 9)
            {
                int id = 1;
                foreach (string item in Application_Libs.Libs.Array_Bulan)
                {
                    if (item.Trim().ToLower() == bulan.Trim().ToLower())
                    {
                        int tahun1 = int.Parse(tahun_ajaran.Substring(0, 4));
                        int tahun2 = int.Parse(tahun_ajaran.Substring(5, 4));

                        if (id <= 6)
                        {
                            hasil = (tahun2 * 100) + id;
                        }
                        else
                        {
                            hasil = (tahun1 * 100) + id;
                        }
                        break;
                    }
                    id++;
                }
            }

            return hasil;
        }

        public static List<BiayaDet> GetListBiaya(string tahun_ajaran, string rel_sekolah, string rel_kelas, string rel_siswa)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            List<BiayaDet> lst_biaya = new List<BiayaDet>();
            lst_biaya.Clear();

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_FOR_CREATE;

                comm.Parameters.Add(new SqlParameter("@TahunAjaran", tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@Rel_Sekolah", rel_sekolah));
                comm.Parameters.Add(new SqlParameter("@Rel_Kelas", rel_kelas));
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", rel_siswa));

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);

                foreach (DataRow row in dtResult.Rows)
                {
                    lst_biaya.Add(DAO_BiayaDet.GetEntityFromDataRow(row));
                }

                return lst_biaya;

            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static void Create(
            string rel_siswa,
            bool is_siswa_internal,
            string tahun_ajaran,
            string rel_sekolah,
            string rel_kelas,
            Application_Libs.Libs.BulanTahunAjaran mulai_bulan,
            bool hanya_bulanan = false
        )
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Keu();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_FOR_CREATE;

                comm.Parameters.Add(new SqlParameter("@TahunAjaran", tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@Rel_Sekolah", rel_sekolah));
                comm.Parameters.Add(new SqlParameter("@Rel_Kelas", rel_kelas));
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", rel_siswa));

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);

                int urut = 1;
                foreach (DataRow row in dtResult.Rows)
                {
                    string rel_item_biaya = row["Rel_ItemBiaya"].ToString();
                    ItemBiaya item_biaya = DAO_ItemBiaya.GetByID_Entity(rel_item_biaya);
                    Siswa m_siswa = DAO_Siswa.GetByID_Entity("_", "_", rel_siswa);

                    bool is_dalam = (row["IsSiswaDalam"] == DBNull.Value ? false : Convert.ToBoolean(row["IsSiswaDalam"]));
                    bool is_luar = (row["IsSiswaLuar"] == DBNull.Value ? false : Convert.ToBoolean(row["IsSiswaLuar"]));
                    bool is_laki_laki = (row["IsLakiLaki"] == DBNull.Value ? false : Convert.ToBoolean(row["IsLakiLaki"]));
                    bool is_perempuan = (row["IsPerempuan"] == DBNull.Value ? false : Convert.ToBoolean(row["IsPerempuan"]));

                    if (item_biaya != null && m_siswa != null)
                    {
                        if (item_biaya.IsBulanan)
                        {
                            int id_bulan = 0;
                            List<string> lst_biaya_bulanan = GenerateKeteranganBiayaBulanan(item_biaya.Kode);
                            if (lst_biaya_bulanan.Count == Application_Libs.Libs.Array_Bulan_Tahun_Ajaran.Length)
                            {
                                foreach (string keterangan_biaya in lst_biaya_bulanan)
                                {
                                    if (id_bulan >= (int)mulai_bulan)
                                    {
                                        bool b_valid = false;
                                        if (is_dalam && is_siswa_internal && (
                                                (is_laki_laki && m_siswa.JenisKelamin.ToUpper() == "L") ||
                                                (is_perempuan && m_siswa.JenisKelamin.ToUpper() == "P")
                                            ))
                                        {
                                            b_valid = true;
                                        }
                                        else if (is_luar && !is_siswa_internal && (
                                                (is_laki_laki && m_siswa.JenisKelamin.ToUpper() == "L") ||
                                                (is_perempuan && m_siswa.JenisKelamin.ToUpper() == "P")
                                            ))
                                        {
                                            b_valid = true;
                                        }
                                        if (b_valid)
                                        {
                                            SiswaBiaya siswa_biaya = new SiswaBiaya();
                                            siswa_biaya.TahunAjaran = row["TahunAjaran"].ToString();
                                            siswa_biaya.Rel_Siswa = m_siswa.NIS;
                                            siswa_biaya.Rel_Sekolah = m_siswa.Rel_Sekolah;
                                            siswa_biaya.Rel_Kelas = m_siswa.Rel_Kelas;
                                            siswa_biaya.Rel_KelasDet = m_siswa.Rel_KelasDet;
                                            siswa_biaya.Rel_ItemBiaya = item_biaya.Kode;
                                            siswa_biaya.PeriodeTagih = GetPeriodeTagih(Application_Libs.Libs.Array_Bulan_Tahun_Ajaran[id_bulan], row["TahunAjaran"].ToString());
                                            siswa_biaya.Jumlah = Convert.ToDecimal(row["Jumlah"]);
                                            siswa_biaya.Denda = 0;
                                            siswa_biaya.Keterangan = keterangan_biaya.Replace("|", "").Replace(";", "");
                                            siswa_biaya.IsBebas = false;
                                            siswa_biaya.IsBebasDenda = false;
                                            siswa_biaya.Urut = urut;

                                            DAO_SiswaBiaya.Insert(siswa_biaya);
                                            id_bulan++;
                                            urut++;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!hanya_bulanan)
                            {
                                bool b_valid = false;
                                if (is_dalam && is_siswa_internal && (
                                                    (is_laki_laki && m_siswa.JenisKelamin.ToUpper() == "L") ||
                                                    (is_perempuan && m_siswa.JenisKelamin.ToUpper() == "P")
                                                ))
                                {
                                    b_valid = true;
                                }
                                else if (is_luar && !is_siswa_internal && (
                                        (is_laki_laki && m_siswa.JenisKelamin.ToUpper() == "L") ||
                                        (is_perempuan && m_siswa.JenisKelamin.ToUpper() == "P")
                                    ))
                                {
                                    b_valid = true;
                                }
                                if (b_valid)
                                {
                                    SiswaBiaya siswa_biaya = new SiswaBiaya();
                                    siswa_biaya.TahunAjaran = row["TahunAjaran"].ToString();
                                    siswa_biaya.Rel_Siswa = m_siswa.NIS;
                                    siswa_biaya.Rel_Sekolah = m_siswa.Rel_Sekolah;
                                    siswa_biaya.Rel_Kelas = m_siswa.Rel_Kelas;
                                    siswa_biaya.Rel_KelasDet = m_siswa.Rel_KelasDet;
                                    siswa_biaya.Rel_ItemBiaya = item_biaya.Kode;
                                    siswa_biaya.PeriodeTagih = GetPeriodeTagih(mulai_bulan.ToString(), row["TahunAjaran"].ToString());
                                    siswa_biaya.Jumlah = Convert.ToDecimal(row["Jumlah"]);
                                    siswa_biaya.Denda = 0;
                                    siswa_biaya.Keterangan = item_biaya.Nama.ToLower().Trim().Replace("|", "").Replace(";", "");
                                    siswa_biaya.IsBebas = false;
                                    siswa_biaya.IsBebasDenda = false;
                                    siswa_biaya.Urut = urut;

                                    DAO_SiswaBiaya.Insert(siswa_biaya);
                                    urut++;
                                }
                            }
                        }
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
        }
    }
}