using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning;

namespace AI_ERP.Application_Modules.EDUCATION.Elearning
{
    public partial class wf_Guru_Praota : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAGURUPRAOTA";

        public enum JenisAction
        {
            Add,
            AddDet,
            AddWithMessage,
            Edit,
            Update,
            Delete,
            Search,
            DoAdd,
            DoAddDet,
            DoUpdate,
            DoDelete,
            DoDeleteDet,
            DoSearch,
            DoShowData,
            DoShowDataDet,
            DoShowConfirmHapus,
            DoShowConfirmHapusDet,
            DoShowConfirmPublish
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" +
                                            ResolveUrl("~/Application_CLibs/images/svg/school-material-0.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Materi Pembelajaran";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitInput();
                InitKeyEventClient();
                this.Master.txtCariData.Text = Libs.GetQ();                
            }
            BindListView(!IsPostBack, Libs.GetQ());
            BindListViewDet(true);

            if (Libs.URL_IDENTIFIER_ORTU.IsAdaIDUrlIdOrtu())
            {
                btnDoAdd.Visible = false;
            }
            else
            {
                btnDoAdd.Visible = true;
            }
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            cboTahunAjaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboMataPelajaran.ClientID + "').focus(); return false; }");
            cboMataPelajaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");

            cboMataPelajaran.Attributes.Add("onchange", txtMapel.ClientID + ".value = this.value; return false;");
            cboKelas.Attributes.Add("onchange", txtKelas.ClientID + ".value = this.value; return false;");
            cboGuru.Attributes.Add("onchange", txtGuru.ClientID + ".value = this.value; return false;");

