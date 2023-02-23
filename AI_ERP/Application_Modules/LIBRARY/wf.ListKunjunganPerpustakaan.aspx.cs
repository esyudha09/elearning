using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities.Perpustakaan;
using AI_ERP.Application_DAOs.Perpustakaan;

namespace AI_ERP.Application_Modules.LIBRARY
{
    public partial class wf_ListKunjunganPerpustakaan : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATALISTKUNJUNGANPERPUSTAKAAN";

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            Edit,
            Update,
            Delete,
            Search,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowConfirmHapus,
            ShowInfoKunjungan,
            ShowPengaturanTampilan
        }

        private static class QS
        {
            public static string GetUnit()
            {
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(Libs.GetQueryString("kd"));
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                        return m_kelas.Rel_Sekolah.ToString();
                    }
                }

                return "";
            }

            public static string GetLevel()
            {
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(Libs.GetQueryString("kd"));
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        return m_kelas_det.Rel_Kelas.ToString();
                    }
                }

                return "";
            }

            public static string GetKelas()
            {
                return Libs.GetQueryString("kd");
            }

            public static string GetMapel()
            {
                return Libs.GetQueryString("m");
            }

            public static string GetTahunAjaran()
            {
                string t = Libs.GetQueryString("t");
                return RandomLibs.GetParseTahunAjaran(t);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            string s_caption = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/wall-calendar.svg") + "\">" +
                                    "&nbsp;&nbsp;" +
                               "Jadwal Kunjungan Perpustakaan";
            this.Master.HeaderTittle = s_caption;
            ltrCaptionDataList.Text = s_caption;

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitKeyEventClient();
                InitAppearance();
                txtTanggal1.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
                txtTanggal2.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
                Libs.ListUnitSekolahToDropDown(cboUnitSekolah, "(Semua Unit)", "-");
                ltrCaptionDataList.Text = s_caption;
            }
            BindListView(!IsPostBack, Libs.GetQ());
            if (!IsPostBack) this.Master.txtCariData.Text = Libs.GetQ();
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
        }

        protected void InitAppearance()
        {
            string ft = Libs.Decryptdata(Libs.GetQueryString("ft"));
            string[] arr_bg =
                { "#FD6933", "#0AC6AE", "#F7921E", "#4AA4A4", "#43B8C9", "#95D1C5", "#019ADD", "#31384B", "#18AEC7", "#5299CF", "#2D2C28", "#D5C5C6", "#262726", "#01ACAC", "#322D3A", "#3B4F5D", "#009E00", "#E90080", "#549092", "#00A9A9", "#9B993A" };
            string[] arr_bg_image =
                { "a.png", "b.png", "c.png", "d.png", "e.png", "f.png", "g.png", "h.png", "i.png", "j.png", "k.png", "l.png", "m.png", "n.png", "o.png", "p.png", "q.png", "r.png", "u.png", "s.png", "t.png" };
            string bg_image = "";

            if (ft.Trim() != "")
            {
                int _id = 0;
                for (int i = 0; i < arr_bg.Length; i++)
                {
                    if (arr_bg_image[i] == ft)
                    {
                        _id = i;
                        bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + arr_bg_image[i]);
                        break;
                    }
                }
                ltrBGHeader.Text = "background: url(" + bg_image + "); background-color: " + arr_bg[_id] + "; background-size: 60px 60px; background-repeat: no-repeat; background-position: left center;";
            }

            string s_html = "";
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            string rel_kelas_det = Libs.GetQueryString("kd");
            string rel_mapel = Libs.GetQueryString("m");

            KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(rel_kelas_det);

            string s_mapel = "";
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

            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    if (s_mapel.Trim() != "")
                    {
                        s_html = "<label style=\"margin-left: 45px; color: white;\">" +
                                    "<label style=\"font-weight: bold;\">Kunjungan ke Perpustakaan</label><br />" +
                                    "Kelas" +
                                    "&nbsp;" +
                                    "<span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span>" +
                                    "&nbsp;" +
                                    tahun_ajaran +
                                 "</label>" +
                                 "<br />" +
                                 "<label style=\"margin-left: 45px; color: white; font-weight: bold;\">" +
                                    s_mapel +
                                 "</label>";
                    }
                    else
                    {
                        s_html = "<label style=\"margin-left: 45px; color: white; margin-top: 0px; margin-bottom: 0px;\">" +
                                    "<label style=\"font-weight: bold;\">Kunjungan ke Perpustakaan</label><br />" +
                                    "Kelas" +
                                    "&nbsp;" +
                                    "<span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span>" +
                                    "&nbsp;" +
                                    tahun_ajaran +
                                    "<br />" +
                                    "<label style=\"font-size: medium; color: white; font-size: small; font-weight: bold; color: yellow;\">Guru Kelas atau Wali Kelas</label>" +
                                 "</label>";
                    }
                }
            }

            ltrCaptionDataList.Text = s_html;
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectParameters.Add("Tanggal1", Libs.GetDateFromTanggalIndonesiaStr(txtTanggal1.Text).ToString("yyyy-MM-dd"));
                sql_ds.SelectParameters.Add("Tanggal2", Libs.GetDateFromTanggalIndonesiaStr(txtTanggal2.Text).ToString("yyyy-MM-dd"));
                sql_ds.SelectParameters.Add("Rel_Sekolah", cboUnitSekolah.SelectedValue);
                sql_ds.SelectCommand = DAO_PerpustakaanKunjungan.SP_SELECT_ALL_BY_TANGGAL_BY_UNIT_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("Tanggal1", Libs.GetDateFromTanggalIndonesiaStr(txtTanggal1.Text).ToString("yyyy-MM-dd"));
                sql_ds.SelectParameters.Add("Tanggal2", Libs.GetDateFromTanggalIndonesiaStr(txtTanggal2.Text).ToString("yyyy-MM-dd"));
                sql_ds.SelectParameters.Add("Rel_Sekolah", cboUnitSekolah.SelectedValue);
                sql_ds.SelectCommand = DAO_PerpustakaanKunjungan.SP_SELECT_ALL_BY_TANGGAL_BY_UNIT;
            }
            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tanggal = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tanggal");
            System.Web.UI.WebControls.Literal imgh_keterangan = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_keterangan");
            System.Web.UI.WebControls.Literal imgh_status = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_status");

            string html_image = "";
            if (e.SortDirection == SortDirection.Ascending)
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-up\" style=\"color: white;\"></i>";
            }
            else
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-down\" style=\"color: white;\"></i>";
            }

            imgh_tanggal.Text = html_image;
            imgh_keterangan.Text = html_image;
            imgh_status.Text = html_image;

            imgh_tanggal.Visible = false;
            imgh_keterangan.Visible = false;
            imgh_status.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "Tanggal":
                    imgh_tanggal.Visible = true;
                    break;
                case "Keterangan":
                    imgh_keterangan.Visible = true;
                    break;
                case "Status":
                    imgh_status.Visible = true;
                    break;
            }

            int pageindex = int.Parse(Math.Ceiling(Convert.ToDecimal(dpData.StartRowIndex / 20)).ToString());
            pageindex--;
            this.Session[SessionViewDataName] = (pageindex < 0 ? 0 : pageindex);
        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Libs.FILE_PAGE_URL);
        }

        protected void InitFields()
        {
            txtID.Value = "";
            txtParseJadwal.Value = "";
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_PerpustakaanKunjungan.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            Response.Redirect(Libs.FILE_PAGE_URL + (this.Master.txtCariData.Text.Trim() != "" ? "?q=" + this.Master.txtCariData.Text : ""));
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                PerpustakaanKunjungan m = DAO_PerpustakaanKunjungan.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        ltrMsgConfirmHapus.Text = "Hapus jadwal kunjungan perpustakaan <span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetTanggalIndonesiaFromDate(m.Tanggal, false) +
                                                      "\"</span>?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void ShowList()
        {
            BindListView(true);
            mvMain.ActiveViewIndex = 0;
        }

        protected void lnkBatalIsiKunjungan_Click(object sender, EventArgs e)
        {
            ShowList();
        }

        public static string GetJamKe(string rel_header, bool is_bold = true)
        {
            string hasil = "";

            List<PerpustakaanKunjunganDet> lst_perpustakaan_det = DAO_PerpustakaanKunjunganDet.GetByHeader_Entity(rel_header);
            foreach (PerpustakaanKunjunganDet m in lst_perpustakaan_det)
            {
                hasil += (hasil.Trim() != "" && m.JamKe.Trim() != "" ? "<br />" : "") +
                         m.JamKe + (m.JamKe.Trim() != "" ? " <span " + (is_bold ? "style=\"font-weight: bold;\"" : "") + " ><i class=\"fa fa-clock-o\"></i>&nbsp;" + m.Waktu + "</span>" : "");
            }

            return hasil;
        }

        protected void lnkOKPilihan_Click(object sender, EventArgs e)
        {

        }

        protected void btnShowInfoKunjungan_Click(object sender, EventArgs e)
        {
            ShowInfoKunjungan();
            txtKeyAction.Value = JenisAction.ShowInfoKunjungan.ToString();
        }

        protected void ShowInfoKunjungan()
        {
            string html = "";

            if (txtID.Value.Trim() != "")
            {
                string s_kelas = "";
                PerpustakaanKunjungan m_kunjungan = DAO_PerpustakaanKunjungan.GetByID_Entity(txtID.Value);
                if (m_kunjungan != null)
                {
                    if (m_kunjungan.TahunAjaran != null)
                    {
                        KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(m_kunjungan.Rel_KelasDet);
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                s_kelas = m_kelas.Nama;
                            }
                        }

                        html = "<div class=\"row\">" +
                                    "<div class=\"col-md-12\">" +
                                        "<label style=\"color: #bfbfbf; font-weight: normal; margin-bottom: 10px;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Tahun Pelajaran & Semester</label>" +
                                        "<div style=\"margin-left: 20px;\">" +
                                            "<span style=\"color: grey; font-weight: bold;\">" + m_kunjungan.TahunAjaran + "</span>" +
                                            "&nbsp;" +
                                            "<sup class=\"badge\" style=\"font-size: x-small; color: grey; background-color: white; border-style: solid; border-width: 1px; border-color: #bfbfbf; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                "Semester " + m_kunjungan.Semester +
                                            "</sup>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                                (
                                    s_kelas.Trim() != ""
                                    ? "<div class=\"row\">" +
                                            "<div class=\"col-md-12\" style=\"padding: 0px;\">" +
                                                "<hr style=\"margin: 10px; border-color: #e6e6e6; border-width: 1px;\" />" +
                                            "</div>" +
                                      "</div>" +
                                      "<div class=\"row\">" +
                                            "<div class=\"col-md-12\">" +
                                                "<span style=\"color: #bfbfbf; font-weight: normal;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Kelas</span>" +
                                                "<div style=\"margin-left: 20px;\">" +
                                                    "<span style=\"color: grey; font-weight: bold;\">" + s_kelas + "</span>" +
                                                "</div>" +
                                            "</div>" +
                                      "</div>"
                                    : ""
                                ) +
                                "<div class=\"row\">" +
                                    "<div class=\"col-md-12\" style=\"padding: 0px;\">" +
                                        "<hr style=\"margin: 10px; border-color: #e6e6e6; border-width: 1px;\" />" +
                                    "</div>" +
                                "</div>" +
                                "<div class=\"row\">" +
                                    "<div class=\"col-md-12\">" +
                                        "<span style=\"color: #bfbfbf; font-weight: normal;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Guru</span>" +
                                        "<div style=\"margin-left: 20px;\">" +
                                            "<span style=\"color: grey; font-weight: bold;\">" + DAO_Pegawai.GetByID_Entity(m_kunjungan.Rel_Guru).Nama + "</span>" +
                                        "</div>" +
                                    "</div>" +
                               "</div>" +
                               (
                                    m_kunjungan.Rel_Mapel.Trim() != ""
                                    ? "<div class=\"row\">" +
                                            "<div class=\"col-md-12\" style=\"padding: 0px;\">" +
                                                "<hr style=\"margin: 10px; border-color: #e6e6e6; border-width: 1px;\" />" +
                                            "</div>" +
                                       "</div>" +
                                       "<div class=\"row\">" +
                                            "<div class=\"col-md-12\">" +
                                                "<span style=\"color: #bfbfbf; font-weight: normal;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Mata Pelajaran</span>" +
                                                "<div style=\"margin-left: 20px;\">" +
                                                    "<span style=\"color: grey; font-weight: bold;\">" + DAO_Mapel.GetByID_Entity(m_kunjungan.Rel_Mapel).Nama + "</span>" +
                                                "</div>" +
                                            "</div>" +
                                       "</div>"
                                    : ""
                               ) +                               
                               "<div class=\"row\">" +
                                    "<div class=\"col-md-12\" style=\"padding: 0px;\">" +
                                        "<hr style=\"margin: 10px; border-color: #e6e6e6; border-width: 1px;\" />" +
                                    "</div>" +
                               "</div>" +
                               "<div class=\"row\">" +
                                    "<div class=\"col-md-12\">" +
                                        "<span style=\"color: #bfbfbf; font-weight: normal;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Tanggal Kunjungan</span>" +
                                        "<div style=\"margin-left: 20px;\">" +
                                            "<span style=\"color: grey; font-weight: bold;\">" + Libs.GetTanggalIndonesiaFromDate(m_kunjungan.Tanggal, false) + "</span>" +
                                        "</div>" +
                                    "</div>" +
                               "</div>" +
                               "<div class=\"row\">" +
                                    "<div class=\"col-md-12\" style=\"padding: 0px;\">" +
                                        "<hr style=\"margin: 10px; border-color: #e6e6e6; border-width: 1px;\" />" +
                                    "</div>" +
                               "</div>" +
                               "<div class=\"row\">" +
                                    "<div class=\"col-md-12\">" +
                                        "<span style=\"color: #bfbfbf; font-weight: normal;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Jam Ke/Waktu</span>" +
                                        "<div style=\"margin-left: 20px;\">" +
                                            "<span style=\"color: grey; font-weight: bold;\">" +
                                                AI_ERP.Application_Modules.LIBRARY.wf_KunjunganPerpustakaan.GetJamKe(m_kunjungan.Kode.ToString(), false) +
                                            "</span>" +
                                        "</div>" +
                                    "</div>" +
                               "</div>" +
                               "<div class=\"row\">" +
                                    "<div class=\"col-md-12\" style=\"padding: 0px;\">" +
                                        "<hr style=\"margin: 10px; border-color: #e6e6e6; border-width: 1px;\" />" +
                                    "</div>" +
                               "</div>" +
                               (
                                    Libs.GetHTMLSimpleText(m_kunjungan.Keterangan.Trim()) != ""
                                    ? "<div class=\"row\">" +
                                            "<div class=\"col-md-12\" style=\"padding: 0px;\">" +
                                                "<hr style=\"margin: 10px; border-color: #e6e6e6; border-width: 1px;\" />" +
                                            "</div>" +
                                      "</div>" +
                                      "<div class=\"row\">" +
                                            "<div class=\"col-md-12\">" +
                                                "<span style=\"color: #bfbfbf; font-weight: normal;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Keterangan</span>" +
                                                "<div style=\"margin-left: 20px;\">" +
                                                    "<span style=\"color: grey; font-weight: bold;\">" +
                                                        m_kunjungan.Keterangan +
                                                    "</span>" +
                                                "</div>" +
                                            "</div>" +
                                      "</div>"
                                    : ""
                               );

                        cboStatus.Items.Clear();
                        cboStatus.Items.Add("");
                        cboStatus.Items.Add(new ListItem { Value = ((int)JenisStatusKunjungan.JenisStatus.Diproses).ToString(), Text = "Diproses", Selected = (m_kunjungan.Status == ((int)JenisStatusKunjungan.JenisStatus.Diproses).ToString() ? true : false) });
                        cboStatus.Items.Add(new ListItem { Value = ((int)JenisStatusKunjungan.JenisStatus.Dibatalkan).ToString(), Text = "Dibatalkan", Selected = (m_kunjungan.Status == ((int)JenisStatusKunjungan.JenisStatus.Dibatalkan).ToString() ? true : false) });
                        cboStatus.Items.Add(new ListItem { Value = ((int)JenisStatusKunjungan.JenisStatus.Dilaksanakan).ToString(), Text = "Dilaksanakan", Selected = (m_kunjungan.Status == ((int)JenisStatusKunjungan.JenisStatus.Dilaksanakan).ToString() ? true : false) });
                    }
                }
            }

            ltrInfoKunjungan.Text = html;            
        }

        protected void lnkOKSave_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                PerpustakaanKunjungan m = DAO_PerpustakaanKunjungan.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        m.Status = cboStatus.SelectedValue;
                        DAO_PerpustakaanKunjungan.Update(
                            m,
                            Libs.LOGGED_USER_M.UserID
                        );
                        BindListView(true, Libs.GetQ());
                        txtID.Value = "";
                    }
                }                
            }
        }

        protected void btnDoShowTampilanData_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.ShowPengaturanTampilan.ToString();
        }

        protected void lnkOKTampilanData_Click(object sender, EventArgs e)
        {
            BindListView(true, Libs.GetQ());
        }

        protected void lnkOKTampilanDataSemua_Click(object sender, EventArgs e)
        {
            txtTanggal1.Text = Libs.GetTanggalIndonesiaFromDate(Convert.ToDateTime("2018-07-01"), false);
            txtTanggal2.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now.AddYears(5), false);
            BindListView(true, Libs.GetQ());
        }

        protected void lnkOKHariIni_Click(object sender, EventArgs e)
        {
            txtTanggal1.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
            txtTanggal2.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
            BindListView(true, Libs.GetQ());
        }
    }
}