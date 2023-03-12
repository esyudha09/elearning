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
    public partial class wf_BankSoal : System.Web.UI.Page
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
                                       "Data Bank Soal";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                //InitInput();
                InitKeyEventClient();
                id_login.Value = Libs.LOGGED_USER_M.UserID;
            }
            BindListView(!IsPostBack, this.Master.txtCariData.Text);
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            //cboUnit.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtNama.ClientID + "').focus(); return false; }");
            //txtSoal.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtSoal.ClientID + "').focus(); return false; }");
            //txtJwbEssay.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtSoal.ClientID + "').focus(); return false; }");
            //cboJenis.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKeterangan.ClientID + "').focus(); return false; }");
            //txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        //private void InitInput()
        //{
        //    cboUnit.Items.Clear();
        //    cboUnit.Items.Add("");
        //    List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
        //    div_input_filter_unit.Visible = true;
        //    if (IsByAdminUnit())
        //    {
        //        cboUnit.Items.Clear();
        //        lst_sekolah = lst_sekolah.FindAll(m => m.Kode.ToString().ToUpper().Trim() == QS.GetUnit().ToUpper().Trim()).ToList();
        //        div_input_filter_unit.Visible = false;
        //    }
        //    foreach (Sekolah m in lst_sekolah)
        //    {
        //        cboUnit.Items.Add(new ListItem
        //        {
        //            Value = m.Kode.ToString().ToUpper(),
        //            Text = m.Nama
        //        });
        //    }

        //    Libs.JENIS_MAPEL.ListToDropdown(cboJenis, true, QS.GetUnit());
        //}

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;


            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectParameters.Add("Rel_Mapel", Libs.GetQueryString("m"));
                sql_ds.SelectParameters.Add("Rel_Kelas", Libs.GetQueryString("k"));
                sql_ds.SelectCommand = DAO_CBT_BankSoal.SP_SELECT_ALL_BY_MAPEL_BY_KELAS_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("Rel_Mapel", Libs.GetQueryString("m"));
                sql_ds.SelectParameters.Add("Rel_Kelas", Libs.GetQueryString("k"));
                sql_ds.SelectCommand = DAO_CBT_BankSoal.SP_SELECT_ALL_BY_MAPEL_BY_KELAS;
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

        

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            //InitFields();
            //txtKeyAction.Value = JenisAction.Add.ToString();
            var mapel = Libs.GetQueryString("m");
            var kelas = Libs.GetQueryString("k");
            Response.Redirect(ResolveUrl(Routing.URL.APPLIACTION_MODULES.CBT.SOAL_INPUT.ROUTE + "?m=" + mapel + "&k="+ kelas));
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_CBT_BankSoal.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text);
                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        //protected void lnkOKInput_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        CBT_BankSoal m = new CBT_BankSoal();
        //        m.Rel_Mapel = Libs.GetQueryString("m");
        //        m.Rel_Guru = Libs.LOGGED_USER_M.NoInduk;
        //        m.Soal = txtSoal.Text;
        //        m.JwbEssay = txtJwbEssay.Text;
        //        m.JwbGanda1 = txtJwbGanda1.Text;
        //        m.JwbGanda2 = txtJwbGanda2.Text;
        //        m.JwbGanda3 = txtJwbGanda3.Text;
        //        m.JwbGanda4 = txtJwbGanda4.Text;
        //        m.JwbGanda5 = txtJwbGanda5.Text;
        //        if (chkEssay.Checked)
        //        {
        //            m.Jenis = "essay";
        //        }
        //        else if (chkGanda.Checked)
        //        {
        //            m.Jenis = "pilihan ganda";
        //        }

        //        if (txtID.Value.Trim() != "")
        //        {
        //            m.Kode = new Guid(txtID.Value);
        //            DAO_CBT_BankSoal.Update(m, Libs.LOGGED_USER_M.UserID);
        //            BindListView(!IsPostBack, this.Master.txtCariData.Text.Trim());
        //            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        //        }
        //        else
        //        {
        //            DAO_CBT_BankSoal.Insert(m, Libs.LOGGED_USER_M.UserID);
        //            BindListView(!IsPostBack, this.Master.txtCariData.Text.Trim());
        //            InitFields();
        //            txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        txtKeyAction.Value = ex.Message;
        //    }
        //}

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            var mapel = Libs.GetQueryString("m");
            var kelas = Libs.GetQueryString("k");
            Response.Redirect(ResolveUrl(Routing.URL.APPLIACTION_MODULES.CBT.SOAL_INPUT.ROUTE + "?id=" + txtID.Value + "&m=" + mapel + "&k=" + kelas));
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
                CBT_BankSoal m = DAO_CBT_BankSoal.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Soal != null)
                    {
                        ltrMsgConfirmHapus.Text = "Hapus <span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetHTMLSimpleText(m.Soal) +
                                                      "\"</span>?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void btnBackToMapel_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var kp = Libs.GetQueryString("kp");
            var kur = Libs.GetQueryString("kur");
            var unit = Libs.GetQueryString("u");
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.MAPEL.ROUTE // + "?&m=" + m + "&kp=" + kp + "&kur=" + kur + "&u=" + unit
                        )
                );
        }

        protected void btnBackToKelas_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var unit = Libs.GetQueryString("u");
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.STRUKTUR_PENILAIAN_SMA.ROUTE + "?&m=" + m 
                        )
                );
        }
    }
}