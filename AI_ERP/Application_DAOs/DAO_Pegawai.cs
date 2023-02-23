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
    public static class DAO_Pegawai
    {
        public const string SP_SELECT_ALL = "Pegawai_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "Pegawai_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_UNIT = "Pegawai_SELECT_ALL_BY_UNIT";
        public const string SP_SELECT_ALL_BY_UNIT_FOR_SEARCH = "Pegawai_SELECT_ALL_BY_UNIT_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_NAMA = "Pegawai_SELECT_ALL_BY_NAMA";
        public const string SP_SELECT_ALL_BY_UNIT_SIMPLE = "Pegawai_SELECT_ALL_BY_UNIT_SIMPLE";
        public const string SP_SELECT_ALL_BY_UNIT_FOR_SEARCH_SIMPLE = "Pegawai_SELECT_ALL_BY_UNIT_FOR_SEARCH_SIMPLE";
        public const string SP_SELECT_ALL_BY_NAMA_SIMPLE = "Pegawai_SELECT_ALL_BY_NAMA_SIMPLE";
        public const string SP_SELECT_BY_ID = "Pegawai_SELECT_BY_ID";
        public const string SP_SELECT_BY_UNIT = "Pegawai_SELECT_BY_UNIT";

        public const string SP_FormasiGuru_SELECT_COUNT_BY_GURU = "FormasiGuru_SELECT_COUNT_BY_GURU";

        public const string SP_INSERT = "Pegawai_INSERT";

        public const string SP_UPDATE = "Pegawai_UPDATE";

        public const string SP_DELETE = "Pegawai_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Nama = "Nama";
            public const string NamaRekening = "NamaRekening";
            public const string NamaKartu = "NamaKartu";
            public const string TanggalMasuk = "TanggalMasuk";
            public const string TanggalKeluar = "TanggalKeluar";
            public const string TempatLahir = "TempatLahir";
            public const string TanggalLahir = "TanggalLahir";
            public const string JenisKelamin = "JenisKelamin";
            public const string Rel_Jabatan = "Rel_Jabatan";
            public const string NoKTP = "NoKTP";
            public const string NoKK = "NoKK";
            public const string StatusPegawai = "StatusPegawai";
            public const string TempatTinggal = "TempatTinggal";
            public const string AlamatRumah = "AlamatRumah";
            public const string Kota = "Kota";
            public const string KodePOS = "KodePOS";
            public const string Telpon = "Telpon";
            public const string Fax = "Fax";
            public const string Email = "Email";
            public const string FileFoto = "FileFoto";
            public const string Agama = "Agama";
            public const string StatusKeluarga = "StatusKeluarga";
            public const string KodeTransfer = "KodeTransfer";
            public const string NoRekening = "NoRekening";
            public const string IsNonAktif = "IsNonAktif";
            public const string NoHP = "NoHP";
            public const string StatusPajak = "StatusPajak";
            public const string Golongan = "Golongan";
            public const string IsPengajar = "IsPengajar";
            public const string SerialNumber = "SerialNumber";
            public const string NPWP = "NPWP";
            public const string EmailPribadi = "EmailPribadi";
            public const string JenisKartu = "JenisKartu";
            public const string JabatanKartu = "JabatanKartu";
            public const string IsSatpamRegular = "IsSatpamRegular";
            public const string NoKPJ = "NoKPJ";
            public const string NoBPJSKesehatan = "NoBPJSKesehatan";
            public const string NoBPJSKetenagakerjaan = "NoBPJSKetenagakerjaan";
            public const string IsKaryawanAsing = "IsKaryawanAsing";
            public const string Rel_Unit = "Rel_Unit";
            public const string NamaAyah = "NamaAyah";
            public const string TempatLahirAyah = "TempatLahirAyah";
            public const string TanggalLahirAyah = "TanggalLahirAyah";
            public const string NIKAyah = "NIKAyah";
            public const string NoBPJSKesAyah = "NoBPJSKesAyah";
            public const string NamaIbu = "NamaIbu";
            public const string TempatLahirIbu = "TempatLahirIbu";
            public const string TanggalLahirIbu = "TanggalLahirIbu";
            public const string NIKIbu = "NIKIbu";
            public const string NoBPJSKesIbu = "NoBPJSKesIbu";
            public const string NamaAyahMertua = "NamaAyahMertua";
            public const string TempatLahirAyahMertua = "TempatLahirAyahMertua";
            public const string TanggalLahirAyahMertua = "TanggalLahirAyahMertua";
            public const string NIKAyahMertua = "NIKAyahMertua";
            public const string NoBPJSKesAyahMertua = "NoBPJSKesAyahMertua";
            public const string NamaIbuMertua = "NamaIbuMertua";
            public const string TempatLahirIbuMertua = "TempatLahirIbuMertua";
            public const string TanggalLahirIbuMertua = "TanggalLahirIbuMertua";
            public const string NIKIbuMertua = "NIKIbuMertua";
            public const string NoBPJSKesIbuMertua = "NoBPJSKesIbuMertua";
            public const string NamaSuamiIstri = "NamaSuamiIstri";
            public const string TempatLahirSuamiIstri = "TempatLahirSuamiIstri";
            public const string TanggalLahirSuamiIstri = "TanggalLahirSuamiIstri";
            public const string NIKSuamiIstri = "NIKSuamiIstri";
            public const string NoBPJSKesSuamiIstri = "NoBPJSKesSuamiIstri";
            public const string NamaAnakKe1 = "NamaAnakKe1";
            public const string TempatLahirAnakKe1 = "TempatLahirAnakKe1";
            public const string TanggalLahirAnakKe1 = "TanggalLahirAnakKe1";
            public const string NIKAnakKe1 = "NIKAnakKe1";
            public const string NoBPJSKesAnakKe1 = "NoBPJSKesAnakKe1";
            public const string NamaAnakKe2 = "NamaAnakKe2";
            public const string TempatLahirAnakKe2 = "TempatLahirAnakKe2";
            public const string TanggalLahirAnakKe2 = "TanggalLahirAnakKe2";
            public const string NIKAnakKe2 = "NIKAnakKe2";
            public const string NoBPJSKesAnakKe2 = "NoBPJSKesAnakKe2";
            public const string NamaAnakKe3 = "NamaAnakKe3";
            public const string TempatLahirAnakKe3 = "TempatLahirAnakKe3";
            public const string TanggalLahirAnakKe3 = "TanggalLahirAnakKe3";
            public const string NIKAnakKe3 = "NIKAnakKe3";
            public const string NoBPJSKesAnakKe3 = "NoBPJSKesAnakKe3";
            public const string NamaAnakKe4 = "NamaAnakKe4";
            public const string TempatLahirAnakKe4 = "TempatLahirAnakKe4";
            public const string TanggalLahirAnakKe4 = "TanggalLahirAnakKe4";
            public const string NIKAnakKe4 = "NIKAnakKe4";
            public const string NoBPJSKesAnakKe4 = "NoBPJSKesAnakKe4";
            public const string NamaAnakKe5 = "NamaAnakKe5";
            public const string TempatLahirAnakKe5 = "TempatLahirAnakKe5";
            public const string TanggalLahirAnakKe5 = "TanggalLahirAnakKe5";
            public const string NIKAnakKe5 = "NIKAnakKe5";
            public const string NoBPJSKesAnakKe5 = "NoBPJSKesAnakKe5";
            public const string NamaAnakKe6 = "NamaAnakKe6";
            public const string TempatLahirAnakKe6 = "TempatLahirAnakKe6";
            public const string TanggalLahirAnakKe6 = "TanggalLahirAnakKe6";
            public const string NIKAnakKe6 = "NIKAnakKe6";
            public const string NoBPJSKesAnakKe6 = "NoBPJSKesAnakKe6";
        }

        public static Pegawai GetEntityFromDataRow(DataRow row)
        {
            return new Pegawai
            {
                Kode = row[NamaField.Kode].ToString(),
                Nama = row[NamaField.Nama].ToString(),
                NamaRekening = row[NamaField.NamaRekening].ToString(),
                NamaKartu = row[NamaField.NamaKartu].ToString(),
                TanggalMasuk = (row[NamaField.TanggalMasuk] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalMasuk])),
                TanggalKeluar = (row[NamaField.TanggalKeluar] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalKeluar])),
                TempatLahir = row[NamaField.TempatLahir].ToString(),
                TanggalLahir = (row[NamaField.TanggalLahir] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahir])),
                JenisKelamin = row[NamaField.JenisKelamin].ToString().Trim(),
                Rel_Jabatan = row[NamaField.Rel_Jabatan].ToString(),
                NoKTP = row[NamaField.NoKTP].ToString(),
                NoKK = row[NamaField.NoKK].ToString(),
                StatusPegawai = row[NamaField.StatusPegawai].ToString(),
                TempatTinggal = row[NamaField.TempatTinggal].ToString().Trim(),
                AlamatRumah = row[NamaField.AlamatRumah].ToString(),
                Kota = row[NamaField.Kota].ToString(),
                KodePOS = row[NamaField.KodePOS].ToString(),
                Telpon = row[NamaField.Telpon].ToString(),
                Fax = row[NamaField.Fax].ToString(),
                Email = row[NamaField.Email].ToString(),
                FileFoto = row[NamaField.FileFoto].ToString(),
                Agama = row[NamaField.Agama].ToString().Trim(),
                StatusKeluarga = row[NamaField.StatusKeluarga].ToString(),
                KodeTransfer = row[NamaField.KodeTransfer].ToString(),
                NoRekening = row[NamaField.NoRekening].ToString(),
                IsNonAktif = (row[NamaField.IsNonAktif] == DBNull.Value ? false : (Convert.ToBoolean(row[NamaField.IsNonAktif]))),
                NoHP = row[NamaField.NoHP].ToString(),
                StatusPajak = row[NamaField.StatusPajak].ToString(),
                Golongan = row[NamaField.Golongan].ToString(),
                IsPengajar = (row[NamaField.IsPengajar] == DBNull.Value ? false : (Convert.ToBoolean(row[NamaField.IsPengajar]))),
                SerialNumber = row[NamaField.SerialNumber].ToString(),
                NPWP = row[NamaField.NPWP].ToString(),
                EmailPribadi = row[NamaField.EmailPribadi].ToString(),
                JenisKartu = row[NamaField.JenisKartu].ToString(),
                JabatanKartu = row[NamaField.JabatanKartu].ToString(),
                IsSatpamRegular = (row[NamaField.IsSatpamRegular] == DBNull.Value ? false : (Convert.ToBoolean(row[NamaField.IsSatpamRegular]))),
                NoKPJ = row[NamaField.NoKPJ].ToString(),
                NoBPJSKesehatan = row[NamaField.NoBPJSKesehatan].ToString(),
                NoBPJSKetenagakerjaan = row[NamaField.NoBPJSKetenagakerjaan].ToString(),
                IsKaryawanAsing = (row[NamaField.IsKaryawanAsing] == DBNull.Value ? false : (Convert.ToBoolean(row[NamaField.IsKaryawanAsing]))),
                Rel_Unit = row[NamaField.Rel_Unit].ToString(),
                NamaAyah = row[NamaField.NamaAyah].ToString(),
                TempatLahirAyah = row[NamaField.TempatLahirAyah].ToString(),
                TanggalLahirAyah = (row[NamaField.TanggalLahirAyah] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirAyah])),
                NIKAyah = row[NamaField.NIKAyah].ToString(),
                NoBPJSKesAyah = row[NamaField.NoBPJSKesAyah].ToString(),
                NamaIbu = row[NamaField.NamaIbu].ToString(),
                TempatLahirIbu = row[NamaField.TempatLahirIbu].ToString(),
                TanggalLahirIbu = (row[NamaField.TanggalLahirIbu] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirIbu])),
                NIKIbu = row[NamaField.NIKIbu].ToString(),
                NoBPJSKesIbu = row[NamaField.NoBPJSKesIbu].ToString(),
                NamaAyahMertua = row[NamaField.NamaAyahMertua].ToString(),
                TempatLahirAyahMertua = row[NamaField.TempatLahirAyahMertua].ToString(),
                TanggalLahirAyahMertua = (row[NamaField.TanggalLahirAyahMertua] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirAyahMertua])),
                NIKAyahMertua = row[NamaField.NIKAyahMertua].ToString(),
                NoBPJSKesAyahMertua = row[NamaField.NoBPJSKesAyahMertua].ToString(),
                NamaIbuMertua = row[NamaField.NamaIbuMertua].ToString(),
                TempatLahirIbuMertua = row[NamaField.TempatLahirIbuMertua].ToString(),
                TanggalLahirIbuMertua = (row[NamaField.TanggalLahirIbuMertua] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirIbuMertua])),
                NIKIbuMertua = row[NamaField.NIKIbuMertua].ToString(),
                NoBPJSKesIbuMertua = row[NamaField.NoBPJSKesIbuMertua].ToString(),
                NamaSuamiIstri = row[NamaField.NamaSuamiIstri].ToString(),
                TempatLahirSuamiIstri = row[NamaField.TempatLahirSuamiIstri].ToString(),
                TanggalLahirSuamiIstri = (row[NamaField.TanggalLahirSuamiIstri] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirSuamiIstri])),
                NIKSuamiIstri = row[NamaField.NIKSuamiIstri].ToString(),
                NoBPJSKesSuamiIstri = row[NamaField.NoBPJSKesSuamiIstri].ToString(),
                NamaAnakKe1 = row[NamaField.NamaAnakKe1].ToString(),
                TempatLahirAnakKe1 = row[NamaField.TempatLahirAnakKe1].ToString(),
                TanggalLahirAnakKe1 = (row[NamaField.TanggalLahirAnakKe1] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirAnakKe1])),
                NIKAnakKe1 = row[NamaField.NIKAnakKe1].ToString(),
                NoBPJSKesAnakKe1 = row[NamaField.NoBPJSKesAnakKe1].ToString(),
                NamaAnakKe2 = row[NamaField.NamaAnakKe2].ToString(),
                TempatLahirAnakKe2 = row[NamaField.TempatLahirAnakKe2].ToString(),
                TanggalLahirAnakKe2 = (row[NamaField.TanggalLahirAnakKe2] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirAnakKe2])),
                NIKAnakKe2 = row[NamaField.NIKAnakKe2].ToString(),
                NoBPJSKesAnakKe2 = row[NamaField.NoBPJSKesAnakKe2].ToString(),
                NamaAnakKe3 = row[NamaField.NamaAnakKe3].ToString(),
                TempatLahirAnakKe3 = row[NamaField.TempatLahirAnakKe3].ToString(),
                TanggalLahirAnakKe3 = (row[NamaField.TanggalLahirAnakKe3] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirAnakKe3])),
                NIKAnakKe3 = row[NamaField.NIKAnakKe3].ToString(),
                NoBPJSKesAnakKe3 = row[NamaField.NoBPJSKesAnakKe3].ToString(),
                NamaAnakKe4 = row[NamaField.NamaAnakKe4].ToString(),
                TempatLahirAnakKe4 = row[NamaField.TempatLahirAnakKe4].ToString(),
                TanggalLahirAnakKe4 = (row[NamaField.TanggalLahirAnakKe4] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirAnakKe4])),
                NIKAnakKe4 = row[NamaField.NIKAnakKe4].ToString(),
                NoBPJSKesAnakKe4 = row[NamaField.NoBPJSKesAnakKe4].ToString(),
                NamaAnakKe5 = row[NamaField.NamaAnakKe5].ToString(),
                TempatLahirAnakKe5 = row[NamaField.TempatLahirAnakKe5].ToString(),
                TanggalLahirAnakKe5 = (row[NamaField.TanggalLahirAnakKe5] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirAnakKe5])),
                NIKAnakKe5 = row[NamaField.NIKAnakKe5].ToString(),
                NoBPJSKesAnakKe5 = row[NamaField.NoBPJSKesAnakKe5].ToString(),
                NamaAnakKe6 = row[NamaField.NamaAnakKe6].ToString(),
                TempatLahirAnakKe6 = row[NamaField.TempatLahirAnakKe6].ToString(),
                TanggalLahirAnakKe6 = (row[NamaField.TanggalLahirAnakKe6] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirAnakKe6])),
                NIKAnakKe6 = row[NamaField.NIKAnakKe6].ToString(),
                NoBPJSKesAnakKe6 = row[NamaField.NoBPJSKesAnakKe6].ToString()
            };
        }

        public static List<Pegawai> GetAll_Entity()
        {
            List<Pegawai> hasil = new List<Pegawai>();
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

        public static Pegawai GetByID_Entity(string kode)
        {
            Pegawai hasil = new Pegawai();
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

        public static int GetCountFormasiGuruByID_Entity(string rel_guru)
        {
            int hasil = 0;
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_FormasiGuru_SELECT_COUNT_BY_GURU;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = Convert.ToInt16(row[0]);
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

        public static List<Pegawai> GetByUnit_Entity(string rel_unit)
        {
            List<Pegawai> hasil = new List<Pegawai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_UNIT;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Unit, rel_unit);

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

        public static void Delete(string Kode, string user_id)
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
        public static void Insert(Pegawai pegawai, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, pegawai.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, pegawai.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaRekening, pegawai.NamaRekening));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaKartu, pegawai.NamaKartu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalMasuk, pegawai.TanggalMasuk));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalKeluar, pegawai.TanggalKeluar));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahir, pegawai.TempatLahir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahir, pegawai.TanggalLahir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKelamin, pegawai.JenisKelamin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Jabatan, pegawai.Rel_Jabatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoKTP, pegawai.NoKTP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoKK, pegawai.NoKK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusPegawai, pegawai.StatusPegawai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatTinggal, pegawai.TempatTinggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatRumah, pegawai.AlamatRumah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kota, pegawai.Kota));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KodePOS, pegawai.KodePOS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Telpon, pegawai.Telpon));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Fax, pegawai.Fax));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Email, pegawai.Email));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.FileFoto, pegawai.FileFoto));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Agama, pegawai.Agama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusKeluarga, pegawai.StatusKeluarga));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KodeTransfer, pegawai.KodeTransfer));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoRekening, pegawai.NoRekening));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNonAktif, pegawai.IsNonAktif));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoHP, pegawai.NoHP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusPajak, pegawai.StatusPajak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Golongan, pegawai.Golongan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsPengajar, pegawai.IsPengajar));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SerialNumber, pegawai.SerialNumber));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NPWP, pegawai.NPWP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EmailPribadi, pegawai.EmailPribadi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKartu, pegawai.JenisKartu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JabatanKartu, pegawai.JabatanKartu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsSatpamRegular, pegawai.IsSatpamRegular));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoKPJ, pegawai.NoKPJ));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesehatan, pegawai.NoBPJSKesehatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKetenagakerjaan, pegawai.NoBPJSKetenagakerjaan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKaryawanAsing, pegawai.IsKaryawanAsing));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAyah, pegawai.NamaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAyah, pegawai.TempatLahirAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAyah, pegawai.TanggalLahirAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAyah, pegawai.NIKAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAyah, pegawai.NoBPJSKesAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaIbu, pegawai.NamaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirIbu, pegawai.TempatLahirIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirIbu, pegawai.TanggalLahirIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKIbu, pegawai.NIKIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesIbu, pegawai.NoBPJSKesIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAyahMertua, pegawai.NamaAyahMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAyahMertua, pegawai.TempatLahirAyahMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAyahMertua, pegawai.TanggalLahirAyahMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAyahMertua, pegawai.NIKAyahMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAyahMertua, pegawai.NoBPJSKesAyahMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaIbuMertua, pegawai.NamaIbuMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirIbuMertua, pegawai.TempatLahirIbuMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirIbuMertua, pegawai.TanggalLahirIbuMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKIbuMertua, pegawai.NIKIbuMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesIbuMertua, pegawai.NoBPJSKesIbuMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaSuamiIstri, pegawai.NamaSuamiIstri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirSuamiIstri, pegawai.TempatLahirSuamiIstri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirSuamiIstri, pegawai.TanggalLahirSuamiIstri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKSuamiIstri, pegawai.NIKSuamiIstri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesSuamiIstri, pegawai.NoBPJSKesSuamiIstri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAnakKe1, pegawai.NamaAnakKe1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAnakKe1, pegawai.TempatLahirAnakKe1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe1, pegawai.TanggalLahirAnakKe1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAnakKe1, pegawai.NIKAnakKe1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAnakKe1, pegawai.NoBPJSKesAnakKe1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAnakKe2, pegawai.NamaAnakKe2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAnakKe2, pegawai.TempatLahirAnakKe2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe2, pegawai.TanggalLahirAnakKe2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAnakKe2, pegawai.NIKAnakKe2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAnakKe2, pegawai.NoBPJSKesAnakKe2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAnakKe3, pegawai.NamaAnakKe3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAnakKe3, pegawai.TempatLahirAnakKe3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe3, pegawai.TanggalLahirAnakKe3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAnakKe3, pegawai.NIKAnakKe3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAnakKe3, pegawai.NoBPJSKesAnakKe3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAnakKe4, pegawai.NamaAnakKe4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAnakKe4, pegawai.TempatLahirAnakKe4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe4, pegawai.TanggalLahirAnakKe4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAnakKe4, pegawai.NIKAnakKe4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAnakKe4, pegawai.NoBPJSKesAnakKe4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAnakKe5, pegawai.NamaAnakKe5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAnakKe5, pegawai.TempatLahirAnakKe5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe5, pegawai.TanggalLahirAnakKe5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAnakKe5, pegawai.NIKAnakKe5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAnakKe5, pegawai.NoBPJSKesAnakKe5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAnakKe6, pegawai.NamaAnakKe6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAnakKe6, pegawai.TempatLahirAnakKe6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe6, pegawai.TanggalLahirAnakKe6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAnakKe6, pegawai.NIKAnakKe6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAnakKe6, pegawai.NoBPJSKesAnakKe6));
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

        public static void Update(Pegawai pegawai, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, pegawai.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, pegawai.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaRekening, pegawai.NamaRekening));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaKartu, pegawai.NamaKartu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalMasuk, pegawai.TanggalMasuk));
                if (pegawai.TanggalKeluar == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalKeluar, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalKeluar, pegawai.TanggalKeluar));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahir, pegawai.TempatLahir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahir, pegawai.TanggalLahir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKelamin, pegawai.JenisKelamin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Jabatan, pegawai.Rel_Jabatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoKTP, pegawai.NoKTP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoKK, pegawai.NoKK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusPegawai, pegawai.StatusPegawai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatTinggal, pegawai.TempatTinggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatRumah, pegawai.AlamatRumah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kota, pegawai.Kota));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KodePOS, pegawai.KodePOS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Telpon, pegawai.Telpon));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Fax, pegawai.Fax));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Email, pegawai.Email));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.FileFoto, pegawai.FileFoto));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Agama, pegawai.Agama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusKeluarga, pegawai.StatusKeluarga));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KodeTransfer, pegawai.KodeTransfer));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoRekening, pegawai.NoRekening));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNonAktif, pegawai.IsNonAktif));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoHP, pegawai.NoHP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusPajak, pegawai.StatusPajak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Golongan, pegawai.Golongan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsPengajar, pegawai.IsPengajar));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SerialNumber, pegawai.SerialNumber));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NPWP, pegawai.NPWP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EmailPribadi, pegawai.EmailPribadi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKartu, pegawai.JenisKartu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JabatanKartu, pegawai.JabatanKartu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsSatpamRegular, pegawai.IsSatpamRegular));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoKPJ, pegawai.NoKPJ));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesehatan, pegawai.NoBPJSKesehatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKetenagakerjaan, pegawai.NoBPJSKetenagakerjaan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsKaryawanAsing, pegawai.IsKaryawanAsing));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAyah, pegawai.NamaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAyah, pegawai.TempatLahirAyah));
                if (pegawai.TanggalLahirAyah == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAyah, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAyah, pegawai.TanggalLahirAyah));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAyah, pegawai.NIKAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAyah, pegawai.NoBPJSKesAyah));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaIbu, pegawai.NamaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirIbu, pegawai.TempatLahirIbu));
                if (pegawai.TanggalLahirIbu == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirIbu, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirIbu, pegawai.TanggalLahirIbu));
                }                
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKIbu, pegawai.NIKIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesIbu, pegawai.NoBPJSKesIbu));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAyahMertua, pegawai.NamaAyahMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAyahMertua, pegawai.TempatLahirAyahMertua));
                if (pegawai.TanggalLahirAyahMertua == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAyahMertua, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAyahMertua, pegawai.TanggalLahirAyahMertua));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAyahMertua, pegawai.NIKAyahMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAyahMertua, pegawai.NoBPJSKesAyahMertua));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaIbuMertua, pegawai.NamaIbuMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirIbuMertua, pegawai.TempatLahirIbuMertua));
                if (pegawai.TanggalLahirIbuMertua == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirIbuMertua, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirIbuMertua, pegawai.TanggalLahirIbuMertua));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKIbuMertua, pegawai.NIKIbuMertua));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesIbuMertua, pegawai.NoBPJSKesIbuMertua));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaSuamiIstri, pegawai.NamaSuamiIstri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirSuamiIstri, pegawai.TempatLahirSuamiIstri));
                if (pegawai.TanggalLahirSuamiIstri == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirSuamiIstri, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirSuamiIstri, pegawai.TanggalLahirSuamiIstri));

                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKSuamiIstri, pegawai.NIKSuamiIstri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesSuamiIstri, pegawai.NoBPJSKesSuamiIstri));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAnakKe1, pegawai.NamaAnakKe1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAnakKe1, pegawai.TempatLahirAnakKe1));
                if (pegawai.TanggalLahirAnakKe1 == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe1, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe1, pegawai.TanggalLahirAnakKe1));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAnakKe1, pegawai.NIKAnakKe1));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAnakKe1, pegawai.NoBPJSKesAnakKe1));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAnakKe2, pegawai.NamaAnakKe2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAnakKe2, pegawai.TempatLahirAnakKe2));
                if (pegawai.TanggalLahirAnakKe2 == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe2, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe2, pegawai.TanggalLahirAnakKe2));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAnakKe2, pegawai.NIKAnakKe2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAnakKe2, pegawai.NoBPJSKesAnakKe2));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAnakKe3, pegawai.NamaAnakKe3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAnakKe3, pegawai.TempatLahirAnakKe3));
                if (pegawai.TanggalLahirAnakKe3 == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe3, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe3, pegawai.TanggalLahirAnakKe3));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAnakKe3, pegawai.NIKAnakKe3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAnakKe3, pegawai.NoBPJSKesAnakKe3));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAnakKe4, pegawai.NamaAnakKe4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAnakKe4, pegawai.TempatLahirAnakKe4));
                if (pegawai.TanggalLahirAnakKe4 == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe4, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe4, pegawai.TanggalLahirAnakKe4));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAnakKe4, pegawai.NIKAnakKe4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAnakKe4, pegawai.NoBPJSKesAnakKe4));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAnakKe5, pegawai.NamaAnakKe5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAnakKe5, pegawai.TempatLahirAnakKe5));
                if (pegawai.TanggalLahirAnakKe5 == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe5, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe5, pegawai.TanggalLahirAnakKe5));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAnakKe5, pegawai.NIKAnakKe5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAnakKe5, pegawai.NoBPJSKesAnakKe5));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAnakKe6, pegawai.NamaAnakKe6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAnakKe6, pegawai.TempatLahirAnakKe6));
                if (pegawai.TanggalLahirAnakKe6 == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe6, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAnakKe6, pegawai.TanggalLahirAnakKe6));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAnakKe6, pegawai.NIKAnakKe6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoBPJSKesAnakKe6, pegawai.NoBPJSKesAnakKe6));

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
    }
}