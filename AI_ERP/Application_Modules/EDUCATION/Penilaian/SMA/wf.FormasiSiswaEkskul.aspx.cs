using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA
{
    public partial class wf_FormasiSiswaEkskul : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAFORMASISISWASMAEKSKUL";

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            Edit,
            Update,
            Delete,
            Search,
            ShowDataList,
            ShowDataFormasi,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowInputFormasiEkskul,
            DoShowConfirmHapus,
            DoShowConfirmHapusMengajar,
            DoShowTampilanData,
            DoTampilkanData,
            DoChangePage
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
                return Libs.GetQueryString("s");
            }

            public static string GetMapel()
            {
                return Libs.GetQueryString("m");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }
            
            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitKeyEventClient();
                ListDropdown();
                InitMapelUnit();
                txtSiswaEkskul.KodeUnit = GetUnitSekolah().Kode.ToString();
                txtSiswaEkskul.TahunAjaran = GetTahunAjaran();
                txtSiswaEkskul.Semester = GetSemester();
            }

            if (!(IsPostBack || this.Session[SessionViewDataName] == null))
            {
                dpData.SetPageProperties((int)this.Session[SessionViewDataName], dpData.MaximumRows, true);
            }
            
            BindListView(true, this.Master.txtCariData.Text);
            if (mvMain.ActiveViewIndex != 0) this.Master.ShowHeaderTools = false;
        }

        public static string GetURLNilaiEkskul(string tahun_ajaran, string semester, string rel_kelas, string rel_mapel)
        {
            List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_sn = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                    tahun_ajaran, semester, rel_kelas, rel_mapel
                );

            if (lst_sn.Count == 1)
            {
                DAO_Rapor_StrukturNilai.StrukturNilai m_sn = lst_sn.FirstOrDefault();
                if (m_sn != null)
                {
                    if (m_sn.TahunAjaran != null)
                    {
                        if (m_sn.IsNilaiAkhir)
                        {
                            return AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_EKSKUL.ROUTE;
                        }
                        else
                        {
                            return AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.ROUTE;
                        }
                    }
                }
            }

            return "";
        }

        protected bool IsByAdminUnit()
        {
            return (QS.GetUnit().Trim() != "" && QS.GetToken().Trim() != "" &&
                    DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")) ? true : false);
        }

        protected void InitMapelUnit()
        {
            cboMapel.Items.Clear();
            cboMapel.Items.Add("");

            List<Mapel> lst_mapel = DAO_Mapel.GetAll_Entity().FindAll(
                    m => m.Rel_Sekolah.ToUpper() == GetUnitSekolah().Kode.ToString().ToUpper() && 
                         (
                            m.Jenis == Application_Libs.Libs.JENIS_MAPEL.EKSKUL ||
                            m.Jenis == Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER
                         )
                );
            foreach (Mapel m in lst_mapel)
            {
                cboMapel.Items.Add(new ListItem {
                    Value = m.Kode.ToString(),
                    Text = m.Nama
                });
            }
        }

        protected Sekolah GetUnitSekolah()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMA).FirstOrDefault();
        }

        protected void ListDropdown()
        {
            cboPeriode.Items.Clear();
            foreach (var item in DAO_FormasiEkskul.GetAll_Entity().Select(
                m => new { m.TahunAjaran, m.Semester }).Distinct().OrderByDescending(m => m.TahunAjaran).ThenByDescending(m => m.Semester).ToList())
            {
                cboPeriode.Items.Add(new ListItem
                {
                    Value = item.TahunAjaran + "|" + item.Semester,
                    Text = item.TahunAjaran + " semester " + item.Semester
                });
            }
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            txtTahunPelajaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboSemester.ClientID + "').focus(); return false; }");
            cboSemester.Attributes.Add("onchange", sKeyEnter + "document.getElementById('" + cboMapel.ClientID + "').focus(); return false; }");
            cboMapel.Attributes.Add("onchange", sKeyEnter + "document.getElementById('" + txtNamaGuru.ClientID + "').focus(); return false; }");
            txtNamaGuru.Attributes.Add("onkeydown", sKeyEnter + "return false; }");
            
            txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKFormasiSiswa.ClientID + "').click(); return false; }");
        }

        protected string GetTahunAjaran()
        {
            string tahun_ajaran = "";
            
            if (!IsPostBack)
            {
                tahun_ajaran = Libs.GetTahunAjaranByTanggal(DateTime.Now);
            }
            else
            {
                if (cboPeriode.SelectedValue.Trim() != "")
                {
                    tahun_ajaran = cboPeriode.SelectedValue.Substring(0, 9);
                }
            }

            return tahun_ajaran;
        }

        protected string GetSemester()
        {
            string semester = "";

            if (!IsPostBack)
            {
                semester = Libs.GetSemesterByTanggal(DateTime.Now).ToString();
            }
            else
            {
                if (cboPeriode.SelectedValue.Trim() != "")
                {
                    semester = cboPeriode.SelectedValue.Substring(cboPeriode.SelectedValue.Length - 1, 1);
                }
            }

            return semester;
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            string tahun_ajaran = GetTahunAjaran();
            string semester = GetSemester();
            
            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                sql_ds.SelectParameters.Add("Semester", semester);
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectCommand = DAO_FormasiEkskul.SP_SELECT_ALL_BY_PERIODE_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                sql_ds.SelectParameters.Add("Semester", semester);
                sql_ds.SelectCommand = DAO_FormasiEkskul.SP_SELECT_ALL_BY_PERIODE;
            }
            if (isbind) lvData.DataBind();
        }

        private void BindListViewFormasiEkskul(string rel_formasiekskul, bool isbind = true)
        {
            sql_ds_formasi_siswa.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_formasi_siswa.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_formasi_siswa.SelectParameters.Clear();
            sql_ds_formasi_siswa.SelectParameters.Add("Rel_FormasiEkskul", rel_formasiekskul);
            sql_ds_formasi_siswa.SelectCommand = DAO_FormasiEkskulDet.SP_SELECT_BY_HEADER;
            if (isbind) lvListSiswaEkskul.DataBind();
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
            this.Session[SessionViewDataName] = e.StartRowIndex;
            txtKeyAction.Value = JenisAction.DoChangePage.ToString();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            this.Master.txtCariData.Text = "";
            BindListView(true, "");
        }

        protected void InitFields()
        {
            txtID.Value = "";
            InitMapelUnit();
            txtNamaGuru.Text = "";
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
                    DAO_FormasiEkskul.Delete(txtID.Value);
                    BindListView(true, this.Master.txtCariData.Text);
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
                FormasiEkskul m = new FormasiEkskul();
                m.Rel_Mapel = cboMapel.SelectedValue;
                m.TahunAjaran = txtTahunPelajaran.Text;
                m.Semester = cboSemester.SelectedValue;
                m.Rel_Guru = txtNamaGuru.Text;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_FormasiEkskul.Update(m);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text);
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_FormasiEkskul.Insert(m);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text);
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
                FormasiEkskul m = DAO_FormasiEkskul.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        txtNamaGuru.Text = m.Rel_Guru;
                        cboMapel.SelectedValue = m.Rel_Mapel;
                        txtTahunPelajaran.Text = m.TahunAjaran;
                        cboSemester.SelectedValue = m.Semester;
                        BindListView(true, this.Master.txtCariData.Text);
                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                    }
                }
            }
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            BindListView(true, this.Master.txtCariData.Text);
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                FormasiEkskul m = DAO_FormasiEkskul.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        string mapel = "";
                        string guru = "";

                        Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel);
                        if (m_mapel != null)
                        {
                            if (m_mapel.Nama != null)
                            {
                                mapel = m_mapel.Nama;
                            }
                        }

                        Pegawai m_guru = DAO_Pegawai.GetByID_Entity(m.Rel_Guru);
                        if (m_guru != null)
                        {
                            if (m_guru.Nama != null)
                            {
                                guru = m_guru.Nama;
                            }
                        }

                        ltrMsgConfirmHapus.Text = "Hapus<br />" +
                                                  (
                                                    mapel.Trim() != ""
                                                    ? "<i class=\"fa fa-hashtag\"></i>&nbsp;" +
                                                      "<span style=\"font-weight: bold;\">" +
                                                        mapel +
                                                      "</span><br />"
                                                    : ""
                                                  ) +
                                                  (
                                                    guru.Trim() != ""
                                                    ? "<i class=\"fa fa-hashtag\"></i>&nbsp;" + 
                                                      "<span style=\"font-weight: bold;\">" +
                                                        guru +
                                                      "</span><br />"
                                                    : ""
                                                  ) +
                                                    "<i class=\"fa fa-hashtag\"></i>&nbsp;" + 
                                                    "<span style=\"font-weight: bold;\">" +
                                                        Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                        " Semester " +
                                                        m.Semester +
                                                    "</span>";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void btnShowDataFormasi_Click(object sender, EventArgs e)
        {
            ltrCaptionFormasi.Text = "";
            FormasiEkskul m = DAO_FormasiEkskul.GetByID_Entity(txtID.Value);
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel);
                    Pegawai m_guru = DAO_Pegawai.GetByID_Entity(m.Rel_Guru.ToString());
                    string s_guru = "";
                    if (m_guru != null)
                    {
                        if (m_guru.Nama != null)
                        {
                            s_guru = m_guru.Nama;
                        }
                    }
                    if (s_guru.Trim() == "")
                    {
                        s_guru = m.Rel_Guru;
                    }

                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
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
                                             s_guru +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             m_mapel.Nama +
                                             "&nbsp;" +
                                             "</span>";

                            BindListViewFormasiEkskul(txtID.Value, true);
                            mvMain.ActiveViewIndex = 1;
                            this.Master.ShowHeaderTools = false;
                            txtKeyAction.Value = JenisAction.ShowDataFormasi.ToString();
                        }
                    }
                }
            }
        }
        
        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
            BindListView(true, this.Master.txtCariData.Text);
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

        protected void lnkOKFormasiSiswa_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                FormasiEkskulDet m = new FormasiEkskulDet();
                m.Kode = Guid.NewGuid();
                m.Rel_FormasiEkskul = txtID.Value;
                m.Rel_Siswa = txtSiswaEkskul.Value;
                m.Keterangan = txtKeterangan.Text;
                if (txtIDItem.Value.Trim() == "")
                {
                    DAO_FormasiEkskulDet.Insert(m);
                }
                else
                {
                    m.Kode = new Guid(txtIDItem.Value);
                    DAO_FormasiEkskulDet.Update(m);
                }
            }
            BindListViewFormasiEkskul(txtID.Value, true);
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void InitInputFormasiEkskul()
        {
            if (txtID.Value.Trim() != "")
            {
                FormasiEkskul m = DAO_FormasiEkskul.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {

                        txtSiswaEkskul.Value = "";
                    }
                }
            }
        }

        protected void btnShowEditFormasi_Click(object sender, EventArgs e)
        {
            if (txtIDItem.Value.Trim() != "")
            {
                FormasiEkskulDet m = DAO_FormasiEkskulDet.GetByID_Entity(txtIDItem.Value);
                if (m != null)
                {
                    if (m.Keterangan != null)
                    {
                        InitInputFormasiEkskul();
                        txtSiswaEkskul.Value = m.Rel_Siswa;
                        txtKeterangan.Text = m.Keterangan;
                        BindListViewFormasiEkskul(txtID.Value, true);
                        txtKeyAction.Value = JenisAction.DoShowInputFormasiEkskul.ToString();
                    }
                }
            }
        }

        protected void btnDoShowTampilan_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowTampilanData.ToString();
        }

        protected void lnkOKTampilanData_Click(object sender, EventArgs e)
        {
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.DoTampilkanData.ToString();
        }

        protected void btnShowInputSiswaEkskul_Click(object sender, EventArgs e)
        {
            txtSiswaEkskul.Value = "";            
            txtIDItem.Value = "";
            txtKeterangan.Text = "";
            InitInputFormasiEkskul();
            txtKeyAction.Value = JenisAction.DoShowInputFormasiEkskul.ToString();
        }

        protected void lvListSiswaEkskul_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void lvListSiswaEkskul_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void lnkOKHapusItemFormasi_Click(object sender, EventArgs e)
        {
            if (txtSelItem.Value.Trim() != "")
            {
                string[] arr_item = txtSelItem.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr_item)
                {
                    DAO_FormasiEkskulDet.Delete(item);
                }
                txtSelItem.Value = "";
                BindListViewFormasiEkskul(txtID.Value, true);
                txtKeyAction.Value = JenisAction.DoDelete.ToString();
            }
        }
    }
}