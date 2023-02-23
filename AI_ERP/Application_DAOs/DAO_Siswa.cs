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
    public static class DAO_Siswa
    {
        public const string SP_SELECT_ALL = "Siswa___SELECT_ALL__BY_PERIODE";
        public const string SP_SELECT_ALL_ = "Siswa_SELECT_ALL_";
        public const string SP_SELECT_ALL_SISWA_DATA_SIMPLE = "Siswa___SELECT_ALL__BY_PERIODE_SISWA_DATA_SIMPLE";
        public const string SP_SELECT_ALL_BY_KELAS_PERWALIAN = "Siswa___SELECT_ALL__BY_PERIODE_BY_KELAS_PERWALIAN";        
        public const string SP_SELECT_ALL_BY_UNIT = "Siswa___SELECT_ALL__BY_PERIODE_BY_UNIT";
        public const string SP_SELECT_ALL_BY_UNIT_BY_KELAS = "Siswa___SELECT_ALL__BY_PERIODE_BY_UNIT_BY_KELAS";
        public const string SP_SELECT_ALL_CURRENT_BY_UNIT_BY_TA = "Siswa_SELECT_ALL_CURRENT_BY_UNIT_BY_TA";
        public const string SP_SELECT_ALL_BY_NAMA_SIMPLE = "Siswa_SELECT_ALL_BY_NAMA_SIMPLE";
        public const string SP_SELECT_ALL_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE = "Siswa___SELECT_ALL__BY_PERIODE_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE";
        public const string SP_SELECT_ALL_BY_NISSEKOLAH_SIMPLE = "Siswa_SELECT_ALL_BY_NISSEKOLAH_SIMPLE";
        public const string SP_SELECT_ALL_BY_NISSEKOLAH_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE = "Siswa___SELECT_ALL__BY_PERIODE_BY_NISSEKOLAH_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE";
        public const string SP_SELECT_ALL_BY_NISSEKOLAH_BY_LEVEL_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE = "Siswa___SELECT_ALL__BY_PERIODE_BY_NISSEKOLAH_BY_LEVEL_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE";

        public const string SP_SELECT_BY_ID = "Siswa___SELECT_ALL__BY_PERIODE_BY_ID";
        public const string SP_SELECT_BY_KODE = "Siswa___SELECT_ALL__BY_PERIODE_BY_KODE";
        public const string SP_SELECT_BY_NAMA = "Siswa_SELECT_BY_NAMA";
        public const string SP_SELECT_BY_ID_LIKE = "Siswa_SELECT_BY_ID_LIKE";
        public const string SP_SELECT_BY_NAMA_NO_NS = "Siswa_SELECT_BY_NAMA_NO_NS";
        public const string SP_SELECT_BY_ID_LIKE_NO_NS = "Siswa_SELECT_BY_ID_LIKE_NO_NS";
        public const string SP_SELECT_BY_SEKOLAH = "Siswa_SELECT_BY_SEKOLAH";

        public const string SP_SELECT_ALL_FOR_SEARCH = "Siswa___SELECT_ALL__BY_PERIODE_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_UNIT_FOR_SEARCH = "Siswa___SELECT_ALL__BY_PERIODE_BY_UNIT_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_UNIT_BY_KELAS_FOR_SEARCH = "Siswa___SELECT_ALL__BY_PERIODE_BY_UNIT_BY_KELAS_FOR_SEARCH";
        public const string SP_SELECT_ALL__FOR_SEARCH = "Siswa_SELECT_ALL__FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_KELAS_PERWALIAN_FOR_SEARCH = "Siswa___SELECT_ALL__BY_PERIODE_BY_KELAS_PERWALIAN_FOR_SEARCH";
        public const string SP_SELECT_DISTINCT_KELAS_BY_SEKOLAH = "Siswa_SELECT_DISTINCT_KELAS_BY_SEKOLAH";
        public const string SP_SELECT_NS_BY_SEKOLAH_BY_TAHUN_AJARAN = "Siswa_SELECT_NS_BY_SEKOLAH_BY_TAHUN_AJARAN";

        public const string SP_DELETE = "Siswa_DELETE";

        public const string SP_CREATE_BACKUP_BY_TA = "Siswa_CREATE_BACKUP_BY_TA";
        public const string SP_CREATE_BACKUP_BY_TA_BY_SM = "Siswa_CREATE_BACKUP_BY_TA_BY_SM";

        public const string SP_INSERT = "Siswa_INSERT";
        public const string SP_INSERT_SISWA_MANDIRI = "Siswa_INSERT_SISWA_MANDIRI";

        public const string SP_UPDATE = "Siswa_UPDATE";
        public const string SP_UPDATE_EMAIL = "Siswa_UPDATE_EMAIL";
        public const string SP_UPDATE_TITIPAN = "Siswa_UPDATE_TITIPAN";
        public const string SP_UPDATE_KELAS = "Siswa_UPDATE_KELAS";

        public const string SP_REVISI_NIS = "Siswa_REVISI_NIS";

        public const string SP_IS_URUTAN_KELAS_AKHIR = "Siswa_IS_URUTAN_KELAS_AKHIR";
        public const string SF_IS_URUTAN_KELAS_AKHIR = "Siswa_IS_URUTAN_KELAS_AKHIR";
        public const string SF_GENERATE_NIS = "Siswa_GENEREATE_NIS";

        public const string SP_SAUDARA_DELETE_BY_SISWA = "SiswaSaudara_DELETE_BY_SISWA";
        public const string SP_SAUDARA_SELECT_BY_SISWA = "SiswaSaudara_SELECT_BY_SISWA";
        public const string SP_SAUDARA_INSERT = "SiswaSaudara_INSERT";

        public class SiswaDataSimple
        {
            public Guid Kode { get; set; }
            public string NISSekolah { get; set; }
            public string NISN { get; set; }
            public string Nama { get; set; }
            public string JenisKelamin { get; set; }
            public string Rel_KelasDetJurusan { get; set; }
            public string Rel_KelasDetSosialisasi { get; set; }
        }

        public class SiswaByFormasiMapel : SiswaDataSimple
        {
            public string JenisKelas { get; set; }
            public string Rel_Kelas { get; set; }
            public string Rel_KelasDet { get; set; }
            public string Rel_Mapel { get; set; }
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string NISGlobal = "NISGlobal";
            public const string NIS = "NIS";
            public const string NISN = "NISN";
            public const string NISSekolah = "NISSekolah";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string IsNonAktif = "IsNonAktif";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_KelasBidangStudi = "Rel_KelasBidangStudi";
            public const string Rel_KelasDetPerwalian = "Rel_KelasDetPerwalian";
            public const string Rel_KelasDetJurusan = "Rel_KelasDetJurusan";
            public const string Rel_KelasDetSosialisasi = "Rel_KelasDetSosialisasi";
            public const string Rel_BidangStudi = "Rel_BidangStudi";
            public const string Nama = "Nama";
            public const string Panggilan = "Panggilan";
            public const string TempatLahir = "TempatLahir";
            public const string TanggalLahir = "TanggalLahir";
            public const string JenisKelamin = "JenisKelamin";
            public const string NISLama = "NISLama";
            public const string NoSeleksi = "NoSeleksi";
            public const string Agama = "Agama";
            public const string TelpRumah = "TelpRumah";
            public const string HP = "HP";
            public const string Email = "Email";
            public const string StatusAnak = "StatusAnak";
            public const string AnakKe = "AnakKe";
            public const string DariBersaudara = "DariBersaudara";
            public const string JumlahKakak = "JumlahKakak";
            public const string JumlahAdik = "JumlahAdik";
            public const string JumlahSaudaraKandung = "JumlahSaudaraKandung";
            public const string JumlahSaudaraTiri = "JumlahSaudaraTiri";
            public const string JumlahSaudaraAngkat = "JumlahSaudaraAngkat";
            public const string NIK = "NIK";
            public const string WargaNegara = "WargaNegara";
            public const string BahasaSehariHari = "BahasaSehariHari";
            public const string Hobi = "Hobi";
            public const string HobiLainnya = "HobiLainnya";
            public const string Alamat = "Alamat";
            public const string RT = "RT";
            public const string RW = "RW";
            public const string Kelurahan = "Kelurahan";
            public const string Kecamatan = "Kecamatan";
            public const string Kabupaten = "Kabupaten";
            public const string Provinsi = "Provinsi";
            public const string KodePOS = "KodePOS";
            public const string StatusTempatTinggal = "StatusTempatTinggal";
            public const string JarakKeSekolah = "JarakKeSekolah";
            public const string KeSekolahDengan = "KeSekolahDengan";
            public const string WaktuTempuh = "WaktuTempuh";
            public const string AsalSMA = "AsalSMA";
            public const string AsalSMP = "AsalSMP";
            public const string AsalSD = "AsalSD";
            public const string AsalTK = "AsalTK";
            public const string AsalKB = "AsalKB";
            public const string BakatKesenian = "BakatKesenian";
            public const string BakatOlahRaga = "BakatOlahRaga";
            public const string BakatKemasyarakatan = "BakatKemasyarakatan";
            public const string BakatLainLain = "BakatLainLain";
            public const string StatusHubunganDenganOrtu = "StatusHubunganDenganOrtu";
            public const string StatusPernikahanOrtu = "StatusPernikahanOrtu";
            public const string SiswaTinggalDengan = "SiswaTinggalDengan";
            public const string NamaAyah = "NamaAyah";
            public const string TempatLahirAyah = "TempatLahirAyah";
            public const string TanggalLahirAyah = "TanggalLahirAyah";
            public const string AgamaAyah = "AgamaAyah";
            public const string SukuBangsaAyah = "SukuBangsaAyah";
            public const string WargaNegaraAyah = "WargaNegaraAyah";
            public const string PendidikanAyah = "PendidikanAyah";
            public const string PendidikanAyahLainnya = "PendidikanAyahLainnya";
            public const string JurusanPendidikanAyah = "JurusanPendidikanAyah";
            public const string AlamatRumahAyah = "AlamatRumahAyah";
            public const string NIKAyah = "NIKAyah";
            public const string NoTelponAyah = "NoTelponAyah";
            public const string EmailAyah = "EmailAyah";
            public const string PekerjaanAyah = "PekerjaanAyah";
            public const string NamaInstansiAyah = "NamaInstansiAyah";
            public const string NoTelponKantorAyah = "NoTelponKantorAyah";
            public const string AlamatKantorAyah = "AlamatKantorAyah";
            public const string NamaIbu = "NamaIbu";
            public const string TempatLahirIbu = "TempatLahirIbu";
            public const string TanggalLahirIbu = "TanggalLahirIbu";
            public const string AgamaIbu = "AgamaIbu";
            public const string SukuBangsaIbu = "SukuBangsaIbu";
            public const string WargaNegaraIbu = "WargaNegaraIbu";
            public const string PendidikanIbu = "PendidikanIbu";
            public const string PendidikanIbuLainnya = "PendidikanIbuLainnya";
            public const string JurusanPendidikanIbu = "JurusanPendidikanIbu";
            public const string AlamatRumahIbu = "AlamatRumahIbu";
            public const string NIKIbu = "NIKIbu";
            public const string NoTelponIbu = "NoTelponIbu";
            public const string EmailIbu = "EmailIbu";
            public const string PekerjaanIbu = "PekerjaanIbu";
            public const string NamaInstansiIbu = "NamaInstansiIbu";
            public const string NoTelponKantorIbu = "NoTelponKantorIbu";
            public const string AlamatKantorIbu = "AlamatKantorIbu";
            public const string NamaKontakDarurat = "NamaKontakDarurat";
            public const string HubunganKontakDarurat = "HubunganKontakDarurat";
            public const string NoTelponKontakDarurat = "NoTelponKontakDarurat";
            public const string AlamatKontakDarurat = "AlamatKontakDarurat";
            public const string Catatan = "Catatan";
            public const string IsTinggalDgAyahKandung = "IsTinggalDgAyahKandung";
            public const string IsTinggalDgIbuKandung = "IsTinggalDgIbuKandung";
            public const string IsTinggalDgAyahTiri = "IsTinggalDgAyahTiri";
            public const string IsTinggalDgIbuTiri = "IsTinggalDgIbuTiri";
            public const string IsTinggalDgKakek = "IsTinggalDgKakek";
            public const string IsTinggalDgNenek = "IsTinggalDgNenek";
            public const string IsTinggalDgKakak = "IsTinggalDgKakak";
            public const string IsTinggalDgAdik = "IsTinggalDgAdik";
            public const string TinggalDenganLainnya = "TinggalDenganLainnya";
        }

        public static Siswa GetEntityFromDataRow(DataRow row)
        {
            Siswa siswa = new Siswa();

            siswa.Kode = new Guid(row[NamaField.Kode].ToString());
            siswa.NISGlobal = row[NamaField.NISGlobal].ToString();
            siswa.NIS = row[NamaField.NIS].ToString();
            siswa.NISN = row[NamaField.NISN].ToString();
            siswa.NISSekolah = row[NamaField.NISSekolah].ToString();
            siswa.TahunAjaran = row[NamaField.TahunAjaran].ToString();
            siswa.Rel_Sekolah = row[NamaField.Rel_Sekolah].ToString();
            siswa.IsNonAktif = (row[NamaField.IsNonAktif] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsNonAktif])); ;
            siswa.Rel_Kelas = row[NamaField.Rel_Kelas].ToString();
            siswa.Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString();
            siswa.Rel_KelasDetPerwalian = row[NamaField.Rel_KelasDetPerwalian].ToString();
            siswa.Rel_KelasDetJurusan = row[NamaField.Rel_KelasDetJurusan].ToString();
            siswa.Rel_KelasDetSosialisasi = row[NamaField.Rel_KelasDetSosialisasi].ToString();
            siswa.Rel_KelasBidangStudi = row[NamaField.Rel_KelasBidangStudi].ToString();
            siswa.Rel_BidangStudi = row[NamaField.Rel_BidangStudi].ToString();
            siswa.Nama = row[NamaField.Nama].ToString();
            siswa.Panggilan = row[NamaField.Panggilan].ToString();
            siswa.TempatLahir = row[NamaField.TempatLahir].ToString();
            siswa.TanggalLahir = (row[NamaField.TanggalLahir] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahir])); 
            siswa.JenisKelamin = row[NamaField.JenisKelamin].ToString();
            siswa.NISLama = row[NamaField.NISLama].ToString();
            siswa.NoSeleksi = row[NamaField.NoSeleksi].ToString();
            siswa.Agama = row[NamaField.Agama].ToString();
            siswa.TelpRumah = row[NamaField.TelpRumah].ToString();
            siswa.HP = row[NamaField.HP].ToString();
            siswa.Email = row[NamaField.Email].ToString();
            siswa.StatusAnak = row[NamaField.StatusAnak].ToString();
            siswa.AnakKe = row[NamaField.AnakKe].ToString();
            siswa.DariBersaudara = row[NamaField.DariBersaudara].ToString();
            siswa.JumlahKakak = row[NamaField.JumlahKakak].ToString();
            siswa.JumlahAdik = row[NamaField.JumlahAdik].ToString();
            siswa.JumlahSaudaraKandung = row[NamaField.JumlahSaudaraKandung].ToString();
            siswa.JumlahSaudaraTiri = row[NamaField.JumlahSaudaraTiri].ToString();
            siswa.JumlahSaudaraAngkat = row[NamaField.JumlahSaudaraAngkat].ToString();
            siswa.NIK = row[NamaField.NIK].ToString();
            siswa.WargaNegara = row[NamaField.WargaNegara].ToString();
            siswa.BahasaSehariHari = row[NamaField.BahasaSehariHari].ToString();
            siswa.Hobi = row[NamaField.Hobi].ToString();
            siswa.HobiLainnya = row[NamaField.HobiLainnya].ToString();
            siswa.Alamat = row[NamaField.Alamat].ToString();
            siswa.RT = row[NamaField.RT].ToString();
            siswa.RW = row[NamaField.RW].ToString();
            siswa.Kelurahan = row[NamaField.Kelurahan].ToString();
            siswa.Kecamatan = row[NamaField.Kecamatan].ToString();
            siswa.Kabupaten = row[NamaField.Kabupaten].ToString();
            siswa.Provinsi = row[NamaField.Provinsi].ToString();
            siswa.KodePOS = row[NamaField.KodePOS].ToString();
            siswa.StatusTempatTinggal = row[NamaField.StatusTempatTinggal].ToString();
            siswa.JarakKeSekolah = row[NamaField.JarakKeSekolah].ToString();
            siswa.KeSekolahDengan = row[NamaField.KeSekolahDengan].ToString();
            siswa.WaktuTempuh = row[NamaField.WaktuTempuh].ToString();
            siswa.AsalSMA = row[NamaField.AsalSMA].ToString();
            siswa.AsalSMP = row[NamaField.AsalSMP].ToString();
            siswa.AsalSD = row[NamaField.AsalSD].ToString();
            siswa.AsalTK = row[NamaField.AsalTK].ToString();
            siswa.AsalKB = row[NamaField.AsalKB].ToString();
            siswa.BakatKesenian = row[NamaField.BakatKesenian].ToString();
            siswa.BakatOlahRaga = row[NamaField.BakatOlahRaga].ToString();
            siswa.BakatKemasyarakatan = row[NamaField.BakatKemasyarakatan].ToString();
            siswa.BakatLainLain = row[NamaField.BakatLainLain].ToString();
            siswa.StatusHubunganDenganOrtu = row[NamaField.StatusHubunganDenganOrtu].ToString();
            siswa.StatusPernikahanOrtu = row[NamaField.StatusPernikahanOrtu].ToString();
            siswa.SiswaTinggalDengan = row[NamaField.SiswaTinggalDengan].ToString();
            siswa.NamaAyah = row[NamaField.NamaAyah].ToString();
            siswa.TempatLahirAyah = row[NamaField.TempatLahirAyah].ToString();
            siswa.TanggalLahirAyah = (row[NamaField.TanggalLahirAyah] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirAyah]));
            siswa.AgamaAyah = row[NamaField.AgamaAyah].ToString();
            siswa.SukuBangsaAyah = row[NamaField.SukuBangsaAyah].ToString();
            siswa.WargaNegaraAyah = row[NamaField.WargaNegaraAyah].ToString();
            siswa.PendidikanAyah = row[NamaField.PendidikanAyah].ToString();
            siswa.PendidikanAyahLainnya = row[NamaField.PendidikanAyahLainnya].ToString();
            siswa.JurusanPendidikanAyah = row[NamaField.JurusanPendidikanAyah].ToString();
            siswa.AlamatRumahAyah = row[NamaField.AlamatRumahAyah].ToString();
            siswa.NIKAyah = row[NamaField.NIKAyah].ToString();
            siswa.NoTelponAyah = row[NamaField.NoTelponAyah].ToString();
            siswa.EmailAyah = row[NamaField.EmailAyah].ToString();
            siswa.PekerjaanAyah = row[NamaField.PekerjaanAyah].ToString();
            siswa.NamaInstansiAyah = row[NamaField.NamaInstansiAyah].ToString();
            siswa.NoTelponKantorAyah = row[NamaField.NoTelponKantorAyah].ToString();
            siswa.AlamatKantorAyah = row[NamaField.AlamatKantorAyah].ToString();
            siswa.NamaIbu = row[NamaField.NamaIbu].ToString();
            siswa.TempatLahirIbu = row[NamaField.TempatLahirIbu].ToString();
            siswa.TanggalLahirIbu = (row[NamaField.TanggalLahirIbu] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirIbu]));
            siswa.AgamaIbu = row[NamaField.AgamaIbu].ToString();
            siswa.SukuBangsaIbu = row[NamaField.SukuBangsaIbu].ToString();
            siswa.WargaNegaraIbu = row[NamaField.WargaNegaraIbu].ToString();
            siswa.PendidikanIbu = row[NamaField.PendidikanIbu].ToString();
            siswa.PendidikanIbuLainnya = row[NamaField.PendidikanIbuLainnya].ToString();
            siswa.JurusanPendidikanIbu = row[NamaField.JurusanPendidikanIbu].ToString();
            siswa.AlamatRumahIbu = row[NamaField.AlamatRumahIbu].ToString();
            siswa.NIKIbu = row[NamaField.NIKIbu].ToString();
            siswa.NoTelponIbu = row[NamaField.NoTelponIbu].ToString();
            siswa.EmailIbu = row[NamaField.EmailIbu].ToString();
            siswa.PekerjaanIbu = row[NamaField.PekerjaanIbu].ToString();
            siswa.NamaInstansiIbu = row[NamaField.NamaInstansiIbu].ToString();
            siswa.NoTelponKantorIbu = row[NamaField.NoTelponKantorIbu].ToString();
            siswa.AlamatKantorIbu = row[NamaField.AlamatKantorIbu].ToString();
            siswa.NamaKontakDarurat = row[NamaField.NamaKontakDarurat].ToString();
            siswa.HubunganKontakDarurat = row[NamaField.HubunganKontakDarurat].ToString();
            siswa.NoTelponKontakDarurat = row[NamaField.NoTelponKontakDarurat].ToString();
            siswa.AlamatKontakDarurat = row[NamaField.AlamatKontakDarurat].ToString();
            siswa.Catatan = row[NamaField.Catatan].ToString();
            siswa.IsTinggalDgAyahKandung = (row[NamaField.IsTinggalDgAyahKandung] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgAyahKandung]));
            siswa.IsTinggalDgIbuKandung = (row[NamaField.IsTinggalDgIbuKandung] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgIbuKandung]));
            siswa.IsTinggalDgAyahTiri = (row[NamaField.IsTinggalDgAyahTiri] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgAyahTiri]));
            siswa.IsTinggalDgIbuTiri = (row[NamaField.IsTinggalDgIbuTiri] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgIbuTiri]));
            siswa.IsTinggalDgKakek = (row[NamaField.IsTinggalDgKakek] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgKakek]));
            siswa.IsTinggalDgNenek = (row[NamaField.IsTinggalDgNenek] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgNenek]));
            siswa.IsTinggalDgKakak = (row[NamaField.IsTinggalDgKakak] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgKakak]));
            siswa.IsTinggalDgAdik = (row[NamaField.IsTinggalDgAdik] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgAdik]));
            siswa.TinggalDenganLainnya = row[NamaField.TinggalDenganLainnya].ToString();

            return siswa;
        }
        
        public static Siswa GetByID_Entity(string tahun_ajaran, string semester, string nis)
        {
            Siswa hasil = new Siswa();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (nis == null) return hasil;
            if (nis.Trim() == "") return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_ID;
                comm.Parameters.AddWithValue("@TahunAjaran", nis);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@NIS", nis);

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

        public static Siswa GetByKode_Entity(string tahun_ajaran, string semester, string kode)
        {
            Siswa hasil = new Siswa();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            if (kode.Trim() == "") return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_KODE;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Kode", kode);

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

        public static List<Siswa> GetByRombel_Entity(
                string rel_sekolah,
                string rel_kelasdet,
                string tahun_ajaran,
                string semester
            )
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
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

        public static List<Siswa> GetByUnit_Entity(
                string rel_sekolah,
                string tahun_ajaran,
                string semester
            )
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_BY_UNIT;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);
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

        public static List<Siswa> GetAllByTahunAjaranUnitKelas_Entity(string rel_sekolah, string kelas_det, string tahun_ajaran, string semester)
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL;
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, rel_sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, kelas_det));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));

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

        public static List<SiswaDataSimple> GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(string rel_sekolah, string kelas_det, string tahun_ajaran, string semester)
        {
            List<SiswaDataSimple> hasil = new List<SiswaDataSimple>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_SISWA_DATA_SIMPLE;
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, rel_sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, kelas_det));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new SiswaDataSimple {
                                Kode = new Guid(row["Kode"].ToString()),
                                NISSekolah = row["NISSekolah"].ToString(),
                                NISN = row["NISN"].ToString(),
                                Nama = row["Nama"].ToString(),
                                JenisKelamin = row["JenisKelamin"].ToString(),
                                Rel_KelasDetJurusan = row["Rel_KelasDetJurusan"].ToString(),
                                Rel_KelasDetSosialisasi = row["Rel_KelasDetSosialisasi"].ToString()
                            }
                        );
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
        
        public static List<Siswa> GetAllByTahunAjaranUnitKelasPerwalian_Entity(string rel_sekolah, string kelas_det_perwalian, string tahun_ajaran, string semester)
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_BY_KELAS_PERWALIAN;
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, rel_sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetPerwalian, kelas_det_perwalian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));

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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIS, Kode));
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

        public static void CreateBackUp(string rel_sekolah, string tahun_ajaran_sc, string tahun_ajaran_ds)
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
                comm.CommandText = SP_CREATE_BACKUP_BY_TA;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, rel_sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran + "SC", tahun_ajaran_sc));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran + "DS", tahun_ajaran_ds));
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

        public static void CreateBackUp(string rel_sekolah, string tahun_ajaran_sc, string semester_sc, string tahun_ajaran_ds, string semester_ds)
        {
            if (semester_ds == "2")
            {
                tahun_ajaran_sc = tahun_ajaran_ds;
                semester_sc = "1";
            }

            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_CREATE_BACKUP_BY_TA_BY_SM;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, rel_sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran + "SC", tahun_ajaran_sc));
                comm.Parameters.Add(new SqlParameter("@SemesterSC", semester_sc));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran + "DS", tahun_ajaran_ds));                
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

        public static void Insert(Siswa m)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISGlobal, m.NISGlobal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIS, m.NIS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISN, m.NISN));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISSekolah, m.NISSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, m.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNonAktif, m.IsNonAktif));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetPerwalian, m.Rel_KelasDetPerwalian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetJurusan, m.Rel_KelasDetJurusan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetSosialisasi, m.Rel_KelasDetSosialisasi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasBidangStudi, m.Rel_KelasBidangStudi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_BidangStudi, m.Rel_BidangStudi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, m.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Panggilan, m.Panggilan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahir, m.TempatLahir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahir, m.TanggalLahir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKelamin, m.JenisKelamin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISLama, m.NISLama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoSeleksi, m.NoSeleksi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Agama, m.Agama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TelpRumah, m.TelpRumah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HP, m.HP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Email, m.Email));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusAnak, m.StatusAnak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AnakKe, m.AnakKe));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DariBersaudara, m.DariBersaudara));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahKakak, m.JumlahKakak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahAdik, m.JumlahAdik));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahSaudaraKandung, m.JumlahSaudaraKandung));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahSaudaraTiri, m.JumlahSaudaraTiri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahSaudaraAngkat, m.JumlahSaudaraAngkat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIK, m.NIK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WargaNegara, m.WargaNegara));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BahasaSehariHari, m.BahasaSehariHari));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Hobi, m.Hobi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HobiLainnya, m.HobiLainnya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alamat, m.Alamat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RT, m.RT));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RW, m.RW));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kelurahan, m.Kelurahan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kecamatan, m.Kecamatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kabupaten, m.Kabupaten));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Provinsi, m.Provinsi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KodePOS, m.KodePOS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusTempatTinggal, m.StatusTempatTinggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JarakKeSekolah, m.JarakKeSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KeSekolahDengan, m.KeSekolahDengan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WaktuTempuh, m.WaktuTempuh));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalSMA, m.AsalSMA));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalSMP, m.AsalSMP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalSD, m.AsalSD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalTK, m.AsalTK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalKB, m.AsalKB));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatKesenian, m.BakatKesenian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatOlahRaga, m.BakatOlahRaga));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatKemasyarakatan, m.BakatKemasyarakatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatLainLain, m.BakatLainLain));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusHubunganDenganOrtu, m.StatusHubunganDenganOrtu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusPernikahanOrtu, m.StatusPernikahanOrtu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SiswaTinggalDengan, m.SiswaTinggalDengan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAyah, m.NamaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAyah, m.TempatLahirAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAyah, m.TanggalLahirAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AgamaAyah, m.AgamaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SukuBangsaAyah, m.SukuBangsaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WargaNegaraAyah, m.WargaNegaraAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanAyah, m.PendidikanAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanAyahLainnya, m.PendidikanAyahLainnya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JurusanPendidikanAyah, m.JurusanPendidikanAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatRumahAyah, m.AlamatRumahAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAyah, m.NIKAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponAyah, m.NoTelponAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EmailAyah, m.EmailAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PekerjaanAyah, m.PekerjaanAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaInstansiAyah, m.NamaInstansiAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponKantorAyah, m.NoTelponKantorAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatKantorAyah, m.AlamatKantorAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaIbu, m.NamaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirIbu, m.TempatLahirIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirIbu, m.TanggalLahirIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AgamaIbu, m.AgamaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SukuBangsaIbu, m.SukuBangsaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WargaNegaraIbu, m.WargaNegaraIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanIbu, m.PendidikanIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanIbuLainnya, m.PendidikanIbuLainnya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JurusanPendidikanIbu, m.JurusanPendidikanIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatRumahIbu, m.AlamatRumahIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKIbu, m.NIKIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponIbu, m.NoTelponIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EmailIbu, m.EmailIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PekerjaanIbu, m.PekerjaanIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaInstansiIbu, m.NamaInstansiIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponKantorIbu, m.NoTelponKantorIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatKantorIbu, m.AlamatKantorIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaKontakDarurat, m.NamaKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HubunganKontakDarurat, m.HubunganKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponKontakDarurat, m.NoTelponKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatKontakDarurat, m.AlamatKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Catatan, m.Catatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgAyahKandung, m.IsTinggalDgAyahKandung));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgIbuKandung, m.IsTinggalDgIbuKandung));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgAyahTiri, m.IsTinggalDgAyahTiri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgIbuTiri, m.IsTinggalDgIbuTiri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgKakek, m.IsTinggalDgKakek));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgNenek, m.IsTinggalDgNenek));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgKakak, m.IsTinggalDgKakak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgAdik, m.IsTinggalDgAdik));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TinggalDenganLainnya, m.TinggalDenganLainnya));

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

        public static void Update(Siswa m, List<SiswaSaudara> lst_saudara)
        {
            if (m.NIS.Trim() == "") return;

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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISGlobal, m.NISGlobal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIS, m.NIS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISN, m.NISN));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISSekolah, m.NISSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, m.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNonAktif, m.IsNonAktif));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetPerwalian, m.Rel_KelasDetPerwalian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetJurusan, m.Rel_KelasDetJurusan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetSosialisasi, m.Rel_KelasDetSosialisasi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasBidangStudi, m.Rel_KelasBidangStudi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_BidangStudi, m.Rel_BidangStudi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, m.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Panggilan, m.Panggilan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahir, m.TempatLahir));
                if (m.TanggalLahir == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahir, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahir, m.TanggalLahir));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKelamin, m.JenisKelamin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISLama, m.NISLama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoSeleksi, m.NoSeleksi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Agama, m.Agama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TelpRumah, m.TelpRumah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HP, m.HP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Email, m.Email));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusAnak, m.StatusAnak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AnakKe, m.AnakKe));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DariBersaudara, m.DariBersaudara));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahKakak, m.JumlahKakak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahAdik, m.JumlahAdik));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahSaudaraKandung, m.JumlahSaudaraKandung));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahSaudaraTiri, m.JumlahSaudaraTiri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahSaudaraAngkat, m.JumlahSaudaraAngkat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIK, m.NIK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WargaNegara, m.WargaNegara));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BahasaSehariHari, m.BahasaSehariHari));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Hobi, m.Hobi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HobiLainnya, m.HobiLainnya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alamat, m.Alamat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RT, m.RT));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RW, m.RW));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kelurahan, m.Kelurahan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kecamatan, m.Kecamatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kabupaten, m.Kabupaten));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Provinsi, m.Provinsi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KodePOS, m.KodePOS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusTempatTinggal, m.StatusTempatTinggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JarakKeSekolah, m.JarakKeSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KeSekolahDengan, m.KeSekolahDengan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WaktuTempuh, m.WaktuTempuh));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalSMA, m.AsalSMA));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalSMP, m.AsalSMP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalSD, m.AsalSD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalTK, m.AsalTK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalKB, m.AsalKB));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatKesenian, m.BakatKesenian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatOlahRaga, m.BakatOlahRaga));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatKemasyarakatan, m.BakatKemasyarakatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatLainLain, m.BakatLainLain));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusHubunganDenganOrtu, m.StatusHubunganDenganOrtu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusPernikahanOrtu, m.StatusPernikahanOrtu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SiswaTinggalDengan, m.SiswaTinggalDengan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAyah, m.NamaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAyah, m.TempatLahirAyah));
                if (m.TanggalLahirAyah == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAyah, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAyah, m.TanggalLahirAyah));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AgamaAyah, m.AgamaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SukuBangsaAyah, m.SukuBangsaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WargaNegaraAyah, m.WargaNegaraAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanAyah, m.PendidikanAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanAyahLainnya, m.PendidikanAyahLainnya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JurusanPendidikanAyah, m.JurusanPendidikanAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatRumahAyah, m.AlamatRumahAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAyah, m.NIKAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponAyah, m.NoTelponAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EmailAyah, m.EmailAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PekerjaanAyah, m.PekerjaanAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaInstansiAyah, m.NamaInstansiAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponKantorAyah, m.NoTelponKantorAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatKantorAyah, m.AlamatKantorAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaIbu, m.NamaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirIbu, m.TempatLahirIbu));
                if (m.TanggalLahirIbu == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirIbu, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirIbu, m.TanggalLahirIbu));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AgamaIbu, m.AgamaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SukuBangsaIbu, m.SukuBangsaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WargaNegaraIbu, m.WargaNegaraIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanIbu, m.PendidikanIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanIbuLainnya, m.PendidikanIbuLainnya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JurusanPendidikanIbu, m.JurusanPendidikanIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatRumahIbu, m.AlamatRumahIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKIbu, m.NIKIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponIbu, m.NoTelponIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EmailIbu, m.EmailIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PekerjaanIbu, m.PekerjaanIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaInstansiIbu, m.NamaInstansiIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponKantorIbu, m.NoTelponKantorIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatKantorIbu, m.AlamatKantorIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaKontakDarurat, m.NamaKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HubunganKontakDarurat, m.HubunganKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponKontakDarurat, m.NoTelponKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatKontakDarurat, m.AlamatKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Catatan, m.Catatan));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgAyahKandung, m.IsTinggalDgAyahKandung));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgIbuKandung, m.IsTinggalDgIbuKandung));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgAyahTiri, m.IsTinggalDgAyahTiri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgIbuTiri, m.IsTinggalDgIbuTiri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgKakek, m.IsTinggalDgKakek));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgNenek, m.IsTinggalDgNenek));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgKakak, m.IsTinggalDgKakak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgAdik, m.IsTinggalDgAdik));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TinggalDenganLainnya, m.TinggalDenganLainnya));

                comm.ExecuteNonQuery();

                //saudara calon siswa
                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SAUDARA_DELETE_BY_SISWA;
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", m.Kode));
                comm.ExecuteNonQuery();

                int id = 0;
                foreach (var saudara in lst_saudara)
                {
                    comm.Parameters.Clear();
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = SP_SAUDARA_INSERT;
                    comm.Parameters.Add(new SqlParameter("@Rel_Siswa", m.Kode));
                    comm.Parameters.Add(new SqlParameter("@Urut", id));
                    comm.Parameters.Add(new SqlParameter("@Hubungan", saudara.Hubungan));
                    comm.Parameters.Add(new SqlParameter("@Nama", saudara.Nama));
                    comm.Parameters.Add(new SqlParameter("@JenisKelamin", saudara.JenisKelamin));
                    comm.Parameters.Add(new SqlParameter("@Umur", saudara.Umur));
                    comm.Parameters.Add(new SqlParameter("@Sekolah", saudara.Sekolah));
                    comm.Parameters.Add(new SqlParameter("@IsSaudaraKandung", saudara.IsSaudaraKandung));
                    comm.Parameters.Add(new SqlParameter("@Keterangan", saudara.Keterangan));
                    comm.Parameters.Add(new SqlParameter("@KeteranganLain", saudara.KeteranganLain));
                    comm.ExecuteNonQuery();

                    id++;
                }

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

        public static void UpdateEmail(string kode, string email)
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
                comm.CommandText = SP_UPDATE_EMAIL;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Email, email));
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

        public static List<SiswaSaudara> GetSaudaraByCalonSiswa(string kode)
        {
            List<SiswaSaudara> hasil = new List<SiswaSaudara>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SAUDARA_SELECT_BY_SISWA;
                comm.Parameters.AddWithValue("@Rel_Siswa", kode);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(new SiswaSaudara
                    {
                        Rel_Siswa = new Guid(kode),
                        Urut = Convert.ToInt16(row["Urut"]),
                        Hubungan = row["Hubungan"].ToString(),
                        Nama = row["Nama"].ToString(),
                        JenisKelamin = row["JenisKelamin"].ToString(),
                        Umur = row["Umur"].ToString(),
                        Sekolah = row["Sekolah"].ToString(),
                        IsSaudaraKandung = Convert.ToBoolean(row["IsSaudaraKandung"]),
                        Keterangan = row["Keterangan"].ToString(),
                        KeteranganLain = row["KeteranganLain"].ToString()
                    });
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

    public static class DAO_Siswa__OLD
    {
        public const string SP_SELECT_ALL = "Siswa_SELECT_ALL";
        public const string SP_SELECT_ALL_ = "Siswa_SELECT_ALL_";
        public const string SP_SELECT_ALL_SISWA_DATA_SIMPLE = "Siswa_SELECT_ALL_SISWA_DATA_SIMPLE";
        public const string SP_SELECT_ALL_BY_KELAS_PERWALIAN = "Siswa_SELECT_ALL_BY_KELAS_PERWALIAN";
        public const string SP_SELECT_ALL_BY_FILTER = "Siswa_SELECT_ALL_BY_FILTER";
        public const string SP_SELECT_ALL_BY_UNIT = "Siswa_SELECT_ALL_BY_UNIT";
        public const string SP_SELECT_ALL_BY_UNIT_BY_KELAS = "Siswa_SELECT_ALL_BY_UNIT_BY_KELAS";
        public const string SP_SELECT_ALL_CURRENT_BY_UNIT_BY_TA = "Siswa_SELECT_ALL_CURRENT_BY_UNIT_BY_TA";
        public const string SP_SELECT_ALL_BY_NAMA_SIMPLE = "Siswa_SELECT_ALL_BY_NAMA_SIMPLE";
        public const string SP_SELECT_ALL_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE = "Siswa_SELECT_ALL_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE";
        public const string SP_SELECT_ALL_BY_NISSEKOLAH_SIMPLE = "Siswa_SELECT_ALL_BY_NISSEKOLAH_SIMPLE";
        public const string SP_SELECT_ALL_BY_NISSEKOLAH_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE = "Siswa_SELECT_ALL_BY_NISSEKOLAH_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE";
        public const string SP_SELECT_ALL_BY_NISSEKOLAH_BY_LEVEL_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE = "Siswa_SELECT_ALL_BY_NISSEKOLAH_BY_LEVEL_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE";

        public const string SP_SELECT_BY_ID = "Siswa_SELECT_BY_ID";
        public const string SP_SELECT_BY_KODE = "Siswa_SELECT_BY_KODE";
        public const string SP_SELECT_BY_NAMA = "Siswa_SELECT_BY_NAMA";
        public const string SP_SELECT_BY_ID_LIKE = "Siswa_SELECT_BY_ID_LIKE";
        public const string SP_SELECT_BY_NAMA_NO_NS = "Siswa_SELECT_BY_NAMA_NO_NS";
        public const string SP_SELECT_BY_ID_LIKE_NO_NS = "Siswa_SELECT_BY_ID_LIKE_NO_NS";
        public const string SP_SELECT_BY_SEKOLAH = "Siswa_SELECT_BY_SEKOLAH";

        public const string SP_SELECT_ALL_FOR_SEARCH = "Siswa_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_UNIT_FOR_SEARCH = "Siswa_SELECT_ALL_BY_UNIT_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_UNIT_BY_KELAS_FOR_SEARCH = "Siswa_SELECT_ALL_BY_UNIT_BY_KELAS_FOR_SEARCH";
        public const string SP_SELECT_ALL__FOR_SEARCH = "Siswa_SELECT_ALL__FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_KELAS_PERWALIAN_FOR_SEARCH = "Siswa_SELECT_ALL_BY_KELAS_PERWALIAN_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_FILTER_FOR_SEARCH = "Siswa_SELECT_ALL_BY_FILTER_FOR_SEARCH";
        public const string SP_SELECT_ALL_ACTIVE = "Siswa_SELECT_ALL_ACTIVE";
        public const string SP_SELECT_DISTINCT_KELAS_BY_SEKOLAH = "Siswa_SELECT_DISTINCT_KELAS_BY_SEKOLAH";
        public const string SP_SELECT_NS_BY_SEKOLAH_BY_TAHUN_AJARAN = "Siswa_SELECT_NS_BY_SEKOLAH_BY_TAHUN_AJARAN";

        public const string SP_SELECT_ALL_ACTIVE_TAGIHAN = "Siswa_SELECT_ALL_ACTIVE_TAGIHAN";
        public const string SP_SELECT_ALL_ACTIVE_TAGIHAN_BY_ID = "Siswa_SELECT_ALL_ACTIVE_TAGIHAN_BY_ID";
        public const string SP_SELECT_ALL_ACTIVE_TAGIHAN_BY_UNIT = "Siswa_SELECT_ALL_ACTIVE_TAGIHAN_BY_UNIT";
        public const string SP_SELECT_ALL_ACTIVE_TAGIHAN_FOR_SEARCH = "Siswa_SELECT_ALL_ACTIVE_TAGIHAN_FOR_SEARCH";

        public const string SP_SELECT_ALL_ACTIVE_TAGIHAN_MANDIRI = "Siswa_SELECT_ALL_ACTIVE_TAGIHAN_MANDIRI";
        public const string SP_SELECT_ALL_ACTIVE_TAGIHAN_MANDIRI_BY_ID = "Siswa_SELECT_ALL_ACTIVE_TAGIHAN_MANDIRI_BY_ID";
        public const string SP_SELECT_ALL_ACTIVE_TAGIHAN_MANDIRI_BY_UNIT = "Siswa_SELECT_ALL_ACTIVE_TAGIHAN_MANDIRI_BY_UNIT";
        public const string SP_SELECT_ALL_ACTIVE_TAGIHAN_MANDIRI_FOR_SEARCH = "Siswa_SELECT_ALL_ACTIVE_TAGIHAN_MANDIRI_FOR_SEARCH";

        public const string SP_DELETE = "Siswa_DELETE";

        public const string SP_CREATE_BACKUP_BY_TA = "Siswa_CREATE_BACKUP_BY_TA";
        public const string SP_CREATE_BACKUP_BY_TA_BY_SM = "Siswa_CREATE_BACKUP_BY_TA_BY_SM";

        public const string SP_INSERT = "Siswa_INSERT";
        public const string SP_INSERT_SISWA_MANDIRI = "Siswa_INSERT_SISWA_MANDIRI";

        public const string SP_UPDATE = "Siswa_UPDATE";
        public const string SP_UPDATE_TITIPAN = "Siswa_UPDATE_TITIPAN";
        public const string SP_UPDATE_KELAS = "Siswa_UPDATE_KELAS";

        public const string SP_REVISI_NIS = "Siswa_REVISI_NIS";

        public const string SP_IS_URUTAN_KELAS_AKHIR = "Siswa_IS_URUTAN_KELAS_AKHIR";
        public const string SF_IS_URUTAN_KELAS_AKHIR = "Siswa_IS_URUTAN_KELAS_AKHIR";
        public const string SF_GENERATE_NIS = "Siswa_GENEREATE_NIS";

        public const string SP_SAUDARA_DELETE_BY_SISWA = "SiswaSaudara_DELETE_BY_SISWA";
        public const string SP_SAUDARA_SELECT_BY_SISWA = "SiswaSaudara_SELECT_BY_SISWA";
        public const string SP_SAUDARA_INSERT = "SiswaSaudara_INSERT";

        public class SiswaDataSimple
        {
            public Guid Kode { get; set; }
            public string NISSekolah { get; set; }
            public string NISN { get; set; }
            public string Nama { get; set; }
            public string JenisKelamin { get; set; }
            public string Rel_KelasDetJurusan { get; set; }
            public string Rel_KelasDetSosialisasi { get; set; }
        }

        public class SiswaByFormasiMapel : SiswaDataSimple
        {
            public string JenisKelas { get; set; }
            public string Rel_Kelas { get; set; }
            public string Rel_KelasDet { get; set; }
            public string Rel_Mapel { get; set; }
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string NISGlobal = "NISGlobal";
            public const string NIS = "NIS";
            public const string NISN = "NISN";
            public const string NISSekolah = "NISSekolah";
            public const string TahunAjaran = "TahunAjaran";
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string IsNonAktif = "IsNonAktif";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_KelasBidangStudi = "Rel_KelasBidangStudi";
            public const string Rel_KelasDetPerwalian = "Rel_KelasDetPerwalian";
            public const string Rel_KelasDetJurusan = "Rel_KelasDetJurusan";
            public const string Rel_KelasDetSosialisasi = "Rel_KelasDetSosialisasi";
            public const string Rel_BidangStudi = "Rel_BidangStudi";
            public const string Nama = "Nama";
            public const string Panggilan = "Panggilan";
            public const string TempatLahir = "TempatLahir";
            public const string TanggalLahir = "TanggalLahir";
            public const string JenisKelamin = "JenisKelamin";
            public const string NISLama = "NISLama";
            public const string NoSeleksi = "NoSeleksi";
            public const string Agama = "Agama";
            public const string TelpRumah = "TelpRumah";
            public const string HP = "HP";
            public const string Email = "Email";
            public const string StatusAnak = "StatusAnak";
            public const string AnakKe = "AnakKe";
            public const string DariBersaudara = "DariBersaudara";
            public const string JumlahKakak = "JumlahKakak";
            public const string JumlahAdik = "JumlahAdik";
            public const string JumlahSaudaraKandung = "JumlahSaudaraKandung";
            public const string JumlahSaudaraTiri = "JumlahSaudaraTiri";
            public const string JumlahSaudaraAngkat = "JumlahSaudaraAngkat";
            public const string NIK = "NIK";
            public const string WargaNegara = "WargaNegara";
            public const string BahasaSehariHari = "BahasaSehariHari";
            public const string Hobi = "Hobi";
            public const string HobiLainnya = "HobiLainnya";
            public const string Alamat = "Alamat";
            public const string RT = "RT";
            public const string RW = "RW";
            public const string Kelurahan = "Kelurahan";
            public const string Kecamatan = "Kecamatan";
            public const string Kabupaten = "Kabupaten";
            public const string Provinsi = "Provinsi";
            public const string KodePOS = "KodePOS";
            public const string StatusTempatTinggal = "StatusTempatTinggal";
            public const string JarakKeSekolah = "JarakKeSekolah";
            public const string KeSekolahDengan = "KeSekolahDengan";
            public const string WaktuTempuh = "WaktuTempuh";
            public const string AsalSMA = "AsalSMA";
            public const string AsalSMP = "AsalSMP";
            public const string AsalSD = "AsalSD";
            public const string AsalTK = "AsalTK";
            public const string AsalKB = "AsalKB";
            public const string BakatKesenian = "BakatKesenian";
            public const string BakatOlahRaga = "BakatOlahRaga";
            public const string BakatKemasyarakatan = "BakatKemasyarakatan";
            public const string BakatLainLain = "BakatLainLain";
            public const string StatusHubunganDenganOrtu = "StatusHubunganDenganOrtu";
            public const string StatusPernikahanOrtu = "StatusPernikahanOrtu";
            public const string SiswaTinggalDengan = "SiswaTinggalDengan";
            public const string NamaAyah = "NamaAyah";
            public const string TempatLahirAyah = "TempatLahirAyah";
            public const string TanggalLahirAyah = "TanggalLahirAyah";
            public const string AgamaAyah = "AgamaAyah";
            public const string SukuBangsaAyah = "SukuBangsaAyah";
            public const string WargaNegaraAyah = "WargaNegaraAyah";
            public const string PendidikanAyah = "PendidikanAyah";
            public const string PendidikanAyahLainnya = "PendidikanAyahLainnya";
            public const string JurusanPendidikanAyah = "JurusanPendidikanAyah";
            public const string AlamatRumahAyah = "AlamatRumahAyah";
            public const string NIKAyah = "NIKAyah";
            public const string NoTelponAyah = "NoTelponAyah";
            public const string EmailAyah = "EmailAyah";
            public const string PekerjaanAyah = "PekerjaanAyah";
            public const string NamaInstansiAyah = "NamaInstansiAyah";
            public const string NoTelponKantorAyah = "NoTelponKantorAyah";
            public const string AlamatKantorAyah = "AlamatKantorAyah";
            public const string NamaIbu = "NamaIbu";
            public const string TempatLahirIbu = "TempatLahirIbu";
            public const string TanggalLahirIbu = "TanggalLahirIbu";
            public const string AgamaIbu = "AgamaIbu";
            public const string SukuBangsaIbu = "SukuBangsaIbu";
            public const string WargaNegaraIbu = "WargaNegaraIbu";
            public const string PendidikanIbu = "PendidikanIbu";
            public const string PendidikanIbuLainnya = "PendidikanIbuLainnya";
            public const string JurusanPendidikanIbu = "JurusanPendidikanIbu";
            public const string AlamatRumahIbu = "AlamatRumahIbu";
            public const string NIKIbu = "NIKIbu";
            public const string NoTelponIbu = "NoTelponIbu";
            public const string EmailIbu = "EmailIbu";
            public const string PekerjaanIbu = "PekerjaanIbu";
            public const string NamaInstansiIbu = "NamaInstansiIbu";
            public const string NoTelponKantorIbu = "NoTelponKantorIbu";
            public const string AlamatKantorIbu = "AlamatKantorIbu";
            public const string NamaKontakDarurat = "NamaKontakDarurat";
            public const string HubunganKontakDarurat = "HubunganKontakDarurat";
            public const string NoTelponKontakDarurat = "NoTelponKontakDarurat";
            public const string AlamatKontakDarurat = "AlamatKontakDarurat";
            public const string Catatan = "Catatan";
            public const string IsTinggalDgAyahKandung = "IsTinggalDgAyahKandung";
            public const string IsTinggalDgIbuKandung = "IsTinggalDgIbuKandung";
            public const string IsTinggalDgAyahTiri = "IsTinggalDgAyahTiri";
            public const string IsTinggalDgIbuTiri = "IsTinggalDgIbuTiri";
            public const string IsTinggalDgKakek = "IsTinggalDgKakek";
            public const string IsTinggalDgNenek = "IsTinggalDgNenek";
            public const string IsTinggalDgKakak = "IsTinggalDgKakak";
            public const string IsTinggalDgAdik = "IsTinggalDgAdik";
            public const string TinggalDenganLainnya = "TinggalDenganLainnya";
        }

        public static Siswa GetEntityFromDataRow(DataRow row)
        {
            Siswa siswa = new Siswa();

            siswa.Kode = new Guid(row[NamaField.Kode].ToString());
            siswa.NISGlobal = row[NamaField.NISGlobal].ToString();
            siswa.NIS = row[NamaField.NIS].ToString();
            siswa.NISN = row[NamaField.NISN].ToString();
            siswa.NISSekolah = row[NamaField.NISSekolah].ToString();
            siswa.TahunAjaran = row[NamaField.TahunAjaran].ToString();
            siswa.Rel_Sekolah = row[NamaField.Rel_Sekolah].ToString();
            siswa.IsNonAktif = (row[NamaField.IsNonAktif] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsNonAktif])); ;
            siswa.Rel_Kelas = row[NamaField.Rel_Kelas].ToString();
            siswa.Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString();
            siswa.Rel_KelasDetPerwalian = row[NamaField.Rel_KelasDetPerwalian].ToString();
            siswa.Rel_KelasDetJurusan = row[NamaField.Rel_KelasDetJurusan].ToString();
            siswa.Rel_KelasDetSosialisasi = row[NamaField.Rel_KelasDetSosialisasi].ToString();
            siswa.Rel_KelasBidangStudi = row[NamaField.Rel_KelasBidangStudi].ToString();
            siswa.Rel_BidangStudi = row[NamaField.Rel_BidangStudi].ToString();
            siswa.Nama = row[NamaField.Nama].ToString();
            siswa.Panggilan = row[NamaField.Panggilan].ToString();
            siswa.TempatLahir = row[NamaField.TempatLahir].ToString();
            siswa.TanggalLahir = (row[NamaField.TanggalLahir] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahir]));
            siswa.JenisKelamin = row[NamaField.JenisKelamin].ToString();
            siswa.NISLama = row[NamaField.NISLama].ToString();
            siswa.NoSeleksi = row[NamaField.NoSeleksi].ToString();
            siswa.Agama = row[NamaField.Agama].ToString();
            siswa.TelpRumah = row[NamaField.TelpRumah].ToString();
            siswa.HP = row[NamaField.HP].ToString();
            siswa.Email = row[NamaField.Email].ToString();
            siswa.StatusAnak = row[NamaField.StatusAnak].ToString();
            siswa.AnakKe = row[NamaField.AnakKe].ToString();
            siswa.DariBersaudara = row[NamaField.DariBersaudara].ToString();
            siswa.JumlahKakak = row[NamaField.JumlahKakak].ToString();
            siswa.JumlahAdik = row[NamaField.JumlahAdik].ToString();
            siswa.JumlahSaudaraKandung = row[NamaField.JumlahSaudaraKandung].ToString();
            siswa.JumlahSaudaraTiri = row[NamaField.JumlahSaudaraTiri].ToString();
            siswa.JumlahSaudaraAngkat = row[NamaField.JumlahSaudaraAngkat].ToString();
            siswa.NIK = row[NamaField.NIK].ToString();
            siswa.WargaNegara = row[NamaField.WargaNegara].ToString();
            siswa.BahasaSehariHari = row[NamaField.BahasaSehariHari].ToString();
            siswa.Hobi = row[NamaField.Hobi].ToString();
            siswa.HobiLainnya = row[NamaField.HobiLainnya].ToString();
            siswa.Alamat = row[NamaField.Alamat].ToString();
            siswa.RT = row[NamaField.RT].ToString();
            siswa.RW = row[NamaField.RW].ToString();
            siswa.Kelurahan = row[NamaField.Kelurahan].ToString();
            siswa.Kecamatan = row[NamaField.Kecamatan].ToString();
            siswa.Kabupaten = row[NamaField.Kabupaten].ToString();
            siswa.Provinsi = row[NamaField.Provinsi].ToString();
            siswa.KodePOS = row[NamaField.KodePOS].ToString();
            siswa.StatusTempatTinggal = row[NamaField.StatusTempatTinggal].ToString();
            siswa.JarakKeSekolah = row[NamaField.JarakKeSekolah].ToString();
            siswa.KeSekolahDengan = row[NamaField.KeSekolahDengan].ToString();
            siswa.WaktuTempuh = row[NamaField.WaktuTempuh].ToString();
            siswa.AsalSMA = row[NamaField.AsalSMA].ToString();
            siswa.AsalSMP = row[NamaField.AsalSMP].ToString();
            siswa.AsalSD = row[NamaField.AsalSD].ToString();
            siswa.AsalTK = row[NamaField.AsalTK].ToString();
            siswa.AsalKB = row[NamaField.AsalKB].ToString();
            siswa.BakatKesenian = row[NamaField.BakatKesenian].ToString();
            siswa.BakatOlahRaga = row[NamaField.BakatOlahRaga].ToString();
            siswa.BakatKemasyarakatan = row[NamaField.BakatKemasyarakatan].ToString();
            siswa.BakatLainLain = row[NamaField.BakatLainLain].ToString();
            siswa.StatusHubunganDenganOrtu = row[NamaField.StatusHubunganDenganOrtu].ToString();
            siswa.StatusPernikahanOrtu = row[NamaField.StatusPernikahanOrtu].ToString();
            siswa.SiswaTinggalDengan = row[NamaField.SiswaTinggalDengan].ToString();
            siswa.NamaAyah = row[NamaField.NamaAyah].ToString();
            siswa.TempatLahirAyah = row[NamaField.TempatLahirAyah].ToString();
            siswa.TanggalLahirAyah = (row[NamaField.TanggalLahirAyah] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirAyah]));
            siswa.AgamaAyah = row[NamaField.AgamaAyah].ToString();
            siswa.SukuBangsaAyah = row[NamaField.SukuBangsaAyah].ToString();
            siswa.WargaNegaraAyah = row[NamaField.WargaNegaraAyah].ToString();
            siswa.PendidikanAyah = row[NamaField.PendidikanAyah].ToString();
            siswa.PendidikanAyahLainnya = row[NamaField.PendidikanAyahLainnya].ToString();
            siswa.JurusanPendidikanAyah = row[NamaField.JurusanPendidikanAyah].ToString();
            siswa.AlamatRumahAyah = row[NamaField.AlamatRumahAyah].ToString();
            siswa.NIKAyah = row[NamaField.NIKAyah].ToString();
            siswa.NoTelponAyah = row[NamaField.NoTelponAyah].ToString();
            siswa.EmailAyah = row[NamaField.EmailAyah].ToString();
            siswa.PekerjaanAyah = row[NamaField.PekerjaanAyah].ToString();
            siswa.NamaInstansiAyah = row[NamaField.NamaInstansiAyah].ToString();
            siswa.NoTelponKantorAyah = row[NamaField.NoTelponKantorAyah].ToString();
            siswa.AlamatKantorAyah = row[NamaField.AlamatKantorAyah].ToString();
            siswa.NamaIbu = row[NamaField.NamaIbu].ToString();
            siswa.TempatLahirIbu = row[NamaField.TempatLahirIbu].ToString();
            siswa.TanggalLahirIbu = (row[NamaField.TanggalLahirIbu] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.TanggalLahirIbu]));
            siswa.AgamaIbu = row[NamaField.AgamaIbu].ToString();
            siswa.SukuBangsaIbu = row[NamaField.SukuBangsaIbu].ToString();
            siswa.WargaNegaraIbu = row[NamaField.WargaNegaraIbu].ToString();
            siswa.PendidikanIbu = row[NamaField.PendidikanIbu].ToString();
            siswa.PendidikanIbuLainnya = row[NamaField.PendidikanIbuLainnya].ToString();
            siswa.JurusanPendidikanIbu = row[NamaField.JurusanPendidikanIbu].ToString();
            siswa.AlamatRumahIbu = row[NamaField.AlamatRumahIbu].ToString();
            siswa.NIKIbu = row[NamaField.NIKIbu].ToString();
            siswa.NoTelponIbu = row[NamaField.NoTelponIbu].ToString();
            siswa.EmailIbu = row[NamaField.EmailIbu].ToString();
            siswa.PekerjaanIbu = row[NamaField.PekerjaanIbu].ToString();
            siswa.NamaInstansiIbu = row[NamaField.NamaInstansiIbu].ToString();
            siswa.NoTelponKantorIbu = row[NamaField.NoTelponKantorIbu].ToString();
            siswa.AlamatKantorIbu = row[NamaField.AlamatKantorIbu].ToString();
            siswa.NamaKontakDarurat = row[NamaField.NamaKontakDarurat].ToString();
            siswa.HubunganKontakDarurat = row[NamaField.HubunganKontakDarurat].ToString();
            siswa.NoTelponKontakDarurat = row[NamaField.NoTelponKontakDarurat].ToString();
            siswa.AlamatKontakDarurat = row[NamaField.AlamatKontakDarurat].ToString();
            siswa.Catatan = row[NamaField.Catatan].ToString();
            siswa.IsTinggalDgAyahKandung = (row[NamaField.IsTinggalDgAyahKandung] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgAyahKandung]));
            siswa.IsTinggalDgIbuKandung = (row[NamaField.IsTinggalDgIbuKandung] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgIbuKandung]));
            siswa.IsTinggalDgAyahTiri = (row[NamaField.IsTinggalDgAyahTiri] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgAyahTiri]));
            siswa.IsTinggalDgIbuTiri = (row[NamaField.IsTinggalDgIbuTiri] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgIbuTiri]));
            siswa.IsTinggalDgKakek = (row[NamaField.IsTinggalDgKakek] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgKakek]));
            siswa.IsTinggalDgNenek = (row[NamaField.IsTinggalDgNenek] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgNenek]));
            siswa.IsTinggalDgKakak = (row[NamaField.IsTinggalDgKakak] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgKakak]));
            siswa.IsTinggalDgAdik = (row[NamaField.IsTinggalDgAdik] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsTinggalDgAdik]));
            siswa.TinggalDenganLainnya = row[NamaField.TinggalDenganLainnya].ToString();

            return siswa;
        }

        public static Siswa GetAllActiveByID_Entity(string kode)
        {
            Siswa hasil = new Siswa();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_ACTIVE_TAGIHAN_BY_ID;

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

        public static Siswa GetByID_Entity(string kode)
        {
            Siswa hasil = new Siswa();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            if (kode.Trim() == "") return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_ID;
                comm.Parameters.AddWithValue("@NIS", kode);

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

        public static Siswa GetByKode_Entity(string kode)
        {
            Siswa hasil = new Siswa();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            if (kode.Trim() == "") return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_KODE;
                comm.Parameters.AddWithValue("@Kode", kode);

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

        public static List<Siswa> GetAllActive_Entity()
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_ACTIVE;

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

        public static List<Siswa> GetCurrentByUnitByTA_Entity(
                string rel_sekolah, string tahun_ajaran
            )
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_CURRENT_BY_UNIT_BY_TA;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);

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

        public static List<Siswa> GetByRombel_Entity(
                string rel_sekolah,
                string rel_kelasdet,
                string tahun_ajaran
            )
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);

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

        public static List<Siswa> GetAllActiveTagihan_Entity()
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_ACTIVE_TAGIHAN;

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

        public static bool IsOnKelasAkhir(string rel_kelas, string rel_sekolah)
        {
            bool hasil = false;
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_IS_URUTAN_KELAS_AKHIR;
                comm.Parameters.AddWithValue("@Kode", rel_kelas);
                comm.Parameters.AddWithValue("@Rel_Sekolah", rel_sekolah);

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

        public static List<string> GetDistinctKelasByUnit_Entity(string unit)
        {
            List<string> hasil = new List<string>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_KELAS_BY_SEKOLAH;
                comm.Parameters.AddWithValue("@Rel_Sekolah", unit);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(row["Rel_KelasDet"].ToString());
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

        public static List<Siswa> GetAll_Entity()
        {
            List<Siswa> hasil = new List<Siswa>();
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

        public static List<Siswa> GetAllByTahunAjaranUnitKelas_Entity(string rel_sekolah, string kelas_det, string tahun_ajaran)
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL;
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, rel_sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, kelas_det));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));

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

        public static List<SiswaDataSimple> GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(string rel_sekolah, string kelas_det, string tahun_ajaran)
        {
            List<SiswaDataSimple> hasil = new List<SiswaDataSimple>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_SISWA_DATA_SIMPLE;
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, rel_sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, kelas_det));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new SiswaDataSimple
                            {
                                Kode = new Guid(row["Kode"].ToString()),
                                NISSekolah = row["NISSekolah"].ToString(),
                                NISN = row["NISN"].ToString(),
                                Nama = row["Nama"].ToString(),
                                JenisKelamin = row["JenisKelamin"].ToString(),
                                Rel_KelasDetJurusan = row["Rel_KelasDetJurusan"].ToString(),
                                Rel_KelasDetSosialisasi = row["Rel_KelasDetSosialisasi"].ToString()
                            }
                        );
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

        public static List<Siswa> GetAllByTahunAjaranUnitKelasPerwalian_Entity(string rel_sekolah, string kelas_det_perwalian, string tahun_ajaran)
        {
            List<Siswa> hasil = new List<Siswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_BY_KELAS_PERWALIAN;
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, rel_sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetPerwalian, kelas_det_perwalian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));

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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIS, Kode));
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

        public static void CreateBackUp(string rel_sekolah, string tahun_ajaran_sc, string tahun_ajaran_ds)
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
                comm.CommandText = SP_CREATE_BACKUP_BY_TA;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, rel_sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran + "SC", tahun_ajaran_sc));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran + "DS", tahun_ajaran_ds));
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

        public static void CreateBackUp(string rel_sekolah, string tahun_ajaran_sc, string semester_sc, string tahun_ajaran_ds)
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
                comm.CommandText = SP_CREATE_BACKUP_BY_TA_BY_SM;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, rel_sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran + "SC", tahun_ajaran_sc));
                comm.Parameters.Add(new SqlParameter("@SemesterSC", semester_sc));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran + "DS", tahun_ajaran_ds));
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

        public static void Insert(Siswa m)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISGlobal, m.NISGlobal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIS, m.NIS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISN, m.NISN));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISSekolah, m.NISSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, m.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNonAktif, m.IsNonAktif));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetPerwalian, m.Rel_KelasDetPerwalian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetJurusan, m.Rel_KelasDetJurusan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetSosialisasi, m.Rel_KelasDetSosialisasi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasBidangStudi, m.Rel_KelasBidangStudi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_BidangStudi, m.Rel_BidangStudi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, m.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Panggilan, m.Panggilan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahir, m.TempatLahir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahir, m.TanggalLahir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKelamin, m.JenisKelamin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISLama, m.NISLama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoSeleksi, m.NoSeleksi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Agama, m.Agama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TelpRumah, m.TelpRumah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HP, m.HP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Email, m.Email));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusAnak, m.StatusAnak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AnakKe, m.AnakKe));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DariBersaudara, m.DariBersaudara));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahKakak, m.JumlahKakak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahAdik, m.JumlahAdik));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahSaudaraKandung, m.JumlahSaudaraKandung));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahSaudaraTiri, m.JumlahSaudaraTiri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahSaudaraAngkat, m.JumlahSaudaraAngkat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIK, m.NIK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WargaNegara, m.WargaNegara));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BahasaSehariHari, m.BahasaSehariHari));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Hobi, m.Hobi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HobiLainnya, m.HobiLainnya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alamat, m.Alamat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RT, m.RT));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RW, m.RW));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kelurahan, m.Kelurahan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kecamatan, m.Kecamatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kabupaten, m.Kabupaten));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Provinsi, m.Provinsi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KodePOS, m.KodePOS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusTempatTinggal, m.StatusTempatTinggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JarakKeSekolah, m.JarakKeSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KeSekolahDengan, m.KeSekolahDengan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WaktuTempuh, m.WaktuTempuh));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalSMA, m.AsalSMA));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalSMP, m.AsalSMP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalSD, m.AsalSD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalTK, m.AsalTK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalKB, m.AsalKB));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatKesenian, m.BakatKesenian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatOlahRaga, m.BakatOlahRaga));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatKemasyarakatan, m.BakatKemasyarakatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatLainLain, m.BakatLainLain));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusHubunganDenganOrtu, m.StatusHubunganDenganOrtu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusPernikahanOrtu, m.StatusPernikahanOrtu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SiswaTinggalDengan, m.SiswaTinggalDengan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAyah, m.NamaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAyah, m.TempatLahirAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAyah, m.TanggalLahirAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AgamaAyah, m.AgamaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SukuBangsaAyah, m.SukuBangsaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WargaNegaraAyah, m.WargaNegaraAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanAyah, m.PendidikanAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanAyahLainnya, m.PendidikanAyahLainnya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JurusanPendidikanAyah, m.JurusanPendidikanAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatRumahAyah, m.AlamatRumahAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAyah, m.NIKAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponAyah, m.NoTelponAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EmailAyah, m.EmailAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PekerjaanAyah, m.PekerjaanAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaInstansiAyah, m.NamaInstansiAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponKantorAyah, m.NoTelponKantorAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatKantorAyah, m.AlamatKantorAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaIbu, m.NamaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirIbu, m.TempatLahirIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirIbu, m.TanggalLahirIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AgamaIbu, m.AgamaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SukuBangsaIbu, m.SukuBangsaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WargaNegaraIbu, m.WargaNegaraIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanIbu, m.PendidikanIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanIbuLainnya, m.PendidikanIbuLainnya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JurusanPendidikanIbu, m.JurusanPendidikanIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatRumahIbu, m.AlamatRumahIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKIbu, m.NIKIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponIbu, m.NoTelponIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EmailIbu, m.EmailIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PekerjaanIbu, m.PekerjaanIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaInstansiIbu, m.NamaInstansiIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponKantorIbu, m.NoTelponKantorIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatKantorIbu, m.AlamatKantorIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaKontakDarurat, m.NamaKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HubunganKontakDarurat, m.HubunganKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponKontakDarurat, m.NoTelponKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatKontakDarurat, m.AlamatKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Catatan, m.Catatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgAyahKandung, m.IsTinggalDgAyahKandung));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgIbuKandung, m.IsTinggalDgIbuKandung));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgAyahTiri, m.IsTinggalDgAyahTiri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgIbuTiri, m.IsTinggalDgIbuTiri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgKakek, m.IsTinggalDgKakek));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgNenek, m.IsTinggalDgNenek));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgKakak, m.IsTinggalDgKakak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgAdik, m.IsTinggalDgAdik));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TinggalDenganLainnya, m.TinggalDenganLainnya));

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

        public static void UpdateKelas(string nis, string nis_sekolah, string rel_kelasdet, string tahun_ajaran)
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
                comm.CommandText = SP_UPDATE_KELAS;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIS, nis));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISSekolah, nis_sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, rel_kelasdet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));

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

        public static void RevisiNIS(string nis_lama, string nis_baru)
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
                comm.CommandText = SP_REVISI_NIS;

                comm.Parameters.Add(new SqlParameter("@OldNIS", nis_lama));
                comm.Parameters.Add(new SqlParameter("@NewNIS", nis_baru));

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

        public static string GetGenerateNIS()
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            string hasil = "";

            try
            {
                conn.Open();
                comm.CommandType = CommandType.Text;
                comm.CommandText = "SELECT dbo." + SF_GENERATE_NIS + "()";

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = row[0].ToString();
                }

                return hasil;
            }
            catch (Exception)
            {
                return "";
            }
            finally
            {
                conn.Close();
            }
        }

        public static void Update(Siswa m, List<SiswaSaudara> lst_saudara)
        {
            if (m.NIS.Trim() == "") return;

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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISGlobal, m.NISGlobal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIS, m.NIS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISN, m.NISN));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISSekolah, m.NISSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, m.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNonAktif, m.IsNonAktif));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetPerwalian, m.Rel_KelasDetPerwalian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetJurusan, m.Rel_KelasDetJurusan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDetSosialisasi, m.Rel_KelasDetSosialisasi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasBidangStudi, m.Rel_KelasBidangStudi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_BidangStudi, m.Rel_BidangStudi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, m.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Panggilan, m.Panggilan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahir, m.TempatLahir));
                if (m.TanggalLahir == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahir, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahir, m.TanggalLahir));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisKelamin, m.JenisKelamin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NISLama, m.NISLama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoSeleksi, m.NoSeleksi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Agama, m.Agama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TelpRumah, m.TelpRumah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HP, m.HP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Email, m.Email));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusAnak, m.StatusAnak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AnakKe, m.AnakKe));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DariBersaudara, m.DariBersaudara));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahKakak, m.JumlahKakak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahAdik, m.JumlahAdik));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahSaudaraKandung, m.JumlahSaudaraKandung));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahSaudaraTiri, m.JumlahSaudaraTiri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JumlahSaudaraAngkat, m.JumlahSaudaraAngkat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIK, m.NIK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WargaNegara, m.WargaNegara));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BahasaSehariHari, m.BahasaSehariHari));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Hobi, m.Hobi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HobiLainnya, m.HobiLainnya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alamat, m.Alamat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RT, m.RT));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.RW, m.RW));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kelurahan, m.Kelurahan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kecamatan, m.Kecamatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kabupaten, m.Kabupaten));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Provinsi, m.Provinsi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KodePOS, m.KodePOS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusTempatTinggal, m.StatusTempatTinggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JarakKeSekolah, m.JarakKeSekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KeSekolahDengan, m.KeSekolahDengan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WaktuTempuh, m.WaktuTempuh));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalSMA, m.AsalSMA));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalSMP, m.AsalSMP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalSD, m.AsalSD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalTK, m.AsalTK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AsalKB, m.AsalKB));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatKesenian, m.BakatKesenian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatOlahRaga, m.BakatOlahRaga));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatKemasyarakatan, m.BakatKemasyarakatan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BakatLainLain, m.BakatLainLain));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusHubunganDenganOrtu, m.StatusHubunganDenganOrtu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.StatusPernikahanOrtu, m.StatusPernikahanOrtu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SiswaTinggalDengan, m.SiswaTinggalDengan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaAyah, m.NamaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirAyah, m.TempatLahirAyah));
                if (m.TanggalLahirAyah == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAyah, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirAyah, m.TanggalLahirAyah));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AgamaAyah, m.AgamaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SukuBangsaAyah, m.SukuBangsaAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WargaNegaraAyah, m.WargaNegaraAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanAyah, m.PendidikanAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanAyahLainnya, m.PendidikanAyahLainnya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JurusanPendidikanAyah, m.JurusanPendidikanAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatRumahAyah, m.AlamatRumahAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKAyah, m.NIKAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponAyah, m.NoTelponAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EmailAyah, m.EmailAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PekerjaanAyah, m.PekerjaanAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaInstansiAyah, m.NamaInstansiAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponKantorAyah, m.NoTelponKantorAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatKantorAyah, m.AlamatKantorAyah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaIbu, m.NamaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TempatLahirIbu, m.TempatLahirIbu));
                if (m.TanggalLahirIbu == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirIbu, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TanggalLahirIbu, m.TanggalLahirIbu));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AgamaIbu, m.AgamaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SukuBangsaIbu, m.SukuBangsaIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.WargaNegaraIbu, m.WargaNegaraIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanIbu, m.PendidikanIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PendidikanIbuLainnya, m.PendidikanIbuLainnya));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JurusanPendidikanIbu, m.JurusanPendidikanIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatRumahIbu, m.AlamatRumahIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NIKIbu, m.NIKIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponIbu, m.NoTelponIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.EmailIbu, m.EmailIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PekerjaanIbu, m.PekerjaanIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaInstansiIbu, m.NamaInstansiIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponKantorIbu, m.NoTelponKantorIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatKantorIbu, m.AlamatKantorIbu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NamaKontakDarurat, m.NamaKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HubunganKontakDarurat, m.HubunganKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NoTelponKontakDarurat, m.NoTelponKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.AlamatKontakDarurat, m.AlamatKontakDarurat));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Catatan, m.Catatan));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgAyahKandung, m.IsTinggalDgAyahKandung));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgIbuKandung, m.IsTinggalDgIbuKandung));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgAyahTiri, m.IsTinggalDgAyahTiri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgIbuTiri, m.IsTinggalDgIbuTiri));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgKakek, m.IsTinggalDgKakek));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgNenek, m.IsTinggalDgNenek));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgKakak, m.IsTinggalDgKakak));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsTinggalDgAdik, m.IsTinggalDgAdik));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TinggalDenganLainnya, m.TinggalDenganLainnya));

                comm.ExecuteNonQuery();

                //saudara calon siswa
                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SAUDARA_DELETE_BY_SISWA;
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", m.Kode));
                comm.ExecuteNonQuery();

                int id = 0;
                foreach (var saudara in lst_saudara)
                {
                    comm.Parameters.Clear();
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = SP_SAUDARA_INSERT;
                    comm.Parameters.Add(new SqlParameter("@Rel_Siswa", m.Kode));
                    comm.Parameters.Add(new SqlParameter("@Urut", id));
                    comm.Parameters.Add(new SqlParameter("@Hubungan", saudara.Hubungan));
                    comm.Parameters.Add(new SqlParameter("@Nama", saudara.Nama));
                    comm.Parameters.Add(new SqlParameter("@JenisKelamin", saudara.JenisKelamin));
                    comm.Parameters.Add(new SqlParameter("@Umur", saudara.Umur));
                    comm.Parameters.Add(new SqlParameter("@Sekolah", saudara.Sekolah));
                    comm.Parameters.Add(new SqlParameter("@IsSaudaraKandung", saudara.IsSaudaraKandung));
                    comm.Parameters.Add(new SqlParameter("@Keterangan", saudara.Keterangan));
                    comm.Parameters.Add(new SqlParameter("@KeteranganLain", saudara.KeteranganLain));
                    comm.ExecuteNonQuery();

                    id++;
                }

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

        public static List<SiswaSaudara> GetSaudaraByCalonSiswa(string kode)
        {
            List<SiswaSaudara> hasil = new List<SiswaSaudara>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SAUDARA_SELECT_BY_SISWA;
                comm.Parameters.AddWithValue("@Rel_Siswa", kode);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(new SiswaSaudara
                    {
                        Rel_Siswa = new Guid(kode),
                        Urut = Convert.ToInt16(row["Urut"]),
                        Hubungan = row["Hubungan"].ToString(),
                        Nama = row["Nama"].ToString(),
                        JenisKelamin = row["JenisKelamin"].ToString(),
                        Umur = row["Umur"].ToString(),
                        Sekolah = row["Sekolah"].ToString(),
                        IsSaudaraKandung = Convert.ToBoolean(row["IsSaudaraKandung"]),
                        Keterangan = row["Keterangan"].ToString(),
                        KeteranganLain = row["KeteranganLain"].ToString()
                    });
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