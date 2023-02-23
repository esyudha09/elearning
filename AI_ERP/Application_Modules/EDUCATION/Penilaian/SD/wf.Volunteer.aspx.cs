using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
{
    public partial class wf_Volunteer : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAVOLUNTEER_SD";

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
            DoShowConfirmHapus
        }

        private static class QS
        {
            public static string GetUnit()
            {
                KelasDet m_kelasdet = DAO_KelasDet.GetByID_Entity(Libs.GetQueryString("kd"));
                if (m_kelasdet != null)
                {
                    if (m_kelasdet.Nama != null)
                    {
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelasdet.Rel_Kelas.ToString());
                        return m_kelas.Rel_Sekolah.ToString();
                    }
                }

                return "";
            }

            public static string GetKelas()
            {
                return Libs.GetQueryString("kd");
            }

            public static string GetTahunAjaran()
            {
                string t = Libs.GetQueryString("t");
                return RandomLibs.GetParseTahunAjaran(t);
            }

            public static string GetTahunAjaranPure()
            {
                string t = Libs.GetQueryString("t");
                return t;
            }

            public static string GetSemester()
            {
                string s = Libs.GetQueryString("s");
                return s;
            }

            public static string GetMapel()
            {
                string m = Libs.GetQueryString("m");
                return m;
            }
        }

        protected List<Siswa> GetListSiswa()
        {
            return DAO_Siswa.GetByRombel_Entity(
                        QS.GetUnit(),
                        QS.GetKelas(),
                        QS.GetTahunAjaran(),
                        QS.GetSemester()
                    );
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<i class=\"fa fa-handshake-o\" style=\"font-size: 16pt\"></i>" +
                                       "&nbsp;&nbsp;" +
                                       "Volunteer & Kerja Sosial";

            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;
            this.Master.ShowSubHeaderGuru = true;
            this.Master.ShowHeaderSubTitle = false;
            this.Master.SelectMenuGuru_Penilaian();

            InitURLOnMenu();

            if (!IsPostBack)
            {
                ShowListSiswa();
                InitKeyEventClient();

                txtTahunAjaran.Value = QS.GetTahunAjaran();
                txtSemester.Value = QS.GetSemester();
                txtKelasDet.Value = QS.GetKelas();
            }

            ShowDataSiswa();
            BindListView(!IsPostBack);            
        }

        protected void InitURLOnMenu()
        {
            this.Master.SetURLGuru_TimeLine(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE +
                            "?t=" + Libs.GetQueryString("t") +
                            "&s=" + Libs.GetQueryString("s") +
                            "&k=" + Libs.GetQueryString("k") +
                            "&kd=" + Libs.GetQueryString("kd") +
                            "&m=" + Libs.GetQueryString("m")
                        )
                );
            this.Master.SetURLGuru_Siswa(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWA.ROUTE +
                            "?t=" + Libs.GetQueryString("t") +
                            "&s=" + Libs.GetQueryString("s") +
                            "&k=" + Libs.GetQueryString("k") +
                            "&kd=" + Libs.GetQueryString("kd") +
                            "&m=" + Libs.GetQueryString("m")
                        )
                );
            this.Master.SetURLGuru_Penilaian(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA.ROUTE +
                            "?t=" + Libs.GetQueryString("t") +
                            "&s=" + Libs.GetQueryString("s") +
                            "&k=" + Libs.GetQueryString("k") +
                            "&kd=" + Libs.GetQueryString("kd") +
                            "&m=" + Libs.GetQueryString("m")
                        )
                );
        }

        public static string GetNamaKelas()
        {
            KelasDet m = DAO_KelasDet.GetByID_Entity(QS.GetKelas());
            if (m != null)
            {
                if (m.Nama != null)
                {
                    return m.Nama;
                }
            }

            return "";
        }

        protected void ShowListSiswa()
        {
            ltrListSiswa.Text = "";
            int id = 1;
            foreach (Siswa m_siswa in GetListSiswa())
            {
                string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                ltrListSiswa.Text += "<div class=\"row\">" +
                                        "<div class=\"col-xs-12\" style=\"width: 100%;\">" +
                                            "<table style=\"margin: 0px; width: 100%;\">" +
                                                "<tr>" +
                                                    "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                        id.ToString() +
                                                        "." +
                                                    "</td>" +
                                                    "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                        "<input name=\"txt_siswa[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                        "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                            "style=\"height: 32px; width: 32px; border-radius: 100%;\">" +
                                                    "</td>" +
                                                    "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                        "<span style=\"color: grey; font-weight: bold;\">" +
                                                            Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 3) +
                                                            (
                                                                m_siswa.Panggilan.Trim() != ""
                                                                ? "<br />" +
                                                                  "<span style=\"color: #bfbfbf; font-weight: normal\">" +
                                                                    Libs.GetNamaPanggilan(m_siswa.Panggilan) +
                                                                  "</span>"
                                                                : ""
                                                            ) +
                                                        "</span>" +
                                                    "</td>" +
                                                    "<td style=\"width: 50px; text-align: right; vertical-align: middle; padding-right: 0px;\">" +
                                                        "<label id=\"img_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                                                               "style=\"display: none; font-size: small; color: grey; font-weight: bold;\">" +

                                                            "<img src=\"" + ResolveUrl("~/Application_CLibs/images/giphy.gif") + "\" " +
                                                                    "style=\"height: 16px; width: 20px;\" />" +

                                                        "</label>" +
                                                        "<a id=\"lbl_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                                                            "onclick=\"ShowProsesPilihSiswa('" + m_siswa.Kode.ToString() + "', true); " + txtIndexSiswa.ClientID + ".value = '" + (id - 1) + "'; " + btnShowNilaiSiswa.ClientID + ".click(); \"" +
                                                            "style=\"font-weight: bold; text-transform: none; padding-bottom: 2px; padding-top: 2px; background-color: #1DA1F2; color: white; border-radius: 15px; font-size: x-small;\" " +
                                                            "class=\"btn btn-flat waves-attach waves-effect\" " +
                                                            "title=\" Buka \">" +
                                                                "<i class=\"fa fa-folder-open\"></i>&nbsp;&nbsp;Buka" +
                                                        "</a>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"row\">" +
                                        "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                            "<hr style=\"margin: 0px; margin-top: 5px; margin-bottom: 5px; border-color: #E9EFF5;\" />" +
                                        "</div>" +
                                    "</div>";

                id++;
            }

            txtCountSiswa.Value = GetListSiswa().Count.ToString();
            txtIndexSiswa.Value = "0";
        }

        protected void ShowDataSiswa()
        {
            if (Libs.GetStringToInteger(txtIndexSiswa.Value) >= 0 &&
                Libs.GetStringToInteger(txtCountSiswa.Value) > 0)
            {
                txtIDSiswa.Value = "";
                Siswa m_siswa = GetListSiswa()[Libs.GetStringToInteger(txtIndexSiswa.Value)];
                if (m_siswa != null)
                {
                    if (m_siswa.Nama != null)
                    {

                        txtIDSiswa.Value = m_siswa.Kode.ToString();
                        lblNamaSiswaInfo.Text = Libs.GetPersingkatNama(m_siswa.Nama).ToUpper();
                        lblKelasSiswaInfo.Text = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet).Nama;
                        lblInfoPeriode.Text = "<span style=\"font-weight: bold;\">" +
                                                QS.GetTahunAjaran() +
                                              "</span>" +
                                              (
                                                txtSemester.Value != ""
                                                ? "&nbsp;Semester&nbsp;" +
                                                  "<span style=\"font-weight: bold;\">" +
                                                    txtSemester.Value +
                                                  "</span>"
                                                : ""
                                              );

                    }
                }
            }
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            txtJumlahJam.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKeterangan.ClientID + "').focus(); return false; }");
            txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        private void BindListView(bool isbind = true)
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds.SelectParameters.Clear();
            sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
            sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
            sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
            sql_ds.SelectParameters.Add("Rel_Siswa", txtIDSiswa.Value);
            sql_ds.SelectCommand = DAO_Volunteer_Det.SP_SELECT_BY_TA_BY_SM_BY_KELAS_BY_SISWA;

            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_kegiatan = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kegiatan");
            System.Web.UI.WebControls.Literal imgh_tanggal = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tanggal");
            System.Web.UI.WebControls.Literal imgh_jumlahjam = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_jumlahjam");
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

            imgh_kegiatan.Text = html_image;
            imgh_tanggal.Text = html_image;
            imgh_jumlahjam.Text = html_image;
            imgh_keterangan.Text = html_image;

            imgh_kegiatan.Visible = false;
            imgh_tanggal.Visible = false;
            imgh_jumlahjam.Visible = false;
            imgh_keterangan.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "Kegiatan":
                    imgh_kegiatan.Visible = true;
                    break;
                case "Tanggal":
                    imgh_tanggal.Visible = true;
                    break;
                case "JumlahJam":
                    imgh_jumlahjam.Visible = true;
                    break;
                case "Keterangan":
                    imgh_keterangan.Visible = true;
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
            BindListView(true);
        }

        protected void InitFields()
        {
            txtID.Value = "";
            txtKegiatan.Text = "";
            txtKegiatanVal.Value = "";
            txtTanggal.Text = "";
            txtJumlahJam.Text = "";
            txtKeterangan.Text = "";
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            BindListView(true);
            InitFields();
            txtKeyAction.Value = JenisAction.Add.ToString();
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_Volunteer_Det.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack);
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
                List<Volunteer> lst_volunteer = DAO_Volunteer.GetByTABySMByKelas_Entity(
                        QS.GetTahunAjaran(),
                        QS.GetSemester(),
                        QS.GetKelas()
                    );

                Guid kode = Guid.NewGuid();
                if (lst_volunteer.Count == 0)
                {
                    DAO_Volunteer.Insert(
                            new Volunteer {
                                Kode = kode,
                                TahunAjaran = QS.GetTahunAjaran(),
                                Semester = QS.GetSemester(),
                                Rel_KelasDet = QS.GetKelas()
                            },
                            Libs.LOGGED_USER_M.UserID
                        );
                }
                else
                {
                    kode = lst_volunteer.FirstOrDefault().Kode;
                }

                Volunteer_Det m = new Volunteer_Det();
                m.Rel_Volunteer = kode;
                m.Kegiatan = txtKegiatanVal.Value;
                m.Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggal.Text);
                m.JumlahJam = txtJumlahJam.Text;
                m.Keterangan = txtKeterangan.Text;
                m.Rel_Siswa = txtIDSiswa.Value;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_Volunteer_Det.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack);
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    m.Kode = Guid.NewGuid();
                    DAO_Volunteer_Det.Insert(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack);
                    InitFields();
                    txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                }

                BindListView(true);
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
                Volunteer_Det m = DAO_Volunteer_Det.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Kegiatan != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        txtKegiatan.Text = m.Kegiatan;
                        txtKegiatanVal.Value = m.Kegiatan;
                        txtTanggal.Text = Libs.GetTanggalIndonesiaFromDate(m.Tanggal, false);
                        txtJumlahJam.Text = m.JumlahJam;
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
                Volunteer_Det m_det = DAO_Volunteer_Det.GetByID_Entity(txtID.Value.Trim());
                if (m_det != null)
                {
                    if (m_det.Kegiatan != null)
                    {
                        Volunteer m = DAO_Volunteer.GetByID_Entity(m_det.Rel_Volunteer.ToString());
                        if (m != null)
                        {
                            if (m.TahunAjaran != null)
                            {
                                ltrMsgConfirmHapus.Text = "Hapus Data Volunteer<br />Tahun Pelajaran " +
                                                              "<span style=\"font-weight: bold;\">\"" +
                                                                    Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                              "\"</span>" +
                                                              " semester " +
                                                              "<span style=\"font-weight: bold;\">\"" +
                                                                    Libs.GetHTMLSimpleText(m.Semester) +
                                                              "\"</span>?";
                                txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                            }
                        }
                    }
                }                
            }
        }

        protected void btnShowNilaiSiswa_Click(object sender, EventArgs e)
        {
            ShowDataSiswa();
            BindListView(true);
        }

        protected void lnkNilaiAkademik_Click(object sender, EventArgs e)
        {
            string url_penilaian = Libs.GetURLPenilaian(Libs.GetQueryString("kd"));

            Response.Redirect(
                    ResolveUrl(
                            url_penilaian +
                            "?t=" + Libs.GetQueryString("t") +
                            "&kd=" + Libs.GetQueryString("kd") +
                            (
                                Libs.GetQueryString("m").Trim() != ""
                                ? "&m=" + Libs.GetQueryString("m")
                                : ""
                            )
                        )
                );
        }
    }
}