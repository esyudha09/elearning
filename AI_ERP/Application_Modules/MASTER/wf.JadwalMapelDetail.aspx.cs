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
    public partial class wf_DetailJadwalMapel : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAJADWALMAPEL";

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
            DoShowConfirmHapus,
            DoShowEditJadwal
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
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }
            if (!DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")))
            {
                Libs.RedirectToBeranda(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/document.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Data Jadwal Mata Pelajaran";

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
            //cboUnit.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtNama.ClientID + "').focus(); return false; }");
            //txtNama.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtAlias.ClientID + "').focus(); return false; }");
            txtAlias.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboJenis.ClientID + "').focus(); return false; }");
            cboJenis.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKeterangan.ClientID + "').focus(); return false; }");
            txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName] = e.StartRowIndex;
            txtKeyAction.Value = JenisAction.DoChangePage.ToString();
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

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, this.Master.txtCariData.Text);
        }

        protected void InitFields()
        {
            txtID.Value = "";
            cboUnit.SelectedValue = (QS.GetUnit().Trim().ToUpper() != "" ? QS.GetUnit().Trim().ToUpper() : "");
            // txtNama.Text = "";
            txtAlias.Text = "";
            cboJenis.SelectedValue = "";
            txtKeterangan.Text = "";
        }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                Mapel m = new Mapel();
                m.Rel_Sekolah = cboUnit.SelectedValue;
                // m.Nama = txtNama.Text;
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

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            InitFields();
            txtKeyAction.Value = JenisAction.Add.ToString();
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

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Master.txtCariData.Text = "";
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, "");
        }

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowEditJadwal.ToString();
        }

        protected void btnShowDetailJadwal_Click(object sender, EventArgs e)
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

        private void BindListView(bool isbind = true, string keyword = "")
        {
            ltrDetailJadwal.Text = GetHTMLDetailJadwal();
        }

        protected string GetHTMLDetailJadwal()
        {
            string s_kelas_det = "";
            List<KelasDet> lst_kelas_det = DAO_KelasDet.GetBySekolah_Entity(DAO_Pegawai.GetByID_Entity(Libs.LOGGED_USER_M.NoInduk).Rel_Unit).FindAll(m0 => m0.IsAktif == true && m0.IsKelasJurusan == false && m0.IsKelasSosialisasi == false);
            foreach (var item in lst_kelas_det)
            {
                s_kelas_det += "<th style=\"background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle; color: white;\">" +
                                    item.Nama +
                                "</th>";
            }

            string s_detail = "<tr style=\"background-color: white;\">" +
                                "<td>" +
                                "</td>" +
                                "<td>" +
                                    //hari
                                "</td>" +
                                "<td>" +
                                    //pukul
                                "</td>" +
                                "<td>" +
                                    //jam
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "teks" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "1" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "2" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "3" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "4" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "5" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "6" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "7" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "8" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "9" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "10" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "11" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "12" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "13" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "14" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "15" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "16" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "17" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "18" +
                                    "</div>" +
                                "</td>" +
                                "<td>" +
                                    "<div onclick=\"" + btnShowDetail.ClientID + ".click();\" style=\" text-align: center;\">" +
                                        "19" +
                                    "</div>" +
                                "</td>" +
                            "</tr>";

            string s_html = "<table class=\"table\" id=\"itemPlaceholderContainer\" runat=\"server\" style=\"width: 100%; margin: 0px;\">" +
								"<thead>" +
								   "<tr style=\"background-color: #3367d6;\">" +
                                        "<th style=\"text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;\">" +
                                            "<i class=\"fa fa-cog\"></i>" +
                                        "</th>" +
                                        "<th style=\"background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle; color: white;\">" +
                                            "Hari" +
									    "</th>" +
                                        "<th style=\"background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle; color: white;\">" +
                                            "Pukul" +
									    "</th>" +
                                        "<th style=\"background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle; color: white;\">" +
                                            "Jam" +
									    "</th>" + 
                                        s_kelas_det +
                                   "</tr>" +
                                "</thead>" +
                                "<tbody>" +
                                    s_detail +
                                "</tbody>" +
                            "</table>";


            return s_html;
        }
    }
}