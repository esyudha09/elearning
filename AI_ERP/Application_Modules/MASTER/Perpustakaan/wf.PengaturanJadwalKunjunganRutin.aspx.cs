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

namespace AI_ERP.Application_Modules.MASTER.Perpustakaan
{
    public partial class wf_PengaturanJadwalKunjunganRutin : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAJADWALKUNJUNGANRUTIN";

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
            DoShowInputJadwal,
            DoShowConfirmHapus,
            DoShowConfirmHapusJadwal
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" " +
                                            "src=\"" + ResolveUrl("~/Application_CLibs/images/svg/093-calendar-1.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Pengaturan Kunjungan Rutin";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitKeyEventClient();
                this.Master.txtCariData.Text = Libs.GetQ();
                ListDropdown();
                InitKelasUnit();
                InitMapelUnit();
            }
            BindListView(!IsPostBack, Libs.GetQ());
        }

        protected void InitKelasUnit()
        {
            txtParseKelasUnit.Value = "";
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            foreach (Sekolah m_sekolah in lst_sekolah)
            {
                txtParseKelasUnit.Value += m_sekolah.Kode.ToString() + "->";
                txtParseKelasUnit.Value += "|;";
                List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(m_sekolah.Kode.ToString());
                foreach (Kelas m in lst_kelas)
                {
                    txtParseKelasUnit.Value += m_sekolah.Kode.ToString() + "->";
                    txtParseKelasUnit.Value += m.Kode.ToString() +
                                               "|" +
                                               m.Nama +
                                               ";";
                }
            }
        }

        protected void InitMapelUnit()
        {
            txtMapelUnit.Value = "";
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            foreach (Sekolah m_sekolah in lst_sekolah)
            {
                txtParseMapelUnit.Value += m_sekolah.Kode.ToString() + "->";
                txtParseMapelUnit.Value += "|;";
                List<Mapel> lst_mapel = DAO_Mapel.GetAll_Entity().FindAll(m => m.Rel_Sekolah.ToUpper() == m_sekolah.Kode.ToString().ToUpper());
                foreach (Mapel m in lst_mapel)
                {
                    txtParseMapelUnit.Value += m_sekolah.Kode.ToString() + "->";
                    txtParseMapelUnit.Value += m.Kode.ToString() +
                                               "|" +
                                               m.Nama +
                                               ";";
                }
            }
        }

        protected void ListDropdown()
        {
            cboJadwalHari.Items.Clear();
            cboJadwalHari.Items.Add("");
            int id = 0;
            foreach (string hari in Libs.Array_Hari_Kerja)
            {
                cboJadwalHari.Items.Add(new ListItem {
                    Value = Libs.Array_Hari_Kerja_Id[id],
                    Text = hari
                });
                id++;
            }

            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            cboUnitSekolah.Items.Clear();
            cboUnitSekolah.Items.Add("");
            foreach (Sekolah m in lst_sekolah)
            {
                cboUnitSekolah.Items.Add(new ListItem
                {
                    Value = m.Kode.ToString(),
                    Text = m.Nama
                });
            }
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            txtTahunPelajaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboUnitSekolah.ClientID + "').focus(); return false; }");
            cboUnitSekolah.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKeterangan.ClientID + "').focus(); return false; }");            
            txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectCommand = DAO_PerpustakaanKunjunganRutin.SP_SELECT_ALL_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommand = DAO_PerpustakaanKunjunganRutin.SP_SELECT_ALL;
            }
            if (isbind) lvData.DataBind();
        }

        private void BindListViewPengaturan(string rel_header, bool isbind = true)
        {
            sql_ds_pengaturan.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds_pengaturan.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_pengaturan.SelectParameters.Clear();
            sql_ds_pengaturan.SelectParameters.Add("Rel_Header", rel_header);
            sql_ds_pengaturan.SelectCommand = DAO_PerpustakaanKunjunganRutinDet.SP_SELECT_BY_HEADER;
            if (isbind) lvListPengaturan.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            System.Web.UI.WebControls.Literal imgh_sekolah = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_sekolah");
            System.Web.UI.WebControls.Literal imgh_keterangan = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_keterangan");
            
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
            imgh_sekolah.Text = html_image;
            imgh_keterangan.Text = html_image;

            imgh_tahunajaran.Visible = false;
            imgh_sekolah.Visible = false;
            imgh_keterangan.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
                    break;
                case "Sekolah":
                    imgh_sekolah.Visible = true;
                    break;
                case "Keterangan":
                    imgh_keterangan.Visible = true;
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
            txtKelasUnit.Value = "";
            txtMapelUnit.Value = "";
            txtTahunPelajaran.Text = "";
            cboUnitSekolah.SelectedValue = "";
            chkSemester1.Checked = true;
            chkSemester2.Checked = true;
            txtKeterangan.Text = "";
            if (cboUnitSekolah.Items.Count > 0) cboUnitSekolah.SelectedIndex = 0;
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            InitFields();
            txtKeyAction.Value = JenisAction.Add.ToString();
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_PerpustakaanKunjunganRutin.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
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
                PerpustakaanKunjunganRutin m = new PerpustakaanKunjunganRutin();
                m.Rel_Sekolah = cboUnitSekolah.SelectedValue;
                m.TahunAjaran = txtTahunPelajaran.Text;
                m.IsSemester_1 = (chkSemester1.Checked ? true : false);
                m.IsSemester_2 = (chkSemester2.Checked ? true : false);
                m.Keterangan = txtKeterangan.Text;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_PerpustakaanKunjunganRutin.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_PerpustakaanKunjunganRutin.Insert(m, Libs.LOGGED_USER_M.UserID);
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
                PerpustakaanKunjunganRutin m = DAO_PerpustakaanKunjunganRutin.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        txtTahunPelajaran.Text = m.TahunAjaran;
                        cboUnitSekolah.SelectedValue = m.Rel_Sekolah.ToString();
                        chkSemester1.Checked = m.IsSemester_1;
                        chkSemester2.Checked = m.IsSemester_2;
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
                PerpustakaanKunjunganRutin m = DAO_PerpustakaanKunjunganRutin.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m.Rel_Sekolah.ToString());
                        if (m_sekolah != null)
                        {
                            if (m_sekolah.Nama != null)
                            {

                                ltrMsgConfirmHapus.Text = "Hapus <span style=\"font-weight: bold;\">\"" +
                                                                Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                                " Unit : " +
                                                                m_sekolah.Nama +
                                                          "\"</span>?";
                                txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();

                            }
                        }
                    }
                }
            }
        }

        protected void lvListPengaturan_Sorting(object sender, ListViewSortEventArgs e)
        {
            
        }

        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
        }

        protected void btnShowInputJadwal_Click(object sender, EventArgs e)
        {
            txtIDItem.Value = "";
            cboJadwalHari.SelectedValue = "";
            cboKelas.SelectedValue = "";
            txtKeteranganJadwalHari.Text = "";
            txtJamJadwalHari.Text = "";
            txtKeyAction.Value = JenisAction.DoShowInputJadwal.ToString();
        }

        protected void lnkOKHapusItemJadwal_Click(object sender, EventArgs e)
        {
            if (txtIDItem.Value.Trim() != "")
            {
                DAO_PerpustakaanKunjunganRutinDet.Delete(txtIDItem.Value, Libs.LOGGED_USER_M.UserID);
                txtIDItem.Value = "";
                BindListViewPengaturan(txtID.Value, true);
                txtKeyAction.Value = JenisAction.DoDelete.ToString();
            }
        }

        protected void btnShowPengaturan_Click(object sender, EventArgs e)
        {
            cboKelas.Items.Clear();

            ltrCaptionFormasi.Text = "";
            PerpustakaanKunjunganRutin m = DAO_PerpustakaanKunjunganRutin.GetByID_Entity(txtID.Value);
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m.Rel_Sekolah.ToString());
                    if (m_sekolah != null)
                    {
                        if (m_sekolah.Nama != null)
                        {
                            cboKelas.Items.Add("");
                            List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(m_sekolah.Kode.ToString()).OrderBy(m0 => m0.UrutanLevel).ToList();
                            foreach (Kelas kelas in lst_kelas)
                            {
                                List<KelasDet> lst_kelas_det = DAO_KelasDet.GetByKelas_Entity(kelas.Kode.ToString()).OrderBy(m0 => m0.UrutanKelas).ToList();
                                foreach (KelasDet m_kelasdet in lst_kelas_det)
                                {
                                    if (m_kelasdet.IsAktif)
                                    {
                                        cboKelas.Items.Add(new ListItem
                                        {
                                            Value = m_kelasdet.Kode.ToString(),
                                            Text = m_kelasdet.Nama
                                        });
                                    }
                                }
                            }
                            
                            ltrCaptionFormasi.Text =
                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             m.TahunAjaran +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             (
                                                m.IsSemester_1 || m.IsSemester_2
                                                ? "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                    "&nbsp;" +
                                                    "Sm." +
                                                    "&nbsp;" +
                                                    (
                                                        m.IsSemester_1 ? "1" : ""
                                                    ) +
                                                    (
                                                        m.IsSemester_1 && m.IsSemester_2
                                                        ? " & "
                                                        : ""
                                                    ) +
                                                    (
                                                        m.IsSemester_2 ? "2" : ""
                                                    ) +
                                                  "</span>"
                                                : ""
                                             ) +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             m_sekolah.Nama +
                                             "&nbsp;" +
                                             "</span>";

                            BindListViewPengaturan(txtID.Value, true);
                            mvMain.ActiveViewIndex = 1;
                        }
                    }
                }
            }
        }

        protected void lvListPengaturan_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void lnkOKJadwalRutin_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                PerpustakaanKunjunganRutinDet m = new PerpustakaanKunjunganRutinDet();
                m.Kode = Guid.NewGuid();
                m.Rel_PerpustakaanKunjunganRutin = new Guid(txtID.Value);
                m.Rel_KelasDet = new Guid(cboKelas.SelectedValue);
                m.Hari = Convert.ToInt16(cboJadwalHari.SelectedValue);
                m.Waktu = txtJamJadwalHari.Text;
                m.Keterangan = txtKeteranganJadwalHari.Text;
                if (txtIDItem.Value.Trim() == "")
                {
                    DAO_PerpustakaanKunjunganRutinDet.Insert(m, Libs.LOGGED_USER_M.UserID);
                }
                else
                {
                    m.Kode = new Guid(txtIDItem.Value);
                    DAO_PerpustakaanKunjunganRutinDet.Update(m, Libs.LOGGED_USER_M.UserID);
                }
            }
            BindListViewPengaturan(txtID.Value, true);
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void btnShowDetailItem_Click(object sender, EventArgs e)
        {
            if (txtIDItem.Value.Trim() != "")
            {
                PerpustakaanKunjunganRutinDet m = DAO_PerpustakaanKunjunganRutinDet.GetByID_Entity(txtIDItem.Value);
                if (m != null)
                {
                    if (m.Waktu != null)
                    {
                        cboJadwalHari.SelectedValue = m.Hari.ToString();
                        cboKelas.SelectedValue = m.Rel_KelasDet.ToString();
                        txtKeteranganJadwalHari.Text = m.Keterangan;
                        txtJamJadwalHari.Text = m.Waktu;
                        txtKeyAction.Value = JenisAction.DoShowInputJadwal.ToString();
                    }
                }
            }
        }

        protected void btnShowConfirmDeleteItem_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                PerpustakaanKunjunganRutinDet m = DAO_PerpustakaanKunjunganRutinDet.GetByID_Entity(txtIDItem.Value.Trim());
                if (m != null)
                {
                    if (m.Waktu != null)
                    {

                        ltrMsgConfirmHapus.Text = "Hapus Jadwal <span style=\"font-weight: bold;\">\"" +
                                                        Libs.GetNamaHariFromUrutHari(m.Hari) +
                                                        " Jam : " +
                                                        m.Waktu +
                                                  "\"</span>?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapusJadwal.ToString();

                    }
                }
            }
        }

        public static string GetGuruFromKode(string kode_item_jadwal)
        {
            DAO_PerpustakaanKunjunganRutinDet.GuruKelas m = DAO_PerpustakaanKunjunganRutinDet.GetGuruKelasByID_Entity(
                    kode_item_jadwal
                ).FirstOrDefault();

            if (m != null)
            {
                if (m.Rel_GuruKelas != null)
                {
                    string hasil = "";

                    hasil += m.NamaGuruKelas +
                             (
                                m.NamaGuruKelas.Trim() != "" && m.NamaGuruPendamping.Trim() != "" &&
                                m.NamaGuruKelas.Trim() != m.NamaGuruPendamping.Trim()
                                ? "<br />"
                                : ""
                             ) +
                             (
                                m.NamaGuruKelas.Trim() != m.NamaGuruPendamping.Trim()
                                ? m.NamaGuruPendamping
                                : ""
                             );

                    return hasil;
                }
            }

            return "";
        }
    }
}