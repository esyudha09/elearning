using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning.TK;
using AI_ERP.Application_DAOs.Elearning.TK;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.TK
{
    public partial class wf_NilaiRaporEkskul : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATANILAIRAPOREKSKUL_TK";
        public const string C_ID = "{{id}}";

        public enum JenisItem
        {
            KategoriPencapaian,
            SubKategoriPencapaian,
            PoinKategoriPencapaian,
            Rekomendasi
        }

        public enum JenisInput
        {
            ItemKriteria,
            ItemReguler,
            ItemEkskul
        }

        public class KriteriaPenilaian
        {
            public string Kode { get; set; }
            public int Urut { get; set; }
        }

        public class ItemPenilaian
        {
            public string Kode { get; set; }
            public JenisItem JenisItemPenilaian { get; set; }
            public int Urut { get; set; }
        }

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
            DoUpdatePoinPenilaian,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowConfirmHapus,
            DoShowConfirmPostingByKelas,
            DoShowConfirmPostingBySiswa,
            DoShowInputDesainKategoriPencapaian,
            DoShowInputDesainSubKategoriPencapaian,
            DoShowInputDesainPoinKategoriPencapaian,
            DoShowKriteriaPencapaian,
            DoPostingPerKelas,
            DoPostingPerSiswa,
            DoUpdateUrut,
            DoShowEditKategoriPencapaian,
            DoShowEditSubKategoriPencapaian,
            DoShowEditPoinKategoriPencapaian,
            DoUpdateItem,
            DoShowPengaturanNilaiStandar,
            DoShowPengaturanNilaiStandarPerAnak,
            DoShowInputRekomendasi,
            DoShowPengaturanPeriode,
            DoShowPrintRapor,
            DoDataBinded,
            DoShowListSiswa
        }

        public static List<KriteriaPenilaian> lst_kriteria_penilaian = new List<KriteriaPenilaian>();
        public static List<ItemPenilaian> lst_item_penilaian = new List<ItemPenilaian>();

        public static class QS
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

                return GetUnitSekolah();
            }

            public static string GetKelas()
            {
                return Libs.GetQueryString("kd");
            }

            public static string GetLevel()
            {
                return Libs.GetQueryString("k").Replace(";", "");
            }

            public static string GetMapel()
            {
                return Libs.GetQueryString("m");
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
        }

        public static string GetUnitSekolah()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.TK).FirstOrDefault().Kode.ToString();
        }

        protected List<Siswa> GetListSiswa(string tahun_ajaran, string semester)
        {
            if (GetIDRaporDesign().Trim() == "") return DAO_Siswa.GetByRombel_Entity(
                         QS.GetUnit(),
                         txtKelasDet.Value,
                         tahun_ajaran,
                         semester
                     );

            Rapor_Design m = DAO_Rapor_Design.GetByID_Entity(GetIDRaporDesign());
            var lst_siswa_by_mapel = DAO_Rapor_DesignDetEkskul.GetByTAByRaporDesignByMapel_Entity(m.Kode.ToString(), txtKodeMapel.Value);

            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
                    {
                        if (GetIDRaporDesign().Trim() != "")
                        {
                            return DAO_Siswa.GetByRombel_Entity(
                                    GetUnitSekolah(),
                                    txtKelasDet.Value,
                                    m.TahunAjaran,
                                    m.Semester
                                ).Where(m0 => lst_siswa_by_mapel.Select(m1 => m1.Rel_Siswa.ToUpper()).ToList().FindAll(m1 => m1 == m0.Kode.ToString().ToUpper()).Count > 0).ToList();
                        }
                    }
                }

            }

            return DAO_Siswa.GetByRombel_Entity(
                        QS.GetUnit(),
                        txtKelasDet.Value,
                        tahun_ajaran,
                        semester
                    ).Where(m0 => lst_siswa_by_mapel.Select(m1 => m1.Rel_Siswa.ToUpper()).ToList().FindAll(m1 => m1 == m0.Kode.ToString().ToUpper()).Count > 0).ToList();
        }

        public static bool IsPosted(string tahun_ajaran, string semester, string rel_kelasdet)
        {
            Rapor_Nilai m_rapor = DAO_Rapor_Nilai.GetAllByTABySMByKelasDet_Entity(
                    tahun_ajaran, semester, rel_kelasdet
                ).FirstOrDefault();

            if (m_rapor != null)
            {
                if (m_rapor.TahunAjaran != null)
                {
                    return m_rapor.IsPosted;
                }
            }

            return false;
        }

        public static bool IsReadonly(string tahun_ajaran, string semester, string rel_kelasdet, string rel_siswa, string rel_mapel)
        {
            var nilai_log = DAO_Rapor_UploadHistMapel.Siswa.GetStatusUpload(
                    tahun_ajaran, semester, rel_kelasdet, rel_siswa, rel_mapel
                );

            if (nilai_log.Trim() != "")
            {
                if (
                    (nilai_log == PosisiUpload.GURU && Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()) ||
                    (nilai_log == PosisiUpload.PIMSEK && !Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
                )
                {
                    return true;
                }
            }
            else if (nilai_log == "" && Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
            {
                return true;
            }

            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            if (!IsPostBack)
            {
                InitKeyEventClient();
                ListDropdown();
                ShowListSiswa();

                if (!IsPostBack)
                {
                    cboPeriode.Items.Clear();
                    foreach (var item in DAO_Rapor_Design.GetAll_Entity().Select(
                        m => new { m.TahunAjaran, m.Semester }).Distinct().OrderByDescending(m => m.TahunAjaran).ThenByDescending(m => m.Semester).ToList())
                    {
                        cboPeriode.Items.Add(new ListItem
                        {
                            Value = item.TahunAjaran + "|" + item.Semester,
                            Text = item.TahunAjaran + " semester " + item.Semester
                        });
                    }
                }
            }

            if (mvMain.ActiveViewIndex == 1)
            {
                BindDataDesain();
            }
            else if (mvMain.ActiveViewIndex == 0)
            {
                BindListView(!IsPostBack, Libs.GetQ());
            }

            ShowDataSiswa();
            InitURLOnMenu();

            this.Master.HeaderTittle = "";
            this.Master.ShowHeaderSubTitle = false;
            this.Master.ShowSubHeaderGuru = false;
            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;
            
            btnDoTampilanData.Visible = Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit();
            div_periode_title.Visible = Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit();
        }

        protected void InitURLOnMenu()
        {
            string ft = Libs.Decryptdata(Libs.GetQueryString("ft"));
            string[] arr_bg =
                { "#FD6933", "#0AC6AE", "#F7921E", "#4AA4A4", "#43B8C9", "#95D1C5", "#019ADD", "#31384B", "#18AEC7", "#5299CF", "#2D2C28", "#D5C5C6", "#262726", "#01ACAC", "#322D3A", "#3B4F5D", "#009E00", "#E90080", "#549092", "#00A9A9", "#9B993A" };
            string[] arr_bg_image =
                { "a.png", "b.png", "c.png", "d.png", "e.png", "f.png", "g.png", "h.png", "i.png", "j.png", "k.png", "l.png", "m.png", "n.png", "o.png", "p.png", "q.png", "r.png", "u.png", "s.png", "t.png" };
            string bg_image = "";

            if (ft.Trim() != "")
            {
                int _id = 0;
                for (int i = 0; i < arr_bg.Length; i++)
                {
                    if (arr_bg_image[i] == ft)
                    {
                        _id = i;
                        bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + arr_bg_image[i]);
                        break;
                    }
                }
                ltrBGHeader.Text = "background: url(" + bg_image + "); background-color: " + arr_bg[_id] + "; background-size: 60px 60px; background-repeat: no-repeat;";
            }
            else
            {
                string s_background = "#295BC8";
                if (mvMain.ActiveViewIndex == 1)
                {
                    if (IsReadonly(txtTahunAjaran.Value, txtSemester.Value, txtKelasDet.Value, txtIDSiswa.Value, txtKodeMapel.Value)) s_background = "#992f52";
                }
                ltrBGHeader.Text = "background: url(" + ResolveUrl("~/Application_CLibs/images/svg/test.svg") + "); background-color: " + s_background + "; background-size: 40px 40px; background-repeat: no-repeat; background-position-x: 15px; background-position-y: 10px; background-repeat: no-repeat;";
            }

            string url_penilaian = Libs.GetURLPenilaian(Libs.GetQueryString("kd"));
            string m = Libs.GetQueryString("m");

            if (m.Trim() != "")
            {
                bool ada = false;
                Mapel mapel = DAO_Mapel.GetByID_Entity(m);
                if (mapel != null)
                {
                    if (mapel.Nama != null)
                    {
                        if (mapel.Jenis.Trim().ToLower() == Libs.JENIS_MAPEL.KHUSUS.Trim().ToLower())
                        {
                            Response.Redirect(
                                            ResolveUrl(
                                        Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWA.ROUTE +
                                        "?t=" + Libs.GetQueryString("t") +
                                        (
                                            Libs.GetQueryString("m").Trim() != ""
                                            ? "&m=" + Libs.GetQueryString("m")
                                            : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                                        ) +
                                        "&ft=" + Libs.GetQueryString("ft") +
                                        "&kd=" + Libs.GetQueryString("kd")
                                    )
                                );
                        }
                        else
                        {
                            this.Master.SetURLGuru_TimeLine(
                                ResolveUrl(
                                        Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE +
                                        "?t=" + Libs.GetQueryString("t") +
                                        (
                                            Libs.GetQueryString("m").Trim() != ""
                                            ? "&m=" + Libs.GetQueryString("m")
                                            : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                                        ) +
                                        "&ft=" + Libs.GetQueryString("ft") +
                                        "&kd=" + Libs.GetQueryString("kd")
                                    )
                            );
                        }
                    }
                }
                if (!ada)
                {
                    this.Master.SetURLGuru_TimeLine(
                            ResolveUrl(
                                    Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE +
                                    "?t=" + Libs.GetQueryString("t") +
                                    (
                                        Libs.GetQueryString("m").Trim() != ""
                                        ? "&m=" + Libs.GetQueryString("m")
                                        : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                                    ) +
                                    "&ft=" + Libs.GetQueryString("ft") +
                                    "&kd=" + Libs.GetQueryString("kd")
                                )
                        );
                }
            }
            else
            {
                this.Master.SetURLGuru_TimeLine(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE +
                            "?t=" + Libs.GetQueryString("t") +
                            (
                                Libs.GetQueryString("m").Trim() != ""
                                ? "&m=" + Libs.GetQueryString("m")
                                : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                            ) +
                            "&ft=" + Libs.GetQueryString("ft") +
                            "&kd=" + Libs.GetQueryString("kd")
                        )
                );
            }

            this.Master.SetURLGuru_Siswa(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWA.ROUTE +
                            "?t=" + Libs.GetQueryString("t") +
                            (
                                Libs.GetQueryString("m").Trim() != ""
                                ? "&m=" + Libs.GetQueryString("m")
                                : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                            ) +
                            "&ft=" + Libs.GetQueryString("ft") +
                            "&kd=" + Libs.GetQueryString("kd")
                        )
                );

            this.Master.SetURLGuru_Penilaian(
                    ResolveUrl(
                            url_penilaian +
                            "?t=" + Libs.GetQueryString("t") +
                            (
                                Libs.GetQueryString("m").Trim() != ""
                                ? "&m=" + Libs.GetQueryString("m")
                                : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                            ) +
                            "&ft=" + Libs.GetQueryString("ft") +
                            "&kd=" + Libs.GetQueryString("kd")
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

            string s_siswa_url = "";
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

            txtCountSiswa.Value = GetListSiswa(tahun_ajaran, semester).Count.ToString();
            txtIndexSiswa.Value = "0";

            int id = 1;
            foreach (Siswa m_siswa in GetListSiswa(tahun_ajaran, semester))
            {
                s_siswa_url += (s_siswa_url.Trim() != "" ? ";" : "") +
                               m_siswa.Kode.ToString();

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
                                                        "<span style=\"color: " +
                                                            (
                                                                IsReadonly(tahun_ajaran, semester, txtKelasDet.Value, m_siswa.Kode.ToString(), txtKodeMapel.Value)
                                                                ? "#b0315c; "
                                                                : "#295BC8; "
                                                            )
                                                            + "font-weight: bold;\">" +
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
                                                        (
                                                            IsReadonly(tahun_ajaran, semester, txtKelasDet.Value, m_siswa.Kode.ToString(), txtKodeMapel.Value)
                                                            ? "<a id=\"lbl_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                                                                    "onclick=\"ShowProsesPilihSiswa('" + m_siswa.Kode.ToString() + "', true); " + txtIndexSiswa.ClientID + ".value = '" + (id - 1) + "'; " + btnShowNilaiSiswa.ClientID + ".click(); \"" +
                                                                    "style=\"font-weight: bold; text-transform: none; padding-bottom: 2px; padding-top: 2px; background-color: #b0315c; color: white; border-radius: 15px; font-size: x-small;\" " +
                                                                    "class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                    "title=\" Buka \">" +
                                                                        "<i class=\"fa fa-lock\"></i>&nbsp;&nbsp;Buka" +
                                                              "</a>"
                                                            : "<a id=\"lbl_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                                                                    "onclick=\"ShowProsesPilihSiswa('" + m_siswa.Kode.ToString() + "', true); " + txtIndexSiswa.ClientID + ".value = '" + (id - 1) + "'; " + btnShowNilaiSiswa.ClientID + ".click(); \"" +
                                                                    "style=\"font-weight: bold; text-transform: none; padding-bottom: 2px; padding-top: 2px; background-color: #1DA1F2; color: white; border-radius: 15px; font-size: x-small;\" " +
                                                                    "class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                    "title=\" Buka \">" +
                                                                        "<i class=\"fa fa-folder-open\"></i>&nbsp;&nbsp;Buka" +
                                                              "</a>"
                                                        ) +
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

            string s_url = (
                            "'" +
                                ResolveUrl(
                                    Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.NILAI_SISWA_PRINT.ROUTE +
                                    (s_siswa_url.Trim() != "" ? "?sis=" : "") + s_siswa_url +
                                    (tahun_ajaran.Trim() != "" ? "&t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) : "") +
                                    (semester.Trim() != "" ? "&s=" + semester.Trim() : "")
                                ) +
                            "' + " +
                            "(" + txtKelasDet.ClientID + ".value.trim() !== '' ? '&kd=' + " + txtKelasDet.ClientID + ".value.trim() : '') + " +
                            "(" + txtID.ClientID + ".value.trim() !== '' ? '&kds=' + " + txtID.ClientID + ".value.trim() : '')"
                        );
        }

        protected void ShowDataSiswa()
        {
            string tahun_ajaran = "";
            string semester = "";

            txtLTSCK_Kehadiran.Text = "";

            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
            {
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
            }
            else
            {
                tahun_ajaran = txtTahunAjaran.Value;
                semester = txtSemester.Value;
            }

            if (Libs.GetStringToInteger(txtIndexSiswa.Value) >= 0 &&
                Libs.GetStringToInteger(txtCountSiswa.Value) > 0)
            {
                txtIDSiswa.Value = "";
                List<Siswa> lst_siswa = GetListSiswa(tahun_ajaran, semester);
                if (lst_siswa.Count > 0)
                {
                    Siswa m_siswa = lst_siswa[Libs.GetStringToInteger(txtIndexSiswa.Value)];
                    if (m_siswa != null)
                    {
                        if (m_siswa.Nama != null)
                        {
                            txtIDSiswa.Value = m_siswa.Kode.ToString();
                            lblNamaSiswa.Text = Libs.GetPersingkatNama(Libs.GetPerbaikiEjaanNama(m_siswa.Nama));
                            lblKelasSiswa.Text = DAO_KelasDet.GetByID_Entity(txtKelasDet.Value).Nama;
                            lblNamaSiswaInfo.Text = lblNamaSiswa.Text;
                            lblKelasSiswaInfo.Text = lblKelasSiswa.Text;
                            lblInfoPeriode.Text = "<span style=\"font-weight: bold;\">" +
                                                    txtTahunAjaran.Value +
                                                  "</span>" +
                                                  (
                                                    txtSemester.Value != ""
                                                    ? "&nbsp;Semester&nbsp;" +
                                                      "<span style=\"font-weight: bold;\">" +
                                                        txtSemester.Value +
                                                      "</span>"
                                                    : ""
                                                  );


                            string kode_nilai = "";
                            var m_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, txtKelasDet.Value).FirstOrDefault();
                            if (m_nilai != null)
                            {
                                if (m_nilai.TahunAjaran != null)
                                {
                                    kode_nilai = m_nilai.Kode.ToString();
                                }
                            }

                            var m_nilai_siswa = DAO_Rapor_NilaiSiswa.GetByHeader_Entity(kode_nilai).FindAll(m => m.Rel_Siswa == txtIDSiswa.Value).FirstOrDefault();
                            if (m_nilai_siswa != null)
                            {
                                if (m_nilai_siswa.Rel_Siswa != null)
                                {
                                    txtLTSCK_Kehadiran.Text = m_nilai_siswa.LTS_CK_KEHADIRAN;
                                }
                            }
                        }
                    }
                }
            }

            bool is_posted = IsReadonly(tahun_ajaran, semester, txtKelasDet.Value, txtIDSiswa.Value, txtKodeMapel.Value);
            txtLTSCK_Kehadiran.Enabled = !is_posted;
        }

        protected void ListDropdown()
        {
            List<Sekolah> lst_unit = DAO_Sekolah.GetAll_Entity();
            Sekolah unit = lst_unit.FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.TK).FirstOrDefault();
        }

        public static string GetByPassRaporDesignForEkskul(string rel_rapordesign, string rel_kelasdet)
        {
            Rapor_Design md = DAO_Rapor_Design.GetByID_Entity(rel_rapordesign);
            if (md != null)
            {
                if (md.TahunAjaran != null)
                {
                    if (md.TipeRapor.ToUpper().Trim() == TipeRapor.LTS.Trim().ToUpper())
                    {
                        var lst_rapor_design = DAO_Rapor_Design.GetAll_Entity().FindAll(
                                m0 => m0.TahunAjaran == md.TahunAjaran &&
                                      m0.Semester == md.Semester &&
                                      m0.Rel_Kelas.ToString().ToUpper().Trim() == DAO_KelasDet.GetByID_Entity(rel_kelasdet).Rel_Kelas.ToString().ToUpper().Trim() &&
                                      m0.TipeRapor.ToUpper().Trim() == TipeRapor.SEMESTER.Trim().ToUpper()
                            );
                        if (lst_rapor_design.Count == 1)
                        {
                            rel_rapordesign = lst_rapor_design.FirstOrDefault().Kode.ToString();
                        }
                    }
                }
            }

            return rel_rapordesign;
        }

        public static bool IsUdahDiaturNilaiDefault(string rel_rapordesign, string rel_kelasdet, string rel_mapel)
        {
            bool hasil = false;

            if (rel_kelasdet.Trim() != "")
            {
                rel_rapordesign = GetByPassRaporDesignForEkskul(rel_rapordesign, rel_kelasdet);

                hasil = (
                        DAO_Rapor_NilaiStandar.GetByRaporDesignByKelasDetByMapel(rel_rapordesign, rel_kelasdet, rel_mapel).Count > 0
                        ? hasil = true
                        : hasil = false
                    );
            }

            return hasil;
        }

        public static string GetHTMLRdoKriteriaPenilaian(string tahun_ajaran, string semester, string kelas_det, string rel_siswa, string rel_rapor_desain, string kode_item_desain, string rel_rapor_kriteria, string rel_mapel, AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor jenis, bool align_right = true)
        {
            string hasil = "";

            List<Rapor_DesignKriteria> lst_desain_kriteria = DAO_Rapor_DesignKriteria.GetByHeader_Entity(rel_rapor_desain).OrderBy(
                    m => m.Urut
                ).ToList();
            bool is_posted = IsReadonly(tahun_ajaran, semester, kelas_det, rel_siswa, rel_mapel);

            int id = 1;
            foreach (Rapor_DesignKriteria kriteria in lst_desain_kriteria)
            {
                string nama = kriteria.Alias;
                string deskripsi = Libs.GetHTMLSimpleText3(kriteria.NamaKriteria);

                if (jenis == AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.PoinKategoriPencapaian)
                {
                    //get data
                    string s_checked = "";
                    s_checked = (
                                rel_rapor_kriteria.Trim().ToUpper() == kriteria.Kode.ToString().Trim().ToUpper()
                                ? " checked "
                                : ""
                            );

                    string js_input_nilai = "txtPoinPenilaian().value = '" + kode_item_desain + "'; " +
                                            "txtKriteriaPenilaian().value = '" + kriteria.Kode.ToString() + "'; " +
                                            "SaveNilaiRapor(''); ";

                    if (!is_posted)
                    {
                        hasil += "<td style=\"width: 30px; padding: 0px; padding-right: 5px;\" title=\"" + deskripsi + "\">" +
                                    "<div class=\"checkbox checkbox-adv\" style=\"" + (align_right ? " float: right; " : "") + "\">" +
                                        "<label style=\"color: grey;\" for=\"chk_desain_" + kode_item_desain.Replace("-", "_") + "_" + id.ToString() + "\">" +
                                            "<input value=\"" + kriteria.Kode.ToString() + "\" " +
                                                   s_checked +
                                                   "onclick=\"CheckKriteriaPenilaian(this.name, this.id); " + js_input_nilai + "\" " +
                                                   "class=\"access-hide\" " +
                                                   "id=\"chk_desain_" + kode_item_desain.Replace("-", "_") + "_" + id.ToString() + "\" " +
                                                   "name=\"chk_desain_" + kode_item_desain.Replace("-", "_") + "[]\" " +
                                                   "type=\"checkbox\" />" +
                                            "<span style=\"font-weight: normal; font-size: small;\">" +
                                                nama +
                                            "</span>" +
                                            "<span class=\"checkbox-circle\"></span>" +
                                            "<span class=\"checkbox-circle-check\"></span>" +
                                            "<span class=\"checkbox-circle-icon icon\">done</span>" +
                                        "</label>" +
                                    "</div>" +
                                "</td>";

                    }
                    else
                    {
                        hasil += "<td style=\"width: 30px; padding: 0px; padding-right: 5px;\" title=\"" + deskripsi + "\">" +
                            "<div class=\"checkbox checkbox-adv\" style=\"" + (align_right ? " float: right; " : "") + "; cursor: default;\">" +
                                "<label style=\"color: grey; cursor: default;\" for=\"chk_desain_" + kode_item_desain.Replace("-", "_") + "_" + id.ToString() + "\">" +
                                    "<input value=\"" + kriteria.Kode.ToString() + "\" " +
                                           s_checked +
                                           "disabled=\"disabled\" " +
                                           "onclick=\"CheckKriteriaPenilaian(this.name, this.id); " + js_input_nilai + "\" " +
                                           "class=\"access-hide\" " +
                                           "id=\"chk_desain_" + kode_item_desain.Replace("-", "_") + "_" + id.ToString() + "\" " +
                                           "name=\"chk_desain_" + kode_item_desain.Replace("-", "_") + "[]\" " +
                                           "type=\"checkbox\" />" +
                                    (
                                        s_checked.Trim() != ""
                                        ? "<i class=\"fa fa-check-square\" style=\"color: #FF4081; font-weight: bold;\"></i>&nbsp;&nbsp;" +
                                          "<span style=\"font-weight: bold; font-size: small;\">" +
                                            nama +
                                          "</span>"
                                        : "<i class=\"fa fa-square-o\" style=\"color: #B8B8B8; font-weight: bold;\"></i>&nbsp;&nbsp;&nbsp;" +
                                          "<span style=\"font-weight: normal; font-size: small; color: #bfbfbf;\">" +
                                            nama +
                                          "</span>"
                                    ) +
                                "</label>" +
                            "</div>" +
                        "</td>";

                    }

                }
                else if (jenis == AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi)
                {
                    hasil += "";
                }
                else
                {
                    hasil += "<td style=\"width: 30px; padding: 0px; padding-right: 5px;\" title=\"" + deskripsi + "\">" +
                                "&nbsp;" +
                             "</td>";
                }

                id++;
            }

            return hasil;
        }

        public static string GetHTMLDeskripsiPenilaian(string tahun_ajaran, string semester, string kelas_det, string rel_siswa, string rel_rapor_desain, string kode_item_desain, AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor jenis, bool align_right = true)
        {
            string hasil = "";

            List<Rapor_DesignKriteria> lst_desain_kriteria = DAO_Rapor_DesignKriteria.GetByHeader_Entity(rel_rapor_desain).OrderBy(
                    m => m.Urut
                ).ToList();

            //load header nilai
            string kode_nilai = "";
            List<Rapor_Nilai> lst_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDet_Entity(
                        tahun_ajaran, semester, kelas_det
                    );
            if (lst_nilai.Count > 0) kode_nilai = lst_nilai.FirstOrDefault().Kode.ToString();

            List<Rapor_NilaiSiswa> lst_nilai_siswa = DAO_Rapor_NilaiSiswa.GetByHeader_Entity(kode_nilai);
            lst_nilai_siswa = lst_nilai_siswa.FindAll(m => m.Rel_Siswa == rel_siswa);

            string kode_nilai_siswa = "";
            if (lst_nilai_siswa.Count > 0) kode_nilai_siswa = lst_nilai_siswa.FirstOrDefault().Kode.ToString();
            //end load header nilai

            if (jenis == AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi)
            {
                if (kode_nilai_siswa.Trim() != "" && rel_siswa.Trim() != "")
                {
                    List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetPoinPenilaian_Entity(
                        kode_nilai_siswa, rel_siswa, kode_item_desain
                    );

                    if (lst_nilai_siswa_det.Count > 0)
                    {
                        if (lst_nilai_siswa_det.FirstOrDefault().Deskripsi != null)
                        {
                            if (lst_nilai_siswa_det.FirstOrDefault().Deskripsi.Trim() == "")
                            {
                                return "<br /><br /><br /><br />";
                            }

                            hasil = lst_nilai_siswa_det.FirstOrDefault().Deskripsi;
                            hasil = hasil.Replace("<ol>", "<ol style=\"margin: 0px 0; padding: 0 0 0 32px;\">");
                            hasil = hasil.Replace("<ul>", "<ol style=\"margin: 0px 0; padding: 0 0 0 32px;\">");

                            return Libs.GetHTMLSimpleText3(
                                        hasil,
                                        false
                                    );
                        }
                    }
                }
            }

            if (hasil.Trim() == "") hasil = "<br /><br /><br /><br />";

            return hasil;
        }

        public static string GetHTMLRdoCaptionKriteriaPenilaian(string rel_rapor_desain)
        {
            string hasil = "";

            List<Rapor_DesignKriteria> lst_desain_kriteria = DAO_Rapor_DesignKriteria.GetByHeader_Entity(rel_rapor_desain).OrderBy(
                    m => m.Urut
                ).ToList();

            int id = 1;
            foreach (Rapor_DesignKriteria kriteria in lst_desain_kriteria)
            {
                Rapor_Kriteria m = DAO_Rapor_Kriteria.GetByID_Entity(kriteria.Rel_Rapor_Kriteria.ToString());
                if (m != null)
                {
                    if (m.Nama != null)
                    {

                        hasil += "<td style=\"width: 30px; font-weight: bold;\">" +
                                    m.Nama +
                                 "</td>";

                    }
                }

                id++;
            }

            return hasil;
        }

        public static int GetColspanKriteriaPenilaian(string rel_rapor_desain, int penambah = 0)
        {
            List<Rapor_DesignKriteria> lst_desain_kriteria = DAO_Rapor_DesignKriteria.GetByHeader_Entity(rel_rapor_desain).OrderBy(
                    m => m.Urut
                ).ToList();

            return (lst_desain_kriteria.Count + penambah);
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
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

                if (keyword.Trim() != "")
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                    sql_ds.SelectParameters.Add("Semester", semester);
                    sql_ds.SelectParameters.Add("nama", keyword);
                    sql_ds.SelectCommand = DAO_Rapor_Design.SP_SELECT_NILAI_MAPEL_EKSKUL_BY_TAHUNAJARAN_BY_SEMESTER_FOR_SEARCH;
                }
                else
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                    sql_ds.SelectParameters.Add("Semester", semester);
                    sql_ds.SelectCommand = DAO_Rapor_Design.SP_SELECT_NILAI_MAPEL_EKSKUL_BY_TAHUNAJARAN_BY_SEMESTER;
                }

                ltrPeriode.Text = tahun_ajaran + "&nbsp;<sup style=\"color: yellow;\">" + semester + "</sup>";

                if (isbind) lvData.DataBind();
            }
            else
            {
                if (keyword.Trim() != "")
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("Rel_Kelas", QS.GetLevel());
                    sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                    sql_ds.SelectParameters.Add("Rel_Guru", Libs.LOGGED_USER_M.NoInduk);
                    sql_ds.SelectParameters.Add("Rel_Mapel", QS.GetMapel());
                    sql_ds.SelectParameters.Add("nama", keyword);
                    sql_ds.SelectCommand = DAO_Rapor_Design.SP_SELECT_BY_KELAS_BY_TAHUNAJARAN_BY_GURU_BY_MAPEL_FOR_SEARCH;
                }
                else
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("Rel_Kelas", QS.GetLevel());
                    sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                    sql_ds.SelectParameters.Add("Rel_Guru", Libs.LOGGED_USER_M.NoInduk);
                    sql_ds.SelectParameters.Add("Rel_Mapel", QS.GetMapel());
                    sql_ds.SelectCommand = DAO_Rapor_Design.SP_SELECT_BY_KELAS_BY_TAHUNAJARAN_BY_GURU_BY_MAPEL;
                }
                if (isbind) lvData.DataBind();
            }

            txtKeyAction.Value = JenisAction.DoDataBinded.ToString();
        }

        private void BindListViewEkskulBySiswa(string rel_desain_rapor, string rel_siswa, bool isbind = true)
        {
            sql_ds_ekskul.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_ekskul.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_ekskul.SelectParameters.Clear();
            sql_ds_ekskul.SelectParameters.Add("Rel_Rapor_Design", rel_desain_rapor);
            sql_ds_ekskul.SelectParameters.Add("Rel_Siswa", rel_siswa);
            sql_ds_ekskul.SelectCommand = DAO_Rapor_DesignDetEkskul.SP_SELECT_BY_HEADER_BY_SISWA;

            Rapor_Design m = DAO_Rapor_Design.GetByID_Entity(GetIDRaporDesign());
            div_body_nilai_rapor.Attributes["style"] = "background-color: #3367D6; " +
                                                       "box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23);  " +
                                                       "background-image: none;  " +
                                                       "color: white; " +
                                                       "display: block; " +
                                                       "z-index: 5; " +
                                                       "position: fixed; bottom: 50px; right: -25px; width: 320px; border-radius: 25px; " +
                                                       "padding: 12px;  " +
                                                       "padding-top: 0px; " +
                                                       "padding-left: 0px; " +
                                                       "margin: 0px;";
            div_header_nilai_rapor.Attributes["style"] = "width: 100%; background-color: #295BC8; padding: 10px;";
            btnDoPengaturan.Visible = true;
            btnDoPostingData.Visible = true;
            ltrHeaderNilaiRapor.Text = "<span style=\"font-weight: bold; float: right; color: #99b9ff;\" title=\" Data dapat diubah \">" +
                                            "<i class=\"fa fa-unlock\"></i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                       "</span>";

            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    string s_kelas = txtKelasDet.Value;
                    if (IsReadonly(m.TahunAjaran, m.Semester, s_kelas, txtIDSiswa.Value, txtKodeMapel.Value))
                    {
                        div_body_nilai_rapor.Attributes["style"] = "background-color: #b0315c; " +
                                                       "box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23);  " +
                                                       "background-image: none;  " +
                                                       "color: white; " +
                                                       "display: block; " +
                                                       "z-index: 5; " +
                                                       "position: fixed; bottom: 50px; right: -25px; width: 320px; border-radius: 25px; " +
                                                       "padding: 12px;  " +
                                                       "padding-top: 0px; " +
                                                       "padding-left: 0px; " +
                                                       "margin: 0px;";
                        div_header_nilai_rapor.Attributes["style"] = "width: 100%; background-color: #992f52; padding: 10px;";

                        btnDoPengaturan.Visible = false;
                        btnDoPostingData.Visible = false;
                        ltrHeaderNilaiRapor.Text = "<span style=\"font-weight: bold; float: right; color: #ff7b7b;\" title=\" Data terkunci \">" +
                                                        "<i class=\"fa fa-lock\"></i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                                   "</span>";
                    }
                }
            }

            if (isbind) lvEkskul.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            System.Web.UI.WebControls.Literal imgh_semester = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_semester");
            System.Web.UI.WebControls.Literal imgh_urutan_level = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_urutan_level");

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
            imgh_semester.Text = html_image;
            imgh_urutan_level.Text = html_image;

            imgh_tahunajaran.Visible = false;
            imgh_semester.Visible = false;
            imgh_urutan_level.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
                    break;
                case "Semester":
                    imgh_semester.Visible = true;
                    break;
                case "UrutanLevel":
                    imgh_urutan_level.Visible = true;
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
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (GetIDRaporDesign().Trim() != "")
                {
                    DAO_Rapor_Design.Delete(GetIDRaporDesign(), Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            Response.Redirect(Libs.FILE_PAGE_URL + (this.Master.txtCariData.Text.Trim() != "" ? "?q=" + this.Master.txtCariData.Text : ""));
        }

        protected void ShowDataList()
        {
            BindListView(true, Libs.GetQ());
            mvMain.ActiveViewIndex = 0;
        }

        protected void ShowDesain()
        {
            if (GetIDRaporDesign().Trim() != "")
            {
                Rapor_Design m = DAO_Rapor_Design.GetByID_Entity(GetIDRaporDesign());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        ltrInfoDesain.Text = "<span style=\"font-weight: bold;\">" + m.TahunAjaran + "</span>" +
                                             "&nbsp;&nbsp;" +
                                             "<i class=\"fa fa-arrow-right\" style=\"font-weight: normal;\"></i>" +
                                             "&nbsp;&nbsp;" +
                                             "Semester " +
                                             "<span style=\"font-weight: bold;\">" + m.Semester + "</span>" +
                                             "&nbsp;&nbsp;" +
                                             "<i class=\"fa fa-arrow-right\" style=\"font-weight: normal;\"></i>" +
                                             "&nbsp;&nbsp;" +
                                             "Kelas " +
                                             "<span style=\"font-weight: bold;\">" +
                                                DAO_KelasDet.GetByID_Entity(txtKelasDet.Value).Nama +
                                             "</span>";

                        this.Session[SessionViewDataName] = 0;
                        this.Master.ShowHeaderTools = false;

                        txtTahunAjaran.Value = m.TahunAjaran;
                        txtSemester.Value = m.Semester;

                        BindDataDesain();
                        mvMain.ActiveViewIndex = 1;
                    }
                }
            }
        }

        protected string GetIDRaporDesign()
        {
            return GetByPassRaporDesignForEkskul(txtID.Value, GetKelas());
        }

        protected void BindDataDesain()
        {
            BindListViewEkskulBySiswa(GetIDRaporDesign(), txtIDSiswa.Value, true);
        }

        public string GetKelas()
        {
            return txtKelasDet.Value;
        }

        protected void btnShowDesain_Click(object sender, EventArgs e)
        {
            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
            {
                ShowListSiswa();
                ShowDataSiswa();
                Rapor_Design m = DAO_Rapor_Design.GetByID_Entity(GetIDRaporDesign());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtTahunAjaran.Value = m.TahunAjaran;
                        txtSemester.Value = m.Semester;
                    }
                }
                BindDataDesain();
            }
            else
            {
                ShowListSiswa();
                Rapor_Design m = DAO_Rapor_Design.GetByID_Entity(GetIDRaporDesign());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtTahunAjaran.Value = m.TahunAjaran;
                        txtSemester.Value = m.Semester;
                    }
                }
                ShowDataSiswa();
            }

            ShowDesain();
            InitURLOnMenu();
        }

        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            ShowDataList();
        }

        protected void lvDesain_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName] = e.StartRowIndex;
        }

        protected void btnDoKembali_Click(object sender, EventArgs e)
        {
            ShowDataList();
            InitURLOnMenu();
        }

        protected void btnDoPostingData_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowConfirmPostingBySiswa.ToString();
        }

        protected void btnShowNilaiSiswa_Click(object sender, EventArgs e)
        {
            ShowDataSiswa();
            BindDataDesain();
        }

        protected void UpdateDeskripsi()
        {
            //update header
            List<Rapor_Nilai> lst_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDet_Entity(
                    txtTahunAjaran.Value, txtSemester.Value, txtKelasDet.Value
                );

            Guid kode = Guid.NewGuid();
            if (lst_nilai.Count == 0)
            {
                DAO_Rapor_Nilai.Insert(new Rapor_Nilai
                {
                    Kode = kode,
                    TahunAjaran = txtTahunAjaran.Value,
                    Semester = txtSemester.Value,
                    Rel_KelasDet = txtKelasDet.Value,
                    IsLocked = false,
                    IsPosted = false
                });
            }
            else
            {
                kode = lst_nilai.FirstOrDefault().Kode;
            }

            //update detail 1
            Guid kode_nilai_siswa = Guid.NewGuid();
            List<Rapor_NilaiSiswa> lst_nilai_siswa = DAO_Rapor_NilaiSiswa.GetByHeader_Entity(kode.ToString());
            lst_nilai_siswa = lst_nilai_siswa.FindAll(m => m.Rel_Siswa == txtIDSiswa.Value);

            if (lst_nilai_siswa.Count == 0)
            {
                DAO_Rapor_NilaiSiswa.Insert(new Rapor_NilaiSiswa
                {
                    Kode = kode_nilai_siswa,
                    Rel_Rapor_Nilai = kode,
                    Rel_Siswa = txtIDSiswa.Value,
                    IsLocked = false,
                    IsPosted = false
                });
            }
            else
            {
                kode_nilai_siswa = lst_nilai_siswa.FirstOrDefault().Kode;
            }

            //update detail 2 (nilai)
            Guid kode_nilai_siswa_det = Guid.NewGuid();
            List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetPoinPenilaian_Entity(
                    kode_nilai_siswa.ToString(), txtIDSiswa.Value, txtPoinPenilaianDeskripsi.Value
                );

            if (lst_nilai_siswa_det.Count == 0)
            {
                DAO_Rapor_NilaiSiswa_Det.Insert(new Rapor_NilaiSiswa_Det
                {
                    Kode = kode_nilai_siswa_det,
                    Rel_Siswa = txtIDSiswa.Value,
                    Rel_Rapor_NilaiSiswa = kode_nilai_siswa,
                    Rel_Rapor_DesignDet = txtPoinPenilaianDeskripsi.Value,
                    Rel_Rapor_Kriteria = "",
                    Deskripsi = txtDeskripsi.Value
                });
            }
            else
            {
                kode_nilai_siswa_det = lst_nilai_siswa_det.FirstOrDefault().Kode;
                DAO_Rapor_NilaiSiswa_Det.Update(new Rapor_NilaiSiswa_Det
                {
                    Kode = kode_nilai_siswa_det,
                    Rel_Siswa = txtIDSiswa.Value,
                    Rel_Rapor_NilaiSiswa = kode_nilai_siswa,
                    Rel_Rapor_DesignDet = txtPoinPenilaianDeskripsi.Value,
                    Rel_Rapor_Kriteria = "",
                    Deskripsi = txtDeskripsi.Value
                });
            }
        }

        protected void btnDoSaveDeskripsi_Click(object sender, EventArgs e)
        {
            UpdateDeskripsi();
            upMainDeskripsi.Update();
        }

        protected void lnkOKPosting_Click(object sender, EventArgs e)
        {
            try
            {
                if (GetIDRaporDesign().Trim() != "")
                {
                    Rapor_Design m = DAO_Rapor_Design.GetByID_Entity(GetIDRaporDesign());
                    if (m != null)
                    {
                        if (m.TahunAjaran != null)
                        {
                            string s_status = "";
                            string rel_kelas = txtKelasDet.Value;
                            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
                            {
                                s_status = PosisiUpload.GURU;
                            }
                            else if (!Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
                            {
                                s_status = PosisiUpload.PIMSEK;
                            }

                            //upload per siswa
                            if (mvMain.ActiveViewIndex == 1 && txtIDSiswa.Value.Trim() != "")
                            {
                                DAO_Rapor_UploadHistMapel.Siswa.Insert(new Rapor_UploadHistSiswaMapel
                                {
                                    Kode = Guid.NewGuid(),
                                    TahunAjaran = m.TahunAjaran,
                                    Semester = m.Semester,
                                    PosisiUpload = s_status,
                                    Rel_KelasDet = rel_kelas,
                                    Tanggal = DateTime.Now,
                                    Rel_Siswa = txtIDSiswa.Value,
                                    Rel_Mapel = txtKodeMapel.Value
                                });
                                DoBindData();
                                txtKeyAction.Value = JenisAction.DoPostingPerSiswa.ToString();
                            }
                            //upload per kelas
                            else if (mvMain.ActiveViewIndex == 0)
                            {
                                DAO_Rapor_UploadHistMapel.Kelas.Insert(new Rapor_UploadHistKelasMapel
                                {
                                    Kode = Guid.NewGuid(),
                                    TahunAjaran = m.TahunAjaran,
                                    Semester = m.Semester,
                                    PosisiUpload = s_status,
                                    Rel_KelasDet = rel_kelas,
                                    Tanggal = DateTime.Now,
                                    Rel_Mapel = txtKodeMapel.Value
                                });
                                BindListView(true, Libs.GetQ());
                                txtKeyAction.Value = JenisAction.DoPostingPerKelas.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
            finally
            {
                InitURLOnMenu();
            }
        }

        protected void btnShowConfirmPostingByKelas_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowConfirmPostingByKelas.ToString();
        }

        protected void ShowListKriteriaPenilaian()
        {
            ltrKriteriaPenilaian.Text = "";
            string html = "";
            foreach (var item in DAO_Rapor_DesignKriteria.GetByHeader_Entity(GetIDRaporDesign()).OrderBy(m => m.Urut).ToList())
            {
                Rapor_Kriteria m_kriteria = DAO_Rapor_Kriteria.GetByID_Entity(item.Rel_Rapor_Kriteria.ToString());
                if (m_kriteria != null)
                {
                    if (m_kriteria.Nama != null)
                    {
                        string s_id = "chk_" + item.Kode.ToString().Replace("-", "_");
                        html += "<div class=\"row\">" +
                                    "<div class=\"col-xs-12\">" +
                                        "<div class=\"form-group form-group-label\" style=\"margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px; margin-bottom: 0px;\">" +
                                            "<div class=\"radiobtn radiobtn-adv\">" +
                                                "<label for=\"" + s_id + "\" style=\"font-weight: bold; color: grey;\">" +
                                                    "<input " + (item.Kode.ToString().ToLower().Trim() == "".ToLower().Trim() ? " checked=\"checked\" " : "") + " value=\"" + item.Kode.ToString() + "\" name=\"rdo_kriteria[]\" type=\"radio\" id=\"" + s_id + "\" class=\"access-hide\" />" +
                                                    "<span class=\"radiobtn-circle\"></span>" +
                                                    "<span class=\"radiobtn-circle-check\"></span>" +
                                                    "<span style=\"font-weight: bold; color: black;\">" +
                                                        m_kriteria.Alias +
                                                    "</span>" +
                                                    "<br />" +
                                                    "<span style=\"font-weight: normal;\">" +
                                                        Libs.GetHTMLSimpleText3(m_kriteria.Nama) +
                                                    "</span>" +
                                                "</label>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                                "<div class=\"row\">" +
                                    "<div class=\"col-xs-12\" style=\"margin: 0px;\">" +
                                        "<hr style=\"margin: 0px;\" />" +
                                    "</div>" +
                                "</div>";
                    }
                }
            }
            ltrKriteriaPenilaian.Text = html;
        }

        protected void btnShowPengaturanNilaiStandar_Click(object sender, EventArgs e)
        {
            ShowListKriteriaPenilaian();
            txtJenisNilaiDefault.Value = "";
            txtKeyAction.Value = JenisAction.DoShowPengaturanNilaiStandar.ToString();
        }

        protected void btnDoBindListViewNilai_Click(object sender, EventArgs e)
        {
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void btnShowDeskripsi_Click(object sender, EventArgs e)
        {
            txtRekomendasi.Text = "";
            txtRekomendasiVal.Value = "";
            if (txtIDRekomendasi.Value.Trim() != "")
            {
                string deskripsi = GetHTMLDeskripsiPenilaian(
                        txtTahunAjaran.Value,
                        txtSemester.Value,
                        txtKelasDet.Value,
                        txtIDSiswa.Value,
                        GetIDRaporDesign(),
                        txtIDRekomendasi.Value,
                        AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi
                    );
                txtRekomendasi.Text = deskripsi;
                txtRekomendasiVal.Value = deskripsi;
            }
            txtKeyAction.Value = JenisAction.DoShowInputRekomendasi.ToString();
        }

        protected void DoBindData()
        {
            BindDataDesain();
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void btnDoBindData_Click(object sender, EventArgs e)
        {
            DoBindData();
        }

        protected void btnDoPengaturan_Click(object sender, EventArgs e)
        {
            ShowListKriteriaPenilaian();
            txtJenisNilaiDefault.Value = txtIDSiswa.Value;
            txtKeyAction.Value = JenisAction.DoShowPengaturanNilaiStandarPerAnak.ToString();
        }

        protected void lnkOKRekomendasi_Click(object sender, EventArgs e)
        {
            string deskripsi = txtRekomendasiVal.Value;
            DAO_Rapor_Nilai.SaveNilai(
                    txtTahunAjaran.Value,
                    txtSemester.Value,
                    txtIDSiswa.Value,
                    txtPoinPenilaian.Value,
                    txtKelasDet.Value,
                    txtKriteriaPenilaian.Value,
                    deskripsi
                );

            DoBindData();
        }

        protected void lnkOKPeriode_Click(object sender, EventArgs e)
        {
            BindListView(true, Libs.GetQ());
        }

        protected void btnDoTampilanData_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowPengaturanPeriode.ToString();
        }

        public static int GetJumlahNoReadOnly(string rel_kelasdet, string rel_designrapor, string rel_mapel)
        {
            rel_designrapor = GetByPassRaporDesignForEkskul(rel_designrapor, rel_kelasdet);

            var m_rapor_design = DAO_Rapor_Design.GetByID_Entity(rel_designrapor);
            if (m_rapor_design != null)
            {
                if (m_rapor_design.TahunAjaran != null)
                {
                    var lst_siswa_by_mapel = DAO_Rapor_DesignDetEkskul.GetByTAByRaporDesignByMapel_Entity(m_rapor_design.Kode.ToString(), rel_mapel);
                    var lst_siswa = DAO_Siswa.GetByRombel_Entity(
                                GetUnitSekolah(),
                                rel_kelasdet,
                                m_rapor_design.TahunAjaran,
                                m_rapor_design.Semester
                            ).Where(m0 => lst_siswa_by_mapel.Select(m1 => m1.Rel_Siswa.ToUpper()).ToList().FindAll(m1 => m1 == m0.Kode.ToString().ToUpper()).Count > 0).ToList();

                    int jml_not_readonly = 0;
                    foreach (var siswa in lst_siswa)
                    {
                        if (!IsReadonly(m_rapor_design.TahunAjaran, m_rapor_design.Semester, rel_kelasdet, siswa.Kode.ToString(), rel_mapel)) jml_not_readonly++;
                    }

                    return jml_not_readonly;
                }
            }

            return 0;
        }

        public static string GetHTMLNotifJumlahSiswa(string rel_kelasdet, string rel_designrapor, string rel_mapel)
        {
            int jml_not_readonly = GetJumlahNoReadOnly(rel_kelasdet, rel_designrapor, rel_mapel);
            if (jml_not_readonly > 0)
            {
                var m_rapor_design = DAO_Rapor_Design.GetByID_Entity(rel_designrapor);
                int jml_siswa = 0;
                if (m_rapor_design != null)
                {
                    if (m_rapor_design.TahunAjaran != null)
                    {
                        var lst_siswa = DAO_Siswa.GetByRombel_Entity(
                                    GetUnitSekolah(),
                                    rel_kelasdet,
                                    m_rapor_design.TahunAjaran,
                                    m_rapor_design.Semester
                                );
                        jml_siswa = lst_siswa.Count;
                    }
                }

                return "<sup class=\"badge\" " +
                            "title=\"" + jml_not_readonly.ToString() + "&nbsp;dari&nbsp;" +
                                         jml_siswa.ToString() + "&nbsp;siswa bisa diedit nilainya\" " +
                            "style=\"background-color: #d93fa9; color: white; font-size: xx-small; padding: 3px; margin-top: -10px; border-bottom-left-radius: 0px;\">" +
                                "&nbsp;" + jml_not_readonly.ToString() + "&nbsp;" +
                        "</sup>";
            }

            return "";
        }

        protected void btnShowDataListSiswa_Click(object sender, EventArgs e)
        {
            ShowListSiswa();
            txtKeyAction.Value = JenisAction.DoShowListSiswa.ToString();
        }
    }
}