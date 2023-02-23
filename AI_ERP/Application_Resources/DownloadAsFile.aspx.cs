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

using AI_ERP.Application_Entities.Elearning.TK;
using AI_ERP.Application_DAOs.Elearning.TK;
using AI_ERP.Application_DAOs.Elearning.SD;

using System.Data;
using System.Data.SqlClient;

using System.IO;
using System.Text;

namespace AI_ERP.Application_Resources
{
    public partial class DownloadAsFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Downloads.GetQS_JenisDownload() == Downloads.JenisDownload.MATRIX_RAPOR_TK)
                {
                    DownloadXLSMatrixRaporTK(
                            Libs.GetQueryString("t").Replace("-", "/"),
                            Libs.GetQueryString("s"),
                            Libs.GetQueryString("u"),
                            Libs.GetQueryString("kd"),
                            Libs.GetQueryString("ds")
                        );
                }
                else if (Downloads.GetQS_JenisDownload() == Downloads.JenisDownload.DATA_KD)
                {
                    DoDownloadKD();
                }
                else if (Downloads.GetQS_JenisDownload() == Downloads.JenisDownload.HISTORY_LINK_PEMBELAJARAN_EKSTERNAL)
                {
                    DoDownloadHistLinkPembelajaranEksternal();
                }
                else if (Downloads.GetQS_JenisDownload() == Downloads.JenisDownload.REKAP_ABSENSI_SISWA_V2)
                {
                    DoDownloadRekapAbsensiSiswaV2();
                }
                else
                {
                    DoDownloadToFile();
                }                
            }
        }

        public static string GetKeterangan()
        {
            return "<br />Keterangan : KWU adalah Keterangan Waktu Terlambat/Tidak, KAF adalah Keterangan Kamera Aktif/Tidak, SLP adalah Keterangan Seragam Lengkap/Tidak";
        }

        protected void DoDownload()
        {
            if (Downloads.GetQS_JenisDownload() ==
                Downloads.JenisDownload.REKAP_ABSENSI_SISWA)
            {
                string tgl1 = Libs.GetQueryString("tgl1");
                string tgl2 = Libs.GetQueryString("tgl2");
                string kd = Libs.GetQueryString("kd");
                string m = Libs.GetQueryString("m");
                string t = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));

                ltrDownload.Text += GetHTMLRekapAbsen(
                        Libs.LOGGED_USER_M.NoInduk,
                        Libs.GetDateFromTanggalIndonesiaStr(tgl1),
                        Libs.GetDateFromTanggalIndonesiaStr(tgl2),
                        kd,
                        m,
                        t
                    );

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
        }

        protected void DoDownloadRekapAbsensiSiswaV2()
        {
            if (Downloads.GetQS_JenisDownload() ==
                Downloads.JenisDownload.REKAP_ABSENSI_SISWA_V2)
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
                s2 = s1;

                string h = Libs.GetQueryString("h");
                string s = Libs.GetQueryString("s");
                string i = Libs.GetQueryString("i");
                string a = Libs.GetQueryString("a");
                string kd = Libs.GetQueryString("kd");

                string fp = Libs.GetQueryString("fp");
                string fk = Libs.GetQueryString("fk");

                string s_kode_siswa = "";
                if (s1.Trim() != "")
                {
                    List<Siswa> lst_siswa = DAO_Siswa.GetByUnit_Entity(
                        u, Libs.GetTahunAjaranByTanggal(Libs.GetDateFromTanggalIndonesiaStr(dt)), Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(dt)).ToString()
                    ).OrderBy(m0 => m0.Nama).ToList().FindAll(m0 => m0.Nama.ToUpper().Trim() == s1.ToUpper().Trim());

                    if (lst_siswa.Count > 0)
                    {
                        var m_siswa = lst_siswa.FirstOrDefault();
                        if (m_siswa != null)
                        {
                            if (m_siswa.Nama != null) s_kode_siswa = m_siswa.Kode.ToString();
                        }
                    }
                }

                Dictionary<string, string> lst_kd = new Dictionary<string, string>();
                string[] arr_kd = kd.Split(new string[] { ";" }, StringSplitOptions.None);
                List<Kedisiplinan> lst_kedisiplinan = DAO_Kedisiplinan.GetAll_Entity();
                int id = 0;
                foreach (var item in arr_kd)
                {
                    string[] arr_item = item.Split(new string[] { "|" }, StringSplitOptions.None);
                    if (arr_item.Length == 2)
                    {
                        Kedisiplinan m_kedisiplinan = lst_kedisiplinan.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == arr_item[0].ToString().ToUpper().Trim()).FirstOrDefault();
                        if (m_kedisiplinan != null)
                        {
                            if (m_kedisiplinan.Alias != null)
                            {
                                lst_kd.Add(id.ToString() + "|" + m_kedisiplinan.Alias, (arr_item[1] == "100" ? "100" : "0"));
                                id++;
                            }
                        }
                    }
                }

                string s_group = "";

                if (gb == "0") //by unit
                {
                    ltrDownload.Text = GetHTMLReportAbsensiSiswa_GroupBy_Unit(
                            Libs.GetDateFromTanggalIndonesiaStr(dt),
                            Libs.GetDateFromTanggalIndonesiaStr(st),
                            u,
                            k,
                            m,
                            g,
                            s1,
                            s2,
                            (h == "1" ? true : false),
                            (s == "1" ? true : false),
                            (i == "1" ? true : false),
                            (a == "1" ? true : false),
                            lst_kd,
                            fp,
                            fk
                        );

                    s_group = "Unit";
                }
                else if (gb == "1") //by kelas
                {
                    ltrDownload.Text = GetHTMLReportAbsensiSiswa_GroupBy_Kelas(
                            Libs.GetDateFromTanggalIndonesiaStr(dt),
                            Libs.GetDateFromTanggalIndonesiaStr(st),
                            u,
                            k,
                            m,
                            g,
                            s1,
                            s2,
                            (h == "1" ? true : false),
                            (s == "1" ? true : false),
                            (i == "1" ? true : false),
                            (a == "1" ? true : false),
                            lst_kd,
                            fp,
                            fk
                        );
                    s_group = "Kelas";
                }
                else if (gb == "2") //by matpel
                {
                    ltrDownload.Text = GetHTMLReportAbsensiSiswa_GroupBy_Mapel(
                            Libs.GetDateFromTanggalIndonesiaStr(dt),
                            Libs.GetDateFromTanggalIndonesiaStr(st),
                            u,
                            k,
                            m,
                            g,
                            s1,
                            s2,
                            (h == "1" ? true : false),
                            (s == "1" ? true : false),
                            (i == "1" ? true : false),
                            (a == "1" ? true : false),
                            lst_kd,
                            fp,
                            fk
                        );
                    s_group = "Mapel";
                }
                else if (gb == "3") //by guru
                {
                    ltrDownload.Text = GetHTMLReportAbsensiSiswa_GroupBy_Guru(
                            Libs.GetDateFromTanggalIndonesiaStr(dt),
                            Libs.GetDateFromTanggalIndonesiaStr(st),
                            u,
                            k,
                            m,
                            g,
                            s1,
                            s2,
                            (h == "1" ? true : false),
                            (s == "1" ? true : false),
                            (i == "1" ? true : false),
                            (a == "1" ? true : false),
                            lst_kd,
                            fp,
                            fk
                        );
                    s_group = "Guru";
                }
                else if (gb == "4") //by murid
                {
                    ltrDownload.Text = GetHTMLReportAbsensiSiswa_GroupBy_Siswa(
                            Libs.GetDateFromTanggalIndonesiaStr(dt),
                            Libs.GetDateFromTanggalIndonesiaStr(st),
                            u,
                            k,
                            m,
                            g,
                            s1,
                            s2,
                            (h == "1" ? true : false),
                            (s == "1" ? true : false),
                            (i == "1" ? true : false),
                            (a == "1" ? true : false),
                            lst_kd,
                            fp,
                            fk
                        );
                    s_group = "Murid";
                }
                else if (gb == "5") //by murid by mapel
                {
                    ltrDownload.Text = GetHTMLReportAbsensiSiswa_GroupBy_MapelSiswa(
                            Libs.GetDateFromTanggalIndonesiaStr(dt),
                            Libs.GetDateFromTanggalIndonesiaStr(st),
                            u,
                            k,
                            m,
                            g,
                            s1,
                            s2,
                            (h == "1" ? true : false),
                            (s == "1" ? true : false),
                            (i == "1" ? true : false),
                            (a == "1" ? true : false),
                            lst_kd,
                            fp,
                            fk
                        );
                    s_group = "Detail Murid Per Mapel";
                }
                else if (gb == "6") //by detail ketidakhadiran
                {
                    ltrDownload.Text = GetHTMLReportAbsensiSiswaDetail_GroupBy_AbsensiSiswa(
                            Libs.GetDateFromTanggalIndonesiaStr(dt),
                            Libs.GetDateFromTanggalIndonesiaStr(st),
                            u,
                            k,
                            m,
                            g,
                            s1,
                            s2,
                            (h == "1" ? true : false),
                            (s == "1" ? true : false),
                            (i == "1" ? true : false),
                            (a == "1" ? true : false),
                            lst_kd,
                            fp,
                            fk
                        );
                    s_group = "Detail Murid Per Absensi & Ketidak Disiplinan";
                }
                
                string nama_file = DateTime.Now.ToString("ddMMyy~HHmm") + "_" +
                                   (
                                        Libs.GetDateFromTanggalIndonesiaStr(dt) == Libs.GetDateFromTanggalIndonesiaStr(st)
                                        ? Libs.GetDateFromTanggalIndonesiaStr(dt).ToString("ddMMyy")
                                        : Libs.GetDateFromTanggalIndonesiaStr(dt).ToString("ddMMyy") + "s.d" +
                                          Libs.GetDateFromTanggalIndonesiaStr(st).ToString("ddMMyy")
                                   ) + "_" +
                                   s_group + "_" +
                                   (
                                        u.Trim() != ""
                                        ? DAO_Sekolah.GetByID_Entity(u).Nama
                                        : ""
                                   ) +
                                   (u.Trim() != "" && k.Trim() != "" ? "~" : "") +
                                   (
                                        k.Trim() != ""
                                        ? DAO_KelasDet.GetByID_Entity(k).Nama
                                        : ""
                                   ) +
                                   ((u.Trim() != "" || k.Trim() != "") && m.Trim() != "" ? "~" : "") +
                                   (
                                        m.Trim() != ""
                                        ? DAO_Mapel.GetByID_Entity(m).Nama
                                        : ""
                                   ) +
                                   ((u.Trim() != "" || k.Trim() != "" || m.Trim() != "") && g.Trim() != "" ? "~" : "") +
                                   (
                                        g.Trim() != ""
                                        ? DAO_Pegawai.GetByID_Entity(g).Nama
                                        : ""
                                   ) +
                                   ((u.Trim() != "" || k.Trim() != "" || m.Trim() != "" || g.Trim() != "") && s1.Trim() != "" ? "~" : "") +
                                   (
                                        s_kode_siswa.Trim() != ""
                                        ? DAO_Siswa.GetByKode_Entity(Libs.GetTahunAjaranByTanggal(Libs.GetDateFromTanggalIndonesiaStr(dt)), Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(dt)).ToString(), s_kode_siswa).Nama
                                        : ""
                                   );
                
                Response.ContentType = "application/x-msexcel";
                Response.AddHeader("Content-Disposition", "attachment; filename = " + nama_file.Replace(".", "").Replace("," , "") + ".xls");
                Response.ContentEncoding = Encoding.UTF8;
                StringWriter tw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(tw);
                ltrDownload.RenderControl(hw);
                Response.Write(tw.ToString());
                Response.End();

                return;
            }
        }

        protected void DoDownloadHistLinkPembelajaranEksternal()
        {
            if (Downloads.GetQS_JenisDownload() ==
                Downloads.JenisDownload.HISTORY_LINK_PEMBELAJARAN_EKSTERNAL)
            {
                string tgl1 = Libs.GetQueryString("tgl1");
                string tgl2 = Libs.GetQueryString("tgl2");
                string p = Libs.GetQueryString("p");

                ltrDownload.Text += GetHTMLHistLinkPembelajaranEksternal(
                        Libs.GetDateFromTanggalIndonesiaStr(tgl1),
                        Libs.GetDateFromTanggalIndonesiaStr(tgl2),
                        p
                    );

                Response.ContentType = "application/x-msexcel";
                Response.AddHeader("Content-Disposition", "attachment; filename = Laporan History Link Pembelajaran " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                Response.ContentEncoding = Encoding.UTF8;
                StringWriter tw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(tw);
                ltrDownload.RenderControl(hw);
                Response.Write(tw.ToString());
                Response.End();

                return;
            }
        }

        protected string GetHTMLHistLinkPembelajaranEksternal(DateTime tgl1, DateTime tgl2, string rel_pegawai)
        {
            string html = "";

            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            conn.Open();
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = AI_ERP.Application_DAOs.Elearning.DAO_LinkPembelajaranEksternalBuka.SP_SELECT_HISTORY_BY_PEGAWAI_BY_TANGGAL;
            comm.Parameters.AddWithValue("@Tanggal1", tgl1);
            comm.Parameters.AddWithValue("@Tanggal2", tgl2);
            comm.Parameters.AddWithValue("@Rel_Pegawai", rel_pegawai);

            DataTable dtResult = new DataTable();
            sqlDA = new SqlDataAdapter(comm);
            sqlDA.Fill(dtResult);

            foreach (DataRow row in dtResult.Rows)
            {
                html += "<tr>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + row["Nama"].ToString() + "</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + row["Kategori"].ToString() + "</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + row["Unit"].ToString() + "</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + row["Link"].ToString() + "</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + row["LinkOwner"].ToString() + "</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + Libs.GetTanggalIndonesiaFromDate(Convert.ToDateTime(row["Tanggal"]), true) + "</td>" +
                        "</tr>";
            }

            return "<br />" +
                   "HISTORY AKSES LINK PEMBELAJARAN EKSTERNAL<br />" +
                   "Dari Tanggal : " + Libs.GetTanggalIndonesiaFromDate(tgl1, false) + "<br />" +
                   "Sampai Tanggal : " + Libs.GetTanggalIndonesiaFromDate(tgl2, false) + "<br />" +
                   "Diakses Oleh : " + DAO_Pegawai.GetByID_Entity(rel_pegawai).Nama + 

                   "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Nama</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Kategori</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Unit</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Link</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Link Owner</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Tanggal</td>" +
                        "</tr>" +
                        html +
                   "</table>";
        }

        protected void DoDownloadKD()
        {
            if (Downloads.GetQS_JenisDownload() ==
                Downloads.JenisDownload.DATA_KD)
            {
                string t = Libs.GetQueryString("t").Replace("-", "/");
                string s = Libs.GetQueryString("s");
                string k = Libs.GetQueryString("k");

                ltrDownload.Text += GetHTMLKD_SD(
                        t, s, k
                    );

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
                default:
                    if (Downloads.GetQS_JenisDownload() ==
                        Downloads.JenisDownload.REKAP_ABSENSI_SISWA)
                    {
                        string tgl1 = Libs.GetQueryString("tgl1");
                        string tgl2 = Libs.GetQueryString("tgl2");
                        string kd = Libs.GetQueryString("kd");
                        string m0 = Libs.GetQueryString("m");
                        string jl = Libs.GetQueryString("jl");
                        string t = Libs.GetQueryString("t");

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

        protected void DownloadXLSMatrixRaporTK(string tahun_ajaran, string semester, string rel_sekolah, string rel_kelas_det, string rel_design)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            Rapor_Design m_desain = DAO_Rapor_Design.GetByID_Entity(rel_design);
            if (m_desain != null)
            {
                if (m_desain.TahunAjaran != null)
                {
                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = AI_ERP.Application_DAOs.Elearning.TK.DAO_Rapor_Nilai.SP_SELECT_BY_HEADER_NO_EKSKUL;
                    comm.Parameters.AddWithValue("@" + DAO_Rapor_DesignDet.NamaField.Rel_Rapor_Design, rel_design);

                    DataTable dtResult = new DataTable();
                    sqlDA = new SqlDataAdapter(comm);
                    sqlDA.Fill(dtResult);

                    List<AI_ERP.Application_DAOs.Elearning.TK.DAO_Rapor_Nilai.ItemDesign> lst_design = 
                        new List<AI_ERP.Application_DAOs.Elearning.TK.DAO_Rapor_Nilai.ItemDesign>();
                    foreach (DataRow row in dtResult.Rows)
                    {
                        lst_design.Add(new AI_ERP.Application_DAOs.Elearning.TK.DAO_Rapor_Nilai.ItemDesign {
                            Kode = row["Kode"].ToString(),
                            TahunAjaran = row["TahunAjaran"].ToString(),
                            Semester = row["Semester"].ToString(),
                            UrutKategori = row["UrutKategori"].ToString(),
                            PoinKategori = row["PoinKategori"].ToString(),
                            NamaKategori = row["NamaKategori"].ToString(),
                            UrutSubKategori = row["UrutSubKategori"].ToString(),
                            PoinSubKategori = row["PoinSubKategori"].ToString(),
                            NamaSubKategori = row["NamaSubKategori"].ToString(),
                            PoinItemPenilaian = row["PoinItemPenilaian"].ToString(),
                            NamaItemPenilaian = row["NamaItemPenilaian"].ToString(),
                            JenisKomponen = row["JenisKomponen"].ToString(),
                            IsNewPage = Convert.ToBoolean(row["IsNewPage"] == DBNull.Value ? false : row["IsNewPage"]),
                            Urut = Convert.ToInt32(row["Urut"] == DBNull.Value ? false : row["Urut"]),
                            Kriteria1 = row["Kriteria1"].ToString(),
                            NamaKriteria1 = row["NamaKriteria1"].ToString(),
                            Kriteria2 = row["Kriteria2"].ToString(),
                            NamaKriteria2 = row["NamaKriteria2"].ToString(),
                            Kriteria3 = row["Kriteria3"].ToString(),
                            NamaKriteria3 = row["NamaKriteria3"].ToString()
                        });
                    }

                    string s_html = "";
                    string s_html_header = "";
                    List<Rapor_DesignKriteria> lst_design_kriteria = DAO_Rapor_DesignKriteria.GetByHeader_Entity(rel_design);
                    List<Rapor_Kriteria> lst_kriteria = DAO_Rapor_Kriteria.GetAll_Entity();
                    List<Rapor_NilaiSiswa_Det> lst_nilai_siswa = AI_ERP.Application_DAOs.Elearning.TK.DAO_Rapor_NilaiSiswa_Det.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det);
                    Rapor_DesignKriteria m_design_kriteria = new Rapor_DesignKriteria();

                    int id = 0;
                    s_html_header += "<td style=\"font-weight: bold;\">Indikator</td>";
                    foreach (AI_ERP.Application_DAOs.Elearning.TK.DAO_Rapor_Nilai.ItemDesign item_design in lst_design)
                    {
                        List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                rel_sekolah, rel_kelas_det, tahun_ajaran, semester
                            ).OrderBy(m => m.Nama).ToList();

                        if (item_design.JenisKomponen.Trim() != "3")
                        {
                            s_html += "<tr>";
                            s_html += "<td>" +
                                        (
                                            item_design.JenisKomponen.Trim() == "0"
                                            ? ((item_design.PoinItemPenilaian.Trim() != "")
                                                  ? item_design.PoinItemPenilaian.Trim() + "&nbsp;"
                                                  : ""
                                              ) + Libs.GetHTMLSimpleText(item_design.NamaKategori)
                                            : (
                                                item_design.JenisKomponen.Trim() == "1"
                                                ? "&nbsp;&nbsp;" +
                                                  ((item_design.PoinItemPenilaian.Trim() != "")
                                                      ? item_design.PoinItemPenilaian.Trim() + "&nbsp;"
                                                      : ""
                                                  ) + Libs.GetHTMLSimpleText(item_design.NamaSubKategori)
                                                : "&nbsp;&nbsp;" +
                                                  "&nbsp;&nbsp;" +
                                                  ((item_design.PoinItemPenilaian.Trim() != "")
                                                      ? item_design.PoinItemPenilaian.Trim() + "&nbsp;"
                                                      : ""
                                                  ) + Libs.GetHTMLSimpleText(item_design.NamaItemPenilaian)
                                              )
                                        ) +
                                       "</td>";
                            foreach (Siswa m_siswa in lst_siswa)
                            {
                                if (id == 0)
                                {
                                    s_html_header += "<td>" +
                                                        m_siswa.Panggilan +
                                                     "</td>";
                                }

                                string item_nilei_kriteria = lst_nilai_siswa.FindAll(
                                        m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim() &&
                                              m0.Rel_Rapor_DesignDet.ToString().ToUpper().Trim() == item_design.Kode.ToString().ToUpper().Trim()
                                    ).FirstOrDefault().Rel_Rapor_Kriteria;
                                m_design_kriteria = lst_design_kriteria.FindAll(m0 => m0.Kode.ToString().Trim().ToUpper() == item_nilei_kriteria.Trim().ToUpper()).FirstOrDefault();
                                string s_nilai = "";

                                if (m_design_kriteria != null)
                                {
                                    if (m_design_kriteria.Alias != null)
                                    {
                                        s_nilai = m_design_kriteria.Alias;
                                    }
                                }

                                s_html += "<td>" +
                                                 s_nilai +
                                          "</td>";
                            }

                            id++;
                            s_html += "</tr>";
                        }
                    }

                    s_html = "<table style=\"border-collapse: collapse; border-width: 1px; border-style: solid;\">" +
                                "<tr>" +
                                    s_html_header +
                                "</tr>" +
                                s_html +
                             "</table>";

                    ltrDownload.Text = s_html;
                    Response.ContentType = "application/x-msexcel";
                    Response.AddHeader("Content-Disposition", "attachment; filename = Matriks Rapor TK " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                    Response.ContentEncoding = Encoding.UTF8;
                    StringWriter tw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(tw);
                    ltrDownload.RenderControl(hw);
                    Response.Write(tw.ToString());
                    Response.End();
                }
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
                                    Libs.GetQueryString("t"),
                                    Libs.GetSemesterByTanggal(tgl1).ToString()
                                );

                            }
                            else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && rel_mapel.Trim() != "")
                            {
                                lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    rel_kelas_det,
                                    Libs.GetQueryString("t"),
                                    Libs.GetSemesterByTanggal(tgl1).ToString()
                                );

                                if (lst_siswa.Count == 0)
                                {
                                    lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        Libs.GetQueryString("t"),
                                        Libs.GetSemesterByTanggal(tgl1).ToString()
                                    );
                                }

                            }
                            else
                            {
                                lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    rel_kelas_det,
                                    Libs.GetQueryString("t"),
                                    Libs.GetSemesterByTanggal(tgl1).ToString()
                                );
                            }


                            int id = 1;
                            if (tgl1 <= tgl2)
                            {
                                double jml = (tgl2 - tgl1).TotalDays;
                                DateTime tanggal = tgl1;
                                for (double i = 0; i <= jml; i++)
                                {
                                    html_header_tgl += "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: center; font-weight: bold;\">" +
                                                           tanggal.Day.ToString() +
                                                       "</td>";

                                    tanggal = tanggal.AddDays(1);
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
                                    for (double i = 0; i <= jml; i++)
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

                                        string s_color = "";
                                        if (s_absen == "H")
                                        {
                                            s_color = "color: green; ";
                                        }
                                        else if (s_absen == "D")
                                        {
                                            s_color = "color: blue; font-weight: bold; ";
                                        }
                                        else if (s_absen == "T")
                                        {
                                            s_color = "color: #0BA1A1; font-weight: bold; ";
                                        }
                                        else if (s_absen == "S")
                                        {
                                            s_color = "color: darkorange; font-weight: bold; ";
                                        }
                                        else if (s_absen == "I")
                                        {
                                            s_color = "color: orange; font-weight: bold; ";
                                        }
                                        else if (s_absen == "A")
                                        {
                                            s_color = "color: #E90080; font-weight: bold; ";
                                        }

                                        s_html_detail += "<td style=\"" + s_color + "border-style: solid; border-width: 1px; border-color: black; text-align: center; " + (is_libur ? " background-color: red; " : "") + "\">" + s_absen + "</td>";
                                        tanggal = tanggal.AddDays(1);
                                    }

                                    string s_html_rekap =
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + id.ToString() + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "</td>" +
                                            s_html_detail +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right; color: green; font-weight: bold; background-color: #FCFDDB;\">" + H + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right; color: blue; font-weight: bold; background-color: #FCFDDB;\">" + D + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right; color: #0BA1A1; font-weight: bold; background-color: #FCFDDB;\">" + T + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right; color: darkorange; font-weight: bold; background-color: #FCFDDB;\">" + S + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right; color: orange; font-weight: bold; background-color: #FCFDDB;\">" + I + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right; color: #E90080; font-weight: bold; background-color: #FCFDDB;\">" + A + "</td>";

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
                   "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; vertical-align: middle; font-weight: bold;\">No.</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; vertical-align: middle; font-weight: bold;\">Nama Siswa</td>" +
                            html_header_tgl +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; vertical-align: middle; font-weight: bold;\">Hadir</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; vertical-align: middle; font-weight: bold;\">Ditugaskan</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; vertical-align: middle; font-weight: bold;\">Terlambat</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; vertical-align: middle; font-weight: bold;\">Sakit</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; vertical-align: middle; font-weight: bold;\">Izin</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; vertical-align: middle; font-weight: bold;\">Tanpa Keterangan</td>" +
                        "</tr>" +
                        html +
                   "</table>";
        }

        protected string GetHTMLKD_SD(string tahun_ajaran, string semester, string rel_kelas)
        {
            string html = "";
            string s_kelas = "";
            
            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    s_kelas = m_kelas.Nama;

                    SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                    SqlCommand comm = conn.CreateCommand();
                    SqlDataAdapter sqlDA;

                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai.SP_SELECT_KD_BY_TA_BY_SM_KELAS;
                    comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                    comm.Parameters.AddWithValue("@Semester", semester);
                    comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);

                    DataTable dtResult = new DataTable();
                    sqlDA = new SqlDataAdapter(comm);
                    sqlDA.Fill(dtResult);

                    foreach (DataRow row in dtResult.Rows)
                    {
                        html += "<tr>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + row["Kelas"].ToString() + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + row["MataPelajaran"].ToString() + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + row["AspekPengetahuan"].ToString() + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + row["AspekPengetahuan"].ToString() + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + row["KompetensiDasar"].ToString() + "</td>" +
                                "</tr>";
                    }
                }
            }

            return "<br />" +
                   "DESKRIPSI KOMPETENSI DASAR<br />" +
                   "Periode : " + tahun_ajaran + "<br />" +
                   "Semester : " + semester +
                   (
                        s_kelas.Trim() != ""
                        ? "Kelas : " + s_kelas.Trim() + "<br />"
                        : ""
                   ) +
                   "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Kelas</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Mata Pelajaran</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Aspek Penilaian</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Kompetensi Dasar</td>" +
                        "</tr>" +
                        html +
                   "</table>";
        }

        protected string GetHTMLRekapAbsen(string rel_guru, DateTime tgl1, DateTime tgl2, string rel_kelas_det, string rel_mapel, string tahun_ajaran)
        {
            string html = "";
            string s_mapel = "";
            string s_kelas = "";
            bool b_mapel_pilihan = false;

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(DAO_KelasDet.GetByID_Entity(rel_kelas_det).Rel_Kelas.ToString());
            if (rel_mapel.Trim() != "")
            {
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {
                        s_mapel = m_mapel.Nama;
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                b_mapel_pilihan = (m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN && m_kelas.Nama.Trim().ToUpper() != "X" ? true : false);
                            }
                        }   
                    }
                }
            }

            if (rel_mapel.Length <= 10) rel_mapel = "";

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

                            if (b_mapel_pilihan)
                            {
                                lst_siswa = DAO_FormasiGuruMapelDetSiswaDet.GetSiswaByTABySMByMapelByKelasDet_Entity(
                                    Libs.GetQueryString("t"),
                                    Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                                    rel_mapel,
                                    rel_kelas_det
                                );
                            }
                            else
                            {
                                if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && rel_mapel.Trim() == "")
                                {

                                    lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        Libs.GetQueryString("t"),
                                        Libs.GetSemesterByTanggal(DateTime.Now).ToString()
                                    );

                                }
                                else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && rel_mapel.Trim() != "")
                                {
                                    lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        Libs.GetQueryString("t"),
                                        Libs.GetSemesterByTanggal(DateTime.Now).ToString()
                                    );

                                    if (lst_siswa.Count == 0)
                                    {
                                        lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                            m_kelas.Rel_Sekolah.ToString(),
                                            rel_kelas_det,
                                            Libs.GetQueryString("t"),
                                            Libs.GetSemesterByTanggal(DateTime.Now).ToString()
                                        );
                                    }

                                }
                                else
                                {
                                    lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        Libs.GetQueryString("t"),
                                        Libs.GetSemesterByTanggal(DateTime.Now).ToString()
                                    );
                                }
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
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + id.ToString() + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + H + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + D + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + T + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + S + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + I + "</td>" +
                                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + A + "</td>" +
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
                   "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">No.</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Nama Siswa</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Hadir</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Ditugaskan</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Terlambat</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Sakit</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Izin</td>" +
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">Tanpa Keterangan</td>" +
                        "</tr>" +
                        html +
                   "</table>";
        }

        public static List<Reports.AbsensiSiswa> GetHTMLReportAbsensiSiswa(
                DateTime dari_tanggal,
                DateTime sampai_tanggal,
                string rel_unit,
                string rel_kelasdet,
                string rel_mapel,
                string rel_guru,
                string nama_siswa1,
                string nama_siswa2,
                bool is_hadir,
                bool is_sakit,
                bool is_izin,
                bool is_alpa,
                Dictionary<string, string> lst_kedisiplinan
            )
        {
            List<Reports.AbsensiSiswa> lst_absensi = DAO_Reports.AbsensiSiswa.GetAbsenSiswa(dari_tanggal, sampai_tanggal, rel_unit, rel_kelasdet, rel_mapel, rel_guru);

            if (rel_unit.Trim() != "")
            {
                lst_absensi = lst_absensi.FindAll(m0 => m0.Rel_Unit.Trim().ToUpper() == rel_unit.Trim().ToUpper());
            }
            if (rel_kelasdet.Trim() != "")
            {
                lst_absensi = lst_absensi.FindAll(m0 => m0.Rel_KelasDet.Trim().ToUpper() == rel_kelasdet.Trim().ToUpper());
            }
            if (rel_mapel.Trim() != "")
            {
                lst_absensi = lst_absensi.FindAll(m0 => m0.Rel_Mapel.Trim().ToUpper() == rel_mapel.Trim().ToUpper());
            }
            if (rel_guru.Trim() != "")
            {
                lst_absensi = lst_absensi.FindAll(m0 => m0.Rel_Guru.Trim().ToUpper() == rel_guru.Trim().ToUpper());
            }
            if (nama_siswa1.Trim() != "" && nama_siswa2.Trim() != "")
            {
                //Siswa m_siswa_1 = DAO_Siswa.GetByKode_Entity(Libs.GetTahunAjaranByTanggal(dari_tanggal), Libs.GetSemesterByTanggal(dari_tanggal).ToString(), nama_siswa1);
                //Siswa m_siswa_2 = DAO_Siswa.GetByKode_Entity(Libs.GetTahunAjaranByTanggal(dari_tanggal), Libs.GetSemesterByTanggal(dari_tanggal).ToString(), nama_siswa2);
                lst_absensi = lst_absensi.Where(m0 => m0.Nama.CompareTo(nama_siswa1) >=0 && m0.Nama.CompareTo(nama_siswa2) <= 0).ToList();
            }
            else
            {
                if (nama_siswa1.Trim() != "")
                {
                    lst_absensi = lst_absensi.FindAll(m0 => m0.Nama.Trim().ToUpper() == nama_siswa1.Trim().ToUpper());
                }
                if (nama_siswa2.Trim() != "")
                {
                    lst_absensi = lst_absensi.FindAll(m0 => m0.Nama.Trim().ToUpper() == nama_siswa2.Trim().ToUpper());
                }
            }   

            //lst_absensi = lst_absensi.FindAll(
            //    m0 => (
            //            (m0.Kehadiran.Trim() != "" && m0.Kehadiran.Trim().IndexOf("100") < 0 && is_hadir == false) ||
            //            (m0.Kehadiran.Trim() != "" && m0.Kehadiran.Trim().IndexOf("100") >= 0 && is_hadir == true)
            //          ) ||
            //          (
            //            (m0.Sakit.Trim() != "" && m0.Sakit.Trim().IndexOf("100") < 0 && is_sakit == false) ||
            //            (m0.Sakit.Trim() != "" && m0.Sakit.Trim().IndexOf("100") >= 0 && is_sakit == true)
            //          ) ||
            //          (
            //            (m0.Izin.Trim() != "" && m0.Izin.Trim().IndexOf("100") < 0 && is_izin == false) ||
            //            (m0.Izin.Trim() != "" && m0.Izin.Trim().IndexOf("100") >= 0 && is_izin == true)
            //          ) ||
            //          (
            //            (m0.Alpa.Trim() != "" && m0.Alpa.Trim().IndexOf("100") < 0 && is_alpa == false) ||
            //            (m0.Alpa.Trim() != "" && m0.Alpa.Trim().IndexOf("100") >= 0 && is_alpa == true)
            //          )
            //    );

            //int id = 1;
            //foreach (var item_kedisiplinan in lst_kedisiplinan)
            //{
            //    switch (id)
            //    {
            //        case 1:
            //            lst_absensi = lst_absensi.FindAll(
            //                        m0 => m0.Is_Cat01 == item_kedisiplinan.Value
            //                    );
            //            break;
            //        case 2:
            //            lst_absensi = lst_absensi.FindAll(
            //                        m0 => m0.Is_Cat02 == item_kedisiplinan.Value
            //                    );
            //            break;
            //        case 3:
            //            lst_absensi = lst_absensi.FindAll(
            //                        m0 => m0.Is_Cat03 == item_kedisiplinan.Value
            //                    );
            //            break;
            //        case 4:
            //            lst_absensi = lst_absensi.FindAll(
            //                        m0 => m0.Is_Cat04 == item_kedisiplinan.Value
            //                    );
            //            break;
            //        case 5:
            //            lst_absensi = lst_absensi.FindAll(
            //                        m0 => m0.Is_Cat05 == item_kedisiplinan.Value
            //                    );
            //            break;
            //        case 6:
            //            lst_absensi = lst_absensi.FindAll(
            //                        m0 => m0.Is_Cat06 == item_kedisiplinan.Value
            //                    );
            //            break;
            //        case 7:
            //            lst_absensi = lst_absensi.FindAll(
            //                        m0 => m0.Is_Cat07 == item_kedisiplinan.Value
            //                    );
            //            break;
            //        case 8:
            //            lst_absensi = lst_absensi.FindAll(
            //                        m0 => m0.Is_Cat08 == item_kedisiplinan.Value
            //                    );
            //            break;
            //        case 9:
            //            lst_absensi = lst_absensi.FindAll(
            //                        m0 => m0.Is_Cat09 == item_kedisiplinan.Value
            //                    );
            //            break;
            //        case 10:
            //            lst_absensi = lst_absensi.FindAll(
            //                        m0 => m0.Is_Cat10 == item_kedisiplinan.Value
            //                    );
            //            break;
            //        default:
            //            break;
            //    }

            //    id++;
            //}

            return lst_absensi;
        }

        public static string GetHTMLReportAbsensiSiswa_GroupBy_Unit(
                DateTime dari_tanggal,
                DateTime sampai_tanggal,
                string rel_unit,
                string rel_kelasdet,
                string rel_mapel,
                string rel_guru,
                string nama_siswa1,
                string nama_siswa2,
                bool is_hadir,
                bool is_sakit,
                bool is_izin,
                bool is_alpa,
                Dictionary<string, string> lst_kedisiplinan,
                string fp,
                string fk
            )
        {
            List<Reports.AbsensiSiswa> lst = GetHTMLReportAbsensiSiswa(
                    dari_tanggal,
                    sampai_tanggal,
                    rel_unit,
                    rel_kelasdet,
                    rel_mapel,
                    rel_guru,
                    nama_siswa1,
                    nama_siswa2,
                    is_hadir,
                    is_sakit,
                    is_izin,
                    is_alpa,
                    lst_kedisiplinan
                );

            string html_content = "";
            string html_field_performansi = "";
            string html_field_performansi_ket = "";

            //filter presensi dan kedisiplinan
            for (int i = lst_kedisiplinan.Count - 1; i >= 0; i--)
            {
                string key = lst_kedisiplinan.ToList()[i].Key;
                string value = lst_kedisiplinan.ToList()[i].Value;
                if (value == "0")
                {
                    lst_kedisiplinan.Remove(key);
                }
            }
            //end filter

            if (dari_tanggal == sampai_tanggal) //harian
            {
                foreach (var item in lst_kedisiplinan)
                {
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " xy</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " x</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " y</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " %</td>";

                    html_field_performansi_ket +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                }

                var lst_distinct = lst.Select(m0 => new { m0.TahunAjaran, m0.Semester, m0.Unit, m0.Tanggal }).Distinct().ToList();
                foreach (var item_distinct in lst_distinct)
                {
                    string html_performansi_kedisiplinan_content = "";
                    string html_keterangan_kedisiplinan_content = "";

                    //kehadiran
                    decimal d_kehadiran = 0;
                    decimal d_total = 0;
                    if (lst.FindAll(
                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                m0.Semester == item_distinct.Semester &&
                                m0.Unit == item_distinct.Unit &&
                                m0.Tanggal == item_distinct.Tanggal).Count > 0)
                    {
                        d_total = lst.FindAll(
                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                m0.Semester == item_distinct.Semester &&
                                                m0.Unit == item_distinct.Unit &&
                                                m0.Tanggal == item_distinct.Tanggal
                                    ).Count;

                        d_kehadiran = lst.FindAll(
                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                m0.Semester == item_distinct.Semester &&
                                                m0.Unit == item_distinct.Unit &&
                                                m0.Tanggal == item_distinct.Tanggal &&
                                                m0.Kehadiran == "100"
                                    ).Count;
                    }
                    if (is_hadir)
                    {
                        if (d_total > 0)
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_kehadiran.ToString() + "/" + d_total.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round(((d_kehadiran / d_total) * 100), 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }
                    }

                    //keterangan ketidakhadiran
                    string s_ket_sakit = "";
                    string s_ket_izin = "";
                    string s_ket_alpa = "";
                    var lst_keterangan_kedisiplinan =
                            lst.FindAll(
                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                      m0.Semester == item_distinct.Semester &&
                                      m0.Unit == item_distinct.Unit &&
                                      m0.Tanggal == item_distinct.Tanggal
                                    );
                    if (lst_keterangan_kedisiplinan.Count > 0)
                    {
                        foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                        {
                            s_ket_sakit +=
                                (
                                    s_ket_sakit.Trim() != "" && item_ketidakhadiran.Is_Sakit_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Sakit_Keterangan;
                            s_ket_izin +=
                                (
                                    s_ket_izin.Trim() != "" && item_ketidakhadiran.Is_Izin_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Izin_Keterangan;
                            s_ket_alpa +=
                                (
                                    s_ket_alpa.Trim() != "" && item_ketidakhadiran.Is_Alpa_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Alpa_Keterangan;
                        }
                    }
                    if (is_sakit)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_sakit.ToString() + "</td>";
                    }
                    if (is_izin)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_izin.ToString() + "</td>";
                    }
                    if (is_alpa)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_alpa.ToString() + "</td>";
                    }

                    //kedisiplinan
                    int id = 1;
                    string s_ket_kedisiplinan = "";
                    foreach (var item_kedisiplinan in lst_kedisiplinan)
                    {
                        decimal d_kedisiplinan = 0;
                        int d_total_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                                m0.Semester == item_distinct.Semester &&
                                                                m0.Unit == item_distinct.Unit &&
                                                                m0.Tanggal == item_distinct.Tanggal &&
                                                                m0.Kehadiran == "100"
                                                    ).Count;

                        if (id == 1 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat01 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 2 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat02 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 3 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat03 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 4 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Is_Cat04 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 5 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat05 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 6 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat06 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 7 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat07 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 8 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat08 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 9 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat09 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 10 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat10 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }

                        if (d_total_kedisiplinan > 0)
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_kedisiplinan.ToString() + "/" + d_total_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round(((d_kedisiplinan / d_total_kedisiplinan) * 100), 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }

                        s_ket_kedisiplinan = "";
                        if (lst_keterangan_kedisiplinan.Count > 0)
                        {
                            foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                            {
                                if (id == 1)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat01_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat01_Keterangan;
                                }
                                else if (id == 2)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat02_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat02_Keterangan;
                                }
                                else if (id == 3)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat03_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat03_Keterangan;
                                }
                                else if (id == 4)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat04_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat04_Keterangan;
                                }
                                else if (id == 5)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat05_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat05_Keterangan;
                                }
                                else if (id == 6)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat06_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat06_Keterangan;
                                }
                                else if (id == 7)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat07_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat07_Keterangan;
                                }
                                else if (id == 8)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat08_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat08_Keterangan;
                                }
                                else if (id == 9)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat09_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat09_Keterangan;
                                }
                                else if (id == 10)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat10_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat10_Keterangan;
                                }
                            }
                        }
                        html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                s_ket_kedisiplinan + "</td>";

                        id++;
                    }

                    html_content += "<tr>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.TahunAjaran + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Semester + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Unit + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">'" + item_distinct.Tanggal.ToString("dd/MM/yyyy") + "</td>" +
                                        html_performansi_kedisiplinan_content +
                                        html_keterangan_kedisiplinan_content +
                                    "</tr>";
                }

                return "<br />" +
                    "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY UNIT [KELAS PERWALIAN & MATPEL]<br />" +
                    "PERIODE : " +
                        Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                        (
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                            ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                            : ""
                        ) +
                    GetKeterangan() +
                    "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tanggal</td>" +
                            "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * 4).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Persentase Performansi</td>" +
                            "<td colspan=\"" + (lst_kedisiplinan.Count + (is_sakit ? 1 : 0) + (is_izin ? 1 : 0) + (is_alpa ? 1 : 0)).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                        "</tr>" +
                        "<tr>" +
                            (
                                is_hadir
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran xy</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran x</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran y</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran %</td>"
                                : ""
                            ) +
                            html_field_performansi +
                            (
                                is_sakit
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>"
                                : ""
                            ) +
                            (
                                is_izin
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>"
                                : ""
                            ) +
                            (
                                is_alpa
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>"
                                : ""
                            ) +
                            html_field_performansi_ket +
                        "</tr>" +
                        html_content +
                    "</table>";
            }
            else
            {
                string html_field_performansi_det = "";
                int jumlah_hari = Math.Abs((dari_tanggal - sampai_tanggal).Days) + 1;
                DateTime dt_tanggal = dari_tanggal;

                foreach (var item in lst_kedisiplinan)
                {
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " xy</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " x</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " y</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " %</td>";

                    html_field_performansi +=
                        "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " (%)</td>";
                    html_field_performansi_ket +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                }

                var lst_distinct = lst.Select(m0 => new { m0.TahunAjaran, m0.Semester, m0.Unit }).Distinct().ToList();
                foreach (var item_distinct in lst_distinct)
                {
                    string html_performansi_kedisiplinan_content = "";
                    string html_performansi_kedisiplinan_content_det = "";
                    string html_keterangan_kedisiplinan_content = "";

                    //kehadiran
                    decimal d_total_kehadiran = 0;
                    decimal d_total_all = 0;

                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        decimal d_kehadiran = 0;
                        decimal d_total = 0;
                        if (lst.FindAll(
                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                    m0.Semester == item_distinct.Semester &&
                                    m0.Unit == item_distinct.Unit &&
                                    m0.Tanggal == dt_tanggal).Count > 0)
                        {
                            d_kehadiran = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.Tanggal == dt_tanggal &&
                                                    m0.Kehadiran == "100"
                                        ).Count;
                            d_total = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.Tanggal == dt_tanggal
                                        ).Count;
                        }
                        if (is_hadir)
                        {
                            if (d_total > 0)
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        Math.Round((d_kehadiran / d_total) * 100, 1).ToString() + "</td>";
                            }
                            else
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        "&nbsp;</td>";
                            }
                        }
                        dt_tanggal = dt_tanggal.AddDays(1);

                        d_total_kehadiran += d_kehadiran;
                        d_total_all += d_total;
                    }

                    if (is_hadir)
                    {
                        if (d_total_all > 0)
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_total_kehadiran.ToString() + "/" + d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round((d_total_kehadiran / d_total_all) * 100, 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }
                    }

                    //keterangan ketidakhadiran
                    string s_ket_sakit = "";
                    string s_ket_izin = "";
                    string s_ket_alpa = "";
                    var lst_keterangan_kedisiplinan =
                            lst.FindAll(
                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                      m0.Semester == item_distinct.Semester &&
                                      m0.Unit == item_distinct.Unit
                                    );
                    if (lst_keterangan_kedisiplinan.Count > 0)
                    {
                        foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                        {
                            s_ket_sakit +=
                                (
                                    s_ket_sakit.Trim() != "" && item_ketidakhadiran.Is_Sakit_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Sakit_Keterangan;
                            s_ket_izin +=
                                (
                                    s_ket_izin.Trim() != "" && item_ketidakhadiran.Is_Izin_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Izin_Keterangan;
                            s_ket_alpa +=
                                (
                                    s_ket_alpa.Trim() != "" && item_ketidakhadiran.Is_Alpa_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Alpa_Keterangan;
                        }
                    }
                    if (is_sakit)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_sakit.ToString() + "</td>";
                    }
                    if (is_izin)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_izin.ToString() + "</td>";
                    }
                    if (is_alpa)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_alpa.ToString() + "</td>";
                    }

                    //kedisiplinan
                    int id = 1;
                    string s_ket_kedisiplinan = "";
                    foreach (var item_kedisiplinan in lst_kedisiplinan)
                    {
                        d_total_kehadiran = 0;
                        d_total_all = 0;

                        dt_tanggal = dari_tanggal;
                        for (int j = 1; j <= jumlah_hari; j++)
                        {
                            decimal d_kedisiplinan = 0;
                            decimal d_kehadiran = 0;
                            int i_count = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.Tanggal == dt_tanggal
                                            ).Count;
                            d_kehadiran = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.Tanggal == dt_tanggal &&
                                                        m0.Kehadiran == "100"
                                            ).Count;

                            if (id == 1 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat01 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 2 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat02 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 3 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat03 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 4 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat04 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 5 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat05 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 6 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat06 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 7 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat07 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 8 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat08 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 9 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat09 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 10 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat10 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }

                            if (d_kehadiran > 0)
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        Math.Round((d_kedisiplinan / d_kehadiran) * 100, 1).ToString() + "</td>";
                            }
                            else
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        "&nbsp;</td>";
                            }

                            d_total_kehadiran += d_kedisiplinan;
                            d_total_all += d_kehadiran;
                            dt_tanggal = dt_tanggal.AddDays(1);
                        }

                        s_ket_kedisiplinan = "";
                        if (lst_keterangan_kedisiplinan.Count > 0)
                        {
                            foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                            {
                                if (id == 1)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat01_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat01_Keterangan;
                                }
                                else if (id == 2)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat02_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat02_Keterangan;
                                }
                                else if (id == 3)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat03_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat03_Keterangan;
                                }
                                else if (id == 4)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat04_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat04_Keterangan;
                                }
                                else if (id == 5)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat05_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat05_Keterangan;
                                }
                                else if (id == 6)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat06_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat06_Keterangan;
                                }
                                else if (id == 7)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat07_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat07_Keterangan;
                                }
                                else if (id == 8)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat08_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat08_Keterangan;
                                }
                                else if (id == 9)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat09_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat09_Keterangan;
                                }
                                else if (id == 10)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat10_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat10_Keterangan;
                                }
                            }
                        }

                        if (d_total_all > 0)
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_total_kehadiran.ToString() + "/" + d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round((d_total_kehadiran / d_total_all) * 100, 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }

                        html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                s_ket_kedisiplinan + "</td>";

                        id++;
                    }

                    html_content += "<tr>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.TahunAjaran + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Semester + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Unit + "</td>" +
                                        html_performansi_kedisiplinan_content +
                                        html_performansi_kedisiplinan_content_det +
                                        html_keterangan_kedisiplinan_content +
                                    "</tr>";
                }

                string s_html_field_tanggal = "";
                for (int i = 1; i <= (lst_kedisiplinan.Count + (is_hadir ? 1 : 0)); i++)
                {
                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        s_html_field_tanggal +=
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: center;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                "'" + dt_tanggal.ToString("dd/MM") +
                            "</td>";
                        dt_tanggal = dt_tanggal.AddDays(1);
                    }
                }

                return "<br />" +
                        "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY UNIT [KELAS PERWALIAN & MATPEL]<br />" +
                        "PERIODE : " +
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                            (
                                Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                : ""
                            ) +
                        GetKeterangan() +
                        "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                            "<tr>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * jumlah_hari).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Persentase Performansi</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * 4).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Performansi Detail</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_sakit ? 1 : 0) + (is_izin ? 1 : 0) + (is_alpa ? 1 : 0))).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                            "</tr>" +
                            "<tr>" +
                                (
                                    is_hadir
                                    ? "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran (%)</td>"
                                    : ""
                                ) +
                                html_field_performansi +
                                (
                                    is_hadir
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran xy</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran x</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran y</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran %</td>"
                                    : ""
                                ) +
                                html_field_performansi_det +
                                (
                                    is_sakit
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>"
                                    : ""
                                ) +
                                (
                                    is_izin
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>"
                                    : ""
                                ) +
                                (
                                    is_alpa
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>"
                                    : ""
                                ) +
                                html_field_performansi_ket +
                            "</tr>" +
                            "<tr>" +
                                s_html_field_tanggal +
                            "</tr>" +
                            html_content +
                        "</table>";
            }
        }

        public static string GetHTMLReportAbsensiSiswa_GroupBy_Siswa(
                DateTime dari_tanggal,
                DateTime sampai_tanggal,
                string rel_unit,
                string rel_kelasdet,
                string rel_mapel,
                string rel_guru,
                string nama_siswa1,
                string nama_siswa2,
                bool is_hadir,
                bool is_sakit,
                bool is_izin,
                bool is_alpa,
                Dictionary<string, string> lst_kedisiplinan,
                string fp,
                string fk
            )
        {
            List<Reports.AbsensiSiswa> lst = GetHTMLReportAbsensiSiswa(
                    dari_tanggal,
                    sampai_tanggal,
                    rel_unit,
                    rel_kelasdet,
                    rel_mapel,
                    rel_guru,
                    nama_siswa1,
                    nama_siswa2,
                    is_hadir,
                    is_sakit,
                    is_izin,
                    is_alpa,
                    lst_kedisiplinan
                );

            string html_content = "";
            string html_field_performansi = "";
            string html_field_performansi_ket = "";

            //filter presensi dan kedisiplinan
            for (int i = lst_kedisiplinan.Count - 1; i >= 0; i--)
            {
                string key = lst_kedisiplinan.ToList()[i].Key;
                string value = lst_kedisiplinan.ToList()[i].Value;
                if (value == "0")
                {
                    lst_kedisiplinan.Remove(key);
                }
            }
            //end filter

            if (dari_tanggal == sampai_tanggal) //harian
            {
                foreach (var item in lst_kedisiplinan)
                {
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " xy</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " x</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " y</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " %</td>";

                    html_field_performansi_ket +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                }

                var lst_distinct = lst.Select(m0 => new { m0.TahunAjaran, m0.Semester, m0.Unit, m0.Rel_KelasDet, m0.KelasDet, m0.Rel_Siswa, m0.Nama, m0.Tanggal }).Distinct().ToList();
                foreach (var item_distinct in lst_distinct)
                {
                    string html_performansi_kedisiplinan_content = "";
                    string html_keterangan_kedisiplinan_content = "";

                    //kehadiran
                    decimal d_kehadiran = 0;
                    decimal d_total = 0;
                    if (lst.FindAll(
                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                m0.Semester == item_distinct.Semester &&
                                m0.Unit == item_distinct.Unit &&
                                m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                m0.Nama == item_distinct.Nama &&
                                m0.Tanggal == item_distinct.Tanggal).Count > 0)
                    {
                        d_total = lst.FindAll(
                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                m0.Semester == item_distinct.Semester &&
                                                m0.Unit == item_distinct.Unit &&
                                                m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                m0.Nama == item_distinct.Nama &&
                                                m0.Tanggal == item_distinct.Tanggal
                                    ).Count;

                        d_kehadiran = lst.FindAll(
                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                m0.Semester == item_distinct.Semester &&
                                                m0.Unit == item_distinct.Unit &&
                                                m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                m0.Nama == item_distinct.Nama &&
                                                m0.Tanggal == item_distinct.Tanggal &&
                                                m0.Kehadiran == "100"
                                    ).Count;
                    }
                    if (is_hadir)
                    {
                        if (d_total > 0)
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_kehadiran.ToString() + "/" + d_total.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round(((d_kehadiran / d_total) * 100), 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }
                    }

                    //keterangan ketidakhadiran
                    string s_ket_sakit = "";
                    string s_ket_izin = "";
                    string s_ket_alpa = "";
                    var lst_keterangan_kedisiplinan =
                            lst.FindAll(
                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                      m0.Semester == item_distinct.Semester &&
                                      m0.Unit == item_distinct.Unit &&
                                      m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                      m0.Nama == item_distinct.Nama &&
                                      m0.Tanggal == item_distinct.Tanggal
                                    );
                    if (lst_keterangan_kedisiplinan.Count > 0)
                    {
                        foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                        {
                            s_ket_sakit +=
                                (
                                    s_ket_sakit.Trim() != "" && item_ketidakhadiran.Is_Sakit_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Sakit_Keterangan;
                            s_ket_izin +=
                                (
                                    s_ket_izin.Trim() != "" && item_ketidakhadiran.Is_Izin_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Izin_Keterangan;
                            s_ket_alpa +=
                                (
                                    s_ket_alpa.Trim() != "" && item_ketidakhadiran.Is_Alpa_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Alpa_Keterangan;
                        }
                    }
                    if (is_sakit)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_sakit.ToString() + "</td>";
                    }
                    if (is_izin)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_izin.ToString() + "</td>";
                    }
                    if (is_alpa)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_alpa.ToString() + "</td>";
                    }

                    //kedisiplinan
                    int id = 1;
                    string s_ket_kedisiplinan = "";
                    foreach (var item_kedisiplinan in lst_kedisiplinan)
                    {
                        decimal d_kedisiplinan = 0;
                        int d_total_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                                m0.Semester == item_distinct.Semester &&
                                                                m0.Unit == item_distinct.Unit &&
                                                                m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                                m0.Nama == item_distinct.Nama &&
                                                                m0.Tanggal == item_distinct.Tanggal &&
                                                                m0.Kehadiran == "100"
                                                    ).Count;

                        if (id == 1 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                          m0.Nama == item_distinct.Nama &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat01 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 2 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                          m0.Nama == item_distinct.Nama &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat02 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 3 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                          m0.Nama == item_distinct.Nama &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat03 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 4 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                          m0.Nama == item_distinct.Nama &&
                                                          m0.Is_Cat04 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 5 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                          m0.Nama == item_distinct.Nama &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat05 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 6 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                          m0.Nama == item_distinct.Nama &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat06 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 7 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                          m0.Nama == item_distinct.Nama &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat07 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 8 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                          m0.Nama == item_distinct.Nama &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat08 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 9 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                          m0.Nama == item_distinct.Nama &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat09 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 10 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                          m0.Nama == item_distinct.Nama &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat10 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }

                        if (d_total_kedisiplinan > 0)
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_kedisiplinan.ToString() + "/" + d_total_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round(((d_kedisiplinan / d_total_kedisiplinan) * 100), 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }

                        s_ket_kedisiplinan = "";
                        if (lst_keterangan_kedisiplinan.Count > 0)
                        {
                            foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                            {
                                if (id == 1)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat01_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat01_Keterangan;
                                }
                                else if (id == 2)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat02_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat02_Keterangan;
                                }
                                else if (id == 3)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat03_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat03_Keterangan;
                                }
                                else if (id == 4)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat04_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat04_Keterangan;
                                }
                                else if (id == 5)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat05_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat05_Keterangan;
                                }
                                else if (id == 6)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat06_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat06_Keterangan;
                                }
                                else if (id == 7)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat07_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat07_Keterangan;
                                }
                                else if (id == 8)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat08_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat08_Keterangan;
                                }
                                else if (id == 9)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat09_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat09_Keterangan;
                                }
                                else if (id == 10)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat10_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat10_Keterangan;
                                }
                            }
                        }
                        html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                s_ket_kedisiplinan + "</td>";

                        id++;
                    }

                    html_content += "<tr>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.TahunAjaran + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Semester + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Unit + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.KelasDet + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Nama + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">'" + item_distinct.Tanggal.ToString("dd/MM/yyyy") + "</td>" +
                                        html_performansi_kedisiplinan_content +
                                        html_keterangan_kedisiplinan_content +
                                    "</tr>";
                }

                return "<br />" +
                    "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY MURID [KELAS PERWALIAN & MATPEL]<br />" +
                    "PERIODE : " +
                        Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                        (
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                            ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                            : ""
                        ) +
                    GetKeterangan() +
                    "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kelas</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Siswa</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tanggal</td>" +
                            "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * 4).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Persentase Performansi</td>" +
                            "<td colspan=\"" + (lst_kedisiplinan.Count + (is_sakit ? 1 : 0) + (is_izin ? 1 : 0) + (is_alpa ? 1 : 0)).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                        "</tr>" +
                        "<tr>" +
                            (
                                is_hadir
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran xy</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran x</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran y</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran %</td>"
                                : ""
                            ) +
                            html_field_performansi +
                            (
                                is_sakit
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>"
                                : ""
                            ) +
                            (
                                is_izin
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>"
                                : ""
                            ) +
                            (
                                is_alpa
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>"
                                : ""
                            ) +
                            html_field_performansi_ket +
                        "</tr>" +
                        html_content +
                    "</table>";
            }
            else
            {
                string html_field_performansi_det = "";
                int jumlah_hari = Math.Abs((dari_tanggal - sampai_tanggal).Days) + 1;
                DateTime dt_tanggal = dari_tanggal;

                foreach (var item in lst_kedisiplinan)
                {
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " xy</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " x</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " y</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " %</td>";

                    html_field_performansi +=
                        "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " (%)</td>";
                    html_field_performansi_ket +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                }

                var lst_distinct = lst.Select(m0 => new { m0.TahunAjaran, m0.Semester, m0.Unit, m0.Rel_KelasDet, m0.KelasDet, m0.Rel_Siswa, m0.Nama }).Distinct().ToList();
                foreach (var item_distinct in lst_distinct)
                {
                    string html_performansi_kedisiplinan_content = "";
                    string html_performansi_kedisiplinan_content_det = "";
                    string html_keterangan_kedisiplinan_content = "";

                    //kehadiran
                    decimal d_total_kehadiran = 0;
                    decimal d_total_all = 0;

                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        decimal d_kehadiran = 0;
                        decimal d_total = 0;
                        if (lst.FindAll(
                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                    m0.Semester == item_distinct.Semester &&
                                    m0.Unit == item_distinct.Unit &&
                                    m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                    m0.Nama == item_distinct.Nama &&
                                    m0.Tanggal == dt_tanggal).Count > 0)
                        {
                            d_kehadiran = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                    m0.Nama == item_distinct.Nama &&
                                                    m0.Tanggal == dt_tanggal &&
                                                    m0.Kehadiran == "100"
                                        ).Count;
                            d_total = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                    m0.Nama == item_distinct.Nama &&
                                                    m0.Tanggal == dt_tanggal
                                        ).Count;
                        }
                        if (is_hadir)
                        {
                            if (d_total > 0)
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        Math.Round((d_kehadiran / d_total) * 100, 1).ToString() + "</td>";
                            }
                            else
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        "&nbsp;</td>";
                            }
                        }
                        dt_tanggal = dt_tanggal.AddDays(1);

                        d_total_kehadiran += d_kehadiran;
                        d_total_all += d_total;
                    }

                    if (is_hadir)
                    {
                        if (d_total_all > 0)
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_total_kehadiran.ToString() + "/" + d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round((d_total_kehadiran / d_total_all) * 100, 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }
                    }

                    //keterangan ketidakhadiran
                    string s_ket_sakit = "";
                    string s_ket_izin = "";
                    string s_ket_alpa = "";
                    var lst_keterangan_kedisiplinan =
                            lst.FindAll(
                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                      m0.Semester == item_distinct.Semester &&
                                      m0.Unit == item_distinct.Unit &&
                                      m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                      m0.Nama == item_distinct.Nama
                                    );
                    if (lst_keterangan_kedisiplinan.Count > 0)
                    {
                        foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                        {
                            s_ket_sakit +=
                                (
                                    s_ket_sakit.Trim() != "" && item_ketidakhadiran.Is_Sakit_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Sakit_Keterangan;
                            s_ket_izin +=
                                (
                                    s_ket_izin.Trim() != "" && item_ketidakhadiran.Is_Izin_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Izin_Keterangan;
                            s_ket_alpa +=
                                (
                                    s_ket_alpa.Trim() != "" && item_ketidakhadiran.Is_Alpa_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Alpa_Keterangan;
                        }
                    }
                    if (is_sakit)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_sakit.ToString() + "</td>";
                    }
                    if (is_izin)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_izin.ToString() + "</td>";
                    }
                    if (is_alpa)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_alpa.ToString() + "</td>";
                    }

                    //kedisiplinan
                    int id = 1;
                    string s_ket_kedisiplinan = "";
                    foreach (var item_kedisiplinan in lst_kedisiplinan)
                    {
                        d_total_kehadiran = 0;
                        d_total_all = 0;

                        dt_tanggal = dari_tanggal;
                        for (int j = 1; j <= jumlah_hari; j++)
                        {
                            decimal d_kedisiplinan = 0;
                            decimal d_kehadiran = 0;
                            int i_count = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                        m0.Nama == item_distinct.Nama &&
                                                        m0.Tanggal == dt_tanggal
                                            ).Count;
                            d_kehadiran = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                        m0.Nama == item_distinct.Nama &&
                                                        m0.Tanggal == dt_tanggal &&
                                                        m0.Kehadiran == "100"
                                            ).Count;

                            if (id == 1 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                              m0.Nama == item_distinct.Nama &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat01 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 2 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                              m0.Nama == item_distinct.Nama &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat02 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 3 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                              m0.Nama == item_distinct.Nama &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat03 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 4 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                              m0.Nama == item_distinct.Nama &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat04 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 5 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                              m0.Nama == item_distinct.Nama &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat05 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 6 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                              m0.Nama == item_distinct.Nama &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat06 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 7 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                              m0.Nama == item_distinct.Nama &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat07 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 8 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                              m0.Nama == item_distinct.Nama &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat08 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 9 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                              m0.Nama == item_distinct.Nama &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat09 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 10 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Siswa == item_distinct.Rel_Siswa &&
                                                              m0.Nama == item_distinct.Nama &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat10 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }

                            if (d_kehadiran > 0)
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        Math.Round((d_kedisiplinan / d_kehadiran) * 100, 1).ToString() + "</td>";
                            }
                            else
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        "&nbsp;</td>";
                            }

                            d_total_kehadiran += d_kedisiplinan;
                            d_total_all += d_kehadiran;
                            dt_tanggal = dt_tanggal.AddDays(1);
                        }

                        s_ket_kedisiplinan = "";
                        if (lst_keterangan_kedisiplinan.Count > 0)
                        {
                            foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                            {
                                if (id == 1)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat01_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat01_Keterangan;
                                }
                                else if (id == 2)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat02_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat02_Keterangan;
                                }
                                else if (id == 3)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat03_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat03_Keterangan;
                                }
                                else if (id == 4)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat04_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat04_Keterangan;
                                }
                                else if (id == 5)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat05_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat05_Keterangan;
                                }
                                else if (id == 6)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat06_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat06_Keterangan;
                                }
                                else if (id == 7)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat07_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat07_Keterangan;
                                }
                                else if (id == 8)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat08_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat08_Keterangan;
                                }
                                else if (id == 9)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat09_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat09_Keterangan;
                                }
                                else if (id == 10)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat10_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat10_Keterangan;
                                }
                            }
                        }
                        
                        if (d_total_all > 0)
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_total_kehadiran.ToString() + "/" + d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round((d_total_kehadiran / d_total_all) * 100, 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }

                        html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                s_ket_kedisiplinan + "</td>";

                        id++;
                    }

                    html_content += "<tr>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.TahunAjaran + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Semester + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Unit + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.KelasDet + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Nama + "</td>" +
                                        html_performansi_kedisiplinan_content +
                                        html_performansi_kedisiplinan_content_det +
                                        html_keterangan_kedisiplinan_content +
                                    "</tr>";
                }

                string s_html_field_tanggal = "";
                for (int i = 1; i <= (lst_kedisiplinan.Count + (is_hadir ? 1 : 0)); i++)
                {
                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        s_html_field_tanggal +=
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: center;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                "'" + dt_tanggal.ToString("dd/MM") +
                            "</td>";
                        dt_tanggal = dt_tanggal.AddDays(1);
                    }
                }

                return "<br />" +
                        "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY MURID [KELAS PERWALIAN & MATPEL]<br />" +
                        "PERIODE : " +
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                            (
                                Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                : ""
                            ) +
                        GetKeterangan() +
                        "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                            "<tr>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kelas</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Siswa</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * jumlah_hari).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Persentase Performansi</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * 4).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Performansi Detail</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_sakit ? 1 : 0) + (is_izin ? 1 : 0) + (is_alpa ? 1 : 0))).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                            "</tr>" +
                            "<tr>" +
                                (
                                    is_hadir
                                    ? "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran (%)</td>"
                                    : ""
                                ) +
                                html_field_performansi +
                                (
                                    is_hadir
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran xy</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran x</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran y</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran %</td>"
                                    : ""
                                ) +
                                html_field_performansi_det +
                                (
                                    is_sakit
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>"
                                    : ""
                                ) +
                                (
                                    is_izin
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>"
                                    : ""
                                ) +
                                (
                                    is_alpa
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>"
                                    : ""
                                ) +
                                html_field_performansi_ket +
                            "</tr>" +
                            "<tr>" +
                                s_html_field_tanggal +
                            "</tr>" +
                            html_content +
                        "</table>";
            }
        }

        public static string GetHTMLReportAbsensiSiswa_GroupBy_Siswa_V0(
                DateTime dari_tanggal,
                DateTime sampai_tanggal,
                string rel_unit,
                string rel_kelasdet,
                string rel_mapel,
                string rel_guru,
                string nama_siswa1,
                string nama_siswa2,
                bool is_hadir,
                bool is_sakit,
                bool is_izin,
                bool is_alpa,
                Dictionary<string, string> lst_kedisiplinan
            )
        {
            List<Reports.AbsensiSiswa> lst = GetHTMLReportAbsensiSiswa(
                    dari_tanggal,
                    sampai_tanggal,
                    rel_unit,
                    rel_kelasdet,
                    rel_mapel,
                    rel_guru,
                    nama_siswa1,
                    nama_siswa2,
                    is_hadir,
                    is_sakit,
                    is_izin,
                    is_alpa,
                    lst_kedisiplinan
                );

            string html_content = "";
            string html_field_performansi = "";
            string html_field_performansi_ket = "";

            if (dari_tanggal == sampai_tanggal) //harian
            {
                int jumlah_hari = 1;
                DateTime dt_tanggal = dari_tanggal;

                foreach (var item in lst_kedisiplinan)
                {
                    html_field_performansi +=
                        "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                    html_field_performansi_ket +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                }

                var lst_distinct = lst.Select(m0 => new { m0.TahunAjaran, m0.Semester, m0.Unit, m0.KelasDet, m0.NIS, m0.Nama }).Distinct().ToList();
                foreach (var item_distinct in lst_distinct)
                {
                    string html_performansi_kedisiplinan_content = "";
                    string html_keterangan_kedisiplinan_content = "";

                    //kehadiran
                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        decimal d_kehadiran = 0;
                        decimal d_total = 0;
                        if (lst.FindAll(
                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                    m0.Semester == item_distinct.Semester &&
                                    m0.Unit == item_distinct.Unit &&
                                    m0.KelasDet == item_distinct.KelasDet &&
                                    m0.NIS == item_distinct.NIS &&
                                    m0.Tanggal == dt_tanggal).Count > 0)
                        {
                            d_kehadiran = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.KelasDet == item_distinct.KelasDet &&
                                                    m0.NIS == item_distinct.NIS &&
                                                    m0.Tanggal == dt_tanggal &&
                                                    m0.Kehadiran == "100"
                                        ).Count;
                            d_total = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.KelasDet == item_distinct.KelasDet &&
                                                    m0.NIS == item_distinct.NIS &&
                                                    m0.Tanggal == dt_tanggal
                                        ).Count;
                        }
                        if (d_total > 0)
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                    Math.Round((d_kehadiran / d_total) * 100, 1).ToString() + "</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                    "&nbsp;</td>";
                        }
                        dt_tanggal = dt_tanggal.AddDays(1);
                    }

                    //keterangan ketidakhadiran
                    string s_ket_sakit = "";
                    string s_ket_izin = "";
                    string s_ket_alpa = "";
                    var lst_keterangan_kedisiplinan =
                            lst.FindAll(
                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                      m0.Semester == item_distinct.Semester &&
                                      m0.Unit == item_distinct.Unit &&
                                      m0.KelasDet == item_distinct.KelasDet &&
                                      m0.NIS == item_distinct.NIS
                                    );
                    if (lst_keterangan_kedisiplinan.Count > 0)
                    {
                        foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                        {
                            s_ket_sakit +=
                                (
                                    s_ket_sakit.Trim() != "" && item_ketidakhadiran.Is_Sakit_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Sakit_Keterangan;
                            s_ket_izin +=
                                (
                                    s_ket_izin.Trim() != "" && item_ketidakhadiran.Is_Izin_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Izin_Keterangan;
                            s_ket_alpa +=
                                (
                                    s_ket_alpa.Trim() != "" && item_ketidakhadiran.Is_Alpa_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Alpa_Keterangan;
                        }
                    }
                    html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                    s_ket_sakit.ToString() + "</td>";
                    html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                    s_ket_izin.ToString() + "</td>";
                    html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                    s_ket_alpa.ToString() + "</td>";

                    //kedisiplinan
                    int id = 1;
                    string s_ket_kedisiplinan = "";
                    foreach (var item_kedisiplinan in lst_kedisiplinan)
                    {
                        dt_tanggal = dari_tanggal;
                        for (int j = 1; j <= jumlah_hari; j++)
                        {
                            decimal d_kedisiplinan = 0;
                            decimal d_kehadiran = 0;
                            int i_count = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.KelasDet == item_distinct.KelasDet &&
                                                        m0.NIS == item_distinct.NIS &&
                                                        m0.Tanggal == dt_tanggal
                                            ).Count;
                            d_kehadiran = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.KelasDet == item_distinct.KelasDet &&
                                                        m0.NIS == item_distinct.NIS &&
                                                        m0.Tanggal == dt_tanggal &&
                                                        m0.Kehadiran == "100"
                                            ).Count;

                            if (id == 1 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat01 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 2 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat02 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 3 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat03 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 4 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat04 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 5 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat05 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 6 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat06 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 7 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat07 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 8 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat08 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 9 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat09 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 10 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat10 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }

                            if (d_kehadiran > 0)
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        Math.Round((d_kedisiplinan / d_kehadiran) * 100, 1).ToString() + "</td>";
                            }
                            else
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        "&nbsp;</td>";
                            }
                            dt_tanggal = dt_tanggal.AddDays(1);
                        }

                        s_ket_kedisiplinan = "";
                        if (lst_keterangan_kedisiplinan.Count > 0)
                        {
                            foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                            {
                                if (id == 1)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat01_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat01_Keterangan;
                                }
                                else if (id == 2)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat02_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat02_Keterangan;
                                }
                                else if (id == 3)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat03_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat03_Keterangan;
                                }
                                else if (id == 4)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat04_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat04_Keterangan;
                                }
                                else if (id == 5)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat05_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat05_Keterangan;
                                }
                                else if (id == 6)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat06_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat06_Keterangan;
                                }
                                else if (id == 7)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat07_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat07_Keterangan;
                                }
                                else if (id == 8)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat08_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat08_Keterangan;
                                }
                                else if (id == 9)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat09_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat09_Keterangan;
                                }
                                else if (id == 10)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat10_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat10_Keterangan;
                                }
                            }
                        }
                        html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                s_ket_kedisiplinan + "</td>";

                        id++;
                    }

                    html_content += "<tr>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.TahunAjaran + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Semester + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Unit + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.KelasDet + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.NIS + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + Libs.GetPerbaikiEjaanNama(item_distinct.Nama) + "</td>" +
                                        html_performansi_kedisiplinan_content +
                                        html_keterangan_kedisiplinan_content +
                                    "</tr>";
                }

                string s_html_field_tanggal = "";
                for (int i = 1; i <= (lst_kedisiplinan.Count + 1); i++)
                {
                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        s_html_field_tanggal +=
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: center;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                "'" + dt_tanggal.ToString("dd/MM") +
                            "</td>";
                        dt_tanggal = dt_tanggal.AddDays(1);
                    }
                }

                return "<br />" +
                        "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY MURID<br />" +
                        "PERIODE : " +
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                        GetKeterangan() +
                        "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                            "<tr>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kelas</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">NIS</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Nama</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + 1) * jumlah_hari).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Performansi</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + 3)).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran</td>" +
                                html_field_performansi +
                                "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>" +
                                "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>" +
                                "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>" +
                                html_field_performansi_ket +
                            "</tr>" +
                            "<tr>" +
                                s_html_field_tanggal +
                            "</tr>" +
                            html_content +
                        "</table>";
            }
            else
            {
                int jumlah_hari = Math.Abs((dari_tanggal - sampai_tanggal).Days) + 1;
                DateTime dt_tanggal = dari_tanggal;

                foreach (var item in lst_kedisiplinan)
                {
                    html_field_performansi +=
                        "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                    html_field_performansi_ket +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                }

                var lst_distinct = lst.Select(m0 => new { m0.TahunAjaran, m0.Semester, m0.Unit, m0.KelasDet, m0.NIS, m0.Nama }).Distinct().ToList();
                foreach (var item_distinct in lst_distinct)
                {
                    string html_performansi_kedisiplinan_content = "";
                    string html_keterangan_kedisiplinan_content = "";

                    //kehadiran
                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        decimal d_kehadiran = 0;
                        decimal d_total = 0;
                        if (lst.FindAll(
                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                    m0.Semester == item_distinct.Semester &&
                                    m0.Unit == item_distinct.Unit &&
                                    m0.KelasDet == item_distinct.KelasDet &&
                                    m0.NIS == item_distinct.NIS &&
                                    m0.Tanggal == dt_tanggal).Count > 0)
                        {
                            d_kehadiran = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.KelasDet == item_distinct.KelasDet &&
                                                    m0.NIS == item_distinct.NIS &&
                                                    m0.Tanggal == dt_tanggal &&
                                                    m0.Kehadiran == "100"
                                        ).Count;
                            d_total = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.KelasDet == item_distinct.KelasDet &&
                                                    m0.NIS == item_distinct.NIS &&
                                                    m0.Tanggal == dt_tanggal
                                        ).Count;
                        }
                        if (d_total > 0)
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                    Math.Round((d_kehadiran / d_total) * 100, 1).ToString() + "</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                    "&nbsp;</td>";
                        }
                        dt_tanggal = dt_tanggal.AddDays(1);
                    }

                    //keterangan ketidakhadiran
                    string s_ket_sakit = "";
                    string s_ket_izin = "";
                    string s_ket_alpa = "";
                    var lst_keterangan_kedisiplinan =
                            lst.FindAll(
                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                      m0.Semester == item_distinct.Semester &&
                                      m0.Unit == item_distinct.Unit &&
                                      m0.KelasDet == item_distinct.KelasDet &&
                                      m0.NIS == item_distinct.NIS
                                    );
                    if (lst_keterangan_kedisiplinan.Count > 0)
                    {
                        foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                        {
                            s_ket_sakit +=
                                (
                                    s_ket_sakit.Trim() != "" && item_ketidakhadiran.Is_Sakit_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Sakit_Keterangan;
                            s_ket_izin +=
                                (
                                    s_ket_izin.Trim() != "" && item_ketidakhadiran.Is_Izin_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Izin_Keterangan;
                            s_ket_alpa +=
                                (
                                    s_ket_alpa.Trim() != "" && item_ketidakhadiran.Is_Alpa_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Alpa_Keterangan;
                        }
                    }
                    html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                    s_ket_sakit.ToString() + "</td>";
                    html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                    s_ket_izin.ToString() + "</td>";
                    html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                    s_ket_alpa.ToString() + "</td>";

                    //kedisiplinan
                    int id = 1;
                    string s_ket_kedisiplinan = "";
                    foreach (var item_kedisiplinan in lst_kedisiplinan)
                    {
                        dt_tanggal = dari_tanggal;
                        for (int j = 1; j <= jumlah_hari; j++)
                        {
                            decimal d_kedisiplinan = 0;
                            decimal d_kehadiran = 0;
                            int i_count = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.KelasDet == item_distinct.KelasDet &&
                                                        m0.NIS == item_distinct.NIS &&
                                                        m0.Tanggal == dt_tanggal
                                            ).Count;
                            d_kehadiran = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.KelasDet == item_distinct.KelasDet &&
                                                        m0.NIS == item_distinct.NIS &&
                                                        m0.Tanggal == dt_tanggal &&
                                                        m0.Kehadiran == "100"
                                            ).Count;

                            if (id == 1 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat01 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 2 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat02 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 3 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat03 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 4 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat04 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 5 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat05 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 6 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat06 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 7 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat07 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 8 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat08 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 9 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat09 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 10 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.NIS == item_distinct.NIS &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat10 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }

                            if (d_kehadiran > 0)
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        Math.Round((d_kedisiplinan / d_kehadiran) * 100, 1).ToString() + "</td>";
                            }
                            else
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        "&nbsp;</td>";
                            }
                            dt_tanggal = dt_tanggal.AddDays(1);
                        }

                        s_ket_kedisiplinan = "";
                        if (lst_keterangan_kedisiplinan.Count > 0)
                        {
                            foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                            {
                                if (id == 1)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat01_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat01_Keterangan;
                                }
                                else if (id == 2)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat02_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat02_Keterangan;
                                }
                                else if (id == 3)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat03_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat03_Keterangan;
                                }
                                else if (id == 4)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat04_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat04_Keterangan;
                                }
                                else if (id == 5)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat05_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat05_Keterangan;
                                }
                                else if (id == 6)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat06_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat06_Keterangan;
                                }
                                else if (id == 7)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat07_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat07_Keterangan;
                                }
                                else if (id == 8)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat08_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat08_Keterangan;
                                }
                                else if (id == 9)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat09_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat09_Keterangan;
                                }
                                else if (id == 10)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat10_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat10_Keterangan;
                                }
                            }
                        }
                        html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                s_ket_kedisiplinan + "</td>";

                        id++;
                    }

                    html_content += "<tr>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.TahunAjaran + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Semester + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Unit + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.KelasDet + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.NIS + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + Libs.GetPerbaikiEjaanNama(item_distinct.Nama) + "</td>" +
                                        html_performansi_kedisiplinan_content +
                                        html_keterangan_kedisiplinan_content +
                                    "</tr>";
                }

                string s_html_field_tanggal = "";
                for (int i = 1; i <= (lst_kedisiplinan.Count + 1); i++)
                {
                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        s_html_field_tanggal +=
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: center;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                "'" + dt_tanggal.ToString("dd/MM") + 
                            "</td>";
                        dt_tanggal = dt_tanggal.AddDays(1);
                    }
                }

                return "<br />" +
                        "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY MURID<br />" +
                        "PERIODE : " +
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                            (
                                Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                : ""
                            ) +
                        GetKeterangan() +
                        "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                            "<tr>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kelas</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">NIS</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Nama</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + 1) * jumlah_hari).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Performansi</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + 3)).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran</td>" +
                                html_field_performansi +
                                "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>" +
                                "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>" +
                                "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>" +
                                html_field_performansi_ket +
                            "</tr>" +
                            "<tr>" +
                                s_html_field_tanggal +
                            "</tr>" +
                            html_content +
                        "</table>";
            }
        }

        public static string GetHTMLReportAbsensiSiswa_GroupBy_Guru(
                DateTime dari_tanggal,
                DateTime sampai_tanggal,
                string rel_unit,
                string rel_kelasdet,
                string rel_mapel,
                string rel_guru,
                string nama_siswa1,
                string nama_siswa2,
                bool is_hadir,
                bool is_sakit,
                bool is_izin,
                bool is_alpa,
                Dictionary<string, string> lst_kedisiplinan,
                string fp,
                string fk
            )
        {
            List<Reports.AbsensiSiswa> lst = GetHTMLReportAbsensiSiswa(
                    dari_tanggal,
                    sampai_tanggal,
                    rel_unit,
                    rel_kelasdet,
                    rel_mapel,
                    rel_guru,
                    nama_siswa1,
                    nama_siswa2,
                    is_hadir,
                    is_sakit,
                    is_izin,
                    is_alpa,
                    lst_kedisiplinan
                );

            //filter presensi dan kedisiplinan
            for (int i = lst_kedisiplinan.Count - 1; i >= 0; i--)
            {
                string key = lst_kedisiplinan.ToList()[i].Key;
                string value = lst_kedisiplinan.ToList()[i].Value;
                if (value == "0")
                {
                    lst_kedisiplinan.Remove(key);
                }
            }
            //end filter

            string html_content = "";
            string html_field_performansi = "";
            string html_field_performansi_ket = "";

            if (dari_tanggal == sampai_tanggal) //harian
            {
                foreach (var item in lst_kedisiplinan)
                {
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " xy</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " x</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " y</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " %</td>";

                    html_field_performansi_ket +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                }

                var lst_distinct = lst.Select(m0 => new { m0.TahunAjaran, m0.Semester, m0.Unit, m0.Rel_Guru, m0.Guru, m0.Tanggal }).Distinct().ToList();
                foreach (var item_distinct in lst_distinct)
                {
                    string html_performansi_kedisiplinan_content = "";
                    string html_keterangan_kedisiplinan_content = "";

                    //kehadiran
                    decimal d_kehadiran = 0;
                    decimal d_total = 0;
                    if (lst.FindAll(
                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                m0.Semester == item_distinct.Semester &&
                                m0.Unit == item_distinct.Unit &&
                                m0.Rel_Guru == item_distinct.Rel_Guru &&
                                m0.Guru == item_distinct.Guru &&
                                m0.Tanggal == item_distinct.Tanggal).Count > 0)
                    {
                        d_total = lst.FindAll(
                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                m0.Semester == item_distinct.Semester &&
                                                m0.Unit == item_distinct.Unit &&
                                                m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                m0.Guru == item_distinct.Guru &&
                                                m0.Tanggal == item_distinct.Tanggal
                                    ).Count;

                        d_kehadiran = lst.FindAll(
                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                m0.Semester == item_distinct.Semester &&
                                                m0.Unit == item_distinct.Unit && 
                                                m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                m0.Guru == item_distinct.Guru &&
                                                m0.Tanggal == item_distinct.Tanggal &&
                                                m0.Kehadiran == "100"
                                    ).Count;
                    }
                    if (is_hadir)
                    {
                        if (d_total > 0)
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_kehadiran.ToString() + "/" + d_total.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round(((d_kehadiran / d_total) * 100), 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }
                    }

                    //keterangan ketidakhadiran
                    string s_ket_sakit = "";
                    string s_ket_izin = "";
                    string s_ket_alpa = "";
                    var lst_keterangan_kedisiplinan =
                            lst.FindAll(
                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                      m0.Semester == item_distinct.Semester &&
                                      m0.Unit == item_distinct.Unit &&
                                      m0.Rel_Guru == item_distinct.Rel_Guru &&
                                      m0.Guru == item_distinct.Guru &&
                                      m0.Tanggal == item_distinct.Tanggal
                                    );
                    if (lst_keterangan_kedisiplinan.Count > 0)
                    {
                        foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                        {
                            s_ket_sakit +=
                                (
                                    s_ket_sakit.Trim() != "" && item_ketidakhadiran.Is_Sakit_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Sakit_Keterangan;
                            s_ket_izin +=
                                (
                                    s_ket_izin.Trim() != "" && item_ketidakhadiran.Is_Izin_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Izin_Keterangan;
                            s_ket_alpa +=
                                (
                                    s_ket_alpa.Trim() != "" && item_ketidakhadiran.Is_Alpa_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Alpa_Keterangan;
                        }
                    }
                    if (is_sakit)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_sakit.ToString() + "</td>";
                    }
                    if (is_izin)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_izin.ToString() + "</td>";
                    }
                    if (is_alpa)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_alpa.ToString() + "</td>";
                    }

                    //kedisiplinan
                    int id = 1;
                    string s_ket_kedisiplinan = "";
                    foreach (var item_kedisiplinan in lst_kedisiplinan)
                    {
                        decimal d_kedisiplinan = 0;
                        int d_total_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                                m0.Semester == item_distinct.Semester &&
                                                                m0.Unit == item_distinct.Unit &&
                                                                m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                                m0.Guru == item_distinct.Guru &&
                                                                m0.Tanggal == item_distinct.Tanggal &&
                                                                m0.Kehadiran == "100"
                                                    ).Count;

                        if (id == 1 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                          m0.Guru == item_distinct.Guru &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat01 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 2 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                          m0.Guru == item_distinct.Guru &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat02 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 3 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                          m0.Guru == item_distinct.Guru &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat03 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 4 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                          m0.Guru == item_distinct.Guru &&
                                                          m0.Is_Cat04 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 5 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                          m0.Guru == item_distinct.Guru &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat05 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 6 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                          m0.Guru == item_distinct.Guru &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat06 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 7 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                          m0.Guru == item_distinct.Guru &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat07 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 8 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                          m0.Guru == item_distinct.Guru &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat08 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 9 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                          m0.Guru == item_distinct.Guru &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat09 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 10 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                          m0.Guru == item_distinct.Guru &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat10 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }

                        if (d_total_kedisiplinan > 0)
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_kedisiplinan.ToString() + "/" + d_total_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round(((d_kedisiplinan / d_total_kedisiplinan) * 100), 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }

                        s_ket_kedisiplinan = "";
                        if (lst_keterangan_kedisiplinan.Count > 0)
                        {
                            foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                            {
                                if (id == 1)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat01_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat01_Keterangan;
                                }
                                else if (id == 2)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat02_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat02_Keterangan;
                                }
                                else if (id == 3)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat03_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat03_Keterangan;
                                }
                                else if (id == 4)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat04_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat04_Keterangan;
                                }
                                else if (id == 5)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat05_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat05_Keterangan;
                                }
                                else if (id == 6)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat06_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat06_Keterangan;
                                }
                                else if (id == 7)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat07_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat07_Keterangan;
                                }
                                else if (id == 8)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat08_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat08_Keterangan;
                                }
                                else if (id == 9)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat09_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat09_Keterangan;
                                }
                                else if (id == 10)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat10_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat10_Keterangan;
                                }
                            }
                        }
                        html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                s_ket_kedisiplinan + "</td>";

                        id++;
                    }

                    html_content += "<tr>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.TahunAjaran + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Semester + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Unit + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Guru + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">'" + item_distinct.Tanggal.ToString("dd/MM/yyyy") + "</td>" +
                                        html_performansi_kedisiplinan_content +
                                        html_keterangan_kedisiplinan_content +
                                    "</tr>";
                }

                return "<br />" +
                    "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY GURU [KELAS PERWALIAN & MATPEL]<br />" +
                    "PERIODE : " +
                        Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                        (
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                            ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                            : ""
                        ) +
                    GetKeterangan() +
                    "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Guru</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tanggal</td>" +
                            "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * 4).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Persentase Performansi</td>" +
                            "<td colspan=\"" + (lst_kedisiplinan.Count + (is_sakit ? 1 : 0) + (is_izin ? 1 : 0) + (is_alpa ? 1 : 0)).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                        "</tr>" +
                        "<tr>" +
                            (
                                is_hadir
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran xy</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran x</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran y</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran %</td>"
                                : ""
                            ) +
                            html_field_performansi +
                            (
                                is_sakit
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>"
                                : ""
                            ) +
                            (
                                is_izin
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>"
                                : ""
                            ) +
                            (
                                is_alpa
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>"
                                : ""
                            ) +
                            html_field_performansi_ket +
                        "</tr>" +
                        html_content +
                    "</table>";
            }
            else
            {
                string html_field_performansi_det = "";
                int jumlah_hari = Math.Abs((dari_tanggal - sampai_tanggal).Days) + 1;
                DateTime dt_tanggal = dari_tanggal;

                foreach (var item in lst_kedisiplinan)
                {
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " xy</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " x</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " y</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " %</td>";

                    html_field_performansi +=
                        "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " (%)</td>";
                    html_field_performansi_ket +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                }

                var lst_distinct = lst.Select(m0 => new { m0.TahunAjaran, m0.Semester, m0.Unit, m0.Rel_Guru, m0.Guru }).Distinct().ToList();
                foreach (var item_distinct in lst_distinct)
                {
                    string html_performansi_kedisiplinan_content = "";
                    string html_performansi_kedisiplinan_content_det = "";
                    string html_keterangan_kedisiplinan_content = "";

                    //kehadiran
                    decimal d_total_kehadiran = 0;
                    decimal d_total_all = 0;

                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        decimal d_kehadiran = 0;
                        decimal d_total = 0;
                        if (lst.FindAll(
                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                    m0.Semester == item_distinct.Semester &&
                                    m0.Unit == item_distinct.Unit &&
                                    m0.Rel_Guru == item_distinct.Rel_Guru &&
                                    m0.Guru == item_distinct.Guru &&
                                    m0.Tanggal == dt_tanggal).Count > 0)
                        {
                            d_kehadiran = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                    m0.Guru == item_distinct.Guru &&
                                                    m0.Tanggal == dt_tanggal &&
                                                    m0.Kehadiran == "100"
                                        ).Count;
                            d_total = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                    m0.Guru == item_distinct.Guru &&
                                                    m0.Tanggal == dt_tanggal
                                        ).Count;
                        }
                        if (is_hadir)
                        {
                            if (d_total > 0)
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        Math.Round((d_kehadiran / d_total) * 100, 1).ToString() + "</td>";
                            }
                            else
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        "&nbsp;</td>";
                            }
                        }
                        dt_tanggal = dt_tanggal.AddDays(1);

                        d_total_kehadiran += d_kehadiran;
                        d_total_all += d_total;
                    }

                    if (is_hadir)
                    {
                        if (d_total_all > 0)
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_total_kehadiran.ToString() + "/" + d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round((d_total_kehadiran / d_total_all) * 100, 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }
                    }

                    //keterangan ketidakhadiran
                    string s_ket_sakit = "";
                    string s_ket_izin = "";
                    string s_ket_alpa = "";
                    var lst_keterangan_kedisiplinan =
                            lst.FindAll(
                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                      m0.Semester == item_distinct.Semester &&
                                      m0.Unit == item_distinct.Unit &&
                                      m0.Rel_Guru == item_distinct.Rel_Guru &&
                                      m0.Guru == item_distinct.Guru
                                    );
                    if (lst_keterangan_kedisiplinan.Count > 0)
                    {
                        foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                        {
                            s_ket_sakit +=
                                (
                                    s_ket_sakit.Trim() != "" && item_ketidakhadiran.Is_Sakit_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Sakit_Keterangan;
                            s_ket_izin +=
                                (
                                    s_ket_izin.Trim() != "" && item_ketidakhadiran.Is_Izin_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Izin_Keterangan;
                            s_ket_alpa +=
                                (
                                    s_ket_alpa.Trim() != "" && item_ketidakhadiran.Is_Alpa_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Alpa_Keterangan;
                        }
                    }
                    if (is_sakit)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_sakit.ToString() + "</td>";
                    }
                    if (is_izin)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_izin.ToString() + "</td>";
                    }
                    if (is_alpa)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_alpa.ToString() + "</td>";
                    }

                    //kedisiplinan
                    int id = 1;
                    string s_ket_kedisiplinan = "";
                    foreach (var item_kedisiplinan in lst_kedisiplinan)
                    {
                        d_total_kehadiran = 0;
                        d_total_all = 0;

                        dt_tanggal = dari_tanggal;
                        for (int j = 1; j <= jumlah_hari; j++)
                        {
                            decimal d_kedisiplinan = 0;
                            decimal d_kehadiran = 0;
                            int i_count = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit && 
                                                        m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                        m0.Guru == item_distinct.Guru &&
                                                        m0.Tanggal == dt_tanggal
                                            ).Count;
                            d_kehadiran = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                        m0.Guru == item_distinct.Guru &&
                                                        m0.Tanggal == dt_tanggal &&
                                                        m0.Kehadiran == "100"
                                            ).Count;

                            if (id == 1 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                              m0.Guru == item_distinct.Guru &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat01 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 2 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                              m0.Guru == item_distinct.Guru &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat02 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 3 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                              m0.Guru == item_distinct.Guru &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat03 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 4 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                              m0.Guru == item_distinct.Guru &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat04 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 5 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                              m0.Guru == item_distinct.Guru &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat05 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 6 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                              m0.Guru == item_distinct.Guru &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat06 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 7 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                              m0.Guru == item_distinct.Guru &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat07 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 8 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                              m0.Guru == item_distinct.Guru &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat08 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 9 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                              m0.Guru == item_distinct.Guru &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat09 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 10 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Guru == item_distinct.Rel_Guru &&
                                                              m0.Guru == item_distinct.Guru &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat10 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }

                            if (d_kehadiran > 0)
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        Math.Round((d_kedisiplinan / d_kehadiran) * 100, 1).ToString() + "</td>";
                            }
                            else
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        "&nbsp;</td>";
                            }

                            d_total_kehadiran += d_kedisiplinan;
                            d_total_all += d_kehadiran;
                            dt_tanggal = dt_tanggal.AddDays(1);
                        }

                        s_ket_kedisiplinan = "";
                        if (lst_keterangan_kedisiplinan.Count > 0)
                        {
                            foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                            {
                                if (id == 1)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat01_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat01_Keterangan;
                                }
                                else if (id == 2)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat02_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat02_Keterangan;
                                }
                                else if (id == 3)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat03_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat03_Keterangan;
                                }
                                else if (id == 4)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat04_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat04_Keterangan;
                                }
                                else if (id == 5)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat05_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat05_Keterangan;
                                }
                                else if (id == 6)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat06_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat06_Keterangan;
                                }
                                else if (id == 7)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat07_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat07_Keterangan;
                                }
                                else if (id == 8)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat08_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat08_Keterangan;
                                }
                                else if (id == 9)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat09_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat09_Keterangan;
                                }
                                else if (id == 10)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat10_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat10_Keterangan;
                                }
                            }
                        }

                        if (d_total_all > 0)
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_total_kehadiran.ToString() + "/" + d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round((d_total_kehadiran / d_total_all) * 100, 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }

                        html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                s_ket_kedisiplinan + "</td>";

                        id++;
                    }

                    html_content += "<tr>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.TahunAjaran + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Semester + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Unit + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Guru + "</td>" +
                                        html_performansi_kedisiplinan_content +
                                        html_performansi_kedisiplinan_content_det +
                                        html_keterangan_kedisiplinan_content +
                                    "</tr>";
                }

                string s_html_field_tanggal = "";
                for (int i = 1; i <= (lst_kedisiplinan.Count + (is_hadir ? 1 : 0)); i++)
                {
                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        s_html_field_tanggal +=
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: center;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                "'" + dt_tanggal.ToString("dd/MM") +
                            "</td>";
                        dt_tanggal = dt_tanggal.AddDays(1);
                    }
                }

                return "<br />" +
                        "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY GURU [KELAS PERWALIAN & MATPEL]<br />" +
                        "PERIODE : " +
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                            (
                                Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                : ""
                            ) +
                        GetKeterangan() +
                        "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                            "<tr>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Mata Pelajaran</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * jumlah_hari).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Persentase Performansi</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * 4).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Performansi Detail</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_sakit ? 1 : 0) + (is_izin ? 1 : 0) + (is_alpa ? 1 : 0))).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                            "</tr>" +
                            "<tr>" +
                                (
                                    is_hadir
                                    ? "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran (%)</td>"
                                    : ""
                                ) +
                                html_field_performansi +
                                (
                                    is_hadir
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran xy</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran x</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran y</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran %</td>"
                                    : ""
                                ) +
                                html_field_performansi_det +
                                (
                                    is_sakit
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>"
                                    : ""
                                ) +
                                (
                                    is_izin
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>"
                                    : ""
                                ) +
                                (
                                    is_alpa
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>"
                                    : ""
                                ) +
                                html_field_performansi_ket +
                            "</tr>" +
                            "<tr>" +
                                s_html_field_tanggal +
                            "</tr>" +
                            html_content +
                        "</table>";
            }
        }

        public static string GetHTMLReportAbsensiSiswa_GroupBy_Mapel(
                DateTime dari_tanggal,
                DateTime sampai_tanggal,
                string rel_unit,
                string rel_kelasdet,
                string rel_mapel,
                string rel_guru,
                string nama_siswa1,
                string nama_siswa2,
                bool is_hadir,
                bool is_sakit,
                bool is_izin,
                bool is_alpa,
                Dictionary<string, string> lst_kedisiplinan,
                string fp,
                string fk
            )
        {
            List<Reports.AbsensiSiswa> lst = GetHTMLReportAbsensiSiswa(
                    dari_tanggal,
                    sampai_tanggal,
                    rel_unit,
                    rel_kelasdet,
                    rel_mapel,
                    rel_guru,
                    nama_siswa1,
                    nama_siswa2,
                    is_hadir,
                    is_sakit,
                    is_izin,
                    is_alpa,
                    lst_kedisiplinan
                );

            string html_content = "";
            string html_field_performansi = "";
            string html_field_performansi_ket = "";

            //filter presensi dan kedisiplinan
            for (int i = lst_kedisiplinan.Count - 1; i >= 0; i--)
            {
                string key = lst_kedisiplinan.ToList()[i].Key;
                string value = lst_kedisiplinan.ToList()[i].Value;
                if (value == "0")
                {
                    lst_kedisiplinan.Remove(key);
                }
            }
            //end filter

            if (dari_tanggal == sampai_tanggal) //harian
            {
                foreach (var item in lst_kedisiplinan)
                {
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " xy</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " x</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " y</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " %</td>";

                    html_field_performansi_ket +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                }

                var lst_distinct = lst.Select(m0 => new { m0.TahunAjaran, m0.Semester, m0.Unit, m0.Rel_Mapel, m0.Mapel, m0.Tanggal }).Distinct().ToList().FindAll(m0 => m0.Rel_Mapel.Trim() != "");
                foreach (var item_distinct in lst_distinct)
                {
                    string html_performansi_kedisiplinan_content = "";
                    string html_keterangan_kedisiplinan_content = "";

                    //kehadiran
                    decimal d_kehadiran = 0;
                    decimal d_total = 0;
                    if (lst.FindAll(
                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                m0.Semester == item_distinct.Semester &&
                                m0.Unit == item_distinct.Unit &&
                                m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                m0.Mapel == item_distinct.Mapel &&
                                m0.Tanggal == item_distinct.Tanggal).Count > 0)
                    {
                        d_total = lst.FindAll(
                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                m0.Semester == item_distinct.Semester &&
                                                m0.Unit == item_distinct.Unit &&
                                                m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                m0.Mapel == item_distinct.Mapel &&
                                                m0.Tanggal == item_distinct.Tanggal
                                    ).Count;

                        d_kehadiran = lst.FindAll(
                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                m0.Semester == item_distinct.Semester &&
                                                m0.Unit == item_distinct.Unit &&
                                                m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                m0.Mapel == item_distinct.Mapel &&
                                                m0.Tanggal == item_distinct.Tanggal &&
                                                m0.Kehadiran == "100"
                                    ).Count;
                    }
                    if (is_hadir)
                    {
                        if (d_total > 0)
                        {
                            html_performansi_kedisiplinan_content +=
                               "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                   "'" + d_kehadiran.ToString() + "/" + d_total.ToString() + "</td>" +
                               "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                   d_kehadiran.ToString() + "</td>" +
                               "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                   d_total.ToString() + "</td>" +
                               "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                   Math.Round(((d_kehadiran / d_total) * 100), 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }
                    }

                    //keterangan ketidakhadiran
                    string s_ket_sakit = "";
                    string s_ket_izin = "";
                    string s_ket_alpa = "";
                    var lst_keterangan_kedisiplinan =
                            lst.FindAll(
                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                      m0.Semester == item_distinct.Semester &&
                                      m0.Unit == item_distinct.Unit &&
                                      m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                      m0.Mapel == item_distinct.Mapel &&
                                      m0.Tanggal == item_distinct.Tanggal
                                    );
                    if (lst_keterangan_kedisiplinan.Count > 0)
                    {
                        foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                        {
                            s_ket_sakit +=
                                (
                                    s_ket_sakit.Trim() != "" && item_ketidakhadiran.Is_Sakit_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Sakit_Keterangan;
                            s_ket_izin +=
                                (
                                    s_ket_izin.Trim() != "" && item_ketidakhadiran.Is_Izin_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Izin_Keterangan;
                            s_ket_alpa +=
                                (
                                    s_ket_alpa.Trim() != "" && item_ketidakhadiran.Is_Alpa_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Alpa_Keterangan;
                        }
                    }
                    if (is_sakit)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_sakit.ToString() + "</td>";
                    }
                    if (is_izin)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_izin.ToString() + "</td>";
                    }
                    if (is_alpa)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_alpa.ToString() + "</td>";
                    }

                    //kedisiplinan
                    int id = 1;
                    string s_ket_kedisiplinan = "";
                    foreach (var item_kedisiplinan in lst_kedisiplinan)
                    {
                        decimal d_kedisiplinan = 0;
                        int d_total_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                                m0.Semester == item_distinct.Semester &&
                                                                m0.Unit == item_distinct.Unit &&
                                                                m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                                m0.Mapel == item_distinct.Mapel &&
                                                                m0.Tanggal == item_distinct.Tanggal &&
                                                                m0.Kehadiran == "100"
                                                    ).Count;

                        if (id == 1 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                          m0.Mapel == item_distinct.Mapel &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat01 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 2 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                          m0.Mapel == item_distinct.Mapel &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat02 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 3 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                          m0.Mapel == item_distinct.Mapel &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat03 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 4 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                          m0.Mapel == item_distinct.Mapel &&
                                                          m0.Is_Cat04 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 5 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                          m0.Mapel == item_distinct.Mapel &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat05 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 6 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                          m0.Mapel == item_distinct.Mapel &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat06 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 7 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                          m0.Mapel == item_distinct.Mapel &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat07 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 8 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                          m0.Mapel == item_distinct.Mapel &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat08 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 9 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                          m0.Mapel == item_distinct.Mapel &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat09 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 10 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                          m0.Mapel == item_distinct.Mapel &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat10 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }

                        if (d_total_kedisiplinan > 0)
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_kedisiplinan.ToString() + "/" + d_total_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round(((d_kedisiplinan / d_total_kedisiplinan) * 100), 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }

                        s_ket_kedisiplinan = "";
                        if (lst_keterangan_kedisiplinan.Count > 0)
                        {
                            foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                            {
                                if (id == 1)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat01_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat01_Keterangan;
                                }
                                else if (id == 2)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat02_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat02_Keterangan;
                                }
                                else if (id == 3)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat03_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat03_Keterangan;
                                }
                                else if (id == 4)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat04_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat04_Keterangan;
                                }
                                else if (id == 5)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat05_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat05_Keterangan;
                                }
                                else if (id == 6)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat06_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat06_Keterangan;
                                }
                                else if (id == 7)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat07_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat07_Keterangan;
                                }
                                else if (id == 8)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat08_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat08_Keterangan;
                                }
                                else if (id == 9)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat09_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat09_Keterangan;
                                }
                                else if (id == 10)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat10_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat10_Keterangan;
                                }
                            }
                        }
                        html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                s_ket_kedisiplinan + "</td>";

                        id++;
                    }

                    html_content += "<tr>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.TahunAjaran + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Semester + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Unit + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Mapel + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">'" + item_distinct.Tanggal.ToString("dd/MM/yyyy") + "</td>" +
                                        html_performansi_kedisiplinan_content +
                                        html_keterangan_kedisiplinan_content +
                                    "</tr>";
                }

                return "<br />" +
                    "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY MATA PELAJARAN [KELAS MATPEL]<br />" +
                    "PERIODE : " +
                        Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                        (
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                            ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                            : ""
                        ) +
                    GetKeterangan() +
                    "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Mata Pelajaran</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tanggal</td>" +
                            "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * 4).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Persentase Performansi</td>" +
                            "<td colspan=\"" + (lst_kedisiplinan.Count + (is_sakit ? 1 : 0) + (is_izin ? 1 : 0) + (is_alpa ? 1 : 0)).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                        "</tr>" +
                        "<tr>" +
                            (
                                is_hadir
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran xy</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran x</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran y</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran %</td>"
                                : ""
                            ) +
                            html_field_performansi +
                            (
                                is_sakit
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>"
                                : ""
                            ) +
                            (
                                is_izin
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>"
                                : ""
                            ) +
                            (
                                is_alpa
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>"
                                : ""
                            ) +
                            html_field_performansi_ket +
                        "</tr>" +
                        html_content +
                    "</table>";
            }
            else
            {
                string html_field_performansi_det = "";
                int jumlah_hari = Math.Abs((dari_tanggal - sampai_tanggal).Days) + 1;
                DateTime dt_tanggal = dari_tanggal;

                foreach (var item in lst_kedisiplinan)
                {
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " xy</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " x</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " y</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " %</td>";

                    html_field_performansi +=
                        "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " (%)</td>";
                    html_field_performansi_ket +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                }

                var lst_distinct = lst.Select(m0 => new { m0.TahunAjaran, m0.Semester, m0.Unit, m0.Rel_Mapel, m0.Mapel }).Distinct().ToList().FindAll(m0 => m0.Rel_Mapel.Trim() != "");
                foreach (var item_distinct in lst_distinct)
                {
                    string html_performansi_kedisiplinan_content = "";
                    string html_performansi_kedisiplinan_content_det = "";
                    string html_keterangan_kedisiplinan_content = "";

                    //kehadiran
                    decimal d_total_kehadiran = 0;
                    decimal d_total_all = 0;

                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        decimal d_kehadiran = 0;
                        decimal d_total = 0;
                        if (lst.FindAll(
                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                    m0.Semester == item_distinct.Semester &&
                                    m0.Unit == item_distinct.Unit &&
                                    m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                    m0.Mapel == item_distinct.Mapel &&
                                    m0.Tanggal == dt_tanggal).Count > 0)
                        {
                            d_kehadiran = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                    m0.Mapel == item_distinct.Mapel &&
                                                    m0.Tanggal == dt_tanggal &&
                                                    m0.Kehadiran == "100"
                                        ).Count;
                            d_total = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                    m0.Mapel == item_distinct.Mapel &&
                                                    m0.Tanggal == dt_tanggal
                                        ).Count;
                        }
                        if (is_hadir)
                        {
                            if (d_total > 0)
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        Math.Round((d_kehadiran / d_total) * 100, 1).ToString() + "</td>";
                            }
                            else
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        "&nbsp;</td>";
                            }
                        }
                        dt_tanggal = dt_tanggal.AddDays(1);

                        d_total_kehadiran += d_kehadiran;
                        d_total_all += d_total;
                    }

                    if (is_hadir)
                    {
                        if (d_total_all > 0)
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_total_kehadiran.ToString() + "/" + d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round((d_total_kehadiran / d_total_all) * 100, 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }
                    }

                    //keterangan ketidakhadiran
                    string s_ket_sakit = "";
                    string s_ket_izin = "";
                    string s_ket_alpa = "";
                    var lst_keterangan_kedisiplinan =
                            lst.FindAll(
                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                      m0.Semester == item_distinct.Semester &&
                                      m0.Unit == item_distinct.Unit &&
                                      m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                      m0.Mapel == item_distinct.Mapel
                                    );
                    if (lst_keterangan_kedisiplinan.Count > 0)
                    {
                        foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                        {
                            s_ket_sakit +=
                                (
                                    s_ket_sakit.Trim() != "" && item_ketidakhadiran.Is_Sakit_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Sakit_Keterangan;
                            s_ket_izin +=
                                (
                                    s_ket_izin.Trim() != "" && item_ketidakhadiran.Is_Izin_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Izin_Keterangan;
                            s_ket_alpa +=
                                (
                                    s_ket_alpa.Trim() != "" && item_ketidakhadiran.Is_Alpa_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Alpa_Keterangan;
                        }
                    }
                    if (is_sakit)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_sakit.ToString() + "</td>";
                    }
                    if (is_izin)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_izin.ToString() + "</td>";
                    }
                    if (is_alpa)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_alpa.ToString() + "</td>";
                    }

                    //kedisiplinan
                    int id = 1;
                    string s_ket_kedisiplinan = "";
                    foreach (var item_kedisiplinan in lst_kedisiplinan)
                    {
                        d_total_kehadiran = 0;
                        d_total_all = 0;

                        dt_tanggal = dari_tanggal;
                        for (int j = 1; j <= jumlah_hari; j++)
                        {
                            decimal d_kedisiplinan = 0;
                            decimal d_kehadiran = 0;
                            int i_count = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                        m0.Mapel == item_distinct.Mapel &&
                                                        m0.Tanggal == dt_tanggal
                                            ).Count;
                            d_kehadiran = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                        m0.Mapel == item_distinct.Mapel &&
                                                        m0.Tanggal == dt_tanggal &&
                                                        m0.Kehadiran == "100"
                                            ).Count;

                            if (id == 1 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                        m0.Mapel == item_distinct.Mapel &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat01 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 2 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                              m0.Mapel == item_distinct.Mapel &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat02 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 3 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                              m0.Mapel == item_distinct.Mapel &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat03 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 4 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                              m0.Mapel == item_distinct.Mapel &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat04 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 5 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                              m0.Mapel == item_distinct.Mapel &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat05 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 6 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                              m0.Mapel == item_distinct.Mapel &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat06 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 7 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                              m0.Mapel == item_distinct.Mapel &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat07 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 8 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                              m0.Mapel == item_distinct.Mapel &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat08 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 9 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                              m0.Mapel == item_distinct.Mapel &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat09 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 10 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_Mapel == item_distinct.Rel_Mapel &&
                                                              m0.Mapel == item_distinct.Mapel &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat10 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }

                            if (d_kehadiran > 0)
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        Math.Round((d_kedisiplinan / d_kehadiran) * 100, 1).ToString() + "</td>";
                            }
                            else
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        "&nbsp;</td>";
                            }

                            d_total_kehadiran += d_kedisiplinan;
                            d_total_all += d_kehadiran;
                            dt_tanggal = dt_tanggal.AddDays(1);
                        }

                        s_ket_kedisiplinan = "";
                        if (lst_keterangan_kedisiplinan.Count > 0)
                        {
                            foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                            {
                                if (id == 1)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat01_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat01_Keterangan;
                                }
                                else if (id == 2)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat02_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat02_Keterangan;
                                }
                                else if (id == 3)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat03_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat03_Keterangan;
                                }
                                else if (id == 4)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat04_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat04_Keterangan;
                                }
                                else if (id == 5)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat05_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat05_Keterangan;
                                }
                                else if (id == 6)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat06_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat06_Keterangan;
                                }
                                else if (id == 7)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat07_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat07_Keterangan;
                                }
                                else if (id == 8)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat08_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat08_Keterangan;
                                }
                                else if (id == 9)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat09_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat09_Keterangan;
                                }
                                else if (id == 10)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat10_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat10_Keterangan;
                                }
                            }
                        }

                        if (d_total_all > 0)
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_total_kehadiran.ToString() + "/" + d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round((d_total_kehadiran / d_total_all) * 100, 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }

                        html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                s_ket_kedisiplinan + "</td>";

                        id++;
                    }

                    html_content += "<tr>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.TahunAjaran + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Semester + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Unit + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Mapel + "</td>" +
                                        html_performansi_kedisiplinan_content +
                                        html_performansi_kedisiplinan_content_det +
                                        html_keterangan_kedisiplinan_content +
                                    "</tr>";
                }

                string s_html_field_tanggal = "";
                for (int i = 1; i <= (lst_kedisiplinan.Count + (is_hadir ? 1 : 0)); i++)
                {
                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        s_html_field_tanggal +=
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: center;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                "'" + dt_tanggal.ToString("dd/MM") +
                            "</td>";
                        dt_tanggal = dt_tanggal.AddDays(1);
                    }
                }

                return "<br />" +
                        "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY MATA PELAJARAN [KELAS MATPEL]<br />" +
                        "PERIODE : " +
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                            (
                                Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                : ""
                            ) +
                        GetKeterangan() +
                        "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                            "<tr>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Mata Pelajaran</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * jumlah_hari).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Persentase Performansi</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * 4).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Performansi Detail</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_sakit ? 1 : 0) + (is_izin ? 1 : 0) + (is_alpa ? 1 : 0))).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                            "</tr>" +
                            "<tr>" +
                                (
                                    is_hadir
                                    ? "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran (%)</td>"
                                    : ""
                                ) +
                                html_field_performansi +
                                (
                                    is_hadir
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran xy</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran x</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran y</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran %</td>"
                                    : ""
                                ) +
                                html_field_performansi_det +
                                (
                                    is_sakit
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>"
                                    : ""
                                ) +
                                (
                                    is_izin
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>"
                                    : ""
                                ) +
                                (
                                    is_alpa
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>"
                                    : ""
                                ) +
                                html_field_performansi_ket +
                            "</tr>" +
                            "<tr>" +
                                s_html_field_tanggal +
                            "</tr>" +
                            html_content +
                        "</table>";
            }
        }

        public static string GetHTMLReportAbsensiSiswa_GroupBy_Kelas(
                DateTime dari_tanggal,
                DateTime sampai_tanggal,
                string rel_unit,
                string rel_kelasdet,
                string rel_mapel,
                string rel_guru,
                string nama_siswa1,
                string nama_siswa2,
                bool is_hadir,
                bool is_sakit,
                bool is_izin,
                bool is_alpa,
                Dictionary<string, string> lst_kedisiplinan,
                string fp,
                string fk
            )
        {
            List<Reports.AbsensiSiswa> lst = GetHTMLReportAbsensiSiswa(
                    dari_tanggal,
                    sampai_tanggal,
                    rel_unit,
                    rel_kelasdet,
                    rel_mapel,
                    rel_guru,
                    nama_siswa1,
                    nama_siswa2,
                    is_hadir,
                    is_sakit,
                    is_izin,
                    is_alpa,
                    lst_kedisiplinan
                ).FindAll(m0 => m0.Rel_Mapel.Trim() == "")
                 .OrderBy(m0 => m0.TahunAjaran)
                 .ThenBy(m0 => m0.Semester)
                 .ThenBy(m0 => m0.UrutanJenjang)
                 .ThenBy(m0 => m0.UrutanLevel)
                 .ThenBy(m0 => m0.UrutanKelas)
                 .ThenBy(m0 => m0.Tanggal)
                 .ToList();

            string html_content = "";
            string html_field_performansi = "";
            string html_field_performansi_ket = "";

            //filter presensi dan kedisiplinan
            for (int i = lst_kedisiplinan.Count - 1; i >= 0; i--)
            {
                string key = lst_kedisiplinan.ToList()[i].Key;
                string value = lst_kedisiplinan.ToList()[i].Value;
                if (value == "0")
                {
                    lst_kedisiplinan.Remove(key);
                }
            }
            //end filter

            if (dari_tanggal == sampai_tanggal) //harian
            {
                foreach (var item in lst_kedisiplinan)
                {
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " xy</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " x</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " y</td>";
                    html_field_performansi +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " %</td>";

                    html_field_performansi_ket +=
                        "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                }

                var lst_distinct = lst.Select(m0 => new { m0.TahunAjaran, m0.Semester, m0.Unit, m0.Rel_KelasDet, m0.KelasDet, m0.Tanggal }).Distinct().ToList();
                foreach (var item_distinct in lst_distinct)
                {
                    string html_performansi_kedisiplinan_content = "";
                    string html_keterangan_kedisiplinan_content = "";

                    //kehadiran
                    decimal d_kehadiran = 0;
                    decimal d_total = 0;
                    if (lst.FindAll(
                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                m0.Semester == item_distinct.Semester &&
                                m0.Unit == item_distinct.Unit &&
                                m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                m0.KelasDet == item_distinct.KelasDet &&
                                m0.Tanggal == item_distinct.Tanggal).Count > 0)
                    {
                        d_total = lst.FindAll(
                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                m0.Semester == item_distinct.Semester &&
                                                m0.Unit == item_distinct.Unit &&
                                                m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                m0.KelasDet == item_distinct.KelasDet &&
                                                m0.Tanggal == item_distinct.Tanggal
                                    ).Count;

                        d_kehadiran = lst.FindAll(
                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                m0.Semester == item_distinct.Semester &&
                                                m0.Unit == item_distinct.Unit &&
                                                m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                m0.KelasDet == item_distinct.KelasDet &&
                                                m0.Tanggal == item_distinct.Tanggal &&
                                                m0.Kehadiran == "100"
                                    ).Count;
                    }
                    if (is_hadir)
                    {
                        if (d_total > 0)
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_kehadiran.ToString() + "/" + d_total.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round(((d_kehadiran / d_total) * 100), 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }
                    }

                    //keterangan ketidakhadiran
                    string s_ket_sakit = "";
                    string s_ket_izin = "";
                    string s_ket_alpa = "";
                    var lst_keterangan_kedisiplinan =
                            lst.FindAll(
                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                      m0.Semester == item_distinct.Semester &&
                                      m0.Unit == item_distinct.Unit &&
                                      m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                      m0.KelasDet == item_distinct.KelasDet &&
                                      m0.Tanggal == item_distinct.Tanggal
                                    );
                    if (lst_keterangan_kedisiplinan.Count > 0)
                    {
                        foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                        {
                            s_ket_sakit +=
                                (
                                    s_ket_sakit.Trim() != "" && item_ketidakhadiran.Is_Sakit_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Sakit_Keterangan;
                            s_ket_izin +=
                                (
                                    s_ket_izin.Trim() != "" && item_ketidakhadiran.Is_Izin_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Izin_Keterangan;
                            s_ket_alpa +=
                                (
                                    s_ket_alpa.Trim() != "" && item_ketidakhadiran.Is_Alpa_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Alpa_Keterangan;
                        }
                    }
                    if (is_sakit)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_sakit.ToString() + "</td>";
                    }
                    if (is_izin)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_izin.ToString() + "</td>";
                    }
                    if (is_alpa)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_alpa.ToString() + "</td>";
                    }

                    //kedisiplinan
                    int id = 1;
                    string s_ket_kedisiplinan = "";
                    foreach (var item_kedisiplinan in lst_kedisiplinan)
                    {
                        decimal d_kedisiplinan = 0;
                        int d_total_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                                m0.Semester == item_distinct.Semester &&
                                                                m0.Unit == item_distinct.Unit &&
                                                                m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                                m0.KelasDet == item_distinct.KelasDet &&
                                                                m0.Tanggal == item_distinct.Tanggal &&
                                                                m0.Kehadiran == "100"
                                                    ).Count;

                        if (id == 1 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                          m0.KelasDet == item_distinct.KelasDet &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat01 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 2 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                          m0.KelasDet == item_distinct.KelasDet &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat02 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 3 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                          m0.KelasDet == item_distinct.KelasDet &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat03 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 4 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                          m0.KelasDet == item_distinct.KelasDet &&
                                                          m0.Is_Cat04 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 5 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                          m0.KelasDet == item_distinct.KelasDet &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat05 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 6 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                          m0.KelasDet == item_distinct.KelasDet &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat06 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 7 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                          m0.KelasDet == item_distinct.KelasDet &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat07 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 8 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                          m0.KelasDet == item_distinct.KelasDet &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat08 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 9 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                          m0.KelasDet == item_distinct.KelasDet &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat09 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }
                        else if (id == 10 && d_total_kedisiplinan > 0)
                        {
                            d_kedisiplinan = lst.FindAll(
                                                    m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                          m0.Semester == item_distinct.Semester &&
                                                          m0.Unit == item_distinct.Unit &&
                                                          m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                          m0.KelasDet == item_distinct.KelasDet &&
                                                          m0.Tanggal == item_distinct.Tanggal &&
                                                          m0.Is_Cat10 == "100" &&
                                                          m0.Kehadiran == "100"
                                                ).Count;
                        }

                        if (d_total_kedisiplinan > 0)
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_kedisiplinan.ToString() + "/" + d_total_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kedisiplinan.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round(((d_kedisiplinan / d_total_kedisiplinan) * 100), 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }

                        s_ket_kedisiplinan = "";
                        if (lst_keterangan_kedisiplinan.Count > 0)
                        {
                            foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                            {
                                if (id == 1)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat01_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat01_Keterangan;
                                }
                                else if (id == 2)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat02_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat02_Keterangan;
                                }
                                else if (id == 3)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat03_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat03_Keterangan;
                                }
                                else if (id == 4)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat04_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat04_Keterangan;
                                }
                                else if (id == 5)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat05_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat05_Keterangan;
                                }
                                else if (id == 6)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat06_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat06_Keterangan;
                                }
                                else if (id == 7)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat07_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat07_Keterangan;
                                }
                                else if (id == 8)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat08_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat08_Keterangan;
                                }
                                else if (id == 9)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat09_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat09_Keterangan;
                                }
                                else if (id == 10)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat10_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat10_Keterangan;
                                }
                            }
                        }
                        html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                s_ket_kedisiplinan + "</td>";

                        id++;
                    }

                    html_content += "<tr>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.TahunAjaran + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Semester + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Unit + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.KelasDet + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">'" + item_distinct.Tanggal.ToString("dd/MM/yyyy") + "</td>" +
                                        html_performansi_kedisiplinan_content +
                                        html_keterangan_kedisiplinan_content +
                                    "</tr>";
                }

                return "<br />" +
                    "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY KELAS [KELAS PERWALIAN]<br />" +
                    "PERIODE : " +
                        Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                        (
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                            ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                            : ""
                        ) +
                    GetKeterangan() +
                    "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kelas</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tanggal</td>" +
                            "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * 4).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Persentase Performansi</td>" +
                            "<td colspan=\"" + (lst_kedisiplinan.Count + (is_sakit ? 1 : 0) + (is_izin ? 1 : 0) + (is_alpa ? 1 : 0)).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                        "</tr>" +
                        "<tr>" +
                            (
                                is_hadir
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran xy</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran x</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran y</td>" +
                                  "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran %</td>"
                                : ""
                            ) +
                            html_field_performansi +
                            (
                                is_sakit
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>"
                                : ""
                            ) +
                            (
                                is_izin
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>"
                                : ""
                            ) +
                            (
                                is_alpa
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>"
                                : ""
                            ) +
                            html_field_performansi_ket +
                        "</tr>" +
                        html_content +
                    "</table>";
            }
            else
            {
                string html_field_performansi_det = "";
                int jumlah_hari = Math.Abs((dari_tanggal - sampai_tanggal).Days) + 1;
                DateTime dt_tanggal = dari_tanggal;

                foreach (var item in lst_kedisiplinan)
                {
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " xy</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " x</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " y</td>";
                    html_field_performansi_det +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " %</td>";
                    
                    html_field_performansi +=
                        "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " (%)</td>";
                    html_field_performansi_ket +=
                        "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
                }

                var lst_distinct = lst.Select(m0 => new { m0.TahunAjaran, m0.Semester, m0.Unit, m0.Rel_KelasDet, m0.KelasDet }).Distinct().ToList().FindAll(m0 => m0.Rel_KelasDet.Trim() != "");
                foreach (var item_distinct in lst_distinct)
                {
                    string html_performansi_kedisiplinan_content = "";
                    string html_performansi_kedisiplinan_content_det = "";
                    string html_keterangan_kedisiplinan_content = "";

                    //kehadiran
                    decimal d_total_kehadiran = 0;
                    decimal d_total_all = 0;

                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        decimal d_kehadiran = 0;
                        decimal d_total = 0;
                        if (lst.FindAll(
                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                    m0.Semester == item_distinct.Semester &&
                                    m0.Unit == item_distinct.Unit &&
                                    m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                    m0.KelasDet == item_distinct.KelasDet &&
                                    m0.Tanggal == dt_tanggal).Count > 0)
                        {
                            d_kehadiran = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                    m0.KelasDet == item_distinct.KelasDet &&
                                                    m0.Tanggal == dt_tanggal &&
                                                    m0.Kehadiran == "100"
                                        ).Count;
                            d_total = lst.FindAll(
                                            m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                    m0.Semester == item_distinct.Semester &&
                                                    m0.Unit == item_distinct.Unit &&
                                                    m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                    m0.KelasDet == item_distinct.KelasDet &&
                                                    m0.Tanggal == dt_tanggal
                                        ).Count;
                        }
                        if (is_hadir)
                        {
                            if (d_total > 0)
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        Math.Round((d_kehadiran / d_total) * 100, 1).ToString() + "</td>";
                            }
                            else
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        "&nbsp;</td>";
                            }
                        }
                        dt_tanggal = dt_tanggal.AddDays(1);

                        d_total_kehadiran += d_kehadiran;
                        d_total_all += d_total;
                    }

                    if (is_hadir)
                    {
                        if (d_total_all > 0)
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_total_kehadiran.ToString() + "/" + d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round((d_total_kehadiran / d_total_all) * 100, 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }
                    }

                    //keterangan ketidakhadiran
                    string s_ket_sakit = "";
                    string s_ket_izin = "";
                    string s_ket_alpa = "";
                    var lst_keterangan_kedisiplinan =
                            lst.FindAll(
                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                      m0.Semester == item_distinct.Semester &&
                                      m0.Unit == item_distinct.Unit &&
                                      m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                      m0.KelasDet == item_distinct.KelasDet
                                    );
                    if (lst_keterangan_kedisiplinan.Count > 0)
                    {
                        foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                        {
                            s_ket_sakit +=
                                (
                                    s_ket_sakit.Trim() != "" && item_ketidakhadiran.Is_Sakit_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Sakit_Keterangan;
                            s_ket_izin +=
                                (
                                    s_ket_izin.Trim() != "" && item_ketidakhadiran.Is_Izin_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Izin_Keterangan;
                            s_ket_alpa +=
                                (
                                    s_ket_alpa.Trim() != "" && item_ketidakhadiran.Is_Alpa_Keterangan.Trim() != ""
                                    ? " | "
                                    : ""
                                ) + item_ketidakhadiran.Is_Alpa_Keterangan;
                        }
                    }
                    if (is_sakit)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_sakit.ToString() + "</td>";
                    }
                    if (is_izin)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_izin.ToString() + "</td>";
                    }
                    if (is_alpa)
                    {
                        html_keterangan_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                        s_ket_alpa.ToString() + "</td>";
                    }

                    //kedisiplinan
                    int id = 1;
                    string s_ket_kedisiplinan = "";
                    foreach (var item_kedisiplinan in lst_kedisiplinan)
                    {
                        d_total_kehadiran = 0;
                        d_total_all = 0;

                        dt_tanggal = dari_tanggal;
                        for (int j = 1; j <= jumlah_hari; j++)
                        {
                            decimal d_kedisiplinan = 0;
                            decimal d_kehadiran = 0;
                            int i_count = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                        m0.KelasDet == item_distinct.KelasDet &&
                                                        m0.Tanggal == dt_tanggal
                                            ).Count;
                            d_kehadiran = lst.FindAll(
                                                m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                        m0.Semester == item_distinct.Semester &&
                                                        m0.Unit == item_distinct.Unit &&
                                                        m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                        m0.KelasDet == item_distinct.KelasDet &&
                                                        m0.Tanggal == dt_tanggal &&
                                                        m0.Kehadiran == "100"
                                            ).Count;

                            if (id == 1 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat01 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 2 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat02 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 3 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat03 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 4 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat04 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 5 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat05 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 6 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat06 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 7 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat07 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 8 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat08 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 9 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat09 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }
                            else if (id == 10 && i_count > 0)
                            {
                                d_kedisiplinan = lst.FindAll(
                                                        m0 => m0.TahunAjaran == item_distinct.TahunAjaran &&
                                                              m0.Semester == item_distinct.Semester &&
                                                              m0.Unit == item_distinct.Unit &&
                                                              m0.Rel_KelasDet == item_distinct.Rel_KelasDet &&
                                                              m0.KelasDet == item_distinct.KelasDet &&
                                                              m0.Tanggal == dt_tanggal &&
                                                              m0.Is_Cat10 == "100" &&
                                                              m0.Kehadiran == "100"
                                                    ).Count;
                            }

                            if (d_kehadiran > 0)
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        Math.Round((d_kedisiplinan / d_kehadiran) * 100, 1).ToString() + "</td>";
                            }
                            else
                            {
                                html_performansi_kedisiplinan_content +=
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                        "&nbsp;</td>";
                            }

                            d_total_kehadiran += d_kedisiplinan;
                            d_total_all += d_kehadiran;
                            dt_tanggal = dt_tanggal.AddDays(1);
                        }

                        s_ket_kedisiplinan = "";
                        if (lst_keterangan_kedisiplinan.Count > 0)
                        {
                            foreach (var item_ketidakhadiran in lst_keterangan_kedisiplinan)
                            {
                                if (id == 1)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat01_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat01_Keterangan;
                                }
                                else if (id == 2)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat02_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat02_Keterangan;
                                }
                                else if (id == 3)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat03_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat03_Keterangan;
                                }
                                else if (id == 4)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat04_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat04_Keterangan;
                                }
                                else if (id == 5)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat05_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat05_Keterangan;
                                }
                                else if (id == 6)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat06_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat06_Keterangan;
                                }
                                else if (id == 7)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat07_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat07_Keterangan;
                                }
                                else if (id == 8)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat08_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat08_Keterangan;
                                }
                                else if (id == 9)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat09_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat09_Keterangan;
                                }
                                else if (id == 10)
                                {
                                    s_ket_kedisiplinan +=
                                        (
                                            s_ket_kedisiplinan.Trim() != "" && item_ketidakhadiran.Is_Cat10_Keterangan.Trim() != ""
                                            ? " | "
                                            : ""
                                        ) + item_ketidakhadiran.Is_Cat10_Keterangan;
                                }
                            }
                        }

                        if (d_total_all > 0)
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "'" + d_total_kehadiran.ToString() + "/" + d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_kehadiran.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    d_total_all.ToString() + "</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    Math.Round((d_total_kehadiran / d_total_all) * 100, 1).ToString() + "%</td>";
                        }
                        else
                        {
                            html_performansi_kedisiplinan_content_det +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>" +
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                    "&nbsp;</td>";
                        }

                        html_keterangan_kedisiplinan_content +=
                                "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                                s_ket_kedisiplinan + "</td>";

                        id++;
                    }

                    html_content += "<tr>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.TahunAjaran + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Semester + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.Unit + "</td>" +
                                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item_distinct.KelasDet + "</td>" +
                                        html_performansi_kedisiplinan_content +
                                        html_performansi_kedisiplinan_content_det +
                                        html_keterangan_kedisiplinan_content +
                                    "</tr>";
                }

                string s_html_field_tanggal = "";
                for (int i = 1; i <= (lst_kedisiplinan.Count + (is_hadir ? 1 : 0)); i++)
                {
                    dt_tanggal = dari_tanggal;
                    for (int j = 1; j <= jumlah_hari; j++)
                    {
                        s_html_field_tanggal +=
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: center;" + (Libs.GetIsHariLibur(dt_tanggal) ? "background-color: red;" : "") + "\">" +
                                "'" + dt_tanggal.ToString("dd/MM") +
                            "</td>";
                        dt_tanggal = dt_tanggal.AddDays(1);
                    }
                }

                return "<br />" +
                        "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY [KELAS PERWALIAN]<br />" +
                        "PERIODE : " +
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                            (
                                Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                : ""
                            ) +
                        GetKeterangan() +
                        "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                            "<tr>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                                "<td rowspan=\"3\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Mata Pelajaran</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * jumlah_hari).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Persentase Performansi</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_hadir ? 1 : 0)) * 4).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Performansi Detail</td>" +
                                "<td colspan=\"" + ((lst_kedisiplinan.Count + (is_sakit ? 1 : 0) + (is_izin ? 1 : 0) + (is_alpa ? 1 : 0))).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                            "</tr>" +
                            "<tr>" +
                                (
                                    is_hadir
                                    ? "<td colspan=\"" + jumlah_hari.ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran (%)</td>"
                                    : ""
                                ) +
                                html_field_performansi +
                                (
                                    is_hadir
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran xy</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran x</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran y</td>" +
                                      "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran %</td>"
                                    : ""
                                ) +
                                html_field_performansi_det +
                                (
                                    is_sakit
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>"
                                    : ""
                                ) +
                                (
                                    is_izin
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>"
                                    : ""
                                ) +
                                (
                                    is_alpa
                                    ? "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>"
                                    : ""
                                ) +
                                html_field_performansi_ket +
                            "</tr>" +
                            "<tr>" +
                                s_html_field_tanggal +
                            "</tr>" +
                            html_content +
                        "</table>";
            }
        }

        public static string GetHTMLReportAbsensiSiswa_GroupBy_MapelSiswa(
                DateTime dari_tanggal,
                DateTime sampai_tanggal,
                string rel_unit,
                string rel_kelasdet,
                string rel_mapel,
                string rel_guru,
                string nama_siswa1,
                string nama_siswa2,
                bool is_hadir,
                bool is_sakit,
                bool is_izin,
                bool is_alpa,
                Dictionary<string, string> lst_kedisiplinan,
                string fp,
                string fk
            )
        {
            List<Reports.AbsensiSiswa> lst = GetHTMLReportAbsensiSiswa(
                    dari_tanggal,
                    sampai_tanggal,
                    rel_unit,
                    rel_kelasdet,
                    rel_mapel,
                    rel_guru,
                    nama_siswa1,
                    nama_siswa2,
                    is_hadir,
                    is_sakit,
                    is_izin,
                    is_alpa,
                    lst_kedisiplinan
                )//.FindAll(m0 => m0.Rel_Mapel.Trim() != ""
                //)
                 .OrderBy(m0 => m0.TahunAjaran)
                 .ThenBy(m0 => m0.Semester)
                 .ThenBy(m0 => m0.UrutanJenjang)
                 .ThenBy(m0 => m0.UrutanLevel)
                 .ThenBy(m0 => m0.UrutanKelas)
                 .ThenBy(m0 => m0.Tanggal)
                 .ThenBy(m0 => m0.Guru)
                 .ThenBy(m0 => m0.Mapel)
                 .ThenBy(m0 => m0.Nama)
                 .ToList();

            string html_content = "";
            string html_field_performansi = "";
            string html_field_performansi_ket = "";

            //filter presensi dan kedisiplinan
            for (int i = lst_kedisiplinan.Count - 1; i >= 0; i--)
            {
                string key = lst_kedisiplinan.ToList()[i].Key;
                string value = lst_kedisiplinan.ToList()[i].Value;
                if (value == "0")
                {
                    lst_kedisiplinan.Remove(key);
                }
            }
            //end filter

            foreach (var item in lst_kedisiplinan)
            {
                html_field_performansi +=
                    "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " (%)</td>";
                html_field_performansi_ket +=
                    "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
            }

            foreach (var item in lst)
            {
                string html_performansi_kedisiplinan_content = "";
                string html_keterangan_kedisiplinan_content = "";
                int id = 1;
                foreach (var item_kedisiplinan in lst_kedisiplinan)
                {
                    string s_performansi = "";
                    string s_keterangan = "";
                    if (id == 1)
                    {
                        s_performansi = item.Is_Cat01;
                        s_keterangan = item.Is_Cat01_Keterangan;
                    }
                    else if (id == 2)
                    {
                        s_performansi = item.Is_Cat02;
                        s_keterangan = item.Is_Cat02_Keterangan;
                    }
                    else if (id == 3)
                    {
                        s_performansi = item.Is_Cat03;
                        s_keterangan = item.Is_Cat03_Keterangan;
                    }
                    else if (id == 4)
                    {
                        s_performansi = item.Is_Cat04;
                        s_keterangan = item.Is_Cat04_Keterangan;
                    }
                    else if (id == 5)
                    {
                        s_performansi = item.Is_Cat05;
                        s_keterangan = item.Is_Cat05_Keterangan;
                    }
                    else if (id == 6)
                    {
                        s_performansi = item.Is_Cat06;
                        s_keterangan = item.Is_Cat06_Keterangan;
                    }
                    else if (id == 7)
                    {
                        s_performansi = item.Is_Cat07;
                        s_keterangan = item.Is_Cat07_Keterangan;
                    }
                    else if (id == 8)
                    {
                        s_performansi = item.Is_Cat08;
                        s_keterangan = item.Is_Cat08_Keterangan;
                    }
                    else if (id == 9)
                    {
                        s_performansi = item.Is_Cat09;
                        s_keterangan = item.Is_Cat09_Keterangan;
                    }
                    else if (id == 10)
                    {
                        s_performansi = item.Is_Cat10;
                        s_keterangan = item.Is_Cat10_Keterangan;
                    }

                    if (item.Kehadiran == "100")
                    {
                        html_performansi_kedisiplinan_content +=
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                s_performansi + "</td>";
                    }
                    else
                    {
                        html_performansi_kedisiplinan_content +=
                            "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                                "0</td>";
                    }

                    html_keterangan_kedisiplinan_content +=
                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                            s_keterangan + "</td>";
                    id++;
                }

                html_content += "<tr>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.TahunAjaran + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Semester + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Unit + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Rel_Guru + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Guru + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Mapel + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.KelasDet + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.NIS + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + Libs.GetPerbaikiEjaanNama(item.Nama) + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">'" + item.Tanggal.ToString("dd/MM/yyyy") + "</td>" +
                                    (
                                        is_hadir
                                        ? "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + item.Kehadiran + "</td>"
                                        : ""
                                    ) +
                                    html_performansi_kedisiplinan_content +
                                    (
                                        is_sakit
                                        ? "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Is_Sakit_Keterangan + "</td>"
                                        : ""
                                    ) +
                                    (
                                        is_izin
                                        ? "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Is_Izin_Keterangan + "</td>"
                                        : ""
                                    ) +
                                    (
                                        is_alpa
                                        ? "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Is_Alpa_Keterangan + "</td>"
                                        : ""
                                    ) +
                                    html_keterangan_kedisiplinan_content +
                                "</tr>";
            }

            return "<br />" +
                    "REKAP PRESENSI & KEDISIPLINAN MURID - GROUP BY MATA PELAJARAN & MURID [KELAS MATPEL]<br />" +
                    "PERIODE : " +
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                            (
                                Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                : ""
                            ) +
                    GetKeterangan() +
                    "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">NIK</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Guru</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Mata Pelajaran</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kelas</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">NIS</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Nama</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tanggal Presensi</td>" +
                            "<td colspan=\"" + (lst_kedisiplinan.Count + (is_hadir ? 1 : 0)).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Performansi</td>" +
                            "<td colspan=\"" + (lst_kedisiplinan.Count + (is_sakit ? 1 : 0) + (is_izin ? 1 : 0) + (is_alpa ? 1 : 0)).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                        "</tr>" +
                        "<tr>" +
                            (
                                is_hadir
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran (%)</td>"
                                : ""
                            ) +
                            html_field_performansi +
                            (
                                is_sakit
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>"
                                : ""
                            ) +
                            (
                                is_izin
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>"
                                : ""
                            ) +
                            (
                                is_alpa
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>"
                                : ""
                            ) +
                            html_field_performansi_ket +
                        "</tr>" +
                        html_content +
                    "</table>";
        }

        public static string GetHTMLReportAbsensiSiswaDetail_GroupBy_AbsensiSiswa(
                DateTime dari_tanggal,
                DateTime sampai_tanggal,
                string rel_unit,
                string rel_kelasdet,
                string rel_mapel,
                string rel_guru,
                string nama_siswa1,
                string nama_siswa2,
                bool is_hadir,
                bool is_sakit,
                bool is_izin,
                bool is_alpa,
                Dictionary<string, string> lst_kedisiplinan,
                string fp,
                string fk
            )
        {
            List<Reports.AbsensiSiswa> lst = GetHTMLReportAbsensiSiswa(
                    dari_tanggal,
                    sampai_tanggal,
                    rel_unit,
                    rel_kelasdet,
                    rel_mapel,
                    rel_guru,
                    nama_siswa1,
                    nama_siswa2,
                    is_hadir,
                    is_sakit,
                    is_izin,
                    is_alpa,
                    lst_kedisiplinan
                ).FindAll(m0 => m0.Rel_Mapel.Trim() == "" &&
                                (
                                    m0.Kehadiran == "0" ||
                                    (
                                        m0.Kehadiran == "100" &&
                                        (
                                            m0.Is_Cat01 == "0" ||
                                            m0.Is_Cat02 == "0" ||
                                            m0.Is_Cat03 == "0"
                                        )
                                    )
                                )
                ).OrderBy(m0 => m0.TahunAjaran)
                 .ThenBy(m0 => m0.Semester)
                 .ThenBy(m0 => m0.UrutanJenjang)
                 .ThenBy(m0 => m0.UrutanLevel)
                 .ThenBy(m0 => m0.UrutanKelas)
                 .ThenBy(m0 => m0.Tanggal)
                 .ThenBy(m0 => m0.Guru)
                 .ThenBy(m0 => m0.Mapel)
                 .ThenBy(m0 => m0.Nama)
                 .ToList();

            string html_content = "";
            string html_field_performansi = "";
            string html_field_performansi_ket = "";

            //filter presensi dan kedisiplinan
            for (int i = lst_kedisiplinan.Count - 1; i >= 0; i--)
            {
                string key = lst_kedisiplinan.ToList()[i].Key;
                string value = lst_kedisiplinan.ToList()[i].Value;
                if (value == "0")
                {
                    lst_kedisiplinan.Remove(key);
                }
            }
            //end filter

            foreach (var item in lst_kedisiplinan)
            {
                html_field_performansi +=
                    "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">" + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + " (%)</td>";
                html_field_performansi_ket +=
                    "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. " + (item.Key.Length > 2 ? item.Key.Substring(2) : item.Key) + "</td>";
            }

            foreach (var item in lst)
            {
                string html_performansi_kedisiplinan_content = "";
                string html_keterangan_kedisiplinan_content = "";
                int id = 1;
                foreach (var item_kedisiplinan in lst_kedisiplinan)
                {
                    string s_performansi = "";
                    string s_keterangan = "";
                    if (id == 1)
                    {
                        s_performansi = item.Is_Cat01;
                        s_keterangan = item.Is_Cat01_Keterangan;
                    }
                    else if (id == 2)
                    {
                        s_performansi = item.Is_Cat02;
                        s_keterangan = item.Is_Cat02_Keterangan;
                    }
                    else if (id == 3)
                    {
                        s_performansi = item.Is_Cat03;
                        s_keterangan = item.Is_Cat03_Keterangan;
                    }
                    else if (id == 4)
                    {
                        s_performansi = item.Is_Cat04;
                        s_keterangan = item.Is_Cat04_Keterangan;
                    }
                    else if (id == 5)
                    {
                        s_performansi = item.Is_Cat05;
                        s_keterangan = item.Is_Cat05_Keterangan;
                    }
                    else if (id == 6)
                    {
                        s_performansi = item.Is_Cat06;
                        s_keterangan = item.Is_Cat06_Keterangan;
                    }
                    else if (id == 7)
                    {
                        s_performansi = item.Is_Cat07;
                        s_keterangan = item.Is_Cat07_Keterangan;
                    }
                    else if (id == 8)
                    {
                        s_performansi = item.Is_Cat08;
                        s_keterangan = item.Is_Cat08_Keterangan;
                    }
                    else if (id == 9)
                    {
                        s_performansi = item.Is_Cat09;
                        s_keterangan = item.Is_Cat09_Keterangan;
                    }
                    else if (id == 10)
                    {
                        s_performansi = item.Is_Cat10;
                        s_keterangan = item.Is_Cat10_Keterangan;
                    }

                    if (item.Kehadiran == "0") s_performansi = "0";

                    html_performansi_kedisiplinan_content +=
                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" +
                            s_performansi + "</td>";
                    html_keterangan_kedisiplinan_content +=
                        "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: left;\">" +
                            s_keterangan + "</td>";
                    id++;
                }

                html_content += "<tr>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.TahunAjaran + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Semester + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Unit + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Rel_Guru + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Guru + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.KelasDet + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.NIS + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + Libs.GetPerbaikiEjaanNama(item.Nama) + "</td>" +
                                    "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">'" + item.Tanggal.ToString("dd/MM/yyyy") + "</td>" +
                                    (
                                        is_hadir
                                        ? "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black; text-align: right;\">" + item.Kehadiran + "</td>"
                                        : ""
                                    ) +
                                    html_performansi_kedisiplinan_content +
                                    (
                                        is_sakit
                                        ? "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Is_Sakit_Keterangan + "</td>"
                                        : ""
                                    ) +
                                    (
                                        is_izin
                                        ? "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Is_Izin_Keterangan + "</td>"
                                        : ""
                                    ) +
                                    (
                                        is_alpa
                                        ? "<td style=\"vertical-align: top; border-style: solid; border-width: 1px; border-color: black;\">" + item.Is_Alpa_Keterangan + "</td>"
                                        : ""
                                    ) +
                                    html_keterangan_kedisiplinan_content +
                                "</tr>";
            }

            return "<br />" +
                    "REKAP ABSENSI & KEDISIPLINAN MURID - GROUP BY MURID [KELAS PERWALIAN]<br />" +
                    "PERIODE : " +
                            Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false) +
                            (
                                Libs.GetTanggalIndonesiaFromDate(dari_tanggal, false).Trim() != Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                ? " s.d " + Libs.GetTanggalIndonesiaFromDate(sampai_tanggal, false)
                                : ""
                            ) +
                    GetKeterangan() +
                    "<table style=\"vertical-align: top; border-style: solid; border-collapse: collapse; border-width: 1px;\">" +
                        "<tr>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tahun Pelajaran</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Semester</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Unit</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">NIK</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Guru</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kelas</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">NIS</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Nama</td>" +
                            "<td rowspan=\"2\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Tanggal Presensi</td>" +
                            "<td colspan=\"" + (lst_kedisiplinan.Count + (is_hadir ? 1 : 0)).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Performansi</td>" +
                            "<td colspan=\"" + (lst_kedisiplinan.Count + (is_sakit ? 1 : 0) + (is_izin ? 1 : 0) + (is_alpa ? 1 : 0)).ToString() + "\" style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Keterangan</td>" +
                        "</tr>" +
                        "<tr>" +
                            (
                                is_hadir
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Kehadiran (%)</td>"
                                : ""
                            ) +
                            html_field_performansi +
                            (
                                is_sakit
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Sakit</td>"
                                : ""
                            ) +
                            (
                                is_izin
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Izin</td>"
                                : ""
                            ) +
                            (
                                is_alpa
                                ? "<td style=\"vertical-align: middle; border-style: solid; border-width: 1px; border-color: black; text-align: center;\">Ket. Alpa</td>"
                                : ""
                            ) +
                            html_field_performansi_ket +
                        "</tr>" +
                        html_content +
                    "</table>";
        }
    }
}