            cboJenisFile.Attributes.Add("onchange", "UbahViewFile();");
        }

        protected void InitInput()
        {
            List<FormasiGuruKelas_ByGuru> lst_kelasguru = DAO_FormasiGuruKelas.GetByGuruByTA_Entity(Libs.LOGGED_USER_M.NoInduk, Libs.GetTahunAjaranNow());
            List<string> lst_kelas = new List<string>();
            foreach (FormasiGuruKelas_ByGuru m in lst_kelasguru)
            {
                KelasDet m_kelasdet = DAO_KelasDet.GetByID_Entity(m.Rel_KelasDet);
                if (m_kelasdet != null)
                {
                    if (m_kelasdet.Nama != null)
                    {
                        if (lst_kelas.FindAll(k => k.Trim().ToLower() == m_kelasdet.Rel_Kelas.ToString().ToLower()).Count == 0)
                        {
                            lst_kelas.Add(m_kelasdet.Rel_Kelas.ToString());
                        }
                    }
                }
            }

            cboKelas.Items.Clear();
            cboKelas.Items.Add("");
            foreach (string kelas in lst_kelas)
            {
                Kelas m = DAO_Kelas.GetByID_Entity(kelas);
                if (m != null)
                {
                    if (m.Nama != null)
                    {
                        cboKelas.Items.Add(new ListItem {
                            Value = m.Kode.ToString(),
                            Text = m.Nama
                        });
                    }
                }
            }

            List<string> lst_tahun_ajaran =
                DAO_FormasiGuruMapel.GetAll_Entity().Select(m => m.TahunAjaran).Distinct().ToList();
            lst_tahun_ajaran = lst_tahun_ajaran.OrderByDescending(m => m).ToList();
            cboTahunAjaran.Items.Clear();
            foreach (string tahun_ajaran in lst_tahun_ajaran)
            {
                cboTahunAjaran.Items.Add(new ListItem
                {
                    Value = tahun_ajaran,
                    Text = tahun_ajaran
                });
            }
            
            Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(Libs.LOGGED_USER_M.NoInduk);
            if (m_pegawai != null)
            {
                if (m_pegawai.Nama != null)
                {
                    cboGuru.Items.Clear();
                    cboGuru.Items.Add(new ListItem
                    {
                        Value = m_pegawai.Kode,
                        Text = m_pegawai.Nama
                    });

                    List<Mapel> lst_mapel = DAO_Mapel.GetAllByGuru_Entity(Libs.LOGGED_USER_M.NoInduk, m_pegawai.Rel_Unit);
                    cboMataPelajaran.Items.Clear();
                    cboMataPelajaran.Items.Add("");
                    foreach (var item in lst_mapel)
                    {
                        cboMataPelajaran.Items.Add(new ListItem {
                            Value = item.Kode.ToString(),
                            Text = item.Nama
                        });
                    }
                }
            }
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            string rel_guru = "";
            rel_guru = Libs.LOGGED_USER_M.NoInduk;
            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            if (Libs.URL_IDENTIFIER_ORTU.IsAdaIDUrlIdOrtu()) {
                if (keyword.Trim() != "")
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("nama", keyword);
                    sql_ds.SelectCommand = DAO_Praota.SP_SELECT_ALL_FOR_SEARCH;
                }
                else
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectCommand = DAO_Praota.SP_SELECT_ALL;
                }
            }
            else
            {
                if (keyword.Trim() != "")
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("nama", keyword);
                    sql_ds.SelectParameters.Add("Rel_Guru", rel_guru);
                    sql_ds.SelectCommand = DAO_Praota.SP_SELECT_BY_GURU_FOR_SEARCH;
                }
                else
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("Rel_Guru", rel_guru);
                    sql_ds.SelectCommand = DAO_Praota.SP_SELECT_BY_GURU;
                }
            }
            
            if (isbind) lvData.DataBind();
        }

        private void BindListViewDet(bool isbind = true, string keyword = "")
        {
            sql_ds_det.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_det.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_det.SelectParameters.Clear();
            sql_ds_det.SelectParameters.Add("Rel_Praota", txtID.Value);
            sql_ds_det.SelectCommand = DAO_Praota_Det.SP_SELECT_BY_HEADER;
            if (isbind) lvDataDet.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            System.Web.UI.WebControls.Literal imgh_kelas = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kelas");
            System.Web.UI.WebControls.Literal imgh_guru = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_guru");
            System.Web.UI.WebControls.Literal imgh_mapel = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_mapel");

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
            imgh_kelas.Text = html_image;
            imgh_guru.Text = html_image;
            imgh_mapel.Text = html_image;

            imgh_tahunajaran.Visible = false;
            imgh_kelas.Visible = false;
            imgh_guru.Visible = false;
            imgh_mapel.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
                    break;
                case "Kelas":
                    imgh_kelas.Visible = true;
                    break;
                case "Guru":
                    imgh_guru.Visible = true;
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

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Libs.FILE_PAGE_URL);
        }

        protected void InitFields()
        {
            txtID.Value = "";
            cboKelas.SelectedValue = "";
        }

        protected void InitFieldsDet()
        {
            cboSemesterDet.SelectedValue = Libs.GetSemesterByTanggal(DateTime.Now).ToString();
            txtMateriPokok.Text = "";
            txtMateriPokokDeskripsi.Text = "";
            txtMateriPokokDeskripsiVal.Value = "";
            txtAlokasiWaktu.Text = "";
            cboKelas.SelectedValue = "";
            txtIDDet.Value = "";
            txtURLEmbed.Text = "";
            txtIDDetNew.Value = Guid.NewGuid().ToString();
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            InitFields();
            EnabledInput(true);
            txtKeyAction.Value = JenisAction.Add.ToString();
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
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
                Kelas m_kelas = DAO_Kelas.GetByID_Entity(txtKelas.Value);

                Praota m = new Praota();
                m.TahunAjaran = cboTahunAjaran.SelectedValue;
                m.Rel_Sekolah = m_kelas.Rel_Sekolah.ToString();
                m.Rel_Kelas = txtKelas.Value;
                m.Rel_Guru = txtGuru.Value;
                m.Rel_Mapel = txtMapel.Value;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_Praota.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_Praota.Insert(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    InitFields();
                    txtKeyAction.Value = JenisAction.DoAdd.ToString();
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
                Praota m = DAO_Praota.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        cboTahunAjaran.SelectedValue = m.TahunAjaran;
                        txtKelas.Value = m.Rel_Kelas;
                        txtMapel.Value = m.Rel_Mapel;
                        txtGuru.Value = m.Rel_Guru;
                        cboKelas.SelectedValue = m.Rel_Kelas;
                        cboMataPelajaran.SelectedValue = m.Rel_Mapel;
                        cboGuru.SelectedValue = m.Rel_Guru;
                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                        EnabledInput(!m.IsPublished);
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
                Praota m = DAO_Praota.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m.Rel_Guru);
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas);
                        Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel);

                        if (m_mapel != null)
                        {
                            if (m_mapel.Nama != null)
                            {
                                ltrMsgConfirmHapus.Text = "Hapus Materi Pembelajaran <span style=\"font-weight: bold;\">\"" +
                                                                Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                          "\"</span><br />" +
                                                          "Mata Pelajaran : <span style=\"font-weight: bold;\">" + m_mapel.Nama + "</span><br />" +
                                                          "Kelas : <span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span><br />" +
                                                          "Guru : <span style=\"font-weight: bold;\">" + m_pegawai.Nama + "</span>" +
                                                          "?";
                                txtURLDelete.Value = GetURLDelete(txtID.Value, "");
                                txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                            }
                        }
                    }
                }
            }
        }

        protected string GetURLDelete(string id, string id2 = "")
        {
            return ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DELETE_FILE.ROUTE) + 
                              "?jenis=" + Libs.JENIS_UPLOAD.MATERI_PEMBELAJARAN + 
                              "&id=" + id.ToString() + 
                              (id2.Trim() != "" ? "&id2=" + id2.ToString() : "");
        }

        protected void btnShowMateriPembelajaran_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                ltrJudulMapel.Text = "";
                Praota m = DAO_Praota.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        ltrJudulMapel.Text = "<span style=\"color: #959595;\">" +
                                                "<i class=\"fa fa-hashtag\"></i>&nbsp;" +
                                                "Kelas : " +
                                             "</span>" +
                                             "<div style=\"font-weight: bold; margin-left: 15px; border-style: solid; border-width: 1px; border-color: #d9d9d9; padding: 5px; padding-left: 15px; padding-right: 15px; border-radius: 5px;\">" + DAO_Kelas.GetByID_Entity(m.Rel_Kelas).Nama + "</div>" +
                                             "<span style=\"color: #959595;\">" +
                                                "<i class=\"fa fa-hashtag\"></i>&nbsp;" +
                                                "Tahun Pelajaran : " +
                                             "</span>" +
                                             "<div style=\"font-weight: bold; margin-left: 15px; border-style: solid; border-width: 1px; border-color: #d9d9d9; padding: 5px; padding-left: 15px; padding-right: 15px; border-radius: 5px;\">" + m.TahunAjaran + "</div>" +
                                             "<span style=\"color: #959595;\">" +
                                                "<i class=\"fa fa-hashtag\"></i>&nbsp;" +
                                                "Mata Pelajaran : " +
                                             "</span>" +
                                             "<div style=\"font-weight: bold; margin-left: 15px; border-style: solid; border-width: 1px; border-color: #d9d9d9; padding: 5px; padding-left: 15px; padding-right: 15px; border-radius: 5px;\">" + DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString()).Nama + "</div>";
                    }
                }

                mvMain.ActiveViewIndex = 1;
                this.Master.ShowHeaderTools = false;
            }
        }

        protected void lnkBatalPengaturanMateri_Click(object sender, EventArgs e)
        {
            ShowDataList();
        }

        protected void ShowDataList()
        {
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
        }

        protected void btnOKSave_Click(object sender, EventArgs e)
        {
            ShowDataList();
        }

        protected void btnShowConfirmPublish_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Praota m = DAO_Praota.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m.Rel_Guru);
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas);
                        Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel);

                        if (m_mapel != null)
                        {
                            if (m_mapel.Nama != null)
                            {
                                ltrMsgConfirmPublish.Text = (m.IsPublished ? "<span style=\"color: darkorange; font-weight: bold;\">UnPublish</span>" : "<span style=\"color: green; font-weight: bold;\">Publish</span>") +
                                                            " Materi Pembelajaran <span style=\"font-weight: bold;\">\"" +
                                                                Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                            "\"</span><br />" +
                                                            "Mata Pelajaran : <span style=\"font-weight: bold;\">" + m_mapel.Nama + "</span><br />" +
                                                            "Kelas : <span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span><br />" +
                                                            "Guru : <span style=\"font-weight: bold;\">" + m_pegawai.Nama + "</span>" +
                                                            "?";
                                txtKeyAction.Value = JenisAction.DoShowConfirmPublish.ToString();
                            }
                        }
                    }
                }
            }
        }

        protected void btnOKPublish_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Praota m = DAO_Praota.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        DAO_Praota.UpdatePublished(
                            txtID.Value,
                            !m.IsPublished
                        );

                        BindListView(true, Libs.GetQ());
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                }
            }
        }

        protected void btnDoAddDet_Click(object sender, EventArgs e)
        {
            InitFieldsDet();
            InitInputDetByPublished(false);
            txtKeyAction.Value = JenisAction.AddDet.ToString();
        }

        protected void lnkOKInputDet_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIDDet.Value.Trim() == "")
                {
                    DAO_Praota_Det.Insert(new Praota_Det
                    {
                        Kode = new Guid(txtIDDetNew.Value),
                        MateriPokok = txtMateriPokok.Text,
                        DeskripsiMateriPokok = txtMateriPokokDeskripsiVal.Value,
                        AlokasiWaktu = txtAlokasiWaktu.Text,
                        EstimasiWaktuDari = "",
                        EstimasiWaktuSampai = "",
                        Keterangan = "",
                        Rel_Praota = new Guid(txtID.Value),
                        Semester = cboSemesterDet.SelectedValue,
                        JenisFile = cboJenisFile.SelectedValue,
                        URLEmbed = (cboJenisFile.SelectedValue == "0" ? "" : txtURLEmbed.Text)
                    }, Libs.LOGGED_USER_M.UserID);
                }
                else
                {
                    DAO_Praota_Det.Update(new Praota_Det
                    {
                        Kode = new Guid(txtIDDet.Value),
                        MateriPokok = txtMateriPokok.Text,
                        DeskripsiMateriPokok = txtMateriPokokDeskripsiVal.Value,
                        AlokasiWaktu = txtAlokasiWaktu.Text,
                        EstimasiWaktuDari = "",
                        EstimasiWaktuSampai = "",
                        Keterangan = "",
                        Rel_Praota = new Guid(txtID.Value),
                        Semester = cboSemesterDet.SelectedValue,
                        JenisFile = cboJenisFile.SelectedValue,
                        URLEmbed = (cboJenisFile.SelectedValue == "0" ? "" : txtURLEmbed.Text)
                    }, Libs.LOGGED_USER_M.UserID);
                }
                BindListViewDet(true);
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnShowConfirmDeleteDet_Click(object sender, EventArgs e)
        {
            if (txtIDDet.Value.Trim() != "")
            {
                Praota m = DAO_Praota.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        if (m.IsPublished)
                        {
                            txtKeyAction.Value = "Data yang sudah di-publish tidak dapat dihapus, lakukan UnPublish terlebih dahulu untuk menghapus atau mengubah data.";                            
                        }
                        else
                        {
                            txtURLDelete.Value = GetURLDelete(txtID.Value, txtIDDet.Value);
                            txtKeyAction.Value = JenisAction.DoShowConfirmHapusDet.ToString();
                        }                        
                    }
                }                
            }
        }

        protected void lnkOKHapusDet_Click(object sender, EventArgs e)
        {
            try
            {
                DAO_Praota_Det.Delete(txtIDDet.Value, Libs.LOGGED_USER_M.UserID);
                BindListViewDet(true);
                txtKeyAction.Value = JenisAction.DoDeleteDet.ToString();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnShowDetailDet_Click(object sender, EventArgs e)
        {
            if (txtIDDet.Value.Trim() != "")
            {
                Praota_Det m = DAO_Praota_Det.GetByID_Entity(txtIDDet.Value.Trim());
                if (m != null)
                {
                    if (m.Semester != null)
                    {
                        Praota m_praota = DAO_Praota.GetByID_Entity(m.Rel_Praota.ToString());
                        if (m_praota != null)
                        {
                            if (m_praota.TahunAjaran != null)
                            {
                                cboSemesterDet.SelectedValue = m.Semester;
                                txtMateriPokok.Text = m.MateriPokok;
                                txtMateriPokokDeskripsi.Text = m.DeskripsiMateriPokok;
                                txtAlokasiWaktu.Text = m.AlokasiWaktu;
                                txtMateriPokokDeskripsiVal.Value = m.DeskripsiMateriPokok;
                                ltrDeskripsiMateriView.Text = m.DeskripsiMateriPokok;
                                divUploadedFiles.Visible = false;
                                ltrUploadedFiles.Text = Libs.GetHTMLListUploadedFiles(
                                        this.Page, 
                                        Libs.GetFolderElearningMateriPembelajaran(txtID.Value, m.Kode.ToString()), 
                                        Libs.JENIS_UPLOAD.MATERI_PEMBELAJARAN, 
                                        txtID.Value, 
                                        txtIDDet.Value,
                                        !m_praota.IsPublished
                                    );
                                if (ltrUploadedFiles.Text.Trim() != "")
                                {
                                    divUploadedFiles.Visible = true;
                                }
                                cboJenisFile.SelectedValue = (m.JenisFile == "" ? "0" : m.JenisFile);
                                txtURLEmbed.Text = m.URLEmbed;
                                InitInputDetByPublished(m_praota.IsPublished);
                                txtKeyAction.Value = JenisAction.DoShowDataDet.ToString();
                            }
                        }
                    }
                }
            }
        }

        protected void InitInputDetByPublished(bool is_published)
        {
            div_deskripsi_materi_edit.Visible = !is_published;
            div_deskripsi_materi_view.Visible = is_published;
            div_upload.Visible = !is_published;
            EnabledInputDet(!is_published);
        }

        protected void EnabledInput(bool is_enabled)
        {
            cboTahunAjaran.Enabled = is_enabled;
            cboKelas.Enabled = is_enabled;
            cboMataPelajaran.Enabled = is_enabled;
            cboGuru.Enabled = is_enabled;
        }

        protected void EnabledInputDet(bool is_enabled)
        {
            cboSemesterDet.Enabled = is_enabled;
            txtMateriPokok.Enabled = is_enabled;
            txtMateriPokokDeskripsi.Enabled = is_enabled;
            txtAlokasiWaktu.Enabled = is_enabled;
        }

        protected void btnDoBack_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
        }

        protected void btnBindDataDet_Click(object sender, EventArgs e)
        {
            BindListViewDet(true);
        }

        protected void btnBindData_Click(object sender, EventArgs e)
        {
            BindListView(true);
        }

        protected void btnDoHapus_Click(object sender, EventArgs e)
        {
            DAO_Praota.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
            BindListView(!IsPostBack, Libs.GetQ().Trim());
        }
    }
}