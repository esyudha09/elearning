using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Modules.MASTER
{
    public partial class wf_Pegawai : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAPEGAWAI";
        public const string C_ID = "{{id}}";

        public enum JenisAction
        {
            Add,
            Edit,
            AddItemKelas,
            EditItemKelas,
            Update,
            Delete,
            DeleteItemKelas,
            Search,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoChangePage,
            DoShowConfirmHapus,
            ItemKelasDetKosong
        }

        protected void btnBackToMenu_Click(object sender, EventArgs e)
        {

        }

        protected void btnSaveData_Click(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }
            if (!DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")))
            {
                Libs.RedirectToBeranda(this.Page);
            }

            this.Master.ShowHeaderSubTitle = false;
            this.Master.HeaderTittle = "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/student-3.svg") + "\" " +
                                            "style=\"margin: 0 auto; height: 40px; width: 40px; margin-top: -10px; margin-right: 5px; float: left;\" />" +
                                       "&nbsp;&nbsp;" +
                                       "Data Pegawai";
            this.Master.ShowHeaderTools = true;

            if (!IsPostBack)
            {
                InitKeyEventClient();
            }
            BindListView(!IsPostBack, this.Master.txtCariData.Text);
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");            
        }

        private static class QS
        {
            public static string GetUnit()
            {
                return Libs.GetQueryString("unit");
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
                s_url_var += (s_url_var.Trim() != "" && Libs.GetQueryString("q").Trim() != "" ? "&" : "") + (Libs.GetQueryString("q").Trim() != "" ? "q=" : "") + Libs.GetQueryString("q").Trim();

                return (QS.GetUnit().Trim() != "" || QS.GetToken().Trim() != "" || Libs.GetQueryString("q").Trim() != "" ? "?" : "") +
                        s_url_var;
            }
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            string rel_unit = QS.GetUnit();
            if (rel_unit.Trim() != "")
            {
                if (keyword.Trim() != "")
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("Rel_Unit", rel_unit);
                    sql_ds.SelectParameters.Add("nama", keyword);
                    sql_ds.SelectCommand = DAO_Pegawai.SP_SELECT_ALL_BY_UNIT_FOR_SEARCH;
                }
                else
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("Rel_Unit", rel_unit);
                    sql_ds.SelectCommand = DAO_Pegawai.SP_SELECT_ALL_BY_UNIT;
                }
            }
            else
            {
                if (keyword.Trim() != "")
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("nama", keyword);
                    sql_ds.SelectCommand = DAO_Pegawai.SP_SELECT_ALL_FOR_SEARCH;
                }
                else
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectCommand = DAO_Pegawai.SP_SELECT_ALL;
                }
            }
            
            if (isbind) lvData.DataBind();
        }

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    Routing.URL.APPLIACTION_MODULES.MASTER.PEGAWAI_INPUT.ROUTE +
                    QS.GetURLVariable().Trim() +
                    (
                        QS.GetURLVariable().Trim() != ""
                        ? "&"
                        : "?"
                    ) +
                    "p=" + txtID.Value
                );
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {

        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName] = e.StartRowIndex;
            txtKeyAction.Value = JenisAction.DoChangePage.ToString();
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Master.txtCariData.Text = "";
            Response.Redirect(
                    Libs.FILE_PAGE_URL +
                    QS.GetURLVariable()
                );
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(!IsPostBack, this.Master.txtCariData.Text);
        }
    }
}