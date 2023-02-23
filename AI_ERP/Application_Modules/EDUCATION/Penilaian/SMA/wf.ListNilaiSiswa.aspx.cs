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
    public partial class wf_ListNilaiSiswa : System.Web.UI.Page
    {
        public static string s_url_ok = AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CETAK_LTS.ROUTE;
        public const string SessionViewDataName = "PAGEDATALISTNILAI_SMA";

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
            DoShowInputRaporLTS,
            DoShowInputRaporSemester,
            DoShowListNilai,
            DoShowShowEmailLTS,
            DoShowKontenEmailLTS,
            DoKirimEmail,
            DoRefresh,
            DoShowUbahEmail,
            DoShowListHistoryEmailLTS
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

            if (txtTahunAjaran.Value == "2020/2021")
            {
                lnkOKRaporLTS.Attributes.Add(
                    "onclick",
                    "if(!ValidateCheckedSiswaRapor()) { ShowSBMessage('Data siswa belum dipilih'); return false; } " +
                    "window.open(" +
                        "'" + ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.ROUTE) + "?'" +
                            " + 'j=' + '" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_LTS_SMA + "' " +
                            " + '&t=' + " + txtTahunAjaran.ClientID + ".value.replaceAll('/', '-')" +
                            " + '&kd=' + " + txtKelasDet.ClientID + ".value" +
                            " + '&s=' + " + txtSemester.ClientID + ".value + '&sw=' + GetCheckedSiswaRapor(), '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0'); " +
                    "return false; "
                );

                lnkOKRaporSemester.Attributes.Add(
                        "onclick",
                        "if(!ValidateCheckedSiswaRapor()) { ShowSBMessage('Data siswa belum dipilih'); return false; } " +
                        "window.open(" +
                            "'" + ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.ROUTE) + "?'" +
                                " + 'j=' + '" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_SEMESTER_SMA + "' " +
                                " + '&t=' + " + txtTahunAjaran.ClientID + ".value.replaceAll('/', '-')" +
                                " + '&kd=' + " + txtKelasDet.ClientID + ".value" +
                                " + '&hal=' + " + txtHalaman.ClientID + ".value" +
                                " + '&s=' + " + txtSemester.ClientID + ".value + '&sw=' + GetCheckedSiswaRapor(), '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0'); " +
                        "return false; "
                    );
            }
            else
            {
                lnkOKRaporLTS.Attributes.Add(
                    "onclick",
                    "if(!ValidateCheckedSiswaRapor()) { ShowSBMessage('Data siswa belum dipilih'); return false; } " +
                    "window.open(" +
                        "'" + ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.ROUTE) + "?'" +
                            " + 'j=' + '" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_LTS_SMA + "' " +
                            " + '&t=' + " + txtTahunAjaran.ClientID + ".value.replaceAll('/', '-')" +
                            " + '&kd=' + " + txtKelasDet.ClientID + ".value" +
                            " + '&s=' + " + txtSemester.ClientID + ".value + '&sw=' + GetCheckedSiswaRapor(), '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0'); " +
                    "return false; "
                );

                lnkOKRaporSemester.Attributes.Add(
                        "onclick",
                        "if(!ValidateCheckedSiswaRapor()) { ShowSBMessage('Data siswa belum dipilih'); return false; } " +
                        "window.open(" +
                            "'" + ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.ROUTE) + "?'" +
                                " + 'j=' + '" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_SEMESTER_SMA + "' " +
                                " + '&t=' + " + txtTahunAjaran.ClientID + ".value.replaceAll('/', '-')" +
                                " + '&kd=' + " + txtKelasDet.ClientID + ".value" +
                                " + '&hal=' + " + txtHalaman.ClientID + ".value" +
                                " + '&s=' + " + txtSemester.ClientID + ".value + '&sw=' + GetCheckedSiswaRapor(), '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0'); " +
                        "return false; "
                    );
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

        public static string GetUnit()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMA).FirstOrDefault().Kode.ToString();
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

        protected void ShowListSiswaShowRaporLTS()
        {
            ltrPilihSiswaCetakRapor.Text = "";

            string tahun_ajaran = txtTahunAjaran.Value;
            string semester = txtSemester.Value;
            string rel_kelas_det = txtKelasDet.Value;

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

        protected void ShowListSiswaShowRaporSemester()
        {
            ltrPilihSiswaCetakRaporSemester.Text = "";

            string tahun_ajaran = txtTahunAjaran.Value;
            string semester = txtSemester.Value;
            string rel_kelas_det = txtKelasDet.Value;

            ltrPilihSiswaCetakRaporSemester.Text = "";

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

                        ltrPilihSiswaRaporSemesterCaption.Text = "Pilih Siswa";

                        KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet);
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                ltrPilihSiswaRaporSemesterCaption.Text = "<div style=\"width: 100%; background-color: #f1f9f7; border-color: #e0f1e9; color: #1d9d74; padding: 15px; border-width: 1px; border-style: solid; border-radius: 5px; font-weight: normal;\">" +
                                                                    "<i class=\"fa fa-info-circle\"></i>" +
                                                                    "&nbsp;&nbsp;" +
                                                                    "Pilih siswa kelas <span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span> yang akan dilihat/dicetak" +
                                                                "</div>";
                            }
                        }

                        string chk_id = "chk_" + m_siswa.Kode.ToString().Replace("-", "_");
                        ltrPilihSiswaCetakRaporSemester.Text +=
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
            ShowListSiswaShowRaporLTS();
            txtKeyAction.Value = JenisAction.DoShowInputRaporLTS.ToString();
        }

        protected void btnShowCetakRaporSemester_Click(object sender, EventArgs e)
        {
            ShowListSiswaShowRaporSemester();
            txtKeyAction.Value = JenisAction.DoShowInputRaporSemester.ToString();
        }

        protected void ShowEmailLTS()
        {
            ltrEmailSiswa.Text = "";

            string tahun_ajaran = txtTahunAjaran.Value;
            string semester = txtSemester.Value;
            string rel_kelas_det = txtKelasDet.Value;

            string html = "Email Nilai & Deskripsi LTS " +
                          "Tahun Pelajaran <span style=\"font-weight: bold;\">" + tahun_ajaran + "</span>, " +
                          "Kelas <span style=\"font-weight: bold;\">" + DAO_KelasDet.GetByID_Entity(rel_kelas_det).Nama + "</span>";

            string html_email_siswa 
                        = "Email Nilai & Deskripsi LTS " +
                          "Tahun Pelajaran <span style=\"font-weight: bold;\">" + tahun_ajaran + "</span>, " +
                          "Kelas <span style=\"font-weight: bold;\">" + DAO_KelasDet.GetByID_Entity(rel_kelas_det).Nama + "</span>" +
                          "<br />" +
                          "<table style=\"border-collapse: collapse; border-style: solid; border-width: 1px;\">" +
                            "<tr>" +
                                "<td style=\"padding: 5px; width: 30px; text-align: center; font-weight: bold; border-style: solid; border-width: 1px; border-color: black;\">" +
                                    "No." +
                                "</td>" +
                                "<td style=\"padding: 5px; text-align: center; font-weight: bold; border-style: solid; border-width: 1px; border-color: black;\">" +
                                    "Nama Siswa" +
                                "</td>" +
                                "<td style=\"padding: 5px; text-align: center; font-weight: bold; border-style: solid; border-width: 1px; border-color: black;\">" +
                                    "Email" +
                                "</td>" +
                            "</tr>";

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
                        html_email_siswa
                                +=  "<tr>" +
                                        "<td style=\"padding: 5px; width: 30px; text-align: center; font-weight: normal; border-style: solid; border-width: 1px; border-color: black;\">" +
                                            id.ToString() + "." +
                                        "</td>" +
                                        "<td style=\"padding: 5px; text-align: left; font-weight: bold; border-style: solid; border-width: 1px; border-color: black;\">" +
                                            m_siswa.Nama.ToUpper() +
                                        "</td>" +
                                        "<td style=\"padding: 5px; text-align: left; font-weight: normal; border-style: solid; border-width: 1px; border-color: black;\">" +
                                            m_siswa.Email.ToLower().Trim() +
                                        "</td>" +
                                    "</tr>";

                        string s_panggilan = "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                              "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";
                        List<MailJobs> lst_mail_jobs = DAO_MailJobs.GetPengumumanLTSBySiswa_Entity(m_siswa.Kode.ToString()).OrderByDescending(m0 => m0.Tanggal).ToList();

                        string chk_id = "chk_" + m_siswa.Kode.ToString().Replace("-", "_");
                        string s_event_hist_lts = " onclick=\"" + 
                                                                txtIDSiswa.ClientID + ".value = '" + m_siswa.Kode.ToString() + "'; " +                                                                
                                                                btnShowKontenHistEmailLTS.ClientID + ".click(); " +
                                                           "\" ";

                        List<Rapor_ViewEmailLTS> lst_view_email =
                            DAO_Rapor_ViewEmailLTS.GetBySiswa_Entity(m_siswa.Kode.ToString()).OrderByDescending(m0 => m0.Tanggal).ToList();
                        string html_dilihat = "";
                        if (lst_view_email.Count > 0)
                        {
                            html_dilihat = "<label title=\" Dilihat Tanggal \" style=\"font-size: small; color: gray; font-weight: bold; min-width: 250px; width: 250px; max-width: 250px; text-align: right;\">" +
                                                "<i class=\"fa fa-eye\"></i>&nbsp;&nbsp;" +
                                                Libs.GetTanggalIndonesiaFromDate(lst_view_email.FirstOrDefault().Tanggal, true) +
                                           "</label>" +
                                           "&nbsp;&nbsp;|&nbsp;&nbsp;";
                        }
                        else
                        {
                            html_dilihat = "<label style=\"font-size: small; color: gray; font-weight: bold; min-width: 250px; width: 250px; max-width: 250px; text-align: right;\">" +
                                                "&nbsp;&nbsp;" +
                                           "</label>" +
                                           "<label style=\"visibility: hidden;\">" +
                                                "&nbsp;&nbsp;|&nbsp;&nbsp;" +
                                           "</label>";
                        }

                        html +=
                                "<div class=\"row\" style=\"" + (txtIDSiswa.Value.ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim() ? " background-color: #e2f5f8; " : "") + " padding-left: 15px; padding-right: 15px; " + (id == 1 ? "margin-top: 10px;" : "") + "\">" +
                                    "<div class=\"col-xs-1\" style=\"color: #bfbfbf; padding-top: 7px; padding-left: 5px; width: 30px;\">" +
                                        id.ToString() + "." +
                                    "</div>" +
                                    "<div class=\"col-xs-11\" style=\"padding-top: 7px; padding-left: 5px; padding-bottom: 6px;\">" +
                                        "<div class=\"checkbox checkbox-adv\">" +
                                            "<label for=\"" + chk_id + "\">" +
                                                "<input " + 
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
                                            "<label onclick=\"" + txtIDSiswa.ClientID + ".value = '" + m_siswa.Kode.ToString() + "'; " + btnShowUbahEmail.ClientID + ".click();\" title=\" Ubah Email \" style=\"margin-left: 0px; cursor: pointer; padding-left: 7px;\">" +
                                                "<i class=\"fa fa-pencil\" style=\"color: lightcoral;\"></i>" +
                                            "</label>" +
                                            (
                                                m_siswa.Email.Trim().ToLower().Trim() != ""
                                                ? "&nbsp;&nbsp;&nbsp;&nbsp;" +
                                                  html_dilihat +
                                                  "<label onclick=\"" + txtIDSiswa.ClientID + ".value = '" + m_siswa.Kode.ToString() + "'; " + btnShowKontenEmailLTS.ClientID + ".click();\" title=\" Lihat Konten Email \" style=\"cursor: pointer; font-size: small; color: #00198d; font-weight: bold;\">" +
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
                                                        ? "<label " + s_event_hist_lts + " title=\" Lihat Status Pengiriman Email \" style=\"cursor: pointer; font-size: small; color: #00198d; font-weight: bold;\">" +
                                                                "<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;Status" +
                                                          "</label>"
                                                        : (
                                                            lst_mail_jobs.FirstOrDefault().Status == DAO_MailJobs.MSG_STATUS_MENGIRIM
                                                            ? "<label " + s_event_hist_lts + " title=\" Lihat Status Pengiriman Email \" style=\"cursor: pointer; font-size: small; color: darkorange; font-weight: bold;\">" +
                                                                    "<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;Status" +
                                                              "</label>"
                                                            : "<label " + s_event_hist_lts + " title=\" Lihat Status Pengiriman Email \" style=\"cursor: pointer; font-size: small; color: red; font-weight: bold;\">" +
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
                                (
                                    id < lst_siswa.Count
                                    ? "<hr style=\"margin: 0px; margin-left: -15px; margin-right: -15px;\" />"
                                    : ""
                                );

                        id++;

                    }

                }
            }

            html_email_siswa += "</table>";
            ltrEmailSiswa.Text = html_email_siswa;
            ltrEmailLTS.Text = html;
        }

        protected string GetHTMLEmailLTS(string tahun_ajaran, string semester, string rel_siswa, string kode_email, bool is_admin)
        {
            string html = "";

            Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                    tahun_ajaran, semester, rel_siswa
                );
            Rapor_Pengaturan m_pengaturan = DAO_Rapor_Pengaturan.GetByTABySM_Entity(
                    tahun_ajaran, semester
                );
            PengaturanSMA m_pengaturan_ = DAO_PengaturanSMA.GetAll_Entity().FirstOrDefault();
            string teks_link_buka_lts = "Buka LTS";

            if (m_siswa != null)
            {
                if (m_siswa.Nama != null)
                {

                    if (m_pengaturan != null)
                    {
                        if (m_pengaturan.TahunAjaran != null)
                        {
                            if (m_pengaturan_ != null)
                            {
                                if (m_pengaturan_.TeksLinkLTS != null)
                                {
                                    teks_link_buka_lts = m_pengaturan_.TeksLinkLTS;
                                }
                            }

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
                                    html = html.Replace("@link_teks",
                                                            Libs.GetHTMLLinkLTS(
                                                                this.Page,
                                                                tahun_ajaran,
                                                                semester,
                                                                m_siswa.Rel_KelasDet,
                                                                m_siswa.Kode.ToString(),
                                                                false,
                                                                kode_email,
                                                                teks_link_buka_lts,
                                                                (is_admin ? "&act=adm" : "")
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
                                                                kode_email,
                                                                teks_link_buka_lts,
                                                                (is_admin ? "&act=adm" : "")
                                                            )
                                                        );

                                }

                            }

                        }
                    }

                }
            }

            return Libs.GetHTMLEmailTemplate(html, GetUnit(), true);
        }

        protected void ShowKontenEmailLTS()
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
                        txtEMailLTS.Text = m_siswa.Email.ToString().ToLower();
                        ltrEMailLTSContent.Text = GetHTMLEmailLTS(txtTahunAjaran.Value, txtSemester.Value, txtIDSiswa.Value, txtIDEmail.Value, true);
                    }
                }

                string email_penerima = txtEMailLTS.Text;
                PengaturanSMA m_pengaturan = DAO_PengaturanSMA.GetAll_Entity().FirstOrDefault();
                if (m_pengaturan != null)
                {
                    if (m_pengaturan.HeaderKop != null)
                    {
                        if (m_pengaturan.IsTestEmail)
                        {
                            email_penerima = m_pengaturan.TestEmail;
                            txtEMailLTS.Text = email_penerima;
                        }
                    }
                }

                ShowEmailLTS();
                txtKeyAction.Value = JenisAction.DoShowKontenEmailLTS.ToString();
            }
        }

        protected void btnShowEmailLTS_Click(object sender, EventArgs e)
        {
            ShowEmailLTS();
            mvMain.ActiveViewIndex = 1;
            txtKeyAction.Value = JenisAction.DoShowShowEmailLTS.ToString();
        }

        protected void btnBindDataList_Click(object sender, EventArgs e)
        {
            BindListView(true, Libs.GetQ().Trim());
        }

        protected void btnShowKontenEmailLTS_Click(object sender, EventArgs e)
        {
            ShowKontenEmailLTS();
        }

        protected void btnOKKirimEmailLTS_Click(object sender, EventArgs e)
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
                            DoEmailLTS(item_siswa, Guid.NewGuid(), m_siswa.Email.ToLower().Trim());
                        }
                    }
                }
            }
            BindListView(true, Libs.GetQ());
        }

        protected void lnkBatalEmailLTS_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.DoShowListNilai.ToString();
        }

        protected void DoEmailLTS(string rel_siswa, Guid kode_email, string email_penerima)
        {
            //untuk test
            int exp_hari = 0;
            int exp_jam = 0;
            int exp_menit = 0;
            PengaturanSMA m_pengaturan = DAO_PengaturanSMA.GetAll_Entity().FirstOrDefault();
            if (m_pengaturan != null)
            {
                if (m_pengaturan.HeaderKop != null)
                {
                    if (m_pengaturan.IsTestEmail)
                    {
                        email_penerima = m_pengaturan.TestEmail;                        
                    }
                    exp_hari = m_pengaturan.ExpiredLinkLTSHari;
                    exp_jam = m_pengaturan.ExpiredLinkLTSJam;
                    exp_menit = m_pengaturan.ExpiredLinkLTSMenit;
                }
            }
            //end untuk test

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
            
            int port = Email.PORT_DEFAULT;

            if (lst_mail_jobs.Count > 0)
            {
                var m_mail_jobs = lst_mail_jobs.OrderByDescending(m0 => m0.Tanggal).FirstOrDefault();
                txtEMailLTS.Text = email_penerima;
                html_msg = m_mail_jobs.Message;
            }
            else
            {
                string msg = GetHTMLEmailLTS(txtTahunAjaran.Value, txtSemester.Value, rel_siswa, kode_email.ToString(), false);
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
                    Keterangan = "Pengumuman LTS",
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
                    LinkExpiredDate = (
                            exp_hari == 0 && exp_jam == 0 && exp_menit == 0
                            ? DateTime.MinValue
                            : DateTime.Now.AddDays(exp_hari).AddMinutes(exp_jam).AddMinutes(exp_menit)
                        )                    
                });
            }
        }

        protected void lnkOKKirimEmailLTS_Click(object sender, EventArgs e)
        {
            try
            {
                DoEmailLTS(txtIDSiswa.Value, new Guid(txtIDEmail.Value), txtEMailLTS.Text);
                ShowEmailLTS();
                txtKeyAction.Value = JenisAction.DoKirimEmail.ToString();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = "Email tidak valid : " + ex.Message;
            }            
        }

        protected void lnkOKRefreshListRaporLTS_Click(object sender, EventArgs e)
        {
            ShowEmailLTS();
            txtKeyAction.Value = JenisAction.DoRefresh.ToString();
        }

        protected void ShowHistoryEmailLTS()
        {
            List<MailJobs> lst_mail_jobs = DAO_MailJobs.GetPengumumanLTSBySiswa_Entity(txtIDSiswa.Value).OrderByDescending(m0 => m0.Tanggal).ToList(); 
            string html = "<table style=\"margin: 0px; width: 100%;\">";
            int id = 1;
            foreach (var item in lst_mail_jobs)
            {
                List<Rapor_ViewEmailLTS> lst_view_email = 
                    DAO_Rapor_ViewEmailLTS.GetByEmail_Entity(item.Kode.ToString()).OrderByDescending(m0 => m0.Tanggal).ToList();
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
            ltrHistoryEmailLTS.Text = html;
        }

        protected void btnShowKontenHistEmailLTS_Click(object sender, EventArgs e)
        {
            ShowHistoryEmailLTS();
            ShowEmailLTS();
            txtKeyAction.Value = JenisAction.DoShowListHistoryEmailLTS.ToString();
        }

        protected void btnBindDataListEmailLTS_Click(object sender, EventArgs e)
        {
            ShowEmailLTS();
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
                            ShowEmailLTS();
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
                        ShowEmailLTS();
                    }
                }
            }
            txtKeyAction.Value = JenisAction.DoShowUbahEmail.ToString();
        }
    }
}