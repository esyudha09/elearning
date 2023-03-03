using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

namespace AI_ERP.Application_Modules.CBT
{
    public partial class wf_MapelCBT : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAMAPEL";

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
            DoChangePage,
            DoShowConfirmHapus
        }

        private static class QS
        {
            public static string GetUnit()
            {
                if (Libs.GetQueryString("u").Trim() != "")
                {
                    return Libs.GetQueryString("u");
                }
                else
                {
                    return Libs.GetQueryString("unit");
                }
            }

            public static string GetToken()
            {
                return Libs.GetQueryString("token");
            }

            public static string GetKelas()
            {
                return Libs.GetQueryString("k");
            }

            public static string GetTahunAjaran()
            {
                string t = Libs.GetQueryString("t");
                return RandomLibs.GetParseTahunAjaran(t);
            }

            public static string GetURLVariable()
            {
                string s_url_var = "";
                s_url_var += (QS.GetUnit().Trim() != "" ? "unit=" + QS.GetUnit().Trim() : "");
                s_url_var += (s_url_var.Trim() != "" && QS.GetToken().Trim() != "" ? "&" : "") + (QS.GetToken().Trim() != "" ? "token=" : "") + QS.GetToken().Trim();

                return (
                            QS.GetUnit().Trim() != "" || QS.GetToken().Trim() != ""
                        ? "?" : "") +
                        s_url_var;
            }
        }

        protected bool IsByAdminUnit()
        {
            return (QS.GetUnit().Trim() != "" && QS.GetToken().Trim() != "" &&
                    DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")) ? true : false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            //{
            //    Libs.RedirectToLogin(this.Page);
            //}
            //if (!DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")))
            //{
            //    Libs.RedirectToBeranda(this.Page);
            //}

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/document.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Data Mata Pelajaran";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitInput();
                InitKeyEventClient();
            }       
            BindListView(!IsPostBack, this.Master.txtCariData.Text);
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            cboUnit.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtNama.ClientID + "').focus(); return false; }");
            txtNama.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtAlias.ClientID + "').focus(); return false; }");
            txtAlias.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboJenis.ClientID + "').focus(); return false; }");
            cboJenis.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKeterangan.ClientID + "').focus(); return false; }");
            txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        private void InitInput()
        {
            cboUnit.Items.Clear();
            cboUnit.Items.Add("");
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            div_input_filter_unit.Visible = true;
            if (IsByAdminUnit())
            {
                cboUnit.Items.Clear();
                lst_sekolah = lst_sekolah.FindAll(m => m.Kode.ToString().ToUpper().Trim() == QS.GetUnit().ToUpper().Trim()).ToList();
                div_input_filter_unit.Visible = false;
            }
            foreach (Sekolah m in lst_sekolah)
            {
                cboUnit.Items.Add(new ListItem
                {
                    Value = m.Kode.ToString().ToUpper(),
                    Text = m.Nama
                });
            }

            Libs.JENIS_MAPEL.ListToDropdown(cboJenis, true, QS.GetUnit());
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;


            //Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(Libs.LOGGED_USER_M.NoInduk);
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("Rel_Guru", Libs.LOGGED_USER_M.NoInduk);
                sql_ds.SelectParameters.Add("TahunAjaran", Libs.GetTahunAjaranNow());
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectCommand = DAO_CBT_BankSoal.SP_SELECT_BY_GURU_BY_TA_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("Rel_Guru", Libs.LOGGED_USER_M.NoInduk);
                sql_ds.SelectParameters.Add("TahunAjaran", Libs.GetTahunAjaranNow());
                sql_ds.SelectCommand = DAO_CBT_BankSoal.SP_SELECT_BY_GURU_BY_TA;
            }

            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_unit = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_unit");
            System.Web.UI.WebControls.Literal imgh_nama = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_nama");
            System.Web.UI.WebControls.Literal imgh_alias = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_alias");
            System.Web.UI.WebControls.Literal imgh_jenis = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_jenis");
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
            imgh_alias.Text = html_image;
            imgh_jenis.Text = html_image;
            imgh_keterangan.Text = html_image;

            imgh_unit.Visible = false;
            imgh_nama.Visible = false;
            imgh_alias.Visible = false;
            imgh_jenis.Visible = false;
            imgh_keterangan.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "Unit":
                    imgh_unit.Visible = true;
                    break;
                case "Nama":
                    imgh_nama.Visible = true;
                    break;
                case "Alias":
                    imgh_alias.Visible = true;
                    break;
                case "Jenis":
                    imgh_jenis.Visible = true;
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
            this.Session[SessionViewDataName] = e.StartRowIndex;
            txtKeyAction.Value = JenisAction.DoChangePage.ToString();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Master.txtCariData.Text = "";
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, "");
        }

        protected void InitFields()
        {
            txtID.Value = "";
            cboUnit.SelectedValue = (QS.GetUnit().Trim().ToUpper() != "" ? QS.GetUnit().Trim().ToUpper() : "");
            txtNama.Text = "";
            txtAlias.Text = "";
            cboJenis.SelectedValue = "";
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
                    DAO_Mapel.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text);
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
                Mapel m = new Mapel();
                m.Rel_Sekolah = cboUnit.SelectedValue;
                m.Nama = txtNama.Text;
                m.Alias = txtAlias.Text;
                m.Jenis = cboJenis.SelectedValue;
                m.Keterangan = txtKeterangan.Text;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_Mapel.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text.Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_Mapel.Insert(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text.Trim());
                    InitFields();
                    txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnRumahSoal_Click(object sender, EventArgs e)
        {
            string[] commandArgs = (sender as LinkButton).CommandArgument.ToString().Split(new char[] { ',' });
            string mapel = commandArgs[0];
            string unit = commandArgs[1];
           
            if (unit == "SMA")
            {
                Response.Redirect(Routing.URL.APPLIACTION_MODULES.CBT.RUMAH_SOAL_SMA.ROUTE + "?m=" + mapel + "&u=" + unit);
            }
            else if (unit == "SMP")
            {
                Response.Redirect(Routing.URL.APPLIACTION_MODULES.CBT.RUMAH_SOAL_SMP.ROUTE + "?m=" + mapel + "&u=" + unit);
            }

        }
        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Mapel m = DAO_Mapel.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Nama != null)
                    {
                        cboUnit.SelectedValue = m.Rel_Sekolah.Trim().ToUpper();
                        txtID.Value = m.Kode.ToString();
                        txtNama.Text = m.Nama;
                        txtAlias.Text = m.Alias;
                        Libs.JENIS_MAPEL.SelectDropdown(cboJenis, m.Jenis);
                        txtKeterangan.Text = m.Keterangan;
                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                    }
                }
            }
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, this.Master.txtCariData.Text);
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Mapel m = DAO_Mapel.GetByID_Entity(txtID.Value.Trim());
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