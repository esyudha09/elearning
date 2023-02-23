using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning;

using System.IO;
using System.Text;

namespace AI_ERP.Application_Resources
{
    public partial class Download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (
                    Libs.GetQueryString("j") == Libs.JENIS_UPLOAD.MATERI_PEMBELAJARAN || 
                    Libs.GetQueryString("j") == Libs.JENIS_UPLOAD.PENDIDIKAN_NON_FORMAL ||
                    Libs.GetQueryString("j") == Libs.JENIS_UPLOAD.FILE_PENDUKUNG ||
                    Libs.GetQueryString("j") == Libs.JENIS_UPLOAD.RIWAYAT_KESEHATAN ||
                    Libs.GetQueryString("j") == Libs.JENIS_UPLOAD.RIWAYAT_MCU
                )
                {
                    DoDownloadToFile();
                }
                else
                {
                    if (Downloads.GetQS_JenisDownload() ==
                        Downloads.JenisDownload.REKAP_ABSENSI_SISWA)
                    {
                        DoDownloadRekapAbsensiSiswa();
                    }
                    else if (Downloads.GetQS_JenisDownload() ==
                        Downloads.JenisDownload.REKAP_ABSENSI_SISWA_V2)
                    {
                        DoDownloadRekapAbsensiSiswaV2();
                    }
                    else if (Downloads.GetQS_JenisDownload() ==
                        Downloads.JenisDownload.DATA_KD)
                    {
                        DoDownloadKD();
                    }
                    else if (Downloads.GetQS_JenisDownload() ==
                        Downloads.JenisDownload.HISTORY_LINK_PEMBELAJARAN_EKSTERNAL)
                    {
                        DoDownloadHistLinkPembelajaranEksternal();
                    }
                }                
            }
        }

        protected void DoDownloadRekapAbsensiSiswaV2()
        {
            string p = Libs.GetQueryString("p");
            string dt = Libs.GetQueryString("dt");
            string st = Libs.GetQueryString("st");
            string gb = Libs.GetQueryString("gb");

            string u = Libs.GetQueryString("u");
            string k = Libs.GetQueryString("k");
            string m = Libs.GetQueryString("m");
            string g = Libs.GetQueryString("g");
            string s1 = Libs.GetQueryString("s1");
            string s2 = Libs.GetQueryString("s2");

            string h = Libs.GetQueryString("h");
            string s = Libs.GetQueryString("s");
            string i = Libs.GetQueryString("i");
            string a = Libs.GetQueryString("a");
            string kd = Libs.GetQueryString("kd");

            string fp = Libs.GetQueryString("fp");
            string fk = Libs.GetQueryString("fk");

            string url = "";

            url = ResolveUrl("~/Application_Resources/DownloadAsFile.aspx");

            url += "?" + Downloads.JENIS_DOWNLOAD_KEY + "=" + Downloads.GetQS_JenisDownload() +
                   "&p=" + p +
                   "&dt=" + dt +
                   "&st=" + st +
                   "&gb=" + gb +

                   "&u=" + u +
                   "&k=" + k +
                   "&m=" + m +
                   "&g=" + g +
                   "&s1=" + s1 +
                   "&s2=" + s2 +

                   "&h=" + h +
                   "&s=" + s +
                   "&i=" + i +
                   "&a=" + a +
                   "&kd=" + kd +

                   "&fp=" + fp +
                   "&fk=" + fk;

            ltrDownload.Text = "<iframe src=\"" + url + "\" name=\"fra_download\" id=\"fra_download\" height=\"0\" width=\"0\"></iframe>";
        }

        protected void DoDownloadHistLinkPembelajaranEksternal()
        {
            string tgl1 = Libs.GetQueryString("tgl1");
            string tgl2 = Libs.GetQueryString("tgl2");
            string p = Libs.GetQueryString("p");

            string url = "";

            url = ResolveUrl("~/Application_Resources/DownloadAsFile.aspx");

            url += "?" + Downloads.JENIS_DOWNLOAD_KEY + "=" + Downloads.GetQS_JenisDownload() +
                   "&tgl1=" + tgl1 +
                   "&tgl2=" + tgl2 +
                   "&p=" + p;

            ltrDownload.Text = "<iframe src=\"" + url + "\" name=\"fra_download\" id=\"fra_download\" height=\"0\" width=\"0\"></iframe>";
        }

        protected void DoDownloadRekapAbsensiSiswa()
        {
            string tgl1 = Libs.GetQueryString("tgl1");
            string tgl2 = Libs.GetQueryString("tgl2");
            string kd = Libs.GetQueryString("kd");
            string m0 = Libs.GetQueryString("m");
            string jl = Libs.GetQueryString("jl");
            string t = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));

            string url = "";

            url = ResolveUrl("~/Application_Resources/DownloadAsFile.aspx");

            url += "?" + Downloads.JENIS_DOWNLOAD_KEY + "=" + Downloads.GetQS_JenisDownload() +
                   "&tgl1=" + tgl1 +
                   "&tgl2=" + tgl2 +
                   "&kd=" + kd +
                   "&m=" + m0 +
                   "&jl=" + jl +
                   "&t=" + t;

            ltrDownload.Text = "<iframe src=\"" + url + "\" name=\"fra_download\" id=\"fra_download\" height=\"0\" width=\"0\"></iframe>";
        }

        protected void DoDownloadKD()
        {
            string t = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            string s = Libs.GetQueryString("s");
            string k = Libs.GetQueryString("k");            

            string url = "";

            url = ResolveUrl("~/Application_Resources/DownloadAsFile.aspx");

            url += "?" + Downloads.JENIS_DOWNLOAD_KEY + "=" + Downloads.GetQS_JenisDownload() +
                   "&k=" + k +
                   "&s=" + s +
                   "&t=" + t;

            ltrDownload.Text = "<iframe src=\"" + url + "\" name=\"fra_download\" id=\"fra_download\" height=\"0\" width=\"0\"></iframe>";
        }

        protected void DoDownloadToFile()
        {
            string jenis = Libs.GetQueryString("j");
            string id = Libs.GetQueryString("id");
            string id2 = Libs.GetQueryString("id2");
            string file = Crypto.SimpleDecrypt(Libs.GetQueryString("f"));
            string url = "";

            switch (jenis)
            {
                case Libs.JENIS_UPLOAD.MATERI_PEMBELAJARAN:
                    url = "~/Application_Files/Elearning/Materi/";
                    Praota m = DAO_Praota.GetByID_Entity(id);
                    if (m != null)
                    {
                        if (m.TahunAjaran != null)
                        {
                            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m.Rel_Sekolah);
                            if (m_sekolah != null)
                            {
                                if (m_sekolah.Nama != null)
                                {
                                    url += m.TahunAjaran.Replace("/", "-") + "/" +
                                           m_sekolah.UrutanJenjang.ToString() + "/" +
                                           id + "/" +
                                           id2 + "/" +
                                           file;

                                    Response.Redirect(ResolveUrl(url));
                                }
                            }
                        }
                    }
                    break;
                case Libs.JENIS_UPLOAD.PENDIDIKAN_NON_FORMAL:
                    url = "~/Application_Files/Master/Pegawai/";
                    url += id + "/" +
                           "Pendidikan Non Formal/" +
                           id2 + "/" +
                           file;

                    Response.Redirect(ResolveUrl(url));
                    break;
                case Libs.JENIS_UPLOAD.FILE_PENDUKUNG:
                    url = "~/Application_Files/Master/Pegawai/";
                    url += id + "/" +
                           id2 + "/" +
                           file;

                    Response.Redirect(ResolveUrl(url));
                    break;
                case Libs.JENIS_UPLOAD.RIWAYAT_KESEHATAN:
                    url = "~/Application_Files/Master/Pegawai/";
                    url += id + "/" +
                           "Riwayat Kesehatan/" +
                           id2 + "/" +
                           file;

                    Response.Redirect(ResolveUrl(url));
                    break;
                case Libs.JENIS_UPLOAD.RIWAYAT_MCU:
                    url = "~/Application_Files/Master/Pegawai/";
                    url += id + "/" +
                           "Riwayat MCU/" +
                           id2 + "/" +
                           file;

                    Response.Redirect(ResolveUrl(url));
                    break;
                default:
                    if (Downloads.GetQS_JenisDownload() ==
                        Downloads.JenisDownload.REKAP_ABSENSI_SISWA)
                    {
                        string tgl1 = Libs.GetQueryString("tgl1");
                        string tgl2 = Libs.GetQueryString("tgl2");
                        string kd = Libs.GetQueryString("kd");
                        string m0 = Libs.GetQueryString("m");
                        string jl = Libs.GetQueryString("jl");
                        string t = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));

                        if (jl == "0")
                        {
                            ltrDownload.Text += GetHTMLRekapAbsen(
                                Libs.LOGGED_USER_M.NoInduk,
                                Libs.GetDateFromTanggalIndonesiaStr(tgl1),
                                Libs.GetDateFromTanggalIndonesiaStr(tgl2),
                                kd,
                                m0,
                                t
                            );
                        }
                        else if (jl == "1")
                        {
                            ltrDownload.Text += GetHTMLDetailAbsen(
                                Libs.LOGGED_USER_M.NoInduk,
                                Libs.GetDateFromTanggalIndonesiaStr(tgl1),
                                Libs.GetDateFromTanggalIndonesiaStr(tgl2),
                                kd,
                                m0,
                                t
                            );
                        }

                        Response.ContentType = "application/x-msexcel";
                        Response.AddHeader("Content-Disposition", "attachment; filename = Laporan Rekap Absensi Siswa " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                        Response.ContentEncoding = Encoding.UTF8;
                        StringWriter tw = new StringWriter();
                        HtmlTextWriter hw = new HtmlTextWriter(tw);
                        ltrDownload.RenderControl(hw);
                        Response.Write(tw.ToString());
                        Response.End();

                        return;
                    }
                    break;
            }
        }

        protected string GetHTMLDetailAbsen(string rel_guru, DateTime tgl1, DateTime tgl2, string rel_kelas_det, string rel_mapel, string tahun_ajaran)
        {
            string html = "";
            string s_mapel = "";
            string s_kelas = "";
            string html_header_tgl = "";
            
            if (rel_mapel.Trim() != "")
            {
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {
                        s_mapel = m_mapel.Nama;
                    }
                }
            }

            if (rel_mapel.Length <= 10) rel_mapel = "";

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(DAO_KelasDet.GetByID_Entity(rel_kelas_det).Rel_Kelas.ToString());
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            s_kelas = m_kelas_det.Nama;
                        }
                    }

                    //list siswa
                    List<Siswa> lst_siswa = new List<Siswa>();

                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                    if (m_sekolah != null)
                    {
                        if (m_sekolah.Nama != null)
                        {

                            if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && rel_mapel.Trim() == "")
                            {

                                lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    rel_kelas_det,
                                    RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t")),
                                    Libs.GetSemesterByTanggal(tgl1).ToString()
                                );

                            }
                            else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && rel_mapel.Trim() != "")
                            {
                                lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    rel_kelas_det,
                                    RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t")),
                                    Libs.GetSemesterByTanggal(tgl1).ToString()
                                );

                                if (lst_siswa.Count == 0)
                                {
                                    lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t")),
                                        Libs.GetSemesterByTanggal(tgl1).ToString()
                                    );
                                }

                            }
                            else
                            {
                                lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    rel_kelas_det,
                                    RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t")),
                                    Libs.GetSemesterByTanggal(tgl1).ToString()
                                );
                            }


                            int id = 1;
                            if (tgl1 <= tgl2)
                            {
                                double jml = (tgl2 - tgl1).TotalDays;
                                DateTime tanggal = tgl1;
                                for (double i = 0; i < jml; i++)
                                {
                                    html_header_tgl += "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" +
                                                           tanggal.Day.ToString() +
                                                       "</td>";
                                }

                                foreach (Siswa m_siswa in lst_siswa)
                                {
                                    SiswaAbsenMapelRekap m_absen = DAO_SiswaAbsenMapel.GetRekapPerSiswa_Entity(
                                            rel_kelas_det, rel_guru, (rel_mapel.Trim() == "" ? "-" : rel_mapel), tgl1, tgl2, m_siswa.Kode.ToString()
                                        ).FirstOrDefault();

                                    string H = "";
                                    string D = "";
                                    string T = "";
                                    string S = "";
                                    string I = "";
                                    string A = "";

                                    if (m_absen != null)
                                    {
                                        if (m_absen.Hadir != null)
                                        {
                                            H = m_absen.Hadir;
                                            D = m_absen.Ditugaskan;
                                            T = m_absen.Terlambat;
                                            S = m_absen.Sakit;
                                            I = m_absen.Izin;
                                            A = m_absen.TanpaKeterangan;
                                        }
                                    }

                                    string s_html_detail = "";
                                    tanggal = tgl1;
                                    for (double i = 0; i < jml; i++)
                                    {
                                        string s_absen = "";
                                        bool is_libur = DAO_AbsenPegawai.GetIsHariLibur(tanggal);
                                        if (rel_mapel.Trim() != "")
                                        {
                                            SiswaAbsenMapel m = DAO_SiswaAbsenMapel.
                                                GetAllBySekolahByKelasDetBySiswaByMapelByTanggal_Entity(
                                                    m_sekolah.Kode.ToString(),
                                                    rel_kelas_det,
                                                    m_siswa.Kode.ToString(),
                                                    rel_mapel,
                                                    tanggal
                                                ).FirstOrDefault();

                                            if (m != null)
                                            {
                                                if (m.TahunAjaran != null)
                                                {
                                                    s_absen = m.Absen;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            SiswaAbsen m = DAO_SiswaAbsen.
                                                GetAllBySekolahByKelasDetBySiswaByTanggal_Entity(
                                                    m_sekolah.Kode.ToString(),
                                                    rel_kelas_det,
                                                    m_siswa.Kode.ToString(),
                                                    tanggal
                                                ).FirstOrDefault();

                                            if (m != null)
                                            {
                                                if (m.TahunAjaran != null)
                                                {
                                                    s_absen = m.Absen;
                                                }
                                            }
                                        }

                                        s_html_detail += "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: center; " + (is_libur ? " background-color: red; " : "") + "\">" + s_absen + "</td>";
                                        tanggal = tanggal.AddDays(1);
                                    }

                                    string s_html_rekap = 
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black;\">" + id.ToString() + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black;\">" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "</td>" +
                                            s_html_detail +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + H + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + D + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + T + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + S + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + I + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + A + "</td>";

                                    html += "<tr>" +
                                                s_html_rekap +
                                            "</tr>";

                                    id++;

                                }
                            }
                        }
                    }
                }
            }

            return "<br />" +
                   "ABSENSI SISWA<br />" +
                   "PERIODE : " +
                        Libs.GetTanggalIndonesiaFromDate(tgl1, false) +
                        (
                            Libs.GetTanggalIndonesiaFromDate(tgl1, false).Trim() != Libs.GetTanggalIndonesiaFromDate(tgl2, false)
                            ? " s.d " + Libs.GetTanggalIndonesiaFromDate(tgl2, false)
                            : ""
                        ) +
                   "<br />" +
                   (
                        s_mapel.Trim() != ""
                        ? "Mata Pelajaran : " + s_mapel.Trim() + "<br />"
                        : ""
                   ) +
                   (
                        s_kelas.Trim() != ""
                        ? "Kelas : " + s_kelas.Trim() + "<br />"
                        : ""
                   ) +
                   "<table style=\"border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; vertical-align: middle;\">No.</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; vertical-align: middle;\">Nama Siswa</td>" +
                            html_header_tgl +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; vertical-align: middle;\">Hadir</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; vertical-align: middle;\">Ditugaskan</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; vertical-align: middle;\">Terlambat</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; vertical-align: middle;\">Sakit</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; vertical-align: middle;\">Izin</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; vertical-align: middle;\">Tanpa Keterangan</td>" +
                        "</tr>" +
                        html +
                   "</table>";
        }

        protected string GetHTMLRekapAbsen(string rel_guru, DateTime tgl1, DateTime tgl2, string rel_kelas_det, string rel_mapel, string tahun_ajaran)
        {
            string html = "";
            string s_mapel = "";
            string s_kelas = "";

            if (rel_mapel.Trim() != "")
            {
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {
                        s_mapel = m_mapel.Nama;
                    }
                }
            }

            if (rel_mapel.Length <= 10) rel_mapel = "";

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(DAO_KelasDet.GetByID_Entity(rel_kelas_det).Rel_Kelas.ToString());
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            s_kelas = m_kelas_det.Nama;
                        }
                    }

                    //list siswa
                    List<Siswa> lst_siswa = new List<Siswa>();

                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                    if (m_sekolah != null)
                    {
                        if (m_sekolah.Nama != null)
                        {

                            if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && rel_mapel.Trim() == "")
                            {

                                lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    rel_kelas_det,
                                    RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t")),
                                    Libs.GetSemesterByTanggal(tgl1).ToString()
                                );

                            }
                            else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && rel_mapel.Trim() != "")
                            {
                                lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    rel_kelas_det,
                                    RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t")),
                                    Libs.GetSemesterByTanggal(tgl1).ToString()
                                );

                                if (lst_siswa.Count == 0)
                                {
                                    lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t")),
                                        Libs.GetSemesterByTanggal(tgl1).ToString()
                                    );
                                }

                            }
                            else
                            {
                                lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    rel_kelas_det,
                                    RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t")),
                                    Libs.GetSemesterByTanggal(tgl1).ToString()
                                );
                            }


                            int id = 1;

                            foreach (Siswa m_siswa in lst_siswa)
                            {

                                SiswaAbsenMapelRekap m_absen = DAO_SiswaAbsenMapel.GetRekapPerSiswa_Entity(
                                        rel_kelas_det, rel_guru, (rel_mapel.Trim() == "" ? "-" : rel_mapel), tgl1, tgl2, m_siswa.Kode.ToString()
                                    ).FirstOrDefault();

                                string H = "";
                                string D = "";
                                string T = "";
                                string S = "";
                                string I = "";
                                string A = "";

                                if (m_absen != null)
                                {
                                    if (m_absen.Hadir != null)
                                    {
                                        H = m_absen.Hadir;
                                        D = m_absen.Ditugaskan;
                                        T = m_absen.Terlambat;
                                        S = m_absen.Sakit;
                                        I = m_absen.Izin;
                                        A = m_absen.TanpaKeterangan;
                                    }
                                }

                                html += "<tr>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black;\">" + id.ToString() + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black;\">" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + H + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + D + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + T + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + S + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + I + "</td>" +
                                            "<td style=\"border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + A + "</td>" +
                                        "</tr>";

                                id++;

                            }
                        }
                    }
                }
            }

            return "<br />" + 
                   "REKAP ABSENSI SISWA<br />" +
                   "PERIODE : " + 
                        Libs.GetTanggalIndonesiaFromDate(tgl1, false) + 
                        (
                            Libs.GetTanggalIndonesiaFromDate(tgl1, false).Trim() != Libs.GetTanggalIndonesiaFromDate(tgl2, false) 
                            ? " s.d " + Libs.GetTanggalIndonesiaFromDate(tgl2, false)
                            : ""
                        ) +
                   "<br />" +
                   (
                        s_mapel.Trim() != "" 
                        ? "Mata Pelajaran : " + s_mapel.Trim() + "<br />"
                        : ""
                   ) +
                   (
                        s_kelas.Trim() != ""
                        ? "Kelas : " + s_kelas.Trim() + "<br />"
                        : ""
                   ) +
                   "<table style=\"border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black;\">No.</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black;\">Nama Siswa</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black;\">Hadir</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black;\">Ditugaskan</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black;\">Terlambat</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black;\">Sakit</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black;\">Izin</td>" +
                            "<td style=\"border-style: solid; border-width: 1px; border-color: black;\">Tanpa Keterangan</td>" +
                        "</tr>" +
                        html +
                   "</table>";
        }
    }
}