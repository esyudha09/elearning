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
    public partial class wf_PengaturanJadwalKunjungan : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PENGATURANJADWALKUNJUNGANPERPUS";

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

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/test-2.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Pengaturan Kunjungan Perpustakaan";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitDropDown();
                InitKeyEventClient();
            }
            BindListView(!IsPostBack, Libs.GetQ());
            if (!IsPostBack) this.Master.txtCariData.Text = Libs.GetQ();
        }

        protected void InitDropDown()
        {
            cboUnit.Items.Clear();
            cboUnit.Items.Add(new ListItem { Value = Constantas.GUID_NOL, Text = "Pilih Unit Sekolah" });
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            foreach (Sekolah m in lst_sekolah)
            {
                cboUnit.Items.Add(new ListItem {
                    Value = m.Kode.ToString(),
                    Text = m.Nama
                });
            }

            cboJamKe.Items.Clear();
            cboJamKe.Items.Add("");
            for (int i = 1; i <= 10; i++)
            {
                cboJamKe.Items.Add(new ListItem {
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
            cboJamKe.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtWaktu.ClientID + "').focus(); return false; }");
            txtWaktu.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds.SelectParameters.Clear();
            sql_ds.SelectParameters.Add("Rel_Sekolah", cboUnit.SelectedValue);
            sql_ds.SelectCommand = DAO_PerpustakaanKunjunganJamSettings.SP_SELECT_BY_SEKOLAH;
            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_jamke = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_jamke");
            System.Web.UI.WebControls.Literal imgh_waktu = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_waktu");
            
            string html_image = "";
            if (e.SortDirection == SortDirection.Ascending)
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-up\" style=\"color: white;\"></i>";
            }
            else
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-down\" style=\"color: white;\"></i>";
            }

            imgh_jamke.Text = html_image;
            imgh_waktu.Text = html_image;
            
            imgh_jamke.Visible = false;
            imgh_waktu.Visible = false;
            
            switch (e.SortExpression.ToString().Trim())
            {
                case "JamKe":
                    imgh_jamke.Visible = true;
                    break;
                case "Waktu":
                    imgh_waktu.Visible = true;
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
            BindListView(true);
        }

        protected void InitFields()
        {
            txtID.Value = "";
            cboJamKe.SelectedValue = "";
            txtWaktu.Text = "";
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
                    DAO_PerpustakaanKunjunganJamSettings.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
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
                PerpustakaanKunjunganJamSettings m = new PerpustakaanKunjunganJamSettings();
                m.Rel_Sekolah = new Guid(cboUnit.SelectedValue);
                m.JamKe = cboJamKe.SelectedValue;
                m.Waktu = txtWaktu.Text;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_PerpustakaanKunjunganJamSettings.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_PerpustakaanKunjunganJamSettings.Insert(m, Libs.LOGGED_USER_M.UserID);
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
                PerpustakaanKunjunganJamSettings m = DAO_PerpustakaanKunjunganJamSettings.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Waktu != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        cboJamKe.SelectedValue = m.JamKe;
                        txtWaktu.Text = m.Waktu;
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
                PerpustakaanKunjunganJamSettings m = DAO_PerpustakaanKunjunganJamSettings.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Waktu != null)
                    {
                        ltrMsgConfirmHapus.Text = "Hapus " +
                                                    "Jam Ke : <span style=\"font-weight: bold;\">\"" + Libs.GetHTMLSimpleText(m.JamKe) + "\"</span><br />" +
                                                    "Waktu : <span style=\"font-weight: bold;\">\"" + Libs.GetHTMLSimpleText(m.Waktu) + "\"</span>" +
                                                  "?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void cboUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindListView(true);
        }
    }
}