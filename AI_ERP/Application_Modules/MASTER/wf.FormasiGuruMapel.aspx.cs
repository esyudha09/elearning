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
    public partial class wf_FormasiGuruMapel : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAFORMASIGURUMAPEL";

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            AddFormasiSiswaWithMessage,
            Edit,
            Update,
            Delete,
            Search,
            ShowDataList,
            ShowDataMengajar,
            ShowDataFormasiSiswa,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowInputMengajar,
            DoShowInputSiswaMapel,
            DoShowConfirmHapus,
            DoShowConfirmHapusMengajar,
            DoShowTampilanData,
            DoShowPilihSiswa,
            DoOKShowPilihSiswa,
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
            if (!DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")))
            {
                //Libs.RedirectToBeranda(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/network.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Formasi Guru Mata Pelajaran";
            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitKeyEventClient();
                ListDropdown();
                InitKelasUnit();
                InitMapelUnit();
            }

            if (!(IsPostBack || this.Session[SessionViewDataName] == null))
            {
                dpData.SetPageProperties((int)this.Session[SessionViewDataName], dpData.MaximumRows, true);
            }

            BindListView(true, this.Master.txtCariData.Text);
            this.Master.ShowHeaderTools = (mvMain.ActiveViewIndex == 0 ? true : false);
        }

        protected bool IsByAdminUnit()
        {
            return (QS.GetUnit().Trim() != "" && QS.GetToken().Trim() != "" &&
                    DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")) ? true : false);
        }

        protected void InitKelasUnit()
        {
            txtParseKelasUnit.Value = "";
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            foreach (Sekolah m_sekolah in lst_sekolah)
            {
                txtParseKelasUnit.Value += m_sekolah.Kode.ToString() + "->";
                txtParseKelasUnit.Value += "|;";
                List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(m_sekolah.Kode.ToString());
                foreach (Kelas m in lst_kelas)
                {
                    txtParseKelasUnit.Value += m_sekolah.Kode.ToString() + "->";
                    txtParseKelasUnit.Value += m.Kode.ToString() +
                                               "|" +
                                               m.Nama +
                                               ";";
                }
            }
        }

        public static bool IsShowDetail(string kode)
        {
            FormasiGuruMapel m = DAO_FormasiGuruMapel.GetByID_Entity(kode);
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m.Rel_Sekolah.ToString());
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());
                    if (m_sekolah != null)
                    {
                        if (m_sekolah.Nama != null)
                        {
                            if (
                                (
                                    (Libs.UnitSekolah)m_sekolah.UrutanJenjang == Libs.UnitSekolah.SMP ||
                                    (Libs.UnitSekolah)m_sekolah.UrutanJenjang == Libs.UnitSekolah.SMA
                                ) &&
                                DAO_Mapel.GetJenisMapelByJenis(m_mapel.Jenis) == Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER
                            )
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        protected void InitMapelUnit()
        {
            txtMapelUnit.Value = "";
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            foreach (Sekolah m_sekolah in lst_sekolah)
            {
                txtParseMapelUnit.Value += m_sekolah.Kode.ToString() + "->";
                txtParseMapelUnit.Value += "|;";
                List<Mapel> lst_mapel = DAO_Mapel.GetAll_Entity().FindAll(m => m.Rel_Sekolah.ToUpper() == m_sekolah.Kode.ToString().ToUpper());
                foreach (Mapel m in lst_mapel)
                {
                    if (
                        (
                            (
                                (Libs.UnitSekolah)m_sekolah.UrutanJenjang == Libs.UnitSekolah.SMP ||
                                (Libs.UnitSekolah)m_sekolah.UrutanJenjang == Libs.UnitSekolah.SMA
                            ) &&
                            DAO_Mapel.GetJenisMapelByJenis(m.Jenis) != Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER
                        ) || 
                        !(
                            (Libs.UnitSekolah)m_sekolah.UrutanJenjang == Libs.UnitSekolah.SMP ||
                            (Libs.UnitSekolah)m_sekolah.UrutanJenjang == Libs.UnitSekolah.SMA
                        )
                    )
                    {
                        txtParseMapelUnit.Value += m_sekolah.Kode.ToString() + "->";
                        txtParseMapelUnit.Value += m.Kode.ToString() +
                                                   "|" +
                                                   m.Nama +
                                                   ";";
                    }
                }
            }
        }

        protected void ListDropdown()
        {
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            cboUnitSekolah.Items.Clear();
            cboUnitSekolah.Items.Add("");
            cboUnit.Items.Clear();
            cboUnit.Items.Add(new ListItem { Value = "-", Text = "Semua" });
            if (IsByAdminUnit())
            {
                cboUnitSekolah.Items.Clear();
                cboUnit.Items.Clear();
                lst_sekolah = lst_sekolah.FindAll(m => m.Kode.ToString().ToUpper().Trim() == QS.GetUnit().ToUpper().Trim()).ToList();
            }
            foreach (Sekolah m in lst_sekolah)
            {
                cboUnitSekolah.Items.Add(new ListItem
                {
                    Value = m.Kode.ToString(),
                    Text = m.Nama
                });
                cboUnit.Items.Add(new ListItem
                {
                    Value = m.Kode.ToString(),
                    Text = m.Nama
                });
            }

            cboPeriode.Items.Clear();
            foreach (var item in DAO_FormasiGuruKelas.GetAll_Entity().Select(
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
            cboSemester.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboUnitSekolah.ClientID + "').focus(); return false; }");
            cboUnitSekolah.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboMapel.ClientID + "').focus(); return false; }");
            cboMapel.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboKelas.ClientID + "').focus(); return false; }");
            cboKelas.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
            cboKelas.Attributes.Add("onchange", txtKelasUnit.ClientID + ".value = this.value; return false;");
            cboMapel.Attributes.Add("onchange", txtMapelUnit.ClientID + ".value = this.value; return false;");
            cboUnitSekolah.Attributes["onchange"] = "ShowKelasByUnit(this.value); ShowMapelByUnit(this.value);";

            txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKGuruMengajar.ClientID + "').click(); return false; }");

            lnkOKPilihSiswa.Attributes.Add(
                    "onclick",
                    "if(!ValidateCheckedSiswa()) { ShowSBMessage('Data siswa belum dipilih'); return false; } " +
                    txtParsePilihSiswa.ClientID + ".value = GetCheckedSiswa(); "
                );
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            string tahun_ajaran = "";
            string semester = "";
            if (!IsPostBack)
            {
                tahun_ajaran = Libs.GetTahunAjaranByTanggal(DateTime.Now);
                semester = Libs.GetSemesterByTanggal(DateTime.Now).ToString();
            }
            else
            {
                if (cboPeriode.SelectedValue.Trim() != "")
                {
                    tahun_ajaran = cboPeriode.SelectedValue.Substring(0, 9);
                    semester = cboPeriode.SelectedValue.Substring(cboPeriode.SelectedValue.Length - 1, 1);
                }
            }

            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                sql_ds.SelectParameters.Add("Semester", semester);
                sql_ds.SelectParameters.Add("Rel_Sekolah", cboUnit.SelectedValue);
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectCommand = DAO_FormasiGuruMapel.SP_SELECT_ALL_BY_PERIODE_BY_UNIT_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                sql_ds.SelectParameters.Add("Semester", semester);
                sql_ds.SelectParameters.Add("Rel_Sekolah", cboUnit.SelectedValue);
                sql_ds.SelectCommand = DAO_FormasiGuruMapel.SP_SELECT_ALL_BY_PERIODE_BY_UNIT;
            }
            if (isbind) lvData.DataBind();
        }

        private void BindListViewFormasiMengajar(string rel_formasigurumengajar, bool isbind = true)
        {
            sql_ds_formasi_guru.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds_formasi_guru.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_formasi_guru.SelectParameters.Clear();
            sql_ds_formasi_guru.SelectParameters.Add("Rel_FormasiGuruMapel", rel_formasigurumengajar);
            sql_ds_formasi_guru.SelectCommand = DAO_FormasiGuruMapelDet.SP_SELECT_BY_HEADER;
            if (isbind) lvListGuruMengajar.DataBind();
        }

        private void BindListViewFormasiSiswa(string rel_formasigurumengajar, bool isbind = true)
        {
            sql_ds_formasi_siswa.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds_formasi_siswa.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_formasi_siswa.SelectParameters.Clear();
            sql_ds_formasi_siswa.SelectParameters.Add("Rel_FormasiGuruMapel", rel_formasigurumengajar);
            sql_ds_formasi_siswa.SelectCommand = DAO_FormasiGuruMapelDetSiswa.SP_SELECT_BY_HEADER;
            if (isbind) lvListSiswaMapel.DataBind();
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

            int pageindex = int.Parse(Math.Ceiling(Convert.ToDecimal(dpData.StartRowIndex / 100)).ToString());
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
            txtKelasUnit.Value = "";
            txtMapelUnit.Value = "";
            if (cboUnitSekolah.Items.Count > 0) cboUnitSekolah.SelectedIndex = 0;
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
                    DAO_FormasiGuruMapel.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
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
                FormasiGuruMapel m = new FormasiGuruMapel();
                m.Rel_Sekolah = new Guid(cboUnitSekolah.SelectedValue);
                m.Rel_Mapel = txtMapelUnit.Value;
                m.TahunAjaran = txtTahunPelajaran.Text;
                m.Semester = cboSemester.SelectedValue;
                m.Rel_Kelas = txtKelasUnit.Value;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_FormasiGuruMapel.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text);
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_FormasiGuruMapel.Insert(m, Libs.LOGGED_USER_M.UserID);
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
                FormasiGuruMapel m = DAO_FormasiGuruMapel.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        cboUnitSekolah.SelectedValue = m.Rel_Sekolah.ToString();
                        txtMapelUnit.Value = m.Rel_Mapel;
                        cboMapel.SelectedValue = m.Rel_Mapel;
                        txtTahunPelajaran.Text = m.TahunAjaran;
                        cboSemester.SelectedValue = m.Semester;
                        txtKelasUnit.Value = m.Rel_Kelas;
                        cboKelas.SelectedValue = m.Rel_Kelas;
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
                FormasiGuruMapel m = DAO_FormasiGuruMapel.GetByID_Entity(txtID.Value.Trim());
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

        protected void btnShowDataMengajar_Click(object sender, EventArgs e)
        {
            ltrCaptionFormasi.Text = "";
            FormasiGuruMapel m = DAO_FormasiGuruMapel.GetByID_Entity(txtID.Value);
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel);
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas);
                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m.Rel_Sekolah.ToString());
                    if (m_mapel != null && m_kelas != null && m_sekolah != null)
                    {
                        if (m_mapel.Nama != null && m_kelas.Nama != null && m_sekolah.Nama != null)
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
                                             m_sekolah.Nama +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             m_mapel.Nama +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             "Kelas&nbsp;" +
                                             m_kelas.Nama +
                                             "&nbsp;" +
                                             "</span>";

                            BindListViewFormasiMengajar(txtID.Value, true);
                            mvMain.ActiveViewIndex = 1;
                            this.Master.ShowHeaderTools = false;
                            txtKeyAction.Value = JenisAction.ShowDataMengajar.ToString();
                        }
                    }                    
                }
            }
        }

        protected void lvListGuruMengajar_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_rel_guru = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_rel_guru");
            System.Web.UI.WebControls.Literal imgh_guru = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_guru");
            System.Web.UI.WebControls.Literal imgh_kelasdet = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kelasdet");
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

            imgh_rel_guru.Text = html_image;
            imgh_guru.Text = html_image;
            imgh_keterangan.Text = html_image;
            imgh_kelasdet.Text = html_image;
            
            imgh_rel_guru.Visible = false;
            imgh_guru.Visible = false;
            imgh_keterangan.Visible = false;
            imgh_kelasdet.Visible = false;
            
            switch (e.SortExpression.ToString().Trim())
            {
                case "Rel_Guru":
                    imgh_rel_guru.Visible = true;
                    break;
                case "Guru":
                    imgh_guru.Visible = true;
                    break;
                case "KelasDet":
                    imgh_kelasdet.Visible = true;
                    break;
                case "Keterangan":
                    imgh_keterangan.Visible = true;
                    break;
            }
        }

        protected void lvListGuruMengajar_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

        protected void lnkOKGuruMengajar_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                FormasiGuruMapelDet m = new FormasiGuruMapelDet();
                m.Kode = Guid.NewGuid();
                m.Rel_FormasiGuruMapel = new Guid(txtID.Value);
                m.Rel_Guru = txtGuru.Value;
                if (div_kelas_guru.Visible)
                {
                    m.Rel_KelasDet = new Guid(cboKelasGuru.SelectedValue);
                }
                else
                {
                    m.Rel_KelasDet = new Guid(Constantas.GUID_NOL);
                }
                m.Keterangan = txtKeterangan.Text;

                List<FormasiGuruMapelDet> lst_det = DAO_FormasiGuruMapelDet.GetByHeader_Entity(txtID.Value);                
                if (txtIDItem.Value.Trim() == "")
                {
                    if (lst_det.FindAll(m0 => m0.Rel_Guru == txtGuru.Value).Count > 0 && div_kelas_guru.Visible == false)
                    {
                        txtKeyAction.Value = "Data guru&nbsp;&nbsp;<span style=\"font-weight: bold; color: yellow;\">" + txtGuru.Text + "</span>&nbsp;&nbsp;sudah ada.";
                        return;
                    }
                    if (lst_det.FindAll(m0 => m0.Rel_Guru == txtGuru.Value && m0.Rel_KelasDet == new Guid(cboKelasGuru.SelectedValue)).Count > 0 && div_kelas_guru.Visible == true)
                    {
                        txtKeyAction.Value = "Data guru&nbsp;&nbsp;<span style=\"font-weight: bold; color: yellow;\">" + txtGuru.Text + "</span>&nbsp;&nbsp;sudah ada.";
                        return;
                    }

                    DAO_FormasiGuruMapelDet.Insert(m, Libs.LOGGED_USER_M.UserID);
                }
                else
                {
                    if (lst_det.FindAll(m0 => m0.Rel_Guru == txtGuru.Value && m0.Kode != new Guid(txtIDItem.Value)).Count > 0 && div_kelas_guru.Visible == false)
                    {
                        txtKeyAction.Value = "Data guru&nbsp;&nbsp;<span style=\"font-weight: bold; color: yellow;\">" + txtGuru.Text + "</span>&nbsp;&nbsp;sudah ada.";
                        return;
                    }
                    if (lst_det.FindAll(m0 => m0.Rel_Guru == txtGuru.Value && m0.Kode != new Guid(txtIDItem.Value) && m0.Rel_KelasDet == new Guid(cboKelasGuru.SelectedValue)).Count > 0 && div_kelas_guru.Visible == true)
                    {
                        txtKeyAction.Value = "Data guru&nbsp;&nbsp;<span style=\"font-weight: bold; color: yellow;\">" + txtGuru.Text + "</span>&nbsp;&nbsp;sudah ada.";
                        return;
                    }

                    m.Kode = new Guid(txtIDItem.Value);
                    DAO_FormasiGuruMapelDet.Update(m, Libs.LOGGED_USER_M.UserID);

                    if (txtKelasDet.Value.Trim().ToUpper() != m.Rel_KelasDet.ToString().Trim().ToUpper())
                    {
                        DAO_FormasiGuruMapelDetSiswaDet.DeleteByHeader(m.Kode.ToString());
                    }
                }
            }
            BindListViewFormasiMengajar(txtID.Value, true);
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void InitInputGuruMengajar()
        {
            if (txtID.Value.Trim() != "")
            {
                FormasiGuruMapel m = DAO_FormasiGuruMapel.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {

                        Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m.Rel_Sekolah.ToString());
                        string jenis_kelas = "Kelas Perwalian";

                        if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA)
                        {
                            cboKelasGuru.Items.Clear();
                            cboKelasGuru.Items.Add("");
                            List<KelasDet> lst_kelas = DAO_KelasDet.GetByKelas_Entity(m.Rel_Kelas).OrderBy(m1 => m1.UrutanKelas).ToList();
                            foreach (KelasDet m_kelas in lst_kelas)
                            {
                                if (m_kelas.IsAktif)
                                {
                                    jenis_kelas = "Kelas Perwalian";
                                    if (m_kelas.IsKelasJurusan)
                                    {
                                        jenis_kelas = "Kelas Jurusan";
                                    }
                                    else if (m_kelas.IsKelasSosialisasi)
                                    {
                                        jenis_kelas = "Kelas Sosialisasi";
                                    }
                                    cboKelasGuru.Items.Add(new ListItem
                                    {
                                        Value = m_kelas.Kode.ToString().Trim().ToUpper(),
                                        Text = jenis_kelas +
                                               HttpUtility.HtmlDecode("&nbsp;&nbsp;&rarr;&nbsp;&nbsp;") +
                                               m_kelas.Nama
                                    });
                                }
                            }
                        }
                        else
                        {
                            cboKelasGuru.Items.Clear();
                            cboKelasGuru.Items.Add("");
                            List<KelasDet> lst_kelas = DAO_KelasDet.GetByKelas_Entity(m.Rel_Kelas).OrderBy(m1 => m1.UrutanKelas).ToList();
                            foreach (KelasDet m_kelas in lst_kelas)
                            {
                                if (m_kelas.IsAktif)
                                {
                                    cboKelasGuru.Items.Add(new ListItem
                                    {
                                        Value = m_kelas.Kode.ToString().Trim().ToUpper(),
                                        Text = m_kelas.Nama
                                    });
                                }
                            }
                        }   
                    }
                }
            }
        }

        protected void btnShowInputGuruMengajar_Click(object sender, EventArgs e)
        {
            FormasiGuruMapel m_for = DAO_FormasiGuruMapel.GetByID_Entity(txtID.Value);
            if (m_for != null)
            {
                if (m_for.TahunAjaran != null)
                {
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m_for.Rel_Mapel);
                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
                        {

                            div_kelas_guru.Visible = true;
                            if (
                                    m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT 
                                )
                            {
                                div_kelas_guru.Visible = false;
                            }

                        }
                    }
                }
            }

            txtGuru.Value = "";
            txtIDItem.Value = "";
            txtKeterangan.Text = "";
            InitInputGuruMengajar();
            txtKeyAction.Value = JenisAction.DoShowInputMengajar.ToString();
        }

        protected void lnkOKHapusItemMengajar_Click(object sender, EventArgs e)
        {
            if (txtSelItem.Value.Trim() != "")
            {
                string[] arr_item = txtSelItem.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr_item)
                {
                    DAO_FormasiGuruMapelDet.Delete(item, Libs.LOGGED_USER_M.UserID);
                }
                txtSelItem.Value = "";
                BindListViewFormasiMengajar(txtID.Value, true);
                txtKeyAction.Value = JenisAction.DoDelete.ToString();
            }
        }

        protected void btnShowEditMengajar_Click(object sender, EventArgs e)
        {
            if (txtIDItem.Value.Trim() != "")
            {
                FormasiGuruMapelDet m = DAO_FormasiGuruMapelDet.GetByID_Entity(txtIDItem.Value);
                if (m != null)
                {
                    if (m.Keterangan != null)
                    {
                        FormasiGuruMapel m_for = DAO_FormasiGuruMapel.GetByID_Entity(m.Rel_FormasiGuruMapel.ToString());
                        if (m_for != null)
                        {
                            if (m_for.TahunAjaran != null)
                            {
                                Mapel m_mapel = DAO_Mapel.GetByID_Entity(m_for.Rel_Mapel);
                                if (m_mapel != null)
                                {
                                    if (m_mapel.Nama != null)
                                    {
                                        InitInputGuruMengajar();
                                        div_kelas_guru.Visible = true;
                                        if (
                                            m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT
                                        )
                                        {
                                            div_kelas_guru.Visible = false;
                                            cboKelasGuru.SelectedValue = "";
                                        }
                                        else
                                        {
                                            Libs.SelectDropdownListByValue(cboKelasGuru, m.Rel_KelasDet.ToString().Trim().ToUpper());
                                        }

                                        txtGuru.Value = m.Rel_Guru;                                        
                                        txtKeterangan.Text = m.Keterangan;
                                        BindListViewFormasiMengajar(txtID.Value, true);
                                        txtKeyAction.Value = JenisAction.DoShowInputMengajar.ToString();

                                    }
                                }
                            }
                        }
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
            BindListView(true, this.Master.txtCariData.Text);            
            txtKeyAction.Value = JenisAction.DoTampilkanData.ToString();
        }

        protected void lvListSiswaMapel_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void lvListSiswaMapel_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void btnShowEditFormasiSiswa_Click(object sender, EventArgs e)
        {
            if (txtIDItemSiswa.Value.Trim() != "")
            {
                FormasiGuruMapelDetSiswa m = DAO_FormasiGuruMapelDetSiswa.GetByID_Entity(txtIDItemSiswa.Value);
                if (m != null)
                {
                    if (m.Rel_Siswa != null)
                    {
                        FormasiGuruMapel m_for = DAO_FormasiGuruMapel.GetByID_Entity(m.Rel_FormasiGuruMapel.ToString());
                        if (m_for != null)
                        {
                            if (m_for.TahunAjaran != null)
                            {
                                Mapel m_mapel = DAO_Mapel.GetByID_Entity(m_for.Rel_Mapel);
                                if (m_mapel != null)
                                {
                                    if (m_mapel.Nama != null)
                                    {
                                        txtSiswaMapel.Value = m.Rel_Siswa;
                                        BindListViewFormasiSiswa(txtID.Value, true);
                                        txtKeyAction.Value = JenisAction.DoShowInputSiswaMapel.ToString();

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void btnShowInputFormasiSiswa_Click(object sender, EventArgs e)
        {
            txtSiswaMapel.Value = "";
            txtSiswaMapel.Text = "";
            txtIDItemSiswa.Value = "";
            txtKeyAction.Value = JenisAction.DoShowInputSiswaMapel.ToString();
        }

        protected void btnShowDataFormasiSiswa_Click(object sender, EventArgs e)
        {
            ltrCaptionFormasiSiswa.Text = "";
            FormasiGuruMapel m = DAO_FormasiGuruMapel.GetByID_Entity(txtID.Value);
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel);
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas);
                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m.Rel_Sekolah.ToString());
                    if (m_mapel != null && m_kelas != null && m_sekolah != null)
                    {
                        if (m_mapel.Nama != null && m_kelas.Nama != null && m_sekolah.Nama != null)
                        {
                            ltrCaptionFormasiSiswa.Text =
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
                                             m_sekolah.Nama +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: #a50068;\">" +
                                             "&nbsp;" +
                                             m_mapel.Nama +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             "Kelas&nbsp;" +
                                             m_kelas.Nama +
                                             "&nbsp;" +
                                             "</span>";

                            txtSiswaMapel.KodeUnit = m.Rel_Sekolah.ToString();
                            txtSiswaMapel.TahunAjaran = m.TahunAjaran;
                            txtSiswaMapel.Semester = m.Semester;

                            BindListViewFormasiSiswa(txtID.Value, true);
                            mvMain.ActiveViewIndex = 2;
                            this.Master.ShowHeaderTools = false;
                            txtKeyAction.Value = JenisAction.ShowDataFormasiSiswa.ToString();
                        }
                    }
                }
            }
        }

        protected void lnkOKInputSiswa_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                FormasiGuruMapelDetSiswa m = new FormasiGuruMapelDetSiswa();
                m.Kode = Guid.NewGuid();
                m.Rel_FormasiGuruMapel = new Guid(txtID.Value);
                m.Rel_Siswa = txtSiswaMapel.Value;

                List<FormasiGuruMapelDetSiswa> lst_det = DAO_FormasiGuruMapelDetSiswa.GetByHeader_Entity(txtID.Value);
                if (txtIDItemSiswa.Value.Trim() == "")
                {
                    if (lst_det.FindAll(m0 => m0.Rel_Siswa == txtSiswaMapel.Value).Count > 0)
                    {
                        txtKeyAction.Value = "Data siswa&nbsp;&nbsp;<span style=\"font-weight: bold; color: yellow;\">" + txtSiswaMapel.Text + "</span>&nbsp;&nbsp;sudah ada.";
                        return;
                    }

                    DAO_FormasiGuruMapelDetSiswa.Insert(m, Libs.LOGGED_USER_M.UserID);
                    BindListViewFormasiSiswa(txtID.Value, true);
                    txtSiswaMapel.Value = "";
                    txtKeyAction.Value = JenisAction.AddFormasiSiswaWithMessage.ToString();
                }
                else
                {
                    if (lst_det.FindAll(m0 => m0.Rel_Siswa == txtSiswaMapel.Value && m0.Kode != new Guid(txtIDItemSiswa.Value)).Count > 0)
                    {
                        txtKeyAction.Value = "Data siswa&nbsp;&nbsp;<span style=\"font-weight: bold; color: yellow;\">" + txtSiswaMapel.Text + "</span>&nbsp;&nbsp;sudah ada.";
                        return;
                    }

                    m.Kode = new Guid(txtIDItemSiswa.Value);
                    DAO_FormasiGuruMapelDetSiswa.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListViewFormasiSiswa(txtID.Value, true);
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
            }            
        }

        protected void lnkOKHapusItemSiswa_Click(object sender, EventArgs e)
        {
            if (txtSelItem.Value.Trim() != "")
            {
                string[] arr_item = txtSelItem.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr_item)
                {
                    DAO_FormasiGuruMapelDetSiswa.Delete(item, Libs.LOGGED_USER_M.UserID);
                }
                txtSelItem.Value = "";
                BindListViewFormasiSiswa(txtID.Value, true);
                txtKeyAction.Value = JenisAction.DoDelete.ToString();
            }
        }

        protected void ShowListPilihSiswa(string tahun_ajaran, string semester, string rel_kelas_det)
        {
            ltrPilihSiswa.Text = "";
            var lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                    QS.GetUnit(),                                    
                                    rel_kelas_det,
                                    tahun_ajaran,
                                    semester
                                );

            List<FormasiGuruMapelDetSiswaDet> lst_formasi_det_siswa_det = DAO_FormasiGuruMapelDetSiswaDet.GetByHeader_Entity(txtIDItem.Value);

            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(QS.GetUnit());
            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {
                    int id = 1;

                    foreach (Siswa m_siswa in lst_siswa)
                    {
                        string s_panggilan = "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                              "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

                        ltrPilihSiswaCaption.Text = "Pilih Siswa";

                        KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet);
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                ltrPilihSiswaCaption.Text = "<div style=\"width: 100%; background-color: #f1f9f7; border-color: #e0f1e9; color: #1d9d74; padding: 15px; border-width: 1px; border-style: solid; border-radius: 5px; font-weight: normal;\">" +
                                                                "<i class=\"fa fa-info-circle\"></i>" +
                                                                "&nbsp;&nbsp;" +
                                                                "Pilih siswa kelas <span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span>" +
                                                            "</div>";
                            }
                        }

                        string chk_id = "chk_" + m_siswa.Kode.ToString().Replace("-", "_");
                        ltrPilihSiswa.Text +=
                                    "<div class=\"row\">" +
                                        "<div class=\"col-xs-1\" style=\"color: #bfbfbf; padding-top: 7px;\">" +
                                            id.ToString() + "." +
                                        "</div>" +
                                        "<div class=\"col-xs-11\">" +
                                            "<div class=\"checkbox checkbox-adv\">" +
                                                "<label for=\"" + chk_id + "\">" +
                                                    "<input " +
                                                            "name=\"chk_pilih_siswa[]\" " +
                                                            "value=\"" + m_siswa.Kode.ToString() + "\" " +
                                                            "class=\"access-hide\" id=\"" + chk_id + "\" " +
                                                            (
                                                                lst_formasi_det_siswa_det.FindAll(mx => mx.Rel_Siswa.Trim().ToUpper() == m_siswa.Kode.ToString().ToUpper()).Count > 0
                                                                ? " checked=\"checked\" "
                                                                : (
                                                                    lst_formasi_det_siswa_det.Count == 0
                                                                    ? " checked=\"checked\" "
                                                                    : ""
                                                                  )
                                                            ) +
                                                            "type=\"checkbox\">" +
                                                    "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                                    "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                                        "&nbsp;&nbsp;" +
                                                        m_siswa.Nama.Trim().ToUpper() +
                                                    "</span>" +
                                                "</label>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>";

                        id++;

                    }

                }
            }
        }

        protected void btnShowDataPilihSiswa_Click(object sender, EventArgs e)
        {
            txtKelasDet.Value = "";
            if (txtIDItem.Value.Trim() != "")
            {
                FormasiGuruMapelDet m = DAO_FormasiGuruMapelDet.GetByID_Entity(txtIDItem.Value);
                if (m.Rel_Guru != null)
                {
                    txtKelasDet.Value = m.Rel_KelasDet.ToString();
                    FormasiGuruMapel m0 = DAO_FormasiGuruMapel.GetByID_Entity(m.Rel_FormasiGuruMapel.ToString());
                    if (m0 != null)
                    {
                        if (m0.TahunAjaran != null)
                        {
                            ShowListPilihSiswa(m0.TahunAjaran, m0.Semester, m.Rel_KelasDet.ToString());
                        }
                    }
                }
                txtKeyAction.Value = JenisAction.DoShowPilihSiswa.ToString();
            }
        }

        protected void lnkOKPilihSiswa_Click(object sender, EventArgs e)
        {
            if (txtParsePilihSiswa.Value.Trim() != "")
            {
                string[] arr_siswa = txtParsePilihSiswa.Value.Split(new string[] { ";" }, StringSplitOptions.None);
                DAO_FormasiGuruMapelDetSiswaDet.DeleteByHeader(txtIDItem.Value);
                int id_urutan = 1;
                foreach (string item_siswa in arr_siswa)
                {
                    if (item_siswa.Trim() != "")
                    {
                        DAO_FormasiGuruMapelDetSiswaDet.Insert(new FormasiGuruMapelDetSiswaDet
                        {
                            Kode = Guid.NewGuid(),
                            Rel_FormasiGuruMapelDet = txtIDItem.Value,
                            Rel_Siswa = item_siswa,
                            Urutan = id_urutan
                        });
                        id_urutan++;
                    }
                }
            }
            txtKeyAction.Value = JenisAction.DoOKShowPilihSiswa.ToString();
        }

        protected void btnDoUpdateSiswaPilihan_Click(object sender, EventArgs e)
        {
            try
            {
                DAO_FormasiGuruMapelDet.UpdateIsSiswaPilihan(
                        txtIDItem.Value, (txtIsSiswaPilihan.Value == "1" ? true : false)
                    );
                BindListViewFormasiMengajar(txtID.Value, true);
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }
    }
}