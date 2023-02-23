using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.UI.Adapters;
using System.Reflection;
using System.IO;
using System.Net;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Libs
{
    public class PeriodeAbsen
    {
        public int Bulan { get; set; }
        public int Tahun { get; set; }
    }

    public class PeriodeTahunAjaranAbsen
    {
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
    }

    public static class APP
    {
        public const string ID = "477P513"; 
        public enum TipeUser
        {
            User,            
            NggakAdaAkses
        }
    }

    public static class TipeRapor
    {
        public const string LTS = "6092CE8B-0863-4F39-A753-279A5FC4C232";
        public const string SEMESTER = "C85B184C-CFD7-4A75-807C-443D0D0A10F9";

        public static string GetJenisRaporFromKode(string kode)
        {
            if (kode.Trim().ToUpper() == "LTS") return LTS;
            if (kode.Trim().ToUpper() == "SEMESTER") return SEMESTER;
            return "";
        }
    }

    public static class KategoriCatatanSiswa
    {
        public static class Prestasi
        {
            public static string Kode { get { return "CFDFC30B-4C25-401E-9B83-E610B7D2C1C4"; } }
            public static string Nama { get { return "Prestasi"; } }
        }

        public static class Pelanggaran
        {
            public static string Kode { get { return "40240C5F-ACF7-47BC-89D5-8B6DCEFAD2FC"; } }
            public static string Nama { get { return "Pelanggaran"; } }
        }

        public static class Lainnya
        {
            public static string Kode { get { return "00323D6B-2933-4595-9F90-6AB23202CCF4"; } }
            public static string Nama { get { return "Lainnya"; } }
        }
    }

    public static class JenisStatusKunjungan
    {
        public enum JenisStatus
        {
            Diproses = 0,
            Approved = 1,
            Dibatalkan = 2,
            Dilaksanakan = 3
        }

        public static string GetJenisStatus(int jenis)
        {
            switch (jenis)
            {
                case 0: return "Diproses";
                case 1: return "Disetujui";
                case 2: return "Dibatalkan";
                case 3: return "Dilaksanakan";
                default:
                    break;
            }

            return "";
        }
    }



    public static class SessionLogin
    {
        public static void CreateLoginAdminSession(UserLogin login)
        {
            HttpContext.Current.Session.Timeout = 525600;
            HttpContext.Current.Session.Add(Constantas.NAMA_SESSION_LOGIN, login);
        }

        //buat presentasi
        public const string USR_GURU_TK = "admin.gurutk";
        public const string USR_GURU = "admin.guru";
        public const string USR_ORTU = "admin.ortu";

        public static void CreateLoginGuruSession()
        {
            HttpContext.Current.Session.Timeout = 525600;
            HttpContext.Current.Session.Add(Constantas.NAMA_SESSION_LOGIN,
                new UserLogin
                {
                    NoInduk = "123",
                    UserID = USR_GURU,
                    Password = "123"
                }
            );
        }

        public static void CreateLoginGuruTKSession()
        {
            HttpContext.Current.Session.Timeout = 525600;
            HttpContext.Current.Session.Add(Constantas.NAMA_SESSION_LOGIN,
                new UserLogin
                {
                    NoInduk = "123",
                    UserID = USR_GURU_TK,
                    Password = "123"
                }
            );
        }

        public static void CreateLoginOrtuSession()
        {
            HttpContext.Current.Session.Timeout = 525600;
            HttpContext.Current.Session.Add(Constantas.NAMA_SESSION_LOGIN,
                new UserLogin
                {
                    NoInduk = "A3897",
                    UserID = USR_ORTU,
                    Password = "123"
                }
            );
        }
        //end buat presentasi
    }

    public static class Constantas
    {
        public static string ApplicationName = "E-Learning AL IZHAR";
        public static string ID_APP = "481091b9-685f-4f00-a577-6b42199bb085";

        public const string NAMA_SESSION_LOGIN = "AdminLogin";
        public const string NAMA_SESSION_LOGIN_SISWA = "SiswaLogin";
        public const string WRONG_LOGIN_SISWA = "WrongSiswaLogin";

        public const string MSG_DATA_MASIH_DIPAKAI_DATA_SISWA = "Data tidak dapat dihapus, karena masih digunakan oleh data penerimaan calon siswa";
        public const string MSG_KETERANGAN_USER_ID = "Harap diisi dengan karakter alfanumerik maksimal 8 karakter huruf dan angka";
        public const string MSG_NIS_UNRECOMMENDED = "Mohon maaf anda tidak bisa melakukan pendaftaran, calon siswa tidak termasuk yang direkomendasikan untuk melakukan pendaftaran";

        public const string MSG_MAX_UPLOAD_FOTO = "5 MB";

        public const string URL_FOTO_SISWA = "http://aplikasi.alizhar.sch.id/keuangan/Application_Files/Siswa/";        
        public const string GUID_NOL = "00000000-0000-0000-0000-000000000000";
        public const decimal NilaiDesimalNULL = -99;

        public const string ALFABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        public const int PEMBULATAN_DESIMAL_NILAI_SD = 1;
        public const int PEMBULATAN_DESIMAL_NILAI_SMP = 0;
        public const int PEMBULATAN_DESIMAL_NILAI_SMP_2DES = 2;
        public const int PEMBULATAN_DESIMAL_NILAI_SMA = 0;
        public const int PEMBULATAN_DESIMAL_NILAI_SMA_2DES = 2;

        public const string HASHTAG_REP = "(@__@)";
        public const string NEWLINE_REP = "(@@__@@)";

        public const string TOKEN_ADMIN = "UCrRpYEytIHGyDgNWO6VbHlQUC6MTowFYbG8SK5GvTWjxSvgUCigNaporFrKIPHiF1FcWfwA";
        public const string TOKEN_VIEW_ORTU = "ELnn9V01EiI4UsIqmHUAtw";

        public const string JENIS_FILE_PDF = "PDF";
        public const string JENIS_FILE_HTML = "HTML";

        public const string JENIS_KD_PENGETAHUAN = "PENGETAHUAN";
        public const string JENIS_KD_KETERAMPILAN = "KETERAMPILAN";

        public static class SD
        {
            public const string SD_GUID_UKK_PAT = "33333333-0000-0000-0000-000000000000";
            public const string SD_GUID_NILAI_KD_FOR_AP = "33333333-1111-";
            public const string SD_GUID_NILAI_KD_FOR_AP_BOBOT = "33333333-1112-";
            public const string SD_GUID_NILAI_KD_UKKPAT_FOR_AP = "33333333-1113-";
            public const string SD_GUID_NILAI_KD_UKKPAT_FOR_AP_BOBOT = "33333333-1114-";

            public static string GetKode_NK_KD(string kode_ap)
            {
                return SD_GUID_NILAI_KD_FOR_AP + kode_ap.Substring(14, 22);
            }

            public static string GetKode_NK_KD_BOBOT(string kode_ap)
            {
                return SD_GUID_NILAI_KD_FOR_AP_BOBOT + kode_ap.Substring(14, 22);
            }

            public static string GetKode_NK_KD_UKKPAT(string kode_ap)
            {
                return SD_GUID_NILAI_KD_UKKPAT_FOR_AP + kode_ap.Substring(14, 22);
            }

            public static string GetKode_NK_KD_UKKPAT_BOBOT(string kode_ap)
            {
                return SD_GUID_NILAI_KD_UKKPAT_FOR_AP_BOBOT + kode_ap.Substring(14, 22);
            }
        }

        public static class SMP
        {
            public const string SMP_GUID_PENGETAHUAN_PH = "44444444-1111-0000-0000-000000000000";
            public const string SMP_GUID_PENGETAHUAN_PTS = "44444444-2222-0000-0000-000000000000";
            public const string SMP_GUID_PENGETAHUAN_PAS = "44444444-3333-0000-0000-000000000000";
        }

        public static class SMA
        {
            public const string TOKEN_PREVIEW_NILAI = "1yHbXp_C1XJVog04eutsAyUC6MTowCj0KCQjwvdXpBRCoARIsAMJSKqJ4J0ZiUcdCZIv";
            public const string TOKEN_NILAI_EKSKUL = "zmDPzF6YYx9MQPOtF6u828IxCj0KCQjwvdXpBRCoARIsAMJSKqJ4J0ZiUcdCZIv68bM76H";
            public const string TOKEN_NILAI_SIKAP = "UC6MTowFYbG8SK5GvTWjxSvg0KCQjwvdXpBRCoUC6MTowFYbG8SK5GvTWjxSvg";
        }
    }

    public static class Messages
    {
        public const string MSG_HAPUS_TIDAK_VALID = "Data yang akan dihapus tidak valid";
        public const string MSG_SUDAH_DISIMPAN = "Data sudah disimpan";
        public const string MSG_SUDAH_DIUPDATE = "Data sudah di-update";

        public static string DataSudahAda(string key = "")
        {
            return key + (key.Trim() != "" ? ". " : "") + "Data sudah ada.";
        }
    }

    public static class DataControls
    {
        public static void SelectComboByValue(DropDownList ddl, string value)
        {
            ddl.ClearSelection();
            int id = 0;
            foreach (ListItem item in ddl.Items)
            {
                if (item.Value == value)
                {
                    ddl.SelectedIndex = id;
                }
                id++;
            }
        }
    }

    public static class Libs
    {
        public static string StatusProses;

        public enum JenisPerhitunganNilai
        {
            Bobot = 0,
            RataRata = 1
        }

        public enum VABank
        {
            Mandiri,
            Permata
        }

        public enum JenisAsalSekolah
        {
            BelumSekolah,
            SiswaAlizhar,
            SiswaNonAlizhar
        }

        public enum JenisSiswa
        {
            Bergulir,
            Internal,
            Baru
        }

        public static class JENIS_MAPEL
        {
            public const string KHUSUS = "Khusus";
            public const string WAJIB = "Wajib";
            public const string WAJIB_A = "Wajib A";
            public const string WAJIB_B = "Wajib B";
            public const string WAJIB_B_PILIHAN = "Wajib B (Pilihan)";
            public const string PEMINATAN = "Peminatan";
            public const string LINTAS_MINAT = "Lintas Minat";
            public const string PILIHAN = "Pilihan";
            public const string EKSTRAKURIKULER = "Ekstrakurikuler";
            public const string EKSKUL = "Ekskul";
            public const string SIKAP = "Sikap";
            public const string VOLUNTEER = "Volunteer";

            public static void ListToDropdown(DropDownList cbo, bool ada_kosong = true, string rel_sekolah = "")
            {
                bool b_valid = true;
                cbo.Items.Clear();
                if (ada_kosong) cbo.Items.Add("");

                if (rel_sekolah.Trim() != "")
                {
                    Sekolah m_sekolah = AI_ERP.Application_DAOs.DAO_Sekolah.GetByID_Entity(rel_sekolah);
                    if (m_sekolah != null && m_sekolah.Nama != null)
                    {
                        if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA)
                        {
                            cbo.Items.Add(new ListItem { Value = WAJIB_A, Text = WAJIB_A });
                            cbo.Items.Add(new ListItem { Value = WAJIB_B, Text = WAJIB_B });
                            cbo.Items.Add(new ListItem { Value = WAJIB_B_PILIHAN, Text = WAJIB_B_PILIHAN });
                            cbo.Items.Add(new ListItem { Value = PEMINATAN, Text = PEMINATAN });
                            cbo.Items.Add(new ListItem { Value = LINTAS_MINAT, Text = LINTAS_MINAT });
                            cbo.Items.Add(new ListItem { Value = EKSTRAKURIKULER, Text = EKSTRAKURIKULER });
                            cbo.Items.Add(new ListItem { Value = SIKAP, Text = SIKAP });
                            b_valid = false;
                        }
                    }
                }

                if (b_valid)
                {
                    cbo.Items.Add(new ListItem { Value = WAJIB, Text = WAJIB });
                    cbo.Items.Add(new ListItem { Value = PILIHAN, Text = PILIHAN });
                    cbo.Items.Add(new ListItem { Value = EKSTRAKURIKULER, Text = EKSTRAKURIKULER });
                    cbo.Items.Add(new ListItem { Value = KHUSUS, Text = KHUSUS });
                    cbo.Items.Add(new ListItem { Value = SIKAP, Text = SIKAP });
                    cbo.Items.Add(new ListItem { Value = VOLUNTEER, Text = VOLUNTEER });
                }
            }

            public static void SelectDropdown(DropDownList cbo, string key)
            {
                foreach (ListItem item in cbo.Items)
                {
                    if (key == item.Value) {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
            }
        }

        public static void ListJamToDropdown(DropDownList cbo)
        {
            string[] arr_menit = new string[] { "00", "15", "30", "45" };
            for (var i = 0; i <= 23; i++)
            {
                for (var m = 0; m < arr_menit.Length; m++)
                {
                    string jam = (i < 10 ? "0" : "") + i.ToString() + ":" + arr_menit[m];
                    cbo.Items.Add(new ListItem { Value = jam, Text = jam });
                }
            }
        }

        public static string GetJamFromTanggal(DateTime tanggal)
        {
            return (tanggal.Hour < 10 ? "0" : "") + tanggal.Hour.ToString() +
                   ":" +
                   (tanggal.Minute < 10 ? "0" : "") + tanggal.Minute.ToString();
        }

        public static class URL_IDENTIFIER
        {
            public const string URL_ID_GURU_KELAS = "gkxref=g7h89nfkk90ynqwr3";
            public const string URL_ID_GURU_KELAS_LTS = "xlatesem=EramVuc29uK3N0cm9uZythbWF6b24rc2V4";
            public const string URL_ID_GURU_KELAS_LEDGER = "xlgr=hbmcuY29tL2ludGVyZXN0aW5nLzE3Lw";
            public const string URL_ID_GURU_KELAS_RAPOR = "xgkrpr=Y29tL25ld192aWRlb3MvOS8";
            public const string URL_ID_ADMIN_UNIT = "adxref=j8gj67854ghgfdre456576YTXfghg";

            public static bool IsAdaIDUrlIdAdminUnit() { return (GetQueryString("adxref") == "j8gj67854ghgfdre456576YTXfghg" ? true : false); }
            public static bool IsAdaIDUrlIdGuruKelas() { return (GetQueryString("gkxref") == "g7h89nfkk90ynqwr3" ? true : false); }
            public static bool IsAdaIDUrlLTSGuruKelas() { return (GetQueryString("xlatesem") == "EramVuc29uK3N0cm9uZythbWF6b24rc2V4" ? true : false); }
            public static bool IsAdaIDUrlLedgerGuruKelas() { return (GetQueryString("xlgr") == "hbmcuY29tL2ludGVyZXN0aW5nLzE3Lw" ? true : false); }
            public static bool IsAdaIDUrlRaporGuruKelas() { return (GetQueryString("xgkrpr") == "Y29tL25ld192aWRlb3MvOS8" ? true : false); }
        }

        public static class URL_IDENTIFIER_ORTU
        {
            public const string URL_ID_ORTU = "uikxref=9yr679ftgd4568";
            public static bool IsAdaIDUrlIdOrtu() { return (GetQueryString("uikxref") == "9yr679ftgd4568" ? true : false); }
        }

        public static class JENIS_ABSENSI
        {
            public const string HADIR = "Hadir";
            public const string TERLAMBAT = "Terlambat";
            public const string DITUGASKAN = "Ditugaskan";
            public const string SAKIT = "Sakit";
            public const string IZIN = "Izin";
            public const string ALPA = "Alpa";
        }

        public static string GetNamaPeriodeReportRapor(string tahun_ajaran, string semester, string nama_report)
        {
            return tahun_ajaran.Replace("/", "") + "_SM" + semester + "_" + nama_report;
        }

        public static string GetCleanDeskripsiKD(string teks)
        {
            return
            teks.Replace("LK01/", "")
                .Replace("LK02/", "")
                .Replace("LK03/", "")
                .Replace("LK04/", "")
                .Replace("LK05/", "")
                .Replace("LK06/", "")
                .Replace("LK07/", "")
                .Replace("LK08/", "")
                .Replace("LK09/", "")
                .Replace("LK10/", "")
                .Replace("UH01/", "")
                .Replace("UH02/", "")
                .Replace("UH03/", "")
                .Replace("UH04/", "")
                .Replace("UH05/", "")
                .Replace("UH06/", "")
                .Replace("UH07/", "")
                .Replace("UH08/", "")
                .Replace("UH09/", "")
                .Replace("UH10/", "")
                .Replace("PH01/", "")
                .Replace("PH02/", "")
                .Replace("PH03/", "")
                .Replace("PH04/", "")
                .Replace("PH05/", "")
                .Replace("PH06/", "")
                .Replace("PH07/", "")
                .Replace("PH08/", "")
                .Replace("PH09/", "")
                .Replace("PH10/", "")

                .Replace("LK 01/", "")
                .Replace("LK 02/", "")
                .Replace("LK 03/", "")
                .Replace("LK 04/", "")
                .Replace("LK 05/", "")
                .Replace("LK 06/", "")
                .Replace("LK 07/", "")
                .Replace("LK 08/", "")
                .Replace("LK 09/", "")
                .Replace("LK 10/", "")
                .Replace("UH 01/", "")
                .Replace("UH 02/", "")
                .Replace("UH 03/", "")
                .Replace("UH 04/", "")
                .Replace("UH 05/", "")
                .Replace("UH 06/", "")
                .Replace("UH 07/", "")
                .Replace("UH 08/", "")
                .Replace("UH 09/", "")
                .Replace("UH 10/", "")
                .Replace("PH 01/", "")
                .Replace("PH 02/", "")
                .Replace("PH 03/", "")
                .Replace("PH 04/", "")
                .Replace("PH 05/", "")
                .Replace("PH 06/", "")
                .Replace("PH 07/", "")
                .Replace("PH 08/", "")
                .Replace("PH 09/", "")
                .Replace("PH 10/", "");
        }

        public static class BUTIR_SIKAP
        {
            public const string BERDOA = "Berdoa";
            public const string JUJUR = "Jujur";
            public const string DISIPLIN = "Disiplin";
            public const string BERTANGGUNGJAWAB = "Bertanggungjawab";
            public const string PEDULI = "Peduli";
            public const string MENGHARGAI = "Menghargai";
            public const string LAINNYA = "Lainnya";

            public static string GetHTMLDropdownButirSikap(string id, string name, string selected_value)
            {
                string hasil = "<select onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" title=\" Butir Sikap \" class=\"text-input\" style=\"font-weight: bold; width: 100%; margin-top: 5px; margin-bottom: 5px;\"" +
                                    (id.Trim() != "" ? " id=\"" + id + "\" " : "") +
                                    (name.Trim() != "" ? " name=\"" + name + "\" " : "") +
                               ">";

                hasil += "<option></option>";
                hasil += "<option" + (selected_value.Trim().ToLower() == BERDOA.Trim().ToLower() ? " selected " : "") + " value=\"" + BERDOA + "\">" + BERDOA + "</option>";
                hasil += "<option" + (selected_value.Trim().ToLower() == JUJUR.Trim().ToLower() ? " selected " : "") + "  value=\"" + JUJUR + "\">" + JUJUR + "</option>";
                hasil += "<option" + (selected_value.Trim().ToLower() == DISIPLIN.Trim().ToLower() ? " selected " : "") + "  value=\"" + DISIPLIN + "\">" + DISIPLIN + "</option>";
                hasil += "<option" + (selected_value.Trim().ToLower() == BERTANGGUNGJAWAB.Trim().ToLower() ? " selected " : "") + "  value=\"" + BERTANGGUNGJAWAB + "\">" + BERTANGGUNGJAWAB + "</option>";
                hasil += "<option" + (selected_value.Trim().ToLower() == PEDULI.Trim().ToLower() ? " selected " : "") + "  value=\"" + PEDULI + "\">" + PEDULI + "</option>";
                hasil += "<option" + (selected_value.Trim().ToLower() == MENGHARGAI.Trim().ToLower() ? " selected " : "") + "  value=\"" + MENGHARGAI + "\">" + MENGHARGAI + "</option>";
                hasil += "<option" + (selected_value.Trim().ToLower() == LAINNYA.Trim().ToLower() ? " selected " : "") + "  value=\"" + LAINNYA + "\">" + LAINNYA + "</option>";

                hasil += "</select>";

                return hasil;
            }

            public static string GetHTMLDropdownSikap(string id, string name, string selected_value)
            {
                string hasil = "<select onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" title=\" Sikap \" class=\"text-input\" style=\"font-weight: bold; width: 100%; margin-top: 5px; margin-bottom: 5px;\"" +
                                    (id.Trim() != "" ? " id=\"" + id + "\" " : "") +
                                    (name.Trim() != "" ? " name=\"" + name + "\" " : "") +
                               ">";

                hasil += "<option></option>";
                hasil += "<option" + (selected_value.Trim().ToLower() == "+" ? " selected " : "") + " value=\"+\">Positif (+)</option>";
                hasil += "<option" + (selected_value.Trim().ToLower() == "-" ? " selected " : "") + " value=\"-\">Negatif (-)</option>";

                hasil += "</select>";

                return hasil;
            }
        }

        public static string GetLokasiFolderFileLTS(string rel_siswa, string tahun_ajaran, string semester, string rel_kelasdet, Libs.UnitSekolah unit)
        {
            string tempPath = "~/Application_Files/LTS/";
            string savepath = "";

            switch (unit)
            {
                case UnitSekolah.SALAH:
                    break;
                case UnitSekolah.KB:
                    tempPath += "KB/";
                    break;
                case UnitSekolah.TK:
                    tempPath += "TK/";
                    break;
                case UnitSekolah.SD:
                    tempPath += "SD/";
                    break;
                case UnitSekolah.SMP:
                    tempPath += "SMP/";
                    break;
                case UnitSekolah.SMA:
                    tempPath += "SMA/";
                    break;
                default:
                    break;
            }

            tempPath += tahun_ajaran.Replace("/", "-") + "-0" + semester + "/" + rel_kelasdet + "/" + rel_siswa;
            if (rel_siswa.Trim() != "")
            {
                savepath = tempPath;
            }

            return savepath;
        }

        public static string GetHTMLListUploadedFilesLTS(Page page, string rel_siswa, string tahun_ajaran, string semester, string rel_kelasdet, Libs.UnitSekolah unit, string id, bool show_hapus = true, bool as_info_list = false)
        {
            string lokasi_upload = Libs.GetLokasiFolderFileLTS(
                                rel_siswa, tahun_ajaran, semester, rel_kelasdet, unit
                           );

            string s_unit = ((int)unit).ToString();
            string s_params = "jenis=" + Libs.JENIS_UPLOAD.RAPOR +
                              "&sw=" + rel_siswa +
                              "&ta=" + tahun_ajaran +
                              "&sm=" + semester +
                              "&kd=" + rel_kelasdet +
                              "&un=" + s_unit;

            //show file list
            string list_uploaded = "";

            bool is_show_hapus = show_hapus;
            bool ada_data = false;

            if (Directory.Exists(HttpContext.Current.Server.MapPath(lokasi_upload)))
            {
                int id_file = 0;
                string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(lokasi_upload));
                foreach (string item in filePaths)
                {
                    string ext_file = Path.GetExtension(item).Trim().ToLower();
                    string icon_name = "fa fa-file-o";
                    ada_data = true;

                    switch (ext_file)
                    {
                        case ".pdf":
                            icon_name = "fa fa-file-pdf-o";
                            break;
                        case ".xls":
                        case ".xlsx":
                            icon_name = "fa fa-file-excel-o";
                            break;
                        case ".doc":
                        case ".docx":
                            icon_name = "fa fa-file-word-o";
                            break;
                        case ".ppt":
                        case ".pptx":
                            icon_name = "fa fa-file-powerpoint-o";
                            break;
                        case ".jpg":
                        case ".jpeg":
                        case ".gif":
                        case ".png":
                        case ".bmp":
                            icon_name = "fa fa-file-image-o";
                            break;
                        default:
                            break;
                    }

                    string kode_tr = "tr_" + Guid.NewGuid().ToString().Replace(" - ", "_");
                    string kode_img = "img_" + Guid.NewGuid().ToString().Replace(" - ", "_");
                    string kode_btn = "btn_" + Guid.NewGuid().ToString().Replace(" - ", "_");
                    string lokasi_hapus = HttpContext.Current.Server.MapPath(lokasi_upload + "/" + Path.GetFileName(item)).Replace("\\", "@@");
                    string download_path = HttpContext.Current.Server.MapPath(lokasi_upload + "/" + Path.GetFileName(item));
                    if (as_info_list)
                    {
                        list_uploaded += "<tr id=\"" + kode_tr + "\" style=\"width: 100%; padding: 15px; font-weight: bold; border-bottom-style: dotted; border-bottom-width: 1px; border-bottom-color: #bfbfbf;\">" +
                                            "<td style=\"vertical-align: top; padding: 15px; padding-left: 10px; width: 18px; \">" +
                                                "<i class=\"" + icon_name + "\" style=\"color: grey\"></i>" +
                                            "</td>" +
                                            "<td style=\"vertical-align: top; padding: 15px; padding-left: 0px;\">" +
                                                "<a href=\"" +
                                                        (
                                                            page.ResolveUrl(lokasi_upload + "/" + Path.GetFileName(item))
                                                        )
                                                    + "\" target=\"_blank\" style=\"font-weight: normal; color: #007ACC; font-weight: bold;\">" +
                                                    Path.GetFileNameWithoutExtension(item).Replace("-", " ").ToUpper() +
                                                "</a>" +
                                            "</td>" +
                                            (show_hapus ?
                                                "<td style=\"width: 45px; vertical-align: top; text-align: right; padding: 15px; \">" +
                                                    "<img id=\"" + kode_img + "\" src=\"" + page.ResolveUrl("~/Application_CLibs/images/loading-home.gif") + "\" style=\"display: none; height: 16px; width: 16px;\" />" +
                                                    "<button name=\"hapus_attch[]\" id=\"" + kode_btn + "\" onclick=\"" +
                                                        "if(confirm('Anda yakin akan menghapus file : \\n" + Path.GetFileNameWithoutExtension(item).Replace("-", " ").ToUpper() + "')) { document.getElementById('" + kode_btn + "').style.display = 'none'; document.getElementById('" + kode_img + "').style.display = ''; this.style.display = 'none'; document.getElementById('fraDelete').src = '" + page.ResolveUrl("~/Application_Resources/Delete.aspx") + "?id=" + id.ToString() + "&file=" + Path.GetFileName(item) + "&kode_tr=" + kode_tr + "'; return false; } else { return false; } \" title=\" Hapus File \" style=\"background: transparent; border-style: none; color: #B93221; outline: none;\">" +
                                                        "<i class=\"fa fa-times\"></i>" +
                                                    "</button>" +
                                                "</td>"
                                            : "") +
                                         "</tr>";
                    }
                    else
                    {
                        list_uploaded += "<tr id=\"" + kode_tr + "\" style=\"width: 100%; padding: 15px; font-weight: bold; border-bottom-style: dotted; border-bottom-width: 1px; border-bottom-color: #bfbfbf;\">" +
                                            "<td style=\"vertical-align: top; padding: 15px; padding-left: 10px; width: 18px; \">" +
                                                "<i class=\"" + icon_name + "\" style=\"color: grey\"></i>" +
                                            "</td>" +
                                            "<td style=\"vertical-align: top; padding: 15px; padding-left: 0px;\">" +
                                                "<a href=\"" +
                                                        (
                                                            page.ResolveUrl(lokasi_upload + "/" + Path.GetFileName(item))
                                                        )
                                                    + "\" target=\"_blank\" style=\"font-weight: normal; color: #007ACC; font-weight: bold;\">" +
                                                    Path.GetFileNameWithoutExtension(item).Replace("-", " ").ToUpper() +
                                                "</a>" +
                                            "</td>" +
                                            (show_hapus ?
                                                "<td style=\"width: 45px; vertical-align: top; text-align: right; padding: 15px; \">" +
                                                    "<img id=\"" + kode_img + "\" src=\"" + page.ResolveUrl("~/Application_CLibs/images/loading-home.gif") + "\" style=\"display: none; height: 16px; width: 16px;\" />" +
                                                    "<button name=\"hapus_attch[]\" id=\"" + kode_btn + "\" onclick=\"" +
                                                        "if(confirm('Anda yakin akan menghapus file : \\n" + Path.GetFileNameWithoutExtension(item).Replace("-", " ").ToUpper() + "')) { document.getElementById('" + kode_btn + "').style.display = 'none'; document.getElementById('" + kode_img + "').style.display = ''; this.style.display = 'none'; document.getElementById('fraDelete').src = '" + page.ResolveUrl("~/Application_Resources/Delete.aspx") + "?" + s_params + "&file=" + Path.GetFileName(item) + "&kode_tr=" + kode_tr + "'; return false; } else { return false; } \" title=\" Hapus File \" style=\"background: transparent; border-style: none; color: #B93221; outline: none;\">" +
                                                        "<i class=\"fa fa-times\"></i>" +
                                                    "</button>" +
                                                "</td>"
                                            : "") +
                                         "</tr>";
                    }
                    id_file++;
                }
            }

            if (list_uploaded.Trim() != "")
            {
                list_uploaded = "<table id=\"tbluploadedfiles\" style=\"padding: 0px; width: 100%; margin-top: 0px;\">" +
                                    list_uploaded +
                                "</table>";
            }

            if (!ada_data) list_uploaded = "";

            return list_uploaded;
        }

        public static string GetLokasiFolderFileRapor(string rel_siswa, string tahun_ajaran, string semester, string rel_kelasdet, Libs.UnitSekolah unit)
        {
            string tempPath = "~/Application_Files/Rapor/";
            string savepath = "";

            switch (unit)
            {
                case UnitSekolah.SALAH:
                    break;
                case UnitSekolah.KB:
                    tempPath += "KB/";
                    break;
                case UnitSekolah.TK:
                    tempPath += "TK/";
                    break;
                case UnitSekolah.SD:
                    tempPath += "SD/";
                    break;
                case UnitSekolah.SMP:
                    tempPath += "SMP/";
                    break;
                case UnitSekolah.SMA:
                    tempPath += "SMA/";
                    break;
                default:
                    break;
            }

            tempPath += tahun_ajaran.Replace("/", "-") + "-0" + semester + "/" + rel_kelasdet + "/" + rel_siswa;
            if (rel_siswa.Trim() != "")
            {
                savepath = tempPath;
            }

            return savepath;
        }

        public static string GetHTMLListUploadedFilesRapor(Page page, string rel_siswa, string tahun_ajaran, string semester, string rel_kelasdet, Libs.UnitSekolah unit, string id, string tipe_rapor, bool show_hapus = true, bool as_info_list = false)
        {
            string lokasi_upload = "";
            if (tipe_rapor.Trim().ToUpper() == TipeRapor.LTS.Trim().ToUpper())
            {
                lokasi_upload = Libs.GetLokasiFolderFileLTS(
                                rel_siswa, tahun_ajaran, semester, rel_kelasdet, unit
                           );
            }
            else if (tipe_rapor.Trim().ToUpper() == TipeRapor.SEMESTER.Trim().ToUpper())
            {
                lokasi_upload = Libs.GetLokasiFolderFileRapor(
                                rel_siswa, tahun_ajaran, semester, rel_kelasdet, unit
                           );
            }

            string s_unit = ((int)unit).ToString();
            string s_params = "jenis=" + Libs.JENIS_UPLOAD.RAPOR +
                              "&sw=" + rel_siswa +
                              "&ta=" + tahun_ajaran +
                              "&sm=" + semester +
                              "&kd=" + rel_kelasdet +
                              "&un=" + s_unit +
                              "&tr=" + tipe_rapor;

            //show file list
            string list_uploaded = "";

            bool is_show_hapus = show_hapus;
            bool ada_data = false;

            if (Directory.Exists(HttpContext.Current.Server.MapPath(lokasi_upload)))
            {
                int id_file = 0;
                string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(lokasi_upload));
                foreach (string item in filePaths)
                {
                    string ext_file = Path.GetExtension(item).Trim().ToLower();
                    string icon_name = "fa fa-file-o";
                    ada_data = true;

                    switch (ext_file)
                    {
                        case ".pdf":
                            icon_name = "fa fa-file-pdf-o";
                            break;
                        case ".xls":
                        case ".xlsx":
                            icon_name = "fa fa-file-excel-o";
                            break;
                        case ".doc":
                        case ".docx":
                            icon_name = "fa fa-file-word-o";
                            break;
                        case ".ppt":
                        case ".pptx":
                            icon_name = "fa fa-file-powerpoint-o";
                            break;
                        case ".jpg":
                        case ".jpeg":
                        case ".gif":
                        case ".png":
                        case ".bmp":
                            icon_name = "fa fa-file-image-o";
                            break;
                        default:
                            break;
                    }

                    string kode_tr = "tr_" + Guid.NewGuid().ToString().Replace(" - ", "_");
                    string kode_img = "img_" + Guid.NewGuid().ToString().Replace(" - ", "_");
                    string kode_btn = "btn_" + Guid.NewGuid().ToString().Replace(" - ", "_");
                    string lokasi_hapus = HttpContext.Current.Server.MapPath(lokasi_upload + "/" + Path.GetFileName(item)).Replace("\\", "@@");
                    string download_path = HttpContext.Current.Server.MapPath(lokasi_upload + "/" + Path.GetFileName(item));
                    if (as_info_list)
                    {
                        list_uploaded += "<tr id=\"" + kode_tr + "\" style=\"width: 100%; padding: 15px; font-weight: bold; border-bottom-style: dotted; border-bottom-width: 1px; border-bottom-color: #bfbfbf;\">" +
                                            "<td style=\"vertical-align: top; padding: 15px; padding-left: 10px; width: 18px; \">" +
                                                "<i class=\"" + icon_name + "\" style=\"color: grey\"></i>" +
                                            "</td>" +
                                            "<td style=\"vertical-align: top; padding: 15px; padding-left: 0px;\">" +
                                                "<a href=\"" +
                                                        (
                                                            page.ResolveUrl(lokasi_upload + "/" + Path.GetFileName(item))
                                                        )
                                                    + "\" target=\"_blank\" style=\"font-weight: normal; color: #007ACC; font-weight: bold;\">" +
                                                    //Path.GetFileNameWithoutExtension(item).Replace("-", " ").ToUpper() +
                                                    Path.GetFileNameWithoutExtension(item).Replace("-", "/").ToUpper() +
                                                "</a>" +
                                            "</td>" +
                                            (show_hapus ?
                                                "<td style=\"width: 45px; vertical-align: top; text-align: right; padding: 15px; \">" +
                                                    "<img id=\"" + kode_img + "\" src=\"" + page.ResolveUrl("~/Application_CLibs/images/loading-home.gif") + "\" style=\"display: none; height: 16px; width: 16px;\" />" +
                                                    "<button name=\"hapus_attch[]\" id=\"" + kode_btn + "\" onclick=\"" +
                                                        "if(confirm('Anda yakin akan menghapus file : \\n" + Path.GetFileNameWithoutExtension(item).Replace("-", "/").ToUpper() + "')) { document.getElementById('" + kode_btn + "').style.display = 'none'; document.getElementById('" + kode_img + "').style.display = ''; this.style.display = 'none'; document.getElementById('fraDelete').src = '" + page.ResolveUrl("~/Application_Resources/Delete.aspx") + "?id=" + id.ToString() + "&file=" + Path.GetFileName(item) + "&kode_tr=" + kode_tr + "'; return false; } else { return false; } \" title=\" Hapus File \" style=\"background: transparent; border-style: none; color: #B93221; outline: none;\">" +
                                                        "<i class=\"fa fa-times\"></i>" +
                                                    "</button>" +
                                                "</td>"
                                            : "") +
                                         "</tr>";
                    }
                    else
                    {
                        list_uploaded += "<tr id=\"" + kode_tr + "\" style=\"width: 100%; padding: 15px; font-weight: bold; border-bottom-style: dotted; border-bottom-width: 1px; border-bottom-color: #bfbfbf;\">" +
                                            "<td style=\"vertical-align: top; padding: 15px; padding-left: 10px; width: 18px; \">" +
                                                "<i class=\"" + icon_name + "\" style=\"color: grey\"></i>" +
                                            "</td>" +
                                            "<td style=\"vertical-align: top; padding: 15px; padding-left: 0px;\">" +
                                                "<a href=\"" +
                                                        (   
                                                            page.ResolveUrl(lokasi_upload + "/" + Path.GetFileName(item))
                                                        )
                                                    + "\" target=\"_blank\" style=\"font-weight: normal; color: #007ACC; font-weight: bold;\">" +
                                                    Path.GetFileNameWithoutExtension(item).Replace("-", "/").ToUpper() +
                                                "</a>" +
                                            "</td>" +
                                            (show_hapus ?
                                                "<td style=\"width: 45px; vertical-align: top; text-align: right; padding: 15px; \">" +
                                                    "<img id=\"" + kode_img + "\" src=\"" + page.ResolveUrl("~/Application_CLibs/images/loading-home.gif") + "\" style=\"display: none; height: 16px; width: 16px;\" />" +
                                                    "<button name=\"hapus_attch[]\" id=\"" + kode_btn + "\" onclick=\"" +
                                                        "if(confirm('Anda yakin akan menghapus file : \\n" + Path.GetFileNameWithoutExtension(item).Replace("-", "/").ToUpper() + "')) { document.getElementById('" + kode_btn + "').style.display = 'none'; document.getElementById('" + kode_img + "').style.display = ''; this.style.display = 'none'; document.getElementById('fraDelete').src = '" + page.ResolveUrl("~/Application_Resources/Delete.aspx") + "?" + s_params + "&file=" + Path.GetFileName(item) + "&kode_tr=" + kode_tr + "'; return false; } else { return false; } \" title=\" Hapus File \" style=\"background: transparent; border-style: none; color: #B93221; outline: none;\">" +
                                                        "<i class=\"fa fa-times\"></i>" +
                                                    "</button>" +
                                                "</td>"
                                            : "") +
                                         "</tr>";
                    }
                    id_file++;
                }
            }

            if (list_uploaded.Trim() != "")
            {
                list_uploaded = "<table id=\"tbluploadedfiles\" style=\"padding: 0px; width: 100%; margin-top: 0px;\">" +
                                    list_uploaded +
                                "</table>";
            }

            if (!ada_data) list_uploaded = "";

            return list_uploaded;
        }

        public static List<string> GetListUploadedFiles(Page page, string lokasi_upload)
        {
            //show file list
            List<string> hasil = new List<string>();

            if (Directory.Exists(HttpContext.Current.Server.MapPath(lokasi_upload)))
            {
                string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(lokasi_upload));
                foreach (string item in filePaths)
                {
                    hasil.Add(item);
                }
            }

            return hasil;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static class JENIS_UPLOAD
        {
            public const string MATERI_PEMBELAJARAN = "16jNotKQR_yTco";
            public const string PENDIDIKAN_NON_FORMAL = "5ab7bbe4947b4";
            public const string FILE_PENDUKUNG = "ytIHGyDgNWO6VbHlQ";
            public const string RIWAYAT_MCU = "UC6MTowFYbG8SK5";
            public const string RIWAYAT_KESEHATAN = "bG8SK5GvTWjxSvg";
            public const string RAPOR = "7Xi0GB3teRqkuBusUEcVrP6OlYpD9w";
        }

        public static class NILAI_SIKAP_KURTILAS
        {
            public const string BAIK = "Baik";
            public const string SANGAT_BAIK = "Sangat Baik";
            public static List<string> GetListNilaiSikap()
            {
                List<string> hasil = new List<string>();

                hasil.Add(BAIK);
                hasil.Add(SANGAT_BAIK);

                return hasil;
            }
        }

        public enum UnitSekolah
        {
            SALAH = 99,
            KB = 1, TK = 2, SD = 3, SMP = 4, SMA = 5
        }

        public static class JENIS_LINIMASA
        {
            public const string ABSEN_SISWA_HARIAN = "ABSEN_SISWA_HARIAN";
            public const string ABSEN_SISWA_MAPEL = "ABSEN_SISWA_MAPEL";

            public static string GetDeskripsiJenisLiniMasa(string jenis)
            {
                switch (jenis)
                {
                    case ABSEN_SISWA_HARIAN:
                        return "Absensi harian Siswa";
                    case ABSEN_SISWA_MAPEL:
                        return "Absensi Siswa per Mata Pelajaran";
                }
                return "";
            }
        }

        public static string GetColHeader(int nomor)
        {
            string huruf = Application_Libs.Constantas.ALFABET;

            if (nomor <= huruf.Length)
            {
                return huruf.Substring(nomor - 1, 1);
            }
            else
            {
                int i_kelipatan = Convert.ToInt16(Math.Floor(Convert.ToDecimal(nomor / huruf.Length)));
                int i_id_huruf = nomor -
                                 (i_kelipatan * huruf.Length);
                if (i_id_huruf == 0)
                {
                    i_kelipatan--;
                    i_id_huruf = huruf.Length;
                }

                return huruf.Substring(i_kelipatan - 1, 1) +
                       huruf.Substring(i_id_huruf - 1, 1);
            }
        }

        //public static string GetHTMLLinkLTS(Page page, string tahun_ajaran, string semester, string rel_kelasdet, string rel_siswa, bool is_button_link, string kode_email, string teks_link_buka_lts, string arr_string_url = "")
        //{
        //    string html = "";
        //    string url = "";

        //    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelasdet);
        //    if (m_kelas_det != null)
        //    {
        //        if (m_kelas_det.Nama != null)
        //        {

        //            Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
        //            if (m_kelas != null)
        //            {
        //                if (m_kelas.Nama != null)
        //                {

        //                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
        //                    if (m_sekolah != null)
        //                    {
        //                        if (m_sekolah.Nama != null)
        //                        {

        //                            switch ((UnitSekolah)m_sekolah.UrutanJenjang)
        //                            {
        //                                case UnitSekolah.SALAH:
        //                                    return "";
        //                                case UnitSekolah.KB:
        //                                    return "";
        //                                case UnitSekolah.TK:
        //                                    return "";
        //                                case UnitSekolah.SD:
        //                                    url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
        //                                          page.ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CETAK_LTS.ROUTE) +
        //                                          "?sis=" + rel_siswa +
        //                                          "&t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) +
        //                                          "&s=" + semester +
        //                                          "&kd=" + rel_kelasdet +
        //                                          "&token=" + Application_Libs.Constantas.TOKEN_VIEW_ORTU +
        //                                          (
        //                                            kode_email.Trim() != ""
        //                                            ? "&idm=" + kode_email
        //                                            : ""
        //                                          ) + arr_string_url;
        //                                    break;
        //                                case UnitSekolah.SMP:
        //                                    url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + 
        //                                          page.ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CETAK_LTS.ROUTE) +
        //                                          "?sis=" + rel_siswa +
        //                                          "&t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) +
        //                                          "&s=" + semester +
        //                                          "&kd=" + rel_kelasdet +
        //                                          "&token=" + Application_Libs.Constantas.TOKEN_VIEW_ORTU +
        //                                          (
        //                                            kode_email.Trim() != ""
        //                                            ? "&idm=" + kode_email
        //                                            : ""
        //                                          ) + arr_string_url;
        //                                    break;
        //                                case UnitSekolah.SMA:
        //                                    url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + 
        //                                          page.ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.ROUTE) +
        //                                          "?j=" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_LTS_SMA +
        //                                          "&sw=" + rel_siswa +
        //                                          "&t=" + tahun_ajaran.Replace("/", "-") +
        //                                          "&s=" + semester +
        //                                          "&kd=" + rel_kelasdet +
        //                                          "&token=" + Application_Libs.Constantas.TOKEN_VIEW_ORTU +
        //                                          (
        //                                            kode_email.Trim() != ""
        //                                            ? "&idm=" + kode_email
        //                                            : ""
        //                                          ) + arr_string_url;
        //                                    break;
        //                                default:
        //                                    return "";
        //                            }

        //                        }
        //                    }

        //                }
        //            }

        //        }
        //    }

        //    if (is_button_link)
        //    {
        //        html = "<a href=\"" + url + "\" target=\"_blank\" style=\"cursor: pointer; border-radius: 5px; background-color: #1DA1F2; color: white; font-weight: bold; padding: 10px; text-decoration: none;\">" +
        //                    "&nbsp;&nbsp;" + teks_link_buka_lts + "&nbsp;&nbsp;" +
        //               "</a>";
        //    }
        //    else
        //    {
        //        html = "<a href=\"" + url + "\" target=\"_blank\" style=\"cursor: pointer; border-radius: 5px; color: #1DA1F2; font-weight: bold; text-decoration: none;\">" +
        //                    "&nbsp;&nbsp;" + teks_link_buka_lts + "&nbsp;&nbsp;" +
        //               "</a>";
        //    }

        //    return html;
        //}

        public static string GetHTMLLinkLTS(Page page, string tahun_ajaran, string semester, string rel_kelasdet, string rel_siswa, bool is_button_link, string teks_link_buka_rapor, string kode_email, string tipe_rapor, string arr_string_url = "")
        {
            string html = "";
            string url = "";

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelasdet);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {

                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {

                            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                            if (m_sekolah != null)
                            {
                                if (m_sekolah.Nama != null)
                                {
                                    url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                                          page.ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR_VIEW.ROUTE) +
                                          "?" +
                                          "sw=" + rel_siswa +
                                          "&t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) +
                                          "&sm=" + semester +
                                          "&kd=" + rel_kelasdet +
                                          "&token=" + Application_Libs.Constantas.TOKEN_VIEW_ORTU +
                                          "&u=" + m_sekolah.Kode.ToString() +
                                          "&tr=" + tipe_rapor +
                                          (
                                            kode_email.Trim() != ""
                                            ? "&idm=" + kode_email
                                            : ""
                                          ) +
                                          arr_string_url;

                                }
                            }

                        }
                    }

                }
            }

            if (is_button_link)
            {
                html = "<a href=\"" + url + "\" target=\"_blank\" style=\"cursor: pointer; border-radius: 5px; background-color: #1DA1F2; color: white; font-weight: bold; padding: 10px; text-decoration: none;\">" +
                            "&nbsp;&nbsp;" + teks_link_buka_rapor + "&nbsp;&nbsp;" +
                       "</a>";
            }
            else
            {
                html = "<a href=\"" + url + "\" target=\"_blank\" style=\"cursor: pointer; border-radius: 5px; color: #1DA1F2; font-weight: bold; text-decoration: none;\">" +
                            "&nbsp;&nbsp;" + teks_link_buka_rapor + "&nbsp;&nbsp;" +
                       "</a>";
            }

            return html;
        }


        public static string GetHTMLLinkRapor(Page page, string tahun_ajaran, string semester, string rel_kelasdet, string rel_siswa, bool is_button_link, string teks_link_buka_rapor, string kode_email, string tipe_rapor, string arr_string_url = "")
        {
            string html = "";
            string url = "";

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelasdet);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {

                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {

                            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                            if (m_sekolah != null)
                            {
                                if (m_sekolah.Nama != null)
                                {
                                    url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                                          page.ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR_VIEW.ROUTE) +
                                          "?" +
                                          "sw=" + rel_siswa +
                                          "&t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) +
                                          "&sm=" + semester +
                                          "&kd=" + rel_kelasdet +
                                          "&token=" + Application_Libs.Constantas.TOKEN_VIEW_ORTU +
                                          "&u=" + m_sekolah.Kode.ToString() +
                                          "&tr=" + tipe_rapor +
                                          (
                                            kode_email.Trim() != ""
                                            ? "&idm=" + kode_email
                                            : ""
                                          ) +
                                          arr_string_url;

                                }
                            }

                        }
                    }

                }
            }

            if (is_button_link)
            {
                html = "<a href=\"" + url + "\" target=\"_blank\" style=\"cursor: pointer; border-radius: 5px; background-color: #1DA1F2; color: white; font-weight: bold; padding: 10px; text-decoration: none;\">" +
                            "&nbsp;&nbsp;" + teks_link_buka_rapor + "&nbsp;&nbsp;" +
                       "</a>";
            }
            else
            {
                html = "<a href=\"" + url + "\" target=\"_blank\" style=\"cursor: pointer; border-radius: 5px; color: #1DA1F2; font-weight: bold; text-decoration: none;\">" +
                            "&nbsp;&nbsp;" + teks_link_buka_rapor + "&nbsp;&nbsp;" +
                       "</a>";
            }

            return html;
        }

        public static string GetHTMLEmailTemplate_OLD0(string email_body, string rel_sekolah, bool use_border_top = true)
        {
            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(
                    rel_sekolah
                );
            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {
                    string s_judul = "<span style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:24px;color:gray;font-weight:bold;\">Al-Izhar Pondok Labu</span><br />";
                    string s_alamat = "<span style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:12px;color:gray;\">" +
                                         "Jl. RS Fatmawati Kav.49, Pondok Labu, Cilandak, Kota Jakarta Selatan, Daerah Khusus Ibukota Jakarta 12450" +
                                      "</span>";

                    switch ((Libs.UnitSekolah) m_sekolah.UrutanJenjang)
                    {
                        case UnitSekolah.SALAH:
                            break;
                        case UnitSekolah.KB:
                            break;
                        case UnitSekolah.TK:
                            break;
                        case UnitSekolah.SD:
                            var lst_settings_sd = AI_ERP.Application_DAOs.DAO_PengaturanSD.GetAll_Entity();
                            if (lst_settings_sd.Count > 0)
                            {
                                var settings = lst_settings_sd.FirstOrDefault();
                                if (settings != null)
                                {
                                    if (settings.HeaderKop != null)
                                    {
                                        s_judul = "<span style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;color:black;\">" +
                                                    settings.HeaderKop +
                                                  "</span>";
                                        s_alamat = "<span style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;color:black;\">" +
                                                    settings.HeaderAlamat +
                                                  "</span>";
                                    }
                                }
                            }
                            break;
                        case UnitSekolah.SMP:
                            break;
                        case UnitSekolah.SMA:
                            var lst_settings_sma = AI_ERP.Application_DAOs.DAO_PengaturanSMA.GetAll_Entity();
                            if (lst_settings_sma.Count > 0)
                            {
                                var settings = lst_settings_sma.FirstOrDefault();
                                if (settings != null)
                                {
                                    if (settings.HeaderKop != null)
                                    {
                                        s_judul = "<span style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;color:black;\">" +
                                                    settings.HeaderKop +
                                                  "</span>";
                                        s_alamat = "<span style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;color:black;\">" +
                                                    settings.HeaderAlamat +
                                                  "</span>";
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    return "<table width=\"100%\" height=\"100%\" style=\"min-width:348px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                            "	<tbody>" +
                            "		<tr align=\"center\">" +
                            "			<td style=\"padding: 0px; vertical-align: top;\">" +
                            "				<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"width: 100%;\">" +
                            "					<tbody>" +
                            "						<tr>" +
                            "			                <td style=\"padding: 0px;\">" +
                            "								<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin:0 auto;display:table;min-width:332px;width:100%;border:1px solid #f0f0f0;border-bottom:0;border-top-left-radius:3px;border-top-right-radius:3px;" + (!use_border_top ? "border-top-style: none;" : "") + "\">" +
                            "									<tbody>" +
                            "										<tr>" +
                            "											<td align=\"left\" style=\"vertical-align: top; width: 80px; padding: 15px; padding-left: 30px; background-color: white;\">" +
                            "												<img src=\"http://aplikasi.alizhar.sch.id/res/images/logo-trans.png\" class=\"CToWUd\" style=\"margin-right: 15px;\">" +
                            "											</td>" +
                            "											<td align=\"left\" style=\"padding: 15px; background-color: white;\">" +
                            "												" + s_judul +
                            //"												<br />" +
                            "												" + s_alamat +
                            "											</td>" +
                            "											<td align=\"right\" style=\"padding: 15px; background-color: white;\">" +
                            "												&nbsp;" +
                            "											</td>" +
                            "										</tr>" +
                            "									</tbody>" +
                            "								</table>" +
                            "							</td>" +
                            "						</tr>" +
                            "						<tr>" +
                            "							<td style=\"padding: 0px; background-color: #FAFAFA;\">" +
                            "								<table bgcolor=\"#FAFAFA\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin:0 auto;display:table;min-width:332px;width:100%;border:1px solid #f0f0f0;border-bottom:1px solid #c0c0c0;border-top:0;border-bottom-left-radius:3px;border-bottom-right-radius:3px\">" +
                            "									<tbody>" +
                            "										<tr height=\"16px\">" +
                            "											<td width=\"32px\" rowspan=\"3\" style=\"background-color: #FAFAFA; padding: 0px;\"></td>" +
                            "											<td style=\"background-color: #FAFAFA; padding: 0px;\"></td>" +
                            "											<td width=\"32px\" rowspan=\"3\" style=\"background-color: #FAFAFA; padding: 0px;\"></td>" +
                            "										</tr>" +
                            "										<tr>" +
                            "											<td style=\"background-color: #FAFAFA; padding: 0px;\">" +
                            "												<table style=\"min-width:300px;width:100%;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                            "													<tbody>" +
                            "														<tr>" +
                            "															<td style=\"background-color: #FAFAFA; padding: 0px;font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:13px;color:#202020;line-height:1.5\">" +
                                                                                            email_body +
                            "															</td>" +
                            "														</tr>" +
                            "													</tbody>" +
                            "												</table>" +
                            "											</td>" +
                            "										</tr>" +
                            "										<tr height=\"32px\"><td style=\"background-color: #FAFAFA; padding: 0px;\"></td></tr>" +
                            "									</tbody>" +
                            "								</table>" +
                            "							</td>" +
                            "						</tr>" +
                            "						<tr>" +
                            "							<td style=\"background-color: #FAFAFA; padding: 0px;max-width:600px;font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:10px;color:#bcbcbc;line-height:1.5\"></td>" +
                            "						</tr>" +
                            "					</tbody>" +
                            "				</table>" +
                            "			</td>" +
                            "		</tr>" +
                            "	</tbody>" +
                            "</table>";
                }
            }

            return "";
        }

        public static class JenisKomponenNilaiKTSP
        {
            public static class SMA
            {
                public const string PPK = "PPK";
                public const string PRAKTIK = "Praktik";
                
                public static void ListToDropdown(DropDownList cbo, bool add_kosong = true)
                {
                    cbo.Items.Clear();
                    if (add_kosong) cbo.Items.Add("");
                    cbo.Items.Add(new ListItem { Value = PPK, Text = PPK });
                    cbo.Items.Add(new ListItem { Value = PRAKTIK, Text = PRAKTIK });
                }
            }
        }

        public static class JenisKomponenNilaiKURTILAS
        {
            public static class SMA
            {
                public const string PENGETAHUAN = "Pengetahuan";
                public const string KETERAMPILAN = "Keterampilan";
                public const string UAS = "UAS";
                public const string TOTAL = "TOTAL";

                public static void ListToDropdown(DropDownList cbo, bool add_kosong = true)
                {
                    cbo.Items.Clear();
                    if (add_kosong) cbo.Items.Add("");
                    cbo.Items.Add(new ListItem { Value = PENGETAHUAN, Text = PENGETAHUAN });
                    cbo.Items.Add(new ListItem { Value = KETERAMPILAN, Text = KETERAMPILAN });
                }
            }
        }

        public static class JenisKurikulum
        {
            public static class SD
            {
                public const string KTSP = "KTSP";
                public const string KURTILAS = "KURTILAS";
            }

            public static class SMP
            {
                public const string KTSP = "KTSP";
                public const string KURTILAS = "KURTILAS";
                public const string KURTILAS_SIKAP = "KURTILAS-SIKAP";
            }

            public static class SMA
            {
                public const string KTSP = "KTSP";
                public const string KURTILAS = "KURTILAS";
                public const string KURTILAS_SIKAP = "KURTILAS-SIKAP";
            }
        }

        public static class AsalSekolahAlizhar
        {
            public const string KB = "KB AL-IZHAR PONDOK LABU";
            public const string TK = "TK ISLAM AL-IZHAR PONDOK LABU";
            public const string SD = "SD ISLAM AL-IZHAR PONDOK LABU";
            public const string SMP = "SMP ISLAM AL-IZHAR PONDOK LABU";
            public const string SMA = "SMA ISLAM AL-IZHAR PONDOK LABU";
        }

        public static class LoginWawancara
        {
            public const string USER_ID = "wawancara";
            public const string PASSWORD = "alizhar.wawancara";
        }

        public class LoginModel
        {
            public string UserID { get; set; }
            public string Password { get; set; }
            public string Message { get; set; }
        }

        public const string KODE_NIS_SEMENTARA = "NS";

        public const string KODE_TRANS_PERMATA_NS_NO_SEKOLAH = "866";
        public const string KODE_TRANS_PERMATA_DEFAULT = "899";

        public const string CURRENCY_TO_HTML = "<span class=\"pull-left\" style=\"font-weight: normal;\">Rp.</span>";

        public static string GetURLBGHeaderKB(Page page) { return page.ResolveUrl("~/Application_Templates/front/img/slider/KB-header.jpg"); }
        public static string GetURLBGHeaderTK(Page page) { return page.ResolveUrl("~/Application_Templates/front/img/slider/TK-header.jpg"); }
        public static string GetURLBGHeaderSD(Page page) { return page.ResolveUrl("~/Application_Templates/front/img/slider/SD-header.jpg"); }
        public static string GetURLBGHeaderSMP(Page page) { return page.ResolveUrl("~/Application_Templates/front/img/slider/SMP-header.jpg"); }
        public static string GetURLBGHeaderSMA(Page page) { return page.ResolveUrl("~/Application_Templates/front/img/slider/SMA-header.jpg"); }
        public static string GetURLImagePermata(Page page) { return page.ResolveUrl("~/Application_CLibs/images/permata.png"); }
        public static string GetURLImageMandiri(Page page) { return page.ResolveUrl("~/Application_CLibs/images/mandiri.png"); }
        public static string GetURLImageBank(Page page, string bank)
        {
            string hasil = "";

            if (bank.ToUpper().Trim() == VABank.Mandiri.ToString().ToUpper())
            {
                hasil = GetURLImageMandiri(page);
            }
            else if (bank.ToUpper().Trim() == VABank.Permata.ToString().ToUpper())
            {
                hasil = GetURLImagePermata(page);
            }

            return hasil;
        }

        private const string Angka = "1234567890";
        private static CultureInfo iCultur = new CultureInfo("id-ID");

        public const string PEMBATAS_PARSER = "{($_$)}";

        public static string[] Array_Hari = { "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" };
        public static string[] Array_Hari_Id = { "0", "1", "2", "3", "4", "5", "6" };
        public static string[] Array_Hari_Kerja = { "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu", "Minggu" };
        public static string[] Array_Hari_Kerja_Id = { "1", "2", "3", "4", "5", "6", "0" };

        public static string[] Array_Bulan = {
                                                 "Januari", "Februari","Maret","April",
                                                 "Mei", "Juni","Juli","Agustus",
                                                 "September", "Oktober","November","Desember"
                                             };

        public static string[] Array_Bulan_Singkat = {
                                                         "Jan", "Feb","Mar","Apr",
                                                         "Mei", "Jun","Jul","Ags",
                                                         "Sep", "Okt","Nov","Des"
                                                     };

        public static string[] Array_Bulan_Tahun_Ajaran = {
                                                 "Juli","Agustus", "September", "Oktober","November","Desember",
                                                 "Januari", "Februari","Maret","April","Mei", "Juni"
                                             };

        public enum Bulan {
            Januari,
            Februari,
            Maret,
            April,
            Mei,
            Juni,
            Juli,
            Agustus,
            September,
            Oktober,
            November,
            Desember
        };

        public enum BulanTahunAjaran
        {
            Juli,
            Agustus,
            September,
            Oktober,
            November,
            Desember,
            Januari,
            Februari,
            Maret,
            April,
            Mei,
            Juni
        };

        public static string GetDeskripsiJenisPerhitungan(JenisPerhitunganNilai jenis)
        {
            switch (jenis)
            {
                case JenisPerhitunganNilai.Bobot:
                    return "Bobot";
                case JenisPerhitunganNilai.RataRata:
                    return "Rata-Rata";
                default:
                    break;
            }

            return "";
        }

        public static string GetURLImageFoto(string url_image) { return GetImageFotoURL(url_image); }

        public static string GetImageFotoURL(string url_image)
        {
            string url = url_image.Replace("~", "");
            if (!File.Exists(HttpContext.Current.Server.MapPath(url)))
            {
                return "~/Application_Controls/Res/avatar-001.jpg";
            }

            return url_image;
        }

        public static string GetPersingkatKalimat(string teks, int max_length = 100)
        {
            if (teks.Length > max_length)
            {
                return teks.Substring(0, max_length) + "...";
            }

            return teks;
        }

        public static string GetFolderElearningMateriPembelajaran(string id, string id_materi)
        {
            //struktur path : {folder}/{tahun ajaran}/{unit}/{id}/{id materi}
            string tempPath = "~/" + System.Configuration.ConfigurationManager.AppSettings["FolderPath"] + "/" +
                                     "Elearning/Materi";
            string savepath = "";

            if (id.Trim() != "")
            {
                AI_ERP.Application_Entities.Elearning.Praota m_materi = Application_DAOs.Elearning.DAO_Praota.GetByID_Entity(id);
                if (m_materi != null)
                {
                    if (m_materi.Rel_Guru != null)
                    {
                        AI_ERP.Application_Entities.Sekolah m_sekolah = Application_DAOs.DAO_Sekolah.GetByID_Entity(m_materi.Rel_Sekolah);
                        if (m_sekolah != null)
                        {
                            if (m_sekolah.Nama != null)
                            {
                                savepath = tempPath + "/" +
                                           m_materi.TahunAjaran.Replace("/", "-") + "/" +
                                           m_sekolah.UrutanJenjang + "/" +
                                           m_materi.Kode.ToString() +
                                           (id_materi.Trim() != "" ? "/" : "") +
                                           id_materi;
                            }
                        }                        
                    }
                }
            }

            return savepath;
        }

        public static string GetFolderPendidikanNonFormal(string nik, string id_pendidikan_non_formal)
        {
            //struktur path : {folder}/{nik}/{id pendidikan non formal}
            string tempPath = "~/" + System.Configuration.ConfigurationManager.AppSettings["FolderPath"] + "/" +
                                     "Master/Pegawai";
            string savepath = "";

            if (nik.Trim() != "")
            {
                AI_ERP.Application_Entities.Pegawai m_pegawai = Application_DAOs.DAO_Pegawai.GetByID_Entity(nik);
                if (m_pegawai != null)
                {
                    if (m_pegawai.Nama != null)
                    {
                        savepath = tempPath + "/" +
                                   m_pegawai.Kode.Replace("/", "-") + "/" +
                                   "Pendidikan Non Formal/" +
                                   id_pendidikan_non_formal;
                    }
                }
            }

            return savepath;
        }

        public static string GetFolderRiwayatKesehatan(string nik, string id_riwayat_kesehatan)
        {
            //struktur path : {folder}/{nik}/{id riwayat kesehatan}
            string tempPath = "~/" + System.Configuration.ConfigurationManager.AppSettings["FolderPath"] + "/" +
                                     "Master/Pegawai";
            string savepath = "";

            if (nik.Trim() != "")
            {
                AI_ERP.Application_Entities.Pegawai m_pegawai = Application_DAOs.DAO_Pegawai.GetByID_Entity(nik);
                if (m_pegawai != null)
                {
                    if (m_pegawai.Nama != null)
                    {
                        savepath = tempPath + "/" +
                                   m_pegawai.Kode.Replace("/", "-") + "/" +
                                   "Riwayat Kesehatan/" +
                                   id_riwayat_kesehatan;
                    }
                }
            }

            return savepath;
        }

        public static string GetFolderRiwayatMCU(string nik, string id_riwayat_mcu)
        {
            //struktur path : {folder}/{nik}/{id riwayat mcu}
            string tempPath = "~/" + System.Configuration.ConfigurationManager.AppSettings["FolderPath"] + "/" +
                                     "Master/Pegawai";
            string savepath = "";

            if (nik.Trim() != "")
            {
                AI_ERP.Application_Entities.Pegawai m_pegawai = Application_DAOs.DAO_Pegawai.GetByID_Entity(nik);
                if (m_pegawai != null)
                {
                    if (m_pegawai.Nama != null)
                    {
                        savepath = tempPath + "/" +
                                   m_pegawai.Kode.Replace("/", "-") + "/" +
                                   "Riwayat MCU/" +
                                   id_riwayat_mcu;
                    }
                }
            }

            return savepath;
        }

        public static string GetFolderFilePendukung(string nik, string folder)
        {
            //struktur path : {folder}/{nik}/{id pendidikan non formal}
            string tempPath = "~/" + System.Configuration.ConfigurationManager.AppSettings["FolderPath"] + "/" +
                                     "Master/Pegawai";
            string savepath = "";

            if (nik.Trim() != "")
            {
                AI_ERP.Application_Entities.Pegawai m_pegawai = Application_DAOs.DAO_Pegawai.GetByID_Entity(nik);
                if (m_pegawai != null)
                {
                    if (m_pegawai.Nama != null)
                    {
                        savepath = tempPath + "/" +
                                   nik + "/" +
                                   folder;
                    }
                }
            }

            return savepath;
        }

        public static void ListUnitSekolahToDropDown(DropDownList cbo, string item_kosong = "", string value_kosong = "")
        {
            //list kelas
            cbo.Items.Clear();
            cbo.Items.Add(new ListItem { Value = value_kosong, Text = item_kosong });
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            foreach (Sekolah sekolah in lst_sekolah)
            {
                cbo.Items.Add(new ListItem
                {
                    Value = sekolah.Kode.ToString().ToUpper(),
                    Text = sekolah.Nama
                });
            }
        }

        public static string GetURLPenilaian(string rel_kelas_det)
        {
            string url_penilaian = "";
            string rel_kelas = "";
            KelasDet m_kelas_det = Application_DAOs.DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    rel_kelas = m_kelas_det.Rel_Kelas.ToString();
                }
            }
            if (rel_kelas.Trim() != "")
            {
                Kelas m_kelas = AI_ERP.Application_DAOs.DAO_Kelas.GetByID_Entity(rel_kelas);
                if (m_kelas != null)
                {
                    if (m_kelas.Nama != null)
                    {
                        Sekolah m_sekolah = AI_ERP.Application_DAOs.DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.KB)
                                {
                                    url_penilaian = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.NILAI_SISWA.ROUTE;
                                }
                                else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.TK)
                                {
                                    url_penilaian = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.NILAI_SISWA.ROUTE;
                                }
                                else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SD)
                                {
                                    url_penilaian = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA.ROUTE;
                                }
                                else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMP)
                                {
                                    url_penilaian = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA.ROUTE;
                                }
                                else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA)
                                {
                                    url_penilaian = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.ROUTE;
                                }
                            }
                        }
                    }
                }
            }

            return url_penilaian;
        }

        public static string GetPersingkatNama(string s_nama, int jml_suku_kata = 2)
        {
            string[] arr_nama = s_nama.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            int suku_kata = 0;
            s_nama = "";
            foreach (string nama in arr_nama)
            {
                suku_kata++;
                s_nama += (suku_kata > jml_suku_kata ? nama.Substring(0, 1).ToUpper() + "." : nama) + " ";
            }

            return s_nama;
        }

        public static string GetNamaPanggilan(string s_nama)
        {
            string[] arr_nama = s_nama.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (arr_nama.Length > 1)
            {
                return arr_nama[0];
            }

            return s_nama;
        }

        public static bool IsURLExists(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "HEAD";

            bool exists;
            try
            {
                request.GetResponse();
                exists = true;
            }
            catch
            {
                exists = false;
            }

            return exists;
        }

        public static UserLogin LOGGED_USER_M
        {
            get
            {
                UserLogin usr = (UserLogin)HttpContext.Current.Session[Constantas.NAMA_SESSION_LOGIN];
                return usr;
            }
        }

        public static string GetNamaHariFromTanggal(DateTime tanggal)
        {
            switch (tanggal.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "Minggu";
                case DayOfWeek.Monday:
                    return "Senin";
                case DayOfWeek.Tuesday:
                    return "Selasa";
                case DayOfWeek.Wednesday:
                    return "Rabu";
                case DayOfWeek.Thursday:
                    return "Kamis";
                case DayOfWeek.Friday:
                    return "Jumat";
                case DayOfWeek.Saturday:
                    return "Sabtu";
                default:
                    return "";
            }
        }

        public static bool GetIsHariLibur(DateTime tanggal)
        {
            switch (tanggal.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                case DayOfWeek.Saturday:
                    return true;
                default:
                    return false;
            }
        }

        public static string GetNamaHariFromUrutHari(int urut_hari)
        {
            switch ((DayOfWeek)urut_hari)
            {
                case DayOfWeek.Sunday:
                    return "Minggu";
                case DayOfWeek.Monday:
                    return "Senin";
                case DayOfWeek.Tuesday:
                    return "Selasa";
                case DayOfWeek.Wednesday:
                    return "Rabu";
                case DayOfWeek.Thursday:
                    return "Kamis";
                case DayOfWeek.Friday:
                    return "Jumat";
                case DayOfWeek.Saturday:
                    return "Sabtu";
                default:
                    return "";
            }
        }

        public static string GetNamaHariFromUrutHari(string urut_hari)
        {
            try
            {
                switch ((DayOfWeek)Convert.ToInt16(urut_hari))
                {
                    case DayOfWeek.Sunday:
                        return "Minggu";
                    case DayOfWeek.Monday:
                        return "Senin";
                    case DayOfWeek.Tuesday:
                        return "Selasa";
                    case DayOfWeek.Wednesday:
                        return "Rabu";
                    case DayOfWeek.Thursday:
                        return "Kamis";
                    case DayOfWeek.Friday:
                        return "Jumat";
                    case DayOfWeek.Saturday:
                        return "Sabtu";
                    default:
                        return "";
                }
            }
            catch (Exception)
            {
                return "";
            }            
        }

        public static string GetHTMLSimpleText(string html, bool use_html_ascii = false)
        {
            string hasil = html.
                           Replace("<strong>", "<span style='font-weight: bold;'>").
                           Replace("</strong>", "</span>").
                           Replace("<STRONG>", "<span style='font-weight: bold;'>").
                           Replace("</STRONG>", "</span>").
                           Replace("<p>", "").
                           Replace("</p>", "").
                           Replace("<P>", "").
                           Replace("</P>", "").
                           Replace("<em>", "").
                           Replace("</em>", "").
                           Replace("<EM>", "").
                           Replace("</EM>", "").
                           Replace(System.Environment.NewLine, "<br />").
                           Replace("&nbsp;", " ")
                           ;
            hasil = Regex.Replace(hasil, @"\t|\n|\r", "<br />");

            if (use_html_ascii == true)
            {
                hasil = hasil.Replace("\"", "&#34;").
                              Replace("'", "&#39;");
            }

            return hasil;
        }

        public static string GetHTMLSimpleText2(string html, bool use_html_ascii = false)
        {
            string hasil = html.
                           Replace("<strong>", "<span style='font-weight: bold;'>").
                           Replace("</strong>", "</span>").
                           Replace("<STRONG>", "<span style='font-weight: bold;'>").
                           Replace("</STRONG>", "</span>").
                           Replace("<p>", "").
                           Replace("</p>", "").
                           Replace("<P>", "").
                           Replace("</P>", "").
                           Replace("<em>", "<i>").
                           Replace("</em>", "</i>").
                           Replace("<EM>", "<i>").
                           Replace("</EM>", "</i>").
                           Replace(System.Environment.NewLine, "<br />").
                           Replace("&nbsp;", " ")
                           ;
            hasil = Regex.Replace(hasil, @"\t|\n|\r", "<br />");

            if (use_html_ascii == true)
            {
                hasil = hasil.Replace("\"", "&#34;").
                              Replace("'", "&#39;");
            }

            return hasil;
        }

        public static string GetHTMLSimpleText3(string html, bool use_html_ascii = false)
        {
            string hasil = html.
                           Replace("<strong>", "<span style=\"font-weight: bold;\">").
                           Replace("</strong>", "</span>").
                           Replace("<STRONG>", "<span style=\"font-weight: bold;\">").
                           Replace("</STRONG>", "</span>").
                           Replace("<p>", "").
                           Replace("</p>", "").
                           Replace("<P>", "").
                           Replace("</P>", "").
                           Replace("<em>", "<i>").
                           Replace("</em>", "</i>").
                           Replace("<EM>", "<i>").
                           Replace("</EM>", "</i>").
                           Replace(System.Environment.NewLine, "<br />").
                           Replace("&nbsp;", " ")
                           ;
            hasil = Regex.Replace(hasil, @"\t|\n|\r", "<br />");

            if (use_html_ascii == true)
            {
                hasil = hasil.Replace("\"", "&#34;").
                              Replace("'", "&#39;");
            }

            return hasil;
        }

        public static string GetHTMLBadgeUnit(string unit)
        {
            switch (unit.Trim().ToUpper())
            {
                case "KB":
                    return "<sup style=\"background-color: green; font-weight: bold; font-size: x-small; color: white; border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px;\">" +
                                "&nbsp;&nbsp;" +
                                unit.ToUpper() +
                                "&nbsp;&nbsp;" +
                           "</sup>";
                case "TK":
                    return "<sup style=\"background-color: #446D8C; font-weight: bold; font-size: x-small; color: white; border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px;\">" +
                                "&nbsp;&nbsp;" +
                                unit.ToUpper() +
                                "&nbsp;&nbsp;" +
                           "</sup>";
                case "SD":
                    return "<sup style=\"background-color: #A63B26; font-weight: bold; font-size: x-small; color: white; border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px;\">" +
                                "&nbsp;&nbsp;" +
                                unit.ToUpper() +
                                "&nbsp;&nbsp;" +
                           "</sup>";
                case "SMP":
                    return "<sup style=\"background-color: #00AAD9; font-weight: bold; font-size: x-small; color: white; border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px;\">" +
                                "&nbsp;&nbsp;" +
                                unit.ToUpper() +
                                "&nbsp;&nbsp;" +
                           "</sup>";
                case "SMA":
                    return "<sup style=\"background-color: #AE00AE; font-weight: bold; font-size: x-small; color: white; border-top-left-radius: 5px; border-top-right-radius: 5px; border-bottom-right-radius: 5px;\">" +
                                "&nbsp;&nbsp;" +
                                unit.ToUpper() +
                                "&nbsp;&nbsp;" +
                           "</sup>";
            }

            return "";
        }

        public static string GetHTMLNoParagraphDiAwal(string html, bool use_html_ascii = false)
        {
            if (html == null) html = "";
            string hasil = html.
                           Replace("<strong>", "<span style=\"font-weight: bold;\">").
                           Replace("</strong>", "</span>").
                           Replace("<em>", "<i>").
                           Replace("</em>", "</i>").
                           Replace("<STRONG>", "<span style=\"font-weight: bold;\">").
                           Replace("</STRONG>", "</span>").
                           Replace("&nbsp;", " ").
                           Replace("\"", "&quot;").
                           Trim();

            if (hasil.Length > 7)
            {
                if (hasil.Substring(0, 3) == "<p>" || hasil.Substring(0, 3) == "<P>")
                {
                    hasil = hasil.Substring(3, hasil.Length - 3);
                }

                if (hasil.Substring(hasil.Length - 4, 4) == "</p>" || hasil.Substring(hasil.Length - 4, 4) == "</P>")
                {
                    hasil = hasil.Substring(0, hasil.Length - 4);
                }
            }

            return hasil;
        }

        public static string GetNoHTMLFormat(string html)
        {
            return html.Replace("<strong>", "").
                        Replace("</strong>", "").
                        Replace("<span style='font-weight: bold;'>", "").
                        Replace("</span>", "").
                        Replace("<p>", "").
                        Replace("</p>", "").
                        Replace("<em>", "").
                        Replace("</em>", "").
                        Replace("&ndash;", "-").
                        Replace("&nbsp;", " ")
                        ;
        }

        public static string GetHTMLEmailTemplate(string email_body, string rel_sekolah, bool use_border_top = true)
        {
            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(rel_sekolah);
            string s_image_header = "";
            string s_image_footer = "";
            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {
                    switch ((Libs.UnitSekolah)m_sekolah.UrutanJenjang)
                    {
                        case UnitSekolah.SALAH:
                            break;
                        case UnitSekolah.KB:
                            s_image_header = "kb_header.png";
                            s_image_footer = "kb_footer.png";
                            break;
                        case UnitSekolah.TK:
                            s_image_header = "tk_header.png";
                            s_image_footer = "tk_footer.png";
                            break;
                        case UnitSekolah.SD:
                            s_image_header = "sd_header.png";
                            s_image_footer = "sd_footer.png";
                            break;
                        case UnitSekolah.SMP:
                            s_image_header = "smp_header.png";
                            s_image_footer = "smp_footer.png";
                            break;
                        case UnitSekolah.SMA:
                            s_image_header = "sma_header.png";
                            s_image_footer = "sma_footer.png";
                            break;
                        default:
                            break;
                    }
                }
            }

            return "<table width=\"100%\" height=\"100%\" style=\"min-width:348px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                    "	<tbody>" +
                    "		<tr align=\"center\">" +
                    "			<td style=\"padding: 0px;\">" +
                    "				<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"width: 100%;\">" +
                    "					<tbody>" +
                    "						<tr>" +
                    "			                <td style=\"padding: 0px;\">" +
                    "								<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin:0 auto;display:table;min-width:332px;width:100%;" + "\">" +
                    "									<tbody>" +
                    "										<tr>" +
                    "											<td align=\"left\" style=\"vertical-align: top; padding: 0px; background-color: white;\">" +
                    "												<img src=\"http://aplikasi.alizhar.sch.id/res/images/" + s_image_header + "\" class=\"CToWUd\" style=\"margin-right: 15px; width: 100%;\">" +
                    "											</td>" +
                    "										</tr>" +
                    "									</tbody>" +
                    "								</table>" +
                    "							</td>" +
                    "						</tr>" +
                    "						<tr>" +
                    "							<td style=\"padding: 0px; background-color: #ffffff;\">" +
                    "								<table bgcolor=\"#ffffff\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin:0 auto;display:table;min-width:332px;width:100%;\">" +
                    "									<tbody>" +
                    "										<tr>" +
                    "											<td style=\"background-color: #ffffff; padding: 0px;\">" +
                    "												<table style=\"min-width:300px;width:100%;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                    "													<tbody>" +
                    "														<tr>" +
                    "															<td style=\"background-color: #ffffff; padding: 0px;font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:13px;color:#202020;line-height:1.5\">" +
                                                                                    email_body +
                    "															</td>" +
                    "														</tr>" +
                    "													</tbody>" +
                    "												</table>" +
                    "											</td>" +
                    "										</tr>" +
                    "									</tbody>" +
                    "								</table>" +
                    "							</td>" +
                    "						</tr>" +
                    "						<tr>" +
                    "			                <td style=\"padding: 0px; background-color: white;\">" +
                    "								<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin:0 auto;display:table;min-width:332px;width:100%;margin-top: 15px;" + "\">" +
                    "									<tbody>" +
                    "										<tr>" +
                    "											<td align=\"left\" style=\"vertical-align: top; padding: 0px; background-color: white;\">" +
                    "												<img src=\"http://aplikasi.alizhar.sch.id/res/images/" + s_image_footer + "\" class=\"CToWUd\" style=\"margin-right: 15px; width: 100%;\">" +
                    "											</td>" +
                    "										</tr>" +
                    "									</tbody>" +
                    "								</table>" +
                    "							</td>" +
                    "						</tr>" +
                    "					</tbody>" +
                    "				</table>" +
                    "			</td>" +
                    "		</tr>" +
                    "	</tbody>" +
                    "</table>";
        }

        public static string GetHTMLEmailTemplate_OLD1(string email_body)
        {
            return  "<table width=\"100%\" height=\"100%\" style=\"min-width:348px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                    "	<tbody>" +
                    "		<tr align=\"center\">" +
                    "			<td style=\"padding: 0px;\">" +
                    "				<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"width: 100%;\">" +
                    "					<tbody>" +
                    "						<tr>" +
                    "			                <td style=\"padding: 0px;\">" +
                    "								<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin:0 auto;display:table;min-width:332px;width:100%;border:1px solid #f0f0f0;border-bottom:0;border-top-left-radius:3px;border-top-right-radius:3px\">" +
                    "									<tbody>" +
                    "										<tr>" +
                    "											<td align=\"left\" style=\"vertical-align: top; width: 124px; padding: 15px; padding-left: 30px; background-color: white;\">" +
                    "												<img src=\"http://aplikasi.alizhar.sch.id/res/images/logo-ya.png\" class=\"CToWUd\" style=\"margin-right: 15px;\">" +
                    "											</td>" +
                    "											<td align=\"left\" style=\"padding: 15px; background-color: white;\">" +
                    "												<span style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:24px;color:gray;font-weight:bold;\">Al-Izhar Pondok Labu</span>" +
                    "												<br />" +
                    "												<span style=\"font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:12px;color:gray;\">" +
                    "													Jl. RS Fatmawati Kav.49, Pondok Labu, Cilandak, Kota Jakarta Selatan, Daerah Khusus Ibukota Jakarta 12450" +
                    "												</span>" +
                    "											</td>" +
                    "											<td align=\"right\" style=\"padding: 15px; background-color: white;\">" +
                    "												&nbsp;" +
                    "											</td>" +
                    "										</tr>" +
                    "									</tbody>" +
                    "								</table>" +
                    "							</td>" +
                    "						</tr>" +
                    "						<tr>" +
                    "							<td style=\"padding: 0px; background-color: #FAFAFA;\">" +
                    "								<table bgcolor=\"#FAFAFA\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin:0 auto;display:table;min-width:332px;width:100%;border:1px solid #f0f0f0;border-bottom:1px solid #c0c0c0;border-top:0;border-bottom-left-radius:3px;border-bottom-right-radius:3px\">" +
                    "									<tbody>" +
                    "										<tr height=\"16px\">" +
                    "											<td width=\"32px\" rowspan=\"3\" style=\"background-color: #FAFAFA; padding: 0px;\"></td>" +
                    "											<td style=\"background-color: #FAFAFA; padding: 0px;\"></td>" +
                    "											<td width=\"32px\" rowspan=\"3\" style=\"background-color: #FAFAFA; padding: 0px;\"></td>" +
                    "										</tr>" +
                    "										<tr>" +
                    "											<td style=\"background-color: #FAFAFA; padding: 0px;\">" +
                    "												<table style=\"min-width:300px;width:100%;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                    "													<tbody>" +
                    "														<tr>" +
                    "															<td style=\"background-color: #FAFAFA; padding: 0px;font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:13px;color:#202020;line-height:1.5\">" +
                                                                                    email_body +
                    "															</td>" +
                    "														</tr>" +
                    "													</tbody>" +
                    "												</table>" +
                    "											</td>" +
                    "										</tr>" +
                    "										<tr height=\"32px\"><td style=\"background-color: #FAFAFA; padding: 0px;\"></td></tr>" +
                    "									</tbody>" +
                    "								</table>" +
                    "							</td>" +
                    "						</tr>" +
                    "						<tr>" +
                    "							<td style=\"background-color: #FAFAFA; padding: 0px;max-width:600px;font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:10px;color:#bcbcbc;line-height:1.5\"></td>" +
                    "						</tr>" +
                    "					</tbody>" +
                    "				</table>" +
                    "			</td>" +
                    "		</tr>" +
                    "	</tbody>" +
                    "</table>";
        }

        public static void ListAsalSekolahToDropDown(DropDownList cbo, bool is_unit_kecil, bool show_blank = true)
        {
            cbo.Items.Clear();
            if(show_blank) cbo.Items.Add("");
            if (is_unit_kecil)
            {
                cbo.Items.Add(new ListItem { Value = ((int)Libs.JenisAsalSekolah.BelumSekolah).ToString(), Text = "Belum Bersekolah" });
            }
            cbo.Items.Add(new ListItem { Value = ((int)Libs.JenisAsalSekolah.SiswaAlizhar).ToString(), Text = "Siswa Al-Izhar" });
            cbo.Items.Add(new ListItem { Value = ((int)Libs.JenisAsalSekolah.SiswaNonAlizhar).ToString(), Text = "Siswa Non Al-Izhar" });
        }

        public static string GetPerbaikiEjaanNama(string teks)
        {
            if (teks == null) return "";
            if (teks.Trim() == "") return "";

            string[] arr_teks = teks.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string hasil = "";

            if (teks.ToUpper() == "WNI") return teks.ToUpper();
            if (teks.ToUpper() == "TNI") return teks.ToUpper();
            if (teks.ToUpper() == "RT") return teks.ToUpper();
            if (teks.ToUpper() == "RW") return teks.ToUpper();

            foreach (string item in arr_teks)
            {
                string s_item = item.ToLower();

                if (s_item.Length > 0)
                {
                    hasil += s_item.Substring(0, 1).ToUpper();
                }
                if (s_item.Length > 1)
                {
                    hasil += s_item.Substring(1);
                }

                hasil += " ";
            }

            return hasil.Trim();
        }

        public static void CopyFile(string source_file, string dest_file)
        {
            try
            {
                var currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
                var fullFilePath_sc = currentApplicationPath + source_file;
                var fullFilePath_ds = currentApplicationPath + dest_file;
                if (System.IO.File.Exists(fullFilePath_sc))
                    File.Copy(fullFilePath_sc, fullFilePath_ds, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static byte[] GetBytes(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public enum JenisSave { Insert, Update }

        private static Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);

        public static string GetJSActionKeyNumber(string jmldesimal = "0")
        {
            string js_focus = " onfocus=\"this.value = GetPureNumber('', this.value);\" ";
            string js_blur = " onblur=\"this.value = SetTandaPemisahTitikUseDesimal(this.value, " + jmldesimal.ToString() + ");\" ";
            string js_keydown = " onkeydown=\"return SetInputNumberOnly(event);\" ";

            return js_focus + 
                   js_blur + 
                   js_keydown;
        }

        public static string GetTahunAjaranNow()
        {
            if (DateTime.Now.Month <= 6)
            {
                return (DateTime.Now.Year - 1).ToString() + "/" + (DateTime.Now.Year).ToString();
            }
            else
            {
                return (DateTime.Now.Year).ToString() + "/" + (DateTime.Now.Year + 1).ToString();
            }
        }

        public static string GetTahunAjaranNowPlus(int plus)
        {
            if (DateTime.Now.Month <= 6)
            {
                return ((DateTime.Now.Year - 1) + plus).ToString() + "/" + (DateTime.Now.Year + plus).ToString();
            }
            else
            {
                return (DateTime.Now.Year + plus).ToString() + "/" + (DateTime.Now.Year + 1 + plus).ToString();
            }
        }

        public static string GetTahunAjaranPlus(string tahun_ajaran, int plus)
        {
            string tahun = "";
            if (tahun_ajaran.Length == 9)
            {
                int tahun1 = Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) + plus;
                int tahun2 = Libs.GetStringToInteger(tahun_ajaran.Substring(tahun_ajaran.Length - 4, 4)) + plus;
                tahun = tahun1.ToString() + "/" + tahun2.ToString();
            }
            return tahun;
        }

        public static string GetHTMLEmbed(string url)
        {
            if (url.Trim() == "") return "";
            if (url.ToLower().IndexOf("<iframe") >= 0)
            {
                return url;
            }
            return "<iframe width=\"420\" height=\"315\" src=\"" + url + "\" style=\"border-style: none;\"></iframe>";
        }

        public static string GetNamaFileForURL(string nama_file)
        {
            return nama_file.Replace("'", "[kutip_satu]").Replace("&", "[dan]").Trim();
        }

        public static string GetNamaFileFromURL(string nama_file)
        {
            return nama_file.Replace("[kutip_satu]", "'").Replace("[dan]", "&").Trim();
        }

        public static string GetHTMLListUploadedFiles(Page page, string lokasi_upload, string jenis, string id, string id2, bool show_hapus = true)
        {
            //show file list
            string list_uploaded = "";
            
            bool is_show_hapus = show_hapus;
            bool ada_data = false;

            if (Directory.Exists(HttpContext.Current.Server.MapPath(lokasi_upload)))
            {
                int id_file = 0;
                string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(lokasi_upload));
                foreach (string item in filePaths)
                {
                    string ext_file = Path.GetExtension(item).Trim().ToLower();
                    string icon_name = "fa fa-file-o";
                    ada_data = true;

                    switch (ext_file)
                    {
                        case ".pdf":
                            icon_name = "fa fa-file-pdf-o";
                            break;
                        case ".xls":
                        case ".xlsx":
                            icon_name = "fa fa-file-excel-o";
                            break;
                        case ".doc":
                        case ".docx":
                            icon_name = "fa fa-file-word-o";
                            break;
                        case ".ppt":
                        case ".pptx":
                            icon_name = "fa fa-file-powerpoint-o";
                            break;
                        case ".jpg":
                        case ".jpeg":
                        case ".gif":
                        case ".png":
                        case ".bmp":
                            icon_name = "fa fa-file-image-o";
                            break;
                        default:
                            break;
                    }

                    string kode_tr = "tr_" + Guid.NewGuid().ToString().Replace(" - ", "_");
                    string kode_img = "img_" + Guid.NewGuid().ToString().Replace(" - ", "_");
                    string kode_btn = "btn_" + Guid.NewGuid().ToString().Replace(" - ", "_");
                    string lokasi_hapus = HttpContext.Current.Server.MapPath(lokasi_upload + "/" + Path.GetFileName(item)).Replace("\\", "@@");
                    string download_path = HttpContext.Current.Server.MapPath(lokasi_upload + "/" + Path.GetFileName(item));
                    string file_url = page.ResolveUrl(lokasi_upload + "/" + Path.GetFileName(item));
                    file_url = page.ResolveUrl(Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DOWNLOAD_FILE.ROUTE) +
                               "?j=" + jenis +
                               "&id=" + id +
                               "&id2=" + id2 +
                               "&f=" + Crypto.SimpleEncrypt(Path.GetFileName(item));

                    list_uploaded += "<tr id=\"" + kode_tr + "\" style=\"width: 100%; padding: 15px; font-weight: bold; border-bottom-style: dotted; border-bottom-width: 1px; border-bottom-color: #bfbfbf;\">" +
                                        "<td style=\"vertical-align: top; padding: 15px; padding-left: 10px; width: 18px;\">" +
                                            "<i class=\"" + icon_name + "\" style=\"color: grey\"></i>" +
                                        "</td>" +
                                        "<td style=\"vertical-align: top; padding: 15px; padding-left: 0px;\">" +
                                            "<a href=\"" +
                                                    (
                                                        file_url
                                                    )
                                                + "\" target=\"_blank\" style=\"font-weight: normal; color: #007ACC;\">" +
                                                Path.GetFileName(item) +
                                            "</a>" +
                                        "</td>" +
                                        (show_hapus ?
                                            "<td style=\"width: 45px; vertical-align: top; text-align: right; padding: 15px; \">" +
                                                "<img id=\"" + kode_img + "\" src=\"" + page.ResolveUrl("~/Application_CLibs/images/loading-home.gif") + "\" style=\"display: none; height: 16px; width: 16px;\" />" +
                                                "<button name=\"hapus_attch[]\" id=\"" + kode_btn + "\" onclick=\"" +
                                                    "if(confirm('Anda yakin akan menghapus file : \\n" + Path.GetFileName(item) + "')) { ShowProgress(true); document.getElementById('" + kode_btn + "').style.display = 'none'; document.getElementById('" + kode_img + "').style.display = ''; this.style.display = 'none'; document.getElementById('fraDelete').src = '" + page.ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DELETE_FILE.ROUTE) + 
                                                        "?jenis=" + jenis.ToString() + 
                                                        "&id=" + id.ToString() + 
                                                        "&id2=" + id2.ToString() + 
                                                        "&file=" + GetNamaFileForURL(Path.GetFileName(item)) + 
                                                        "&kode_tr=" + kode_tr + "'; return false; } else { return false; } \" title=\" Hapus File \" style=\"background: transparent; border-style: none; color: #B93221; outline: none;\">" +
                                                    "<i class=\"fa fa-times\"></i>" +
                                                "</button>" +
                                            "</td>"
                                        : "") +
                                     "</tr>";

                    id_file++;
                }
            }

            if (list_uploaded.Trim() != "")
            {
                list_uploaded = "<table id=\"tbluploadedfiles\" style=\"padding: 0px; width: 100%; margin-top: 0px;\">" +
                                    list_uploaded +
                                "</table>";
            }

            if (!ada_data) list_uploaded = "";

            return list_uploaded;
        }

        public static void RedirectToLogin(Page page)
        {
            page.Response.Redirect(Routing.URL.LOGIN.ROUTE);
        }

        public static void RedirectToBeranda(Page page)
        {
            page.Response.Redirect(Routing.URL.BERANDA.ROUTE);
        }

        public static void CRToPDFWithPassword()
        {
            //System.IO.Stream st = CrReport.ExportToStream(ExportFormatType.PortableDocFormat);
            //PdfDocument document = PdfReader.Open(st);

            //PdfSecuritySettings securitySettings = document.SecuritySettings;
            
            //// Setting one of the passwords automatically sets the security level to 
            //// PdfDocumentSecurityLevel.Encrypted128Bit.
            //securitySettings.UserPassword = "user";
            //securitySettings.OwnerPassword = "owner";

            //// Don´t use 40 bit encryption unless needed for compatibility reasons
            ////securitySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.Encrypted40Bit;

            //// Restrict some rights.            
            //securitySettings.PermitAccessibilityExtractContent = false;
            //securitySettings.PermitAnnotations = false;
            //securitySettings.PermitAssembleDocument = false;
            //securitySettings.PermitExtractContent = false;
            //securitySettings.PermitFormsFill = true;
            //securitySettings.PermitFullQualityPrint = false;
            //securitySettings.PermitModifyDocument = true;
            //securitySettings.PermitPrint = false;

            //// Save the document...
            //document.Save(filename);
        }

        public static void ListJamMenitToDropdown(DropDownList cboJam, DropDownList cboMenit)
        {
            cboJam.Items.Clear();
            cboMenit.Items.Clear();

            for (int i = 0; i < 24; i++)
            {
                cboJam.Items.Add(
                        (i < 10 ? "0" : "") + i.ToString()
                    );
            }

            for (int i = 0; i < 60; i++)
            {
                cboMenit.Items.Add(
                        (i < 10 ? "0" : "") + i.ToString()
                    );
            }
        }

        public static void ListTahunAjaranToDropDown(DropDownList cboTahunAjaran)
        {
            cboTahunAjaran.Items.Clear();
            List<string> lst_tahun_ajaran = new List<string>();
            lst_tahun_ajaran.Add(GetTahunAjaranNow());
            lst_tahun_ajaran.Add(GetTahunAjaranNowPlus(1));

            lst_tahun_ajaran = lst_tahun_ajaran.Distinct().ToList().OrderByDescending(m => m).ToList();
            foreach (string tahun_ajaran in lst_tahun_ajaran)
            {
                cboTahunAjaran.Items.Add(new ListItem {
                    Value = tahun_ajaran,
                    Text = tahun_ajaran
                });
            }
        }
                
        public static class WebDirMonitoring
        {
            public static void StopMonitoring()
            {
                PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                object o = p.GetValue(null, null);
                FieldInfo f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                object monitor = f.GetValue(o);
                MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", BindingFlags.Instance | BindingFlags.NonPublic);
                m.Invoke(monitor, new object[] { });
            }
        }

        public static class Constantas
        {
            public static string ApplicationName = "PSB AL IZHAR";
            public const string NAMA_SESSION_LOGIN = "AdminLogin";
            public const string NAMA_SESSION_LOGIN_SISWA = "SiswaLogin";
            public const string MSG_MAX_UPLOAD = "5MB";
        }

        public static string LokasiAppFiles()
        {
            return HttpContext.Current.Server.MapPath("~/Application_Files");
        }

        public static string GetPureNumberString(string number)
        {
            if (number.Trim() == "") return "0";

            string num = number.Replace(",", "@");
            num = num.Replace(".", "");
            num = num.Replace("@", ".");

            return num;
        }

        public static string GetUsia(DateTime tanggal_lahir, DateTime tanggal_usia)
        {
            string hasil = "";
            SqlConnection conn = Application_Libs.Libs.GetConnection_PSB();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.Text;
                comm.CommandText = "SELECT dbo.GET_USIA('" + tanggal_lahir.ToString("YYYY-MM-dd") + "', '" + tanggal_usia.ToString("YYYY-MM-dd") + "')";

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = row[0].ToString();
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

        public static void SetClientTextBoxNumberFormat(TextBox txt, int jmldesimal = 0, bool focusevent = true, bool blurevent = true)
        {
            string js_focus = "this.value = GetPureNumber('', this.value);";
            string js_blur = "this.value = SetTandaPemisahTitikUseDesimal(this.value, " + jmldesimal.ToString() + ");";
            if (focusevent) txt.Attributes.Add("onfocus", js_focus);
            if (blurevent) txt.Attributes.Add("onblur", js_blur);
        }

        public static bool IsGuid(string candidate)
        {
            bool isValid = false;

            if (candidate != null)
            {

                if (isGuid.IsMatch(candidate))
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        public static class JenjangPendidikan
        {
            public const string TK = "TK";
            public const string SD = "SD";
            public const string SMP = "SMP";
            public const string SMA = "SMA";
            public const string D1 = "D1";
            public const string D2 = "D2";
            public const string D3 = "D3";
            public const string D4 = "D4";
            public const string S1 = "S1";
            public const string S2 = "S2";
            public const string S3 = "S3";
        }

        public static void SelectDropdownListByValue(DropDownList cbo, string value)
        {
            if (value == null) return;
            int id = 0;
            bool ada = false;
            foreach (ListItem item in cbo.Items)
            {
                if (item.Value.ToString().ToUpper() == value.ToUpper())
                {
                    ada = true;
                    break;
                }
                id++;
            }
            if (!ada)
            {
                if (cbo.Items.Count > 0)
                {
                    cbo.SelectedIndex = 0;
                    return;
                }
                else
                {
                    return;
                }
            }
            cbo.SelectedIndex = id;
        }

        public static string GetTahunAjaranByTanggal(DateTime tanggal)
        {
            if (tanggal.Month >= 7)
            {
                return tanggal.Year.ToString() + "/" + (tanggal.Year + 1).ToString();
            }
            else
            {
                return (tanggal.Year - 1).ToString() + "/" + (tanggal.Year).ToString();
            }
        }

        public static int GetSemesterByTanggal(DateTime tanggal)
        {
            if (tanggal.Month >= 7 && tanggal.Month <= 12)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public static int GetDiffInYear(DateTime dt1, DateTime dt2)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);

            TimeSpan span = dt2 - dt1;
            int years = (zeroTime + span).Year - 1;

            return years;
        }

        public static string GetTanggalIndonesiaFromDate(DateTime tanggal, bool usetime)
        {
            return (
                        (tanggal.Day < 10 ? "0" : "") + tanggal.Day.ToString() + " " +
                        Array_Bulan[tanggal.Month - 1] + " " +
                        tanggal.Year.ToString() +
                        (
                            usetime ? tanggal.ToString(" HH:mm:ss") : ""
                        )
                   ).ToString();
        }

        public static string GetTanggalIndonesiaFromDate(DateTime tanggal, bool usetime, string ket_jam)
        {
            return (
                        (tanggal.Day < 10 ? "0" : "") + tanggal.Day.ToString() + " " +
                        Array_Bulan[tanggal.Month - 1] + " " +
                        tanggal.Year.ToString() +
                        (
                            usetime ? " " + ket_jam.Trim() + " " + tanggal.ToString("HH:mm:ss") : ""
                        )
                   ).ToString();
        }

        public static string GetInfoTanggalPeriode(DateTime tanggal_1, DateTime tanggal_2, string separator = " s.d ", bool use_singkatan_bulan = false)
        {
            if (tanggal_1.Month == tanggal_2.Month && tanggal_1.Year == tanggal_2.Year)
            {
                return (tanggal_1.Day < 10 ? "0" : "") + tanggal_1.Day.ToString() + 
                           separator + 
                           (
                            use_singkatan_bulan
                            ? Libs.GetTanggalIndonesiaSingkatFromDate(tanggal_2, false)
                            : Libs.GetTanggalIndonesiaSingkatFromDate(tanggal_2, false)
                       );
            }
            else
            {
                if (use_singkatan_bulan)
                {
                    return Libs.GetTanggalIndonesiaSingkatFromDate(tanggal_1, false) +
                           separator +
                           Libs.GetTanggalIndonesiaSingkatFromDate(tanggal_2, false);
                }
                else
                {
                    return Libs.GetTanggalIndonesiaFromDate(tanggal_1, false) +
                           separator +
                           Libs.GetTanggalIndonesiaFromDate(tanggal_2, false);
                }
            }
        }

        public static string GetTanggalIndonesiaSingkatFromDate(DateTime tanggal, bool usetime)
        {
            return (
                        (tanggal.Day < 10 ? "0" : "") + tanggal.Day.ToString() + " " +
                        Array_Bulan_Singkat[tanggal.Month - 1] + " " +
                        tanggal.Year.ToString() +
                        (
                            usetime ? tanggal.ToString(" HH:mm:ss") : ""
                        )
                   ).ToString();
        }

        public static int GetNomorBulanFromNamaBulanIndonesia(string namabulan)
        {
            int id = 1;
            foreach (string item in Array_Bulan)
            {
                if (namabulan.Trim().ToLower() == item.Trim().ToLower())
                {
                    return id;
                }
                id++;
            }
            return 0;
        }

        public static DateTime GetDateFromTanggalIndonesiaStr(string tanggal, bool null_is_date_min = true, string jam = "")
        {
            DateTime hasil = (null_is_date_min ? DateTime.MinValue : DateTime.Now);
            string[] arr_tanggal = tanggal.Split(new string[] { " " }, StringSplitOptions.None);
            if (arr_tanggal.Length == 3)
            {
                hasil = new DateTime(
                        int.Parse(arr_tanggal[2]),
                        GetNomorBulanFromNamaBulanIndonesia(arr_tanggal[1]),
                        int.Parse(arr_tanggal[0]),
                        int.Parse(jam.Trim() != "" ? jam.Substring(0, 2) : "00"),
                        int.Parse(jam.Trim() != "" ? jam.Substring(3, 2) : "00"),
                        0
                    );
            }
            return hasil;
        }

        public static DateTime GetDateFromENGString(string tanggal)
        {
            DateTime hasil = DateTime.MinValue;

            if (tanggal.Length == 8)
            {
                hasil = new DateTime(
                        int.Parse(tanggal.Substring(0, 4)),
                        int.Parse(tanggal.Substring(4, 2)),
                        int.Parse(tanggal.Substring(6, 2))
                    );
            }

            return hasil;
        }

        public static string GetHTMLHighLightSearch(string teks, string keyword, bool use_bold)
        {
            if (teks == null) return "";
            if (teks.Trim() == "") return "";

            string hasil = "";
            string hasil_html = "";

            if (teks.Trim().ToLower() == keyword.Trim().ToLower())
            {
                hasil_html += "<label style=\"background-color: yellow; color: black; padding: 3px; border-radius: 5px;" + (use_bold ? " font-weight: bold; " : "") + "\">" +
                                    teks +
                              "</label>";
            }
            else
            {
                string[] arr_teks = Regex.Split(teks, keyword, RegexOptions.IgnoreCase);

                int idfor = 1;
                foreach (string item in arr_teks)
                {
                    if (!(idfor == 1 && item.Trim() == ""))
                    {
                        if (idfor == 1 && item.Trim() != "")
                        {
                            hasil_html += item;
                            hasil += item;
                        }
                        else if (idfor == teks.Length && item.Trim() == "")
                        {
                            break;
                        }
                        else
                        {
                            hasil_html += "<label style=\"background-color: yellow; color: black; padding: 3px; border-radius: 5px;" + (use_bold ? " font-weight: bold; " : "") + "\">" +
                                                teks.Substring(hasil.Length, keyword.Length) +
                                          "</label>" + item;
                            hasil += keyword + item;
                        }
                    }

                    idfor++;
                }
            }

            return hasil_html;
        }

        public static int GetDbColumnMaxLength(string nama_field, string nama_sp_select, List<SqlParameter> list_params)
        {
            DataTable dtResult = new DataTable();
            SqlConnection conn = Application_Libs.Libs.GetConnection();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = nama_sp_select;
                comm.Parameters.Clear();
                foreach (SqlParameter param in list_params)
                {
                    comm.Parameters.AddWithValue(param.ParameterName, param.Value);
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dtResult);

                sqlDA = new SqlDataAdapter(comm);
                sqlDA.FillSchema(dtResult, SchemaType.Source);
                sqlDA.Fill(ds);

                int maxlength = (dtResult.Columns[nama_field].MaxLength == -1 ? 0 : dtResult.Columns[nama_field].MaxLength);
                return maxlength;
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

        public static class URL_GENERATOR
        {
            public const string ACT_QSTRING = "act";
        }

        public static string GetQ()
        {
            return (HttpContext.Current.Request.QueryString["q"] == null ? "" : HttpContext.Current.Request.QueryString["q"].ToString());
        }

        public static string GetQueryString(string keyword)
        {
            try
            {
                return (HttpContext.Current.Request.QueryString[keyword] == null ? "" : HttpContext.Current.Request.QueryString[keyword].ToString());
            }
            catch (Exception)
            {
                return "";
            }            
        }

        public static string GetHTMLErrorMessage(string teks)
        {
            return "<div id=\"divMessage\" style=\"margin-right: 20px; margin-bottom: 70px; bottom: 0px; right: 0px; display: block; position: fixed; z-index: 999999999999;\">" +
                    "<div class=\"alert alert-danger\" role=\"alert\" style=\"border-style: solid; border-width: 2px; border-color: red;\">" +
                            teks +
                            "<label onclick=\"divMessage.style.display = 'none';\" style=\"padding: 5px; cursor: pointer; margin-left: 10px;\">X</label>" +
                        "</div>" +
                    "</div>";
        }

        public static string FILE_PAGE_URL
        {
            get
            {
                string curl = HttpContext.Current.Request.PhysicalPath;

                string namafile = System.IO.Path.GetFileName(curl);
                return namafile;
            }
        }

        public static string GetPageTitle(string title)
        {
            return Constantas.ApplicationName + (title.Trim() != "" ? " - " : "") + title;
        }

        public static SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["PSBConn"].ConnectionString);
            return con;
        }

        public static SqlConnection GetConnectionHR()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConn"].ConnectionString);
            return con;
        }

        public static SqlConnection GetConnection_ERP()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ERPConn"].ConnectionString);
            return con;
        }

        public static SqlConnection GetConnection_RaporOld()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["RaporOldConn"].ConnectionString);
            return con;
        }

        public static SqlConnection GetConnection_Person()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["PersonConn"].ConnectionString);
            return con;
        }

        public static SqlConnection GetConnection_Keu()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KeuConn"].ConnectionString);
            return con;
        }

        public static SqlConnection GetConnection_PSB()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["PSBConn"].ConnectionString);
            return con;
        }

        public static SqlConnection GetConnection_Rapor()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["RaporConn"].ConnectionString);
            return con;
        }

        public static SqlConnection GetConnection_KeuOld()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KeuOldConn"].ConnectionString);
            return con;
        }

        public static SqlConnection GetConnection_Mailer()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MailerConn"].ConnectionString);
            return con;
        }

        public static SqlConnection GetConnection_Aplikasi()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["AplikasiConn"].ConnectionString);
            return con;
        }

        public static string GetConnectionString()
        {
            return WebConfigurationManager.ConnectionStrings["PSBConn"].ConnectionString;
        }

        public static string GetConnectionString_Rapor()
        {
            return WebConfigurationManager.ConnectionStrings["RaporConn"].ConnectionString;
        }

        public static string GetConnectionString_ERP()
        {
            return WebConfigurationManager.ConnectionStrings["ERPConn"].ConnectionString;
        }

        public static string GetConnectionString_Person()
        {
            return WebConfigurationManager.ConnectionStrings["PersonConn"].ConnectionString;
        }

        public static string GetConnectionString_Keu()
        {
            return WebConfigurationManager.ConnectionStrings["KeuConn"].ConnectionString;
        }

        public static string GetConnectionString_HR()
        {
            return WebConfigurationManager.ConnectionStrings["HRConn"].ConnectionString;
        }

        public static string GetConnectionString_PSB()
        {
            return WebConfigurationManager.ConnectionStrings["PSBConn"].ConnectionString;
        }

        public static string GetConnectionString_PSBAI()
        {
            return WebConfigurationManager.ConnectionStrings["PSBAIConn"].ConnectionString;
        }

        public static void ListBulanToDropdownList(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem { Value = "", Text = "" });

            int id = 1;
            foreach (string item in Array_Bulan)
            {
                ddl.Items.Add(new ListItem { Value = id.ToString(), Text = item });
                id++;
            }
            ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(DateTime.Now.Month.ToString()));
        }

        public static void ListTahunToDropdownList(DropDownList ddl, int iTahunMulai, int iTahunSelesai)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem { Value = "", Text = "" });

            if (iTahunMulai <= iTahunSelesai)
            {
                for (int i = iTahunMulai; i <= iTahunSelesai; i++)
                {
                    ddl.Items.Add(new ListItem { Value = i.ToString(), Text = i.ToString() });
                }
            }
            ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(DateTime.Now.Year.ToString()));
        }

        public static void SelectDropdownBulanTahun(DropDownList ddlBulan, DropDownList ddlTahun, int iBulan, int iTahun)
        {
            ddlBulan.SelectedValue = iBulan.ToString();
            ddlTahun.SelectedValue = iTahun.ToString();
        }

        public static void ListStatusConfirmToDropdownList(DropDownList ddl)
        {
            string[] arr_status = { "", "Y", "N" };

            ddl.Items.Clear();
            foreach (string item in arr_status)
            {
                ddl.Items.Add(new ListItem
                {
                    Value = item,
                    Text = item
                });
            }
        }

        public static List<string> ListTahunAjaran()
        {
            List<string> hasil = new List<string>();

            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "TahunAjaran_SELECT_ALL";

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

        public static DateTime stringToDateTime(string value)
        {
            DateTime DateResult;
            string[] args = value.Split('/');
            DateResult = Convert.ToDateTime(value, iCultur);
            return DateResult;
        }

        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static string Enkrip(string sKata)
        {
            int j = 1;
            string sKode = "TRADIAFOR";
            string enkrip = "";

            for (int i = 1; i <= sKata.Trim().Length; i++)
            {
                byte[] asciiBytes1 = Encoding.ASCII.GetBytes(sKata.Substring(i - 1, 1));
                byte[] asciiBytes2 = Encoding.ASCII.GetBytes(sKode.Substring(j - 1, 1));
                char sBaru = (char)((asciiBytes1[0] - 31) + (asciiBytes2[0] + 31));
                enkrip += sBaru;
                j = (j == 9 ? 1 : j + 1);
            }

            return enkrip;
        }

        public static string Dekrip(string sKata)
        {
            int j = 1;
            string sKode = "TRADIAFOR";
            string dekrip = "";

            for (int i = 1; i <= sKata.Trim().Length; i++)
            {
                byte[] asciiBytes1 = Encoding.ASCII.GetBytes(sKata.Substring(i - 1, 1));
                byte[] asciiBytes2 = Encoding.ASCII.GetBytes(sKode.Substring(j - 1, 1));
                char sBaru = (char)((asciiBytes1[0] + 31) - (asciiBytes2[0] + 31));
                dekrip += sBaru;
                j = (j == 9 ? 1 : j + 1);
            }

            return dekrip;
        }

        public static string Encryptdata(string val)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[val.Length];
            encode = Encoding.UTF8.GetBytes(val);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
        public static string Decryptdata(string val)
        {
            string result = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(val);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            result = new String(decoded_char);
            return result;
        }

        public static bool IsAngka(string teks, bool as0ifstringkosong = false)
        {
            if (as0ifstringkosong && teks.Trim() == "")
            {
                return true;
            }
            if (as0ifstringkosong && teks == null)
            {
                return true;
            }
            else if (!as0ifstringkosong && teks == null)
            {
                return false;
            }

            bool hasil = true;
            teks = teks.Replace(",", "").Replace(".", "").Trim();
            if (teks.Length > 0)
            {
                for (int i = 0; i < teks.Length; i++)
                {
                    if (Angka.IndexOf(teks.Substring(i, 1)) == -1)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return hasil;
        }

        public static decimal GetStringToDecimal(string teks)
        {
            try
            {
                if (IsAngka(teks))
                {
                    return decimal.Parse(teks.Trim() == "" ? "0" : teks);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static double GetStringToDouble(string teks)
        {
            try
            {
                if (IsAngka(teks))
                {
                    return double.Parse(teks.Trim() == "" ? "0" : teks);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool GetValueToBoolean(object value)
        {
            if (value == DBNull.Value)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(value);
            }
        }

        public static int GetStringToInteger(string teks)
        {
            if (IsAngka(teks))
            {
                return int.Parse(teks.Trim() == "" ? "0" : teks);
            }
            else
            {
                return 0;
            }
        }

        public static object GetValue(object value, object valueifnull = null)
        {
            object hasil = null;
            if (value == DBNull.Value)
            {
                hasil = valueifnull;
            }
            else
            {
                return value;
            }
            return hasil;
        }

        public static object GetValidDateValue(DateTime date)
        {
            return GetForDBValue(date, DateTime.MinValue);
        }

        public static object GetForDBValue(object value, object dbnullvalue)
        {
            if (value.Equals(dbnullvalue))
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        public static object GetFromDBValue(object value, object dbnullvalue)
        {
            if (value == DBNull.Value)
            {
                return dbnullvalue;
            }
            else
            {
                return value;
            }
        }

        public static string GetFormatBilangan(decimal bilangan)
        {
            return GetFormatBilangan(bilangan, 0).ToString();
        }

        public static string GetFormatBilangan(decimal bilangan, int jmldesimal, bool indonesiaformat = true)
        {
            string format = "";
            switch (jmldesimal)
            {
                case 0:
                    format = "#,##0";
                    break;
                case 1:
                    format = "#,##0.#";
                    break;
                case 2:
                    format = "#,##0.#0";
                    break;
                case 3:
                    format = "#,##0.##0";
                    break;
                case 4:
                    format = "#,##0.###0";
                    break;
                default:
                    format = "#,##0.####0";
                    break;
            }
            string hasil = bilangan.ToString(format);
            if (indonesiaformat)
            {
                hasil = hasil.Replace(",", "@");
                hasil = hasil.Replace(".", ",");
                hasil = hasil.Replace("@", ".");
                if (hasil.IndexOf(",") < 0) hasil = hasil + ",0";
            }
            else
            {
                if (hasil.IndexOf(".") < 0) hasil = hasil + ".0";
            }
            return hasil;
        }

        public static string Terbilang(long x)
        {
            string[] bilangan = { "", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan", "sepuluh", "sebelas" };
            string temp = "";

            if (x < 12)
            {
                temp = " " + bilangan[x];
            }

            else if (x < 20)
            {
                temp = Terbilang(x - 10).ToString() + " belas";
            }

            else if (x < 100)
            {
                temp = Terbilang(x / 10) + " puluh" + Terbilang(x % 10);
            }

            else if (x < 200)
            {
                temp = " seratus" + Terbilang(x - 100);
            }

            else if (x < 1000)
            {
                temp = Terbilang(x / 100) + " ratus" + Terbilang(x % 100);
            }

            else if (x < 2000)
            {
                temp = " seribu" + Terbilang(x - 1000);
            }

            else if (x < 1000000)
            {
                temp = Terbilang(x / 1000) + " ribu" + Terbilang(x % 1000);
            }

            else if (x < 1000000000)
            {
                temp = Terbilang(x / 1000000) + " juta" + Terbilang(x % 1000000);
            }

            return temp;
        }
    }

    public static class TanggalValidator
    {
        public enum FormatTanggal { DDMMYYYY, MMDDYYYY, YYYYMMDD }

        public static DateTime GetSQLServerMinDate() { return new DateTime(1753, 1, 1); }

        public static DateTime GetStringToDate(string tanggal, FormatTanggal format, string separator)
        {
            DateTime hasil = GetSQLServerMinDate();

            string[] arr_tanggal = tanggal.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (arr_tanggal.Length == 3)
            {
                if (IsValid(tanggal, format, separator))
                {
                    switch (format)
                    {
                        case FormatTanggal.DDMMYYYY:
                            return new DateTime(int.Parse(arr_tanggal[2]), int.Parse(arr_tanggal[1]), int.Parse(arr_tanggal[0]));
                        case FormatTanggal.MMDDYYYY:
                            return new DateTime(int.Parse(arr_tanggal[2]), int.Parse(arr_tanggal[0]), int.Parse(arr_tanggal[1]));
                        case FormatTanggal.YYYYMMDD:
                            return new DateTime(int.Parse(arr_tanggal[0]), int.Parse(arr_tanggal[1]), int.Parse(arr_tanggal[2]));
                    }
                }
            }

            return hasil;
        }

        public static bool IsValid(string tanggal, FormatTanggal format, string separator)
        {
            bool hasil = false;
            string[] arr_tanggal = tanggal.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (arr_tanggal.Length == 3)
            {
                switch (format)
                {
                    case FormatTanggal.DDMMYYYY:
                        if ((arr_tanggal[0].Length > 0 && arr_tanggal[0].Length <= 2) &&
                            (arr_tanggal[1].Length > 0 && arr_tanggal[1].Length <= 2) &&
                            (arr_tanggal[2].Length == 4))
                        {
                            if (Libs.IsAngka(arr_tanggal[0]) && Libs.IsAngka(arr_tanggal[1]) && Libs.IsAngka(arr_tanggal[2]))
                            {
                                if (int.Parse(arr_tanggal[1]) <= 12)
                                {
                                    switch (int.Parse(arr_tanggal[1]))
                                    {
                                        case 1:
                                        case 3:
                                        case 5:
                                        case 7:
                                        case 8:
                                        case 10:
                                        case 12:
                                            if (int.Parse(arr_tanggal[0]) <= 31) return true;
                                            break;
                                        case 2:
                                            if ((int.Parse(arr_tanggal[0]) == 29 && int.Parse(arr_tanggal[2]) % 4 == 0) ||
                                                (int.Parse(arr_tanggal[0]) <= 28)) return true;
                                            break;
                                        case 4:
                                        case 6:
                                        case 9:
                                        case 11:
                                            if (int.Parse(arr_tanggal[0]) <= 30) return true;
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    case FormatTanggal.MMDDYYYY:
                        if ((arr_tanggal[0].Length > 0 && arr_tanggal[0].Length <= 2) &&
                            (arr_tanggal[1].Length > 0 && arr_tanggal[1].Length <= 2) &&
                            (arr_tanggal[2].Length == 4))
                        {
                            if (Libs.IsAngka(arr_tanggal[0]) && Libs.IsAngka(arr_tanggal[1]) && Libs.IsAngka(arr_tanggal[2]))
                            {
                                if (int.Parse(arr_tanggal[0]) <= 12)
                                {
                                    switch (int.Parse(arr_tanggal[0]))
                                    {
                                        case 1:
                                        case 3:
                                        case 5:
                                        case 7:
                                        case 8:
                                        case 10:
                                        case 12:
                                            if (int.Parse(arr_tanggal[1]) <= 31) return true;
                                            break;
                                        case 2:
                                            if ((int.Parse(arr_tanggal[1]) == 29 && int.Parse(arr_tanggal[2]) % 4 == 0) ||
                                                (int.Parse(arr_tanggal[1]) <= 28)) return true;
                                            break;
                                        case 4:
                                        case 6:
                                        case 9:
                                        case 11:
                                            if (int.Parse(arr_tanggal[1]) <= 30) return true;
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    case FormatTanggal.YYYYMMDD:
                        if ((arr_tanggal[1].Length > 0 && arr_tanggal[1].Length <= 2) &&
                            (arr_tanggal[2].Length > 0 && arr_tanggal[2].Length <= 2) &&
                            (arr_tanggal[0].Length == 4))
                        {
                            if (Libs.IsAngka(arr_tanggal[0]) && Libs.IsAngka(arr_tanggal[1]) && Libs.IsAngka(arr_tanggal[2]))
                            {
                                if (int.Parse(arr_tanggal[1]) <= 12)
                                {
                                    switch (int.Parse(arr_tanggal[1]))
                                    {
                                        case 1:
                                        case 3:
                                        case 5:
                                        case 7:
                                        case 8:
                                        case 10:
                                        case 12:
                                            if (int.Parse(arr_tanggal[2]) <= 31) return true;
                                            break;
                                        case 2:
                                            if ((int.Parse(arr_tanggal[2]) == 29 && int.Parse(arr_tanggal[0]) % 4 == 0) ||
                                                (int.Parse(arr_tanggal[2]) <= 28)) return true;
                                            break;
                                        case 4:
                                        case 6:
                                        case 9:
                                        case 11:
                                            if (int.Parse(arr_tanggal[2]) <= 30) return true;
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return hasil;
        }
    }

    //Contoh: ResponseHelper.Redirect("popup.aspx", "_blank", "menubar=0,width=100,height=100");
    public static class ResponseHelper
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {
                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException(
                        "Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }
    }
}
