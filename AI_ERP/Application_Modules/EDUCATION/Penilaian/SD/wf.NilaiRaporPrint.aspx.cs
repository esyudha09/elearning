using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
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
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning.SD.Reports;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
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

            public static string GetTipeRapor()
            {
                string tr = Libs.GetQueryString("tr");
                return tr;
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

            public static string GetSiswa()
            {
                string sis = Libs.GetQueryString("sis");
                return sis;
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
                            string s_lokasi_ttd = GetFileTTDGuru(tahun_ajaran, semester, m_kelas_det.Nama.Trim());
                            string nama_kelas = m_kelas.Nama.Replace("-", "") + "-";
                            string jenis_download_0 = "";
                            string jenis_download_1 = "";

                            if (nama_kelas.Length >= 2)
                            {
                                if (nama_kelas.Substring(0, 2) == "I-" && m.KurikulumRaporLevel1 == Libs.JenisKurikulum.SD.KURTILAS)
                                {
                                    jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
                                    jenis_download_1 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
                                }
                                else if (nama_kelas.Substring(0, 2) == "I-" && m.KurikulumRaporLevel1 == Libs.JenisKurikulum.SD.KTSP)
                                {
                                    jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD;
                                    jenis_download_1 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD;
                                }
                            }
                            if (nama_kelas.Length >= 3)
                            {
                                if (nama_kelas.Substring(0, 3) == "II-" && m.KurikulumRaporLevel2 == Libs.JenisKurikulum.SD.KURTILAS)
                                {
                                    jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
                                    jenis_download_1 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
                                }
                                else if (nama_kelas.Substring(0, 3) == "II-" && m.KurikulumRaporLevel2 == Libs.JenisKurikulum.SD.KTSP)
                                {
                                    jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD;
                                    jenis_download_1 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD;
                                }
                            }
                            if (nama_kelas.Length >= 4)
                            {
                                if (nama_kelas.Substring(0, 4) == "III-" && m.KurikulumRaporLevel3 == Libs.JenisKurikulum.SD.KURTILAS)
                                {
                                    jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
                                    jenis_download_1 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
                                }
                                else if (nama_kelas.Substring(0, 4) == "III-" && m.KurikulumRaporLevel3 == Libs.JenisKurikulum.SD.KTSP)
                                {
                                    jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD;
                                    jenis_download_1 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD;
                                }
                            }
                            if (nama_kelas.Length >= 3)
                            {
                                if (nama_kelas.Substring(0, 3) == "IV-" && m.KurikulumRaporLevel4 == Libs.JenisKurikulum.SD.KURTILAS)
                                {
                                    jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
                                    jenis_download_1 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
                                }
                                else if (nama_kelas.Substring(0, 3) == "IV-" && m.KurikulumRaporLevel4 == Libs.JenisKurikulum.SD.KTSP)
                                {
                                    jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD;
                                    jenis_download_1 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD;
                                }
                            }
                            if (nama_kelas.Length >= 2)
                            {
                                if (nama_kelas.Substring(0, 2) == "V-" && m.KurikulumRaporLevel5 == Libs.JenisKurikulum.SD.KURTILAS)
                                {
                                    jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
                                    jenis_download_1 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
                                }
                                else if (nama_kelas.Substring(0, 2) == "V-" && m.KurikulumRaporLevel5 == Libs.JenisKurikulum.SD.KTSP)
                                {
                                    jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD;
                                    jenis_download_1 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD;
                                }
                            }
                            if (nama_kelas.Length >= 3)
                            {
                                if (nama_kelas.Substring(0, 3) == "VI-" && m.KurikulumRaporLevel6 == Libs.JenisKurikulum.SD.KURTILAS)
                                {
                                    jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD;
                                    jenis_download_1 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD;
                                }
                                else if (nama_kelas.Substring(0, 3) == "VI-" && m.KurikulumRaporLevel6 == Libs.JenisKurikulum.SD.KTSP)
                                {
                                    jenis_download_0 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD;
                                    jenis_download_1 = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD;
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
                                    if (jenis_download_0 == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD)
                                    {
                                        NilaiRapor_KTSP(this.Response, tahun_ajaran, semester, kelas, item_siswa, true);
                                    }
                                    else if (jenis_download_0 == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD)
                                    {
                                        NilaiRapor_KURTILAS(this.Response, tahun_ajaran, semester, kelas, item_siswa, true);
                                    }

                                    if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) < 2020)
                                    {
                                        if (jenis_download_1 == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD)
                                        {
                                            UraianRapor_KTSP(this.Response, tahun_ajaran, semester, kelas, item_siswa, true);
                                        }
                                        else if (jenis_download_1 == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD)
                                        {
                                            UraianRapor_KURTILAS(this.Response, tahun_ajaran, semester, kelas, item_siswa, true);
                                        }
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
            if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD)
            {
                NilaiRapor_KTSP(this.Response, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), QS.GetSiswa());
            }
            else if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KTSP_SD)
            {
                UraianRapor_KTSP(this.Response, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), QS.GetSiswa());
            }
            else if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SD)
            {
                NilaiRapor_KURTILAS(this.Response, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), QS.GetSiswa());
            }
            else if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_URAIAN_KURTILAS_SD)
            {
                UraianRapor_KURTILAS(this.Response, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), QS.GetSiswa());
            }
            else if (Libs.GetQueryString(AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY) == AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_LTS_SD)
            {
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(QS.GetKelas());
                string lokasi_ttd = GetFileTTDGuru(QS.GetTahunAjaran(), QS.GetSemester(), m_kelas_det.Nama);
                NilaiRapor_LTS(this.Response, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), QS.GetSiswa(), lokasi_ttd);
            }
        }

        public static string GetUnit()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SD).FirstOrDefault().Kode.ToString();
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
                        item_siswa, tahun_ajaran, semester, rel_kelas_det, Libs.UnitSekolah.SD
                    );

                    Reports_SD.LTS lts = new Reports_SD.LTS(tahun_ajaran, semester, rel_kelas_det, rel_siswa, s_lokasi_ttd);

                    List<RaporLTS> lst_nilai_rapor_lts = new List<RaporLTS>();
                    lst_nilai_rapor_lts = lts.GetRaporLTS;
                    DataTable dtRaporLTS = Libs.ToDataTable(lst_nilai_rapor_lts);

                    List<RaporLTSDeskripsi> lst_nilai_rapor_lts_deskripsi = new List<RaporLTSDeskripsi>();
                    lst_nilai_rapor_lts_deskripsi = lts.GetRaporLTSDeskripsi;
                    DataTable dtRaporLTSDeskripsi = Libs.ToDataTable(lst_nilai_rapor_lts_deskripsi);

                    List<RaporLTSCapaianKedisiplinan> lst_nilai_rapor_lts_capaiankedisiplinan = new List<RaporLTSCapaianKedisiplinan>();
                    lst_nilai_rapor_lts_capaiankedisiplinan = lts.GetRaporLTSCapaianKedisiplinan;
                    DataTable dtRaporLTSCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_lts_capaiankedisiplinan);

                    var lst_nilai_ekskul_ = DAO_Rapor_Semester.GetNilaiEkskul_KTSP(
                        tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString(), rel_kelas_det, rel_siswa
                    ).Select(m => new { m.Rel_Siswa, m.Kegiatan, m.LTS_CK_KEHADIRAN }).Distinct().ToList();
                    List<RaporLTSEkskul> lst_nilai_ekskul = new List<RaporLTSEkskul>();
                    lst_nilai_ekskul.Clear();
                    foreach (var item in lst_nilai_ekskul_)
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
                                "~/Application_Reports/Penilaian/SD/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "LTS.rpt")
                            )
                        );
                    rpt_doc.SetDataSource(dtRaporLTS);
                    if (dtRaporLTS.Rows.Count > 0)
                    {
                        rpt_doc.OpenSubreport("KeteranganTagihan").SetDataSource(dtRaporLTSDeskripsi);
                        rpt_doc.OpenSubreport("CapaianKedisiplinan").SetDataSource(dtRaporLTSCapaianKedisiplinan);
                        rpt_doc.OpenSubreport("Ekstrakurikuler").SetDataSource(dtRaporLTSEkskul);
                    }

                    string file_name = "SD " +
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

                Reports_SD.LTS lts = new Reports_SD.LTS(tahun_ajaran, semester, rel_kelas_det, rel_siswa, s_lokasi_ttd);

                List<RaporLTS> lst_nilai_rapor_lts = new List<RaporLTS>();
                lst_nilai_rapor_lts = lts.GetRaporLTS;
                DataTable dtRaporLTS = Libs.ToDataTable(lst_nilai_rapor_lts);

                List<RaporLTSDeskripsi> lst_nilai_rapor_lts_deskripsi = new List<RaporLTSDeskripsi>();
                lst_nilai_rapor_lts_deskripsi = lts.GetRaporLTSDeskripsi;
                DataTable dtRaporLTSDeskripsi = Libs.ToDataTable(lst_nilai_rapor_lts_deskripsi);

                List<RaporLTSCapaianKedisiplinan> lst_nilai_rapor_lts_capaiankedisiplinan = new List<RaporLTSCapaianKedisiplinan>();
                lst_nilai_rapor_lts_capaiankedisiplinan = lts.GetRaporLTSCapaianKedisiplinan;
                DataTable dtRaporLTSCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_lts_capaiankedisiplinan);

                var lst_nilai_ekskul_ = DAO_Rapor_Semester.GetNilaiEkskul_KTSP(
                        tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString(), rel_kelas_det, rel_siswa
                    ).Select(m => new { m.Rel_Siswa, m.Kegiatan, m.LTS_CK_KEHADIRAN }).Distinct().ToList();
                List<RaporLTSEkskul> lst_nilai_ekskul = new List<RaporLTSEkskul>();
                lst_nilai_ekskul.Clear();
                foreach (var item in lst_nilai_ekskul_)
                {
                    lst_nilai_ekskul.Add(new RaporLTSEkskul {
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
                            "~/Application_Reports/Penilaian/SD/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "LTS.rpt")
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
                    "RAPOR LTS SD " + tahun_ajaran.Replace("/", "-") + " SM " + semester + " KELAS " + kelas_det.ToUpper().Replace("/", "-")
                );
            }
        }
        
        protected void NilaiRapor_KTSP(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa = "", bool export_to_pdf = false)
        {
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
                        item_siswa, tahun_ajaran, semester, rel_kelas_det, Libs.UnitSekolah.SD
                    );
                    string kelas_det = "";
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            kelas_det = m_kelas_det.Nama;
                        }
                    }

                    string lokasi_ttd = GetFileTTDGuru(tahun_ajaran, semester, kelas_det);
                    List<KTSP_RaporNilai> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KTSP(tahun_ajaran, semester, rel_kelas_det, item_siswa, lokasi_ttd);
                    DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

                    ReportDocument rpt_doc = new ReportDocument();
                    rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());
                    rpt_doc.Load(
                            Server.MapPath(
                                (
                                    (
                                        (kelas_det.Substring(0, 2) == "VI" || kelas_det.Substring(0, 1) == "6") && semester == "2"
                                    )
                                    ? "~/Application_Reports/Penilaian/SD/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP_VI.rpt")
                                    : "~/Application_Reports/Penilaian/SD/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP.rpt")
                                )
                            )
                        );
                    rpt_doc.SetDataSource(dtNilai);

                    string file_name = "SD " +
                                       "RAPOR " +
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

                string lokasi_ttd = ""; //GetFileTTDGuru(tahun_ajaran, semester, kelas_det);
                List<KTSP_RaporNilai> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KTSP(tahun_ajaran, semester, rel_kelas_det, rel_siswa, lokasi_ttd);
                DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

                ReportDocument rpt_doc = new ReportDocument();
                rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());
                rpt_doc.Load(
                        Server.MapPath(
                            (
                                (
                                    (kelas_det.Substring(0, 2) == "VI" || kelas_det.Substring(0, 1) == "6") && semester == "2"
                                )
                                ? "~/Application_Reports/Penilaian/SD/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP_VI.rpt")
                                : "~/Application_Reports/Penilaian/SD/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP.rpt")
                            )
                        )
                    );
                rpt_doc.SetDataSource(dtNilai);

                CRV1.Page.Title = "RAPOR SD " +
                                  tahun_ajaran +
                                  " SEMESTER " +
                                  semester;
                CRV1.ReportSource = rpt_doc;
                rpt_doc.ExportToHttpResponse(
                        ExportFormatType.PortableDocFormat, Response, false,
                        "RAPOR SD " + tahun_ajaran.Replace("/", "-") + " SM " + semester + " KELAS " + kelas_det.ToUpper().Replace("/", "-")
                    );
            }
        }

        protected string GetFileTTDGuru(string s_tahun_ajaran, string s_semester, string s_kelas)
        {
            string lokasi_ttd = Server.MapPath("~/Application_Files/Rapor/__TTD/" + s_tahun_ajaran.Replace("/", "-") + "-0" + s_semester + "/" + s_kelas + ".jpg");
            return lokasi_ttd;
        }

        protected void UraianRapor_KTSP(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa = "", bool export_to_pdf = false)
        {
            if (export_to_pdf)
            {
                string[] arr_siswa = rel_siswa.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item_siswa in arr_siswa)
                {
                    string lokasi_ekspor = Libs.GetLokasiFolderFileRapor(
                        item_siswa, tahun_ajaran, semester, rel_kelas_det, Libs.UnitSekolah.SD
                    );
                    string kelas_det = "";
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            kelas_det = m_kelas_det.Nama;
                        }
                    }

                    string lokasi_ttd = GetFileTTDGuru(tahun_ajaran, semester, kelas_det);
                    List<KTSP_UraianKompetensi> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiUraianKompetensi_KTSP(tahun_ajaran, semester, rel_kelas_det, item_siswa, lokasi_ttd);
                    DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

                    List<KTSP_RaporSikap> lst_nilai_sikap = DAO_Rapor_Semester.GetNilaiSikap_KTSP(tahun_ajaran, semester, rel_kelas_det, item_siswa);
                    DataTable dtNilaiSikap = Libs.ToDataTable(lst_nilai_sikap);

                    List<KTSP_RaporSaran> lst_nilai_saran = DAO_Rapor_Semester.GetSaran_KTSP(tahun_ajaran, semester, rel_kelas_det, item_siswa);
                    DataTable dtNilaiSaran = Libs.ToDataTable(lst_nilai_saran);

                    List<KTSP_RaporEkskul> lst_nilai_ekskul = DAO_Rapor_Semester.GetNilaiEkskul_KTSP(tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString(), rel_kelas_det, rel_siswa);
                    DataTable dtNilaiEkskul = Libs.ToDataTable(lst_nilai_ekskul);

                    List<KTSP_RaporVolunteer> lst_nilai_volunteer = DAO_Rapor_Semester.GetVolunteer(tahun_ajaran, semester, rel_kelas_det);
                    DataTable dtNilaiVolunteer = Libs.ToDataTable(lst_nilai_volunteer);

                    List<KTSP_RaporKetidakhadiran> lst_absen = DAO_Rapor_Semester.GetAbsen(tahun_ajaran, semester, rel_kelas_det, item_siswa);
                    DataTable dtNilaiAbsensi = Libs.ToDataTable(lst_absen);

                    ReportDocument rpt_doc = new ReportDocument();
                    rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());
                    rpt_doc.Load(
                            Server.MapPath(
                                "~/Application_Reports/Penilaian/SD/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP_URAIAN.rpt")
                            )
                        );
                    rpt_doc.SetDataSource(dtNilai);
                    if (dtNilai.Rows.Count > 0)
                    {
                        rpt_doc.OpenSubreport("RaporSikapKTSP").SetDataSource(dtNilaiSikap);
                        rpt_doc.OpenSubreport("RaporSaranKTSP").SetDataSource(dtNilaiSaran);
                        rpt_doc.OpenSubreport("RaporEkskulKTSP").SetDataSource(dtNilaiEkskul);
                        rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtNilaiVolunteer);
                        rpt_doc.OpenSubreport("RaporKetidakhadiran").SetDataSource(dtNilaiAbsensi);
                    }

                    string file_name = "URAIAN RAPOR SD " +
                                      tahun_ajaran +
                                      " SEMESTER " +
                                      semester;
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

                string lokasi_ttd = ""; //GetFileTTDGuru(tahun_ajaran, semester, kelas_det);
                List<KTSP_UraianKompetensi> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiUraianKompetensi_KTSP(tahun_ajaran, semester, rel_kelas_det, rel_siswa, lokasi_ttd);
                DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

                List<KTSP_RaporSikap> lst_nilai_sikap = DAO_Rapor_Semester.GetNilaiSikap_KTSP(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
                DataTable dtNilaiSikap = Libs.ToDataTable(lst_nilai_sikap);

                List<KTSP_RaporSaran> lst_nilai_saran = DAO_Rapor_Semester.GetSaran_KTSP(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
                DataTable dtNilaiSaran = Libs.ToDataTable(lst_nilai_saran);

                List<KTSP_RaporEkskul> lst_nilai_ekskul = DAO_Rapor_Semester.GetNilaiEkskul_KTSP(tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString(), rel_kelas_det, rel_siswa);
                DataTable dtNilaiEkskul = Libs.ToDataTable(lst_nilai_ekskul);

                List<KTSP_RaporVolunteer> lst_nilai_volunteer = DAO_Rapor_Semester.GetVolunteer(tahun_ajaran, semester, rel_kelas_det);
                DataTable dtNilaiVolunteer = Libs.ToDataTable(lst_nilai_volunteer);

                List<KTSP_RaporKetidakhadiran> lst_absen = DAO_Rapor_Semester.GetAbsen(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
                DataTable dtNilaiAbsensi = Libs.ToDataTable(lst_absen);

                ReportDocument rpt_doc = new ReportDocument();
                rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());
                rpt_doc.Load(
                        Server.MapPath(
                            "~/Application_Reports/Penilaian/SD/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP_URAIAN.rpt")
                        )
                    );
                rpt_doc.SetDataSource(dtNilai);
                if (dtNilai.Rows.Count > 0)
                {
                    rpt_doc.OpenSubreport("RaporSikapKTSP").SetDataSource(dtNilaiSikap);
                    rpt_doc.OpenSubreport("RaporSaranKTSP").SetDataSource(dtNilaiSaran);
                    rpt_doc.OpenSubreport("RaporEkskulKTSP").SetDataSource(dtNilaiEkskul);
                    rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtNilaiVolunteer);
                    rpt_doc.OpenSubreport("RaporKetidakhadiran").SetDataSource(dtNilaiAbsensi);
                }

                CRV1.Page.Title = "RAPOR SD " +
                                  tahun_ajaran +
                                  " SEMESTER " +
                                  semester;
                CRV1.ReportSource = rpt_doc;
                rpt_doc.ExportToHttpResponse(
                        ExportFormatType.PortableDocFormat, Response, false,
                        "RAPOR SD " + tahun_ajaran.Replace("/", "-") + " SM " + semester + " KELAS " + kelas_det.ToUpper().Replace("/", "-") + " (URAIAN)"
                    );
            }
        }

        protected void UraianRapor_KURTILAS(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa = "", bool export_to_pdf = false)
        {
            if (export_to_pdf)
            {
                string[] arr_siswa = rel_siswa.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item_siswa in arr_siswa)
                {
                    string lokasi_ekspor = Libs.GetLokasiFolderFileRapor(
                        item_siswa, tahun_ajaran, semester, rel_kelas_det, Libs.UnitSekolah.SD
                    );
                    string kelas_det = "";
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            kelas_det = m_kelas_det.Nama;
                        }
                    }

                    string lokasi_ttd = GetFileTTDGuru(tahun_ajaran, semester, kelas_det);
                    List<KTSP_UraianKompetensi> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiUraianKompetensi_KTSP(tahun_ajaran, semester, rel_kelas_det, item_siswa, lokasi_ttd);
                    DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

                    List<KTSP_RaporSaran> lst_nilai_saran = DAO_Rapor_Semester.GetSaran_KTSP(tahun_ajaran, semester, rel_kelas_det, item_siswa);
                    DataTable dtNilaiSaran = Libs.ToDataTable(lst_nilai_saran);

                    List<KTSP_RaporEkskul> lst_nilai_ekskul = DAO_Rapor_Semester.GetNilaiEkskul_KTSP(tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString(), rel_kelas_det, rel_siswa);
                    DataTable dtNilaiEkskul = Libs.ToDataTable(lst_nilai_ekskul);

                    List<KTSP_RaporVolunteer> lst_nilai_volunteer = DAO_Rapor_Semester.GetVolunteer(tahun_ajaran, semester, rel_kelas_det);
                    DataTable dtNilaiVolunteer = Libs.ToDataTable(lst_nilai_volunteer);

                    List<KTSP_RaporKetidakhadiran> lst_absen = DAO_Rapor_Semester.GetAbsen(tahun_ajaran, semester, rel_kelas_det, item_siswa);
                    DataTable dtNilaiAbsensi = Libs.ToDataTable(lst_absen);

                    ReportDocument rpt_doc = new ReportDocument();
                    rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());
                    rpt_doc.Load(
                            Server.MapPath(
                                "~/Application_Reports/Penilaian/SD/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS_URAIAN.rpt")
                            )
                        );
                    rpt_doc.SetDataSource(dtNilai);
                    if (dtNilai.Rows.Count > 0)
                    {
                        rpt_doc.OpenSubreport("RaporSaranKTSP").SetDataSource(dtNilaiSaran);
                        rpt_doc.OpenSubreport("RaporEkskulKTSP").SetDataSource(dtNilaiEkskul);
                        rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtNilaiVolunteer);
                        rpt_doc.OpenSubreport("RaporKetidakhadiran").SetDataSource(dtNilaiAbsensi);
                    }

                    string file_name = "URAIAN RAPOR SD " +
                                      tahun_ajaran +
                                      " SEMESTER " +
                                      semester;
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

                string lokasi_ttd = ""; //GetFileTTDGuru(tahun_ajaran, semester, kelas_det);
                List<KTSP_UraianKompetensi> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiUraianKompetensi_KTSP(tahun_ajaran, semester, rel_kelas_det, rel_siswa, lokasi_ttd);
                DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

                List<KTSP_RaporSaran> lst_nilai_saran = DAO_Rapor_Semester.GetSaran_KTSP(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
                DataTable dtNilaiSaran = Libs.ToDataTable(lst_nilai_saran);

                List<KTSP_RaporEkskul> lst_nilai_ekskul = DAO_Rapor_Semester.GetNilaiEkskul_KTSP(tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString(), rel_kelas_det, rel_siswa);
                DataTable dtNilaiEkskul = Libs.ToDataTable(lst_nilai_ekskul);

                List<KTSP_RaporVolunteer> lst_nilai_volunteer = DAO_Rapor_Semester.GetVolunteer(tahun_ajaran, semester, rel_kelas_det);
                DataTable dtNilaiVolunteer = Libs.ToDataTable(lst_nilai_volunteer);

                List<KTSP_RaporKetidakhadiran> lst_absen = DAO_Rapor_Semester.GetAbsen(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
                DataTable dtNilaiAbsensi = Libs.ToDataTable(lst_absen);

                ReportDocument rpt_doc = new ReportDocument();
                rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());
                rpt_doc.Load(
                        Server.MapPath(
                            "~/Application_Reports/Penilaian/SD/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS_URAIAN.rpt")
                        )
                    );
                rpt_doc.SetDataSource(dtNilai);
                if (dtNilai.Rows.Count > 0)
                {
                    rpt_doc.OpenSubreport("RaporSaranKTSP").SetDataSource(dtNilaiSaran);
                    rpt_doc.OpenSubreport("RaporEkskulKTSP").SetDataSource(dtNilaiEkskul);
                    rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtNilaiVolunteer);
                    rpt_doc.OpenSubreport("RaporKetidakhadiran").SetDataSource(dtNilaiAbsensi);
                }

                CRV1.Page.Title = "RAPOR SD " +
                                    tahun_ajaran +
                                    " SEMESTER " +
                                    semester;
                CRV1.ReportSource = rpt_doc;
                rpt_doc.ExportToHttpResponse(
                        ExportFormatType.PortableDocFormat, Response, false,
                        "RAPOR SD " + tahun_ajaran.Replace("/", "-") + " SM " + semester + " KELAS " + kelas_det.ToUpper().Replace("/", "-") + " (URAIAN)"
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

        protected void NilaiRapor_KURTILAS(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa = "", bool export_to_pdf = false)
        {
            List<RaporLTSCapaianKedisiplinan> lst_nilai_rapor_lts_capaiankedisiplinan = new List<Application_Entities.Elearning.SD.Reports.RaporLTSCapaianKedisiplinan>();

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
                        item_siswa, tahun_ajaran, semester, rel_kelas_det, Libs.UnitSekolah.SD
                    );
                    string kelas_det = "";
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            kelas_det = m_kelas_det.Nama;
                        }
                    }
                    
                    List<KURTILAS_RaporNilai> lst_nilai_rapor = new List<KURTILAS_RaporNilai>();
                    List<KURTILAS_RaporMulok> lst_nilai_mulok = new List<KURTILAS_RaporMulok>();

                    DataTable dtNilaiMulok = new DataTable();
                    DataTable dtNilaiSaran = new DataTable();
                    DataTable dtNilaiEkskul = new DataTable();
                    DataTable dtNilaiVolunteer = new DataTable();
                    DataTable dtNilaiAbsensi = new DataTable();
                    DataTable dtRaporLTSCapaianKedisiplinan = new DataTable();
                    
                    if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2020)
                    {
                        string lokasi_ttd = GetFileTTDGuru(tahun_ajaran, semester, kelas_det);
                        lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KURTILAS_2020(tahun_ajaran, semester, rel_kelas_det, ref lst_nilai_rapor_lts_capaiankedisiplinan, item_siswa, lokasi_ttd);

                        List<KTSP_RaporSaran> lst_nilai_saran = DAO_Rapor_Semester.GetSaran_KTSP(tahun_ajaran, semester, rel_kelas_det, item_siswa);
                        dtNilaiSaran = Libs.ToDataTable(lst_nilai_saran);

                        List<KTSP_RaporEkskul> lst_nilai_ekskul = DAO_Rapor_Semester.GetNilaiEkskul_KTSP(tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString(), rel_kelas_det, rel_siswa);
                        dtNilaiEkskul = Libs.ToDataTable(lst_nilai_ekskul);

                        List<KTSP_RaporVolunteer> lst_nilai_volunteer = DAO_Rapor_Semester.GetVolunteer(tahun_ajaran, semester, rel_kelas_det);
                        dtNilaiVolunteer = Libs.ToDataTable(lst_nilai_volunteer);

                        List<KTSP_RaporKetidakhadiran> lst_absen = DAO_Rapor_Semester.GetAbsen(tahun_ajaran, semester, rel_kelas_det, item_siswa);
                        dtNilaiAbsensi = Libs.ToDataTable(lst_absen);

                        //List<RaporLTSCapaianKedisiplinan> lst_nilai_rapor_lts_capaiankedisiplinan = new List<RaporLTSCapaianKedisiplinan>();
                        //lst_nilai_rapor_lts_capaiankedisiplinan.Clear();
                        //foreach (var item in lst_nilai_rapor)
                        //{
                        //    foreach (var item_det in item.HasilCapaianKedisiplinan)
                        //    {
                        //        lst_nilai_rapor_lts_capaiankedisiplinan.Add(item_det);
                        //    }
                        //}
                        //var lst_nilai_rapor_lts_capaiankedisiplinan_ = lst_nilai_rapor_lts_capaiankedisiplinan.Select(
                        //        m0 => new {
                        //            m0.Rel_Siswa,
                        //            m0.KodeKelompokMapel,
                        //            m0.KelompokMapel,
                        //            m0.NomorMapel,
                        //            m0.UrutanMapel,
                        //            m0.Rel_Mapel,
                        //            m0.NamaMapel,
                        //            m0.Kehadiran,
                        //            m0.KetepatanWaktu,
                        //            m0.PenggunaanSeragam,
                        //            m0.PenggunaanKamera
                        //        }
                        //   ).ToList();
                        //dtRaporLTSCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_lts_capaiankedisiplinan_);
                        //List<RaporLTSCapaianKedisiplinan> lst_nilai_rapor_lts_capaiankedisiplinan = DAO_Rapor_Semester.ListRaporLTSCapaianKedisiplinan;
                        dtRaporLTSCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_lts_capaiankedisiplinan);
                    }
                    else
                    {
                        lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KURTILAS(tahun_ajaran, semester, rel_kelas_det, item_siswa);
                        lst_nilai_mulok = DAO_Rapor_Semester.GetNilaiMulok_KURTILAS(tahun_ajaran, semester, rel_kelas_det, item_siswa);
                        dtNilaiMulok = Libs.ToDataTable(lst_nilai_mulok);
                    }
                    DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

                    List<KURTILAS_RaporSikap> lst_nilai_sikap = DAO_Rapor_Semester.GetNilaiSikap_KURTILAS(tahun_ajaran, semester, rel_kelas_det, item_siswa);
                    DataTable dtNilaiSikap = Libs.ToDataTable(lst_nilai_sikap);

                    ReportDocument rpt_doc = new ReportDocument();
                    rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());
                    rpt_doc.Load(
                            Server.MapPath(
                                "~/Application_Reports/Penilaian/SD/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS.rpt")
                            )
                        );
                    rpt_doc.SetDataSource(dtNilai);
                    if (dtNilai.Rows.Count > 0)
                    {
                        rpt_doc.OpenSubreport("RaporSikap").SetDataSource(dtNilaiSikap);
                        if (Libs.GetStringToDecimal(tahun_ajaran.Substring(0, 4)) < 2020)
                        {
                            rpt_doc.OpenSubreport("RaporMulok").SetDataSource(dtNilaiMulok);
                        }
                        else
                        {
                            rpt_doc.OpenSubreport("rptCatatanWalas").SetDataSource(dtNilaiSaran);
                            rpt_doc.OpenSubreport("rptEkskul").SetDataSource(dtNilaiEkskul);
                            rpt_doc.OpenSubreport("rptVolunteer").SetDataSource(dtNilaiVolunteer);
                            rpt_doc.OpenSubreport("rptKetidakHadiran").SetDataSource(dtNilaiAbsensi);
                            rpt_doc.OpenSubreport("CapaianKedisiplinan").SetDataSource(dtRaporLTSCapaianKedisiplinan);
                        }
                    }

                    string file_name = "SD " +
                                       "RAPOR " +
                                       semester +
                                       " " +
                                       tahun_ajaran +
                                       " " +
                                       s_nama;
                    ExportToPDF(rpt_doc, lokasi_ekspor, file_name.Replace("/", "-"));

                    rpt_doc.Close();
                    rpt_doc.Dispose();
                    rpt_doc = null;
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

                List<KURTILAS_RaporNilai> lst_nilai_rapor = new List<KURTILAS_RaporNilai>();
                List<KURTILAS_RaporMulok> lst_nilai_mulok = new List<KURTILAS_RaporMulok>();

                DataTable dtNilaiMulok = new DataTable();
                DataTable dtNilaiSaran = new DataTable();
                DataTable dtNilaiEkskul = new DataTable();
                DataTable dtNilaiVolunteer = new DataTable();
                DataTable dtNilaiAbsensi = new DataTable();
                DataTable dtRaporLTSCapaianKedisiplinan = new DataTable();

                if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2020)
                {
                    string lokasi_ttd = GetFileTTDGuru(tahun_ajaran, semester, kelas_det);
                    lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KURTILAS_2020(tahun_ajaran, semester, rel_kelas_det, ref lst_nilai_rapor_lts_capaiankedisiplinan, rel_siswa, lokasi_ttd);
                    
                    List<KTSP_RaporSaran> lst_nilai_saran = DAO_Rapor_Semester.GetSaran_KTSP(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
                    dtNilaiSaran = Libs.ToDataTable(lst_nilai_saran);

                    List<KTSP_RaporEkskul> lst_nilai_ekskul = DAO_Rapor_Semester.GetNilaiEkskul_KTSP(tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString(), rel_kelas_det, rel_siswa);
                    dtNilaiEkskul = Libs.ToDataTable(lst_nilai_ekskul);

                    List<KTSP_RaporVolunteer> lst_nilai_volunteer = DAO_Rapor_Semester.GetVolunteer(tahun_ajaran, semester, rel_kelas_det);
                    dtNilaiVolunteer = Libs.ToDataTable(lst_nilai_volunteer);

                    List<KTSP_RaporKetidakhadiran> lst_absen = DAO_Rapor_Semester.GetAbsen(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
                    dtNilaiAbsensi = Libs.ToDataTable(lst_absen);

                    //List<RaporLTSCapaianKedisiplinan> lst_nilai_rapor_lts_capaiankedisiplinan = new List<RaporLTSCapaianKedisiplinan>();
                    //lst_nilai_rapor_lts_capaiankedisiplinan.Clear();
                    //foreach (var item in lst_nilai_rapor)
                    //{
                    //    foreach (var item_det in item.HasilCapaianKedisiplinan)
                    //    {
                    //        lst_nilai_rapor_lts_capaiankedisiplinan.Add(item_det);
                    //    }
                    //}
                    //var lst_nilai_rapor_lts_capaiankedisiplinan_ = lst_nilai_rapor_lts_capaiankedisiplinan.Select(
                    //        m0 => new {
                    //            m0.Rel_Siswa,
                    //            m0.KodeKelompokMapel,
                    //            m0.KelompokMapel,
                    //            m0.NomorMapel,
                    //            m0.UrutanMapel,
                    //            m0.Rel_Mapel,
                    //            m0.NamaMapel,
                    //            m0.Kehadiran,
                    //            m0.KetepatanWaktu,
                    //            m0.PenggunaanSeragam,
                    //            m0.PenggunaanKamera
                    //        }
                    //   ).ToList();
                    //dtRaporLTSCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_lts_capaiankedisiplinan_);

                    //List<RaporLTSCapaianKedisiplinan> lst_nilai_rapor_lts_capaiankedisiplinan = DAO_Rapor_Semester.ListRaporLTSCapaianKedisiplinan;
                    dtRaporLTSCapaianKedisiplinan = Libs.ToDataTable(lst_nilai_rapor_lts_capaiankedisiplinan);
                }
                else
                {
                    lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KURTILAS(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
                    lst_nilai_mulok = DAO_Rapor_Semester.GetNilaiMulok_KURTILAS(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
                    dtNilaiMulok = Libs.ToDataTable(lst_nilai_mulok);
                }
                DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

                List<KURTILAS_RaporSikap> lst_nilai_sikap = DAO_Rapor_Semester.GetNilaiSikap_KURTILAS(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
                DataTable dtNilaiSikap = Libs.ToDataTable(lst_nilai_sikap);
                
                ReportDocument rpt_doc = new ReportDocument();
                rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());
                rpt_doc.Load(
                        Server.MapPath(
                            "~/Application_Reports/Penilaian/SD/" + Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS.rpt")
                        )
                    );
                rpt_doc.SetDataSource(dtNilai);
                if (dtNilai.Rows.Count > 0)
                {
                    rpt_doc.OpenSubreport("RaporSikap").SetDataSource(dtNilaiSikap);
                    if (Libs.GetStringToDecimal(tahun_ajaran.Substring(0, 4)) < 2020)
                    {
                        rpt_doc.OpenSubreport("RaporMulok").SetDataSource(dtNilaiMulok);
                    }
                    else
                    {
                        rpt_doc.OpenSubreport("rptCatatanWalas").SetDataSource(dtNilaiSaran);
                        rpt_doc.OpenSubreport("rptEkskul").SetDataSource(dtNilaiEkskul);
                        rpt_doc.OpenSubreport("rptVolunteer").SetDataSource(dtNilaiVolunteer);
                        rpt_doc.OpenSubreport("rptKetidakHadiran").SetDataSource(dtNilaiAbsensi);
                        if (!(tahun_ajaran == "2020/2021" && semester == "1"))
                        {
                            rpt_doc.OpenSubreport("CapaianKedisiplinan").SetDataSource(dtRaporLTSCapaianKedisiplinan);
                        }
                    }
                }

                CRV1.Page.Title = "RAPOR SD " +
                                  tahun_ajaran +
                                  " SEMESTER " +
                                  semester;
                CRV1.ReportSource = rpt_doc;
                rpt_doc.ExportToHttpResponse(
                        ExportFormatType.PortableDocFormat, Response, false,
                        "RAPOR SD " + tahun_ajaran.Replace("/", "-") + " SM " + semester + " KELAS " + kelas_det.ToUpper().Replace("/", "-")
                    );
                rpt_doc.Close();
                rpt_doc.Dispose();
                rpt_doc = null;
            }
        }
    }
}