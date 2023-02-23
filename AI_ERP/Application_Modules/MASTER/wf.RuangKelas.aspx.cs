using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

namespace AI_ERP.Application_Modules.MASTER
{
    public partial class wf_RuangKelas : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATARUANGKELAS";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/university-1.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Data Ruang Kelas";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitInput();
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
            cboUnit.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtNama.ClientID + "').focus(); return false; }");
            txtNama.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKeterangan.ClientID + "').focus(); return false; }");
            txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        private void InitInput()
        {
            cboUnit.Items.Clear();
            cboUnit.Items.Add("");
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            foreach (Sekolah m in lst_sekolah)
            {
                cboUnit.Items.Add(new ListItem
                {
                    Value = m.Kode.ToString(),
                    Text = m.Nama
                });
            }
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectCommand = DAO_RuangKelas.SP_SELECT_ALL_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommand = DAO_RuangKelas.SP_SELECT_ALL;
            }
            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_unit = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_unit");
            System.Web.UI.WebControls.Literal imgh_nama = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_nama");
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

            imgh_unit.Text = html_image;
            imgh_nama.Text = html_image;
            imgh_keterangan.Text = html_image;

            imgh_unit.Visible = false;
            imgh_nama.Visible = false;
            imgh_keterangan.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "Unit":
                    imgh_unit.Visible = true;
                    break;
                case "Nama":
                    imgh_nama.Visible = true;
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
            cboUnit.SelectedValue = "";
            txtNama.Text = "";
            txtKeterangan.Text = "";
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
                    DAO_RuangKelas.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
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
                RuangKelas m = new RuangKelas();
                m.Rel_Sekolah = cboUnit.SelectedValue;
                m.Nama = txtNama.Text;
                m.Keterangan = txtKeterangan.Text;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_RuangKelas.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_RuangKelas.Insert(m, Libs.LOGGED_USER_M.UserID);
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
                RuangKelas m = DAO_RuangKelas.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Nama != null)
                    {
                        cboUnit.SelectedValue = m.Rel_Sekolah;
                        txtID.Value = m.Kode.ToString();
                        txtNama.Text = m.Nama;
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
                RuangKelas m = DAO_RuangKelas.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Nama != null)
                    {
                        ltrMsgConfirmHapus.Text = "Hapus <span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetHTMLSimpleText(m.Nama) +
                                                      "\"</span>?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }
    }
}