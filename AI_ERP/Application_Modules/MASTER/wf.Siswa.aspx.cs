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
    public partial class wf_Siswa : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATASISWA";
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
            DoShowTampilkanPilihan,
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
                //Libs.RedirectToBeranda(this.Page);
            }
            this.Master.ShowHeaderSubTitle = false;
            this.Master.HeaderTittle = "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/student.svg") + "\" " +
                                            "style=\"margin: 0 auto; height: 40px; width: 40px; margin-top: -10px; margin-right: 5px; float: left;\" />" +
                                       "&nbsp;&nbsp;" +
                                       "Data Siswa";
            this.Master.ShowHeaderTools = true;
            
            if (!IsPostBack)
            {
                ListDropdown();
                InitKelasUnit();
                InitInput();                
                string sKeyEnter = "if(event.keyCode == 13){";
                this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
                this.Master.txtCariData.Text = Libs.GetQ();
            }
            BindListView(!IsPostBack, Libs.GetQ());
        }
        
        protected void InitInput()
        {
            string unit = "";
            unit = cboTampilanDataUnit.ClientID + ".value";
            if (IsByAdminUnit())
            {
                unit = "'" + QS.GetUnit() + "'";
            }
        }

        protected void InitKelasUnit()
        {
            txtParseKelasUnit.Value = "";
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            if (IsByAdminUnit())
            {
                lst_sekolah = lst_sekolah.FindAll(m => m.Kode.ToString().ToUpper().Trim() == QS.GetUnit().ToUpper().Trim()).ToList();
            }
            foreach (Sekolah m_sekolah in lst_sekolah)
            {
                List<KelasDet> lst_kelas = DAO_KelasDet.GetBySekolah_Entity(m_sekolah.Kode.ToString());
                foreach (KelasDet m in lst_kelas)
                {
                    if (m.IsAktif)
                    {
                        txtParseKelasUnit.Value += m_sekolah.Kode.ToString() + "->";
                        txtParseKelasUnit.Value += m.Kode.ToString() +
                                                   "|" +
                                                   m.Nama +
                                                   ";";
                    }
                }
            }

            cboTampilanDataUnit.Attributes["onchange"] = "ShowKelasByUnit(this.value);";
        }

        protected bool IsByAdminUnit()
        {
            return (QS.GetUnit().Trim() != "" && QS.GetToken().Trim() != "" &&
                    DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")) ? true : false);
        }

        protected void ListDropdown()
        {
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            div_input_filter_unit.Visible = true;
            if (IsByAdminUnit())
            {
                lst_sekolah = lst_sekolah.FindAll(m => m.Kode.ToString().ToUpper().Trim() == QS.GetUnit().ToUpper().Trim()).ToList();
                div_input_filter_unit.Visible = false;
            }

            cboTampilanDataUnit.Items.Clear();
            cboTampilanDataUnit.Items.Add(new ListItem
            {
                Value = "",
                Text = "(Semua)"
            });
            foreach (var m_sekolah in lst_sekolah)
            {
                cboTampilanDataUnit.Items.Add(new ListItem {
                    Value = m_sekolah.Kode.ToString(),
                    Selected = (QS.GetUnit() == m_sekolah.Kode.ToString() ? true : false),
                    Text = m_sekolah.Nama
                });
            }

            List<string> lst_tahun_ajaran = Libs.ListTahunAjaran().OrderByDescending(m => m).ToList();
            cboTampilanDataTahunAjaran.Items.Clear();
            cboTampilanDataTahunAjaran.Items.Add(new ListItem
            {
                Value = "",
                Text = "(Semua)"
            });
            foreach (string tahun_ajaran in lst_tahun_ajaran)
            {
                if (tahun_ajaran.IndexOf("/") >= 0)
                {
                    cboTampilanDataTahunAjaran.Items.Add(new ListItem
                    {
                        Value = RandomLibs.GetRndTahunAjaran(tahun_ajaran),
                        Selected = (QS.GetTahunAjaran() == tahun_ajaran ? true : (tahun_ajaran == Libs.GetTahunAjaranNow() && QS.GetTahunAjaran().Trim() == "" ? true : false)),
                        Text = tahun_ajaran
                    });
                }
            }

            ShowKelasByUnit(cboTampilanDataUnit.SelectedValue);
            Libs.SelectDropdownListByValue(cboTampilanDataSemester, Libs.GetSemesterByTanggal(DateTime.Now).ToString());
        }

        protected void ShowKelasByUnit(string kode_unit)
        {
            List<KelasDet> lst_kelas = DAO_KelasDet.GetBySekolah_Entity(kode_unit);
            cboKelasBiodataSiswa.Items.Clear();
            cboKelasBiodataSiswa.Items.Add(new ListItem {
                Value = "",
                Text = "(Semua)"
            });
            foreach (KelasDet m in lst_kelas)
            {
                if (m.IsAktif)
                {
                    string jenis_kelas = "Kelas Perwalian";
                    if (m.IsKelasJurusan)
                    {
                        jenis_kelas = "Kelas Jurusan";
                    }
                    else if (m.IsKelasSosialisasi)
                    {
                        jenis_kelas = "Kelas Sosialisasi";
                    }
                    cboKelasBiodataSiswa.Items.Add(new ListItem
                    {
                        Value = m.Kode.ToString(),
                        Selected = (QS.GetKelas().Trim().ToUpper() == m.Kode.ToString().ToUpper() ? true : false),
                        Text = jenis_kelas +
                               HttpUtility.HtmlDecode("&nbsp;&nbsp;&rarr;&nbsp;&nbsp;") + 
                               m.Nama
                    });
                }
            }
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

            public static string GetSemester()
            {
                string s = Libs.GetQueryString("s");
                if (s.Trim() == "") return Libs.GetSemesterByTanggal(DateTime.Now).ToString();
                return Libs.GetQueryString("s");
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

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            if (IsByAdminUnit())
            {
                if (cboKelasBiodataSiswa.SelectedValue.Trim() == "")
                {
                    if (keyword.Trim() != "")
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("nama", keyword);
                        sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                        sql_ds.SelectParameters.Add("TahunAjaran", (cboTampilanDataTahunAjaran.SelectedValue.Trim() == "" ? Libs.GetTahunAjaranNow() : cboTampilanDataTahunAjaran.SelectedItem.Text));
                        sql_ds.SelectParameters.Add("Semester", (cboTampilanDataSemester.SelectedValue.Trim() == "" ? Libs.GetSemesterByTanggal(DateTime.Now).ToString() : cboTampilanDataSemester.SelectedItem.Text));
                        sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL_BY_UNIT_FOR_SEARCH;
                    }
                    else
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                        sql_ds.SelectParameters.Add("TahunAjaran", (cboTampilanDataTahunAjaran.SelectedValue.Trim() == "" ? Libs.GetTahunAjaranNow() : cboTampilanDataTahunAjaran.SelectedItem.Text));
                        sql_ds.SelectParameters.Add("Semester", (cboTampilanDataSemester.SelectedValue.Trim() == "" ? Libs.GetSemesterByTanggal(DateTime.Now).ToString() : cboTampilanDataSemester.SelectedItem.Text));
                        sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL_BY_UNIT;
                    }
                }
                else
                {
                    if (keyword.Trim() != "")
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("nama", keyword);
                        sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                        sql_ds.SelectParameters.Add("Rel_KelasDet", cboKelasBiodataSiswa.SelectedValue);
                        sql_ds.SelectParameters.Add("TahunAjaran", (cboTampilanDataTahunAjaran.SelectedValue.Trim() == "" ? Libs.GetTahunAjaranNow() : cboTampilanDataTahunAjaran.SelectedItem.Text));
                        sql_ds.SelectParameters.Add("Semester", (cboTampilanDataSemester.SelectedValue.Trim() == "" ? Libs.GetSemesterByTanggal(DateTime.Now).ToString() : cboTampilanDataSemester.SelectedItem.Text));
                        sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL_BY_UNIT_BY_KELAS_FOR_SEARCH;
                    }
                    else
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                        sql_ds.SelectParameters.Add("Rel_KelasDet", cboKelasBiodataSiswa.SelectedValue);
                        sql_ds.SelectParameters.Add("TahunAjaran", (cboTampilanDataTahunAjaran.SelectedValue.Trim() == "" ? Libs.GetTahunAjaranNow() : cboTampilanDataTahunAjaran.SelectedItem.Text));
                        sql_ds.SelectParameters.Add("Semester", (cboTampilanDataSemester.SelectedValue.Trim() == "" ? Libs.GetSemesterByTanggal(DateTime.Now).ToString() : cboTampilanDataSemester.SelectedItem.Text));
                        sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL_BY_UNIT_BY_KELAS;
                    }
                }
            }
            else
            {
                if (Libs.GetQueryString("id") == "b6Y7887bjhdr")
                {
                    if (keyword.Trim() != "")
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("nama", keyword);
                        sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL__FOR_SEARCH;
                    }
                    else
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL_;
                    }
                }
                else
                {
                    if (keyword.Trim() != "")
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("nama", keyword);
                        sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                        sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
                        sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                        sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
                        sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL_FOR_SEARCH;
                    }
                    else
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                        sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
                        sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                        sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
                        sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL;
                    }
                }
            }
            if (isbind) lvData.DataBind();
        }

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    Routing.URL.APPLIACTION_MODULES.MASTER.SISWA_INPUT.ROUTE +
                    QS.GetURLVariable() +
                    (QS.GetURLVariable().Trim() != "" ? "&" : "?") + "sw=" + txtID.Value +
                    (QS.GetURLVariable().Trim() != "" && cboTampilanDataTahunAjaran.SelectedValue.Trim() != "" ? "&t=" + cboTampilanDataTahunAjaran.SelectedValue : "") +
                    (QS.GetURLVariable().Trim() != "" && cboTampilanDataSemester.SelectedValue.Trim() != "" ? "&s=" + cboTampilanDataSemester.SelectedValue : "") +
                    (QS.GetURLVariable().Trim() != "" && cboKelasBiodataSiswa.SelectedValue.Trim() != "" ? "&k=" + cboKelasBiodataSiswa.SelectedValue : "") +
                    (QS.GetURLVariable().Trim() != "" && this.Master.txtCariData.Text.Trim() != "" ? "&q=" + this.Master.txtCariData.Text : "")
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

        protected void btnTampilkanData_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowTampilkanPilihan.ToString();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Master.txtCariData.Text = "";
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, "");
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, this.Master.txtCariData.Text);
        }

        protected void lnkOKShowBiodataSiswa_Click(object sender, EventArgs e)
        {
            this.Master.txtCariData.Text = "";
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, "");
        }
    }
}