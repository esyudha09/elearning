using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

using AI_ERP.Application_Entities.Elearning.SMP;
using AI_ERP.Application_DAOs.Elearning.SMP;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP
{
    public partial class wf_DesainRaporLTS : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATADESAINRAPORLTSSMP";

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
                                            "src=\"" + ResolveUrl("~/Application_CLibs/images/svg/ebook-2.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Desain Rapor LTS";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitKeyEventClient();
                this.Master.txtCariData.Text = Libs.GetQ();
                InitKelasUnit();
            }
            BindListView(!IsPostBack, Libs.GetQ());
        }

        protected void InitKelasUnit()
        {
            cboKelas.Items.Clear();
            cboKelas.Items.Add("");
            Sekolah m_sekolah = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMP).FirstOrDefault();
            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {
                    List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(m_sekolah.Kode.ToString()).OrderBy(m => m.UrutanLevel).ToList();
                    foreach (Kelas m in lst_kelas)
                    {
                        if (m.IsAktif)
                        {
                            cboKelas.Items.Add(new ListItem
                            {
                                Value = m.Kode.ToString().ToLower().Trim(),
                                Text = m.Nama
                            });
                        }
                    }
                }
            }
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            txtTahunPelajaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboSemester.ClientID + "').focus(); return false; }");
            cboSemester.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboKelas.ClientID + "').focus(); return false; }");
            cboKelas.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
            cboKelas.Attributes.Add("onchange", txtKelasUnit.ClientID + ".value = this.value; return false;");

            cboMapel.Attributes.Add("onchange", "ShowNamaMapelRapor()");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectParameters.Add("JenisRapor", DAO_Rapor_Desain.JenisRapor.LTS);
                sql_ds.SelectCommand = DAO_Rapor_Desain.SP_SELECT_ALL_FOR_SEARCH_BY_JENISRAPOR;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("JenisRapor", DAO_Rapor_Desain.JenisRapor.LTS);
                sql_ds.SelectCommand = DAO_Rapor_Desain.SP_SELECT_ALL_BY_JENISRAPOR;
            }
            if (isbind) lvData.DataBind();
        }

        private void BindListViewDesainRaporLTS(string rel_desain_rapor, bool isbind = true)
        {
            sql_ds_desain_rapor_det.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_desain_rapor_det.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_desain_rapor_det.SelectParameters.Clear();
            sql_ds_desain_rapor_det.SelectParameters.Add("Rel_Rapor_Desain", rel_desain_rapor);
            sql_ds_desain_rapor_det.SelectCommand = DAO_Rapor_Desain_Det.SP_SELECT_BY_HEADER;
            if (isbind) lvListDesainRaporDet.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            System.Web.UI.WebControls.Literal imgh_sekolah = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_sekolah");
            System.Web.UI.WebControls.Literal imgh_mapel = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_mapel");
            System.Web.UI.WebControls.Literal imgh_kelas = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kelas");

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
            imgh_kelas.Text = html_image;
            imgh_mapel.Text = html_image;

            imgh_tahunajaran.Visible = false;
            imgh_sekolah.Visible = false;
            imgh_kelas.Visible = false;
            imgh_mapel.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
                    break;
                case "Sekolah":
                    imgh_sekolah.Visible = true;
                    break;
                case "Kelas":
                    imgh_kelas.Visible = true;
                    break;
                case "Mapel":
                    imgh_mapel.Visible = true;
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
                m.TahunAjaran = txtTahunPelajaran.Text;
                m.Semester = cboSemester.SelectedValue;
                m.Rel_Kelas = cboKelas.SelectedValue;
                m.JenisRapor = DAO_Rapor_Desain.JenisRapor.LTS;
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
                Rapor_Desain m = DAO_Rapor_Desain.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        txtTahunPelajaran.Text = m.TahunAjaran;
                        cboSemester.SelectedValue = m.Semester;
                        cboKelas.SelectedValue = m.Rel_Kelas.ToLower().Trim();
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
                Rapor_Desain m = DAO_Rapor_Desain.GetByID_Entity(txtID.Value.Trim());
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

                            BindListViewDesainRaporLTS(txtID.Value, true);
                            mvMain.ActiveViewIndex = 1;
                        }
                    }
                }
            }
        }

        protected void lvListDesainRaporDet_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
        }

        protected void InitInputItemRapor()
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_Desain m = DAO_Rapor_Desain.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(
                                DAO_Sekolah.GetAll_Entity().FindAll(m0 => m0.UrutanJenjang == (int)Libs.UnitSekolah.SMP).FirstOrDefault().Kode.ToString()
                            ).OrderBy(m0 => m0.Nama).ToList();

                        cboMapel.Items.Clear();
                        cboMapel.Items.Add("");
                        foreach (var mapel in lst_mapel)
                        {
                            cboMapel.Items.Add(new ListItem
                            {
                                Value = mapel.Kode.ToString(),
                                Text = mapel.Nama
                            });
                        }

                        txtNamaMapelRapor.Text = "";
                        txtNomor.Text = "";
                        txtPoin.Text = "";
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
            InitInputItemRapor();
            txtKeyAction.Value = JenisAction.DoShowInputMengajar.ToString();
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
                BindListViewDesainRaporLTS(txtID.Value, true);
                txtKeyAction.Value = JenisAction.DoDelete.ToString();
            }
        }

        protected void btnShowEditDataDesain_Click(object sender, EventArgs e)
        {
            if (txtIDItem.Value.Trim() != "")
            {
                Rapor_Desain_Det m = DAO_Rapor_Desain_Det.GetByID_Entity(txtIDItem.Value);
                if (m != null)
                {
                    if (m.Nomor != null)
                    {
                        InitInputItemRapor();
                        cboMapel.SelectedValue = m.Rel_Mapel.ToString();
                        txtNamaMapelRapor.Text = m.NamaMapelRapor;
                        txtNomor.Text = m.Nomor;
                        txtPoin.Text = m.Poin;
                        txtUrutan.Text = m.Urutan.ToString();
                        txtKeyAction.Value = JenisAction.DoShowInputMengajar.ToString();
                    }
                }
            }
        }

        protected void lnkOKItemDesain_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_Desain_Det m = new Rapor_Desain_Det();
                m.Kode = Guid.NewGuid();
                m.Rel_Rapor_Desain = new Guid(txtID.Value);
                m.Rel_Mapel = cboMapel.SelectedValue;
                m.NamaMapelRapor = txtNamaMapelRapor.Text;
                m.Nomor = txtNomor.Text;
                m.Poin = txtPoin.Text;
                m.Urutan = Libs.GetStringToInteger(txtUrutan.Text);
                m.Alias = "";
                if (txtIDItem.Value.Trim() == "")
                {
                    DAO_Rapor_Desain_Det.Insert(m);
                    BindListViewDesainRaporLTS(txtID.Value, true);
                    ShowInputDataDesain();
                }
                else
                {
                    m.Kode = new Guid(txtIDItem.Value);
                    DAO_Rapor_Desain_Det.Update(m);
                    BindListViewDesainRaporLTS(txtID.Value, true);
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
            }
        }
    }
}