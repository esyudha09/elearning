using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.Drawing;
using System.IO;
using System.Web.UI;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_Entities.Elearning.SD.Reports;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.Application_Libs
{
    public static class Reports_SD
    {
        public static void NilaiRapor_KTSP(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa = "", string lokasi_ttd = "")
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

            List<KTSP_RaporNilai> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KTSP(tahun_ajaran, semester, rel_kelas_det, rel_siswa, lokasi_ttd);
            DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);
            
            ReportDocument rpt_doc = new ReportDocument();
            rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());

            rpt_doc.Load(HttpContext.Current.Server.MapPath("~/Application_Reports/SD/RaporKTSP.rpt"));
            rpt_doc.SetDataSource(dtNilai);

            Stream stream = null;
            string content_type = "";
            content_type = "applications/pdf";

            rpt_doc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, http_response, true, ("Rapor" + tahun_ajaran + "SM" + semester + "" + kelas_det.Replace("-", "") + "" + DateTime.Now.ToString("yyyyMMddHHmmss")).Replace("/", "").Replace("\\", ""));
            stream = rpt_doc.ExportToStream(ExportFormatType.PortableDocFormat);

            byte[] byteArray = new byte[stream.Length];
            stream.Read(byteArray, 0, Convert.ToInt32(stream.Length - 1));

            rpt_doc.Close();
            rpt_doc.Dispose();

            http_response.ClearContent();
            http_response.ClearHeaders();
            http_response.ContentType = content_type;
            http_response.BinaryWrite(byteArray);
            http_response.Flush();
            http_response.Close();
            http_response.End();
        }

        public static Sekolah GetUnitSekolah()
        {
            Sekolah m_unit = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SD).FirstOrDefault();
            return m_unit;
        }

        public static void UraianRapor_KTSP(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa = "")
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

            List<KTSP_UraianKompetensi> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiUraianKompetensi_KTSP(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
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

            rpt_doc.Load(HttpContext.Current.Server.MapPath("~/Application_Reports/SD/RaporUraianKTSP.rpt"));
            
            rpt_doc.SetDataSource(dtNilai);
            if (dtNilai.Rows.Count > 0)
            {
                rpt_doc.OpenSubreport("RaporSikapKTSP").SetDataSource(dtNilaiSikap);
                rpt_doc.OpenSubreport("RaporSaranKTSP").SetDataSource(dtNilaiSaran);
                rpt_doc.OpenSubreport("RaporEkskulKTSP").SetDataSource(dtNilaiEkskul);
                rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtNilaiVolunteer);
                rpt_doc.OpenSubreport("RaporKetidakhadiran").SetDataSource(dtNilaiAbsensi);
            }

            Stream stream = null;
            string content_type = "";
            content_type = "applications/pdf";

            rpt_doc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, http_response, true, ("RaporUraian.PDF"));
            stream = rpt_doc.ExportToStream(ExportFormatType.PortableDocFormat);

            byte[] byteArray = new byte[stream.Length];
            stream.Read(byteArray, 0, Convert.ToInt32(stream.Length - 1));

            rpt_doc.Close();
            rpt_doc.Dispose();

            http_response.ClearContent();
            http_response.ClearHeaders();
            http_response.ContentType = content_type;
            http_response.BinaryWrite(byteArray);
            http_response.Flush();
            http_response.Close();
            http_response.End();
        }

        public static void UraianRapor_KURTILAS(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa = "")
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

            List<KTSP_UraianKompetensi> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiUraianKompetensi_KTSP(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

            //List<KTSP_RaporSikap> lst_nilai_sikap = DAO_Rapor_Semester.GetNilaiSikap_KTSP(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            //DataTable dtNilaiSikap = Libs.ToDataTable(lst_nilai_sikap);

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

            rpt_doc.Load(HttpContext.Current.Server.MapPath("~/Application_Reports/SD/RaporUraianKURTILAS.rpt"));

            rpt_doc.SetDataSource(dtNilai);
            if (dtNilai.Rows.Count > 0)
            {
                //rpt_doc.OpenSubreport("RaporSikapKTSP").SetDataSource(dtNilaiSikap);
                rpt_doc.OpenSubreport("RaporSaranKTSP").SetDataSource(dtNilaiSaran);
                rpt_doc.OpenSubreport("RaporEkskulKTSP").SetDataSource(dtNilaiEkskul);
                rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtNilaiVolunteer);
                rpt_doc.OpenSubreport("RaporKetidakhadiran").SetDataSource(dtNilaiAbsensi);
            }

            Stream stream = null;
            string content_type = "";
            content_type = "applications/pdf";

            rpt_doc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, http_response, true, ("RaporUraian.PDF"));
            stream = rpt_doc.ExportToStream(ExportFormatType.PortableDocFormat);

            byte[] byteArray = new byte[stream.Length];
            stream.Read(byteArray, 0, Convert.ToInt32(stream.Length - 1));

            rpt_doc.Close();
            rpt_doc.Dispose();

            http_response.ClearContent();
            http_response.ClearHeaders();
            http_response.ContentType = content_type;
            http_response.BinaryWrite(byteArray);
            http_response.Flush();
            http_response.Close();
            http_response.End();
        }

        public static class ReportLibs
        {
            public static bool IsTematik(string nama_mapel)
            {
                if (
                    nama_mapel.Trim().ToLower().IndexOf("pancasila") >= 0 ||
                    nama_mapel.Trim().ToLower().IndexOf("warga") >= 0 ||
                    nama_mapel.Trim().ToLower().IndexOf("ppkn") >= 0 ||

                    nama_mapel.Trim().ToLower().IndexOf("bahasa indonesia") >= 0 ||
                    nama_mapel.Trim().ToLower().IndexOf("matematika") >= 0 ||
                    nama_mapel.Trim().ToLower().IndexOf("pengetahuan alam") >= 0 ||
                    nama_mapel.Trim().ToLower().IndexOf("ipa") >= 0 ||
                    nama_mapel.Trim().ToLower().IndexOf("pengetahuan sosial") >= 0 ||
                    nama_mapel.Trim().ToLower().IndexOf("ips") >= 0
                ) return true;
                return false;
            }
        }

        public class LTS
        {
            public string TahunAjaran;
            public string Semester;
            public string Rel_KelasDet;
            public string Rel_Siswa;
            public string GetHTML;

            public List<RaporLTS> GetRaporLTS;
            public List<RaporLTSDeskripsi> GetRaporLTSDeskripsi;
            public List<RaporLTSCapaianKedisiplinan> GetRaporLTSCapaianKedisiplinan;

            public LTS(string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa, string s_lokasi_ttd, bool show_qrcode = true)
            {
                System.Drawing.Image img = null;
                string s_loc = s_lokasi_ttd;
                if (File.Exists(s_loc) && s_loc.Trim() != "")
                {
                    img = System.Drawing.Image.FromFile(s_loc);
                }
                byte[] img_ttd_guru = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));

                List<RaporLTS> lst_hasil_lts = new List<RaporLTS>();
                List<RaporLTSDeskripsi> lst_hasil_deksripsi = new List<RaporLTSDeskripsi>();
                List<RaporLTSCapaianKedisiplinan> lst_hasil_capaian_kedisiplinan = new List<RaporLTSCapaianKedisiplinan>();

                Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                        m0 => m0.TahunAjaran == tahun_ajaran &&
                              m0.Semester == semester &&
                              m0.JenisRapor == "LTS"

                    ).FirstOrDefault();

                List<FormasiGuruKelas> lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                            GetUnitSekolah().Kode.ToString(), tahun_ajaran, semester
                        ).FindAll(m => m.Rel_KelasDet.ToString().ToUpper() == rel_kelas_det.Trim().ToUpper());

                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());

                        if (m_kelas != null)
                        {

                            if (m_kelas.Nama != null)
                            {
                                string s_nama_guru = "";
                                string s_tanggal = "";

                                List<Rapor_Desain_Det> lst_desain_rapor_det_ = DAO_Rapor_Desain_Det.GetAll_Entity();

                                Rapor_LTS_MengetahuiGuruKelas m_mengetahui = DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FirstOrDefault();
                                if (DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).Count > 0)
                                {
                                    s_nama_guru = m_mengetahui.NamaGuru;
                                    s_tanggal = Libs.GetTanggalIndonesiaFromDate(m_mengetahui.Tanggal, false);
                                }

                                if (s_nama_guru.Trim() == "")
                                {
                                    if (lst_formasi_guru_kelas != null)
                                    {
                                        if (lst_formasi_guru_kelas.Count > 0)
                                        {
                                            FormasiGuruKelas m_guru_kelas = lst_formasi_guru_kelas.FirstOrDefault();
                                            if (m_guru_kelas != null)
                                            {
                                                if (m_guru_kelas.TahunAjaran != null)
                                                {
                                                    Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m_guru_kelas.Rel_GuruKelas);
                                                    if (m_pegawai != null)
                                                    {
                                                        if (m_pegawai.Nama != null)
                                                        {
                                                            s_nama_guru = m_pegawai.Nama;

                                                            if (m_rapor_arsip != null)
                                                            {
                                                                if (m_rapor_arsip.JenisRapor != null)
                                                                {
                                                                    s_tanggal = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    rel_kelas_det,
                                    tahun_ajaran,
                                    semester
                                );

                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_det =
                                    DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det);
                                List<Rapor_NilaiSiswa> lst_nilaisiswa =
                                    DAO_Rapor_NilaiSiswa.GetAllByTABySMByKelasDet_ForReport_Entity(tahun_ajaran, semester, rel_kelas_det);

                                List<Rapor_LTS_Mapel_Ext> lst_rapor_lts_mapel = DAO_Rapor_LTS_Mapel.GetByTABySMByKelasDet(tahun_ajaran, semester, rel_kelas_det);
                                List<Rapor_AspekPenilaian> lst_rapor_ap_all = DAO_Rapor_AspekPenilaian.GetAll_Entity();
                                List<Rapor_KompetensiDasar> lst_rapor_kd_all = DAO_Rapor_KompetensiDasar.GetAll_Entity();
                                List<Rapor_StrukturNilai_AP> lst_sn_ap_all = DAO_Rapor_StrukturNilai_AP.GetAll_Entity();
                                List<Rapor_StrukturNilai_KD> lst_sn_kd_all = DAO_Rapor_StrukturNilai_KD.GetAll_Entity();
                                List<Rapor_StrukturNilai> lst_sn_all = DAO_Rapor_StrukturNilai.GetAllByTABySM_Entity(tahun_ajaran, semester);

                                List<Rapor_Desain> lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                            tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.LTS
                                        );

                                if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m0 => (rel_siswa + ";").Trim().ToUpper().IndexOf(m0.Kode.ToString().ToUpper() + ";") >= 0).ToList();
                                foreach (var m_siswa in lst_siswa)
                                {
                                    int nomor_mapel = 0;
                                    int urut_mapel = 0;
                                    int jumlah_tematik = 0;
                                    bool ada_tematik = false;
                                    string s_info_qr = "";
                                    s_info_qr = "NIS = " + m_siswa.NISSekolah + ", " +
                                                "Nama = " + m_siswa.Nama.Trim().ToUpper() + ", " +
                                                "Unit = SD, " +
                                                "Tahun Pelajaran & Semester = " + tahun_ajaran + " & " + semester + ", " +
                                                "Kelas = " + m_kelas_det.Nama;
                                    byte[] qr_code =
                                        (show_qrcode
                                            ? (byte[])(new ImageConverter()).ConvertTo(QRCodeGenerator.GetQRCode(s_info_qr, 20), typeof(byte[]))
                                            : null
                                        );

                                    //absen
                                    string s_sakit = "-";
                                    string s_izin = "-";
                                    string s_alpa = "-";
                                    string s_terlambat = "-";

                                    List<DAO_SiswaAbsen.AbsenRekapRapor> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRapor>();
                                    lst_absen = DAO_SiswaAbsen.GetRekapAbsenRaporBySiswaByPeriode_Entity(
                                            m_siswa.Kode.ToString(),
                                            (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                            (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                        );
                                    foreach (var absen in lst_absen)
                                    {
                                        if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                    }
                                    //end absen

                                    if (lst_desain_rapor.Count == 1)
                                    {
                                        Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                                        if (m_rapor_desain != null)
                                        {
                                            if (m_rapor_desain.TahunAjaran != null)
                                            {
                                                int jml_kolom = 6;
                                                
                                                List<Rapor_Desain_Det> lst_desain_rapor_det = lst_desain_rapor_det_.FindAll(m0 => m0.Rel_Rapor_Desain.ToString().ToUpper().Trim() == m_rapor_desain.Kode.ToString().ToUpper().Trim()).OrderBy(m => m.Urutan).ToList();
                                                int id = 1;
                                                foreach (var m_desain in lst_desain_rapor_det)
                                                {
                                                    List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det =
                                                        lst_nilai_det.FindAll(
                                                                m0 => m0.Rel_Mapel == m_desain.Rel_Mapel &&
                                                                      m0.Rel_Siswa == m_siswa.Kode.ToString()
                                                            ).ToList();
                                                    List<Rapor_LTS_Mapel_Ext> lst_rapor_lts_mapel_ =
                                                        lst_rapor_lts_mapel.FindAll(
                                                                m0 => m0.Rel_Mapel == m_desain.Rel_Mapel
                                                            ).ToList().OrderBy(m0 => m0.Urutan).ToList();

                                                    List<string> lst_nilai_pengetahuan = new List<string>();
                                                    List<string> lst_nilai_pengetahuan_des = new List<string>();

                                                    List<string> lst_nilai_keterampilan = new List<string>();
                                                    List<string> lst_nilai_keterampilan_des = new List<string>();

                                                    decimal d_kkm = 0;
                                                    string rel_rapornilaisiswa = "";

                                                    foreach (var item_lst_rapor_lts_mapel_ in lst_rapor_lts_mapel_)
                                                    {
                                                        bool ada_item = false;
                                                        Rapor_StrukturNilai_AP m_sn_ap = lst_sn_ap_all.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_AP.ToString().ToUpper().Trim()).FirstOrDefault();
                                                        Rapor_StrukturNilai_KD m_sn_kd = lst_sn_kd_all.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_KD.ToString().ToUpper().Trim()).FirstOrDefault();

                                                        foreach (var nilai_siswa_det in lst_nilai_siswa_det.FindAll(
                                                            m0 => m0.Rel_Rapor_StrukturNilai_AP == item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_AP &&
                                                                  m0.Rel_Rapor_StrukturNilai_KD == item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_KD &&
                                                                  m0.Rel_Rapor_StrukturNilai_KP == item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_KP
                                                        ))
                                                        {
                                                            if (m_sn_ap != null)
                                                            {
                                                                if (m_sn_ap.Poin != null)
                                                                {
                                                                    Rapor_StrukturNilai m_sn = lst_sn_all.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == m_sn_ap.Rel_Rapor_StrukturNilai.ToString().ToUpper().Trim()).FirstOrDefault();
                                                                    if (m_sn != null)
                                                                    {
                                                                        if (m_sn.TahunAjaran != null)
                                                                        {
                                                                            d_kkm = m_sn.KKM;
                                                                        }
                                                                    }

                                                                    Rapor_AspekPenilaian m_ap = lst_rapor_ap_all.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == m_sn_ap.Rel_Rapor_AspekPenilaian.ToString().ToUpper().Trim()).FirstOrDefault();
                                                                    Rapor_KompetensiDasar m_kd = lst_rapor_kd_all.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == m_sn_kd.Rel_Rapor_KompetensiDasar.ToString().ToUpper().Trim()).FirstOrDefault();

                                                                    if (m_ap != null)
                                                                    {
                                                                        if (m_ap.Nama != null)
                                                                        {
                                                                            ada_item = true;
                                                                            rel_rapornilaisiswa = nilai_siswa_det.Rel_Rapor_NilaiSiswa.ToString();
                                                                            if (Libs.GetHTMLSimpleText(m_ap.Nama).Trim().ToUpper() == "PENGETAHUAN")
                                                                            {
                                                                                lst_nilai_pengetahuan.Add(nilai_siswa_det.Nilai);
                                                                                //lst_nilai_pengetahuan_des.Add(m_kd.Nama);
                                                                                lst_nilai_pengetahuan_des.Add(
                                                                                    Libs.GetHTMLSimpleText3(
                                                                                        m_sn_kd.Deskripsi
                                                                                    ));
                                                                            }
                                                                            else if (Libs.GetHTMLSimpleText(m_ap.Nama).Trim().ToUpper() == "KETERAMPILAN")
                                                                            {
                                                                                lst_nilai_keterampilan.Add(nilai_siswa_det.Nilai);
                                                                                //lst_nilai_keterampilan_des.Add(m_kd.Nama);
                                                                                lst_nilai_keterampilan_des.Add(
                                                                                    Libs.GetHTMLSimpleText3(
                                                                                        m_sn_kd.Deskripsi
                                                                                    ));
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (!ada_item)
                                                        {
                                                            if (m_sn_ap != null)
                                                            {
                                                                if (m_sn_ap.Poin != null)
                                                                {
                                                                    Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(m_sn_ap.Rel_Rapor_AspekPenilaian.ToString());
                                                                    if (m_ap != null)
                                                                    {
                                                                        if (m_ap.Nama != null)
                                                                        {
                                                                            ada_item = true;
                                                                            if (Libs.GetHTMLSimpleText(m_ap.Nama).Trim().ToUpper() == "PENGETAHUAN")
                                                                            {
                                                                                lst_nilai_pengetahuan.Add("");
                                                                            }
                                                                            else if (Libs.GetHTMLSimpleText(m_ap.Nama).Trim().ToUpper() == "KETERAMPILAN")
                                                                            {
                                                                                lst_nilai_keterampilan.Add("");
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    RaporLTS m_rapor_lts = new RaporLTS();
                                                    m_rapor_lts.TahunAjaran = tahun_ajaran;
                                                    m_rapor_lts.Semester = semester;
                                                    m_rapor_lts.Kelas = m_kelas_det.Nama;
                                                    m_rapor_lts.Rel_Siswa = m_siswa.Kode.ToString();
                                                    m_rapor_lts.NIS = m_siswa.NIS;
                                                    m_rapor_lts.NISN = m_siswa.NISN;
                                                    m_rapor_lts.Nama = m_siswa.Nama;
                                                    m_rapor_lts.KodeKelompokMapel = "";
                                                    m_rapor_lts.KelompokMapel = "";
                                                    m_rapor_lts.NomorMapel = m_desain.Nomor;
                                                    m_rapor_lts.Rel_Mapel = m_desain.Rel_Mapel;
                                                    m_rapor_lts.NamaMapel = (m_desain.Poin.Trim() != "" ? m_desain.Poin + " " : "") + m_desain.NamaMapelRapor;
                                                    m_rapor_lts.KKM = Math.Round(d_kkm).ToString();
                                                    m_rapor_lts.T13 = "";
                                                    m_rapor_lts.T14 = "";
                                                    m_rapor_lts.T15 = "";
                                                    m_rapor_lts.HD = "";
                                                    m_rapor_lts.LK = "";
                                                    m_rapor_lts.RJ = "";
                                                    m_rapor_lts.RPKB = "";
                                                    m_rapor_lts.NamaEkskul1 = "";
                                                    m_rapor_lts.HadirEkskul1 = "";
                                                    m_rapor_lts.NamaEkskul2 = "";
                                                    m_rapor_lts.HadirEkskul2 = "";
                                                    m_rapor_lts.NamaEkskul3 = "";
                                                    m_rapor_lts.HadirEkskul3 = "";
                                                    m_rapor_lts.Sakit = s_sakit;
                                                    m_rapor_lts.Izin = s_izin;
                                                    m_rapor_lts.Alpa = s_alpa;
                                                    m_rapor_lts.TanggalRapor = s_tanggal;
                                                    m_rapor_lts.WaliKelas = s_nama_guru;
                                                    m_rapor_lts.UrutanMapel = m_desain.Urutan;
                                                    m_rapor_lts.TTDGuru = img_ttd_guru;
                                                    m_rapor_lts.QRCode = qr_code;

                                                    RaporLTSDeskripsi m_rapor_lts_des = new RaporLTSDeskripsi();
                                                    m_rapor_lts_des.Rel_Siswa = m_siswa.Kode.ToString();
                                                    m_rapor_lts_des.TahunAjaran = tahun_ajaran;
                                                    m_rapor_lts_des.Semester = semester;
                                                    m_rapor_lts_des.Rel_Mapel = m_desain.Rel_Mapel;
                                                    m_rapor_lts_des.Nama = m_siswa.Nama;
                                                    m_rapor_lts_des.NamaMapel = (m_desain.Poin.Trim() != "" ? m_desain.Poin + " " : "") + m_desain.NamaMapelRapor;
                                                    m_rapor_lts_des.NomorMapel = m_desain.Nomor;
                                                    m_rapor_lts_des.KodeKelompokMapel = "";
                                                    m_rapor_lts_des.KelompokMapel = "";
                                                    m_rapor_lts_des.UrutanMapel = m_desain.Urutan;

                                                    //capaian kedisiplinan
                                                    Rapor_NilaiSiswa m_nilaisiswa = lst_nilaisiswa.FindAll(
                                                                m0 => m0.Kode.ToString().ToUpper().Trim() == rel_rapornilaisiswa.ToUpper().Trim()
                                                            ).FirstOrDefault();
                                                    bool ada_nilai = false;
                                                    RaporLTSCapaianKedisiplinan m_rapor_lts_capaian_kedisiplinan = new RaporLTSCapaianKedisiplinan();

                                                    if (m_nilaisiswa != null)
                                                    {
                                                        if (m_nilaisiswa.Rel_Siswa != null)
                                                        {
                                                            ada_nilai = true;
                                                            
                                                            if (ReportLibs.IsTematik(m_desain.NamaMapelRapor))
                                                            {
                                                                if (!ada_tematik)
                                                                {
                                                                    urut_mapel++;
                                                                    nomor_mapel++;
                                                                    ada_tematik = true;

                                                                    m_rapor_lts_capaian_kedisiplinan.Rel_Siswa = m_nilaisiswa.Rel_Siswa;
                                                                    m_rapor_lts_capaian_kedisiplinan.KodeKelompokMapel = "";
                                                                    m_rapor_lts_capaian_kedisiplinan.KelompokMapel = "tematik";
                                                                    m_rapor_lts_capaian_kedisiplinan.NomorMapel = nomor_mapel.ToString();
                                                                    m_rapor_lts_capaian_kedisiplinan.Rel_Mapel = "";
                                                                    m_rapor_lts_capaian_kedisiplinan.NamaMapel = "Tematik:";
                                                                    m_rapor_lts_capaian_kedisiplinan.Kehadiran = "";
                                                                    m_rapor_lts_capaian_kedisiplinan.KetepatanWaktu = "";
                                                                    m_rapor_lts_capaian_kedisiplinan.PenggunaanSeragam = "";
                                                                    m_rapor_lts_capaian_kedisiplinan.PenggunaanKamera = "";
                                                                    m_rapor_lts_capaian_kedisiplinan.UrutanMapel = urut_mapel;
                                                                    lst_hasil_capaian_kedisiplinan.Add(m_rapor_lts_capaian_kedisiplinan);
                                                                }

                                                                jumlah_tematik++;
                                                                urut_mapel++;

                                                                m_rapor_lts_capaian_kedisiplinan = new RaporLTSCapaianKedisiplinan();
                                                                m_rapor_lts_capaian_kedisiplinan.Rel_Siswa = m_nilaisiswa.Rel_Siswa;
                                                                m_rapor_lts_capaian_kedisiplinan.KodeKelompokMapel = "";
                                                                m_rapor_lts_capaian_kedisiplinan.KelompokMapel = "tematik";
                                                                m_rapor_lts_capaian_kedisiplinan.NomorMapel = "";
                                                                m_rapor_lts_capaian_kedisiplinan.Rel_Mapel = m_desain.Rel_Mapel;
                                                                m_rapor_lts_capaian_kedisiplinan.NamaMapel = (
                                                                                                                jumlah_tematik <= Application_Libs.Constantas.ALFABET.Length
                                                                                                                ? Application_Libs.Constantas.ALFABET.ToLower().Substring(jumlah_tematik - 1, 1) + ". "
                                                                                                                : ""
                                                                                                             ) + m_desain.NamaMapelRapor;
                                                                m_rapor_lts_capaian_kedisiplinan.Kehadiran = m_nilaisiswa.LTS_CK_KEHADIRAN;
                                                                m_rapor_lts_capaian_kedisiplinan.KetepatanWaktu = m_nilaisiswa.LTS_CK_KETEPATAN_WKT;
                                                                m_rapor_lts_capaian_kedisiplinan.PenggunaanSeragam = m_nilaisiswa.LTS_CK_PENGGUNAAN_SRGM;
                                                                m_rapor_lts_capaian_kedisiplinan.PenggunaanKamera = m_nilaisiswa.LTS_CK_PENGGUNAAN_KMR;
                                                                m_rapor_lts_capaian_kedisiplinan.UrutanMapel = urut_mapel;
                                                                lst_hasil_capaian_kedisiplinan.Add(m_rapor_lts_capaian_kedisiplinan);
                                                            }
                                                            else
                                                            {
                                                                urut_mapel++;
                                                                if(m_desain.Poin.Trim() == "") nomor_mapel++;

                                                                m_rapor_lts_capaian_kedisiplinan.Rel_Siswa = m_nilaisiswa.Rel_Siswa;
                                                                m_rapor_lts_capaian_kedisiplinan.KodeKelompokMapel = "";
                                                                m_rapor_lts_capaian_kedisiplinan.KelompokMapel = "";
                                                                m_rapor_lts_capaian_kedisiplinan.NomorMapel = (m_desain.Poin.Trim() != "" ? "" : nomor_mapel.ToString());
                                                                m_rapor_lts_capaian_kedisiplinan.Rel_Mapel = m_desain.Rel_Mapel;
                                                                m_rapor_lts_capaian_kedisiplinan.NamaMapel = (m_desain.Poin.Trim() != "" ? m_desain.Poin + " " : "") + m_desain.NamaMapelRapor;
                                                                m_rapor_lts_capaian_kedisiplinan.Kehadiran = m_nilaisiswa.LTS_CK_KEHADIRAN;
                                                                m_rapor_lts_capaian_kedisiplinan.KetepatanWaktu = m_nilaisiswa.LTS_CK_KETEPATAN_WKT;
                                                                m_rapor_lts_capaian_kedisiplinan.PenggunaanSeragam = m_nilaisiswa.LTS_CK_PENGGUNAAN_SRGM;
                                                                m_rapor_lts_capaian_kedisiplinan.PenggunaanKamera = m_nilaisiswa.LTS_CK_PENGGUNAAN_KMR;
                                                                m_rapor_lts_capaian_kedisiplinan.UrutanMapel = urut_mapel;
                                                                lst_hasil_capaian_kedisiplinan.Add(m_rapor_lts_capaian_kedisiplinan);
                                                            }
                                                        }
                                                    }
                                                    if (!ada_nilai)
                                                    {
                                                        ada_nilai = true;

                                                        if (ReportLibs.IsTematik(m_desain.NamaMapelRapor))
                                                        {
                                                            if (!ada_tematik)
                                                            {
                                                                urut_mapel++;
                                                                nomor_mapel++;
                                                                ada_tematik = true;

                                                                m_rapor_lts_capaian_kedisiplinan.Rel_Siswa = m_siswa.Kode.ToString();
                                                                m_rapor_lts_capaian_kedisiplinan.KodeKelompokMapel = "";
                                                                m_rapor_lts_capaian_kedisiplinan.KelompokMapel = "tematik";
                                                                m_rapor_lts_capaian_kedisiplinan.NomorMapel = nomor_mapel.ToString();
                                                                m_rapor_lts_capaian_kedisiplinan.Rel_Mapel = "";
                                                                m_rapor_lts_capaian_kedisiplinan.NamaMapel = "Tematik:";
                                                                m_rapor_lts_capaian_kedisiplinan.Kehadiran = "";
                                                                m_rapor_lts_capaian_kedisiplinan.KetepatanWaktu = "";
                                                                m_rapor_lts_capaian_kedisiplinan.PenggunaanSeragam = "";
                                                                m_rapor_lts_capaian_kedisiplinan.PenggunaanKamera = "";
                                                                m_rapor_lts_capaian_kedisiplinan.UrutanMapel = urut_mapel;
                                                                lst_hasil_capaian_kedisiplinan.Add(m_rapor_lts_capaian_kedisiplinan);
                                                            }

                                                            jumlah_tematik++;
                                                            urut_mapel++;

                                                            m_rapor_lts_capaian_kedisiplinan = new RaporLTSCapaianKedisiplinan();
                                                            m_rapor_lts_capaian_kedisiplinan.Rel_Siswa = m_siswa.Kode.ToString();
                                                            m_rapor_lts_capaian_kedisiplinan.KodeKelompokMapel = "";
                                                            m_rapor_lts_capaian_kedisiplinan.KelompokMapel = "tematik";
                                                            m_rapor_lts_capaian_kedisiplinan.NomorMapel = "";
                                                            m_rapor_lts_capaian_kedisiplinan.Rel_Mapel = m_desain.Rel_Mapel;
                                                            m_rapor_lts_capaian_kedisiplinan.NamaMapel = (
                                                                                                            jumlah_tematik <= Application_Libs.Constantas.ALFABET.Length
                                                                                                            ? Application_Libs.Constantas.ALFABET.ToLower().Substring(jumlah_tematik - 1, 1) + ". "
                                                                                                            : ""
                                                                                                         ) + m_desain.NamaMapelRapor;
                                                            m_rapor_lts_capaian_kedisiplinan.Kehadiran = "";
                                                            m_rapor_lts_capaian_kedisiplinan.KetepatanWaktu = "";
                                                            m_rapor_lts_capaian_kedisiplinan.PenggunaanSeragam = "";
                                                            m_rapor_lts_capaian_kedisiplinan.PenggunaanKamera = "";
                                                            m_rapor_lts_capaian_kedisiplinan.UrutanMapel = urut_mapel;
                                                            lst_hasil_capaian_kedisiplinan.Add(m_rapor_lts_capaian_kedisiplinan);
                                                        }
                                                        else
                                                        {
                                                            urut_mapel++;
                                                            if (m_desain.Poin.Trim() == "") nomor_mapel++;

                                                            m_rapor_lts_capaian_kedisiplinan.Rel_Siswa = m_siswa.Kode.ToString();
                                                            m_rapor_lts_capaian_kedisiplinan.KodeKelompokMapel = "";
                                                            m_rapor_lts_capaian_kedisiplinan.KelompokMapel = "";
                                                            m_rapor_lts_capaian_kedisiplinan.NomorMapel = (m_desain.Poin.Trim() != "" ? "" : nomor_mapel.ToString());
                                                            m_rapor_lts_capaian_kedisiplinan.Rel_Mapel = m_desain.Rel_Mapel;
                                                            m_rapor_lts_capaian_kedisiplinan.NamaMapel = (m_desain.Poin.Trim() != "" ? m_desain.Poin + " " : "") +
                                                                                                         (
                                                                                                            m_desain.Rel_Mapel.Trim() == ""
                                                                                                            ? m_desain.NamaMapelRapor.Replace(":", "") + ":"
                                                                                                            : m_desain.NamaMapelRapor
                                                                                                         );
                                                            m_rapor_lts_capaian_kedisiplinan.Kehadiran = "";
                                                            m_rapor_lts_capaian_kedisiplinan.KetepatanWaktu = "";
                                                            m_rapor_lts_capaian_kedisiplinan.PenggunaanSeragam = "";
                                                            m_rapor_lts_capaian_kedisiplinan.PenggunaanKamera = "";
                                                            m_rapor_lts_capaian_kedisiplinan.UrutanMapel = urut_mapel;
                                                            lst_hasil_capaian_kedisiplinan.Add(m_rapor_lts_capaian_kedisiplinan);
                                                        }
                                                    }
                                                    //end capaian kedisiplinan

                                                    //nilai pengetahuan
                                                    int id_nilai = 0;
                                                    int id_tagihan = 1;
                                                    for (int i = 1; i <= jml_kolom; i++)
                                                    {
                                                        switch (i)
                                                        {
                                                            case 1:
                                                                m_rapor_lts.T1 =
                                                                    (
                                                                        id_nilai < lst_nilai_pengetahuan.Count
                                                                        ? lst_nilai_pengetahuan[id_nilai]
                                                                        : ""
                                                                    );
                                                                if (m_rapor_lts.T1.Trim() != "")
                                                                {
                                                                    m_rapor_lts_des.PoinTCol1_1 = id_tagihan.ToString(); id_tagihan++;
                                                                }
                                                                m_rapor_lts_des.DesPoinTCol1_1 =
                                                                    (
                                                                        id_nilai < lst_nilai_pengetahuan_des.Count
                                                                        ? lst_nilai_pengetahuan_des[id_nilai]
                                                                        : ""
                                                                    );
                                                                break;
                                                            case 2:
                                                                m_rapor_lts.T2 =
                                                                    (
                                                                        id_nilai < lst_nilai_pengetahuan.Count
                                                                        ? lst_nilai_pengetahuan[id_nilai]
                                                                        : ""
                                                                    );
                                                                if (m_rapor_lts.T2.Trim() != "")
                                                                {
                                                                    m_rapor_lts_des.PoinTCol1_2 = id_tagihan.ToString(); id_tagihan++;
                                                                }
                                                                m_rapor_lts_des.DesPoinTCol1_2 =
                                                                    (
                                                                        id_nilai < lst_nilai_pengetahuan_des.Count
                                                                        ? lst_nilai_pengetahuan_des[id_nilai]
                                                                        : ""
                                                                    );
                                                                break;
                                                            case 3:
                                                                m_rapor_lts.T3 =
                                                                    (
                                                                        id_nilai < lst_nilai_pengetahuan.Count
                                                                        ? lst_nilai_pengetahuan[id_nilai]
                                                                        : ""
                                                                    );
                                                                if (m_rapor_lts.T3.Trim() != "")
                                                                {
                                                                    m_rapor_lts_des.PoinTCol1_3 = id_tagihan.ToString(); id_tagihan++;
                                                                }
                                                                m_rapor_lts_des.DesPoinTCol1_3 =
                                                                    (
                                                                        id_nilai < lst_nilai_pengetahuan_des.Count
                                                                        ? lst_nilai_pengetahuan_des[id_nilai]
                                                                        : ""
                                                                    );
                                                                break;
                                                            case 4:
                                                                m_rapor_lts.T4 =
                                                                    (
                                                                        id_nilai < lst_nilai_pengetahuan.Count
                                                                        ? lst_nilai_pengetahuan[id_nilai]
                                                                        : ""
                                                                    );
                                                                if (m_rapor_lts.T4.Trim() != "")
                                                                {
                                                                    m_rapor_lts_des.PoinTCol1_4 = id_tagihan.ToString(); id_tagihan++;
                                                                }
                                                                m_rapor_lts_des.DesPoinTCol1_4 =
                                                                    (
                                                                        id_nilai < lst_nilai_pengetahuan_des.Count
                                                                        ? lst_nilai_pengetahuan_des[id_nilai]
                                                                        : ""
                                                                    );
                                                                break;
                                                            case 5:
                                                                m_rapor_lts.T5 =
                                                                    (
                                                                        id_nilai < lst_nilai_pengetahuan.Count
                                                                        ? lst_nilai_pengetahuan[id_nilai]
                                                                        : ""
                                                                    );
                                                                if (m_rapor_lts.T5.Trim() != "")
                                                                {
                                                                    m_rapor_lts_des.PoinTCol1_5 = id_tagihan.ToString(); id_tagihan++;
                                                                }
                                                                m_rapor_lts_des.DesPoinTCol1_5 =
                                                                    (
                                                                        id_nilai < lst_nilai_pengetahuan_des.Count
                                                                        ? lst_nilai_pengetahuan_des[id_nilai]
                                                                        : ""
                                                                    );
                                                                break;
                                                            case 6:
                                                                m_rapor_lts.T6 =
                                                                    (
                                                                        id_nilai < lst_nilai_pengetahuan.Count
                                                                        ? lst_nilai_pengetahuan[id_nilai]
                                                                        : ""
                                                                    );
                                                                if (m_rapor_lts.T6.Trim() != "")
                                                                {
                                                                    m_rapor_lts_des.PoinTCol1_6 = id_tagihan.ToString(); id_tagihan++;
                                                                }
                                                                m_rapor_lts_des.DesPoinTCol1_6 =
                                                                    (
                                                                        id_nilai < lst_nilai_pengetahuan_des.Count
                                                                        ? lst_nilai_pengetahuan_des[id_nilai]
                                                                        : ""
                                                                    );
                                                                break;
                                                            default:
                                                                break;
                                                        }

                                                        id_nilai++;
                                                    }

                                                    //nilai keterampilan
                                                    id_nilai = 0;
                                                    id_tagihan = 1;
                                                    for (int i = 1; i <= jml_kolom; i++)
                                                    {
                                                        switch (i)
                                                        {
                                                            case 1:
                                                                m_rapor_lts.T7 =
                                                                    (
                                                                        id_nilai < lst_nilai_keterampilan.Count
                                                                        ? lst_nilai_keterampilan[id_nilai]
                                                                        : ""
                                                                    );
                                                                if (m_rapor_lts.T7.Trim() != "")
                                                                {
                                                                    m_rapor_lts_des.PoinTCol2_1 = id_tagihan.ToString(); id_tagihan++;
                                                                }
                                                                m_rapor_lts_des.DesPoinTCol2_1 =
                                                                    (
                                                                        id_nilai < lst_nilai_keterampilan_des.Count
                                                                        ? lst_nilai_keterampilan_des[id_nilai]
                                                                        : ""
                                                                    ); 
                                                                break;
                                                            case 2:
                                                                m_rapor_lts.T8 =
                                                                    (
                                                                        id_nilai < lst_nilai_keterampilan.Count
                                                                        ? lst_nilai_keterampilan[id_nilai]
                                                                        : ""
                                                                    );
                                                                if (m_rapor_lts.T8.Trim() != "")
                                                                {
                                                                    m_rapor_lts_des.PoinTCol2_2 = id_tagihan.ToString(); id_tagihan++;
                                                                }
                                                                m_rapor_lts_des.DesPoinTCol2_2 =
                                                                    (
                                                                        id_nilai < lst_nilai_keterampilan_des.Count
                                                                        ? lst_nilai_keterampilan_des[id_nilai]
                                                                        : ""
                                                                    );
                                                                break;
                                                            case 3:
                                                                m_rapor_lts.T9 =
                                                                    (
                                                                        id_nilai < lst_nilai_keterampilan.Count
                                                                        ? lst_nilai_keterampilan[id_nilai]
                                                                        : ""
                                                                    );
                                                                if (m_rapor_lts.T9.Trim() != "")
                                                                {
                                                                    m_rapor_lts_des.PoinTCol2_3 = id_tagihan.ToString(); id_tagihan++;
                                                                }
                                                                m_rapor_lts_des.DesPoinTCol2_3 =
                                                                    (
                                                                        id_nilai < lst_nilai_keterampilan_des.Count
                                                                        ? lst_nilai_keterampilan_des[id_nilai]
                                                                        : ""
                                                                    );
                                                                break;
                                                            case 4:
                                                                m_rapor_lts.T10 =
                                                                    (
                                                                        id_nilai < lst_nilai_keterampilan.Count
                                                                        ? lst_nilai_keterampilan[id_nilai]
                                                                        : ""
                                                                    );
                                                                if (m_rapor_lts.T10.Trim() != "")
                                                                {
                                                                    m_rapor_lts_des.PoinTCol2_4 = id_tagihan.ToString(); id_tagihan++;
                                                                }
                                                                m_rapor_lts_des.DesPoinTCol2_4 =
                                                                    (
                                                                        id_nilai < lst_nilai_keterampilan_des.Count
                                                                        ? lst_nilai_keterampilan_des[id_nilai]
                                                                        : ""
                                                                    );
                                                                break;
                                                            case 5:
                                                                m_rapor_lts.T11 =
                                                                    (
                                                                        id_nilai < lst_nilai_keterampilan.Count
                                                                        ? lst_nilai_keterampilan[id_nilai]
                                                                        : ""
                                                                    );
                                                                if (m_rapor_lts.T11.Trim() != "")
                                                                {
                                                                    m_rapor_lts_des.PoinTCol2_5 = id_tagihan.ToString(); id_tagihan++;
                                                                }
                                                                m_rapor_lts_des.DesPoinTCol2_5 =
                                                                    (
                                                                        id_nilai < lst_nilai_keterampilan_des.Count
                                                                        ? lst_nilai_keterampilan_des[id_nilai]
                                                                        : ""
                                                                    );
                                                                break;
                                                            case 6:
                                                                m_rapor_lts.T12 =
                                                                    (
                                                                        id_nilai < lst_nilai_keterampilan.Count
                                                                        ? lst_nilai_keterampilan[id_nilai]
                                                                        : ""
                                                                    );
                                                                if (m_rapor_lts.T12.Trim() != "")
                                                                {
                                                                    m_rapor_lts_des.PoinTCol2_6 = id_tagihan.ToString(); id_tagihan++;
                                                                }
                                                                m_rapor_lts_des.DesPoinTCol2_6 =
                                                                    (
                                                                        id_nilai < lst_nilai_keterampilan_des.Count
                                                                        ? lst_nilai_keterampilan_des[id_nilai]
                                                                        : ""
                                                                    );
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                        id_nilai++;
                                                    }

                                                    lst_hasil_lts.Add(m_rapor_lts);
                                                    lst_hasil_deksripsi.Add(m_rapor_lts_des);
                                                    id++;
                                                }
                                            }
                                        }

                                    }

                                }

                            }

                        }

                    }
                }

                GetRaporLTS = lst_hasil_lts;
                GetRaporLTSDeskripsi = lst_hasil_deksripsi;
                GetRaporLTSCapaianKedisiplinan = lst_hasil_capaian_kedisiplinan;
            }
        }

        public static void NilaiRapor_KURTILAS(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa = "")
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

            List<KURTILAS_RaporNilai> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KURTILAS(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

            List<KURTILAS_RaporSikap> lst_nilai_sikap = DAO_Rapor_Semester.GetNilaiSikap_KURTILAS(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiSikap = Libs.ToDataTable(lst_nilai_sikap);

            List<KURTILAS_RaporMulok> lst_nilai_mulok = DAO_Rapor_Semester.GetNilaiMulok_KURTILAS(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiMulok = Libs.ToDataTable(lst_nilai_mulok);

            ReportDocument rpt_doc = new ReportDocument();
            rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());

            rpt_doc.Load(HttpContext.Current.Server.MapPath("~/Application_Reports/SD/RaporKURTILAS.rpt"));

            rpt_doc.SetDataSource(dtNilai);
            if (dtNilai.Rows.Count > 0)
            {
                rpt_doc.OpenSubreport("RaporSikap").SetDataSource(dtNilaiSikap);
                rpt_doc.OpenSubreport("RaporMulok").SetDataSource(dtNilaiMulok);
            }

            Stream stream = null;
            string content_type = "";
            content_type = "applications/pdf";

            rpt_doc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, http_response, true, ("Rapor" + tahun_ajaran + "SM" + semester + "" + kelas_det.Replace("-", "") + "" + DateTime.Now.ToString("yyyyMMddHHmmss")).Replace("/", "").Replace("\\", ""));
            stream = rpt_doc.ExportToStream(ExportFormatType.PortableDocFormat);

            byte[] byteArray = new byte[stream.Length];
            stream.Read(byteArray, 0, Convert.ToInt32(stream.Length - 1));

            rpt_doc.Close();
            rpt_doc.Dispose();

            http_response.ClearContent();
            http_response.ClearHeaders();
            http_response.ContentType = content_type;
            http_response.BinaryWrite(byteArray);
            http_response.Flush();
            http_response.Close();
            http_response.End();
        }
    }
}