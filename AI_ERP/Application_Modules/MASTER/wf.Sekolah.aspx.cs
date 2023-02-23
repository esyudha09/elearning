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
    public partial class wf_Sekolah : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATASEKOLAH";

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

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/school.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Unit Sekolah";

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

        private void InitInput()
        {
            cboDivisi.Items.Clear();
            cboDivisi.Items.Add("");
            List<Divisi> lst_divisi = DAO_Divisi.GetAll_Entity().OrderBy(m => m.Nama).ToList();
            foreach (Divisi m in lst_divisi)
            {
                cboDivisi.Items.Add(new ListItem
                {
                    Value = m.Kode.ToString(),
                    Text = m.Nama
                });
            }

            cboUrutanJenjang.Items.Clear();
            cboUrutanJenjang.Items.Add("");
            for (int i = 1; i <= 10; i++)
            {
                cboUrutanJenjang.Items.Add(new ListItem {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            cboDivisi.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtNama.ClientID + "').focus(); return false; }");
            txtNama.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboUrutanJenjang.ClientID + "').focus(); return false; }");
            cboUrutanJenjang.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtEmail.ClientID + "').focus(); return false; }");
            txtEmail.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtWebsite.ClientID + "').focus(); return false; }");
            txtWebsite.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKeterangan.ClientID + "').focus(); return false; }");
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
                sql_ds.SelectCommand = DAO_Sekolah.SP_SELECT_ALL_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommand = DAO_Sekolah.SP_SELECT_ALL;
            }
            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_divisi = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_divisi");
            System.Web.UI.WebControls.Literal imgh_nama = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_nama");
            System.Web.UI.WebControls.Literal imgh_urutanjenjang = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_urutanjenjang");
            System.Web.UI.WebControls.Literal imgh_email = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_email");
            System.Web.UI.WebControls.Literal imgh_website = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_website");
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

            imgh_divisi.Text = html_image;
            imgh_nama.Text = html_image;
            imgh_urutanjenjang.Text = html_image;
            imgh_email.Text = html_image;
            imgh_website.Text = html_image;
            imgh_keterangan.Text = html_image;

            imgh_divisi.Visible = false;
            imgh_nama.Visible = false;
            imgh_urutanjenjang.Visible = false;
            imgh_email.Visible = false;
            imgh_website.Visible = false;
            imgh_keterangan.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "Divisi":
                    imgh_divisi.Visible = true;
                    break;
                case "Nama":
                    imgh_nama.Visible = true;
                    break;
                case "UrutanJenjang":
                    imgh_urutanjenjang.Visible = true;
                    break;
                case "Email":
                    imgh_email.Visible = true;
                    break;
                case "Website":
                    imgh_website.Visible = true;
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
            txtNama.Text = "";
            txtEmail.Text = "";
            txtWebsite.Text = "";
            txtKeterangan.Text = "";

            InitInput();
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
                    DAO_Sekolah.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
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
                Sekolah m = new Sekolah();
                m.Rel_Divisi = new Guid(cboDivisi.SelectedValue);
                m.Nama = txtNama.Text;
                m.UrutanJenjang = int.Parse(cboUrutanJenjang.SelectedValue);
                m.Email = txtEmail.Text;
                m.Website = txtWebsite.Text;
                m.Keterangan = txtKeterangan.Text;

                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_Sekolah.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_Sekolah.Insert(m, Libs.LOGGED_USER_M.UserID);
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
                Sekolah m = DAO_Sekolah.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Nama != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        cboDivisi.SelectedValue = m.Rel_Divisi.ToString();
                        txtNama.Text = m.Nama;
                        cboUrutanJenjang.SelectedValue = m.UrutanJenjang.ToString();
                        txtEmail.Text = m.Email;
                        txtWebsite.Text = m.Website;
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
                Sekolah m = DAO_Sekolah.GetByID_Entity(txtID.Value.Trim());
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