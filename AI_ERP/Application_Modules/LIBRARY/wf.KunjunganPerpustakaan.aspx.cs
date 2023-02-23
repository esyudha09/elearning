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
    public partial class wf_KunjunganPerpustakaan : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAKUNJUNGANPERPUSTAKAAN";

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
            DoShowConfirmHapus
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

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/book.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Kunjungan ke Perpustakaan";
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                this.Master.ShowHeaderTools = true;
                InitKeyEventClient();
                InitAppearance();
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
                sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
                sql_ds.SelectParameters.Add("Rel_Guru", Libs.LOGGED_USER_M.NoInduk);
                sql_ds.SelectCommand = DAO_PerpustakaanKunjungan.SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET_BY_GURU_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
                sql_ds.SelectParameters.Add("Rel_Guru", Libs.LOGGED_USER_M.NoInduk);
                sql_ds.SelectCommand = DAO_PerpustakaanKunjungan.SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET_BY_GURU;
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
            BindListView(!IsPostBack, Libs.GetQ().Trim());
        }

        protected void InitFields()
        {
            txtID.Value = "";
            txtTanggal.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
            ListJamKalender();
            txtParseJadwal.Value = "";
            txtKeterangan.Text = "";
        }

        protected void ListJamKalender()
        {
            ltrJamWaktu.Text = "";
            if (txtTanggal.Text.Trim() != "")
            {
                int max_jumlah_kunjungan = 2;

                DateTime tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggal.Text);
                List<PerpustakaanKunjunganJamSettings> lst_perpustakaan = DAO_PerpustakaanKunjunganJamSettings.GetByUnit_Entity(QS.GetUnit());
                string html =
                        "<div class=\"table-responsive\" style=\"margin-bottom: 0px; margin-top: 0px;\">" +
                        "<table class=\"table\">";

                int id = 0;
                foreach (PerpustakaanKunjunganJamSettings m in lst_perpustakaan)
                {
                    if (
                        !(
                            (
                                m.JamKe == "7" || m.JamKe == "8" ||
                                m.JamKe == "9" || m.JamKe == "10"
                            ) &&
                            (
                                tanggal.Year == 2020 &&
                                tanggal.Month == 1 &&
                                tanggal.Day == 28
                            )
                        )
                    )
                    {
                        List<PerpustakaanKunjunganDet> lst_kunjungan = DAO_PerpustakaanKunjunganDet.GetByTanggalByWaktu_Entity(
                                Libs.GetDateFromTanggalIndonesiaStr(txtTanggal.Text),
                                m.Waktu
                            );

                        html += "<tr class=\"" + (id % 2 == 0 ? "standardrow" : "oddrow") + "\">" +
                                    "<td style=\"padding: 15px; padding-top: 5px; padding-bottom: 5px;\">" +
                                        "<div class=\"checkbox checkbox-adv\">" +
                                            "<label style=\"color: grey;\" for=\"chk_item_jadwal_" + m.Kode.ToString().Replace("-", "_") + "\">" +
                                                "<input value=\"" + m.Kode.ToString() + "\" " +
                                                       (
                                                            lst_kunjungan.Count >= max_jumlah_kunjungan
                                                            ? "disabled=\"disabled\" "
                                                            : ""
                                                       ) +
                                                       "class=\"access-hide\" " +
                                                       "id=\"chk_item_jadwal_" + m.Kode.ToString().Replace("-", "_") + "\" " +
                                                       "name=\"chk_item_jadwal[]\" " +
                                                       "type=\"checkbox\" />" +
                                                "<span style=\"font-weight: normal; font-size: small;\">" +
                                                    "Jam Ke " +
                                                "</span>" +
                                                "&nbsp;" +
                                                "<span style=\"font-weight: bold; font-size: small;\">" +
                                                    m.JamKe +
                                                "</span>" +
                                                "&nbsp;" +
                                                "&nbsp;" +
                                                "<span style=\"font-weight: bold; font-size: small;\">" +
                                                    "<i class=\"fa fa-clock-o\"></i>" +
                                                    "&nbsp;" +
                                                    m.Waktu +
                                                "</span>" +
                                                "<br />" +
                                                (
                                                    lst_kunjungan.Count >= max_jumlah_kunjungan
                                                    ? "<i class=\"fa fa-exclamation-triangle\" style=\"color: orange;\"></i>" +
                                                      "&nbsp;" +
                                                      "<span style=\"color: orange\">Tidak tersedia</span>"
                                                    : "<i class=\"fa fa-check-circle\" style=\"color: green;\"></i>" +
                                                      "&nbsp;" +
                                                      "<span style=\"color: green\">Tersedia</span>"
                                                ) +
                                                "<span class=\"checkbox-circle\"></span>" +
                                                "<span class=\"checkbox-circle-check\"></span>" +
                                                "<span class=\"checkbox-circle-icon icon\">done</span>" +
                                            "</label>" +
                                        "</div>" +
                                    "</td>" +
                                 "</tr>";
                        id++;
                    }
                }

                html += "</table>" +
                        "</div>";

                ltrJamWaktu.Text = html;
            }
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            InitFields();
            this.Master.ShowHeaderTools = false;
            mvMain.ActiveViewIndex = 1;
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

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                PerpustakaanKunjungan m = new PerpustakaanKunjungan();
                m.Kode = Guid.NewGuid();
                m.Keterangan = txtKeteranganVal.Value;
                m.Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggal.Text);
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_PerpustakaanKunjungan.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_PerpustakaanKunjungan.Insert(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    InitFields();
                    txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                PerpustakaanKunjungan m = DAO_PerpustakaanKunjungan.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        txtTanggal.Text = Libs.GetTanggalIndonesiaFromDate(m.Tanggal, false);
                        txtKeteranganVal.Value = m.Keterangan;
                        txtKeterangan.Text = m.Keterangan;
                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                    }
                }
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
            this.Master.ShowHeaderTools = true;
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

        protected void lnkOKSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] arr_jadwal = txtParseJadwal.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                Guid kode_h = Guid.NewGuid();
                DateTime dtTanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggal.Text);

                if (txtID.Value.Trim() != "")
                {
                    kode_h = new Guid(txtID.Value);
                    DAO_PerpustakaanKunjungan.Update(
                        new PerpustakaanKunjungan
                        {
                            Kode = kode_h,
                            Rel_Guru = Libs.LOGGED_USER_M.NoInduk,
                            Rel_KelasDet = QS.GetKelas(),
                            Rel_Mapel = QS.GetMapel(),
                            Semester = Libs.GetSemesterByTanggal(dtTanggal).ToString(),
                            Status = ((int)JenisStatusKunjungan.JenisStatus.Diproses).ToString(),
                            Tanggal = dtTanggal,
                            TahunAjaran = Libs.GetTahunAjaranByTanggal(dtTanggal),
                            Keterangan = txtKeteranganVal.Value
                        },
                        Libs.LOGGED_USER_M.UserID
                    );
                }
                else
                {
                    DAO_PerpustakaanKunjungan.Insert(
                        new PerpustakaanKunjungan
                        {
                            Kode = kode_h,
                            Rel_Guru = Libs.LOGGED_USER_M.NoInduk,
                            Rel_KelasDet = QS.GetKelas(),
                            Rel_Mapel = QS.GetMapel(),
                            Semester = Libs.GetSemesterByTanggal(dtTanggal).ToString(),
                            Status = ((int)JenisStatusKunjungan.JenisStatus.Diproses).ToString(),
                            Tanggal = dtTanggal,
                            TahunAjaran = Libs.GetTahunAjaranByTanggal(dtTanggal),
                            Keterangan = txtKeteranganVal.Value
                        },
                        Libs.LOGGED_USER_M.UserID
                    );
                }

                DAO_PerpustakaanKunjunganDet.DeleteByHeader(kode_h.ToString(), Libs.LOGGED_USER_M.UserID);
                foreach (string kode in arr_jadwal)
                {
                    PerpustakaanKunjunganJamSettings m_jam_setting = DAO_PerpustakaanKunjunganJamSettings.GetByID_Entity(
                            kode
                        );
                    if (m_jam_setting != null)
                    {
                        if (m_jam_setting.Waktu != null)
                        {
                            DAO_PerpustakaanKunjunganDet.Insert(
                                new PerpustakaanKunjunganDet
                                {
                                    Kode = Guid.NewGuid(),
                                    Rel_PerpustakaanKunjungan = kode_h,
                                    JamKe = m_jam_setting.JamKe,
                                    Waktu = m_jam_setting.Waktu
                                }
                                , Libs.LOGGED_USER_M.UserID
                            );
                        }
                    }                    
                }

                ShowList();
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }            
        }

        protected void btnShowListJamKunjungan_Click(object sender, EventArgs e)
        {
            ListJamKalender();
        }

        protected void btnDataKelas_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE) +
                    "?t=" + RandomLibs.GetRndTahunAjaran(QS.GetTahunAjaran()) +
                    "&ft=" + Libs.GetQueryString("ft") +
                    (
                        Libs.GetQueryString("m").Trim() != ""
                        ? "&m=" + Libs.GetQueryString("m")
                        : ""
                    ) +
                    "&kd=" + Libs.GetQueryString("kd")
                );
        }
    }
}