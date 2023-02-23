using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning.SMP;
using AI_ERP.Application_DAOs.Elearning.SMP;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP
{
    public partial class wf_ProsesRapor : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEPROSESDATARAPORSMP";

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            Edit,
            Update,
            Delete,
            Search,
            DoAdd,
            DoAddDetail,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowInputLog,
            DoShowEditLog,
            DoShowConfirmHapus,
            DoShowConfirmHapusMengajar
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" " +
                                            "src=\"" + ResolveUrl("~/Application_CLibs/images/svg/browser-1.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Proses Rapor";

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

        protected void InitInput()
        {
            cboJamPenguncianInput.Items.Clear();
            cboJamPenguncianInput.Items.Add("");
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 60; j += 15)
                {
                    string jam = (i < 10 ? "0" : "") + i.ToString() +
                                 ":" +
                                 (j < 10 ? "0" : "") + j.ToString();

                    cboJamPenguncianInput.Items.Add(new ListItem
                    {
                        Value = jam,
                        Text = jam
                    });
                }
            }

            cboTahunPelajaran.Items.Clear();
            foreach (var item in DAO_Rapor_StrukturNilai.GetDistinctTahunAjaran_Entity())
            {
                cboTahunPelajaran.Items.Add(new ListItem
                {
                    Value = item,
                    Text = item
                });
            }

            Sekolah sekolah = DAO_Sekolah.GetAll_Entity().FindAll(
                m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMP).FirstOrDefault();
            if (sekolah != null)
            {
                if (sekolah.Nama != null)
                {
                    List<Pegawai> lst_guru = DAO_Pegawai.GetByUnit_Entity(sekolah.Kode.ToString()).OrderBy(m => m.Nama).ToList();
                    cboGuru.Items.Clear();
                    cboGuru.Items.Add("");
                    foreach (Pegawai m in lst_guru)
                    {
                        if (!m.IsNonAktif)
                        {
                            cboGuru.Items.Add(new ListItem
                            {
                                Value = m.Kode.ToString(),
                                Text = m.Nama
                            });
                        }
                    }

                    List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(sekolah.Kode.ToString()).OrderBy(m => m.UrutanLevel).ToList();
                    cboKelasDet.Items.Clear();
                    cboKelasDet.Items.Add("");
                    foreach (Kelas m in lst_kelas)
                    {
                        if (m.IsAktif)
                        {
                            List<KelasDet> lst_kelas_det = DAO_KelasDet.GetByKelas_Entity(m.Kode.ToString()).OrderBy(m0 => m0.UrutanKelas).ToList();
                            foreach (var kelas_det in lst_kelas_det)
                            {
                                cboKelasDet.Items.Add(new ListItem
                                {
                                    Value = kelas_det.Kode.ToString(),
                                    Text = kelas_det.Nama
                                });
                            }                            
                        }
                    }

                    List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(sekolah.Kode.ToString()).OrderBy(m => m.Nama).ToList();
                    cboMapel.Items.Clear();
                    cboMapel.Items.Add("");
                    foreach (Mapel m in lst_mapel)
                    {
                        cboMapel.Items.Add(new ListItem
                        {
                            Value = m.Kode.ToString(),
                            Text = m.Nama
                        });
                    }
                }
            }

            cboSemester.SelectedValue = Libs.GetSemesterByTanggal(DateTime.Now).ToString();
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            cboTahunPelajaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboSemester.ClientID + "').focus(); return false; }");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectCommand = DAO_Rapor_Arsip.SP_SELECT_ALL_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommand = DAO_Rapor_Arsip.SP_SELECT_ALL;
            }
            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            System.Web.UI.WebControls.Literal imgh_jenis_rapor = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_jenis_rapor");
            System.Web.UI.WebControls.Literal imgh_tanggal_awal_absen = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tanggal_awal_absen");
            System.Web.UI.WebControls.Literal imgh_tanggal_closing = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tanggal_closing");


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
            imgh_jenis_rapor.Text = html_image;
            imgh_tanggal_awal_absen.Text = html_image;
            imgh_tanggal_closing.Text = html_image;

            imgh_tahunajaran.Visible = false;
            imgh_jenis_rapor.Visible = false;
            imgh_tanggal_awal_absen.Visible = false;
            imgh_tanggal_closing.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
                    break;
                case "JenisRapor":
                    imgh_jenis_rapor.Visible = true;
                    break;
                case "TanggalAwalAbsen":
                    imgh_tanggal_awal_absen.Visible = true;
                    break;
                case "TanggalClosing":
                    imgh_tanggal_closing.Visible = true;
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
            cboJenisRapor.SelectedValue = "";
            txtTanggalAwalAbsen.Text = "";
            txtTanggalAkhirAbsen.Text = "";
            txtTanggalPenguncianInput.Text = "";
            txtKepalaSekolah.Text = "";
            txtTanggalRapor.Text = "";
            cboJamPenguncianInput.SelectedValue = "";
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
                    DAO_Rapor_Arsip.Delete(txtID.Value);
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
                if (DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                        m0 => m0.TahunAjaran == cboTahunPelajaran.Text &&
                              m0.Semester == cboSemester.Text &&
                              m0.JenisRapor == cboJenisRapor.SelectedValue
                    ).Count > 0 && txtID.Value.Trim() == "")
                {
                    txtKeyAction.Value = "Proses Rapor \"" + cboJenisRapor.Text + "\" tahun pelajaran " + cboTahunPelajaran.Text + " " +
                                         "semester " + cboSemester.SelectedValue + " sudah ada.";
                    return;
                }

                Rapor_Arsip m = new Rapor_Arsip();
                m.TahunAjaran = cboTahunPelajaran.Text;
                m.Semester = cboSemester.SelectedValue;
                m.JenisRapor = cboJenisRapor.SelectedValue;
                m.TanggalAwalAbsen = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalAwalAbsen.Text);
                m.TanggalAkhirAbsen = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalAkhirAbsen.Text);
                m.TanggalClosing = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalPenguncianInput.Text, true, cboJamPenguncianInput.SelectedValue);
                m.KepalaSekolah = txtKepalaSekolah.Text;
                m.TanggalRapor = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalRapor.Text);
                m.Keterangan = txtKeterangan.Text;
                if (txtID.Value.Trim() != "")
                {
                    Rapor_Arsip m_rapor = DAO_Rapor_Arsip.GetByID_Entity(txtID.Value);
                    if (m_rapor != null)
                    {
                        if (m_rapor.TahunAjaran != null)
                        {
                            m.Kode = new Guid(txtID.Value);
                            m.IsArsip = m_rapor.IsArsip;
                            DAO_Rapor_Arsip.Update(m);
                            BindListView(!IsPostBack, Libs.GetQ().Trim());
                            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                        }
                    }
                }
                else
                {
                    m.Kode = Guid.NewGuid();
                    m.IsArsip = false;
                    DAO_Rapor_Arsip.Insert(m);
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
                Rapor_Arsip m = DAO_Rapor_Arsip.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        cboTahunPelajaran.Text = m.TahunAjaran;
                        cboSemester.SelectedValue = m.Semester;
                        cboJenisRapor.SelectedValue = m.JenisRapor;
                        txtTanggalAwalAbsen.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalAwalAbsen, false);
                        txtTanggalAkhirAbsen.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalAkhirAbsen, false);
                        txtTanggalPenguncianInput.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalClosing, false);
                        txtTanggalRapor.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalRapor, false);
                        txtKepalaSekolah.Text = m.KepalaSekolah;
                        cboJamPenguncianInput.SelectedValue = m.TanggalClosing.ToString("HH:mm");
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

        }

        protected void btnShowDetailLog_Click(object sender, EventArgs e)
        {
            BindListViewLogKunciNilai(txtID.Value, true);
            mvMain.ActiveViewIndex = 1;
        }

        private void BindListViewLogKunciNilai(string rel_prosesrapor, bool isbind = true)
        {
            sql_ds_log_nilai.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_log_nilai.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_log_nilai.SelectParameters.Clear();
            sql_ds_log_nilai.SelectParameters.Add("Rel_ProsesRapor", rel_prosesrapor);
            sql_ds_log_nilai.SelectCommand = "IsiNilai_Log_SELECT_BY_PROSES_RAPOR";
            if (isbind) lvListLogNilai.DataBind();
        }

        protected void InitFieldsDetail()
        {
            cboGuru.SelectedValue = "";
            cboMapel.SelectedValue = "";
            cboKelasDet.SelectedValue = "";
            txtAlasan.Text = "";
            chkKunciNilai.Checked = false;
        }

        protected void btnShowInputKunci_Click(object sender, EventArgs e)
        {
            txtIDItem.Value = "";
            InitFieldsDetail();
            txtKeyAction.Value = JenisAction.DoShowInputLog.ToString();
        }

        protected void btnShowEditKunci_Click(object sender, EventArgs e)
        {
            InitFieldsDetail();
            IsiNilai_Log m = DAO_IsiNilai_Log.GetByID_Entity(txtIDItem.Value);
            if (m != null)
            {
                if (m.Alasan != null)
                {
                    cboGuru.SelectedValue = m.Rel_Guru;
                    cboMapel.SelectedValue = m.Rel_Mapel;
                    cboKelasDet.SelectedValue = m.Rel_KelasDet;
                    txtAlasan.Text = m.Alasan;
                    chkKunciNilai.Checked = m.IsClosed;
                }
            }
            txtKeyAction.Value = JenisAction.DoShowEditLog.ToString();
        }

        protected void lnkOKLog_Click(object sender, EventArgs e)
        {
            if (txtIDItem.Value.Trim() == "")
            {
                DAO_IsiNilai_Log.Insert(
                        new IsiNilai_Log {
                            Tanggal = DateTime.Now,
                            Rel_ProsesRapor = txtID.Value,
                            Rel_Sekolah = "",
                            Rel_Guru = cboGuru.SelectedValue,
                            Rel_Mapel = cboMapel.SelectedValue,
                            Rel_KelasDet = cboKelasDet.SelectedValue,
                            Alasan = txtAlasan.Text,
                            Keterangan = "",
                            IsClosed = false,
                            UserIDOpened = Libs.LOGGED_USER_M.UserID,
                            UserIDClosed = ""
                        }
                    );
            }
            else
            {
                IsiNilai_Log m = DAO_IsiNilai_Log.GetByID_Entity(txtIDItem.Value);
                if (m != null)
                {
                    if (m.Alasan != null)
                    {
                        if (m.IsClosed)
                        {
                            txtKeyAction.Value = "Data tidak bisa diubah";
                            return;
                        }

                        m.Rel_Guru = cboGuru.SelectedValue;
                        m.Rel_Mapel = cboMapel.SelectedValue;
                        m.Rel_KelasDet = cboKelasDet.SelectedValue;
                        m.Alasan = txtAlasan.Text;
                        m.Keterangan = "";
                        m.IsClosed = chkKunciNilai.Checked;
                        if (chkKunciNilai.Checked)
                        {
                            m.UserIDClosed = Libs.LOGGED_USER_M.UserID;
                        }
                        else
                        {
                            m.UserIDClosed = "";
                        }

                        DAO_IsiNilai_Log.Update(
                           m
                       );
                    }
                }
            }
            BindListViewLogKunciNilai(txtID.Value, true);
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }
    }
}