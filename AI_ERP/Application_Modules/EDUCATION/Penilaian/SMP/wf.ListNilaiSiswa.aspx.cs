using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SMP;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning.SMP;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP
{
    public partial class wf_ListNilaiSiswa : System.Web.UI.Page
    {
        public static string s_url_ok = AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CETAK_LTS.ROUTE;
        public const string SessionViewDataName = "PAGEDATALISTNILAI_SMP";

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
            DoShowDownloadNilaiDapodik,
            DoShowListKenaikanKelas,
            DoShowInputHalaman
        }

        public enum JenisLihatCetak
        {
            LTS = 0,
            RAPOR = 1
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
                    cboPeriode.Items.Add(new ListItem
                    {
                        Value = item.TahunAjaran + item.Semester,
                        Text = item.TahunAjaran + " semester " + item.Semester
                    });
                }
                lnkOKHalamanRapor.Attributes.Add("onclick", "");
            }
            BindListView(!IsPostBack, Libs.GetQ());
            if (!IsPostBack) this.Master.txtCariData.Text = Libs.GetQ();
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            cboPeriode.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        protected void SetJSOKPrintViewRapor(JenisLihatCetak jenis)
        {
            string s_url = "";
            lnkOKHalamanRapor.Attributes["onclick"] = "";

            //get jenis download kurikulum
            string s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
            Rapor_Pengaturan m = DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m0 => m0.TahunAjaran == txtTahunAjaran.Value && m0.Semester == txtSemester.Value).FirstOrDefault();
            if (m != null)
            {
                if (txtKelas.Value.Length >= 4)
                {
                    if (txtKelas.Value.Substring(0, 4) == "VII-" && m.KurikulumRaporLevel7 == Libs.JenisKurikulum.SMP.KURTILAS)
                    {
                        s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                    }
                    else if (txtKelas.Value.Substring(0, 4) == "VII-" && m.KurikulumRaporLevel7 == Libs.JenisKurikulum.SMP.KTSP)
                    {
                        s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP;
                    }
                }
                if (txtKelas.Value.Length >= 5)
                {
                    if (txtKelas.Value.Substring(0, 5) == "VIII-" && m.KurikulumRaporLevel8 == Libs.JenisKurikulum.SMP.KURTILAS)
                    {
                        s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                    }
                    else if (txtKelas.Value.Substring(0, 5) == "VIII-" && m.KurikulumRaporLevel8 == Libs.JenisKurikulum.SMP.KTSP)
                    {
                        s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP;
                    }
                }
                if (txtKelas.Value.Length >= 3)
                {
                    if (txtKelas.Value.Substring(0, 3) == "IX-" && m.KurikulumRaporLevel9 == Libs.JenisKurikulum.SMP.KURTILAS)
                    {
                        s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                    }
                    else if (txtKelas.Value.Substring(0, 3) == "IX-" && m.KurikulumRaporLevel9 == Libs.JenisKurikulum.SMP.KTSP)
                    {
                        s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP;
                    }
                }
            }

            if (jenis == JenisLihatCetak.RAPOR)
            {
                lnkOKHalamanRapor.Attributes.Add(
                        "onclick",
                        "if(!ValidateCheckedSiswaRapor()) { ShowSBMessage('Data siswa belum dipilih'); return false; } " +
                        "window.open(" +
                            "'" + ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_PRINT.ROUTE) + "?'" +
                            " + '" + AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=" + s_jenis_download_key + "'" +
                            " + '&t=' + " + txtTahunAjaran.ClientID + ".value.replaceAll('/', '-')" +
                            " + '&kd=' + " + txtKelasDet.ClientID + ".value" +
                            " + (" + txtHalaman.ClientID + ".value.trim() != '' ? '&hal=' + " + txtHalaman.ClientID + ".value.trim() : '') + '&s=' + " + txtSemester.ClientID + ".value + '&sw=' + GetCheckedSiswaRapor(), '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0'); " +
                        "return false; "
                    );
            }
            else if (jenis == JenisLihatCetak.LTS)
            {
                if ((Libs.GetStringToDouble(txtTahunAjaran.Value.Replace("/", "")) < 20202021))
                //if (!(Libs.GetStringToDouble(txtTahunAjaran.Value.Replace("/", "")) < 20202021))
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
                                ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_PRINT.ROUTE)
                            ) +
                        "?'" +
                        " + (GetCheckedSiswaRapor().trim() != '' ? 'sw=' : '') + GetCheckedSiswaRapor().trim()" +
                        " + '&" + AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=' + '" +
                              AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_LTS_SMP + "'" +
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

        protected void ShowDownloadRaporDapodik()
        {
            Rapor_Desain rapor_desain = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                    txtTahunAjaran.Value, txtSemester.Value, txtRelKelasDet.Value, DAO_Rapor_Desain.JenisRapor.Semester
                                ).FirstOrDefault();

            cboMapelDapodik.Items.Clear();
            cboMapelDapodik.Items.Add("");
            List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(
                rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "");
            foreach (Rapor_Desain_Det item_desain_det in lst_rapor_desain_det)
            {
                Mapel m = DAO_Mapel.GetByID_Entity(item_desain_det.Rel_Mapel);
                if (m != null)
                {
                    if (m.Nama != null)
                    {
                        cboMapelDapodik.Items.Add(new ListItem
                        {
                            Value = m.Kode.ToString(),
                            Text = m.Nama,
                            Selected = (m.Nama.ToLower().Trim().IndexOf("bahasa indonesia") >= 0 ? true : false)
                        });
                    }
                }
            }

            txtTahunAjaranDapodik.Text = txtTahunAjaran.Value;
            txtSemesterDapodik.Text = txtSemester.Value;
            txtKelasDapodik.Text = DAO_KelasDet.GetByID_Entity(txtRelKelasDet.Value).Nama;
        }

        protected void btnShowDownloadRaporDapodik_Click(object sender, EventArgs e)
        {
            ShowDownloadRaporDapodik();
            txtKeyAction.Value = JenisAction.DoShowDownloadNilaiDapodik.ToString();
        }

        public static string GetUnit()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMP).FirstOrDefault().Kode.ToString();
        }

        protected void ListAbsenSiswa()
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
            ListAbsenSiswa();
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
                        DAO_KenaikanKelas.Insert(new KenaikanKelas
                        {
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

        protected void btnShowCetakRapor_Click(object sender, EventArgs e)
        {
            ShowListSiswaShowRapor();
            if (txtJenisLihatCetak.Value == JenisLihatCetak.LTS.ToString())
            {
                SetJSOKPrintViewRapor(JenisLihatCetak.LTS);
                div_halaman_rapor.Visible = false;
            }
            else
            {
                SetJSOKPrintViewRapor(JenisLihatCetak.RAPOR);
                div_halaman_rapor.Visible = true;
            }
            txtKeyAction.Value = JenisAction.DoShowInputHalaman.ToString();
        }

        protected void btnBindDataList_Click(object sender, EventArgs e)
        {
            BindListView(true, Libs.GetQ().Trim());
        }
    }
}