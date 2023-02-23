using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
{
    public partial class wf_LTS : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATARAPORLTS";

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            Edit,
            Update,
            Delete,
            Search,
            DoAdd,
            DoAddDetail,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowInputMengajar,
            DoShowConfirmHapus,
            DoShowConfirmHapusMengajar
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" " +
                                            "src=\"" + ResolveUrl("~/Application_CLibs/images/svg/doc.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Rapor LTS";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitKeyEventClient();
                this.Master.txtCariData.Text = Libs.GetQ();
                InitInput();
            }
            BindListView(!IsPostBack, Libs.GetQ());
        }

        protected void InitInput()
        {
            cboKelas.Items.Clear();
            cboKelas.Items.Add(new ListItem { Value = "", Text = "" });
            cboKelas.Items.Add(new ListItem { Value = "-", Text = "(Semua)" });
            Sekolah m_sekolah = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SD).FirstOrDefault();
            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {
                    List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(m_sekolah.Kode.ToString()).OrderBy(m => m.UrutanLevel).ToList();
                    foreach (Kelas m in lst_kelas)
                    {
                        if (m.IsAktif)
                        {

                            List<KelasDet> lst_kelas_det = DAO_KelasDet.GetByKelas_Entity(m.Kode.ToString()).OrderBy(m0 => m0.UrutanKelas).ToList();
                            foreach (KelasDet kelas_det in lst_kelas_det)
                            {
                                if (kelas_det.IsAktif)
                                {
                                    cboKelas.Items.Add(new ListItem
                                    {
                                        Value = kelas_det.Kode.ToString(),
                                        Text = kelas_det.Nama
                                    });
                                }
                            }
                        }
                    }
                }
            }

            cboTahunPelajaran.Items.Clear();
            foreach (var item in DAO_Rapor_StrukturNilai.GetDistinctTahunAjaran_Entity())
            {
                cboTahunPelajaran.Items.Add(new ListItem {
                    Value = item,
                    Text = item
                });
            }

            cboSemester.SelectedValue = Libs.GetSemesterByTanggal(DateTime.Now).ToString();
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            cboTahunPelajaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboSemester.ClientID + "').focus(); return false; }");
            cboSemester.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboKelas.ClientID + "').focus(); return false; }");
            cboKelas.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
            cboKelas.Attributes.Add("onchange", txtKelasUnit.ClientID + ".value = this.value; return false;");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectCommand = DAO_Rapor_LTS.SP_SELECT_ALL_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommand = DAO_Rapor_LTS.SP_SELECT_ALL;
            }
            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            System.Web.UI.WebControls.Literal imgh_sekolah = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_sekolah");
            System.Web.UI.WebControls.Literal imgh_kelas_det = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kelasdet");

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
            imgh_kelas_det.Text = html_image;
            
            imgh_tahunajaran.Visible = false;
            imgh_sekolah.Visible = false;
            imgh_kelas_det.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
                    break;
                case "Sekolah":
                    imgh_sekolah.Visible = true;
                    break;
                case "KelasDet":
                    imgh_kelas_det.Visible = true;
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
            cboKelas.SelectedValue = "";
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
                    DAO_Rapor_Desain.Delete(txtID.Value);
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
                Rapor_Desain m = new Rapor_Desain();
                m.TahunAjaran = cboTahunPelajaran.Text;
                m.Semester = cboSemester.SelectedValue;
                m.Rel_Kelas = txtKelasUnit.Value;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_Rapor_Desain.Update(m);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    m.Kode = Guid.NewGuid();
                    DAO_Rapor_Desain.Insert(m);
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
                FormasiGuruMapel m = DAO_FormasiGuruMapel.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        txtMapelUnit.Value = m.Rel_Mapel;
                        cboTahunPelajaran.Text = m.TahunAjaran;
                        cboSemester.SelectedValue = m.Semester;
                        txtKelasUnit.Value = m.Rel_Kelas;
                        cboKelas.SelectedValue = m.Rel_Kelas;
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
                FormasiGuruMapel m = DAO_FormasiGuruMapel.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        ltrMsgConfirmHapus.Text = "Hapus <span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                            " Semester " +
                                                            m.Semester +
                                                      "\"</span>?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void btnShowDataDesain_Click(object sender, EventArgs e)
        {
            ltrCaptionFormasi.Text = "";
            Rapor_Desain m = DAO_Rapor_Desain.GetByID_Entity(txtID.Value);
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas);
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            ltrCaptionFormasi.Text =
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
                                             "Kelas&nbsp;" +
                                             m_kelas.Nama +
                                             "&nbsp;" +
                                             "</span>";
                            
                            mvMain.ActiveViewIndex = 1;
                        }
                    }
                }
            }
        }

        protected void lvListGuruMengajar_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_rel_guru = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_rel_guru");
            System.Web.UI.WebControls.Literal imgh_guru = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_guru");
            System.Web.UI.WebControls.Literal imgh_kelasdet = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kelasdet");
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

            imgh_rel_guru.Text = html_image;
            imgh_guru.Text = html_image;
            imgh_keterangan.Text = html_image;
            imgh_kelasdet.Text = html_image;

            imgh_rel_guru.Visible = false;
            imgh_guru.Visible = false;
            imgh_keterangan.Visible = false;
            imgh_kelasdet.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "Rel_Guru":
                    imgh_rel_guru.Visible = true;
                    break;
                case "Guru":
                    imgh_guru.Visible = true;
                    break;
                case "KelasDet":
                    imgh_kelasdet.Visible = true;
                    break;
                case "Keterangan":
                    imgh_keterangan.Visible = true;
                    break;
            }
        }

        protected void lvListGuruMengajar_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
        }

        protected void lnkOKHapusItemDesain_Click(object sender, EventArgs e)
        {
            if (txtSelItem.Value.Trim() != "")
            {
                string[] arr_item = txtSelItem.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr_item)
                {
                    DAO_Rapor_Desain_Det.Delete(item);
                }
                txtSelItem.Value = "";
                txtKeyAction.Value = JenisAction.DoDelete.ToString();
            }
        }
    }
}