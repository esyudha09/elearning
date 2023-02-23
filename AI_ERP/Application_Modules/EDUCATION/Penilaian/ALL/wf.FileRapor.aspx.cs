using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs.Elearning;

using AI_ERP.Application_Entities.Elearning.KB;
using AI_ERP.Application_DAOs.Elearning.KB;

using AI_ERP.Application_Entities.Elearning.TK;
using AI_ERP.Application_DAOs.Elearning.TK;

using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;

using AI_ERP.Application_Entities.Elearning.SMP;
using AI_ERP.Application_DAOs.Elearning.SMP;

using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.ALL
{
    public partial class wf_FileRapor : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAFILERAPOR";

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
            DoShowPengaturan,
            DoShowUploadFileRapor,
            DoShowListKenaikanKelas,
            DoShowInputHalaman,
            DoShowListNilai,
            DoShowKontenEmailRapor,
            DoShowShowFileRapor,
            DoShowKonfirmasiEmailRapor,
            DoShowListFileRapor,
            DoShowListFileRaporDet,
            DoShowListFileRaporFromBack,
            DoShowListFileRaporAfterProses,
            DoShowUbahEmail,
            DoKirimEmail,
            DoShowListHistoryEmailRapor,
            DoRefresh
        }

        public enum JenisLihatCetak
        {
            LTS = 0,
            RAPOR = 1,
            RAPOR_URAIAN = 2
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

            public static string GetTipeRapor()
            {
                return Libs.GetQueryString("tr");
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

        public static Libs.UnitSekolah GetUnitSekolah()
        {
            if (QS.GetUnit().Trim() != "")
            {
                Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(QS.GetUnit());
                if (m_sekolah != null)
                {
                    if (m_sekolah.Nama != null)
                    {
                        return (Libs.UnitSekolah)m_sekolah.UrutanJenjang;
                    }
                }
            }

            return Libs.UnitSekolah.SALAH;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/packing-3.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "File Rapor";
            string tipe_rapor = "";
            if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.LTS.ToUpper().Trim())
            {
                tipe_rapor = "&nbsp;<span class=\"badge\" style=\"background-color: white; font-weight: bold; color: red;\">LTS</span>";
            }
            else if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER.ToUpper().Trim())
            {
                tipe_rapor = "&nbsp;<span class=\"badge\" style=\"background-color: white; font-weight: bold; color: green;\">Semester</span>";
            }
            this.Master.HeaderTittle += tipe_rapor;
            ltrTipeRapor.Text = tipe_rapor;

            this.Master.txtCariData.Visible = false;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                txtTipeRapor.Value = QS.GetTipeRapor().ToUpper().Trim();

                InitKeyEventClient();
                cboPeriode.Items.Clear();
                switch (GetUnitSekolah())
                {
                    case Libs.UnitSekolah.SALAH:
                        break;
                    case Libs.UnitSekolah.KB:
                        foreach (var item in AI_ERP.Application_DAOs.Elearning.KB.DAO_Rapor_Design.GetAll_Entity().Select(
                        m => new { m.TahunAjaran, m.Semester }).Distinct().OrderByDescending(m => m.TahunAjaran).ThenByDescending(m => m.Semester).ToList())
                        {
                            cboPeriode.Items.Add(new ListItem
                            {
                                Value = item.TahunAjaran + "|" + item.Semester,
                                Text = item.TahunAjaran + " semester " + item.Semester
                            });
                        }
                        break;
                    case Libs.UnitSekolah.TK:
                        foreach (var item in AI_ERP.Application_DAOs.Elearning.TK.DAO_Rapor_Design.GetAll_Entity().Select(
                        m => new { m.TahunAjaran, m.Semester }).Distinct().OrderByDescending(m => m.TahunAjaran).ThenByDescending(m => m.Semester).ToList())
                        {
                            cboPeriode.Items.Add(new ListItem
                            {
                                Value = item.TahunAjaran + "|" + item.Semester,
                                Text = item.TahunAjaran + " semester " + item.Semester
                            });
                        }

                        break;
                    case Libs.UnitSekolah.SD:
                        foreach (var item in AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai.GetDistinctTahunAjaranPeriode_Entity())
                        {
                            cboPeriode.Items.Add(new ListItem
                            {
                                Value = item.TahunAjaran + "|" + item.Semester,
                                Text = item.TahunAjaran + " semester " + item.Semester
                            });
                        }
                        break;
                    case Libs.UnitSekolah.SMP:
                        foreach (var item in AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.GetDistinctTahunAjaranPeriode_Entity())
                        {
                            cboPeriode.Items.Add(new ListItem
                            {
                                Value = item.TahunAjaran + "|" + item.Semester,
                                Text = item.TahunAjaran + " semester " + item.Semester
                            });
                        }
                        break;
                    case Libs.UnitSekolah.SMA:
                        foreach (var item in AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai.GetDistinctTahunAjaranPeriode_Entity())
                        {
                            cboPeriode.Items.Add(new ListItem
                            {
                                Value = item.TahunAjaran + "|" + item.Semester,
                                Text = item.TahunAjaran + " semester " + item.Semester
                            });
                        }
                        break;
                    default:
                        break;
                }

                div_halaman_rapor.Attributes["style"] = "display: none;";
                switch (GetUnitSekolah())
                {
                    case Libs.UnitSekolah.SALAH:
                        break;
                    case Libs.UnitSekolah.KB:
                        txtURLProsesRapor.Value = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.NILAI_SISWA_PRINT.FILE);
                        break;
                    case Libs.UnitSekolah.TK:
                        txtURLProsesRapor.Value = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.NILAI_SISWA_PRINT.FILE);
                        break;
                    case Libs.UnitSekolah.SD:
                        txtURLProsesRapor.Value = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_PRINT.FILE);
                        break;
                    case Libs.UnitSekolah.SMP:
                        if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER)
                        {
                            div_halaman_rapor.Attributes["style"] = "";
                        }
                        txtURLProsesRapor.Value = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_PRINT.FILE);
                        break;
                    case Libs.UnitSekolah.SMA:
                        if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER)
                        {
                            div_halaman_rapor.Attributes["style"] = "";
                        }
                        txtURLProsesRapor.Value = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.FILE);
                        break;
                    default:
                        break;
                }
            }

            if (mvMain.ActiveViewIndex == 0)
            {
                BindListView(!IsPostBack, Libs.GetQ());
                if (!IsPostBack) this.Master.txtCariData.Text = Libs.GetQ();
            }
        }

        protected string GetUnit()
        {
            if (QS.GetUnit().Trim() != "")
            {
                Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(QS.GetUnit());
                if (m_sekolah != null)
                {
                    if (m_sekolah.Nama != null)
                    {
                        return m_sekolah.Kode.ToString();
                    }
                }
            }

            return "";
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            cboPeriode.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
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

            txtTahunAjaran.Value = tahun_ajaran;
            txtTahunAjaranEnc.Value = RandomLibs.GetRndTahunAjaran(tahun_ajaran);
            txtSemester.Value = semester;
            ltrPeriode.Text = tahun_ajaran + "&nbsp;<sup style=\"color: yellow;\">" + semester + "</sup>";

            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds.SelectParameters.Clear();
            switch (GetUnitSekolah())
            {
                case Libs.UnitSekolah.SALAH:
                    break;
                case Libs.UnitSekolah.KB:
                    sql_ds.SelectCommand = AI_ERP.Application_DAOs.Elearning.KB.DAO_Rapor_Design.SP_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_DISTINCTT_BY_TIPERAPOR;
                    sql_ds.SelectParameters.Add("TipeRapor", QS.GetTipeRapor());
                    break;
                case Libs.UnitSekolah.TK:
                    sql_ds.SelectCommand = AI_ERP.Application_DAOs.Elearning.TK.DAO_Rapor_Design.SP_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_BY_TIPERAPOR;
                    sql_ds.SelectParameters.Add("TipeRapor", QS.GetTipeRapor());
                    break;
                case Libs.UnitSekolah.SD:
                    sql_ds.SelectCommand = AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Nilai.SP_SELECT_PENILAIAN;
                    break;
                case Libs.UnitSekolah.SMP:
                    sql_ds.SelectCommand = AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Nilai.SP_SELECT_PENILAIAN;
                    break;
                case Libs.UnitSekolah.SMA:
                    sql_ds.SelectCommand = AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Nilai.SP_SELECT_PENILAIAN;
                    break;
                default:
                    break;
            }            
            sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
            sql_ds.SelectParameters.Add("Semester", semester);
            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_guru_kelas = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_guru_kelas");
            System.Web.UI.WebControls.Literal imgh_guru_pendamping = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_guru_pendamping");
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

            imgh_guru_kelas.Text = html_image;
            imgh_guru_pendamping.Text = html_image;
            imgh_kelas.Text = html_image;

            imgh_guru_kelas.Visible = false;
            imgh_guru_pendamping.Visible = false;
            imgh_kelas.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "GuruKelas":
                    imgh_guru_kelas.Visible = true;
                    break;
                case "GuruPendamping":
                    imgh_guru_pendamping.Visible = true;
                    break;
                case "Kelas":
                    imgh_kelas.Visible = true;
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
        }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                BindListView(!IsPostBack, Libs.GetQ().Trim());
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

            }
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            Response.Redirect(Libs.FILE_PAGE_URL + (this.Master.txtCariData.Text.Trim() != "" ? "?q=" + this.Master.txtCariData.Text : ""));
        }

        protected void btnDoPengaturan_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowPengaturan.ToString();
        }
        
        protected void btnShowListKenaikanKelas_Click(object sender, EventArgs e)
        {
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.DoShowListKenaikanKelas.ToString();
        }
        
        protected void ShowKontenEmailRapor()
        {
            if (txtIDSiswa.Value.Trim() != "")
            {
                Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                        txtTahunAjaran.Value,
                        txtSemester.Value,
                        txtIDSiswa.Value
                    );

                if (m_siswa != null)
                {
                    if (m_siswa.Nama != null)
                    {
                        txtIDEmail.Value = Guid.NewGuid().ToString();
                        txtEMailRapor.Text = m_siswa.Email.ToString().ToLower();
                        ltrEMailRaporContent.Text = GetHTMLEmailRapor(txtTahunAjaran.Value, txtSemester.Value, txtIDSiswa.Value, txtIDEmail.Value, true).Body;
                    }
                }

                txtKeyAction.Value = JenisAction.DoShowKontenEmailRapor.ToString();
            }
        }

        protected class EmailPengumumanLTSDanRapor
        {
            public string Subjek { get; set; }
            public string Body { get; set; }
        }

        protected EmailPengumumanLTSDanRapor GetHTMLEmailRapor(string tahun_ajaran, string semester, string rel_siswa, string kode_email, bool is_admin)
        {
            EmailPengumumanLTSDanRapor hasil = new EmailPengumumanLTSDanRapor();

            string html = "";

            Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                    tahun_ajaran, semester, rel_siswa
                );

            string s_html_template = "";
            string teks_link_buka_rapor = "Download File";

            if (m_siswa != null)
            {
                if (m_siswa.Nama != null)
                {

                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(QS.GetUnit());
                    if (m_sekolah != null)
                    {
                        if (m_sekolah.Nama != null)
                        {

                            string s_kelas_det = "";
                            KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet);
                            if (m_kelas != null)
                            {
                                switch ((Libs.UnitSekolah)m_sekolah.UrutanJenjang)
                                {
                                    case Libs.UnitSekolah.SALAH:
                                        break;
                                    case Libs.UnitSekolah.KB:
                                        Application_Entities.Elearning.KB.Rapor_Arsip m_arsip_kb =
                                            Application_DAOs.Elearning.KB.DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                                    m0 => m0.TahunAjaran == tahun_ajaran && m0.Semester == semester && 
                                                          m0.JenisRapor == (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER.ToUpper().Trim() ? "Semester" : "LTS")
                                                ).FirstOrDefault();
                                        if (m_arsip_kb != null)
                                        {
                                            if (m_arsip_kb.TahunAjaran != null)
                                            {
                                                s_html_template = m_arsip_kb.TemplateEmailRapor;
                                                hasil.Subjek = "RAPOR SEMESTER " + semester + " TP " + tahun_ajaran;
                                            }
                                        }
                                        break;
                                    case Libs.UnitSekolah.TK:
                                        Application_Entities.Elearning.TK.Rapor_Arsip m_arsip_tk =
                                            Application_DAOs.Elearning.TK.DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                                    m0 => m0.TahunAjaran == tahun_ajaran && m0.Semester == semester &&
                                                          m0.JenisRapor == (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER.ToUpper().Trim() ? "Semester" : "LTS")
                                                ).FirstOrDefault();
                                        if (m_arsip_tk != null)
                                        {
                                            if (m_arsip_tk.TahunAjaran != null)
                                            {
                                                s_html_template = m_arsip_tk.TemplateEmailRapor;
                                                hasil.Subjek = "RAPOR SEMESTER " + semester + " TP " + tahun_ajaran;
                                            }
                                        }
                                        break;
                                    case Libs.UnitSekolah.SD:
                                        Application_Entities.Elearning.SD.Rapor_Pengaturan m_pengaturan_sd =
                                            Application_DAOs.Elearning.SD.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(
                                                    m0 => m0.TahunAjaran == tahun_ajaran && m0.Semester == semester
                                                ).FirstOrDefault();
                                        if (m_pengaturan_sd != null)
                                        {
                                            if (m_pengaturan_sd.TahunAjaran != null)
                                            {
                                                if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.LTS.ToUpper().Trim())
                                                {
                                                    s_html_template = m_pengaturan_sd.TemplateEmailLTS;
                                                    hasil.Subjek = "LTS SEMESTER " + semester + " TP " + tahun_ajaran;
                                                }
                                                else if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER.ToUpper().Trim())
                                                {
                                                    s_html_template = m_pengaturan_sd.TemplateEmailRapor;
                                                    hasil.Subjek = "RAPOR SEMESTER " + semester + " TP " + tahun_ajaran;
                                                }
                                            }
                                        }
                                        break;
                                    case Libs.UnitSekolah.SMP:
                                        Application_Entities.Elearning.SMP.Rapor_Pengaturan m_pengaturan_smp =
                                            Application_DAOs.Elearning.SMP.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(
                                                    m0 => m0.TahunAjaran == tahun_ajaran && m0.Semester == semester
                                                ).FirstOrDefault();
                                        if (m_pengaturan_smp != null)
                                        {
                                            if (m_pengaturan_smp.TahunAjaran != null)
                                            {
                                                if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.LTS.ToUpper().Trim())
                                                {
                                                    s_html_template = m_pengaturan_smp.TemplateEmailLTS;
                                                    hasil.Subjek = "LTS SEMESTER " + semester + " TP " + tahun_ajaran;
                                                }
                                                else if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER.ToUpper().Trim())
                                                {
                                                    s_html_template = m_pengaturan_smp.TemplateEmailRapor;
                                                    hasil.Subjek = "RAPOR SEMESTER " + semester + " TP " + tahun_ajaran;
                                                }
                                            }
                                        }
                                        break;
                                    case Libs.UnitSekolah.SMA:
                                        Application_Entities.Elearning.SMA.Rapor_Pengaturan m_pengaturan_sma =
                                            Application_DAOs.Elearning.SMA.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(
                                                    m0 => m0.TahunAjaran == tahun_ajaran && m0.Semester == semester
                                                ).FirstOrDefault();
                                        if (m_pengaturan_sma != null)
                                        {
                                            if (m_pengaturan_sma.TahunAjaran != null)
                                            {
                                                if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.LTS.ToUpper().Trim())
                                                {
                                                    s_html_template = m_pengaturan_sma.TemplateEmailLTS;
                                                    hasil.Subjek = "LTS SEMESTER " + semester + " TP " + tahun_ajaran;
                                                }
                                                else if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER.ToUpper().Trim())
                                                {
                                                    s_html_template = m_pengaturan_sma.TemplateEmailRapor;
                                                    hasil.Subjek = "RAPOR SEMESTER " + semester + " TP " + tahun_ajaran;
                                                }
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }

                                if (m_kelas.Nama != null)
                                {
                                    s_kelas_det = m_kelas.Nama;
                                    html = s_html_template.Replace("@nama", Libs.GetPerbaikiEjaanNama(m_siswa.Nama));
                                    html = html.Replace("@tahun_ajaran", tahun_ajaran);
                                    html = html.Replace("@semester", semester);
                                    html = html.Replace("@ayah", m_siswa.NamaAyah);
                                    html = html.Replace("@ibu", m_siswa.NamaIbu);
                                    html = html.Replace("@nis", m_siswa.NISSekolah);
                                    html = html.Replace("@kelas", s_kelas_det);
                                    html = html.Replace("@wali_kelas", m_kelas.Nama);
                                    if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.SEMESTER)
                                    {
                                        html = html.Replace("@link_teks",
                                                                Libs.GetHTMLLinkRapor(
                                                                    this.Page,
                                                                    tahun_ajaran,
                                                                    semester,
                                                                    m_siswa.Rel_KelasDet,
                                                                    m_siswa.Kode.ToString(),
                                                                    false,
                                                                    teks_link_buka_rapor,
                                                                    kode_email,
                                                                    QS.GetTipeRapor(),
                                                                    (is_admin ? "&act=" + Constantas.TOKEN_ADMIN : "")
                                                                )
                                                            );
                                        html = html.Replace("@link_tombol",
                                                                Libs.GetHTMLLinkRapor(
                                                                    this.Page,
                                                                    tahun_ajaran,
                                                                    semester,
                                                                    m_siswa.Rel_KelasDet,
                                                                    m_siswa.Kode.ToString(),
                                                                    true,
                                                                    teks_link_buka_rapor,
                                                                    kode_email,
                                                                    QS.GetTipeRapor(),
                                                                    (is_admin ? "&act=" + Constantas.TOKEN_ADMIN : "")
                                                                )
                                                            );
                                    }
                                    else if (QS.GetTipeRapor().ToUpper().Trim() == TipeRapor.LTS)
                                    {
                                        html = html.Replace("@link_teks",
                                                                Libs.GetHTMLLinkLTS(
                                                                    this.Page,
                                                                    tahun_ajaran,
                                                                    semester,
                                                                    m_siswa.Rel_KelasDet,
                                                                    m_siswa.Kode.ToString(),
                                                                    false,
                                                                    teks_link_buka_rapor,
                                                                    kode_email,
                                                                    QS.GetTipeRapor(),
                                                                    (is_admin ? "&act=" + Constantas.TOKEN_ADMIN : "")
                                                                )
                                                            );
                                        html = html.Replace("@link_tombol",
                                                                Libs.GetHTMLLinkLTS(
                                                                    this.Page,
                                                                    tahun_ajaran,
                                                                    semester,
                                                                    m_siswa.Rel_KelasDet,
                                                                    m_siswa.Kode.ToString(),
                                                                    true,
                                                                    teks_link_buka_rapor,
                                                                    kode_email,
                                                                    QS.GetTipeRapor(),
                                                                    (is_admin ? "&act=" + Constantas.TOKEN_ADMIN : "")
                                                                )
                                                            );
                                    }
                                }

                            }
                        }
                    }

                }
            }

            hasil.Body = Libs.GetHTMLEmailTemplate(html, GetUnit(), true);
            return hasil;
        }

        protected void btnBindDataList_Click(object sender, EventArgs e)
        {
            BindListView(true, Libs.GetQ().Trim());
        }

        protected void ShowFileRapor()
        {
            if (txtKelasDet.Value.Trim() != "")
            {
                string s_caption = "";
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(
                        txtKelasDet.Value
                    );
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        if (m_kelas_det.Nama.Length <= 5)
                        {
                            s_caption = "<span style=\"font-weight: bold; color: yellow;\">Kelas " + m_kelas_det.Nama + "</span>" +
                                        "&nbsp;<span style=\"font-weight: normal;\">" + txtTahunAjaran.Value + " Semester " + txtSemester.Value + "</span>";
                        }
                        else
                        {
                            s_caption = "<span style=\"font-weight: bold; color: yellow;\">" + m_kelas_det.Nama + "</span>" +
                                        "&nbsp;<span style=\"font-weight: normal;\">" + txtTahunAjaran.Value + " Semester " + txtSemester.Value + "</span>";
                        }

                        FormasiGuruKelas m_formasi_guru = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                                QS.GetUnit(), txtTahunAjaran.Value, txtSemester.Value
                            ).FindAll(m0 => m0.Rel_KelasDet.ToString().ToUpper().Trim() == m_kelas_det.Kode.ToString().ToUpper().Trim()).FirstOrDefault();

                        if (m_formasi_guru != null)
                        {
                            if (m_formasi_guru.Rel_GuruKelas != null)
                            {
                                Pegawai m_guru_kelas = DAO_Pegawai.GetByID_Entity(m_formasi_guru.Rel_GuruKelas);
                                Pegawai m_guru_pendamping = DAO_Pegawai.GetByID_Entity(m_formasi_guru.Rel_GuruPendamping);
                                if (m_guru_kelas != null)
                                {
                                    if (m_guru_kelas.Nama != null)
                                    {
                                        if (m_guru_kelas.Nama.Trim() != "")
                                        {
                                            s_caption += (s_caption.Trim() != "" ? "<br />" : "") +
                                                         m_guru_kelas.Nama.Trim();
                                        }
                                    }
                                }
                                if (m_guru_pendamping != null)
                                {
                                    if (m_guru_pendamping.Nama != null)
                                    {
                                        if (m_guru_pendamping.Nama.Trim() != "")
                                        {
                                            s_caption += (s_caption.Trim() != "" ? "&nbsp;&&nbsp;" : "") +
                                                         m_guru_pendamping.Nama.Trim();
                                        }
                                    }
                                }
                            }
                        }

                        ShowListFileRapor();
                        ltrCaptionSelectedKelas.Text = s_caption;
                        mvMain.ActiveViewIndex = 1;
                    }
                }
            }            
        }

        protected List<string> GetListUploadedFiles(string rel_siswa, string tahun_ajaran, string semester, string rel_kelas_det, Libs.UnitSekolah rel_sekolah)
        {
            List<string> hasil = new List<string>();

            if (QS.GetTipeRapor().Trim().ToUpper() == TipeRapor.LTS.Trim().ToUpper())
            {
                hasil = Libs.GetListUploadedFiles(this.Page, Libs.GetLokasiFolderFileLTS(
                        rel_siswa, tahun_ajaran, semester, rel_kelas_det, rel_sekolah
                    ));
            }
            else if (QS.GetTipeRapor().Trim().ToUpper() == TipeRapor.SEMESTER.Trim().ToUpper())
            {
                hasil = Libs.GetListUploadedFiles(this.Page, Libs.GetLokasiFolderFileRapor(
                        rel_siswa, tahun_ajaran, semester, rel_kelas_det, rel_sekolah
                    ));
            }

            return hasil;
        }

        protected void ShowListFileRapor()
        {
            ltrFileRapor.Text = "";

            string tahun_ajaran = txtTahunAjaran.Value;
            string semester = txtSemester.Value;
            string rel_kelas_det = txtKelasDet.Value;

            string html = "";
            string html_file_rapor = "";

            var lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                    GetUnit(),
                                    rel_kelas_det,
                                    tahun_ajaran,
                                    semester
                                );

            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(GetUnit());
            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {
                    int id = 1;
                    foreach (Siswa m_siswa in lst_siswa)
                    {
                        //get files
                        string s_badge = "";
                        List<string> lst_file = GetListUploadedFiles(
                                m_siswa.Kode.ToString(), txtTahunAjaran.Value, txtSemester.Value, rel_kelas_det, GetUnitSekolah()
                            );
                        s_badge = "<sup title=\" 0 File \" class=\"badge\" style=\"color: white; background-color: " + (lst_file.Count > 0 ? "green; " : "red; ") + "margin-top: 10px; margin-right: 15px; position: absolute; font-size: xx-small;padding: 3px; padding-left: 5px; padding-right: 5px;\">" +
                                    lst_file.Count.ToString() +
                                  "</sup>";
                        //end getfiles

                        string js_open_folder = "DoScrollPosFileRapor();" +
                                                txtIDSiswa.ClientID + ".value = '" + m_siswa.Kode.ToString() + "';" +
                                                btnBukaFileRaporDet.ClientID + ".click();";

                        string chk_id = "chk_" + m_siswa.Kode.ToString().Replace("-", "_");
                        html_file_rapor
                                += "<tr>" +
                                        "<td style=\"padding: 5px; width: 30px; text-align: center; font-weight: normal; color: #bfbfbf;\">" +
                                            id.ToString() + "." +
                                        "</td>" +
                                        "<td style=\"padding: 5px; text-align: left; font-weight: bold;\">" +
                                            "<div class=\"checkbox checkbox-adv\">" +
                                                "<label for=\"" + chk_id + "\">" +
                                                    "<input checked=\"checked\" " +
                                                            "name=\"chk_pilih_siswa_rapor[]\" " +
                                                            "value=\"" + m_siswa.Kode.ToString() + "\" " +
                                                            "class=\"access-hide\" id=\"" + chk_id + "\" " +
                                                            "type=\"checkbox\">" +
                                                    "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                                    "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                                        "&nbsp;&nbsp;" +
                                                        m_siswa.Nama.Trim().ToUpper() +
                                                    "</span>" +
                                                "</label>" +
                                            "</div>" +
                                        "</td>" +
                                        "<td style=\"padding: 5px; text-align: left; font-weight: normal;\">" +
                                            m_siswa.Email.ToLower().Trim() +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td colspan=\"3\" style=\"padding: 5px; text-align: center; font-weight: normal; vertical-align: center;\">" +
                                            "<hr style=\"margin: 0px;\" />" +
                                        "</td>" +
                                    "</tr>";

                        string s_panggilan = "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                              "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";
                        List<MailJobs> lst_mail_jobs = DAO_MailJobs.GetPengumumanRaporBySiswa_Entity(m_siswa.Kode.ToString()).OrderByDescending(m0 => m0.Tanggal).ToList();

                        string s_event_hist_rapor = " onclick=\"" +
                                                                txtIDSiswa.ClientID + ".value = '" + m_siswa.Kode.ToString() + "'; " +
                                                                btnShowKontenHistEmailRapor.ClientID + ".click(); " +
                                                           "\" ";

                        List<Rapor_ViewEmailLTS> lst_view_email =
                            DAO_Rapor_ViewEmailLTS.GetBySiswa_Entity(m_siswa.Kode.ToString()).OrderByDescending(m0 => m0.Tanggal).ToList();
                        string html_dilihat = "";
                        if (lst_view_email.Count > 0)
                        {
                            html_dilihat = "<label title=\" Dilihat Tanggal \" style=\"font-size: small; color: gray; font-weight: bold; min-width: 200px; width: 200px; max-width: 200px; text-align: right;\">" +
                                                "<i class=\"fa fa-eye\"></i>&nbsp;&nbsp;" +
                                                Libs.GetTanggalIndonesiaFromDate(lst_view_email.FirstOrDefault().Tanggal, true) +
                                           "</label>" +
                                           "&nbsp;&nbsp;|&nbsp;&nbsp;";
                        }
                        else
                        {
                            html_dilihat = "<label style=\"font-size: small; color: gray; font-weight: bold; min-width: 200px; width: 200px; max-width: 200px; text-align: right;\">" +
                                                "&nbsp;&nbsp;" +
                                           "</label>" +
                                           "<label style=\"visibility: hidden;\">" +
                                                "&nbsp;&nbsp;|&nbsp;&nbsp;" +
                                           "</label>";
                        }

                        html +=
                                "<div class=\"row\" style=\"" + 
                                (
                                    txtIDSiswa.Value.ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim() 
                                    ? " background-color: #e2f5f8; " 
                                    : (
                                        m_siswa.Email.Trim().ToLower().Trim() != "" && m_siswa.Email.Trim().ToLower().Trim().Length > 3
                                        ? ""
                                        : " background-color: #fff6d9; "
                                      )
                                ) + " padding-left: 15px; padding-right: 15px; " + (id == 1 ? "margin-top: 10px;" : "") + "\">" +
                                    "<div class=\"col-xs-1\" style=\"color: #bfbfbf; padding-top: 20px; padding-left: 5px; width: 30px; font-weight: normal;\">" +
                                        id.ToString() + "." +
                                    "</div>" +
                                    "<div class=\"col-xs-11\" style=\"padding-top: 7px; padding-left: 0px; padding-bottom: 0px;\">" +

                                        "<div class=\"tile-wrap\" style=\"margin: 0px;\">" +
							                "<div class=\"tile\" style=\"box-shadow: none; background-color: inherit;\">" +
                                                "<div onclick=\"" + js_open_folder + "\" class=\"tile-side pull-left\" style=\"cursor: pointer; min-width: 40px;\" title=\" Buka Folder \">" +
                                                    "<div onclick=\"" + js_open_folder + "\" class=\"avatar avatar-sm\" style=\"cursor: pointer;\" title=\" Buka Folder \">" +
                                                        "<i class=\"fa fa-folder-open\" style=\"color: #ecb400; cursor: pointer;\" title=\" Buka Folder \"></i>" +
                                                        s_badge +
									                "</div>" +
								                "</div>" +
                                                "<div class=\"tile-inner\" style=\"margin: 0px; margin-left: 15px;\">" +

                                                    "<div class=\"checkbox checkbox-adv\">" +
                                                        "<label for=\"" + chk_id + "\">" +
                                                            "<input " +
                                                                    "name=\"chk_pilih_siswa_rapor[]\" " +
                                                                    "value=\"" + m_siswa.Kode.ToString() + "\" " +
                                                                    "class=\"access-hide\" id=\"" + chk_id + "\" " +
                                                                    "type=\"checkbox\">" +
                                                            "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                                            "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                                                "&nbsp;&nbsp;" +
                                                                m_siswa.Nama.Trim().ToUpper() +
                                                            "</span>" +
                                                            "<span style=\"font-size: 14px; color: #1DA1F2; font-weight: bold;\">" +
                                                                "<br />" +
                                                                "&nbsp;&nbsp;" +
                                                                (
                                                                    m_siswa.Email.Trim().ToLower().Trim() != "" && m_siswa.Email.Trim().ToLower().Trim().Length > 3
                                                                    ? "<i class=\"fa fa-envelope-o\" style=\"color: #bfbfbf;\"></i>&nbsp;&nbsp;" +
                                                                      m_siswa.Email.Trim().ToLower()
                                                                    : "<i class=\"fa fa-exclamation-triangle\" style=\"color: #bfbfbf;\"></i>&nbsp;&nbsp;" +
                                                                      "<span style=\"color: #bfbfbf; font-style: italic; font-weight: normal;\">tidak ada email</span>"
                                                                ) +
                                                            "</span>" +
                                                        "</label>" +
                                                    "</div>" +
                                                    "<div style=\"position: absolute; right: -40px; top: 16px; color: #bfbfbf;\">" +
                                                        "<label onclick=\"" + txtIDSiswa.ClientID + ".value = '" + m_siswa.Kode.ToString() + "'; " + btnShowUbahEmail.ClientID + ".click();\" title=\" Ubah Email \" style=\"margin-left: 0px; cursor: pointer; padding-left: 7px;\">" +
                                                            "<i class=\"fa fa-pencil\" style=\"color: lightcoral;\"></i>" +
                                                        "</label>" +
                                                        (
                                                            m_siswa.Email.Trim().ToLower().Trim() != "" && m_siswa.Email.Trim().ToLower().Trim().Length > 3
                                                            ? "&nbsp;&nbsp;&nbsp;&nbsp;" +
                                                                html_dilihat +
                                                                "<label onclick=\"" + txtIDSiswa.ClientID + ".value = '" + m_siswa.Kode.ToString() + "'; " + btnShowKontenEmailFileRapor.ClientID + ".click();\" title=\" Lihat Konten Email \" style=\"cursor: pointer; font-size: small; color: #00198d; font-weight: bold;\">" +
                                                                    "<i class=\"fa fa-envelope\"></i>&nbsp;&nbsp;Konten Email" +
                                                                "</label>" +
                                                                "&nbsp;&nbsp;|&nbsp;&nbsp;" +
                                                                (
                                                                lst_mail_jobs.Count == 0
                                                                ? "<label title=\" Lihat Status Pengiriman Email \" style=\"cursor: default; font-size: small; color: #bfbfbf; font-weight: bold;\">" +
                                                                        "<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;Status" +
                                                                    "</label>"
                                                                : (
                                                                    lst_mail_jobs.FirstOrDefault().Status == DAO_MailJobs.MSG_EMAIL_TERKIRIM
                                                                    ? "<label " + s_event_hist_rapor + " title=\" Lihat Status Pengiriman Email \" style=\"cursor: pointer; font-size: small; color: #00198d; font-weight: bold;\">" +
                                                                            "<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;Status" +
                                                                        "</label>"
                                                                    : (
                                                                        lst_mail_jobs.FirstOrDefault().Status == DAO_MailJobs.MSG_STATUS_MENGIRIM
                                                                        ? "<label " + s_event_hist_rapor + " title=\" Lihat Status Pengiriman Email \" style=\"cursor: pointer; font-size: small; color: darkorange; font-weight: bold;\">" +
                                                                                "<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;Status" +
                                                                            "</label>"
                                                                        : "<label " + s_event_hist_rapor + " title=\" Lihat Status Pengiriman Email \" style=\"cursor: pointer; font-size: small; color: red; font-weight: bold;\">" +
                                                                                "<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;Status" +
                                                                            "</label>"
                                                                        )
                                                                    )
                                                                )
                                                            : ""
                                                        ) +
                                                    "</div>" +

                                                "</div>" +
							                "</div>" +
                                        "</div>" +
                                        
                                    "</div>" +
                                "</div>" +
                                (
                                    id < lst_siswa.Count
                                    ? "<hr style=\"margin: 0px; margin-left: -15px; margin-right: -15px;\" />"
                                    : ""
                                );

                        id++;

                    }

                }
            }

            html_file_rapor += "</table>";
            ltrFileRapor.Text = html;            
        }

        protected void ShowFileRaporDet()
        {
            if (txtKelasDet.Value.Trim() != "" && txtIDSiswa.Value.Trim() != "")
            {
                string s_caption = "";
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(
                        txtKelasDet.Value
                    );
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        if (m_kelas_det.Nama.Length <= 5)
                        {
                            s_caption = "<span style=\"font-weight: bold; color: yellow;\">Kelas " + m_kelas_det.Nama + "</span>" +
                                        "&nbsp;<span style=\"font-weight: normal;\">" + txtTahunAjaran.Value + " Semester " + txtSemester.Value + "</span>";
                        }

                        Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                                QS.GetUnit(), txtTahunAjaran.Value, txtIDSiswa.Value
                            );

                        if (m_siswa != null)
                        {
                            if (m_siswa.Nama != null)
                            {
                                s_caption += (s_caption.Trim() != "" ? "<br />" : "") +
                                             m_siswa.Nama.Trim().ToUpper();
                            }
                        }

                        ShowListFileRaporDet();
                        ltrCaptionSelectedSiswa.Text = s_caption;
                        mvMain.ActiveViewIndex = 2;
                    }
                }
            }
        }

        protected void ShowListFileRaporDet()
        {
            string html = "";
            Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                    txtTahunAjaran.Value,
                    txtSemester.Value,
                    txtIDSiswa.Value
                );
            
            if (m_siswa != null)
            {
                if (m_siswa.Nama != null)
                {
                    List<string> lst_file = GetListUploadedFiles(
                                m_siswa.Kode.ToString(), txtTahunAjaran.Value, txtSemester.Value, m_siswa.Rel_KelasDet, GetUnitSekolah()
                            );

                    if (lst_file.Count == 0)
                    {
                        html = "<div style=\"padding: 25px; text-align: center; font-weight: bold; color: darkorange; padding-top: 0px; padding-bottom: 0px;\"><i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;File Kosong</div>";
                    }
                    else
                    {
                        html = Libs.GetHTMLListUploadedFilesRapor(this.Page,
                                m_siswa.Kode.ToString(), txtTahunAjaran.Value, txtSemester.Value, m_siswa.Rel_KelasDet.ToString(), GetUnitSekolah()
                           , m_siswa.Kode.ToString(), QS.GetTipeRapor());
                    }                    
                }
            }

            ltrFileRaporDet.Text = html;
        }

        protected void btnBukaFileRapor_Click(object sender, EventArgs e)
        {
            ShowFileRapor();
            txtKeyAction.Value = JenisAction.DoShowShowFileRapor.ToString();
        }

        protected void lnkBackToList_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowListNilai.ToString();
            mvMain.ActiveViewIndex = 0;
        }

        protected void btnShowUbahEmail_Click(object sender, EventArgs e)
        {
            if (txtIDSiswa.Value.Trim() != "")
            {
                Siswa m_siswa = DAO_Siswa.GetByKode_Entity(txtTahunAjaran.Value, txtSemester.Value, txtIDSiswa.Value);
                if (m_siswa != null)
                {
                    if (m_siswa.Nama != null)
                    {
                        txtNamaSiswaEmail.Text = m_siswa.Nama.ToUpper().Trim();
                        txtEmailSiswaEmail.Text = m_siswa.Email.ToLower().Trim();
                        ShowFileRapor();
                    }
                }
            }
            txtKeyAction.Value = JenisAction.DoShowUbahEmail.ToString();
        }

        protected void lnkOKUpdateEmail_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIDSiswa.Value.Trim() != "")
                {
                    Siswa m_siswa = DAO_Siswa.GetByKode_Entity(txtTahunAjaran.Value, txtSemester.Value, txtIDSiswa.Value);
                    if (m_siswa != null)
                    {
                        if (m_siswa.Nama != null)
                        {
                            DAO_Siswa.UpdateEmail(txtIDSiswa.Value, txtEmailSiswaEmail.Text);
                            ShowFileRapor();
                            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnShowKontenEmailFileRapor_Click(object sender, EventArgs e)
        {
            ShowKontenEmailRapor();
        }
        
        protected void btnBukaFileRaporDet_Click(object sender, EventArgs e)
        {
            ShowFileRaporDet();
            mvMain.ActiveViewIndex = 2;
        }

        protected void lnkOKBackToFileRapor_Click(object sender, EventArgs e)
        {
            RefreshListFileRapor();
            txtKeyAction.Value = JenisAction.DoShowListFileRaporFromBack.ToString();
        }

        protected void RefreshListFileRapor()
        {
            ShowFileRapor();            
        }

        protected void btnBukaFileRaporDetAfterDeleted_Click(object sender, EventArgs e)
        {
            ShowFileRaporDet();
            mvMain.ActiveViewIndex = 2;
            txtKeyAction.Value = JenisAction.DoDelete.ToString();
        }

        protected void lnkOKUploadFileRapor_Click(object sender, EventArgs e)
        {
            ShowListFileRaporDet();
        }

        protected void btnShowUploadFileRapor_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowUploadFileRapor.ToString();
        }

        protected void lnkBagikanFileRapor_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowKonfirmasiEmailRapor.ToString();
        }

        protected void btnRefreshListFileRapor_Click(object sender, EventArgs e)
        {
            RefreshListFileRapor();
            txtKeyAction.Value = JenisAction.DoShowListFileRaporAfterProses.ToString();
        }

        protected void DoEmailRapor(string rel_siswa, Guid kode_email, string email_penerima)
        {
            string html_msg = "";
            List<MailJobs> lst_mail_jobs = DAO_MailJobs.GetPengumumanLTSBySiswa_Entity(
                rel_siswa
            ).OrderByDescending(m0 => m0.Tanggal).ToList(); ;

            string id_app = Constantas.ID_APP.ToString();
            string pengirim = "";
            string username = "";
            string smtp_address = "";
            string password = "";
            string subjek = "Informasi - Al Izhar Pondok Labu";
            subjek = "RAPOR SEMESTER " + txtSemester.Value + " TP " + txtTahunAjaran.Value;

            int port = Email.PORT_DEFAULT;

            if (lst_mail_jobs.Count > 0)
            {
                var m_mail_jobs = lst_mail_jobs.OrderByDescending(m0 => m0.Tanggal).FirstOrDefault();
                txtEMailRapor.Text = email_penerima;
                html_msg = m_mail_jobs.Message;
            }
            else
            {
                var email_rapor = GetHTMLEmailRapor(txtTahunAjaran.Value, txtSemester.Value, rel_siswa, kode_email.ToString(), false);
                subjek = email_rapor.Subjek;
                string msg = email_rapor.Body;
                html_msg = msg;
            }

            //cek by smtp list
            foreach (SMTP item_smtp in DAO_SMTP.GetDataByApp_Entity(id_app))
            {
                if (DAO_MailJobs.GetDataBySMTPBySenderByTanggal_Entity(item_smtp.Address, item_smtp.UserName, DateTime.Now).Count < item_smtp.MaxKirim)
                {
                    smtp_address = item_smtp.Address;
                    pengirim = item_smtp.DisplayName;
                    username = item_smtp.UserName;
                    password = item_smtp.Password;
                    port = int.Parse(item_smtp.Port);

                    break;
                }
            }

            if (username.Trim() != "")
            {
                DAO_MailJobs.InsertLengkap(new MailJobs
                {
                    Kode = kode_email,
                    Rel_Aplikasi = id_app,
                    Keterangan = "Pengumuman Rapor",
                    Sender = username,
                    SMTP = smtp_address,
                    Port = port.ToString(),
                    PWD = password,
                    SenderName = pengirim,
                    Tujuan = email_penerima,
                    ToPerson = rel_siswa,
                    Subjek = subjek,
                    Message = html_msg,
                    Tanggal = DateTime.Now,
                    Status = DAO_MailJobs.MSG_STATUS_MENGIRIM,
                    IsDone = false,
                    UserIDSender = Libs.LOGGED_USER_M.UserID,
                    LinkExpiredDate = DateTime.MinValue
                });
            }
        }

        protected void lnkOKKirimEmaiRapor_Click(object sender, EventArgs e)
        {
            try
            {
                DoEmailRapor(txtIDSiswa.Value, new Guid(txtIDEmail.Value), txtEMailRapor.Text);
                ShowFileRapor();
                txtKeyAction.Value = JenisAction.DoKirimEmail.ToString();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = "Email tidak valid : " + ex.Message;
            }
        }

        protected void btnOKKirimEmailRapor_Click(object sender, EventArgs e)
        {
            if (txtIDSiswaList.Value.Trim() != "")
            {
                string[] arr_siswa = txtIDSiswaList.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item_siswa in arr_siswa)
                {
                    Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                        txtTahunAjaran.Value,
                        txtSemester.Value,
                        item_siswa
                    );
                    if (m_siswa != null)
                    {
                        if (m_siswa.Nama != null)
                        {
                            DoEmailRapor(item_siswa, Guid.NewGuid(), m_siswa.Email.ToLower().Trim());
                        }
                    }
                }
            }
            ShowFileRapor();
        }

        protected void lnkRefreshFileRapor_Click(object sender, EventArgs e)
        {
            ShowFileRapor();
            txtKeyAction.Value = JenisAction.DoRefresh.ToString();
        }

        protected void ShowHistoryEmailRapor()
        {
            List<MailJobs> lst_mail_jobs = DAO_MailJobs.GetPengumumanRaporBySiswa_Entity(txtIDSiswa.Value).OrderByDescending(m0 => m0.Tanggal).ToList();
            string html = "<table style=\"margin: 0px; width: 100%;\">";
            int id = 1;
            foreach (var item in lst_mail_jobs)
            {
                List<RaporViewEmail> lst_view_email =
                    DAO_RaporViewEmail.GetByEmail_Entity(item.Kode.ToString()).OrderByDescending(m0 => m0.Tanggal).ToList();
                string html_dilihat = "";

                foreach (var item_dilihat in lst_view_email)
                {
                    html_dilihat +=
                        "<li style=\"font-weight: normal;\">" +
                            (html_dilihat.Trim() != "" ? "<hr style=\"margin: 0px;\" />" : "") +
                            Libs.GetTanggalIndonesiaFromDate(item_dilihat.Tanggal, true) +
                        "</li>";
                }

                html += "<tr>" +
                            "<td style=\"padding: 10px; font-weight: bold; text-align: center; vertical-align: top;\">" +
                                id.ToString() +
                            "</td>" +
                            "<td style=\"padding: 10px; font-weight: normal; text-align: left; vertical-align: top;\">" +
                                "<span style=\"font-weight: bold; color: #1DA1F2;\">" +
                                    "<i title=\" Email \" class=\"fa fa-envelope-o\"></i>&nbsp;&nbsp;" + item.Tujuan +
                                "</span>" +
                                "<br />" +
                                "<span style=\"font-weight: normal; color: black;\">" +
                                    "<i title=\" Dikirim Tanggal \" class=\"fa fa-clock-o\"></i>&nbsp;&nbsp;" + Libs.GetTanggalIndonesiaFromDate(item.Tanggal, true) +
                                "</span>" +
                                "<br />" +
                                "<span style=\"font-weight: normal; color: black;\">" +
                                    "<i title=\" User ID Pengirim \" class=\"fa fa-user-circle-o\"></i>&nbsp;&nbsp;" + item.UserIDSender +
                                "</span>" +
                                "<br />" +
                                (
                                    item.Status == DAO_MailJobs.MSG_EMAIL_TERKIRIM
                                    ? "<label style=\"font-weight: normal; background-color: #E7F7FF; color: #446D8C; border-radius: 4px; padding: 5px; border-style: solid; border-width: 1px; border-color: #3298C8; margin-top: 5px; width: 100%;\">" +
                                        "<i title=\" Status \" class=\"fa fa-check-circle-o\"></i>&nbsp;&nbsp;" + item.Status +
                                      "</label>"
                                    : (
                                        item.Status == DAO_MailJobs.MSG_STATUS_MENGIRIM
                                        ? "<label style=\"font-weight: normal; background-color: #FFF4E3; color: #a76501; border-radius: 4px; padding: 5px; border-style: solid; border-width: 1px; border-color: #EEB45E; margin-top: 5px; width: 100%;\">" +
                                            "<i title=\" Status \" class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;" + item.Status +
                                          "</label>"
                                        : "<label style=\"font-weight: normal; background-color: #FFD3EA; color: #E90080; border-radius: 4px; padding: 5px; border-style: solid; border-width: 1px; border-color: #E90080; margin-top: 5px; width: 100%;\">" +
                                            "<i title=\" Status \" class=\"fa fa-exclamation-circle\"></i>&nbsp;&nbsp;" + item.Status +
                                          "</label>"
                                      )
                                ) +
                                (
                                    html_dilihat.Trim() != ""
                                    ? "<label style=\"font-weight: bold; background-color: #E7F7FF; color: #446D8C; border-radius: 4px; padding: 5px; border-style: solid; border-width: 1px; border-color: #3298C8; margin-top: 5px; width: 100%; font-weight: bold;\">" +
                                        "<i title=\" Status \" class=\"fa fa-eye\"></i>&nbsp;&nbsp;Dilihat tanggal : <br />" +
                                        "<ul style=\"margin-top: 0px; margin-bottom: 0px;\">" +
                                            html_dilihat +
                                        "</ul>" +
                                      "</label>"
                                    : ""
                                ) +
                            "</td>" +
                        "</tr>";
                id++;
            }
            if (lst_mail_jobs.Count == 0)
            {
                html += "<tr>" +
                            "<td colspan=\"6\" style=\"padding: 10px; font-weight: bold; text-align: center;\">" +
                                "..:: Data Kosong ::.." +
                            "</td>" +
                        "</tr>";
            }
            html += "</table>";
            ltrHistoryEmailRapor.Text = html;
        }

        protected void btnShowKontenHistEmailRapor_Click(object sender, EventArgs e)
        {
            ShowHistoryEmailRapor();
            ShowFileRapor();
            txtKeyAction.Value = JenisAction.DoShowListHistoryEmailRapor.ToString();
        }
    }
}