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
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
{
    public partial class wf_ListNilaiSiswa : System.Web.UI.Page
    {
        public static string s_url_ok = AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CETAK_LTS.ROUTE;
        public static string s_url_rapor_ok = AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_PRINT.ROUTE;
        public const string SessionViewDataName = "PAGEDATALISTNILAI_SD";

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
            DoShowListKenaikanKelas,
            DoShowInputHalaman,
            DoShowListNilai,
            DoShowShowEmailLTS,
            DoShowKontenEmailLTS
        }

        public enum JenisLihatCetak
        {
            LTS = 0,
            RAPOR = 1,
            RAPOR_URAIAN = 2
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/office-material-3.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Data Penilaian";

            this.Master.txtCariData.Visible = false;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitKeyEventClient();
                cboPeriode.Items.Clear();
                foreach (var item in DAO_Rapor_StrukturNilai.GetDistinctTahunAjaranPeriode_Entity())
                {
                    cboPeriode.Items.Add(new ListItem {
                        Value = item.TahunAjaran + "|" + item.Semester,
                        Text = item.TahunAjaran + " semester " + item.Semester
                    });
                }
            }
            BindListView(!IsPostBack, Libs.GetQ());
            if (!IsPostBack) this.Master.txtCariData.Text = Libs.GetQ();
        }

        public static string GetJenisDownloadNilaiRapor(string tahun_ajaran, string semester, string nama_kelas, string jenis_rapor)
        {
            //get jenis download kurikulum
            string s_jenis_download_key = "";
            if (jenis_rapor == "0")
            {
                s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
            }
            else if (jenis_rapor == "1")
            {
                s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
            }
            else if (jenis_rapor == "2")
            {
                return AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_LTS_SD;
            }

            nama_kelas = nama_kelas.Replace("-", "") + "-";
            Rapor_Pengaturan m = DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m0 => m0.TahunAjaran == tahun_ajaran && m0.Semester == semester).FirstOrDefault();
            if (m != null)
            {
                if (nama_kelas.Length >= 2)
                {
                    if (nama_kelas.Substring(0, 2) == "I-" && m.KurikulumRaporLevel1 == Libs.JenisKurikulum.SD.KURTILAS)
                    {
                        if (jenis_rapor == "0")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
                        }
                        else if (jenis_rapor == "1")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
                        }
                    }
                    else if (nama_kelas.Substring(0, 2) == "I-" && m.KurikulumRaporLevel1 == Libs.JenisKurikulum.SD.KTSP)
                    {
                        if (jenis_rapor == "0")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD;
                        }
                        else if (jenis_rapor == "1")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD;
                        }
                    }
                }
                if (nama_kelas.Length >= 3)
                {
                    if (nama_kelas.Substring(0, 3) == "II-" && m.KurikulumRaporLevel2 == Libs.JenisKurikulum.SD.KURTILAS)
                    {
                        if (jenis_rapor == "0")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
                        }
                        else if (jenis_rapor == "1")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
                        }
                    }
                    else if (nama_kelas.Substring(0, 3) == "II-" && m.KurikulumRaporLevel2 == Libs.JenisKurikulum.SD.KTSP)
                    {
                        if (jenis_rapor == "0")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD;
                        }
                        else if (jenis_rapor == "1")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD;
                        }
                    }
                }
                if (nama_kelas.Length >= 4)
                {
                    if (nama_kelas.Substring(0, 4) == "III-" && m.KurikulumRaporLevel3 == Libs.JenisKurikulum.SD.KURTILAS)
                    {
                        if (jenis_rapor == "0")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
                        }
                        else if (jenis_rapor == "1")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
                        }
                    }
                    else if (nama_kelas.Substring(0, 4) == "III-" && m.KurikulumRaporLevel3 == Libs.JenisKurikulum.SD.KTSP)
                    {
                        if (jenis_rapor == "0")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD;
                        }
                        else if (jenis_rapor == "1")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD;
                        }
                    }
                }
                if (nama_kelas.Length >= 3)
                {
                    if (nama_kelas.Substring(0, 3) == "IV-" && m.KurikulumRaporLevel4 == Libs.JenisKurikulum.SD.KURTILAS)
                    {
                        if (jenis_rapor == "0")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
                        }
                        else if (jenis_rapor == "1")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
                        }
                    }
                    else if (nama_kelas.Substring(0, 3) == "IV-" && m.KurikulumRaporLevel4 == Libs.JenisKurikulum.SD.KTSP)
                    {
                        if (jenis_rapor == "0")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD;
                        }
                        else if (jenis_rapor == "1")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD;
                        }
                    }
                }
                if (nama_kelas.Length >= 2)
                {
                    if (nama_kelas.Substring(0, 2) == "V-" && m.KurikulumRaporLevel5 == Libs.JenisKurikulum.SD.KURTILAS)
                    {
                        if (jenis_rapor == "0")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
                        }
                        else if (jenis_rapor == "1")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
                        }
                    }
                    else if (nama_kelas.Substring(0, 2) == "V-" && m.KurikulumRaporLevel5 == Libs.JenisKurikulum.SD.KTSP)
                    {
                        if (jenis_rapor == "0")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD;
                        }
                        else if (jenis_rapor == "1")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD;
                        }
                    }
                }
                if (nama_kelas.Length >= 3)
                {
                    if (nama_kelas.Substring(0, 3) == "VI-" && m.KurikulumRaporLevel6 == Libs.JenisKurikulum.SD.KURTILAS)
                    {
                        if (jenis_rapor == "0")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
                        }
                        else if (jenis_rapor == "1")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
                        }
                    }
                    else if (nama_kelas.Substring(0, 3) == "VI-" && m.KurikulumRaporLevel6 == Libs.JenisKurikulum.SD.KTSP)
                    {
                        if (jenis_rapor == "0")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD;
                        }
                        else if (jenis_rapor == "1")
                        {
                            s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD;
                        }
                    }
                }
            }

            return s_jenis_download_key;
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
            txtSemester.Value = semester;
            ltrPeriode.Text = tahun_ajaran + "&nbsp;<sup style=\"color: yellow;\">" + semester + "</sup>";

            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds.SelectParameters.Clear();
            sql_ds.SelectCommand = DAO_Rapor_Nilai.SP_SELECT_PENILAIAN;
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

        public static string GetUnit()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SD).FirstOrDefault().Kode.ToString();
        }

        protected void ListKenaikanKelasSiswa()
        {
            string tahun_ajaran = txtTahunAjaran.Value;
            string semester = txtSemester.Value;
            string rel_kelas_det = txtKelasDet.Value;

            txtInfoSiswa.Text = "Kelas " + DAO_KelasDet.GetByID_Entity(rel_kelas_det).Nama + ", " +
                                "Tahun Pelajaran " + tahun_ajaran;

            ltrListSiswaKenaikanKelas.Text = "";

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
                        string s_panggilan = "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                              "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

                        string s_is_naik = "";
                        KenaikanKelas kenaikan_kelas = DAO_KenaikanKelas.GetByTAByKelasBySiswa_Entity(txtTahunAjaran.Value, txtKelasDet.Value, m_siswa.Kode.ToString());
                        if (kenaikan_kelas != null)
                        {
                            if (kenaikan_kelas.TahunAjaran != null)
                            {
                                s_is_naik = (kenaikan_kelas.IsNaik ? "1" : "0");
                            }
                        }

                        string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                        ltrListSiswaKenaikanKelas.Text +=
                                                "<div class=\"row\">" +
                                                    "<div class=\"col-xs-5\">" +
                                                        "<table style=\"margin: 0px; width: 100%;\">" +
                                                            "<tr>" +
                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                                    id.ToString() +
                                                                    "." +
                                                                "</td>" +
                                                                "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                                    "<input name=\"txt_siswa_kenaikan_kelas[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                                    "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                                        "style=\"margin-top: 10px; height: 32px; width: 32px; border-radius: 100%;\">" +
                                                                "</td>" +
                                                                "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                                    (
                                                                        m_siswa.NISSekolah.Trim() != ""
                                                                        ? "<span style=\"color: #bfbfbf; font-weight: normal; font-size: small;\">" +
                                                                            m_siswa.NISSekolah +
                                                                          "</span>" +
                                                                          "<br />"
                                                                        : ""
                                                                    ) +
                                                                    "<span style=\"color: grey; font-weight: bold;\">" +
                                                                        Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 2) +
                                                                        (
                                                                            m_siswa.Panggilan.Trim() != ""
                                                                            ? "<span style=\"font-weight: normal\">" +
                                                                                "&nbsp;" + s_panggilan +
                                                                              "</span>"
                                                                            : ""
                                                                        ) +
                                                                    "</span>" +
                                                                "</td>" +
                                                            "</tr>" +
                                                        "</table>" +
                                                    "</div>" +
                                                    "<div class=\"col-xs-7\" style=\"vertical-align: middle;\">" +
                                                        "<select onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" name=\"cbo_kenaikan_kelas[]\" title=\" Kenaikan Kelas \" class=\"text-input\" style=\"width: 100%; margin-top: 15px;\">" +
                                                            "<option " + (s_is_naik == "1" ? "selected" : "") + " value=\"1\">Naik Kelas</option>" +
                                                            "<option " + (s_is_naik == "0" ? "selected" : "") + " value=\"0\">Tinggal Kelas</option>" +
                                                        "</select>" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class=\"row\">" +
                                                    "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                        "<hr style=\"margin: 0px; margin-top: 10px; border-color: #E9EFF5;\" />" +
                                                    "</div>" +
                                                "</div>";

                        id++;
                            
                    }

                }
            }
        }

        protected void btnShowListKenaikanKelas_Click(object sender, EventArgs e)
        {
            BindListView(true, Libs.GetQ());
            ListKenaikanKelasSiswa();
            txtKeyAction.Value = JenisAction.DoShowListKenaikanKelas.ToString();
        }

        protected void btnOKKenaikanKelas_Click(object sender, EventArgs e)
        {
            if (txtParseKenaikanKelas.Value.Trim() != "")
            {
                DAO_KenaikanKelas.DeleteByTAByKelas(txtTahunAjaran.Value, txtKelasDet.Value);
                string[] arr_kenaikan_kelas = txtParseKenaikanKelas.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item_kenaikan_kelas in arr_kenaikan_kelas)
                {
                    var arr_item_kenaikan_kelas = item_kenaikan_kelas.Split(new string[] { "|" }, StringSplitOptions.None);                    
                    if (arr_item_kenaikan_kelas.Length == 2)
                    {
                        DAO_KenaikanKelas.Insert(new KenaikanKelas {
                            TahunAjaran = txtTahunAjaran.Value,
                            Rel_KelasDet = txtKelasDet.Value,
                            Rel_Siswa = arr_item_kenaikan_kelas[1],
                            IsNaik = (arr_item_kenaikan_kelas[0] == "1" ? true : false)
                        });
                    }
                }
            }
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void ShowListSiswaShowRapor()
        {
            string tahun_ajaran = txtTahunAjaran.Value;
            string semester = txtSemester.Value;
            string rel_kelas_det = txtKelasDet.Value;

            txtInfoSiswa.Text = "Kelas " + DAO_KelasDet.GetByID_Entity(rel_kelas_det).Nama + ", " +
                                "Tahun Pelajaran " + tahun_ajaran;

            ltrPilihSiswaCetakRapor.Text = "";

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
                                                                "Pilih siswa kelas <span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span> yang akan dilihat/dicetak" +
                                                            "</div>";
                            }
                        }

                        string chk_id = "chk_" + m_siswa.Kode.ToString().Replace("-", "_");
                        ltrPilihSiswaCetakRapor.Text +=
                                                "<div class=\"row\">" +
                                                    "<div class=\"col-xs-1\" style=\"color: #bfbfbf; padding-top: 7px;\">" +
                                                        id.ToString() + "." +
                                                    "</div>" +
                                                    "<div class=\"col-xs-11\">" +
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
                                                    "</div>" +
                                                "</div>";

                        id++;

                    }

                }
            }
        }

        protected void SetJSOKPrintViewRapor(JenisLihatCetak jenis, string tahun_ajaran, string semester, string nama_kelas)
        {
            string s_url = "";
            lnkOKHalamanRapor.Attributes["onclick"] = "";
            if (jenis == JenisLihatCetak.RAPOR)
            {
                s_url = "'" +
                            ResolveUrl(
                                s_url_rapor_ok
                            ) +
                        "?'" +
                        " + (GetCheckedSiswaRapor().trim() != '' ? 'sis=' : '') + GetCheckedSiswaRapor().trim()" +
                        " + '&" + AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=' + '" +
                              GetJenisDownloadNilaiRapor(
                                  tahun_ajaran, semester, nama_kelas, "0"
                                ) + "'" +
                        " + '&t=' + '" + RandomLibs.GetRndTahunAjaran(txtTahunAjaran.Value) + "'" +
                        " + (" + txtSemester.ClientID + ".value.trim() != '' ? '&s=' + " + txtSemester.ClientID + ".value.trim() : '')" +
                        " + (" + txtKelasDet.ClientID + ".value.trim() != '' ? '&kd=' + " + txtKelasDet.ClientID + ".value.trim() : '')";

                lnkOKHalamanRapor.Attributes.Add(
                        "onclick",
                        "if(!ValidateCheckedSiswaRapor()) { ShowSBMessage('Data siswa belum dipilih'); return false; } " +
                        "window.open(" +
                                s_url +
                            ", '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0'); " +
                        "return false; "
                    );
            }
            else if (jenis == JenisLihatCetak.RAPOR_URAIAN)
            {
                s_url = "'" +
                            ResolveUrl(
                                s_url_rapor_ok
                            ) +
                        "?'" +
                        " + (GetCheckedSiswaRapor().trim() != '' ? 'sis=' : '') + GetCheckedSiswaRapor().trim()" +
                        " + '&" + AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=' + '" +
                              GetJenisDownloadNilaiRapor(
                                  tahun_ajaran, semester, nama_kelas, "1"
                                ) + "'" +
                        " + '&t=' + '" + RandomLibs.GetRndTahunAjaran(txtTahunAjaran.Value) + "'" +
                        " + (" + txtSemester.ClientID + ".value.trim() != '' ? '&s=' + " + txtSemester.ClientID + ".value.trim() : '')" +
                        " + (" + txtKelasDet.ClientID + ".value.trim() != '' ? '&kd=' + " + txtKelasDet.ClientID + ".value.trim() : '')";

                lnkOKHalamanRapor.Attributes.Add(
                        "onclick",
                        "if(!ValidateCheckedSiswaRapor()) { ShowSBMessage('Data siswa belum dipilih'); return false; } " +
                        "window.open(" +
                                s_url +
                            ", '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0'); " +
                        "return false; "
                    );
            }
            else if (jenis == JenisLihatCetak.LTS)
            {
                if ((Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) < 20202021))
                //if (!(Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) < 20202021))
                {
                    s_url = "'" +
                            ResolveUrl(
                                s_url_ok
                            ) +
                        "?'" +
                        " + (GetCheckedSiswaRapor().trim() != '' ? 'sis=' : '') + GetCheckedSiswaRapor().trim()" +
                        " + '&t=' + '" + RandomLibs.GetRndTahunAjaran(txtTahunAjaran.Value) + "'" +
                        " + (" + txtSemester.ClientID + ".value.trim() != '' ? '&s=' + " + txtSemester.ClientID + ".value.trim() : '')" +
                        " + (" + txtKelasDet.ClientID + ".value.trim() != '' ? '&kd=' + " + txtKelasDet.ClientID + ".value.trim() : '')";

                    lnkOKHalamanRapor.Attributes.Add(
                            "onclick",
                            "if(!ValidateCheckedSiswaRapor()) { ShowSBMessage('Data siswa belum dipilih'); return false; } " +
                            "window.open(" +
                                    s_url +
                                ", '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0'); " +
                            "return false; "
                        );
                }
                else
                {
                    s_url = "'" +
                            ResolveUrl(
                                s_url_rapor_ok
                            ) +
                        "?'" +
                        " + (GetCheckedSiswaRapor().trim() != '' ? 'sis=' : '') + GetCheckedSiswaRapor().trim()" +
                        " + '&" + AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=' + '" +
                              GetJenisDownloadNilaiRapor(
                                  tahun_ajaran, semester, nama_kelas, "2"
                                ) + "'" +
                        " + '&t=' + '" + RandomLibs.GetRndTahunAjaran(txtTahunAjaran.Value) + "'" +
                        " + (" + txtSemester.ClientID + ".value.trim() != '' ? '&s=' + " + txtSemester.ClientID + ".value.trim() : '')" +
                        " + (" + txtKelasDet.ClientID + ".value.trim() != '' ? '&kd=' + " + txtKelasDet.ClientID + ".value.trim() : '')";

                    lnkOKHalamanRapor.Attributes.Add(
                            "onclick",
                            "if(!ValidateCheckedSiswaRapor()) { ShowSBMessage('Data siswa belum dipilih'); return false; } " +
                            "window.open(" +
                                    s_url +
                                ", '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0'); " +
                            "return false; "
                        );
                }
            }
        }

        protected void btnShowCetakRapor_Click(object sender, EventArgs e)
        {
            BindListView(true, Libs.GetQ());
            ShowListSiswaShowRapor();
            if (txtJenisLihatCetak.Value == JenisLihatCetak.LTS.ToString())
            {
                SetJSOKPrintViewRapor(
                        JenisLihatCetak.LTS, txtTahunAjaran.Value, txtSemester.Value,
                        DAO_Kelas.GetByID_Entity(DAO_KelasDet.GetByID_Entity(txtKelasDet.Value).Rel_Kelas.ToString()).Nama
                    );
                div_halaman_rapor.Visible = false;
            }
            else if (txtJenisLihatCetak.Value == JenisLihatCetak.RAPOR.ToString())
            {
                SetJSOKPrintViewRapor(
                        JenisLihatCetak.RAPOR, txtTahunAjaran.Value, txtSemester.Value,
                        DAO_Kelas.GetByID_Entity(DAO_KelasDet.GetByID_Entity(txtKelasDet.Value).Rel_Kelas.ToString()).Nama
                    );
                div_halaman_rapor.Visible = false;
            }
            else if (txtJenisLihatCetak.Value == JenisLihatCetak.RAPOR_URAIAN.ToString())
            {
                SetJSOKPrintViewRapor(
                        JenisLihatCetak.RAPOR_URAIAN, txtTahunAjaran.Value, txtSemester.Value,
                        DAO_Kelas.GetByID_Entity(DAO_KelasDet.GetByID_Entity(txtKelasDet.Value).Rel_Kelas.ToString()).Nama
                    );
                div_halaman_rapor.Visible = false;
            }
            txtKeyAction.Value = JenisAction.DoShowInputHalaman.ToString();
        }

        protected void ShowEmailLTS()
        {
            string tahun_ajaran = txtTahunAjaran.Value;
            string semester = txtSemester.Value;
            string rel_kelas_det = txtKelasDet.Value;

            string html = "Email Nilai & Deskripsi LTS " +
                          "Tahun Pelajaran <span style=\"font-weight: bold;\">" + tahun_ajaran + "</span>, " +
                          "Kelas <span style=\"font-weight: bold;\">" + DAO_KelasDet.GetByID_Entity(rel_kelas_det).Nama + "</span>";

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
                        string s_panggilan = "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                              "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

                        string chk_id = "chk_" + m_siswa.Kode.ToString().Replace("-", "_");
                        html +=
                                "<div class=\"row\" style=\"padding-left: 15px; padding-right: 15px; " + (id == 1 ? "margin-top: 10px;" : "") + "\">" +
                                    "<div class=\"col-xs-1\" style=\"color: #bfbfbf; padding-top: 7px; padding-left: 5px; width: 30px;\">" +
                                        id.ToString() + "." +
                                    "</div>" +
                                    "<div class=\"col-xs-11\" style=\"padding-top: 7px; padding-left: 5px; padding-bottom: 6px;\">" +
                                        "<div class=\"checkbox checkbox-adv\">" +
                                            "<label for=\"" + chk_id + "\">" +
                                                "<input checked=\"checked\" " +
                                                        "name=\"chk_pilih_siswa_email_rapor_lts[]\" " +
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
                                                        m_siswa.Email.Trim().ToLower().Trim() != ""
                                                        ? "<i class=\"fa fa-envelope-o\" style=\"color: #bfbfbf;\"></i>&nbsp;&nbsp;" + 
                                                          m_siswa.Email.Trim().ToLower()
                                                        : "<i class=\"fa fa-exclamation-triangle\" style=\"color: #bfbfbf;\"></i>&nbsp;&nbsp;" + 
                                                          "<span style=\"color: #bfbfbf; font-style: italic; font-weight: normal;\">tidak ada email</span>"
                                                    ) +
                                                "</span>" +
                                            "</label>" +
                                        "</div>" +
                                        "<div style=\"position: absolute; right: -40px; top: 16px; color: #bfbfbf;\">" +
                                            (
                                                m_siswa.Email.Trim().ToLower().Trim() != ""
                                                ? "<label onclick=\"" + txtIDSiswa.ClientID + ".value = '" + m_siswa.Kode.ToString() + "'; " + btnShowKontenEmailLTS.ClientID + ".click();\" title=\" Lihat Konten Email \" style=\"cursor: pointer; font-size: small; color: #00198d; font-weight: bold;\">" +
                                                        "<i class=\"fa fa-envelope\"></i>&nbsp;&nbsp;Konten Email" +
                                                  "</label>" +
                                                  "&nbsp;&nbsp;|&nbsp;&nbsp;" +
                                                  "<label title=\" Lihat Status Pengiriman Email \" style=\"cursor: pointer; font-size: small; color: #00198d; font-weight: bold;\">" +
                                                        "<i class=\"fa fa-eye\"></i>&nbsp;&nbsp;Status" +
                                                  "</label>"
                                                : ""
                                            ) +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                                "<hr style=\"margin: 0px;\" />";

                        id++;

                    }

                }
            }

            ltrEmailLTS.Text = html;
        }

        protected string GetHTMLEmailLTS(string tahun_ajaran, string semester, string rel_siswa)
        {
            string html = "";

            Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                    tahun_ajaran, semester, rel_siswa
                );
            Rapor_Pengaturan m_pengaturan = DAO_Rapor_Pengaturan.GetByTABySM_Entity(
                    tahun_ajaran, semester
                );
            if (m_siswa != null)
            {
                if (m_siswa.Nama != null)
                {

                    if (m_pengaturan != null)
                    {
                        if (m_pengaturan.TahunAjaran != null)
                        {

                            string s_kelas_det = "";
                            KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet);
                            if (m_kelas != null)
                            {

                                if (m_kelas.Nama != null)
                                {
                                    s_kelas_det = m_kelas.Nama;
                                    html = m_pengaturan.TemplateEmailLTS.Replace("@nama", Libs.GetPerbaikiEjaanNama(m_siswa.Nama));
                                    html = html.Replace("@tahun_ajaran", tahun_ajaran);
                                    html = html.Replace("@semester", semester);
                                    html = html.Replace("@ayah", m_siswa.NamaAyah);
                                    html = html.Replace("@ibu", m_siswa.NamaIbu);
                                    html = html.Replace("@nis", m_siswa.NISSekolah);
                                    html = html.Replace("@kelas", s_kelas_det);
                                    html = html.Replace("@wali_kelas", m_kelas.Nama);
                                    //html = html.Replace("@link", 
                                    //                        Libs.GetHTMLLinkLTS(
                                    //                            this.Page, 
                                    //                            tahun_ajaran,
                                    //                            semester,
                                    //                            m_siswa.Rel_KelasDet,
                                    //                            m_siswa.Kode.ToString(),
                                    //                            false
                                    //                        )
                                    //                    );
                                    //html = html.Replace("@link_tombol",
                                    //                        Libs.GetHTMLLinkLTS(
                                    //                            this.Page,
                                    //                            tahun_ajaran,
                                    //                            semester,
                                    //                            m_siswa.Rel_KelasDet,
                                    //                            m_siswa.Kode.ToString(),
                                    //                            true
                                    //                        )
                                    //                    );

                                }

                            }

                        }
                    }

                }
            }

            return html;
        }

        protected void ShowKontenEmailLTS()
        {
            if (txtIDSiswa.Value.Trim() != "")
            {
                Siswa m_siswa = DAO_Siswa.GetByID_Entity(
                        txtTahunAjaran.Value,
                        txtSemester.Value,    
                        txtIDSiswa.Value
                    );

                if (m_siswa != null)
                {
                    if (m_siswa.Nama != null)
                    {
                        txtEMailLTS.Text = m_siswa.Email.ToString().ToLower();
                        ltrEMailLTSContent.Text = GetHTMLEmailLTS(txtTahunAjaran.Value, txtSemester.Value, txtIDSiswa.Value);
                    }
                }

                txtKeyAction.Value = JenisAction.DoShowKontenEmailLTS.ToString();
            }
        }

        protected void btnShowEmailLTS_Click(object sender, EventArgs e)
        {
            ShowEmailLTS();
            mvMain.ActiveViewIndex = 1;
            txtKeyAction.Value = JenisAction.DoShowShowEmailLTS.ToString();
        }

        protected void lnkBatalEmailLTS_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.DoShowListNilai.ToString();
        }

        protected void btnOKKirimEmailLTS_Click(object sender, EventArgs e)
        {
            BindListView(true, Libs.GetQ());
        }

        protected void btnBindDataList_Click(object sender, EventArgs e)
        {
            BindListView(true, Libs.GetQ().Trim());
        }

        protected void lnkOKKirimEmailLTS_Click(object sender, EventArgs e)
        {

        }

        protected void btnShowKontenEmailLTS_Click(object sender, EventArgs e)
        {
            ShowKontenEmailLTS();
        }
    }
}