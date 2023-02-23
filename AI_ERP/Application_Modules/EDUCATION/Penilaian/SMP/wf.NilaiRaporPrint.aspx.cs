using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Web.Services;

using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_Entities.Elearning.SMP;
using AI_ERP.Application_DAOs.Elearning.SMP;
using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning.SMP.Reports;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP
{
    public partial class wf_NilaiRaporPrint : System.Web.UI.Page
    {
        public static string STATUS_PROSES = "";
        bool ada_proses = false;

        static BackgroundWorker _bw;

        static string tahun_ajaran = "";
        static string semester = "";
        static string kelas = "";
        static string siswa = "";
        static string halaman = "";
        static string tipe_rapor = "";

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

            public static string GetLevel()
            {
                return Libs.GetQueryString("k");
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

            public static string GetMapelSikap()
            {
                string ms = Libs.GetQueryString("ms");
                return ms;
            }

            public static string GetGuru()
            {
                string guru = Libs.GetQueryString("g");
                if (guru.Trim() == "") return Libs.LOGGED_USER_M.NoInduk;
                return guru;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.BUAT_FILE_RAPOR)
                {
                    CRV1.AutoDataBind = false;
                    CRV1.Enabled = false;
                    tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
                    semester = Libs.GetQueryString("s");
                    kelas = Libs.GetQueryString("kd");
                    siswa = Libs.GetQueryString("sis");
                    halaman = Libs.GetQueryString("hal");
                    tipe_rapor = Libs.GetQueryString("tr");

                    _bw = new BackgroundWorker
                    {
                        WorkerReportsProgress = true,
                        WorkerSupportsCancellation = true
                    };
                    _bw.DoWork += bw_DoWork;
                    _bw.ProgressChanged += bw_ProgressChanged;
                    _bw.RunWorkerCompleted += bw_RunWorkerCompleted;

                    _bw.RunWorkerAsync();
                }
                else
                {
                    CRV1.AutoDataBind = true;
                    CRV1.Enabled = true;
                    CreateReport();
                }   
            }
        }

        protected string GetFileTTDGuru(string s_tahun_ajaran, string s_semester, string s_kelas)
        {
            string lokasi_ttd = Server.MapPath("~/Application_Files/Rapor/__TTD/" + s_tahun_ajaran.Replace("/", "-") + "-0" + s_semester + "/" + s_kelas + ".jpg");
            return lokasi_ttd;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            STATUS_PROSES = "Sedang proses...";
            if (_bw.CancellationPending) { e.Cancel = true; return; }
            _bw.ReportProgress(0);
            ada_proses = false;

            string[] arr_siswa = siswa.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            Rapor_Pengaturan m = DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m0 => m0.TahunAjaran == tahun_ajaran && m0.Semester == semester).FirstOrDefault();
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(kelas);
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            string s_lokasi_ttd = GetFileTTDGuru(tahun_ajaran, semester, m_kelas_det.Nama.Trim());
                            Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                            string nama_kelas = m_kelas.Nama.Replace("-", "") + "-";
                            string jenis_download_0 = "";
                            
                            if (nama_kelas.Length >= 2)
                            {
                                if (nama_kelas.Length >= 4)
                                {
                                    if (nama_kelas.Substring(0, 4) == "VII-" && m.KurikulumRaporLevel7 == Libs.JenisKurikulum.SMP.KTSP)
                                    {
                                        jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP;
                                    }
                                    else if (nama_kelas.Substring(0, 4) == "VII-" && m.KurikulumRaporLevel7 == Libs.JenisKurikulum.SMP.KURTILAS)
                                    {
                                        jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                                    }
                                }
                                if (nama_kelas.Length >= 5)
                                {
                                    if (nama_kelas.Substring(0, 5) == "VIII-" && m.KurikulumRaporLevel8 == Libs.JenisKurikulum.SMP.KTSP)
                                    {
                                        jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP;
                                    }
                                    else if (nama_kelas.Substring(0, 5) == "VIII-" && m.KurikulumRaporLevel8 == Libs.JenisKurikulum.SMP.KURTILAS)
                                    {
                                        jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                                    }
                                }
                                if (nama_kelas.Length >= 3)
                                {
                                    if (nama_kelas.Substring(0, 3) == "IX-" && m.KurikulumRaporLevel9 == Libs.JenisKurikulum.SMP.KTSP)
                                    {
                                        jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP;
                                    }
                                    else if (nama_kelas.Substring(0, 3) == "IX-" && m.KurikulumRaporLevel9 == Libs.JenisKurikulum.SMP.KURTILAS)
                                    {
                                        jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                                    }
                                }
                            }
                            
                            int idx = 0;
                            foreach (var item_siswa in arr_siswa)
                            {
                                idx++;
                                ada_proses = true;
                                STATUS_PROSES = "Proses data : " + idx.ToString() + " dari " + arr_siswa.Count().ToString();
                                if (_bw.CancellationPending) { e.Cancel = true; return; }
                                _bw.ReportProgress(idx);

                                if (tipe_rapor.Trim().ToUpper() == TipeRapor.SEMESTER.ToUpper().Trim())
                                {
                                    if (jenis_download_0 == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP)
                                    {
                                        NilaiRapor_KTSP(this.Response, tahun_ajaran, semester, kelas, halaman, item_siswa + ";", s_lokasi_ttd, true);
                                    }
                                    else if (jenis_download_0 == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP)
                                    {
                                        NilaiRapor_KURTILAS(this.Response, tahun_ajaran, semester, kelas, halaman, item_siswa + ";", s_lokasi_ttd, true);
                                    }
                                }
                                else if (tipe_rapor.Trim().ToUpper() == TipeRapor.LTS.ToUpper().Trim())
                                {
                                    NilaiRapor_LTS(this.Response, tahun_ajaran, semester, kelas, item_siswa, s_lokasi_ttd, true);
                                }
                            }
                        }
                    }
                }
            }

            e.Result = 123;
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ada_proses)
            {
                STATUS_PROSES = "Selesai";
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //ProgressStatus = e.ProgressPercentage;
        }

        [WebMethod]
        public static string GetData()
        {
            return STATUS_PROSES;
        }

        protected void CreateReport()
        {
            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(QS.GetKelas());
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    string s_lokasi_ttd = "";
                    s_lokasi_ttd = GetFileTTDGuru(QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), m_kelas_det.Nama.Trim());
                    if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP)
                    {
                        NilaiRapor_KTSP(this.Response, QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("hal"), Libs.GetQueryString("sw"), s_lokasi_ttd);
                    }
                    else if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP)
                    {
                        NilaiRapor_KURTILAS(this.Response, QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("hal"), Libs.GetQueryString("sw"), s_lokasi_ttd);
                    }
                    else if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_LTS_SMP)
                    {
                        s_lokasi_ttd = GetFileTTDGuru(QS.GetTahunAjaran(), QS.GetSemester(), m_kelas_det.Nama.Trim());
                        NilaiRapor_LTS(this.Response, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("sw"), s_lokasi_ttd);
                    }
                }
            }
        }

        public static string GetUnit()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMP).FirstOrDefault().Kode.ToString();
        }

        protected void NilaiRapor_LTS(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa = "", string s_lokasi_ttd = "", bool export_to_pdf = false)
        {
            if (export_to_pdf)
            {
                string kelas_det = "";
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        kelas_det = m_kelas_det.Nama;
                    }
                }

                List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(GetUnit(), rel_kelas_det, tahun_ajaran, semester);
                string[] arr_siswa = rel_siswa.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item_siswa in arr_siswa)
                {
                    string s_nama = "";
                    Siswa m_siswa = lst_siswa.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item_siswa.Replace(";", "").ToUpper().Trim()).FirstOrDefault();
                    if (m_siswa != null)
                    {
                        if (m_siswa.Nama != null) s_nama = m_siswa.Nama.ToUpper().Trim();
                    }
                    string lokasi_ekspor = Libs.GetLokasiFolderFileLTS(
                        item_siswa, tahun_ajaran, semester, rel_kelas_det, Libs.UnitSekolah.SMP
                    );

                    Reports_SMP.LTS lts = new Reports_SMP.LTS(tahun_ajaran, semester, rel_kelas_det, rel_siswa, s_lokasi_ttd);

                    List<RaporLTS> lst_nilai_rapor_lts = new List<RaporLTS>();
                    lst_nilai_rapor_lts = lts.GetRaporLTS;
                    DataTable dtRaporLTS = Libs.ToDataTable(lst_nilai_rapor_lts);

                    List<RaporLTSDeskripsi> lst_nilai_rapor_lts_deskripsi = new List<RaporLTSDeskripsi>();
                    lst_nilai_rapor_lts_deskripsi = lts.GetRaporLTSDeskripsi;
                    DataTable dtRaporLTSDeskripsi = Libs.ToDataTable(lst_nilai_rapor_lts_deskripsi);

                    List<RaporLTSCapaianKedisiplinan> lst_nilai_rapor_lts_capaiankedisiplinan = new List<RaporLTSCapaianKedisiplinan>();
                    lst_nilai_rapor_lts_capaiankedisiplinan = lts.GetRaporLTSCapaianKedisiplinan;
                    DataTable dtRaporLTSCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_lts_capaiankedisiplinan);

                    List<KTSP_RaporEkskul> lst_nilai_deskripsi_ekskul = DAO_Rapor_Semester.GetNilaiEkskul_Entity(tahun_ajaran, semester, rel_kelas_det, rel_siswa.Replace(";", "") + ";");
                    List <RaporLTSEkskul> lst_nilai_ekskul = new List<RaporLTSEkskul>();
                    lst_nilai_ekskul.Clear();
                    foreach (var item in lst_nilai_deskripsi_ekskul)
                    {
                        lst_nilai_ekskul.Add(new RaporLTSEkskul
                        {
                            Rel_Siswa = item.Rel_Siswa,
                            Kegiatan = item.Kegiatan,
                            LTS_CK_KEHADIRAN = item.LTS_CK_KEHADIRAN,
                            Urutan = 0
                        });
                    }
                    DataTable dtRaporLTSEkskul = Libs.ToDataTable(lst_nilai_ekskul);

                    ReportDocument rpt_doc = new ReportDocument();
                    rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());

                    rpt_doc.Load(
                            Server.MapPath(
                                "~/Application_Reports/Penilaian/SMP/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "LTS.rpt")
                            )
                        );
                    rpt_doc.SetDataSource(dtRaporLTS);
                    if (dtRaporLTS.Rows.Count > 0)
                    {
                        rpt_doc.OpenSubreport("KeteranganTagihan").SetDataSource(dtRaporLTSDeskripsi);
                        rpt_doc.OpenSubreport("CapaianKedisiplinan").SetDataSource(dtRaporLTSCapaianKedisiplinan);
                        rpt_doc.OpenSubreport("Ekstrakurikuler").SetDataSource(dtRaporLTSEkskul);
                    }

                    string file_name = "SMP " +
                                       "LTS " +
                                       semester +
                                       " " +
                                       tahun_ajaran +
                                       " " +
                                       s_nama;
                    ExportToPDF(rpt_doc, lokasi_ekspor, file_name.Replace("/", "-"));
                }   
            }
            else
            {
                string kelas_det = "";
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        kelas_det = m_kelas_det.Nama;
                    }
                }

                Reports_SMP.LTS lts = new Reports_SMP.LTS(tahun_ajaran, semester, rel_kelas_det, rel_siswa, s_lokasi_ttd);

                List<RaporLTS> lst_nilai_rapor_lts = new List<RaporLTS>();
                lst_nilai_rapor_lts = lts.GetRaporLTS;
                DataTable dtRaporLTS = Libs.ToDataTable(lst_nilai_rapor_lts);

                List<RaporLTSDeskripsi> lst_nilai_rapor_lts_deskripsi = new List<RaporLTSDeskripsi>();
                lst_nilai_rapor_lts_deskripsi = lts.GetRaporLTSDeskripsi;
                DataTable dtRaporLTSDeskripsi = Libs.ToDataTable(lst_nilai_rapor_lts_deskripsi);

                List<RaporLTSCapaianKedisiplinan> lst_nilai_rapor_lts_capaiankedisiplinan = new List<RaporLTSCapaianKedisiplinan>();
                lst_nilai_rapor_lts_capaiankedisiplinan = lts.GetRaporLTSCapaianKedisiplinan;
                DataTable dtRaporLTSCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_lts_capaiankedisiplinan);

                List<KTSP_RaporEkskul> lst_nilai_deskripsi_ekskul = DAO_Rapor_Semester.GetNilaiEkskul_Entity(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
                List<RaporLTSEkskul> lst_nilai_ekskul = new List<RaporLTSEkskul>();
                lst_nilai_ekskul.Clear();
                foreach (var item in lst_nilai_deskripsi_ekskul)
                {
                    lst_nilai_ekskul.Add(new RaporLTSEkskul
                    {
                        Rel_Siswa = item.Rel_Siswa,
                        Kegiatan = item.Kegiatan,
                        LTS_CK_KEHADIRAN = item.LTS_CK_KEHADIRAN,
                        Urutan = 0
                    });
                }
                DataTable dtRaporLTSEkskul = Libs.ToDataTable(lst_nilai_ekskul);

                ReportDocument rpt_doc = new ReportDocument();
                rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());

                rpt_doc.Load(
                        Server.MapPath(
                            "~/Application_Reports/Penilaian/SMP/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "LTS.rpt")
                        )
                    );
                rpt_doc.SetDataSource(dtRaporLTS);
                if (dtRaporLTS.Rows.Count > 0)
                {
                    rpt_doc.OpenSubreport("KeteranganTagihan").SetDataSource(dtRaporLTSDeskripsi);
                    rpt_doc.OpenSubreport("CapaianKedisiplinan").SetDataSource(dtRaporLTSCapaianKedisiplinan);
                    rpt_doc.OpenSubreport("Ekstrakurikuler").SetDataSource(dtRaporLTSEkskul);
                }

                CRV1.Page.Title = "RAPOR LTS " +
                                  tahun_ajaran +
                                  " SEMESTER " +
                                  semester;
                CRV1.ReportSource = rpt_doc;

                rpt_doc.ExportToHttpResponse(
                        ExportFormatType.PortableDocFormat, Response, false,
                        "RAPOR LTS SMP " + tahun_ajaran.Replace("/", "-") + " SM " + semester + " KELAS " + kelas_det.ToUpper().Replace("/", "-")
                    );
            }   
        }

        protected void ExportToPDF(ReportDocument rpt_doc, string lokasi_ekspor, string file_name)
        {
            ExportOptions CrExportOptions;
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            CrDiskFileDestinationOptions.DiskFileName = Server.MapPath(lokasi_ekspor) + "/" + file_name + ".pdf";
            if (File.Exists(CrDiskFileDestinationOptions.DiskFileName)) File.Delete(CrDiskFileDestinationOptions.DiskFileName);
            if (!Directory.Exists(Server.MapPath(lokasi_ekspor))) Directory.CreateDirectory(Server.MapPath(lokasi_ekspor));
            CrExportOptions = rpt_doc.ExportOptions;
            {
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
            }
            rpt_doc.Export();
        }

        protected void NilaiRapor_KTSP(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string halaman, string rel_siswa, string s_lokasi_ttd, bool export_to_pdf = false)
        {
            halaman = Libs.GetStringToDecimal(halaman).ToString();
            string kelas_det = "";
            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    kelas_det = m_kelas_det.Nama;
                }
            }

            List<KTSP_RaporCatatan> lst_nilai_rapor_catatan = DAO_Rapor_Semester.GetCatatan(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiCatatan = Libs.ToDataTable(lst_nilai_rapor_catatan);

            List<KTSP_RaporNilai> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KTSP(tahun_ajaran, semester, rel_kelas_det, halaman, rel_siswa, s_lokasi_ttd);
            DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

            List<KTSP_DeskripsiRapor> lst_nilai_deskripsi_rapor = DAO_Rapor_Semester.GetDeskripsiRapor_KTSP(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiDeskripsi = Libs.ToDataTable(lst_nilai_deskripsi_rapor);

            List<KTSP_RaporEkskul> lst_nilai_deskripsi_ekskul = DAO_Rapor_Semester.GetNilaiEkskul_Entity(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiEkskul = Libs.ToDataTable(lst_nilai_deskripsi_ekskul);

            List<KTSP_RaporKetidakhadiran> lst_nilai_kepribadian = DAO_Rapor_Semester.GetNilaiKetidakhadiran_Entity(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiKetidakhadiran = Libs.ToDataTable(lst_nilai_kepribadian);

            List<KTSP_RaporVolunteer> lst_nilai_volunteer = DAO_Rapor_Semester.GetVolunteer(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiVolunteer = Libs.ToDataTable(lst_nilai_volunteer);

            ReportDocument rpt_doc = new ReportDocument();
            rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());
            rpt_doc.Load(
                    Server.MapPath(
                        (
                            kelas_det.Substring(0, 2) == "IX" || kelas_det.Substring(0, 1) == "9"
                            ? "~/Application_Reports/Penilaian/SMP/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP_IX.rpt")
                            : "~/Application_Reports/Penilaian/SMP/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP.rpt")
                        )                        
                    )
                );            
            rpt_doc.SetDataSource(dtNilai);
            if (dtNilai.Rows.Count > 0)
            {
                rpt_doc.OpenSubreport("RaporDeskripsi").SetDataSource(dtNilaiDeskripsi);
                rpt_doc.OpenSubreport("RaporEkskul").SetDataSource(dtNilaiEkskul);
                rpt_doc.OpenSubreport("RaporKetidakhadiran").SetDataSource(dtNilaiKetidakhadiran);
                rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtNilaiVolunteer);
                rpt_doc.OpenSubreport("RaporCatatanWalas").SetDataSource(dtNilaiCatatan);
            }

            if (export_to_pdf)
            {
                List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(GetUnit(), rel_kelas_det, tahun_ajaran, semester);
                string s_nama = "";
                string[] arr_siswa = (rel_siswa + ";").Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item_siswa in arr_siswa)
                {
                    Siswa m_siswa = lst_siswa.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item_siswa.Replace(";", "").ToUpper().Trim()).FirstOrDefault();
                    if (m_siswa != null)
                    {
                        if (m_siswa.Nama != null) s_nama = m_siswa.Nama.ToUpper().Trim();
                    }
                }

                string lokasi_ekspor = Libs.GetLokasiFolderFileRapor(
                        rel_siswa.Replace(";", ""), tahun_ajaran, semester, rel_kelas_det, Libs.UnitSekolah.SMP
                    );

                string file_name = "SMP " +
                                   "RAPOR " +
                                   semester +
                                   " " +
                                   tahun_ajaran +
                                   " " +
                                   s_nama;
                ExportToPDF(rpt_doc, lokasi_ekspor, file_name.Replace("/", "-"));
            }
            else
            {
                CRV1.Page.Title = "RAPOR SMP " +
                                  tahun_ajaran +
                                  " SEMESTER " +
                                  semester;
                CRV1.ReportSource = rpt_doc;
                rpt_doc.ExportToHttpResponse(
                        ExportFormatType.PortableDocFormat, Response, false,
                        "RAPOR SMP " + tahun_ajaran.Replace("/", "-") + " SM " + semester + " KELAS " + kelas_det.ToUpper().Replace("/", "-")
                    );
            }            
        }

        protected void NilaiRapor_KURTILAS(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string halaman, string rel_siswa, string s_lokasi_ttd, bool export_to_pdf = false)
        {
            List<RaporLTSCapaianKedisiplinan> lst_nilai_rapor_lts_capaiankedisiplinan = new List<RaporLTSCapaianKedisiplinan>();

            halaman = Libs.GetStringToDecimal(halaman).ToString();
            string kelas_det = "";
            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    kelas_det = m_kelas_det.Nama;
                }
            }

            List<KTSP_RaporCatatan> lst_nilai_rapor_catatan = DAO_Rapor_Semester.GetCatatan(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiCatatan = Libs.ToDataTable(lst_nilai_rapor_catatan);

            List<KTSP_RaporKetidakhadiran> lst_nilai_kepribadian = DAO_Rapor_Semester.GetNilaiKetidakhadiran_Entity(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiKetidakhadiran = Libs.ToDataTable(lst_nilai_kepribadian);

            List<KURTILAS_RaporNilai> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KURTILAS(tahun_ajaran, semester, rel_kelas_det, ref lst_nilai_rapor_lts_capaiankedisiplinan, halaman, rel_siswa, s_lokasi_ttd);
            DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

            List<KTSP_RaporEkskul> lst_nilai_deskripsi_ekskul = DAO_Rapor_Semester.GetNilaiEkskul_Entity(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiEkskul = Libs.ToDataTable(lst_nilai_deskripsi_ekskul);

            List<KTSP_RaporVolunteer> lst_nilai_volunteer = DAO_Rapor_Semester.GetVolunteer(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiVolunteer = Libs.ToDataTable(lst_nilai_volunteer);

            DataTable dtRaporLTSCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_lts_capaiankedisiplinan);

            ReportDocument rpt_doc = new ReportDocument();
            rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());
            rpt_doc.Load(
                    Server.MapPath(
                        "~/Application_Reports/Penilaian/SMP/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS.rpt")
                    )
                );            
            rpt_doc.SetDataSource(dtNilai);
            if (dtNilai.Rows.Count > 0)
            {
                rpt_doc.OpenSubreport("RaporEkstrakurikuler").SetDataSource(dtNilaiEkskul);
                rpt_doc.OpenSubreport("RaporKetidakhadiran").SetDataSource(dtNilaiKetidakhadiran);
                rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtNilaiVolunteer);
                rpt_doc.OpenSubreport("RaporCatatanWalas").SetDataSource(dtNilaiCatatan);
                if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2020)
                {
                    rpt_doc.OpenSubreport("CapaianKedisiplinan").SetDataSource(dtRaporLTSCapaianKedisiplinan);
                }
            }

            if (export_to_pdf)
            {
                List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(GetUnit(), rel_kelas_det, tahun_ajaran, semester);
                string s_nama = "";
                string[] arr_siswa = (rel_siswa + ";").Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item_siswa in arr_siswa)
                {
                    Siswa m_siswa = lst_siswa.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item_siswa.Replace(";", "").ToUpper().Trim()).FirstOrDefault();
                    if (m_siswa != null)
                    {
                        if (m_siswa.Nama != null) s_nama = m_siswa.Nama.ToUpper().Trim();
                    }
                }

                string lokasi_ekspor = Libs.GetLokasiFolderFileRapor(
                        rel_siswa.Replace(";", ""), tahun_ajaran, semester, rel_kelas_det, Libs.UnitSekolah.SMP
                    );

                string file_name = "SMP " +
                                   "RAPOR " +
                                   semester +
                                   " " +
                                   tahun_ajaran +
                                   " " +
                                   s_nama;
                ExportToPDF(rpt_doc, lokasi_ekspor, file_name.Replace("/", "-"));

                rpt_doc.Close();
                rpt_doc.Dispose();
            }
            else
            {
                CRV1.Page.Title = "RAPOR SMP " +
                              tahun_ajaran +
                              " SEMESTER " +
                              semester;
                CRV1.ReportSource = rpt_doc;
                rpt_doc.ExportToHttpResponse(
                        ExportFormatType.PortableDocFormat, Response, false,
                        "RAPOR SMP " + tahun_ajaran.Replace("/", "-") + " SM " + semester + " KELAS " + kelas_det.ToUpper().Replace("/", "-")
                    );

                rpt_doc.Close();
                rpt_doc.Dispose();
            }            
        }
    }
}