using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Modules.EDUCATION.Elearning
{
    public partial class wf_Guru_DataSiswa : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATASISWABYGURU";
        public const string C_ID = "{{id}}";

        public enum JenisAction
        {
            AddCatatanSiswa,
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

        protected void ListKategoriCatatan()
        {
            cboKategoriCatatan.Items.Clear();
            cboKategoriCatatan.Items.Add("");
            cboKategoriCatatan.Items.Add(new ListItem { Value = KategoriCatatanSiswa.Prestasi.Kode, Text = KategoriCatatanSiswa.Prestasi.Nama });
            cboKategoriCatatan.Items.Add(new ListItem { Value = KategoriCatatanSiswa.Pelanggaran.Kode, Text = KategoriCatatanSiswa.Pelanggaran.Nama });
            cboKategoriCatatan.Items.Add(new ListItem { Value = KategoriCatatanSiswa.Lainnya.Kode, Text = KategoriCatatanSiswa.Lainnya.Nama });
        }

        protected void ListSiswa()
        {
            bool b_mapel_pilihan = false;
            Mapel m_mapel = DAO_Mapel.GetByID_Entity(QS.GetMapel());
            Kelas m_kelas = DAO_Kelas.GetByID_Entity(DAO_KelasDet.GetByID_Entity(QS.GetKelas()).Rel_Kelas.ToString());
            List<Siswa> lst_siswa = new List<Siswa>();

            cboSiswa.Items.Clear();
            if (m_mapel != null)
            {
                if (m_mapel.Nama != null)
                {
                    b_mapel_pilihan = (m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN && m_kelas.Nama.Trim().ToUpper() != "X" ? true : false);
                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN && m_kelas.Nama.Trim().ToUpper() == "X")
                    {
                        if (DAO_FormasiGuruMapelDet.IsSiswaPilihanByGuru(
                                            Libs.LOGGED_USER_M.NoInduk,
                                            QS.GetTahunAjaran(),
                                            Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                                            QS.GetKelas(),
                                            QS.GetMapel()
                                        ))
                        {
                            b_mapel_pilihan = true;
                        }
                    }
                }
            }

            if (b_mapel_pilihan)
            {
                lst_siswa = DAO_FormasiGuruMapelDetSiswaDet.GetSiswaByTABySMByMapelByKelasDet_Entity(
                        QS.GetTahunAjaran(),
                        Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                        QS.GetMapel(),
                        QS.GetKelas()
                    );
            } else {
                if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT)
                {
                    lst_siswa = DAO_FormasiGuruMapelDetSiswa.GetSiswaByTABySMByMapelByKelasByKelasDet_Entity(
                            QS.GetTahunAjaran(),
                            QS.GetSemester(),
                            QS.GetMapel(),
                            m_kelas.Kode.ToString(),
                            QS.GetKelas()
                        );
                }
                else
                {
                    lst_siswa = DAO_Siswa.GetByRombel_Entity(
                            QS.GetUnit(),
                            QS.GetKelas(),
                            QS.GetTahunAjaran(),
                            QS.GetSemester()
                        );

                    if (lst_siswa.Count == 0)
                    {
                        lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                        QS.GetUnit(), QS.GetKelas(), QS.GetTahunAjaran(), QS.GetSemester()
                                    );
                    }
                }
            }

            cboSiswa.Items.Add("");
            foreach (Siswa m in lst_siswa)
            {
                cboSiswa.Items.Add(new ListItem {
                    Value = m.Kode.ToString(),
                    Text = m.Nama.Trim().ToUpper()
                });
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.ShowHeaderSubTitle = false;
            this.Master.ShowSubHeaderGuru = true;
            this.Master.SelectMenuGuru_Siswa();
            InitURLOnMenu();

            BindListView(!IsPostBack);
            BindListViewCatatan(!IsPostBack);
            if (!IsPostBack)
            {
                ListKategoriCatatan();
                ListSiswa();
                this.Master.txtCariData.Text = Libs.GetQ();
            }
        }

        protected void InitURLOnMenu()
        {
            string ft = Libs.Decryptdata(Libs.GetQueryString("ft"));
            string url_penilaian = Libs.GetURLPenilaian(Libs.GetQueryString("kd"));
            string m = Libs.GetQueryString("m");

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

            if (m.Trim() != "")
            {
                bool ada = false;
                Mapel mapel = DAO_Mapel.GetByID_Entity(m);
                if (mapel != null)
                {
                    if (mapel.Nama != null)
                    {
                        if (mapel.Jenis.Trim().ToLower() == Libs.JENIS_MAPEL.KHUSUS.ToLower().Trim())
                        {
                            this.Master.SetURLGuru_TimeLine(
                                ""
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
                                        (
                                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                            ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                            : ""
                                        ) +
                                        (
                                            Libs.GetQueryString("ft").Trim() != ""
                                            ? "&" + "ft=" + Libs.GetQueryString("ft").Trim()
                                            : "&ft=" + Libs.Encryptdata(arr_bg_image[1])
                                        ) +
                                        (
                                            Libs.GetQueryString("g").Trim() != ""
                                            ? "&" + "g=" + Libs.GetQueryString("g").Trim()
                                            : ""
                                        ) +
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
                                    (
                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                        ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                        : ""
                                    ) +
                                    (
                                        Libs.GetQueryString("ft").Trim() != ""
                                        ? "&" + "ft=" + Libs.GetQueryString("ft").Trim()
                                        : "&ft=" + Libs.Encryptdata(arr_bg_image[1])
                                    ) +
                                    (
                                        Libs.GetQueryString("g").Trim() != ""
                                        ? "&" + "g=" + Libs.GetQueryString("g").Trim()
                                        : ""
                                    ) +
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
                            (
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                : ""
                            ) +
                            (
                                Libs.GetQueryString("ft").Trim() != ""
                                ? "&" + "ft=" + Libs.GetQueryString("ft").Trim()
                                : "&ft=" + Libs.Encryptdata(arr_bg_image[1])
                            ) +
                            (
                                Libs.GetQueryString("g").Trim() != ""
                                ? "&" + "g=" + Libs.GetQueryString("g").Trim()
                                : ""
                            ) +
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
                            (
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                : ""
                            ) +
                            (
                                Libs.GetQueryString("ft").Trim() != ""
                                ? "&" + "ft=" + Libs.GetQueryString("ft").Trim()
                                : "&ft=" + Libs.Encryptdata(arr_bg_image[1])
                            ) +
                            (
                                Libs.GetQueryString("g").Trim() != ""
                                ? "&" + "g=" + Libs.GetQueryString("g").Trim()
                                : ""
                            ) +
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
                            (
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                : ""
                            ) +
                            (
                                Libs.GetQueryString("ft").Trim() != ""
                                ? "&" + "ft=" + Libs.GetQueryString("ft").Trim()
                                : "&ft=" + Libs.Encryptdata(arr_bg_image[1])
                            ) +
                            (
                                Libs.GetQueryString("g").Trim() != ""
                                ? "&" + "g=" + Libs.GetQueryString("g").Trim()
                                : ""
                            ) +
                            "&kd=" + Libs.GetQueryString("kd") 
                        )
                );

            lnkCatatanSaya.NavigateUrl = 
                ResolveUrl(
                    Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWACATATANEDIT.ROUTE +
                    "?t=" + Libs.GetQueryString("t") +
                    (
                        Libs.GetQueryString("m").Trim() != ""
                        ? "&m=" + Libs.GetQueryString("m")
                        : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                    ) +
                    (
                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                        ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                        : ""
                    ) +
                    (
                        Libs.GetQueryString("ft").Trim() != ""
                        ? "&" + "ft=" + Libs.GetQueryString("ft").Trim()
                        : "&ft=" + Libs.Encryptdata(arr_bg_image[1])
                    ) +
                    (
                        Libs.GetQueryString("g").Trim() != ""
                        ? "&" + "g=" + Libs.GetQueryString("g").Trim()
                        : ""
                    ) +
                    "&kd=" + Libs.GetQueryString("kd")
                );

            string s_html = "";
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(QS.GetKelas());

            string s_mapel = "";
            if (QS.GetMapel().Trim() != "")
            {
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(QS.GetMapel());

                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {
                        s_mapel = m_mapel.Nama;
                    }
                }
            }

            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    if (s_mapel.Trim() != "")
                    {
                        s_html = "<label style=\"margin-left: 45px; color: white;\">" +
                                    "Kelas" +
                                    "&nbsp;" +
                                    "<span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span>" +
                                    "&nbsp;" +
                                    tahun_ajaran +
                                 "</label>" +
                                 "<br />" +
                                 "<label style=\"margin-left: 45px; color: white; font-weight: bold;\">" +
                                    s_mapel +
                                 "</label>";
                    }
                    else
                    {
                        s_html = "<label style=\"margin-left: 45px; color: white;\">" +
                                    "Kelas" +
                                    "&nbsp;" +
                                    "<span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span>" +
                                    "&nbsp;" +
                                    tahun_ajaran +
                                    "<br />" +
                                    "<label style=\"font-size: medium; color: white; font-size: small; font-weight: bold; color: yellow;\">Guru Kelas atau Wali Kelas</label>" +
                                 "</label>";
                    }
                }
            }

            ltrCaption.Text = s_html;
        }

        private static class QS
        {
            public static string GetUnit()
            {
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(Libs.GetQueryString("kd"));
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
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

            public static string GetMapel()
            {
                return Libs.GetQueryString("m"); 
            }
            public static string GetLevel()
            {
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(Libs.GetQueryString("kd"));
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        return m_kelas_det.Rel_Kelas.ToString();
                    }
                }

                return "";
            }

            public static string GetSemester()
            {
                string s = Libs.GetQueryString("s");
                if (s.Trim() == "") return Libs.GetSemesterByTanggal(DateTime.Now).ToString();
                return Libs.GetQueryString("s").Trim();
            }
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(QS.GetUnit());
            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {

                    bool b_mapel_pilihan = false;
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(QS.GetMapel());
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(DAO_KelasDet.GetByID_Entity(QS.GetKelas()).Rel_Kelas.ToString());
                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
                        {
                            b_mapel_pilihan = (m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN && m_kelas.Nama.Trim().ToUpper() != "X" ? true : false);
                            if (m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN && m_kelas.Nama.Trim().ToUpper() == "X")
                            {
                                if (DAO_FormasiGuruMapelDet.IsSiswaPilihanByGuru(
                                                    Libs.LOGGED_USER_M.NoInduk,
                                                    QS.GetTahunAjaran(),
                                                    Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                                                    QS.GetKelas(),
                                                    QS.GetMapel()
                                                ))
                                {
                                    b_mapel_pilihan = true;
                                }
                            }
                        }
                    }

                    if (b_mapel_pilihan)
                    {
                        if (keyword.Trim() != "")
                        {
                            sql_ds.SelectParameters.Clear();
                            sql_ds.SelectParameters.Add("nama", keyword);
                            sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                            sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
                            sql_ds.SelectParameters.Add("Rel_Mapel", QS.GetMapel());
                            sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
                            sql_ds.SelectCommand = DAO_FormasiGuruMapelDetSiswaDet.SP_SELECT_SISWA_BY_TA_BY_SM_BY_MAPEL_BY_KELAS_DET_FOR_SEARCH;
                        }
                        else
                        {
                            sql_ds.SelectParameters.Clear();
                            sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                            sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
                            sql_ds.SelectParameters.Add("Rel_Mapel", QS.GetMapel());
                            sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
                            sql_ds.SelectCommand = DAO_FormasiGuruMapelDetSiswaDet.SP_SELECT_SISWA_BY_TA_BY_SM_BY_MAPEL_BY_KELAS_DET;
                        }
                        if (isbind) lvData.DataBind();
                    } else {
                        if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && QS.GetMapel().Trim() == "")
                        {
                            if (keyword.Trim() != "")
                            {
                                sql_ds.SelectParameters.Clear();
                                sql_ds.SelectParameters.Add("nama", keyword);
                                sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                                sql_ds.SelectParameters.Add("Rel_KelasDetPerwalian", QS.GetKelas());
                                sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                                sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
                                sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL_BY_KELAS_PERWALIAN_FOR_SEARCH;
                            }
                            else
                            {
                                sql_ds.SelectParameters.Clear();
                                sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                                sql_ds.SelectParameters.Add("Rel_KelasDetPerwalian", QS.GetKelas());
                                sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                                sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
                                sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL_BY_KELAS_PERWALIAN;
                            }
                            if (isbind) lvData.DataBind();
                        }
                        else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && QS.GetMapel().Trim() != "")
                        {
                            if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT)
                            {
                                if (keyword.Trim() != "")
                                {
                                    sql_ds.SelectParameters.Clear();
                                    sql_ds.SelectParameters.Add("nama", keyword);
                                    sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                                    sql_ds.SelectParameters.Add("Semester", Libs.GetSemesterByTanggal(DateTime.Now).ToString());
                                    sql_ds.SelectParameters.Add("Rel_Mapel", QS.GetMapel());
                                    sql_ds.SelectParameters.Add("Rel_Kelas", QS.GetLevel());
                                    sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
                                    sql_ds.SelectCommand = DAO_FormasiGuruMapelDetSiswa.SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL_BY_KELAS_BY_KELAS_DET_FOR_SEARCH;
                                }
                                else
                                {
                                    sql_ds.SelectParameters.Clear();
                                    sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                                    sql_ds.SelectParameters.Add("Semester", Libs.GetSemesterByTanggal(DateTime.Now).ToString());
                                    sql_ds.SelectParameters.Add("Rel_Mapel", QS.GetMapel());
                                    sql_ds.SelectParameters.Add("Rel_Kelas", QS.GetLevel());
                                    sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
                                    sql_ds.SelectCommand = DAO_FormasiGuruMapelDetSiswa.SP_SELECT_SISWA_BY_TAHUN_AJARAN_BY_SEMESTER_BY_MAPEL_BY_KELAS_BY_KELAS_DET;
                                }
                                if (isbind) lvData.DataBind();
                            }
                            else
                            {
                                if (DAO_Siswa.GetByRombel_Entity(
                                        QS.GetUnit(),
                                        QS.GetKelas(),
                                        QS.GetTahunAjaran(),
                                        QS.GetSemester()
                                    ).Count > 0)
                                {
                                    if (keyword.Trim() != "")
                                    {
                                        sql_ds.SelectParameters.Clear();
                                        sql_ds.SelectParameters.Add("nama", keyword);
                                        sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                                        sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
                                        sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                                        sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
                                        sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL_FOR_SEARCH;
                                    }
                                    else
                                    {
                                        sql_ds.SelectParameters.Clear();
                                        sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                                        sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
                                        sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                                        sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
                                        sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL;
                                    }
                                    if (isbind) lvData.DataBind();
                                }
                                else
                                {
                                    if (keyword.Trim() != "")
                                    {
                                        sql_ds.SelectParameters.Clear();
                                        sql_ds.SelectParameters.Add("nama", keyword);
                                        sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                                        sql_ds.SelectParameters.Add("Rel_KelasDetPerwalian", QS.GetKelas());
                                        sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                                        sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
                                        sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL_BY_KELAS_PERWALIAN_FOR_SEARCH;
                                    }
                                    else
                                    {
                                        sql_ds.SelectParameters.Clear();
                                        sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                                        sql_ds.SelectParameters.Add("Rel_KelasDetPerwalian", QS.GetKelas());
                                        sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                                        sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
                                        sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL_BY_KELAS_PERWALIAN;
                                    }
                                    if (isbind) lvData.DataBind();
                                }
                            }
                        }
                        else
                        {
                            if (keyword.Trim() != "")
                            {
                                sql_ds.SelectParameters.Clear();
                                sql_ds.SelectParameters.Add("nama", keyword);
                                sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                                sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
                                sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                                sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
                                sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL_FOR_SEARCH;
                            }
                            else
                            {
                                sql_ds.SelectParameters.Clear();
                                sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                                sql_ds.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());
                                sql_ds.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
                                sql_ds.SelectParameters.Add("Semester", QS.GetSemester());
                                sql_ds.SelectCommand = DAO_Siswa.SP_SELECT_ALL;
                            }
                            if (isbind) lvData.DataBind();
                        }
                    }
                }
            }
        }

        private void BindListViewCatatan(bool isbind = true)
        {
            sql_ds_catatan.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_catatan.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            sql_ds_catatan.SelectParameters.Clear();
            sql_ds_catatan.SelectParameters.Add("TahunAjaran", QS.GetTahunAjaran());
            sql_ds_catatan.SelectParameters.Add("Rel_KelasDet", QS.GetKelas());

            div_catatan.Visible = false;
            switch (GetUnitSekolah())
            {
                case Libs.UnitSekolah.SALAH:
                    break;
                case Libs.UnitSekolah.KB:
                    div_catatan.Visible = true;
                    sql_ds_catatan.SelectCommand =
                        Application_DAOs.Elearning.KB.DAO_CatatanSiswa.SP_SELECT_DISTINCT_BY_TA_KELASDET;
                    if (isbind) lvCatatan.DataBind();
                    break;
                case Libs.UnitSekolah.TK:
                    div_catatan.Visible = true;
                    sql_ds_catatan.SelectCommand =
                        Application_DAOs.Elearning.TK.DAO_CatatanSiswa.SP_SELECT_DISTINCT_BY_TA_KELASDET;
                    if (isbind) lvCatatan.DataBind();
                    break;
                case Libs.UnitSekolah.SD:
                    div_catatan.Visible = true;
                    sql_ds_catatan.SelectCommand = 
                        Application_DAOs.Elearning.SD.DAO_CatatanSiswa.SP_SELECT_DISTINCT_BY_TA_KELASDET;
                    if (isbind) lvCatatan.DataBind();
                    break;
                case Libs.UnitSekolah.SMP:
                    div_catatan.Visible = true;
                    sql_ds_catatan.SelectCommand =
                        Application_DAOs.Elearning.SMP.DAO_CatatanSiswa.SP_SELECT_DISTINCT_BY_TA_KELASDET;
                    if (isbind) lvCatatan.DataBind();
                    break;
                case Libs.UnitSekolah.SMA:
                    div_catatan.Visible = true;
                    if (QS.GetMapel().Trim() != "")
                    {
                        sql_ds_catatan.SelectParameters.Add("Rel_Mapel", QS.GetMapel());
                        sql_ds_catatan.SelectCommand =
                        Application_DAOs.Elearning.SMA.DAO_CatatanSiswa.SP_SELECT_DISTINCT_BY_TA_KELASDET_BY_MAPEL;
                        if (isbind) lvCatatan.DataBind();
                    }
                    else
                    {
                        sql_ds_catatan.SelectCommand =
                        Application_DAOs.Elearning.SMA.DAO_CatatanSiswa.SP_SELECT_DISTINCT_BY_TA_KELASDET;
                        if (isbind) lvCatatan.DataBind();
                    }                    
                    break;
                default:
                    break;
            }           
        }

        protected Libs.UnitSekolah GetUnitSekolah()
        {
            Sekolah m_unit = DAO_Sekolah.GetByID_Entity(QS.GetUnit());
            if (m_unit != null)
            {
                if (m_unit.Nama != null)
                {
                    if (m_unit.UrutanJenjang == (int)Libs.UnitSekolah.KB)
                    {
                        return Libs.UnitSekolah.KB;
                    }
                    else if (m_unit.UrutanJenjang == (int)Libs.UnitSekolah.TK)
                    {
                        return Libs.UnitSekolah.TK;
                    }
                    else if (m_unit.UrutanJenjang == (int)Libs.UnitSekolah.SD)
                    {
                        return Libs.UnitSekolah.SD;
                    }
                    else if (m_unit.UrutanJenjang == (int)Libs.UnitSekolah.SMP)
                    {
                        return Libs.UnitSekolah.SMP;
                    }
                    else if (m_unit.UrutanJenjang == (int)Libs.UnitSekolah.SMA)
                    {
                        return Libs.UnitSekolah.SMA;
                    }
                }
            }

            return Libs.UnitSekolah.SALAH;
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                ResolveUrl(
                                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWA.ROUTE +
                                "?t=" + Libs.GetQueryString("t") +
                                "&ft=" + Libs.GetQueryString("ft") +
                                "&kd=" + Libs.GetQueryString("kd") +
                                (
                                    Libs.GetQueryString("m").Trim() != ""
                                    ? "&m=" + Libs.GetQueryString("m")
                                    : ""
                                )
                            )
            );
        }

        protected void InitFieldsCatatan()
        {
            cboKategoriCatatan.SelectedValue = "";
            cboSiswa.SelectedValue = "";
            txtTanggalCatatan.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
            txtCatatan.Text = "";
            txtCatatanVal.Value = "";
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            InitFieldsCatatan();
            txtKeyAction.Value = JenisAction.AddCatatanSiswa.ToString();
        }

        public static string GetKelasPerwalian(string kode)
        {
            string hasil = "";
            Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                QS.GetTahunAjaran(),
                QS.GetSemester(),
                kode
            );
            if (m_siswa != null)
            {
                if(m_siswa.Nama != null)
                {
                    if (m_siswa.Rel_KelasDet.Trim() != "")
                    {
                        KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet);
                        if (m_kelas_det != null)
                        {
                            if (m_kelas_det.Nama != null)
                            {
                                return m_kelas_det.Nama;
                            }
                        }
                    }
                }
            }
            return hasil;
        }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                switch (GetUnitSekolah())
                {
                    case Libs.UnitSekolah.SALAH:
                        break;
                    case Libs.UnitSekolah.KB:
                        Application_Entities.Elearning.KB.CatatanSiswa m_kb =
                            new Application_Entities.Elearning.KB.CatatanSiswa
                            {
                                Rel_Siswa = cboSiswa.SelectedValue,
                                Rel_Kategori = cboKategoriCatatan.SelectedValue,
                                Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text),
                                Catatan = txtCatatanVal.Value,
                                TahunAjaran = QS.GetTahunAjaran(),
                                Semester = Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text)).ToString(),
                                Rel_KelasDet = QS.GetKelas(),
                                Rel_Mapel = QS.GetMapel(),
                                Rel_Guru = Libs.LOGGED_USER_M.NoInduk.ToString()
                            };
                        m_kb.Kode = Guid.NewGuid();
                        if (txtIDCatatan.Value.Trim() != "")
                        {
                            m_kb.Kode = new Guid(txtIDCatatan.Value);
                            Application_DAOs.Elearning.KB.DAO_CatatanSiswa.Update(
                                    m_kb, Libs.LOGGED_USER_M.UserID
                                );
                        }
                        else
                        {
                            Application_DAOs.Elearning.KB.DAO_CatatanSiswa.Insert(
                                    m_kb, Libs.LOGGED_USER_M.UserID
                                );
                        }
                        BindListViewCatatan();
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                        break;
                    case Libs.UnitSekolah.TK:
                        Application_Entities.Elearning.TK.CatatanSiswa m_tk =
                            new Application_Entities.Elearning.TK.CatatanSiswa
                            {
                                Rel_Siswa = cboSiswa.SelectedValue,
                                Rel_Kategori = cboKategoriCatatan.SelectedValue,
                                Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text),
                                Catatan = txtCatatanVal.Value,
                                TahunAjaran = QS.GetTahunAjaran(),
                                Semester = Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text)).ToString(),
                                Rel_KelasDet = QS.GetKelas(),
                                Rel_Mapel = QS.GetMapel(),
                                Rel_Guru = Libs.LOGGED_USER_M.NoInduk.ToString()
                            };
                        m_tk.Kode = Guid.NewGuid();
                        if (txtIDCatatan.Value.Trim() != "")
                        {
                            m_tk.Kode = new Guid(txtIDCatatan.Value);
                            Application_DAOs.Elearning.TK.DAO_CatatanSiswa.Update(
                                    m_tk, Libs.LOGGED_USER_M.UserID
                                );
                        }
                        else
                        {
                            Application_DAOs.Elearning.TK.DAO_CatatanSiswa.Insert(
                                    m_tk, Libs.LOGGED_USER_M.UserID
                                );
                        }
                        BindListViewCatatan();
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                        break;
                    case Libs.UnitSekolah.SD:
                        Application_Entities.Elearning.SD.CatatanSiswa m_sd =
                            new Application_Entities.Elearning.SD.CatatanSiswa
                            {
                                Rel_Siswa = cboSiswa.SelectedValue,
                                Rel_Kategori = cboKategoriCatatan.SelectedValue,
                                Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text),
                                Catatan = txtCatatanVal.Value,
                                TahunAjaran = QS.GetTahunAjaran(),
                                Semester = Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text)).ToString(),
                                Rel_KelasDet = QS.GetKelas(),
                                Rel_Mapel = QS.GetMapel(),
                                Rel_Guru = Libs.LOGGED_USER_M.NoInduk.ToString()
                            };
                        m_sd.Kode = Guid.NewGuid();
                        if (txtIDCatatan.Value.Trim() != "")
                        {
                            m_sd.Kode = new Guid(txtIDCatatan.Value);
                            Application_DAOs.Elearning.SD.DAO_CatatanSiswa.Update(
                                    m_sd, Libs.LOGGED_USER_M.UserID
                                );
                        }
                        else
                        {
                            Application_DAOs.Elearning.SD.DAO_CatatanSiswa.Insert(
                                    m_sd, Libs.LOGGED_USER_M.UserID
                                );
                        }
                        BindListViewCatatan();
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                        break;
                    case Libs.UnitSekolah.SMP:
                        Application_Entities.Elearning.SMP.CatatanSiswa m_smp =
                            new Application_Entities.Elearning.SMP.CatatanSiswa
                            {
                                Rel_Siswa = cboSiswa.SelectedValue,
                                Rel_Kategori = cboKategoriCatatan.SelectedValue,
                                Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text),
                                Catatan = txtCatatanVal.Value,
                                TahunAjaran = QS.GetTahunAjaran(),
                                Semester = Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text)).ToString(),
                                Rel_KelasDet = QS.GetKelas(),
                                Rel_Mapel = QS.GetMapel(),
                                Rel_Guru = Libs.LOGGED_USER_M.NoInduk.ToString()
                            };
                        m_smp.Kode = Guid.NewGuid();
                        if (txtIDCatatan.Value.Trim() != "")
                        {
                            m_smp.Kode = new Guid(txtIDCatatan.Value);
                            Application_DAOs.Elearning.SMP.DAO_CatatanSiswa.Update(
                                    m_smp, Libs.LOGGED_USER_M.UserID
                                );
                        }
                        else
                        {
                            Application_DAOs.Elearning.SMP.DAO_CatatanSiswa.Insert(
                                    m_smp, Libs.LOGGED_USER_M.UserID
                                );
                        }
                        BindListViewCatatan();
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                        break;
                    case Libs.UnitSekolah.SMA:
                        Application_Entities.Elearning.SMA.CatatanSiswa m_sma =
                            new Application_Entities.Elearning.SMA.CatatanSiswa
                            {
                                Rel_Siswa = cboSiswa.SelectedValue,
                                Rel_Kategori = cboKategoriCatatan.SelectedValue,
                                Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text),
                                Catatan = txtCatatanVal.Value,
                                TahunAjaran = QS.GetTahunAjaran(),
                                Semester = Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text)).ToString(),
                                Rel_KelasDet = QS.GetKelas(),
                                Rel_Mapel = QS.GetMapel(),
                                Rel_Guru = Libs.LOGGED_USER_M.NoInduk.ToString()
                            };
                        m_sma.Kode = Guid.NewGuid();
                        if (txtIDCatatan.Value.Trim() != "")
                        {
                            m_sma.Kode = new Guid(txtIDCatatan.Value);
                            Application_DAOs.Elearning.SMA.DAO_CatatanSiswa.Update(
                                    m_sma, Libs.LOGGED_USER_M.UserID
                                );
                        }
                        else
                        {
                            Application_DAOs.Elearning.SMA.DAO_CatatanSiswa.Insert(
                                    m_sma, Libs.LOGGED_USER_M.UserID
                                );
                        }
                        BindListViewCatatan();
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }
    }
}