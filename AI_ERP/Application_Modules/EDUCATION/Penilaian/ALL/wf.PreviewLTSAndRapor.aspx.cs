using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.ALL
{
    public partial class wf_PreviewLTSAndRapor : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PREVIEW_LST_DAN_RAPOR";
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
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.ShowHeaderSubTitle = false;
            this.Master.ShowSubHeaderGuru = true;
            this.Master.SelectMenuGuru_Penilaian();
            InitURLOnMenu();

            BindListView(!IsPostBack);
            if (!IsPostBack)
            {
                InitURLRapor();
                this.Master.txtCariData.Text = Libs.GetQ();
            }
        }

        protected void InitURLRapor()
        {
            string url_lts = "";
            string url_rapor = "";
            string jenis_rapor = "0";

            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(QS.GetUnit());
            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {
                    string nama_kelas = DAO_Kelas.GetByID_Entity(
                            DAO_KelasDet.GetByID_Entity(QS.GetKelas()).Rel_Kelas.ToString()
                        ).Nama;

                    if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SD)
                    {
                        nama_kelas = nama_kelas.Replace("-", "") + "-";
                        string s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;

                        AI_ERP.Application_Entities.Elearning.SD.Rapor_Pengaturan m =
                            AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m0 => m0.TahunAjaran == QS.GetTahunAjaran() && m0.Semester == QS.GetSemester()).FirstOrDefault();

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

                        string s_url_ok = AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CETAK_LTS.ROUTE;
                        string s_url_rapor_ok = AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_PRINT.ROUTE;

                        url_rapor =
                            ResolveUrl( 
                                AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_PRINT.ROUTE +
                                "?" + AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=" +
                                      s_jenis_download_key +
                                "&t=" + RandomLibs.GetRndTahunAjaran(QS.GetTahunAjaran()) + "&" +
                                "s=" + QS.GetSemester() + "&" +
                                "kd=" + QS.GetKelas() + "&" +
                                "tr=" + TipeRapor.SEMESTER + "&" +
                                "sis=@sw;"
                            );


                        if ((Libs.GetStringToDouble(QS.GetTahunAjaran().Replace("/", "")) < 20202021))
                        {
                            url_lts =
                                ResolveUrl(
                                    s_url_ok
                                ) +
                                "?" +
                                "sis=@sw" +
                                "&t=" + RandomLibs.GetRndTahunAjaran(QS.GetTahunAjaran()) +
                                "&s=" + QS.GetSemester() +
                                "kd=" + QS.GetKelas();
                        }
                        else
                        {
                            url_lts =
                                ResolveUrl(
                                    s_url_rapor_ok
                                ) +
                                "?" +
                                "sis=@sw" +
                                "&" + AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=" +
                                      AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_LTS_SD +
                                "&t=" + RandomLibs.GetRndTahunAjaran(QS.GetTahunAjaran()) + 
                                "&s=" + QS.GetSemester() +
                                "&kd=" + QS.GetKelas();
                        }
                    }
                    else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMP)
                    {
                        //get jenis download kurikulum
                        string s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                        AI_ERP.Application_Entities.Elearning.SMP.Rapor_Pengaturan m =
                            AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m0 => m0.TahunAjaran == QS.GetTahunAjaran() && m0.Semester == QS.GetSemester()).FirstOrDefault();
                        if (m != null)
                        {
                            if (nama_kelas.Length >= 4)
                            {
                                if (nama_kelas.Substring(0, 4) == "VII-" && m.KurikulumRaporLevel7 == Libs.JenisKurikulum.SMP.KURTILAS)
                                {
                                    s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                                }
                                else if (nama_kelas.Substring(0, 4) == "VII-" && m.KurikulumRaporLevel7 == Libs.JenisKurikulum.SMP.KTSP)
                                {
                                    s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP;
                                }
                            }
                            if (nama_kelas.Length >= 5)
                            {
                                if (nama_kelas.Substring(0, 5) == "VIII-" && m.KurikulumRaporLevel8 == Libs.JenisKurikulum.SMP.KURTILAS)
                                {
                                    s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                                }
                                else if (nama_kelas.Substring(0, 5) == "VIII-" && m.KurikulumRaporLevel8 == Libs.JenisKurikulum.SMP.KTSP)
                                {
                                    s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP;
                                }
                            }
                            if (nama_kelas.Length >= 3)
                            {
                                if (nama_kelas.Substring(0, 3) == "IX-" && m.KurikulumRaporLevel9 == Libs.JenisKurikulum.SMP.KURTILAS)
                                {
                                    s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                                }
                                else if (nama_kelas.Substring(0, 3) == "IX-" && m.KurikulumRaporLevel9 == Libs.JenisKurikulum.SMP.KTSP)
                                {
                                    s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP;
                                }
                            }
                        }

                        string s_url_ok = AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CETAK_LTS.ROUTE;

                        url_rapor = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_PRINT.ROUTE) + 
                                    "?" +
                                    AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=" + s_jenis_download_key + 
                                    "&t=" + QS.GetTahunAjaran() + 
                                    "&kd=" + QS.GetKelas() + 
                                    "&hal=1" + 
                                    "&s=" + QS.GetSemester() +
                                    "&sw=@sw;";

                        if ((Libs.GetStringToDouble(QS.GetTahunAjaran().Replace("/", "")) < 20202021))
                        {
                            url_lts = 
                                ResolveUrl(
                                        s_url_ok
                                    ) +
                                "?" +
                                "sis=@sw" +
                                "&t=" + RandomLibs.GetRndTahunAjaran(QS.GetTahunAjaran()) + 
                                "&s=" + QS.GetSemester() +
                                "&kd=" + QS.GetKelas();
                        }
                        else
                        {
                            url_lts =
                                ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_PRINT.ROUTE) +
                                "?" +
                                "sw=@sw" +
                                "&" + AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=" +
                                      AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_LTS_SMP + 
                                "&t=" + RandomLibs.GetRndTahunAjaran(QS.GetTahunAjaran()) + 
                                "&s=" + QS.GetSemester() +
                                "&kd=" + QS.GetKelas();
                        }
                    }
                    else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA)
                    {
                        if (QS.GetTahunAjaran() == "2020/2021")
                        {
                            url_lts =
                                    ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.ROUTE) +
                                    "?" +
                                    "j=" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_LTS_SMA +
                                    "&t=" + QS.GetTahunAjaran().Replace("/", "-") +
                                    "&kd=" + QS.GetKelas() +
                                    "&s=" + QS.GetSemester() +
                                    "&sw=@sw";

                            url_rapor =
                                    ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.ROUTE) +
                                    "?" +
                                    "j=" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_SEMESTER_SMA +
                                    "&t=" + QS.GetTahunAjaran().Replace("/", "-") +
                                    "&kd=" + QS.GetKelas() +
                                    "&hal=1" +
                                    "&s=" + QS.GetSemester() +
                                    "&sw=@sw";
                        }
                        else
                        {
                            url_lts =
                                    ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.ROUTE) +
                                    "?" +
                                    "j=" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_LTS_SMA +
                                    "&t=" + QS.GetTahunAjaran().Replace("/", "-") +
                                    "&kd=" + QS.GetKelas() +
                                    "&s=" + QS.GetSemester() +
                                    "&sw=@sw";

                            url_rapor =
                                    ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.ROUTE) + 
                                    "?" +
                                    "j=" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_SEMESTER_SMA +
                                    "&t=" + QS.GetTahunAjaran().Replace("/", "-") +
                                    "&kd=" + QS.GetKelas() +
                                    "&hal=1" +
                                    "&s=" + QS.GetSemester() +
                                    "&sw=@sw";
                        }
                    }
                }
            }

            txtURLRapor.Value = url_rapor;
            txtURLLTS.Value = url_lts;
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

            txtURLNilai.Value =
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
                        );

            this.Master.SetURLGuru_Penilaian(
                    txtURLNilai.Value
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
                    }
                    else
                    {
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
        
        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
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
                if (m_siswa.Nama != null)
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

        protected void lnkOKNilaiAkademik_Click(object sender, EventArgs e)
        {
            Response.Redirect(txtURLNilai.Value);
        }
    }
}