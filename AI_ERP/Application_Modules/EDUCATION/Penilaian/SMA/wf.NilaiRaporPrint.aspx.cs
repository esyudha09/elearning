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
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning.SMA.Reports;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA
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

        public static string GetUnit()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMA).FirstOrDefault().Kode.ToString();
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

        protected void CreateReport()
        {
            PengaturanSMA m_pengaturan_ = DAO_PengaturanSMA.GetAll_Entity().FirstOrDefault();
            if (m_pengaturan_ != null)
            {
                if (m_pengaturan_.TeksLinkLTS != null)
                {
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(
                                QS.GetKelas()
                            );
                    string s_lokasi_ttd = "";
                    s_lokasi_ttd = GetFileTTDGuru(QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), m_kelas_det.Nama);

                    if (Libs.GetQueryString("idm").Trim() != "")
                    {
                        MailJobs m = DAO_MailJobs.GetDataByID_Entity(Libs.GetQueryString("idm").Trim());                        
                        if (m != null)
                        {
                            if (m.Subjek != null)
                            {
                                if (m.LinkExpiredDate != DateTime.MinValue)
                                {
                                    if (DateTime.Now >= m.LinkExpiredDate)
                                    {
                                        Response.Write(
                                                Libs.GetHTMLEmailTemplate(m_pengaturan_.TemplateHTMLLinkExpired, GetUnit(), true)
                                            );
                                    }
                                    else
                                    {
                                        if (m_pengaturan_.JenisFileRapor == Constantas.JENIS_FILE_PDF)
                                        {
                                            if (Libs.GetQueryString("j") == Downloads.JenisDownload.RAPOR_LTS_SMA)
                                            {
                                                NilaiRapor_LTS(this.Response, QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("hal"), Libs.GetQueryString("sw"));
                                            }
                                            else if (Libs.GetQueryString("j") == Downloads.JenisDownload.RAPOR_SEMESTER_SMA)
                                            {
                                                NilaiRapor_Semester(this.Response, QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("hal"), Libs.GetQueryString("sw"), false, s_lokasi_ttd);
                                            }
                                        }
                                        else if (m_pengaturan_.JenisFileRapor == Constantas.JENIS_FILE_HTML)
                                        {
                                            Reports_SMA.LTS lts = new Reports_SMA.LTS(QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("sw"), s_lokasi_ttd);
                                            Response.Write(lts.GetHTML);
                                        }
                                    }
                                }
                                else
                                {
                                    if (m_pengaturan_.JenisFileRapor == Constantas.JENIS_FILE_PDF)
                                    {
                                        if (Libs.GetQueryString("j") == Downloads.JenisDownload.RAPOR_LTS_SMA)
                                        {
                                            NilaiRapor_LTS(this.Response, QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("hal"), Libs.GetQueryString("sw"));
                                        }
                                        else if (Libs.GetQueryString("j") == Downloads.JenisDownload.RAPOR_SEMESTER_SMA)
                                        {
                                            NilaiRapor_Semester(this.Response, QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("hal"), Libs.GetQueryString("sw"), false, s_lokasi_ttd);
                                        }
                                    }
                                    else if (m_pengaturan_.JenisFileRapor == Constantas.JENIS_FILE_HTML)
                                    {
                                        Reports_SMA.LTS lts = new Reports_SMA.LTS(QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("sw"), s_lokasi_ttd);
                                        Response.Write(lts.GetHTML);
                                    }
                                }
                            }
                            else
                            {
                                if (m_pengaturan_.JenisFileRapor == Constantas.JENIS_FILE_PDF)
                                {
                                    if (Libs.GetQueryString("j") == Downloads.JenisDownload.RAPOR_LTS_SMA)
                                    {
                                        NilaiRapor_LTS(this.Response, QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("hal"), Libs.GetQueryString("sw"));
                                    }
                                    else if (Libs.GetQueryString("j") == Downloads.JenisDownload.RAPOR_SEMESTER_SMA)
                                    {
                                        NilaiRapor_Semester(this.Response, QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("hal"), Libs.GetQueryString("sw"), false, s_lokasi_ttd);
                                    }
                                }
                                else if (m_pengaturan_.JenisFileRapor == Constantas.JENIS_FILE_HTML)
                                {
                                    Reports_SMA.LTS lts = new Reports_SMA.LTS(QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("sw"), s_lokasi_ttd);
                                    Response.Write(lts.GetHTML);
                                }
                            }
                        }
                        else
                        {
                            if (m_pengaturan_.JenisFileRapor == Constantas.JENIS_FILE_PDF)
                            {
                                if (Libs.GetQueryString("j") == Downloads.JenisDownload.RAPOR_LTS_SMA)
                                {
                                    NilaiRapor_LTS(this.Response, QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("hal"), Libs.GetQueryString("sw"));
                                }
                                else if (Libs.GetQueryString("j") == Downloads.JenisDownload.RAPOR_SEMESTER_SMA)
                                {
                                    NilaiRapor_Semester(this.Response, QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("hal"), Libs.GetQueryString("sw"), false, s_lokasi_ttd);
                                }
                            }
                            else if (m_pengaturan_.JenisFileRapor == Constantas.JENIS_FILE_HTML)
                            {
                                Reports_SMA.LTS lts = new Reports_SMA.LTS(QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("sw"), s_lokasi_ttd);
                                Response.Write(lts.GetHTML);
                            }
                        }
                    }
                    else
                    {
                        if (m_pengaturan_.JenisFileRapor == Constantas.JENIS_FILE_PDF)
                        {
                            if (Libs.GetQueryString("j") == Downloads.JenisDownload.RAPOR_LTS_SMA)
                            {
                                NilaiRapor_LTS(this.Response, QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("hal"), Libs.GetQueryString("sw"));
                            }
                            else if (Libs.GetQueryString("j") == Downloads.JenisDownload.RAPOR_SEMESTER_SMA)
                            {
                                NilaiRapor_Semester(this.Response, QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("hal"), Libs.GetQueryString("sw"), false, s_lokasi_ttd);
                            }
                        }
                        else if (m_pengaturan_.JenisFileRapor == Constantas.JENIS_FILE_HTML)
                        {
                            Reports_SMA.LTS lts = new Reports_SMA.LTS(QS.GetTahunAjaranPure().Replace("-", "/"), QS.GetSemester(), QS.GetKelas(), Libs.GetQueryString("sw"), s_lokasi_ttd);
                            Response.Write(lts.GetHTML);
                        }
                    }

                }
            }
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
                            Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                            string nama_kelas = m_kelas.Nama.Replace("-", "") + "-";
                            string jenis_download_0 = "";

                            if (nama_kelas.Length >= 2)
                            {
                                if (nama_kelas.Length == 2)
                                {
                                    if (nama_kelas.Substring(0, 2) == "X-" && m.KurikulumRaporLevel10 == Libs.JenisKurikulum.SMA.KURTILAS)
                                    {
                                        jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMA;
                                    }
                                    else if (nama_kelas.Substring(0, 2) == "X-" && m.KurikulumRaporLevel10 == Libs.JenisKurikulum.SMA.KTSP)
                                    {
                                        jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMA;
                                    }
                                }
                                if (nama_kelas.Length == 3)
                                {
                                    if (nama_kelas.Substring(0, 3) == "XI-" && m.KurikulumRaporLevel11 == Libs.JenisKurikulum.SMA.KURTILAS)
                                    {
                                        jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMA;
                                    }
                                    else if (nama_kelas.Substring(0, 3) == "XI-" && m.KurikulumRaporLevel11 == Libs.JenisKurikulum.SMA.KTSP)
                                    {
                                        jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMA;
                                    }
                                }
                                if (nama_kelas.Length == 4)
                                {
                                    if (nama_kelas.Substring(0, 4) == "XII-" && m.KurikulumRaporLevel12 == Libs.JenisKurikulum.SMA.KURTILAS)
                                    {
                                        jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMA;
                                    }
                                    else if (nama_kelas.Substring(0, 4) == "XII-" && m.KurikulumRaporLevel12 == Libs.JenisKurikulum.SMA.KTSP)
                                    {
                                        jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMA;
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
                                    NilaiRapor_Semester(this.Response, tahun_ajaran, semester, kelas, halaman, item_siswa, true, GetFileTTDGuru(tahun_ajaran, semester, m_kelas_det.Nama));
                                }
                                else if (tipe_rapor.Trim().ToUpper() == TipeRapor.LTS.ToUpper().Trim())
                                {
                                    NilaiRapor_LTS(this.Response, tahun_ajaran, semester, kelas, halaman, item_siswa, true);
                                }
                            }
                        }
                    }
                }
            }

            e.Result = 123;
        }

        protected string GetFileTTDGuru(string s_tahun_ajaran, string s_semester, string s_kelas)
        {
            string lokasi_ttd = Server.MapPath("~/Application_Files/Rapor/__TTD/" + s_tahun_ajaran.Replace("/", "-") + "-0" + s_semester + "/" + s_kelas + ".jpg");
            return lokasi_ttd;
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

        protected void NilaiRapor_Semester(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string halaman, string rel_siswa, bool export_to_pdf = false, string s_lokasi_ttd = "")
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

            if (export_to_pdf)
            {
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

                    string lokasi_ekspor = Libs.GetLokasiFolderFileRapor(
                        item_siswa.Replace(";", ""), tahun_ajaran, semester, rel_kelas_det, Libs.UnitSekolah.SMA
                    );

                    Reports_SMA.RaporSemester rapor = new Reports_SMA.RaporSemester(tahun_ajaran, semester, rel_kelas_det, item_siswa, Libs.GetStringToInteger(halaman), s_lokasi_ttd);

                    List<Reports_SMA.NilaiRaporKURTILAS> lst_nilai_rapor = new List<Reports_SMA.NilaiRaporKURTILAS>();
                    lst_nilai_rapor = rapor.GetRaporSemester;
                    DataTable dtRaporSemester = Libs.ToDataTable(lst_nilai_rapor);

                    List<Reports_SMA.NilaiRaporSikap> lst_nilai_rapor_sikap = new List<Reports_SMA.NilaiRaporSikap>();
                    lst_nilai_rapor_sikap = rapor.GetRaporSikap;
                    DataTable dtRaporSikap = Libs.ToDataTable(lst_nilai_rapor_sikap);

                    List<Reports_SMA.NilaiRaporEkskul> lst_nilai_rapor_ekskul = new List<Reports_SMA.NilaiRaporEkskul>();
                    lst_nilai_rapor_ekskul = rapor.GetRaporEkskul;
                    DataTable dtRaporEkskul = Libs.ToDataTable(lst_nilai_rapor_ekskul);

                    List<Rapor_CapaianKedisiplinan> lst_nilai_rapor_capaiankedisiplinan = new List<Rapor_CapaianKedisiplinan>();
                    lst_nilai_rapor_capaiankedisiplinan = rapor.GetRaporCapaianKedisiplinan;
                    DataTable dtCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_capaiankedisiplinan);

                    List<Reports_SMA.NilaiRaporVolunteer> lst_nilai_rapor_volunteer = new List<Reports_SMA.NilaiRaporVolunteer>();
                    lst_nilai_rapor_volunteer = rapor.GetRaporVolunteer;
                    DataTable dtRaporVolunteer = Libs.ToDataTable(lst_nilai_rapor_volunteer);

                    ReportDocument rpt_doc = new ReportDocument();
                    rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());

                    rpt_doc.Load(
                            Server.MapPath(
                                "~/Application_Reports/Penilaian/SMA/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS.rpt")
                            )
                        );
                    rpt_doc.SetDataSource(dtRaporSemester);
                    if (dtRaporSemester.Rows.Count > 0)
                    {
                        rpt_doc.OpenSubreport("RaporSikap").SetDataSource(dtRaporSikap);
                        rpt_doc.OpenSubreport("RaporEkskul").SetDataSource(dtRaporEkskul);
                        rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtRaporVolunteer);
                        if (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "").Replace("-", "")) >= 20202021)
                        {
                            rpt_doc.OpenSubreport("CapaianKedisiplinan").SetDataSource(dtCapaianKedisiplinan);
                        }
                    }

                    string file_name = "SMA RAPOR " +
                                       semester +
                                       " " +
                                       tahun_ajaran +
                                       " " +
                                       s_nama;
                    ExportToPDF(rpt_doc, lokasi_ekspor, file_name.Replace("/", "-"));

                    rpt_doc.Close();
                    rpt_doc.Dispose();
                }
            }
            else
            {
                Reports_SMA.RaporSemester rapor = new Reports_SMA.RaporSemester(tahun_ajaran, semester, rel_kelas_det, rel_siswa, Libs.GetStringToInteger(halaman), s_lokasi_ttd);

                List<Reports_SMA.NilaiRaporKURTILAS> lst_nilai_rapor = new List<Reports_SMA.NilaiRaporKURTILAS>();
                lst_nilai_rapor = rapor.GetRaporSemester;
                DataTable dtRaporSemester = Libs.ToDataTable(lst_nilai_rapor);

                List<Reports_SMA.NilaiRaporSikap> lst_nilai_rapor_sikap = new List<Reports_SMA.NilaiRaporSikap>();
                lst_nilai_rapor_sikap = rapor.GetRaporSikap;
                DataTable dtRaporSikap = Libs.ToDataTable(lst_nilai_rapor_sikap);

                List<Reports_SMA.NilaiRaporEkskul> lst_nilai_rapor_ekskul = new List<Reports_SMA.NilaiRaporEkskul>();
                lst_nilai_rapor_ekskul = rapor.GetRaporEkskul;
                DataTable dtRaporEkskul = Libs.ToDataTable(lst_nilai_rapor_ekskul);

                List<Rapor_CapaianKedisiplinan> lst_nilai_rapor_capaiankedisiplinan = new List<Rapor_CapaianKedisiplinan>();
                lst_nilai_rapor_capaiankedisiplinan = rapor.GetRaporCapaianKedisiplinan;
                DataTable dtCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_capaiankedisiplinan);

                List<Reports_SMA.NilaiRaporVolunteer> lst_nilai_rapor_volunteer = new List<Reports_SMA.NilaiRaporVolunteer>();
                lst_nilai_rapor_volunteer = rapor.GetRaporVolunteer;
                DataTable dtRaporVolunteer = Libs.ToDataTable(lst_nilai_rapor_volunteer);

                ReportDocument rpt_doc = new ReportDocument();
                rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());

                rpt_doc.Load(
                        Server.MapPath(
                            "~/Application_Reports/Penilaian/SMA/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS.rpt")
                        )
                    );
                rpt_doc.SetDataSource(dtRaporSemester);
                if (dtRaporSemester.Rows.Count > 0)
                {
                    rpt_doc.OpenSubreport("RaporSikap").SetDataSource(dtRaporSikap);
                    rpt_doc.OpenSubreport("RaporEkskul").SetDataSource(dtRaporEkskul);
                    rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtRaporVolunteer);
                    if (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "").Replace("-", "")) >= 20202021)
                    {
                        rpt_doc.OpenSubreport("CapaianKedisiplinan").SetDataSource(dtCapaianKedisiplinan);
                    }
                }

                CRV1.Page.Title = "RAPOR SMA " +
                                  tahun_ajaran +
                                  " SEMESTER " +
                                  semester;
                CRV1.ReportSource = rpt_doc;

                rpt_doc.ExportToHttpResponse(
                    ExportFormatType.PortableDocFormat, Response, false,
                    "SMA RAPOR SEMESTER " + tahun_ajaran.Replace("/", "-") + " SM " + semester + " KELAS " + kelas_det.ToUpper().Replace("/", "-")
                );

                
                rpt_doc.Close();
                rpt_doc.Dispose();
            }
        }

        protected void NilaiRapor_LTS(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string halaman, string rel_siswa, bool export_to_pdf = false)
        {
            if (tahun_ajaran.IndexOf("/") < 0) tahun_ajaran = RandomLibs.GetParseTahunAjaran(tahun_ajaran);
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
                        item_siswa, tahun_ajaran, semester, rel_kelas_det, Libs.UnitSekolah.SMA
                    );
                    Reports_SMA.LTS lts = new Reports_SMA.LTS(tahun_ajaran, semester, rel_kelas_det, rel_siswa, GetFileTTDGuru(tahun_ajaran, semester, kelas_det));

                    List<RaporLTS> lst_nilai_rapor_lts = new List<RaporLTS>();
                    lst_nilai_rapor_lts = lts.GetRaporLTS;
                    DataTable dtRaporLTS = Libs.ToDataTable(lst_nilai_rapor_lts);

                    List<RaporLTSDeskripsi> lst_nilai_rapor_lts_deskripsi = new List<RaporLTSDeskripsi>();
                    lst_nilai_rapor_lts_deskripsi = lts.GetRaporLTSDeskripsi;
                    DataTable dtRaporLTSDeskripsi = Libs.ToDataTable(lst_nilai_rapor_lts_deskripsi);

                    List<RaporLTSCapaianKedisiplinan> lst_nilai_rapor_lts_capaiankedisiplinan = new List<RaporLTSCapaianKedisiplinan>();
                    lst_nilai_rapor_lts_capaiankedisiplinan = lts.GetRaporLTSCapaianKedisiplinan;
                    DataTable dtRaporLTSCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_lts_capaiankedisiplinan);
                    
                    ReportDocument rpt_doc = new ReportDocument();
                    rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());

                    rpt_doc.Load(
                            Server.MapPath(
                                "~/Application_Reports/Penilaian/SMA/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "LTS.rpt")
                            )
                        );
                    rpt_doc.SetDataSource(dtRaporLTS);
                    if (dtRaporLTS.Rows.Count > 0)
                    {
                        rpt_doc.OpenSubreport("KeteranganTagihan").SetDataSource(dtRaporLTSDeskripsi);
                        rpt_doc.OpenSubreport("CapaianKedisiplinan").SetDataSource(dtRaporLTSCapaianKedisiplinan);
                    }

                    string file_name = "SMA LTS " +
                                       semester +
                                       " " +
                                       tahun_ajaran +
                                       " " +
                                       s_nama;
                    ExportToPDF(rpt_doc, lokasi_ekspor, file_name.Replace("/", "-").Trim());
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

                Reports_SMA.LTS lts = new Reports_SMA.LTS(tahun_ajaran, semester, rel_kelas_det, rel_siswa, GetFileTTDGuru(tahun_ajaran, semester, kelas_det));

                List<RaporLTS> lst_nilai_rapor_lts = new List<RaporLTS>();
                lst_nilai_rapor_lts = lts.GetRaporLTS;
                DataTable dtRaporLTS = Libs.ToDataTable(lst_nilai_rapor_lts);

                List<RaporLTSDeskripsi> lst_nilai_rapor_lts_deskripsi = new List<RaporLTSDeskripsi>();
                lst_nilai_rapor_lts_deskripsi = lts.GetRaporLTSDeskripsi;
                DataTable dtRaporLTSDeskripsi = Libs.ToDataTable(lst_nilai_rapor_lts_deskripsi);

                List<RaporLTSCapaianKedisiplinan> lst_nilai_rapor_lts_capaiankedisiplinan = new List<RaporLTSCapaianKedisiplinan>();
                lst_nilai_rapor_lts_capaiankedisiplinan = lts.GetRaporLTSCapaianKedisiplinan;
                DataTable dtRaporLTSCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_lts_capaiankedisiplinan);
                
                ReportDocument rpt_doc = new ReportDocument();
                rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());

                rpt_doc.Load(
                        Server.MapPath(
                            "~/Application_Reports/Penilaian/SMA/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "LTS.rpt")
                        )
                    );
                rpt_doc.SetDataSource(dtRaporLTS);
                if (dtRaporLTS.Rows.Count > 0)
                {
                    rpt_doc.OpenSubreport("KeteranganTagihan").SetDataSource(dtRaporLTSDeskripsi);
                    rpt_doc.OpenSubreport("CapaianKedisiplinan").SetDataSource(dtRaporLTSCapaianKedisiplinan);
                }

                CRV1.Page.Title = "SMA LTS " +
                                  tahun_ajaran +
                                  " SEMESTER " +
                                  semester;
                CRV1.ReportSource = rpt_doc;

                if (Libs.GetQueryString("idm").Trim() != "" && Libs.GetQueryString("token").Trim() == Application_Libs.Constantas.TOKEN_VIEW_ORTU)
                {
                    //update view
                    if (Libs.GetQueryString("act").Trim() != "adm")
                    {
                        DAO_Rapor_ViewEmailLTS.Insert(
                                new Rapor_ViewEmailLTS
                                {
                                    Kode = Guid.NewGuid(),
                                    Rel_Email = Libs.GetQueryString("idm").Trim(),
                                    Rel_Siswa = Libs.GetQueryString("sw").Trim(),
                                    Tanggal = DateTime.Now,
                                    URL = Libs.GetQueryString("from").Trim()
                                }
                            );
                    }
                    //end update view

                    rpt_doc.ExportToHttpResponse(
                        ExportFormatType.PortableDocFormat, Response, false,
                        "SMA LTS " + tahun_ajaran.Replace("/", "-") + " SM " + semester + " KELAS " + kelas_det.ToUpper().Replace("/", "-")
                    );
                }
                else if (Libs.GetQueryString("idm").Trim() == "" && Libs.GetQueryString("token").Trim() == "")
                {
                    rpt_doc.ExportToHttpResponse(
                        ExportFormatType.PortableDocFormat, Response, false,
                        "SMA LTS " + tahun_ajaran.Replace("/", "-") + " SM " + semester + " KELAS " + kelas_det.ToUpper().Replace("/", "-")
                    );
                }
            }   
        }
    }
}