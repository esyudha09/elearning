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
    public partial class wf_VolunteerSettings : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAVOLUNTEERSETTINGSSD";

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
            DoShowInputVolunteerSettings,
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
                                            "src=\"" + ResolveUrl("~/Application_CLibs/images/svg/check-box.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Pengaturan Volunteer";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitKeyEventClient();
                this.Master.txtCariData.Text = Libs.GetQ();
            }
            BindListView(!IsPostBack, Libs.GetQ());
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            txtTahunPelajaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboSemester.ClientID + "').focus(); return false; }");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectCommand = DAO_Rapor_Volunteer_Settings.SP_SELECT_ALL_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommand = DAO_Rapor_Volunteer_Settings.SP_SELECT_ALL;
            }
            if (isbind) lvData.DataBind();
        }

        private void BindListViewVolunteer(string rel_desain_rapor, bool isbind = true)
        {
            sql_ds_volunteer_det.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_volunteer_det.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_volunteer_det.SelectParameters.Clear();
            sql_ds_volunteer_det.SelectParameters.Add("Rel_Rapor_Volunteer_Settings", rel_desain_rapor);
            sql_ds_volunteer_det.SelectCommand = DAO_Rapor_Volunteer_Settings_Det.SP_SELECT_BY_HEADER;
            if (isbind) lvListVolunteer.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            
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
            
            imgh_tahunajaran.Visible = false;
            
            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
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
                    DAO_Rapor_Volunteer_Settings.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
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
                Rapor_Volunteer_Settings m = new Rapor_Volunteer_Settings();
                m.TahunAjaran = txtTahunPelajaran.Text;
                m.Semester = cboSemester.SelectedValue;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_Rapor_Volunteer_Settings.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    m.Kode = Guid.NewGuid();
                    DAO_Rapor_Volunteer_Settings.Insert(m, Libs.LOGGED_USER_M.UserID);
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
                Rapor_Volunteer_Settings m = DAO_Rapor_Volunteer_Settings.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        txtTahunPelajaran.Text = m.TahunAjaran;
                        cboSemester.SelectedValue = m.Semester;
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
                Rapor_Volunteer_Settings m = DAO_Rapor_Volunteer_Settings.GetByID_Entity(txtID.Value.Trim());
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
            Rapor_Volunteer_Settings m = DAO_Rapor_Volunteer_Settings.GetByID_Entity(txtID.Value);
            if (m != null)
            {
                if (m.TahunAjaran != null)
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
                                             "</span>";

                    BindListViewVolunteer(txtID.Value, true);
                    mvMain.ActiveViewIndex = 1;
                }
            }
        }
        
        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
        }

        protected void InitInputItemVolunteer()
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_Volunteer_Settings m = DAO_Rapor_Volunteer_Settings.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(
                                DAO_Sekolah.GetAll_Entity().FindAll(m0 => m0.UrutanJenjang == (int)Libs.UnitSekolah.SD).FirstOrDefault().Kode.ToString()
                            ).OrderBy(m0 => m0.Nama).ToList();

                        cboMapel.Items.Clear();
                        cboMapel.Items.Add("");
                        foreach (var mapel in lst_mapel)
                        {
                            if (DAO_Mapel.GetJenisMapel(mapel.Kode.ToString()) == Libs.JENIS_MAPEL.VOLUNTEER)
                            {
                                cboMapel.Items.Add(new ListItem
                                {
                                    Value = mapel.Kode.ToString(),
                                    Text = mapel.Nama
                                });
                            }
                        }

                        txtDurasi.Text = "";
                        txtUrutan.Text = "";
                    }
                }
            }
        }

        protected void btnShowInputDataDesain_Click(object sender, EventArgs e)
        {
            ShowInputDataDesain();
        }

        protected void ShowInputDataDesain()
        {
            txtIDItem.Value = "";
            InitInputItemVolunteer();
            txtKeyAction.Value = JenisAction.DoShowInputVolunteerSettings.ToString();
        }

        protected void lnkOKHapusItemDesain_Click(object sender, EventArgs e)
        {
            if (txtSelItem.Value.Trim() != "")
            {
                string[] arr_item = txtSelItem.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr_item)
                {
                    DAO_Rapor_Volunteer_Settings_Det.Delete(item, Libs.LOGGED_USER_M.UserID);
                }
                txtSelItem.Value = "";
                BindListViewVolunteer(txtID.Value, true);
                txtKeyAction.Value = JenisAction.DoDelete.ToString();
            }
        }

        protected void btnShowEditDataDesain_Click(object sender, EventArgs e)
        {
            if (txtIDItem.Value.Trim() != "")
            {
                Rapor_Volunteer_Settings_Det m = DAO_Rapor_Volunteer_Settings_Det.GetByID_Entity(txtIDItem.Value);
                if (m != null)
                {
                    if (m.Rel_Rapor_Volunteer_Settings != null)
                    {
                        InitInputItemVolunteer();
                        cboMapel.SelectedValue = m.Rel_Mapel.ToString();
                        txtDurasi.Text = m.Durasi.ToString();
                        txtUrutan.Text = m.Urutan.ToString();
                        txtKeyAction.Value = JenisAction.DoShowInputVolunteerSettings.ToString();
                    }
                }
            }
        }

        protected void lnkOKItemDesain_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_Volunteer_Settings_Det m = new Rapor_Volunteer_Settings_Det();
                m.Kode = Guid.NewGuid();
                m.Rel_Rapor_Volunteer_Settings = new Guid(txtID.Value);
                m.Rel_Mapel = cboMapel.SelectedValue;
                m.Durasi = Libs.GetStringToDecimal(txtDurasi.Text);
                m.Urutan = Libs.GetStringToInteger(txtUrutan.Text);
                if (txtIDItem.Value.Trim() == "")
                {
                    DAO_Rapor_Volunteer_Settings_Det.Insert(m, Libs.LOGGED_USER_M.UserID);
                    BindListViewVolunteer(txtID.Value, true);
                    ShowInputDataDesain();
                }
                else
                {
                    m.Kode = new Guid(txtIDItem.Value);
                    DAO_Rapor_Volunteer_Settings_Det.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListViewVolunteer(txtID.Value, true);
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
            }
        }
    }
}