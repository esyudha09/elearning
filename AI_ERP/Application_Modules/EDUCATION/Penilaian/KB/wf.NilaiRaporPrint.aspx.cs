using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Web.Services;
using System.Drawing;

using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.KB;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning.KB;

using System.Data;
using System.Data.SqlClient;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.KB
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
                    tipe_rapor = Libs.GetQueryString("tr").ToUpper().Trim();

                    _bw = new BackgroundWorker
                    {
                        WorkerReportsProgress = true,
                        WorkerSupportsCancellation = true
                    };
                    _bw.DoWork += bw_DoWork;
                    _bw.ProgressChanged += bw_ProgressChanged;
                    _bw.RunWorkerCompleted += bw_RunWorkerCompleted;

                    _bw.RunWorkerAsync();
                    //ExportToPDF();
                }
                else
                {
                    CRV1.AutoDataBind = true;
                    CRV1.Enabled = true;
                    ShowData();
                }
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

        protected string GetFileTTDGuru(string s_tahun_ajaran, string s_semester, string s_kelas)
        {
            string lokasi_ttd = Server.MapPath("~/Application_Files/Rapor/__TTD/" + s_tahun_ajaran.Replace("/", "-") + "-0" + s_semester + "/" + s_kelas + ".jpg");
            return lokasi_ttd;
        }

        public static string GetUnit()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.KB).FirstOrDefault().Kode.ToString();
        }

        protected void ExportToPDF()
        {
            ada_proses = false;

            List<Rapor_Design> lst_desain = new List<Rapor_Design>();
            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(kelas);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    string s_rel_kelas = m_kelas_det.Rel_Kelas.ToString();
                    lst_desain = DAO_Rapor_Design.GetAll_Entity().FindAll(
                            m0 => m0.TahunAjaran == tahun_ajaran &&
                                  m0.Semester == semester &&
                                  m0.Rel_Kelas == m_kelas_det.Rel_Kelas &&
                                  m0.TipeRapor.ToUpper().Trim() == tipe_rapor
                        );
                }
            }

            //get ttd
            string lokasi_ttd = GetFileTTDGuru(tahun_ajaran, semester, m_kelas_det.Nama.Trim());
            System.Drawing.Image img = null;
            string s_loc = lokasi_ttd;
            if (File.Exists(s_loc) && s_loc.Trim() != "")
            {
                img = System.Drawing.Image.FromFile(s_loc);
            }
            byte[] img_ttd_guru = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));
            //end get ttd

            int idx = 0;
            List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(GetUnit(), kelas, tahun_ajaran, semester);
            string[] arr_siswa = siswa.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item_siswa in arr_siswa)
            {
                string s_nama = "";
                Siswa m_siswa = lst_siswa.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item_siswa.Replace(";", "").ToUpper().Trim()).FirstOrDefault();
                if (m_siswa != null)
                {
                    if (m_siswa.Nama != null) s_nama = m_siswa.Nama.ToUpper().Trim();
                }

                idx++;
                ada_proses = true;
                STATUS_PROSES = "Proses data : " + idx.ToString() + " dari " + arr_siswa.Count().ToString();

                foreach (var m_desain in lst_desain)
                {
                    string lokasi_ekspor = "";
                    string lokasi_report = "";
                    if (m_desain.TipeRapor == TipeRapor.LTS)
                    {
                        lokasi_ekspor = Libs.GetLokasiFolderFileLTS(
                                item_siswa, tahun_ajaran, semester, kelas, Libs.UnitSekolah.KB
                            );
                        lokasi_report = Server.MapPath("~/Application_Reports/Penilaian/KB/" + Libs.GetNamaPeriodeReportRapor(m_desain.TahunAjaran, m_desain.Semester, "LTS.rpt"));
                    }
                    else if (m_desain.TipeRapor == TipeRapor.SEMESTER)
                    {
                        lokasi_ekspor = Libs.GetLokasiFolderFileRapor(
                                item_siswa, tahun_ajaran, semester, kelas, Libs.UnitSekolah.KB
                            );
                        lokasi_report = Server.MapPath("~/Application_Reports/Penilaian/KB/" + Libs.GetNamaPeriodeReportRapor(m_desain.TahunAjaran, m_desain.Semester, "RAPOR.rpt"));
                    }

                    if (lokasi_ekspor.Trim() != "" && lokasi_report.Trim() != "")
                    {
                        SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                        SqlCommand comm = conn.CreateCommand();
                        SqlDataAdapter sqlDA;

                        conn.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = DAO_Rapor_Nilai.SP_SELECT_BY_HEADER_DENGAN_NILAI_FOR_REPORT;
                        comm.Parameters.AddWithValue("@" + DAO_Rapor_DesignDet.NamaField.Rel_Rapor_Design, m_desain.Kode.ToString());
                        comm.Parameters.AddWithValue("@" + DAO_Rapor_NilaiSiswa.NamaField.Rel_Siswa, item_siswa);

                        DataTable dtResult = new DataTable();
                        sqlDA = new SqlDataAdapter(comm);
                        sqlDA.Fill(dtResult);

                        bool show_qrcode = true;
                        if (dtResult.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtResult.Rows.Count; i++)
                            {
                                string s_kelas = m_kelas_det.Nama;
                                string s_info_qr = "";
                                s_info_qr = "NIS = " + dtResult.Rows[i]["NIS"].ToString() + ", " +
                                            "Nama = " + dtResult.Rows[i]["Nama"].ToString().ToUpper() + ", " +
                                            "Unit = KB, " +
                                            "Tahun Pelajaran & Semester = " + m_desain.TahunAjaran + " & " + m_desain.Semester + ", " +
                                            "Kelas = " + s_kelas;
                                byte[] qr_code =
                                    (show_qrcode
                                        ? (byte[])(new ImageConverter()).ConvertTo(QRCodeGenerator.GetQRCode(s_info_qr, 20), typeof(byte[]))
                                        : null
                                    );

                                dtResult.Rows[i]["TTDGuru"] = img_ttd_guru;
                                dtResult.Rows[i]["QRCode"] = qr_code;
                            }
                        }

                        ReportDocument rpt = new ReportDocument();
                        rpt = ReportFactory.GetReport(rpt.GetType());
                        rpt.Load(lokasi_report);
                        rpt.SetDataSource(dtResult);
                        string file_name = "KB " +
                                           (
                                            m_desain.TipeRapor.Trim().ToUpper() == TipeRapor.LTS
                                            ? "LTS "
                                            : "SEMESTER "
                                           ) +
                                           semester +
                                           " " +
                                           tahun_ajaran +
                                           " " +
                                           s_nama +
                                           (
                                            m_desain.JenisRapor == "Sosial Emosi"
                                            ? " 1"
                                            : " 2"
                                           );
                        ExportToPDF(rpt, lokasi_ekspor, file_name.Replace("/", "-"));

                        rpt.Close();
                        rpt.Dispose();
                        conn.Close();
                        comm.Dispose();
                        conn.Dispose();
                    }

                    break;
                }
            }
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            STATUS_PROSES = "Sedang proses...";
            if (_bw.CancellationPending) { e.Cancel = true; return; }
            _bw.ReportProgress(0);
            ada_proses = false;

            List<Rapor_Design> lst_desain = new List<Rapor_Design>();
            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(kelas);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    string s_rel_kelas = m_kelas_det.Rel_Kelas.ToString();
                    lst_desain = DAO_Rapor_Design.GetAll_Entity().FindAll(
                            m0 => m0.TahunAjaran == tahun_ajaran &&
                                  m0.Semester == semester &&
                                  m0.Rel_Kelas == m_kelas_det.Rel_Kelas &&
                                  m0.TipeRapor.ToUpper().Trim() == tipe_rapor
                        );
                }
            }

            //get ttd
            string lokasi_ttd = GetFileTTDGuru(tahun_ajaran, semester, m_kelas_det.Nama.Trim());
            System.Drawing.Image img = null;
            string s_loc = lokasi_ttd;
            if (File.Exists(s_loc) && s_loc.Trim() != "")
            {
                img = System.Drawing.Image.FromFile(s_loc);
            }
            byte[] img_ttd_guru = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));
            //end get ttd

            List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(GetUnit(), kelas, tahun_ajaran, semester);
            int idx = 0;
            string[] arr_siswa = siswa.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item_siswa in arr_siswa)
            {
                string s_nama = "";
                Siswa m_siswa = lst_siswa.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item_siswa.Replace(";", "").ToUpper().Trim()).FirstOrDefault();
                if (m_siswa != null)
                {
                    if (m_siswa.Nama != null) s_nama = m_siswa.Nama.ToUpper().Trim();
                }

                idx++;
                ada_proses = true;
                STATUS_PROSES = "Proses data : " + idx.ToString() + " dari " + arr_siswa.Count().ToString();
                if (_bw.CancellationPending) { e.Cancel = true; return; }
                _bw.ReportProgress(idx);

                foreach (var m_desain in lst_desain)
                {
                    string lokasi_ekspor = "";
                    string lokasi_report = "";
                    if (m_desain.TipeRapor == TipeRapor.LTS)
                    {
                        lokasi_ekspor = Libs.GetLokasiFolderFileLTS(
                                item_siswa, tahun_ajaran, semester, kelas, Libs.UnitSekolah.KB
                            );
                        lokasi_report = Server.MapPath("~/Application_Reports/Penilaian/KB/" + Libs.GetNamaPeriodeReportRapor(m_desain.TahunAjaran, m_desain.Semester, "LTS.rpt"));
                    }
                    else if (m_desain.TipeRapor == TipeRapor.SEMESTER)
                    {
                        lokasi_ekspor = Libs.GetLokasiFolderFileRapor(
                                item_siswa, tahun_ajaran, semester, kelas, Libs.UnitSekolah.KB
                            );
                        lokasi_report = Server.MapPath("~/Application_Reports/Penilaian/KB/" + Libs.GetNamaPeriodeReportRapor(m_desain.TahunAjaran, m_desain.Semester, "RAPOR.rpt"));
                    }

                    if (lokasi_ekspor.Trim() != "" && lokasi_report.Trim() != "")
                    {
                        SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                        SqlCommand comm = conn.CreateCommand();
                        SqlDataAdapter sqlDA;

                        conn.Open();
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = DAO_Rapor_Nilai.SP_SELECT_BY_HEADER_DENGAN_NILAI_FOR_REPORT;
                        comm.Parameters.AddWithValue("@" + DAO_Rapor_DesignDet.NamaField.Rel_Rapor_Design, m_desain.Kode.ToString());
                        comm.Parameters.AddWithValue("@" + DAO_Rapor_NilaiSiswa.NamaField.Rel_Siswa, item_siswa);

                        DataTable dtResult = new DataTable();
                        sqlDA = new SqlDataAdapter(comm);
                        sqlDA.Fill(dtResult);

                        bool show_qrcode = true;
                        if (dtResult.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtResult.Rows.Count; i++)
                            {
                                string s_kelas = m_kelas_det.Nama;
                                string s_info_qr = "";
                                s_info_qr = "NIS = " + dtResult.Rows[i]["NIS"].ToString() + ", " +
                                            "Nama = " + dtResult.Rows[i]["Nama"].ToString().ToUpper() + ", " +
                                            "Unit = KB, " +
                                            "Tahun Pelajaran & Semester = " + m_desain.TahunAjaran + " & " + m_desain.Semester + ", " +
                                            "Kelas = " + s_kelas;
                                byte[] qr_code =
                                    (show_qrcode
                                        ? (byte[])(new ImageConverter()).ConvertTo(QRCodeGenerator.GetQRCode(s_info_qr, 20), typeof(byte[]))
                                        : null
                                    );

                                dtResult.Rows[i]["TTDGuru"] = img_ttd_guru;
                                dtResult.Rows[i]["QRCode"] = qr_code;
                            }
                        }

                        ReportDocument rpt = new ReportDocument();
                        rpt = ReportFactory.GetReport(rpt.GetType());
                        rpt.Load(lokasi_report);
                        rpt.SetDataSource(dtResult);
                        string file_name = "KB " +
                                           (
                                            m_desain.TipeRapor.Trim().ToUpper() == TipeRapor.LTS
                                            ? "LTS "
                                            : "SEMESTER "
                                           ) +
                                           semester +
                                           " " +
                                           tahun_ajaran +
                                           " " +
                                           s_nama +
                                           (
                                            m_desain.JenisRapor == "Sosial Emosi"
                                            ? " 1"
                                            : " 2"
                                           );
                        ExportToPDF(rpt, lokasi_ekspor, file_name.Replace("/", "-"));


                        rpt.Close();
                        rpt.Dispose();
                        conn.Close();
                        comm.Dispose();
                        conn.Dispose();
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

        protected void ShowData()
        {
            string s = Libs.GetQueryString("sis");
            string j = Libs.GetQueryString("j");
            string kds = Libs.GetQueryString("kds");
            string[] arr_siswa = s.Split(new string[] { ";" }, StringSplitOptions.None);
            string rel_siswa = "";
            int id = 1;
            foreach (var siswa in arr_siswa)
            {
                if (siswa.Trim() != "")
                {
                    rel_siswa = siswa.Trim();
                    id++;
                }
            }

            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            Rapor_Design m_desain = DAO_Rapor_Design.GetByID_Entity(kds);
            if (m_desain != null)
            {
                if (m_desain.TahunAjaran != null)
                {
                    string lokasi_report = "";
                    if (m_desain.TipeRapor == TipeRapor.LTS)
                    {
                        lokasi_report = Server.MapPath("~/Application_Reports/Penilaian/KB/" + Libs.GetNamaPeriodeReportRapor(m_desain.TahunAjaran, m_desain.Semester, "LTS.rpt"));
                    }
                    else if (m_desain.TipeRapor == TipeRapor.SEMESTER)
                    {
                        lokasi_report = Server.MapPath("~/Application_Reports/Penilaian/KB/" + Libs.GetNamaPeriodeReportRapor(m_desain.TahunAjaran, m_desain.Semester, "RAPOR.rpt"));
                    }

                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(QS.GetKelas());
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            //string lokasi_ttd = "";
                            string lokasi_ttd = GetFileTTDGuru(m_desain.TahunAjaran, m_desain.Semester, m_kelas_det.Nama.Trim());
                            System.Drawing.Image img = null;
                            string s_loc = lokasi_ttd;
                            if (File.Exists(s_loc) && s_loc.Trim() != "")
                            {
                                img = System.Drawing.Image.FromFile(s_loc);
                            }
                            byte[] img_ttd_guru = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));
                            //end get ttd

                            conn.Open();
                            comm.CommandType = CommandType.StoredProcedure;
                            comm.CommandText = DAO_Rapor_Nilai.SP_SELECT_BY_HEADER_DENGAN_NILAI_FOR_REPORT;
                            comm.Parameters.AddWithValue("@" + DAO_Rapor_DesignDet.NamaField.Rel_Rapor_Design, kds);
                            comm.Parameters.AddWithValue("@" + DAO_Rapor_NilaiSiswa.NamaField.Rel_Siswa, rel_siswa);

                            DataTable dtResult = new DataTable();
                            sqlDA = new SqlDataAdapter(comm);
                            sqlDA.Fill(dtResult);

                            bool show_qrcode = true;
                            if (dtResult.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtResult.Rows.Count; i++)
                                {
                                    string s_kelas = m_kelas_det.Nama;
                                    string s_info_qr = "";
                                    s_info_qr = "NIS = " + dtResult.Rows[i]["NIS"].ToString() + ", " +
                                                "Nama = " + dtResult.Rows[i]["Nama"].ToString().ToUpper() + ", " +
                                                "Unit = KB, " +
                                                "Tahun Pelajaran & Semester = " + m_desain.TahunAjaran + " & " + m_desain.Semester + ", " +
                                                "Kelas = " + s_kelas;
                                    byte[] qr_code =
                                        (show_qrcode
                                            ? (byte[])(new ImageConverter()).ConvertTo(QRCodeGenerator.GetQRCode(s_info_qr, 20), typeof(byte[]))
                                            : null
                                        );

                                    dtResult.Rows[i]["TTDGuru"] = img_ttd_guru;
                                    dtResult.Rows[i]["QRCode"] = qr_code;
                                }
                            }

                            ReportDocument rpt = new ReportDocument();
                            rpt = ReportFactory.GetReport(rpt.GetType());
                            rpt.Load(lokasi_report);
                            rpt.SetDataSource(dtResult);
                            CRV1.Page.Title = "RAPOR KB " +
                                              m_desain.TahunAjaran +
                                              " SEMESTER " +
                                              m_desain.Semester;
                            CRV1.ReportSource = rpt;
                            rpt.ExportToHttpResponse(
                                    ExportFormatType.PortableDocFormat, Response, false, "RAPOR KB " + m_desain.TahunAjaran.Replace("/", "-")
                                );


                            rpt.Close();
                            rpt.Dispose();
                        }
                    }
                }
            }
        }
    }
}