using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA
{
    public partial class wf_RumahSoalSMA : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATASTRUKTURPENILAIAN_SMA";
        public static List<Rapor_NilaiSiswa_KURTILAS_Det_Lengkap> lst_nilai_kurtilas = new List<Rapor_NilaiSiswa_KURTILAS_Det_Lengkap>();
        public static List<Rapor_NilaiSiswa_KTSP_Det_Lengkap> lst_nilai_ktsp = new List<Rapor_NilaiSiswa_KTSP_Det_Lengkap>();

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            AddAPWithMessage,
            AddKDWithMessage,
            AddKDKurtilasWithMessage,
            AddKPWithMessage,
            AddKPWithMessageKURTILAS,
            Edit,
            ShowDataList,
            Update,
            Delete,
            Search,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoChangePage,
            DoShowBukaSemester,
            DoShowLihatData,
            DoShowInputDeskripsiLTSRapor,
            DoShowInputDeskripsiPerMapel,
            DoShowInputAspekPenilaian,
            DoShowInputKompetensiDasar,
            DoShowInputKompetensiDasarKurtilas,
            DoShowInputKompetensiDasarKurtilasSikap,
            DoShowInputKomponenPenilaian,
            DoShowInputKomponenPenilaianKURTILAS,
            DoShowInputPredikat,
            DoShowConfirmHapus,
            DoShowInfoAdaStrukturNilai,
            DoShowStrukturNilai,
            DoShowPreviewNilai
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/browser-2.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Struktur Penilaian";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {


                string tahun_ajaran = "";
                string semester = "";


                string periode = Libs.GetQueryString("p");
                periode = periode.Replace("-", "/");
                if (periode.Trim() != "")
                {
                    if (periode.Length > 9)
                    {
                        tahun_ajaran = periode.Substring(0, 9);
                        semester = periode.Substring(periode.Length - 1, 1);
                    }
                }

             
                //lst_nilai_kurtilas = DAO_Rapor_NilaiSiswa_KURTILAS_Det.GetAllByTABySM_Entity(tahun_ajaran, semester);
                //lst_nilai_ktsp = DAO_Rapor_NilaiSiswa_KTSP_Det.GetAllByTABySM_Entity(tahun_ajaran, semester);
                BindListView(true, Libs.GetQ());
            }

            switch (mvMain.ActiveViewIndex)
            {
                case 0:
                    this.Master.ShowHeaderTools = true;
                    break;
                case 1:
                    ShowStrukturNilaiKTSP(txtID.Value);
                    this.Master.ShowHeaderTools = false;
                    break;
                case 2:
                    ShowStrukturNilaiKURTILAS(txtID.Value);
                    this.Master.ShowHeaderTools = false;
                    break;
            }

            if (!IsPostBack) this.Master.txtCariData.Text = Libs.GetQ();
        }


        public Sekolah GetUnit()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(
                m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMA).FirstOrDefault();
        }



        private void BindListView(bool isbind = true, string keyword = "")
        {



            string unit = Libs.GetQueryString("u");

            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectParameters.Add("TahunAjaran", Libs.GetTahunAjaranNow());
                sql_ds.SelectParameters.Add("Rel_Mapel", Libs.GetQueryString("m"));
                if (unit == "SMA")
                {
                    sql_ds.SelectCommand = DAO_CBT_RumahSoalSMA.SP_SMA_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_MAPEL_FOR_SEARCH;
                }
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("TahunAjaran", Libs.GetTahunAjaranNow());
                //sql_ds.SelectParameters.Add("Semester", semester);
                sql_ds.SelectParameters.Add("Rel_Mapel", Libs.GetQueryString("m"));

                sql_ds.SelectCommand = DAO_CBT_RumahSoalSMA.SP_SMA_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_MAPEL;
            }
            if (isbind) lvData.DataBind();

        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            System.Web.UI.WebControls.Literal imgh_kelas = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kelas");
            System.Web.UI.WebControls.Literal imgh_mapel = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_mapel");
            System.Web.UI.WebControls.Literal imgh_kurikulum = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kurikulum");
            System.Web.UI.WebControls.Literal imgh_kkm = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kkm");

            string html_image = "";
            if (e.SortDirection == SortDirection.Ascending)
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-up\" style=\"color: white;\"></i>";
            }
            else
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-down\" style=\"color: white;\"></i>";
            }

            imgh_tahunajaran.Text = html_image;
            imgh_kelas.Text = html_image;
            imgh_mapel.Text = html_image;
            imgh_kurikulum.Text = html_image;
            imgh_kkm.Text = html_image;

            imgh_tahunajaran.Visible = false;
            imgh_kelas.Visible = false;
            imgh_mapel.Visible = false;
            imgh_kurikulum.Visible = false;
            imgh_kkm.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
                    break;
                case "Kelas":
                    imgh_kelas.Visible = true;
                    break;
                case "Mapel":
                    imgh_mapel.Visible = true;
                    break;
                case "Kurikulum":
                    imgh_kurikulum.Visible = true;
                    break;
                case "KKM":
                    imgh_kkm.Visible = true;
                    break;
            }

            int pageindex = int.Parse(Math.Ceiling(Convert.ToDecimal(dpData.StartRowIndex / 20)).ToString());
            pageindex--;
            this.Session[SessionViewDataName] = (pageindex < 0 ? 0 : pageindex);
        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName] = e.StartRowIndex;
            txtKeyAction.Value = JenisAction.DoChangePage.ToString();
            BindListView(true, Libs.GetQ());
        }


        protected void btnShowDataListFromKTSPDet_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
            BindListView(true, Libs.GetQ());
            this.Master.ShowHeaderTools = true;
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

        protected void btnShowStrukturNilai_Click(object sender, EventArgs e)
        {
            ltrCaptionKTSPDet.Text = "";

            var m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
            if (m_sn != null)
            {
                if (m_sn.Kurikulum != null)
                {

                    if (m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KTSP)
                    {

                        Rapor_StrukturNilai_KTSP m = DAO_Rapor_StrukturNilai_KTSP.GetByID_Entity(txtID.Value);
                        if (m != null)
                        {
                            if (m.TahunAjaran != null)
                            {
                                Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());
                                string kelas = "";
                                if (m.Rel_Kelas.Trim() == "")
                                {
                                    kelas = "(Semua)";
                                }
                                else
                                {
                                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas.ToString());
                                    if (m_kelas != null)
                                    {
                                        if (m_mapel.Nama != null && m_kelas.Nama != null)
                                        {
                                            kelas = m_kelas.Nama;
                                        }
                                    }
                                }

                                if (m_mapel != null)
                                {
                                    if (m_mapel.Nama != null)
                                    {
                                        ltrCaptionKTSPDet.Text =
                                                         "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                             "&nbsp;" +
                                                             m.TahunAjaran +
                                                             "&nbsp;" +
                                                         "</span>" +

                                                         "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                                         "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                             "&nbsp;" +
                                                             "Sm." + m.Semester +
                                                             "&nbsp;" +
                                                         "</span>" +

                                                         "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                                         "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                             "&nbsp;" +
                                                             m_mapel.Nama +
                                                             "&nbsp;" +
                                                         "</span>" +

                                                         "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                                         "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                             "&nbsp;" +
                                                             "Kelas&nbsp;" +
                                                             kelas +
                                                             "&nbsp;" +
                                                         "</span>" +

                                                         "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                                         "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                             "&nbsp;" +
                                                             "KKM : &nbsp;" +
                                                             Math.Round(m.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString() +
                                                             "&nbsp;" +
                                                         "</span>";

                                        ShowStrukturNilaiKTSP(txtID.Value);
                                        this.Master.ShowHeaderTools = false;
                                        mvMain.ActiveViewIndex = 1;
                                        txtKeyAction.Value = JenisAction.DoShowStrukturNilai.ToString();
                                    }
                                }
                            }
                        }

                    }
                    else if (m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS)
                    {

                        Rapor_StrukturNilai_KURTILAS m = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(txtID.Value);
                        if (m != null)
                        {
                            if (m.TahunAjaran != null)
                            {
                                Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());
                                string kelas = "";
                                if (m.Rel_Kelas.Trim() == "")
                                {
                                    kelas = "(Semua)";
                                }
                                else
                                {
                                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas.ToString());
                                    if (m_kelas != null)
                                    {
                                        if (m_mapel.Nama != null && m_kelas.Nama != null)
                                        {
                                            kelas = m_kelas.Nama;
                                        }
                                    }
                                }
                                if (m_mapel != null)
                                {
                                    if (m_mapel.Nama != null)
                                    {
                                        ltrCaptionKURTILASDet.Text =
                                                         "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                             "&nbsp;" +
                                                             m.TahunAjaran +
                                                             "&nbsp;" +
                                                         "</span>" +

                                                         "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                                         "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                             "&nbsp;" +
                                                             "Sm." + m.Semester +
                                                             "&nbsp;" +
                                                         "</span>" +

                                                         "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                                         "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                             "&nbsp;" +
                                                             m_mapel.Nama +
                                                             "&nbsp;" +
                                                         "</span>" +

                                                         "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                                         "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                             "&nbsp;" +
                                                             "Kelas&nbsp;" +
                                                             kelas +
                                                             "&nbsp;" +
                                                         "</span>" +

                                                         "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                                         "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                             "&nbsp;" +
                                                             "KKM : &nbsp;" +
                                                             Math.Round(m.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString() +
                                                             "&nbsp;" +
                                                         "</span>";

                                        ShowStrukturNilaiKURTILAS(txtID.Value);
                                        this.Master.ShowHeaderTools = false;
                                        mvMain.ActiveViewIndex = 2;
                                        txtKeyAction.Value = JenisAction.DoShowStrukturNilai.ToString();
                                    }
                                }
                            }
                        }

                    }

                }
            }
        }

        public static string GetJSPreviewNilaiEkskul(Page page, string kode_sn, string tahun_ajaran, string semester, string rel_kelas, string rel_kelas_det, string rel_mapel)
        {
            Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
            if (m_mapel != null)
            {
                if (m_mapel.Nama != null)
                {
                    string url = "";
                    string s_url = "";
                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                    {
                        var m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(kode_sn);
                        if (m_sn != null)
                        {
                            if (m_sn.Kurikulum != null)
                            {
                                if (m_sn.IsNilaiAkhir)
                                {
                                    url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_EKSKUL.ROUTE;
                                }
                                else
                                {
                                    url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.ROUTE;
                                }

                                s_url = "window.open(" +
                                            "'" +
                                                page.ResolveUrl(
                                                    url +
                                                    "?token=" + Constantas.SMA.TOKEN_NILAI_EKSKUL +
                                                    (tahun_ajaran.Trim() != "" ? "&t=" : "") + RandomLibs.GetRndTahunAjaran(tahun_ajaran) +
                                                    (semester.Trim() != "" ? "&s=" : "") + semester +
                                                    (rel_kelas.Trim() != "" ? "&k=" : "") + rel_kelas +
                                                    (rel_kelas_det.Trim() != "" ? "&kd=" : "") + rel_kelas_det +
                                                    (rel_mapel.Trim() != "" ? "&m=" : "") + rel_mapel
                                                ) +
                                            "', " +
                                            "'_blank' " +
                                        "); " +
                                        "return false; ";
                            }
                        }
                    }

                    return s_url;
                }
            }

            return "#";
        }

        public static string GetJSPreviewNilai(Page page, string tahun_ajaran, string semester, string rel_kelas, string rel_kelas_det, string rel_mapel)
        {
            string url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.ROUTE;
            string s_url = "window.open(" +
                                "'" +
                                    page.ResolveUrl(
                                        url +
                                        "?token=" + Constantas.SMA.TOKEN_PREVIEW_NILAI +
                                        (tahun_ajaran.Trim() != "" ? "&t=" : "") + RandomLibs.GetRndTahunAjaran(tahun_ajaran) +
                                        (semester.Trim() != "" ? "&s=" : "") + semester +
                                        (rel_kelas.Trim() != "" ? "&k=" : "") + rel_kelas +
                                        (rel_kelas_det.Trim() != "" ? "&kd=" : "") + rel_kelas_det +
                                        (rel_mapel.Trim() != "" ? "&m=" : "") + rel_mapel
                                    ) +
                                "', " +
                                "'_blank' " +
                           "); " +
                           "return false; ";

            return s_url;
        }

        protected void InitLinkPreviewNilai(string tahun_ajaran, string semester, string rel_kelas, string rel_mapel)
        {
            //lnkOKPreviewNilaiKTSP.Attributes["onclick"] = GetJSPreviewNilai(this.Page, tahun_ajaran, semester, rel_kelas, rel_mapel);
            //lnkOKPreviewNilaiKURTILAS.Attributes["onclick"] = GetJSPreviewNilai(this.Page, tahun_ajaran, semester, rel_kelas, rel_mapel);
        }

        protected void ShowStrukturNilaiSikapKURTILAS(string rel_strukturnilai)
        {
            string html = "";

            Rapor_StrukturNilai_KURTILAS m_strukturnilai = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(rel_strukturnilai);
            if (m_strukturnilai != null)
            {
                if (m_strukturnilai.TahunAjaran != null)
                {
                    List<Rapor_StrukturNilai_KURTILAS_Sikap> lst_sikap = DAO_Rapor_StrukturNilai_KURTILAS_Sikap.GetAllByHeader_Entity(rel_strukturnilai);
                    int id_sikap = 1;
                    foreach (var sikap in lst_sikap)
                    {
                        Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(sikap.Rel_Rapor_KompetensiDasar.ToString());
                        if (m_kd != null)
                        {
                            if (m_kd.Nama != null)
                            {
                                html += "<tr>" +
                                            "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 40px; text-align: center; vertical-align: top;\">" +
                                                id_sikap.ToString() +
                                            "</td>" +
                                            "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px; vertical-align: top;\">" +
                                               
                                            "</td>" +
                                            "<td style=\"text-align: justify; background-color: white; color: grey; padding-left: 0px;\">" +
                                                "<span style=\"font-weight: bold;\">" +
                                                    (sikap.Poin.Trim() != "" ? sikap.Poin + " " : "") +
                                                    Libs.GetHTMLSimpleText(m_kd.Nama) +
                                                "</span>" +
                                                "&nbsp;&nbsp;" +
                                                "<label title=\" Ubah Kompetensi Dasar \" onclick=\"" + txtIDKompetensiDasarSikap.ClientID + ".value = '" + sikap.Kode.ToString() + "'; " + " title=\" Ubah \" class=\"badge\" style=\"background-color: rgb(136, 153, 52); cursor: pointer; font-weight: normal; font-size: x-small;\">" +
                                                    "&nbsp;" +
                                                    "<i class=\"fa fa-edit\"></i>" +
                                                    "&nbsp;" +
                                                "</label>" +
                                                (
                                                    sikap.Deskripsi.Trim() != ""
                                                    ? "<br />" +
                                                      "<div class=\"text-input\" style=\"margin-top: 10px; border-radius: 3px; padding-top: 0px; padding-bottom: 0px;\">" +
                                                        sikap.Deskripsi +
                                                      "</div>"
                                                    : ""
                                                ) +
                                            "</td>" +
                                        "</tr>";
                                id_sikap++;
                            }
                        }
                    }
                }
            }

            if (html == "")
            {
                html = "<tr>" +
                            "<td colspan=\"9\" style=\"background-color: white; padding: 0px;\">" +
                                "<div style=\"padding: 10px;\"><label style=\"margin: 0 auto; display: table;\">..:: Data Kosong ::..</label></div>" +
                            "</td>" +
                       "</tr>";
            }
            else
            {
                html = "<tr>" +
                            "<td colspan=\"9\" style=\"background-color: white; padding: 0px;\">" +
                                "<table style=\"margin: 0px; width: 100%;\">" +
                                    html +
                               "</table>" +
                            "</td>" +
                       "</tr>";
            }

            ltrKurtilasSikap.Text = html;
        }

        protected void ShowStrukturNilaiKURTILAS(string rel_strukturnilai)
        {
            ShowStrukturNilaiSikapKURTILAS(rel_strukturnilai);

            string html = "";
            ltrKURTILASDet.Text = "";

            Rapor_StrukturNilai_KURTILAS m_strukturnilai = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(rel_strukturnilai);
            if (m_strukturnilai != null)
            {
                if (m_strukturnilai.TahunAjaran != null)
                {
                    InitLinkPreviewNilai(
                            m_strukturnilai.TahunAjaran, m_strukturnilai.Semester, m_strukturnilai.Rel_Kelas, m_strukturnilai.Rel_Mapel.ToString()
                        );

                    html += "<tr>";
                    html += "<td colspan=\"9\" style=\"background-color: #f9f7e2; font-weight: normal; color: grey; padding: 10px;\">";
                    html += "<div class=\"row\">" +
                                "<div class=\"col-xs-12\">" +
                                    "<span style=\"font-weight: bold; color: grey;\">" +
                                        "Bobot Rapor " +
                                        "<span style=\"color: mediumvioletred; border-radius: 0px;\">PENGETAHUAN</span>" +
                                    "</span>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"row\">" +
                                "<div class=\"col-xs-6\">" +
                                    "<span style=\"font-weight: normal\">Dari Pengetahuan</span> : " +
                                    "<span style=\"font-weight: bold\">" +
                                        Math.Round(m_strukturnilai.BobotRaporPengetahuan, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString() +
                                    "%</span>" +
                                    "&nbsp;&nbsp;" +
                                    "<span style=\"font-weight: normal\">Dari UAS</span> : " +
                                    "<span style=\"font-weight: bold\">" +
                                        Math.Round(m_strukturnilai.BobotRaporUAS, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString() +
                                     "%</span>" +
                                "</div>" +
                            "</div>";
                    html += "</td>";
                    html += "</tr>";
                }
            }

            string s_html_deskripsi = "";

            //header
            html += "<tr>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 40px; border-top-style: solid; border-top-color: #363535;\">" +
                        "&nbsp;" +
                    "</td>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 30px; border-top-style: solid; border-top-color: #363535;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/browser.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td colspan=\"6\" style=\"background-color: #424242; font-weight: bold; color: white; padding-left: 0px; border-top-style: solid; border-top-color: #363535;\">" +
                        "Aspek Penilaian (AP)" +
                    "</td>";
            html += "<td style=\"background-color: #424242; border-top-style: solid; border-top-color: #363535;\">" +
                    "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=\"2\" style=\"background-color: #484848; border-top-style: solid; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #484848; font-weight: normal; color: white; width: 30px; border-top-style: solid; border-top-color: #424242;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/questions.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td colspan=\"3\" style=\"background-color: #484848; font-weight: bold; color: white; padding-left: 0px; border-top-style: solid; border-top-color: #424242;\">" +
                        "Kompetensi Dasar (KD)" +
                    "</td>";
            html += "<td style=\"background-color: #484848; border-top-style: solid; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #484848; border-top-style: solid; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #484848; border-top-style: solid; border-top-color: #424242;\">" +
                    "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=\"4\" style=\"background-color: #4f4f4f; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; font-weight: normal; color: white; width: 30px; border-top-color: #424242;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/test.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td style=\"padding-left: 0px; color: white; background-color: #4f4f4f; font-weight: bold; border-top-color: #424242;\">" +
                        "Komponen Penilaian (KP)" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; border-top-color: #424242;\">" +
                    "</td>";
            html += "</tr>";
            //end header

            //list ap
            int id_ap = 1;
            string span_jenis_perhitungan = "";
            List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian = DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByHeader_Entity(rel_strukturnilai);
            foreach (Rapor_StrukturNilai_KURTILAS_AP struktur_ap in lst_aspek_penilaian)
            {
                Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(struktur_ap.Rel_Rapor_AspekPenilaian.ToString());
                if (m_ap != null)
                {
                    if (m_ap.Nama != null)
                    {
                        span_jenis_perhitungan = "";
                        if (struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                        {
                            span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                     "<sup class=\"badge\" style=\"background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;\">" +
                                                        Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) +
                                                     "</sup>";
                        }
                        else if (struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                        {
                            span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                     "<sup class=\"badge\" style=\"background-color: #68217A; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;\">" +
                                                        Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) +
                                                     "</sup>";
                        }

                        html += "<tr>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 40px;\">" +
                                    id_ap.ToString() +
                                "</td>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px;\">" +
                                    
                                "</td>";
                        html += "<td colspan=\"5\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                    (struktur_ap.Poin.Trim() != "" ? struktur_ap.Poin + " " : "") +
                                    Libs.GetHTMLSimpleText(m_ap.Nama) +
                                    span_jenis_perhitungan +
                                "</td>";
                        html += "<td style=\"background-color: white; font-weight: bold; color: grey; text-align: right;\">" +
                                    (
                                        struktur_ap.BobotRapor > 0
                                        ? "<span title=\" Bobot Rapor \" class=\"badge\" style=\"background-color: #f39100; border-radius: 0px;\">" +
                                                struktur_ap.BobotRapor.ToString() + "%" +
                                          "</span>"
                                        : ""
                                    ) +
                                "</td>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: grey; text-align: right; width: 150px;\">" +
                                    
                                "</td>";
                        html += "</tr>";


                        //list kd
                        int id_kd = 1;
                        List<Rapor_StrukturNilai_KURTILAS_KD> lst_kompetensi_dasar = DAO_Rapor_StrukturNilai_KURTILAS_KD.GetAllByHeader_Entity(struktur_ap.Kode.ToString());
                        foreach (Rapor_StrukturNilai_KURTILAS_KD struktur_kd in lst_kompetensi_dasar)
                        {
                            Rapor_KompetensiDasar kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                            if (kd != null)
                            {
                                if (kd.Nama != null)
                                {
                                    s_html_deskripsi = "";
                                    if (struktur_kd.IsKomponenRapor)
                                    {
                                        s_html_deskripsi = "<tr>" +
                                                                "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                                "</td>" +
                                                                "<td colspan=\"9\" style=\"background-color: white; padding: 0px; padding-left: 60px; padding-right: 20px; padding-bottom: 15px;\">" +
                                                                   "<label style=\"font-weight: normal; color: #bfbfbf; margin-bottom: 7px; font-size: small;\">" +
                                                                        "Deskripsi dalam Rapor" +
                                                                   "</label>" +
                                                                   "<br />" +
                                                                   "<div onclick=\"ShowProgress(true); " + txtJenisDeskripsiRaporLTS.ClientID + ".value = '0'; " + txtIDDeskripsi.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " + " class=\"div-like-text-input\" style=\"cursor: pointer; color: #bfbfbf;\">" +
                                                                        (
                                                                            struktur_kd.DeskripsiRapor.Replace("\"", "&#34;").Trim() != ""
                                                                            ? "<div class=\"reset-this\" style=\"width: 100%; padding: 0px; margin: 0px; font-size: small; color: grey; cursor: pointer;\">" +
                                                                                    Libs.GetHTMLNoParagraphDiAwal(struktur_kd.DeskripsiRapor.Trim()) +
                                                                              "</div>"
                                                                            : "Isi Deskripsi..."
                                                                        ) +
                                                                   "</div>" +
                                                                "</td>" +
                                                           "</tr>";
                                    }

                                    html += "<tr>";
                                    html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                            "</td>";
                                    html += "<td colspan=\"7\" style=\"background-color: white; padding: 0px;\">" +
                                                "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 1px;\" />" +
                                            "</td>";
                                    html += "</tr>";

                                    span_jenis_perhitungan = "";
                                    if (struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                    {
                                        span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                                 "<sup class=\"badge\" style=\"background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                                    Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) +
                                                                 "</sup>";
                                    }
                                    else if (struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                    {
                                        span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                                 "<sup class=\"badge\" style=\"background-color: #68217A; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                                    Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) +
                                                                 "</sup>";
                                    }

                                    string span_kd_komponen_rapor
                                        = (
                                            struktur_kd.IsKomponenRapor
                                            ? "<sup title=\" Komponen Rapor \" class=\"badge\" style=\"background-color: #FF4081; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                "n-KD" +
                                                "&nbsp;" +
                                                "<i class=\"fa fa-check-circle\"></i>" +
                                              "</sup>"
                                            : ""
                                          );

                                    html += "<tr>";
                                    html += "<td colspan=\"2\" style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 40px;\">" +
                                            "</td>";
                                    html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px;\">" +
                                               
                                            "</td>";
                                    html += (
                                                struktur_kd.BobotAP > 0
                                                ? "<td colspan=\"3\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                                        (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " " : "") +
                                                        Libs.GetHTMLSimpleText(kd.Nama) +
                                                        span_jenis_perhitungan +
                                                        span_kd_komponen_rapor +
                                                  "</td>" +
                                                  "<td style=\"background-color: white; font-weight: bold; color: grey; text-align: right;\">" +
                                                        "<span title=\" Bobot Aspek Penilaian \" class=\"badge\" style=\"background-color: #f39100; border-radius: 0px;\">" +
                                                            struktur_kd.BobotAP.ToString() + "%" +
                                                        "</span>" +
                                                  "</td>"
                                                : "<td colspan=\"4\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                                        (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " " : "") +
                                                        Libs.GetHTMLSimpleText(kd.Nama) +
                                                        span_jenis_perhitungan +
                                                        span_kd_komponen_rapor +
                                                  "</td>"
                                            );
                                    html += "<td style=\"background-color: white; font-weight: normal; color: grey; text-align: right; width: 150px;\">" +
                                               
                                            "</td>";
                                    html += "<td style=\"background-color: white; border-top-color: #424242;\">" +
                                            "</td>";
                                    html += "</tr>" +
                                            s_html_deskripsi;

                                    //komponen penilaian
                                    int id_kp = 0;
                                    List<Rapor_StrukturNilai_KURTILAS_KP> lst_komponen_penilaian =
                                        DAO_Rapor_StrukturNilai_KURTILAS_KP.GetAllByHeader_Entity(struktur_kd.Kode.ToString());

                                    if (id_kd <= lst_kompetensi_dasar.Count && lst_komponen_penilaian.Count > 0)
                                    {
                                        html += "<tr>";
                                        html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                "</td>";
                                        html += "<td colspan=\"7\" style=\"background-color: white; padding: 0px;\">" +
                                                    "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 1px;\" />" +
                                                "</td>";
                                        html += "</tr>";
                                    }

                                    foreach (Rapor_StrukturNilai_KURTILAS_KP m in lst_komponen_penilaian)
                                    {
                                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m.Rel_Rapor_KomponenPenilaian.ToString());
                                        if (m_kp != null)
                                        {
                                            if (m_kp.Nama != null)
                                            {
                                                s_html_deskripsi = "<tr>" +
                                                                        "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                                        "</td>" +
                                                                        "<td colspan=\"9\" style=\"background-color: white; padding: 0px; padding-left: 125px; padding-right: 20px; padding-bottom: 15px;\">" +
                                                                            "<label style=\"font-weight: normal; color: #bfbfbf; margin-bottom: 7px; font-size: small;\">" +
                                                                                "Deskripsi LTS" +
                                                                            "</label>" +
                                                                            "<br />" +
                                                                            "<div onclick=\"ShowProgress(true); " + txtJenisDeskripsiRaporLTS.ClientID + ".value = '2'; " + txtIDDeskripsi.ClientID + ".value = '" + m.Kode.ToString() + "'; " + " class=\"div-like-text-input\" style=\"cursor: pointer; color: #bfbfbf;\">" +
                                                                                (
                                                                                    m.Deskripsi.Replace("\"", "&#34;").Trim() != ""
                                                                                    ? "<div class=\"reset-this\" style=\"width: 100%; padding: 0px; margin: 0px; font-size: small; color: grey; cursor: pointer;\">" +
                                                                                            //Libs.GetHTMLNoParagraphDiAwal(m.Deskripsi.Replace("\"", "&#34;").Trim()) +
                                                                                            Libs.GetHTMLNoParagraphDiAwal(m.Deskripsi.Trim()) +
                                                                                      "</div>"
                                                                                    : "Isi Deskripsi..."
                                                                                ) +
                                                                            "</div>" +
                                                                        "</td>" +
                                                                   "</tr>";
                                                s_html_deskripsi = "";
                                                //if (m.IsKomponenRapor && Libs.GetHTMLSimpleText(m_kp.Nama).ToUpper() != Libs.JenisKomponenNilaiKURTILAS.SMA.UAS.ToUpper())
                                                //{
                                                //    s_html_deskripsi += "<tr>" +
                                                //                            "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                //                            "</td>" +
                                                //                            "<td colspan=\"9\" style=\"background-color: white; padding: 0px; padding-left: 125px; padding-right: 20px; padding-bottom: 15px;\">" +
                                                //                               "<label style=\"font-weight: normal; color: #bfbfbf; margin-bottom: 7px; font-size: small;\">" +
                                                //                                    "Deskripsi dalam Rapor" +
                                                //                               "</label>" +
                                                //                               "<br />" +
                                                //                               "<div onclick=\"ShowProgress(true); " + txtJenisDeskripsiRaporLTS.ClientID + ".value = '1'; " + txtIDDeskripsi.ClientID + ".value = '" + m.Kode.ToString() + "'; " + btnShowInputDeskripsiLTSRapor.ClientID + ".click();\" class=\"div-like-text-input\" style=\"cursor: pointer; color: #bfbfbf;\">" +
                                                //                                   (
                                                //                                        m.DeskripsiRapor.Replace("\"", "&#34;").Trim() != ""
                                                //                                        ? "<div class=\"reset-this\" style=\"width: 100%; padding: 0px; margin: 0px; font-size: small; color: grey; cursor: pointer;\">" +
                                                //                                                Libs.GetHTMLNoParagraphDiAwal(m.DeskripsiRapor.Trim()) +
                                                //                                          "</div>"
                                                //                                        : "Isi Deskripsi..."
                                                //                                   ) +
                                                //                               "</div>" +
                                                //                            "</td>" +
                                                //                       "</tr>";
                                                //}

                                                if (id_kp > 0)
                                                {
                                                    html += "<tr>";
                                                    html += "<td colspan=\"4\" style=\"background-color: white; padding: 0px;\">" +
                                                            "</td>";
                                                    html += "<td colspan=\"5\" style=\"background-color: white; padding: 0px;\">" +
                                                            "<hr style=\"margin: 0px; border-color: #ebebeb;\" />" +
                                                            "</td>";
                                                    html += "</tr>";
                                                }

                                                html += "<tr>";
                                                html += "<td colspan=\"4\" style=\"background-color: white;\">" +
                                                        "</td>";
                                                html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px;\">" +
                                                            
                                                        "</td>";
                                                html += "<td style=\"padding-left: 0px; color: black; background-color: white; font-weight: bold;\">" +
                                                            Libs.GetHTMLSimpleText(m_kp.Nama) +
                                                            "&nbsp;&nbsp;" +
                                                            "&nbsp;&nbsp;" +
                                                            (
                                                                m.BobotNK > 0
                                                                ? "<sup class=\"badge\" style=\"background-color: #f39100; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" + m.BobotNK.ToString() + "%</sup>"
                                                                : ""
                                                            ) +
                                                            (
                                                                m.IsKomponenRapor
                                                                ? "<sup title=\" Komponen Rapor \" class=\"badge\" style=\"background-color: #FF4081; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">KR</sup>"
                                                                : ""
                                                            ) +
                                                            "<a class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                    "href=\"" + ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.CBT.RUMAH_SOAL_INPUT.ROUTE) +
                                                                            "?m=" + Libs.GetQueryString("m") + "&u=" + Libs.GetQueryString("u") + "&kp=" + m.Kode+  "&kur='KURTILAS'" + "&nama=" + Libs.GetHTMLSimpleText(m_kp.Nama) +
                                                                            "\" " +
                                                                    "style=\"margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-left: 7px; padding-right: 7px; margin-top: 2px;\">" +
                                                                    "<i class=\"fa fa-home\"></i> Rumah Soal " +
                                                                "</a>" +
                                                        "</td>";
                                                html += "<td style=\"background-color: white;\">" +
                                                        "</td>";
                                                html += "<td style=\"background-color: white;\">" +
                                                        "</td>";
                                                html += "<td style=\"background-color: white;\">" +
                                                        "</td>";
                                                html += "</tr>" +
                                                        s_html_deskripsi;
                                            }
                                        }
                                        id_kp++;
                                    }
                                    id_kd++;
                                }
                            }
                        }

                        if (id_ap < lst_aspek_penilaian.Count)
                        {
                            html += "<tr>";
                            html += "<td colspan=\"9\" style=\"background-color: white; padding: 0px;\">" +
                                        "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 2px;\" />" +
                                    "</td>";
                            html += "</tr>";
                        }

                        id_ap++;
                    }
                }
            }

            if (lst_aspek_penilaian.Count == 0 || rel_strukturnilai.Trim() == "")
            {
                ltrKURTILASDet.Text = "<div style=\"padding: 10px;\"><label style=\"margin: 0 auto; display: table;\">..:: Data Kosong ::..</label></div>";
            }
            else
            {
                html = "<table style=\"margin: 0px; width: 100%;\">" +
                       html +
                       "</table>";
                ltrKURTILASDet.Text = html;
            }
        }

        protected void ShowStrukturNilaiKURTILAS_OLD(string rel_strukturnilai)
        {
            ShowStrukturNilaiSikapKURTILAS(rel_strukturnilai);

            string html = "";
            ltrKURTILASDet.Text = "";

            Rapor_StrukturNilai_KURTILAS m_strukturnilai = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(rel_strukturnilai);
            if (m_strukturnilai != null)
            {
                if (m_strukturnilai.TahunAjaran != null)
                {
                    html += "<tr>";
                    html += "<td colspan=\"9\" style=\"background-color: #f9f7e2; font-weight: normal; color: grey; padding: 10px;\">";
                    html += "<div class=\"row\">" +
                                "<div class=\"col-xs-12\">" +
                                    "<span style=\"font-weight: bold; color: grey;\">" +
                                        "Bobot Rapor " +
                                        "<span style=\"color: mediumvioletred; border-radius: 0px;\">PENGETAHUAN</span>" +
                                    "</span>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"row\">" +
                                "<div class=\"col-xs-6\">" +
                                    "<span style=\"font-weight: normal\">Dari Pengetahuan</span> : " +
                                    "<span style=\"font-weight: bold\">" +
                                        Math.Round(m_strukturnilai.BobotRaporPengetahuan, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString() +
                                    "%</span>" +
                                    "&nbsp;&nbsp;" +
                                    "<span style=\"font-weight: normal\">Dari UAS</span> : " +
                                    "<span style=\"font-weight: bold\">" +
                                        Math.Round(m_strukturnilai.BobotRaporUAS, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString() +
                                     "%</span>" +
                                "</div>" +
                            "</div>";
                    html += "</td>";
                    html += "</tr>";
                }
            }

            string s_html_deskripsi = "";

            //header
            html += "<tr>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 40px; border-top-style: solid; border-top-color: #363535;\">" +
                        "&nbsp;" +
                    "</td>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 30px; border-top-style: solid; border-top-color: #363535;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/browser.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td colspan=\"6\" style=\"background-color: #424242; font-weight: bold; color: white; padding-left: 0px; border-top-style: solid; border-top-color: #363535;\">" +
                        "Aspek Penilaian (AP)" +
                    "</td>";
            html += "<td style=\"background-color: #424242; border-top-style: solid; border-top-color: #363535;\">" +
                    "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=\"2\" style=\"background-color: #484848; border-top-style: solid; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #484848; font-weight: normal; color: white; width: 30px; border-top-style: solid; border-top-color: #424242;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/questions.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td colspan=\"3\" style=\"background-color: #484848; font-weight: bold; color: white; padding-left: 0px; border-top-style: solid; border-top-color: #424242;\">" +
                        "Kompetensi Dasar (KD)" +
                    "</td>";
            html += "<td style=\"background-color: #484848; border-top-style: solid; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #484848; border-top-style: solid; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #484848; border-top-style: solid; border-top-color: #424242;\">" +
                    "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=\"4\" style=\"background-color: #4f4f4f; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; font-weight: normal; color: white; width: 30px; border-top-color: #424242;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/test.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td style=\"padding-left: 0px; color: white; background-color: #4f4f4f; font-weight: bold; border-top-color: #424242;\">" +
                        "Komponen Penilaian (KP)" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; border-top-color: #424242;\">" +
                    "</td>";
            html += "</tr>";
            //end header

            //list ap
            int id_ap = 1;
            string span_jenis_perhitungan = "";
            List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian = DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByHeader_Entity(rel_strukturnilai);
            foreach (Rapor_StrukturNilai_KURTILAS_AP struktur_ap in lst_aspek_penilaian)
            {
                Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(struktur_ap.Rel_Rapor_AspekPenilaian.ToString());
                if (m_ap != null)
                {
                    if (m_ap.Nama != null)
                    {
                        span_jenis_perhitungan = "";
                        if (struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                        {
                            span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                     "<sup class=\"badge\" style=\"background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;\">" +
                                                        Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) +
                                                     "</sup>";
                        }
                        else if (struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                        {
                            span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                     "<sup class=\"badge\" style=\"background-color: #68217A; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;\">" +
                                                        Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) +
                                                     "</sup>";
                        }

                        html += "<tr>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 40px;\">" +
                                    id_ap.ToString() +
                                "</td>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px;\">" +
                                  
                                        "<label for=\"chk_ap_" + struktur_ap.Kode.ToString().Replace("-", "_") + "\">" +
                                            
                                        "</label>" +
                                    "</div>" +
                                "</td>";
                        html += "<td colspan=\"5\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                    (struktur_ap.Poin.Trim() != "" ? struktur_ap.Poin + " " : "") +
                                    Libs.GetHTMLSimpleText(m_ap.Nama) +
                                    span_jenis_perhitungan +
                                "</td>";
                        html += "<td style=\"background-color: white; font-weight: bold; color: grey; text-align: right;\">" +
                                    (
                                        struktur_ap.BobotRapor > 0
                                        ? "<span title=\" Bobot Rapor \" class=\"badge\" style=\"background-color: #f39100; border-radius: 0px;\">" +
                                                struktur_ap.BobotRapor.ToString() + "%" +
                                          "</span>"
                                        : ""
                                    ) +
                                "</td>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: grey; text-align: right; width: 150px;\">" +

                                "</td>";
                        html += "</tr>";


                        //list kd
                        int id_kd = 1;
                        List<Rapor_StrukturNilai_KURTILAS_KD> lst_kompetensi_dasar = DAO_Rapor_StrukturNilai_KURTILAS_KD.GetAllByHeader_Entity(struktur_ap.Kode.ToString());
                        foreach (Rapor_StrukturNilai_KURTILAS_KD struktur_kd in lst_kompetensi_dasar)
                        {
                            Rapor_KompetensiDasar kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                            if (kd != null)
                            {
                                if (kd.Nama != null)
                                {
                                    s_html_deskripsi = "";
                                    if (struktur_kd.IsKomponenRapor)
                                    {
                                        s_html_deskripsi = "<tr>" +
                                                                "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                                "</td>" +
                                                                "<td colspan=\"9\" style=\"background-color: white; padding: 0px; padding-left: 60px; padding-right: 20px; padding-bottom: 15px;\">" +
                                                                   "<label style=\"font-weight: normal; color: #bfbfbf; margin-bottom: 7px; font-size: small;\">" +
                                                                        "Deskripsi dalam Rapor" +
                                                                   "</label>" +
                                                                   "<br />" +
                                                                   "<input id=\"txt_" + struktur_kd.Kode.ToString().Replace("-", "") + "\" " +
                                                                          "onkeydown=\"SetAttrDeskripsi('" + struktur_kd.Kode.ToString() + "', 'KD', this.id); SetIsAutosave('0')\" " +
                                                                          "onkeyup=\"SetIsAutosave('1')\" " +
                                                                          "type=\"text\" " +
                                                                          "value=\"" + struktur_kd.DeskripsiRapor.Replace("\"", "&#34;") + "\" " +
                                                                          "class=\"text-input\" />" +
                                                                "</td>" +
                                                           "</tr>";
                                    }

                                    html += "<tr>";
                                    html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                            "</td>";
                                    html += "<td colspan=\"7\" style=\"background-color: white; padding: 0px;\">" +
                                                "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 1px;\" />" +
                                            "</td>";
                                    html += "</tr>";

                                    span_jenis_perhitungan = "";
                                    if (struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                    {
                                        span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                                 "<sup class=\"badge\" style=\"background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                                    Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) +
                                                                 "</sup>";
                                    }
                                    else if (struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                    {
                                        span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                                 "<sup class=\"badge\" style=\"background-color: #68217A; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                                    Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) +
                                                                 "</sup>";
                                    }

                                    string span_kd_komponen_rapor
                                        = (
                                            struktur_kd.IsKomponenRapor
                                            ? "<sup title=\" Komponen Rapor \" class=\"badge\" style=\"background-color: #FF4081; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                "n-KD" +
                                                "&nbsp;" +
                                                "<i class=\"fa fa-check-circle\"></i>" +
                                              "</sup>"
                                            : ""
                                          );

                                    html += "<tr>";
                                    html += "<td colspan=\"2\" style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 40px;\">" +
                                            "</td>";
                                    html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px;\">" +
                                              
                                                    "<label for=\"chk_kd_" + struktur_kd.Kode.ToString().Replace("-", "_") + "\">" +
                                                        
                                                "</div>" +
                                            "</td>";
                                    html += (
                                                struktur_kd.BobotAP > 0
                                                ? "<td colspan=\"3\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                                        (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " " : "") +
                                                        Libs.GetHTMLSimpleText(kd.Nama) +
                                                        span_jenis_perhitungan +
                                                        span_kd_komponen_rapor +
                                                  "</td>" +
                                                  "<td style=\"background-color: white; font-weight: bold; color: grey; text-align: right;\">" +
                                                        "<span title=\" Bobot Aspek Penilaian \" class=\"badge\" style=\"background-color: #f39100; border-radius: 0px;\">" +
                                                            struktur_kd.BobotAP.ToString() + "%" +
                                                        "</span>" +
                                                  "</td>"
                                                : "<td colspan=\"4\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                                        (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " " : "") +
                                                        Libs.GetHTMLSimpleText(kd.Nama) +
                                                        span_jenis_perhitungan +
                                                        span_kd_komponen_rapor +
                                                  "</td>"
                                            );
                                    html += "<td style=\"background-color: white; font-weight: normal; color: grey; text-align: right; width: 150px;\">" +

                                            "</td>";
                                    html += "<td style=\"background-color: white; border-top-color: #424242;\">" +
                                            "</td>";
                                    html += "</tr>" +
                                            s_html_deskripsi;

                                    //komponen penilaian
                                    int id_kp = 0;
                                    List<Rapor_StrukturNilai_KURTILAS_KP> lst_komponen_penilaian =
                                        DAO_Rapor_StrukturNilai_KURTILAS_KP.GetAllByHeader_Entity(struktur_kd.Kode.ToString());

                                    if (id_kd <= lst_kompetensi_dasar.Count && lst_komponen_penilaian.Count > 0)
                                    {
                                        html += "<tr>";
                                        html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                "</td>";
                                        html += "<td colspan=\"7\" style=\"background-color: white; padding: 0px;\">" +
                                                    "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 1px;\" />" +
                                                "</td>";
                                        html += "</tr>";
                                    }

                                    foreach (Rapor_StrukturNilai_KURTILAS_KP m in lst_komponen_penilaian)
                                    {
                                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m.Rel_Rapor_KomponenPenilaian.ToString());
                                        if (m_kp != null)
                                        {
                                            if (m_kp.Nama != null)
                                            {
                                                s_html_deskripsi = "<tr>" +
                                                                        "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                                        "</td>" +
                                                                        "<td colspan=\"9\" style=\"background-color: white; padding: 0px; padding-left: 125px; padding-right: 20px; padding-bottom: 15px;\">" +
                                                                            "<label style=\"font-weight: normal; color: #bfbfbf; margin-bottom: 7px; font-size: small;\">" +
                                                                                "Deskripsi LTS" +
                                                                            "</label>" +
                                                                            "<br />" +
                                                                            "<input id=\"txt_" + m.Kode.ToString().Replace("-", "") + "\" " +
                                                                                    "onkeydown=\"SetAttrDeskripsi('" + m.Kode.ToString() + "', 'KP_ITEM', this.id); SetIsAutosave('0')\" " +
                                                                                    "onkeyup=\"SetIsAutosave('1')\" " +
                                                                                    "type=\"text\" " +
                                                                                    "value=\"" + m.Deskripsi.Replace("\"", "&#34;") + "\" " +
                                                                                    "class=\"text-input\" />" +
                                                                        "</td>" +
                                                                   "</tr>";
                                                if (m.IsKomponenRapor && Libs.GetHTMLSimpleText(m_kp.Nama).ToUpper() != Libs.JenisKomponenNilaiKURTILAS.SMA.UAS.ToUpper())
                                                {
                                                    s_html_deskripsi += "<tr>" +
                                                                            "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                                            "</td>" +
                                                                            "<td colspan=\"9\" style=\"background-color: white; padding: 0px; padding-left: 125px; padding-right: 20px; padding-bottom: 15px;\">" +
                                                                               "<label style=\"font-weight: normal; color: #bfbfbf; margin-bottom: 7px; font-size: small;\">" +
                                                                                    "Deskripsi dalam Rapor" +
                                                                               "</label>" +
                                                                               "<br />" +
                                                                               "<input id=\"txt_" + m.Kode.ToString().Replace("-", "") + "\" " +
                                                                                      "onkeydown=\"SetAttrDeskripsi('" + m.Kode.ToString() + "', 'KP', this.id); SetIsAutosave('0')\" " +
                                                                                      "onkeyup=\"SetIsAutosave('1')\" " +
                                                                                      "type=\"text\" " +
                                                                                      "value=\"" + m.DeskripsiRapor.Replace("\"", "&#34;") + "\" " +
                                                                                      "class=\"text-input\" />" +
                                                                            "</td>" +
                                                                       "</tr>";
                                                }

                                                if (id_kp > 0)
                                                {
                                                    html += "<tr>";
                                                    html += "<td colspan=\"4\" style=\"background-color: white; padding: 0px;\">" +
                                                            "</td>";
                                                    html += "<td colspan=\"5\" style=\"background-color: white; padding: 0px;\">" +
                                                            "<hr style=\"margin: 0px; border-color: #ebebeb;\" />" +
                                                            "</td>";
                                                    html += "</tr>";
                                                }

                                                html += "<tr>";
                                                html += "<td colspan=\"4\" style=\"background-color: white;\">" +
                                                        "</td>";
                                                html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px;\">" +
                                                          
                                                                "<label for=\"chk_kp_" + m.Kode.ToString().Replace("-", "_") + "\">" +
                                                                    
                                                                "</label>" +
                                                            "</div>" +
                                                        "</td>";
                                                html += "<td style=\"padding-left: 0px; color: black; background-color: white; font-weight: bold;\">" +
                                                            Libs.GetHTMLSimpleText(m_kp.Nama) +
                                                            "&nbsp;&nbsp;" +
                                                            "&nbsp;&nbsp;" +
                                                            (
                                                                m.BobotNK > 0
                                                                ? "<sup class=\"badge\" style=\"background-color: #f39100; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" + m.BobotNK.ToString() + "%</sup>"
                                                                : ""
                                                            ) +
                                                            (
                                                                m.IsKomponenRapor
                                                                ? "<sup title=\" Komponen Rapor \" class=\"badge\" style=\"background-color: #FF4081; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">KR</sup>"
                                                                : ""
                                                            ) +

                                                        "</td>";
                                                html += "<td style=\"background-color: white;\">" +
                                                        "</td>";
                                                html += "<td style=\"background-color: white;\">" +
                                                        "</td>";
                                                html += "<td style=\"background-color: white;\">" +
                                                        "</td>";
                                                html += "</tr>" +
                                                        s_html_deskripsi;
                                            }
                                        }
                                        id_kp++;
                                    }
                                    id_kd++;
                                }
                            }
                        }

                        if (id_ap < lst_aspek_penilaian.Count)
                        {
                            html += "<tr>";
                            html += "<td colspan=\"9\" style=\"background-color: white; padding: 0px;\">" +
                                        "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 2px;\" />" +
                                    "</td>";
                            html += "</tr>";
                        }

                        id_ap++;
                    }
                }
            }

            if (lst_aspek_penilaian.Count == 0 || rel_strukturnilai.Trim() == "")
            {
                ltrKURTILASDet.Text = "<div style=\"padding: 10px;\"><label style=\"margin: 0 auto; display: table;\">..:: Data Kosong ::..</label></div>";
            }
            else
            {
                html = "<table style=\"margin: 0px; width: 100%;\">" +
                       html +
                       "</table>";
                ltrKURTILASDet.Text = html;
            }
        }

        protected void ShowStrukturNilaiKTSP(string rel_strukturnilai)
        {
            Rapor_StrukturNilai_KTSP m_strukturnilai = DAO_Rapor_StrukturNilai_KTSP.GetByID_Entity(rel_strukturnilai);
            if (m_strukturnilai != null)
            {
                if (m_strukturnilai.TahunAjaran != null)
                {
                    InitLinkPreviewNilai(
                            m_strukturnilai.TahunAjaran, m_strukturnilai.Semester, m_strukturnilai.Rel_Kelas, m_strukturnilai.Rel_Mapel.ToString()
                        );
                }
            }

            List<Rapor_StrukturNilai_KTSP_KD> lst_kompetensi_dasar = DAO_Rapor_StrukturNilai_KTSP_KD.GetAllByHeader_Entity(rel_strukturnilai);
            string html = "";
            ltrKTSPDet.Text = "";

            //header
            html += "<tr>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 40px; border-top-style: solid; border-top-color: #424242;\">" +
                        "<i class='fa fa-hashtag'></i>" +
                    "</td>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 30px; border-top-style: solid; border-top-color: #424242;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/questions.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td colspan=\"4\" style=\"background-color: #424242; font-weight: bold; color: white; padding-left: 0px; border-top-style: solid; border-top-color: #424242;\">" +
                        "Kompetensi Dasar (KD)" +
                    "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=\"2\" style=\"background-color: #4f4f4f;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; font-weight: normal; color: white; width: 30px;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/test.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td style=\"padding-left: 0px; color: white; background-color: #4f4f4f; font-weight: bold;\">" +
                        "Komponen Penilaian (KP)" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f;\">" +
                    "</td>";
            html += "</tr>";
            //end header

            int id_kd = 1;
            string span_jenis_perhitungan = "";
            foreach (Rapor_StrukturNilai_KTSP_KD m_kd in lst_kompetensi_dasar)
            {
                Rapor_KompetensiDasar kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_kd.Rel_Rapor_KompetensiDasar.ToString());
                if (kd != null)
                {
                    if (kd.Nama != null)
                    {
                        span_jenis_perhitungan = "";
                        if (m_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                        {
                            span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                     "<sup class=\"badge\" style=\"background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; float: right;\">" +
                                                        Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) +
                                                     "</sup>";
                        }
                        else if (m_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                        {
                            span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                     "<sup class=\"badge\" style=\"background-color: #68217A; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; float: right;\">" +
                                                        Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) +
                                                     "</sup>";
                        }

                        html += "<tr>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 40px;\">" +
                                    id_kd.ToString() +
                                "</td>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px;\">" +
                                  
                                        
                                    "</div>" +
                                "</td>";
                        html += "<td colspan=\"2\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px;\">" +
                                    (m_kd.Poin.Trim() != "" ? m_kd.Poin + " " : "") +
                                    Libs.GetHTMLSimpleText(kd.Nama) +
                                "</td>";
                        html += "<td style=\"background-color: white; font-weight: bold; color: grey; text-align: right;\">" +
                                    (
                                        IsMapelEkskul(m_strukturnilai.Rel_Mapel.ToString())
                                        ? ""
                                        : "<span title=\" Bobot Rapor PPK \" class=\"badge\" style=\"background-color: #40B3D2; border-radius: 0px;\">" +
                                                m_kd.BobotRaporPPK.ToString() + "%" +
                                          "</span>" +
                                          "<span title=\" Bobot Rapor Praktik \" class=\"badge\" style=\"background-color: #68217A; border-radius: 0px;\">" +
                                                m_kd.BobotRaporP.ToString() + "%" +
                                          "</span>"
                                    ) +
                                    span_jenis_perhitungan +
                                "</td>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: grey; text-align: right\">" +

                                "</td>";
                        html += "</tr>";

                        //komponen penilaian
                        int id = 0;
                        List<Rapor_StrukturNilai_KTSP_KP> lst_komponen_penilaian =
                            DAO_Rapor_StrukturNilai_KTSP_KP.GetAllByHeader_Entity(m_kd.Kode.ToString());
                        foreach (Rapor_StrukturNilai_KTSP_KP m in lst_komponen_penilaian)
                        {
                            Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m.Rel_Rapor_KomponenPenilaian.ToString());
                            if (m_kp != null)
                            {
                                if (m_kp.Nama != null)
                                {
                                    html += "<tr>";
                                    html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                            "</td>";
                                    html += "<td colspan=\"4\" style=\"background-color: white; padding: 0px;\">" +
                                            "<hr style=\"margin: 0px; border-color: #ebebeb;\" />" +
                                            "</td>";
                                    html += "</tr>";

                                    string bg_jenis = "#68217A";
                                    switch (m.Jenis)
                                    {
                                        case Libs.JenisKomponenNilaiKTSP.SMA.PPK:
                                            bg_jenis = "#40B3D2";
                                            break;
                                        case Libs.JenisKomponenNilaiKTSP.SMA.PRAKTIK:
                                            bg_jenis = "#68217A";
                                            break;
                                    }

                                    html += "<tr>";
                                    html += "<td colspan=\"2\" style=\"background-color: white;\">" +
                                            "</td>";
                                    html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px;\">" +
                                              
                                                 
                                                "</div>" +
                                            "</td>";
                                    html += "<td style=\"padding-left: 0px; color: black; background-color: white; font-weight: bold;\">" +
                                                Libs.GetHTMLSimpleText(m_kp.Nama) +
                                                (
                                                    !(IsMapelEkskulByStrukturNilaiKTSP(txtID.Value) && m_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                    ? ""
                                                    : "&nbsp;&nbsp;" +
                                                      "&nbsp;&nbsp;" +
                                                      (
                                                        !IsMapelEkskulByStrukturNilaiKTSP(txtID.Value)
                                                        ? "<sup class=\"badge\" style=\"background-color: " + bg_jenis + "; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" + m.Jenis + "</sup>"
                                                        : ""
                                                      ) +
                                                      (
                                                            m.BobotNKD > 0
                                                            ? "<sup class=\"badge\" style=\"background-color: #446D8C; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" + m.BobotNKD.ToString() + "%</sup>"
                                                            : ""
                                                      )
                                                ) +

                                            "</td>";
                                    html += "<td style=\"background-color: white;\">" +
                                            "</td>";
                                    html += "<td style=\"background-color: white;\">" +
                                            "</td>";
                                    html += "</tr>";
                                }
                            }
                            id++;
                        }

                        if (id_kd < lst_kompetensi_dasar.Count)
                        {
                            html += "<tr>";
                            html += "<td colspan=\"6\" style=\"background-color: white; padding: 0px;\">" +
                                        "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: " + (lst_komponen_penilaian.Count > 0 ? "2" : "1") + "px;\" />" +
                                    "</td>";
                            html += "</tr>";
                        }

                        id_kd++;
                    }
                }
            }
            if (lst_kompetensi_dasar.Count == 0 || rel_strukturnilai.Trim() == "")
            {
                ltrKTSPDet.Text = "<div style=\"padding: 10px;\"><label style=\"margin: 0 auto; display: table;\">..:: Data Kosong ::..</label></div>";
            }
            else
            {
                html = "<table style=\"margin: 0px; width: 100%;\">" +
                       html +
                       "</table>";
                ltrKTSPDet.Text = html;
            }
        }


        public static bool IsMapelEkskul(string rel_mapel)
        {
            Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
            if (m_mapel != null)
            {
                if (m_mapel.Nama != null)
                {

                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.EKSKUL ||
                        m_mapel.Jenis == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }

            return false;
        }

        protected bool IsMapelEkskulByStrukturNilaiKTSP(string rel_sn)
        {
            Rapor_StrukturNilai_KTSP m_sn = DAO_Rapor_StrukturNilai_KTSP.GetByID_Entity(rel_sn);
            if (m_sn != null)
            {
                if (m_sn.TahunAjaran != null)
                {

                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m_sn.Rel_Mapel.ToString());
                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
                        {

                            if (m_mapel.Jenis == Libs.JENIS_MAPEL.EKSKUL ||
                                m_mapel.Jenis == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                    }

                }
            }

            return false;
        }

        protected bool IsMapelEkskulByStrukturNilaiKURTILAS(string rel_sn)
        {
            Rapor_StrukturNilai_KURTILAS m_sn = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(rel_sn);
            if (m_sn != null)
            {
                if (m_sn.TahunAjaran != null)
                {

                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m_sn.Rel_Mapel.ToString());
                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
                        {

                            if (m_mapel.Jenis == Libs.JENIS_MAPEL.EKSKUL ||
                                m_mapel.Jenis == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                    }

                }
            }

            return false;
        }


        protected void btnShowDataListFromKURTILASDet_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

       

























        protected void lnkOKDeskripsiLTSRapor_Click(object sender, EventArgs e)
        {
            if (txtJenisDeskripsiRaporLTS.Value == "0")
            {
                DAO_Rapor_StrukturNilai_KURTILAS_KD.UpdateDeskripsiRapor(
                        new Guid(txtIDDeskripsi.Value),
                        txtDeskripsiLTSRaporVal.Value,
                        Libs.LOGGED_USER_M.UserID
                    );
                ShowStrukturNilaiKURTILAS(txtID.Value);
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
            else if (txtJenisDeskripsiRaporLTS.Value == "1")
            {
                DAO_Rapor_StrukturNilai_KURTILAS_KP.UpdateDeskripsiRapor(
                        new Guid(txtIDDeskripsi.Value),
                        txtDeskripsiLTSRaporVal.Value,
                        Libs.LOGGED_USER_M.UserID
                    );
                ShowStrukturNilaiKURTILAS(txtID.Value);
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
            else if (txtJenisDeskripsiRaporLTS.Value == "2")
            {
                DAO_Rapor_StrukturNilai_KURTILAS_KP.UpdateDeskripsi(
                        new Guid(txtIDDeskripsi.Value),
                        txtDeskripsiLTSRaporVal.Value,
                        Libs.LOGGED_USER_M.UserID
                    );
                ShowStrukturNilaiKURTILAS(txtID.Value);
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
        }



        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

        public static string GetHTMLKelasMapelDetIsiNilai(string tahun_ajaran, string semester, string rel_kelas, string rel_mapel, string kurikulum)
        {
            string s_html = "";

            if (kurikulum == Libs.JenisKurikulum.SMA.KURTILAS_SIKAP) return s_html;

            Dictionary<string, string> lst_kelasdet = new Dictionary<string, string>();
            List<DAO_FormasiGuruMapelDet.FormasiGuruMapelDet_Lengkap> lst_guru = DAO_FormasiGuruMapelDet.GetByTABySMByKelasByMapel_Entity(
                    tahun_ajaran, semester, rel_kelas, rel_mapel
                );

            Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
            if (m_mapel != null && m_mapel.Nama != null)
            {
                foreach (DAO_FormasiGuruMapelDet.FormasiGuruMapelDet_Lengkap item_guru in lst_guru)
                {
                    KelasDet m_kelas_det = new KelasDet();
                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT)
                    {
                        List<string> lst_kelas_det = DAO_FormasiGuruMapelDetSiswa.GetByDistinctKelasDetHeader_Entity(item_guru.Rel_FormasiGuruMapel.ToString());
                        foreach (var item_kelas_det in lst_kelas_det)
                        {
                            m_kelas_det = DAO_KelasDet.GetByID_Entity(item_kelas_det);
                            if (m_kelas_det != null)
                            {
                                if (m_kelas_det.Nama != null)
                                {
                                    if (!lst_kelasdet.ContainsKey(m_kelas_det.Kode.ToString()))
                                    {
                                        lst_kelasdet.Add(m_kelas_det.Kode.ToString(), m_kelas_det.Nama);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        m_kelas_det = DAO_KelasDet.GetByID_Entity(item_guru.Rel_KelasDet.ToString());
                        if (m_kelas_det != null)
                        {
                            if (m_kelas_det.Nama != null)
                            {
                                if (!lst_kelasdet.ContainsKey(m_kelas_det.Kode.ToString()))
                                {
                                    lst_kelasdet.Add(m_kelas_det.Kode.ToString(), m_kelas_det.Nama);
                                }
                            }
                        }
                    }
                }
            }

            foreach (var item_kelas in lst_kelasdet)
            {
                bool is_isi_nilai = (
                        (
                            kurikulum == Libs.JenisKurikulum.SMA.KURTILAS
                            ? (
                                lst_nilai_kurtilas.FindAll(m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == rel_mapel.ToString().ToUpper().Trim() && m0.Rel_KelasDet.ToString().ToUpper().Trim() == item_kelas.Key.ToString().ToUpper().Trim()).Count > 0
                                ? true
                                : false
                              )
                            : (
                                lst_nilai_ktsp.FindAll(m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == rel_mapel.ToString().ToUpper().Trim() && m0.Rel_KelasDet.ToString().ToUpper().Trim() == item_kelas.Key.ToString().ToUpper().Trim()).Count > 0
                                ? true
                                : false
                              )
                        )
                    );

                s_html += "<div class=\"tooltip\">" +
                              "<i class=\"fa fa-circle\" style=\"margin-right: 3px; color: " + (is_isi_nilai ? "green" : "darkorange") + "; font-size: x-small;\"></i>" +
                              "<div class=\"top\" style=\"width: 130px;\">" +
                                "<label style=\"display: table; font-weight: bold; width: 100%;\">" +
                                    item_kelas.Value +
                                "</label>" +
                                (
                                    is_isi_nilai
                                    ? "<label style=\"display: table; font-weight: normal; font-size: x-small; width: 100%;\">" +
                                        "Sudah isi Nilai" +
                                      "</label>"
                                    : "<label style=\"display: table; font-weight: normal; font-size: x-small; width: 100%;\">" +
                                        "Belum isi Nilai" +
                                      "</label>"
                                ) +
                                "<i></i>" +
                              "</div>" +
                          "</div>";
            }

            return s_html;
        }




        protected void btnBackToMapel_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var kp = Libs.GetQueryString("kp");
            var kur = Libs.GetQueryString("kur");
            var unit = Libs.GetQueryString("u");
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.MAPEL.ROUTE // + "?&m=" + m + "&kp=" + kp + "&kur=" + kur + "&u=" + unit
                        )
                );
        }





    }
}
