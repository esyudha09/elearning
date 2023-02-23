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
using AI_ERP.Application_Entities.Elearning.SMP;
using AI_ERP.Application_Entities.Elearning.SMP.Reports;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning.SMP;

namespace AI_ERP.Application_Libs
{
    public static class Reports_SMP
    {
        public static Sekolah GetUnitSekolah()
        {
            Sekolah m_unit = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMP).FirstOrDefault();
            return m_unit;
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

                                List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    rel_kelas_det,
                                    tahun_ajaran,
                                    semester
                                );

                                Rapor_LTS_MengetahuiGuruKelas m_mengetahui = DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FirstOrDefault();

                                string s_walikelas = "";
                                string s_tanggal = "";

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
                                                        s_walikelas = m_pegawai.Nama;

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

                                if (s_walikelas.Trim() == "")
                                {
                                    if (DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).Count > 0)
                                    {
                                        s_walikelas = m_mengetahui.NamaGuru;
                                        s_tanggal = Libs.GetTanggalIndonesiaFromDate(m_mengetahui.Tanggal, false);
                                    }
                                }

                                //nilai akademik
                                List<Rapor_Desain> lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, rel_kelas_det
                                    ).FindAll(m => m.JenisRapor == "LTS");

                                List<Rapor_StrukturNilai> lst_sn = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, m_kelas.Kode.ToString()
                                    );

                                List<Rapor_NilaiSiswa_Det_Extend> lst_nilai_siswa_det_ = new List<Rapor_NilaiSiswa_Det_Extend>();
                                lst_nilai_siswa_det_ = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Extend_Entity(
                                        tahun_ajaran, semester, rel_kelas_det
                                    ).FindAll(m => m.Nilai.Trim() != "");
                                List<Rapor_NilaiSiswa> lst_nilaisiswa =
                                    DAO_Rapor_NilaiSiswa.GetAllByTABySMByKelasDet_ForReport_Entity(tahun_ajaran, semester, rel_kelas_det);

                                Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                                List<Rapor_Desain_Det> lst_desain_rapor_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(m_rapor_desain.Kode.ToString()).OrderBy(m => m.Urutan).ToList();

                                List<DAO_SiswaAbsen.AbsenRekapRaporSiswa> lst_absen_ = new List<DAO_SiswaAbsen.AbsenRekapRaporSiswa>();
                                lst_absen_ = DAO_SiswaAbsen.GetRekapAbsenRaporByPeriode_Entity(
                                        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                    );

                                List<DAO_SiswaAbsenMapel.AbsenMapel> lst_absen_mapel = DAO_SiswaAbsenMapel.GetAllByTABySMByPeriodeByKelas_Entity(
                                        tahun_ajaran, semester,
                                        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue),
                                        rel_kelas_det
                                    );

                                List<Rapor_StrukturNilai_KP> lst_sn_kp = DAO_Rapor_StrukturNilai_KP.GetAllByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString()
                                    );

                                //rekap absensi walas
                                List<SiswaAbsenRekap> lst_absen_rekap = DAO_SiswaAbsenRekap.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FindAll(m0 => m0.Rel_Mapel.Trim() == "" && m0.Jenis.ToString().ToUpper() == TipeRapor.LTS.ToUpper().Trim());
                                List<SiswaAbsenRekapDet> lst_rekap_det = new List<SiswaAbsenRekapDet>();
                                if (lst_absen_rekap.Count == 1)
                                {
                                    SiswaAbsenRekap m_rekap_absensi = lst_absen_rekap.FirstOrDefault();
                                    if (m_rekap_absensi != null)
                                    {
                                        if (m_rekap_absensi.TahunAjaran != null)
                                        {
                                            lst_rekap_det = DAO_SiswaAbsenRekapDet.GetAllByHeader_Entity(m_rekap_absensi.Kode.ToString());
                                        }
                                    }
                                }
                                //end rekap absensi walas

                                int nomor_mapel = 0;
                                int urut_mapel = 0;
                                if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m0 => (rel_siswa + ";").Trim().ToUpper().IndexOf(m0.Kode.ToString().ToUpper() + ";") >= 0).ToList();
                                foreach (var m_siswa in lst_siswa)
                                {
                                    string s_info_qr = "";
                                    s_info_qr = "NIS = " + m_siswa.NISSekolah + ", " +
                                                "Nama = " + m_siswa.Nama.Trim().ToUpper() + ", " +
                                                "Unit = SMP, " +
                                                "Tahun Pelajaran & Semester = " + tahun_ajaran + " & " + semester + ", " +
                                                "Kelas = " + m_kelas_det.Nama;
                                    byte[] qr_code =
                                        (show_qrcode
                                            ? (byte[])(new ImageConverter()).ConvertTo(QRCodeGenerator.GetQRCode(s_info_qr, 20), typeof(byte[]))
                                            : null
                                        );

                                    if (lst_desain_rapor.Count == 1)
                                    {
                                        if (m_rapor_desain != null)
                                        {
                                            if (m_rapor_desain.TahunAjaran != null)
                                            {
                                                //absen
                                                string s_sakit = "-";
                                                string s_izin = "-";
                                                string s_alpa = "-";
                                                string s_terlambat = "-";

                                                List<DAO_SiswaAbsen.AbsenRekapRaporSiswa> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRaporSiswa>();
                                                lst_absen = lst_absen_.FindAll(m0 => m0.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper());
                                                foreach (var absen in lst_absen)
                                                {
                                                    if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                                    if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                                    if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                                    if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                                }
                                                if (tahun_ajaran == "2020/2021")
                                                {
                                                    if (lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().Replace(";", "").ToUpper() == m_siswa.Kode.ToString().Replace(";", "").ToUpper().Trim()).Count > 0)
                                                    {
                                                        SiswaAbsenRekapDet m_rekap_absen_siswa = lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().Replace(";", "").ToUpper() == m_siswa.Kode.ToString().Replace(";", "").ToUpper().Trim()).FirstOrDefault();
                                                        if (m_rekap_absen_siswa != null)
                                                        {
                                                            if (m_rekap_absen_siswa.Rel_Siswa != null)
                                                            {
                                                                s_sakit = m_rekap_absen_siswa.Sakit;
                                                                s_izin = m_rekap_absen_siswa.Izin;
                                                                s_alpa = m_rekap_absen_siswa.Alpa;
                                                                s_terlambat = m_rekap_absen_siswa.Terlambat;
                                                            }
                                                        }
                                                    }
                                                }

                                                //absen lts
                                                if (Libs.GetStringToDecimal(tahun_ajaran.Replace("/", "")) >= 20212022)
                                                {
                                                    List<DAO_SiswaAbsen.AbsenRekapRapor> lst_absen_lts = new List<DAO_SiswaAbsen.AbsenRekapRapor>();
                                                    lst_absen_lts = DAO_SiswaAbsen.GetRekapAbsenRaporBySiswaByPeriode_Entity(
                                                            m_siswa.Kode.ToString(),
                                                            (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                                            (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                                        );
                                                    foreach (var absen in lst_absen_lts)
                                                    {
                                                        if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                                        if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                                        if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                                        if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                                    }
                                                }
                                                //end absen lts
                                                
                                                foreach (var m_desain in lst_desain_rapor_det)
                                                {
                                                    string s_kkm = "";

                                                    Rapor_StrukturNilai m_struktur = lst_sn.FindAll(
                                                            m0 => m0.Rel_Mapel.ToString().Trim().ToUpper() == m_desain.Rel_Mapel.ToString().Trim().ToUpper()
                                                        ).FirstOrDefault();

                                                    if (m_struktur != null)
                                                    {
                                                        if (m_struktur.TahunAjaran != null)
                                                        {
                                                            s_kkm = Convert.ToInt16(m_struktur.KKM).ToString();
                                                        }
                                                    }

                                                    List<Rapor_NilaiSiswa_Det_Extend> lst_nilai_siswa_det = new List<Rapor_NilaiSiswa_Det_Extend>();
                                                    lst_nilai_siswa_det = lst_nilai_siswa_det_.FindAll(
                                                            m0 => m0.Rel_Mapel.ToString().Trim().ToUpper() == m_desain.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                  m0.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper()
                                                        ).FindAll(m => m.Nilai.Trim() != "");

                                                    List<string> lst_distinct_kd = lst_nilai_siswa_det.Select(m => m.Rel_Rapor_KompetensiDasar).Distinct().ToList();
                                                    int id_tagihan = 1;

                                                    //absen
                                                    string s_jumlah_hadir = "0";
                                                    string s_jumlah_hadir_max = "0";
                                                    DAO_SiswaAbsenMapel.AbsenMapel m_absen_mapel = lst_absen_mapel.FindAll(
                                                            m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == m_desain.Rel_Mapel.ToString().ToUpper().Trim() &&
                                                                  m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                                                        ).FirstOrDefault();
                                                    if (m_absen_mapel != null)
                                                    {
                                                        if (m_absen_mapel.Rel_Siswa != null)
                                                        {
                                                            s_jumlah_hadir = m_absen_mapel.JumlahHadir;
                                                            s_jumlah_hadir_max = m_absen_mapel.JumlahHadirMax;
                                                        }
                                                    }
                                                    if (tahun_ajaran == "2020/2021")
                                                    {
                                                        if (lst_nilai_siswa_det.Count > 0)
                                                        {
                                                            s_jumlah_hadir = lst_nilai_siswa_det.FirstOrDefault().LTS_HD;
                                                            s_jumlah_hadir_max = lst_nilai_siswa_det.FirstOrDefault().LTS_MAX_HD;
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
                                                    m_rapor_lts.NamaMapel = m_desain.NamaMapelRapor;
                                                    m_rapor_lts.KKM = s_kkm;
                                                    m_rapor_lts.T1 = "";
                                                    m_rapor_lts.T2 = "";
                                                    m_rapor_lts.T3 = "";
                                                    m_rapor_lts.T4 = "";
                                                    m_rapor_lts.T5 = "";
                                                    m_rapor_lts.T6 = "";
                                                    m_rapor_lts.T7 = "";
                                                    m_rapor_lts.T8 = "";
                                                    m_rapor_lts.T9 = "";
                                                    m_rapor_lts.T10 = "";
                                                    m_rapor_lts.T11 = "";
                                                    m_rapor_lts.T12 = "";
                                                    m_rapor_lts.T13 = "";
                                                    m_rapor_lts.T14 = "";
                                                    m_rapor_lts.T15 = "";
                                                    m_rapor_lts.HD = s_jumlah_hadir + "/" + s_jumlah_hadir_max;
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
                                                    m_rapor_lts.Terlambat = s_terlambat;
                                                    m_rapor_lts.TanggalRapor = s_tanggal;
                                                    m_rapor_lts.WaliKelas = s_walikelas;
                                                    m_rapor_lts.UrutanMapel = m_desain.Urutan;
                                                    m_rapor_lts.TTDGuru = img_ttd_guru;
                                                    m_rapor_lts.QRCode = qr_code;

                                                    RaporLTSDeskripsi m_rapor_lts_des = new RaporLTSDeskripsi();
                                                    m_rapor_lts_des.Rel_Siswa = m_siswa.Kode.ToString();
                                                    m_rapor_lts_des.TahunAjaran = tahun_ajaran;
                                                    m_rapor_lts_des.Semester = semester;
                                                    m_rapor_lts_des.Rel_Mapel = m_desain.Rel_Mapel;
                                                    m_rapor_lts_des.Nama = m_siswa.Nama;
                                                    m_rapor_lts_des.Kelas = m_kelas_det.Nama;
                                                    m_rapor_lts_des.NamaMapel = m_desain.NamaMapelRapor;
                                                    m_rapor_lts_des.NomorMapel = m_desain.Nomor;
                                                    m_rapor_lts_des.KodeKelompokMapel = "";
                                                    m_rapor_lts_des.KelompokMapel = "";
                                                    m_rapor_lts_des.UrutanMapel = m_desain.Urutan;

                                                    m_rapor_lts_des.PoinTCol1_1 = "";
                                                    m_rapor_lts_des.PoinTCol1_2 = "";
                                                    m_rapor_lts_des.PoinTCol1_3 = "";
                                                    m_rapor_lts_des.PoinTCol1_4 = "";
                                                    m_rapor_lts_des.PoinTCol1_5 = "";
                                                    m_rapor_lts_des.PoinTCol1_6 = "";

                                                    m_rapor_lts_des.PoinTCol2_1 = "";
                                                    m_rapor_lts_des.PoinTCol2_2 = "";
                                                    m_rapor_lts_des.PoinTCol2_3 = "";
                                                    m_rapor_lts_des.PoinTCol2_4 = "";
                                                    m_rapor_lts_des.PoinTCol2_5 = "";
                                                    m_rapor_lts_des.PoinTCol2_6 = "";

                                                    m_rapor_lts_des.DesPoinTCol1_1 = "";
                                                    m_rapor_lts_des.DesPoinTCol1_2 = "";
                                                    m_rapor_lts_des.DesPoinTCol1_3 = "";
                                                    m_rapor_lts_des.DesPoinTCol1_4 = "";
                                                    m_rapor_lts_des.DesPoinTCol1_5 = "";
                                                    m_rapor_lts_des.DesPoinTCol1_6 = "";

                                                    m_rapor_lts_des.DesPoinTCol2_1 = "";
                                                    m_rapor_lts_des.DesPoinTCol2_2 = "";
                                                    m_rapor_lts_des.DesPoinTCol2_3 = "";
                                                    m_rapor_lts_des.DesPoinTCol2_4 = "";
                                                    m_rapor_lts_des.DesPoinTCol2_5 = "";
                                                    m_rapor_lts_des.DesPoinTCol2_6 = "";

                                                    string rel_rapornilaisiswa = "";
                                                    //tugas
                                                    if (Libs.GetStringToInteger(tahun_ajaran.Replace("/", "")) >= 30202021)
                                                    {
                                                        int id = 0;
                                                        var lst_ = lst_nilai_siswa_det.FindAll(m =>
                                                                    (
                                                                        Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "TUGAS" ||
                                                                        Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                        Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                        Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0 ||
                                                                        Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PTS") >= 0 ||
                                                                        Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PAS") >= 0 ||
                                                                        Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PAT") >= 0
                                                                    )
                                                                );
                                                        foreach (var item_item_lst in lst_)
                                                        {
                                                            id++;
                                                            if (id > 6) break;
                                                            if (
                                                                    lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == item_item_lst.Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                    lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == item_item_lst.Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                                )
                                                            {
                                                                rel_rapornilaisiswa = item_item_lst.Rel_Rapor_NilaiSiswa.ToString();

                                                                decimal d_nilai =
                                                                    (
                                                                        item_item_lst.PB.Trim() != "" && item_item_lst.PB.Trim().Substring(0, 1) != "="
                                                                        ? Libs.GetStringToDecimal(item_item_lst.PB)
                                                                        : Libs.GetStringToDecimal(item_item_lst.Nilai)
                                                                    );
                                                                string s_nilai = Math.Round(d_nilai, MidpointRounding.AwayFromZero).ToString();
                                                                if ((
                                                                        item_item_lst.PB.Trim() != "" && item_item_lst.PB.Trim().Substring(0, 1) != "="
                                                                        ? (item_item_lst.PB)
                                                                        : (item_item_lst.Nilai)
                                                                    ).ToUpper().Trim() == "BL")
                                                                {
                                                                    s_nilai = "BL";
                                                                }

                                                                switch (id_tagihan)
                                                                {
                                                                    case 1:
                                                                        m_rapor_lts.T1 = s_nilai;
                                                                        if (m_rapor_lts.T1.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_1 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_1 = item_item_lst.DeskripsiKP;
                                                                        break;
                                                                    case 2:
                                                                        m_rapor_lts.T2 = s_nilai;
                                                                        if (m_rapor_lts.T2.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_2 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_2 = item_item_lst.DeskripsiKP;
                                                                        break;
                                                                    case 3:
                                                                        m_rapor_lts.T3 = s_nilai;
                                                                        if (m_rapor_lts.T3.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_3 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_3 = item_item_lst.DeskripsiKP;
                                                                        break;
                                                                    case 4:
                                                                        m_rapor_lts.T4 = s_nilai;
                                                                        if (m_rapor_lts.T4.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_4 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_4 = item_item_lst.DeskripsiKP;
                                                                        break;
                                                                    case 5:
                                                                        m_rapor_lts.T5 = s_nilai;
                                                                        if (m_rapor_lts.T5.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_5 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_5 = item_item_lst.DeskripsiKP;
                                                                        break;
                                                                    case 6:
                                                                        m_rapor_lts.T6 = s_nilai;
                                                                        if (m_rapor_lts.T6.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_6 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_6 = item_item_lst.DeskripsiKP;
                                                                        break;
                                                                }
                                                            }
                                                        }

                                                        //UH
                                                        for (int i = 1; i <= 6; i++)
                                                        {
                                                            var lst = lst_nilai_siswa_det.FindAll(m =>
                                                                (
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "UH" //||
                                                                                                                              //Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PTS") >= 0 ||
                                                                                                                              //Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PAS") >= 0 ||
                                                                                                                              //Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PAT") >= 0
                                                                )
                                                                && m.UrutanKP == i);
                                                            if (lst.Count == 1)
                                                            {
                                                                rel_rapornilaisiswa = lst[0].Rel_Rapor_NilaiSiswa.ToString();

                                                                if (
                                                                        lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                        lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                                    )
                                                                {
                                                                    decimal d_nilai =
                                                                        (
                                                                            lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                            ? Libs.GetStringToDecimal(lst[0].PB)
                                                                            : Libs.GetStringToDecimal(lst[0].Nilai)
                                                                        );
                                                                    string s_nilai = Math.Round(d_nilai, MidpointRounding.AwayFromZero).ToString();
                                                                    if ((
                                                                            lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                            ? (lst[0].PB)
                                                                            : (lst[0].Nilai)
                                                                        ).ToUpper().Trim() == "BL")
                                                                    {
                                                                        s_nilai = "BL";
                                                                    }

                                                                    switch (id_tagihan)
                                                                    {
                                                                        case 1:
                                                                            m_rapor_lts.T1 = s_nilai;
                                                                            if (m_rapor_lts.T1.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_1 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_1 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 2:
                                                                            m_rapor_lts.T2 = s_nilai;
                                                                            if (m_rapor_lts.T2.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_2 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_2 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 3:
                                                                            m_rapor_lts.T3 = s_nilai;
                                                                            if (m_rapor_lts.T3.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_3 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_3 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 4:
                                                                            m_rapor_lts.T4 = s_nilai;
                                                                            if (m_rapor_lts.T4.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_4 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_4 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 5:
                                                                            m_rapor_lts.T5 = s_nilai;
                                                                            if (m_rapor_lts.T5.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_5 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_5 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 6:
                                                                            m_rapor_lts.T6 = s_nilai;
                                                                            if (m_rapor_lts.T6.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_6 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_6 = lst[0].DeskripsiKP;
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        for (int i = 1; i <= 6; i++)
                                                        {
                                                            var lst = lst_nilai_siswa_det.FindAll(m =>
                                                                    (
                                                                        Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "TUGAS" ||
                                                                        Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                        Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                        Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0 
                                                                    )
                                                                && m.UrutanKP == i
                                                                );
                                                            if (lst.Count == 1)
                                                            {
                                                                if (
                                                                        lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                        lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                                    )
                                                                {
                                                                    rel_rapornilaisiswa = lst[0].Rel_Rapor_NilaiSiswa.ToString();

                                                                    decimal d_nilai =
                                                                        (
                                                                            lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                            ? Libs.GetStringToDecimal(lst[0].PB)
                                                                            : Libs.GetStringToDecimal(lst[0].Nilai)
                                                                        );
                                                                    string s_nilai = Math.Round(d_nilai, MidpointRounding.AwayFromZero).ToString();
                                                                    if ((
                                                                            lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                            ? (lst[0].PB)
                                                                            : (lst[0].Nilai)
                                                                        ).ToUpper().Trim() == "BL")
                                                                    {
                                                                        s_nilai = "BL";
                                                                    }

                                                                    switch (id_tagihan)
                                                                    {
                                                                        case 1:
                                                                            m_rapor_lts.T1 = s_nilai;
                                                                            if (m_rapor_lts.T1.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_1 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_1 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 2:
                                                                            m_rapor_lts.T2 = s_nilai;
                                                                            if (m_rapor_lts.T2.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_2 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_2 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 3:
                                                                            m_rapor_lts.T3 = s_nilai;
                                                                            if (m_rapor_lts.T3.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_3 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_3 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 4:
                                                                            m_rapor_lts.T4 = s_nilai;
                                                                            if (m_rapor_lts.T4.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_4 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_4 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 5:
                                                                            m_rapor_lts.T5 = s_nilai;
                                                                            if (m_rapor_lts.T5.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_5 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_5 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 6:
                                                                            m_rapor_lts.T6 = s_nilai;
                                                                            if (m_rapor_lts.T6.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_6 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_6 = lst[0].DeskripsiKP;
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        //UH
                                                        for (int i = 1; i <= 6; i++)
                                                        {
                                                            var lst = lst_nilai_siswa_det.FindAll(m =>
                                                                (
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "UH"
                                                                )
                                                                && m.UrutanKP == i);
                                                            if (lst.Count == 1)
                                                            {
                                                                rel_rapornilaisiswa = lst[0].Rel_Rapor_NilaiSiswa.ToString();

                                                                if (
                                                                        lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                        lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                                    )
                                                                {
                                                                    decimal d_nilai =
                                                                        (
                                                                            lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                            ? Libs.GetStringToDecimal(lst[0].PB)
                                                                            : Libs.GetStringToDecimal(lst[0].Nilai)
                                                                        );
                                                                    string s_nilai = Math.Round(d_nilai, MidpointRounding.AwayFromZero).ToString();
                                                                    if ((
                                                                            lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                            ? (lst[0].PB)
                                                                            : (lst[0].Nilai)
                                                                        ).ToUpper().Trim() == "BL")
                                                                    {
                                                                        s_nilai = "BL";
                                                                    }

                                                                    switch (id_tagihan)
                                                                    {
                                                                        case 1:
                                                                            m_rapor_lts.T1 = s_nilai;
                                                                            if (m_rapor_lts.T1.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_1 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_1 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 2:
                                                                            m_rapor_lts.T2 = s_nilai;
                                                                            if (m_rapor_lts.T2.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_2 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_2 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 3:
                                                                            m_rapor_lts.T3 = s_nilai;
                                                                            if (m_rapor_lts.T3.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_3 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_3 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 4:
                                                                            m_rapor_lts.T4 = s_nilai;
                                                                            if (m_rapor_lts.T4.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_4 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_4 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 5:
                                                                            m_rapor_lts.T5 = s_nilai;
                                                                            if (m_rapor_lts.T5.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_5 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_5 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 6:
                                                                            m_rapor_lts.T6 = s_nilai;
                                                                            if (m_rapor_lts.T6.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_6 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_6 = lst[0].DeskripsiKP;
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        for (int i = 1; i <= 6; i++)
                                                        {
                                                            var lst = lst_nilai_siswa_det.FindAll(m =>
                                                                (
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PTS") >= 0 
                                                                )
                                                                && m.UrutanKP == i);
                                                            if (lst.Count == 1)
                                                            {
                                                                rel_rapornilaisiswa = lst[0].Rel_Rapor_NilaiSiswa.ToString();

                                                                if (
                                                                        lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                        lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                                    )
                                                                {
                                                                    decimal d_nilai =
                                                                        (
                                                                            lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                            ? Libs.GetStringToDecimal(lst[0].PB)
                                                                            : Libs.GetStringToDecimal(lst[0].Nilai)
                                                                        );
                                                                    string s_nilai = Math.Round(d_nilai, MidpointRounding.AwayFromZero).ToString();
                                                                    if ((
                                                                            lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                            ? (lst[0].PB)
                                                                            : (lst[0].Nilai)
                                                                        ).ToUpper().Trim() == "BL")
                                                                    {
                                                                        s_nilai = "BL";
                                                                    }

                                                                    switch (id_tagihan)
                                                                    {
                                                                        case 1:
                                                                            m_rapor_lts.T1 = s_nilai;
                                                                            if (m_rapor_lts.T1.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_1 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_1 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 2:
                                                                            m_rapor_lts.T2 = s_nilai;
                                                                            if (m_rapor_lts.T2.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_2 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_2 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 3:
                                                                            m_rapor_lts.T3 = s_nilai;
                                                                            if (m_rapor_lts.T3.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_3 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_3 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 4:
                                                                            m_rapor_lts.T4 = s_nilai;
                                                                            if (m_rapor_lts.T4.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_4 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_4 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 5:
                                                                            m_rapor_lts.T5 = s_nilai;
                                                                            if (m_rapor_lts.T5.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_5 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_5 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 6:
                                                                            m_rapor_lts.T6 = s_nilai;
                                                                            if (m_rapor_lts.T6.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_6 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_6 = lst[0].DeskripsiKP;
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        for (int i = 1; i <= 6; i++)
                                                        {
                                                            var lst = lst_nilai_siswa_det.FindAll(m =>
                                                                (
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PAS") >= 0 
                                                                )
                                                                && m.UrutanKP == i);
                                                            if (lst.Count == 1)
                                                            {
                                                                rel_rapornilaisiswa = lst[0].Rel_Rapor_NilaiSiswa.ToString();

                                                                if (
                                                                        lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                        lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                                    )
                                                                {
                                                                    decimal d_nilai =
                                                                        (
                                                                            lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                            ? Libs.GetStringToDecimal(lst[0].PB)
                                                                            : Libs.GetStringToDecimal(lst[0].Nilai)
                                                                        );
                                                                    string s_nilai = Math.Round(d_nilai, MidpointRounding.AwayFromZero).ToString();
                                                                    if ((
                                                                            lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                            ? (lst[0].PB)
                                                                            : (lst[0].Nilai)
                                                                        ).ToUpper().Trim() == "BL")
                                                                    {
                                                                        s_nilai = "BL";
                                                                    }

                                                                    switch (id_tagihan)
                                                                    {
                                                                        case 1:
                                                                            m_rapor_lts.T1 = s_nilai;
                                                                            if (m_rapor_lts.T1.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_1 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_1 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 2:
                                                                            m_rapor_lts.T2 = s_nilai;
                                                                            if (m_rapor_lts.T2.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_2 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_2 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 3:
                                                                            m_rapor_lts.T3 = s_nilai;
                                                                            if (m_rapor_lts.T3.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_3 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_3 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 4:
                                                                            m_rapor_lts.T4 = s_nilai;
                                                                            if (m_rapor_lts.T4.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_4 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_4 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 5:
                                                                            m_rapor_lts.T5 = s_nilai;
                                                                            if (m_rapor_lts.T5.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_5 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_5 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 6:
                                                                            m_rapor_lts.T6 = s_nilai;
                                                                            if (m_rapor_lts.T6.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_6 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_6 = lst[0].DeskripsiKP;
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        for (int i = 1; i <= 6; i++)
                                                        {
                                                            var lst = lst_nilai_siswa_det.FindAll(m =>
                                                                (
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PAT") >= 0
                                                                )
                                                                && m.UrutanKP == i);
                                                            if (lst.Count == 1)
                                                            {
                                                                rel_rapornilaisiswa = lst[0].Rel_Rapor_NilaiSiswa.ToString();

                                                                if (
                                                                        lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                        lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                                    )
                                                                {
                                                                    decimal d_nilai =
                                                                        (
                                                                            lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                            ? Libs.GetStringToDecimal(lst[0].PB)
                                                                            : Libs.GetStringToDecimal(lst[0].Nilai)
                                                                        );
                                                                    string s_nilai = Math.Round(d_nilai, MidpointRounding.AwayFromZero).ToString();
                                                                    if ((
                                                                            lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                            ? (lst[0].PB)
                                                                            : (lst[0].Nilai)
                                                                        ).ToUpper().Trim() == "BL")
                                                                    {
                                                                        s_nilai = "BL";
                                                                    }

                                                                    switch (id_tagihan)
                                                                    {
                                                                        case 1:
                                                                            m_rapor_lts.T1 = s_nilai;
                                                                            if (m_rapor_lts.T1.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_1 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_1 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 2:
                                                                            m_rapor_lts.T2 = s_nilai;
                                                                            if (m_rapor_lts.T2.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_2 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_2 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 3:
                                                                            m_rapor_lts.T3 = s_nilai;
                                                                            if (m_rapor_lts.T3.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_3 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_3 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 4:
                                                                            m_rapor_lts.T4 = s_nilai;
                                                                            if (m_rapor_lts.T4.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_4 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_4 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 5:
                                                                            m_rapor_lts.T5 = s_nilai;
                                                                            if (m_rapor_lts.T5.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_5 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_5 = lst[0].DeskripsiKP;
                                                                            break;
                                                                        case 6:
                                                                            m_rapor_lts.T6 = s_nilai;
                                                                            if (m_rapor_lts.T6.Trim() != "")
                                                                            {
                                                                                m_rapor_lts_des.PoinTCol1_6 = id_tagihan.ToString(); id_tagihan++;
                                                                            }
                                                                            m_rapor_lts_des.DesPoinTCol1_6 = lst[0].DeskripsiKP;
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //praktik
                                                    id_tagihan = 1;
                                                    for (int i = 1; i <= 6; i++)
                                                    {
                                                        var lst = lst_nilai_siswa_det.FindAll(m =>
                                                                (
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "PRAKTIK" ||
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "KETERAMPILAN"
                                                                )
                                                                && m.UrutanKP == i
                                                            );
                                                        if (lst.Count == 1)
                                                        {
                                                            rel_rapornilaisiswa = lst[0].Rel_Rapor_NilaiSiswa.ToString();

                                                            if (
                                                                    lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                    lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                                )
                                                            {
                                                                decimal d_nilai =
                                                                    (
                                                                        lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                        ? Libs.GetStringToDecimal(lst[0].PB)
                                                                        : Libs.GetStringToDecimal(lst[0].Nilai)
                                                                    );
                                                                string s_nilai = Math.Round(d_nilai, MidpointRounding.AwayFromZero).ToString();
                                                                if ((
                                                                        lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                        ? (lst[0].PB)
                                                                        : (lst[0].Nilai)
                                                                    ).ToUpper().Trim() == "BL")
                                                                {
                                                                    s_nilai = "BL";
                                                                }

                                                                switch (id_tagihan)
                                                                {
                                                                    case 1:
                                                                        m_rapor_lts.T7 = s_nilai;
                                                                        if (m_rapor_lts.T7.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol2_1 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol2_1 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 2:
                                                                        m_rapor_lts.T8 = s_nilai;
                                                                        if (m_rapor_lts.T8.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol2_2 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol2_2 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 3:
                                                                        m_rapor_lts.T9 = s_nilai;
                                                                        if (m_rapor_lts.T9.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol2_3 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol2_3 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 4:
                                                                        m_rapor_lts.T10 = s_nilai;
                                                                        if (m_rapor_lts.T10.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol2_4 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol2_4 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 5:
                                                                        m_rapor_lts.T11 = s_nilai;
                                                                        if (m_rapor_lts.T11.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol2_5 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol2_5 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 6:
                                                                        m_rapor_lts.T12 = s_nilai;
                                                                        if (m_rapor_lts.T12.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol2_6 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol2_6 = lst[0].DeskripsiKP;
                                                                        break;
                                                                }
                                                            }
                                                        }
                                                    }

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
                                                            urut_mapel++;
                                                            if (m_desain.Poin.Trim() == "") nomor_mapel++;

                                                            m_rapor_lts_capaian_kedisiplinan.Rel_Siswa = m_nilaisiswa.Rel_Siswa;
                                                            m_rapor_lts_capaian_kedisiplinan.KodeKelompokMapel = "";
                                                            m_rapor_lts_capaian_kedisiplinan.KelompokMapel = "";
                                                            m_rapor_lts_capaian_kedisiplinan.NomorMapel = m_desain.Nomor;
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
                                                    if (!ada_nilai)
                                                    {
                                                        ada_nilai = true;

                                                        urut_mapel++;
                                                        if (m_desain.Poin.Trim() == "") nomor_mapel++;

                                                        m_rapor_lts_capaian_kedisiplinan.Rel_Siswa = m_siswa.Kode.ToString();
                                                        m_rapor_lts_capaian_kedisiplinan.KodeKelompokMapel = "";
                                                        m_rapor_lts_capaian_kedisiplinan.KelompokMapel = "";
                                                        m_rapor_lts_capaian_kedisiplinan.NomorMapel = m_desain.Nomor;
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
                                                    //end capaian kedisiplinan

                                                    lst_hasil_lts.Add(m_rapor_lts);
                                                    lst_hasil_deksripsi.Add(m_rapor_lts_des);
                                                }
                                            }
                                        }

                                    }
                                    //end nilai akademik
                                }

                            }

                        }

                    }
                }

                GetRaporLTS = lst_hasil_lts;
                GetRaporLTSDeskripsi = lst_hasil_deksripsi;
                GetRaporLTSCapaianKedisiplinan = lst_hasil_capaian_kedisiplinan;
            }

            public LTS(string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa, string s_lokasi_ttd, string test)
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

                                List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    rel_kelas_det,
                                    tahun_ajaran,
                                    semester
                                );

                                Rapor_LTS_MengetahuiGuruKelas m_mengetahui = DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FirstOrDefault();

                                string s_walikelas = "";
                                string s_tanggal = "";

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
                                                        s_walikelas = m_pegawai.Nama;

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

                                if (s_walikelas.Trim() == "")
                                {
                                    if (DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).Count > 0)
                                    {
                                        s_walikelas = m_mengetahui.NamaGuru;
                                        s_tanggal = Libs.GetTanggalIndonesiaFromDate(m_mengetahui.Tanggal, false);
                                    }
                                }

                                //nilai akademik
                                List<Rapor_Desain> lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, rel_kelas_det
                                    ).FindAll(m => m.JenisRapor == "LTS");

                                List<Rapor_StrukturNilai> lst_sn = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, m_kelas.Kode.ToString()
                                    );

                                List<Rapor_NilaiSiswa_Det_Extend> lst_nilai_siswa_det_ = new List<Rapor_NilaiSiswa_Det_Extend>();
                                lst_nilai_siswa_det_ = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Extend_Entity(
                                        tahun_ajaran, semester, rel_kelas_det
                                    ).FindAll(m => m.Nilai.Trim() != "");

                                Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                                List<Rapor_Desain_Det> lst_desain_rapor_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(m_rapor_desain.Kode.ToString()).OrderBy(m => m.Urutan).ToList();

                                List<DAO_SiswaAbsen.AbsenRekapRaporSiswa> lst_absen_ = new List<DAO_SiswaAbsen.AbsenRekapRaporSiswa>();
                                lst_absen_ = DAO_SiswaAbsen.GetRekapAbsenRaporByPeriode_Entity(
                                        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                    );

                                List<DAO_SiswaAbsenMapel.AbsenMapel> lst_absen_mapel = DAO_SiswaAbsenMapel.GetAllByTABySMByPeriodeByKelas_Entity(
                                        tahun_ajaran, semester,
                                        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue),
                                        rel_kelas_det
                                    );

                                List<Rapor_StrukturNilai_KP> lst_sn_kp = DAO_Rapor_StrukturNilai_KP.GetAllByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString()
                                    );

                                //rekap absensi walas
                                List<SiswaAbsenRekap> lst_absen_rekap = DAO_SiswaAbsenRekap.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FindAll(m0 => m0.Rel_Mapel.Trim() == "" && m0.Jenis.ToString().ToUpper() == TipeRapor.LTS.ToUpper().Trim());
                                List<SiswaAbsenRekapDet> lst_rekap_det = new List<SiswaAbsenRekapDet>();
                                if (lst_absen_rekap.Count == 1)
                                {
                                    SiswaAbsenRekap m_rekap_absensi = lst_absen_rekap.FirstOrDefault();
                                    if (m_rekap_absensi != null)
                                    {
                                        if (m_rekap_absensi.TahunAjaran != null)
                                        {
                                            lst_rekap_det = DAO_SiswaAbsenRekapDet.GetAllByHeader_Entity(m_rekap_absensi.Kode.ToString());
                                        }
                                    }
                                }
                                //end rekap absensi walas

                                if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m0 => (rel_siswa + ";").Trim().ToUpper().IndexOf(m0.Kode.ToString().ToUpper() + ";") >= 0).ToList();
                                foreach (var m_siswa in lst_siswa)
                                {
                                    if (lst_desain_rapor.Count == 1)
                                    {
                                        if (m_rapor_desain != null)
                                        {
                                            if (m_rapor_desain.TahunAjaran != null)
                                            {
                                                //absen
                                                string s_sakit = "-";
                                                string s_izin = "-";
                                                string s_alpa = "-";
                                                string s_terlambat = "-";

                                                List<DAO_SiswaAbsen.AbsenRekapRaporSiswa> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRaporSiswa>();
                                                lst_absen = lst_absen_.FindAll(m0 => m0.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper());
                                                foreach (var absen in lst_absen)
                                                {
                                                    if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                                    if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                                    if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                                    if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                                }
                                                if (tahun_ajaran == "2020/2021")
                                                {
                                                    if (lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().Replace(";", "").ToUpper() == m_siswa.Kode.ToString().Replace(";", "").ToUpper().Trim()).Count > 0)
                                                    {
                                                        SiswaAbsenRekapDet m_rekap_absen_siswa = lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().Replace(";", "").ToUpper() == m_siswa.Kode.ToString().Replace(";", "").ToUpper().Trim()).FirstOrDefault();
                                                        if (m_rekap_absen_siswa != null)
                                                        {
                                                            if (m_rekap_absen_siswa.Rel_Siswa != null)
                                                            {
                                                                s_sakit = m_rekap_absen_siswa.Sakit;
                                                                s_izin = m_rekap_absen_siswa.Izin;
                                                                s_alpa = m_rekap_absen_siswa.Alpa;
                                                                s_terlambat = m_rekap_absen_siswa.Terlambat;
                                                            }
                                                        }
                                                    }
                                                }

                                                //absen lts
                                                if (Libs.GetStringToDecimal(tahun_ajaran.Replace("/", "")) >= 20212022)
                                                {
                                                    List<DAO_SiswaAbsen.AbsenRekapRapor> lst_absen_lts = new List<DAO_SiswaAbsen.AbsenRekapRapor>();
                                                    lst_absen_lts = DAO_SiswaAbsen.GetRekapAbsenRaporBySiswaByPeriode_Entity(
                                                            m_siswa.Kode.ToString(),
                                                            (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                                            (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                                        );
                                                    foreach (var absen in lst_absen_lts)
                                                    {
                                                        if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                                        if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                                        if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                                        if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                                    }
                                                }
                                                //end absen lts

                                                foreach (var m_desain in lst_desain_rapor_det)
                                                {
                                                    string s_kkm = "";

                                                    Rapor_StrukturNilai m_struktur = lst_sn.FindAll(
                                                            m0 => m0.Rel_Mapel.ToString().Trim().ToUpper() == m_desain.Rel_Mapel.ToString().Trim().ToUpper()
                                                        ).FirstOrDefault();

                                                    if (m_struktur != null)
                                                    {
                                                        if (m_struktur.TahunAjaran != null)
                                                        {
                                                            s_kkm = Convert.ToInt16(m_struktur.KKM).ToString();
                                                        }
                                                    }

                                                    List<Rapor_NilaiSiswa_Det_Extend> lst_nilai_siswa_det = new List<Rapor_NilaiSiswa_Det_Extend>();
                                                    lst_nilai_siswa_det = lst_nilai_siswa_det_.FindAll(
                                                            m0 => m0.Rel_Mapel.ToString().Trim().ToUpper() == m_desain.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                  m0.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper()
                                                        ).FindAll(m => m.Nilai.Trim() != "");

                                                    List<string> lst_distinct_kd = lst_nilai_siswa_det.Select(m => m.Rel_Rapor_KompetensiDasar).Distinct().ToList();
                                                    int id_tagihan = 1;

                                                    //absen
                                                    string s_jumlah_hadir = "0";
                                                    string s_jumlah_hadir_max = "0";
                                                    DAO_SiswaAbsenMapel.AbsenMapel m_absen_mapel = lst_absen_mapel.FindAll(
                                                            m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == m_desain.Rel_Mapel.ToString().ToUpper().Trim() &&
                                                                  m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                                                        ).FirstOrDefault();
                                                    if (m_absen_mapel != null)
                                                    {
                                                        if (m_absen_mapel.Rel_Siswa != null)
                                                        {
                                                            s_jumlah_hadir = m_absen_mapel.JumlahHadir;
                                                            s_jumlah_hadir_max = m_absen_mapel.JumlahHadirMax;
                                                        }
                                                    }
                                                    if (tahun_ajaran == "2020/2021")
                                                    {
                                                        if (lst_nilai_siswa_det.Count > 0)
                                                        {
                                                            s_jumlah_hadir = lst_nilai_siswa_det.FirstOrDefault().LTS_HD;
                                                            s_jumlah_hadir_max = lst_nilai_siswa_det.FirstOrDefault().LTS_MAX_HD;
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
                                                    m_rapor_lts.NamaMapel = m_desain.NamaMapelRapor;
                                                    m_rapor_lts.KKM = s_kkm;
                                                    m_rapor_lts.T1 = "";
                                                    m_rapor_lts.T2 = "";
                                                    m_rapor_lts.T3 = "";
                                                    m_rapor_lts.T4 = "";
                                                    m_rapor_lts.T5 = "";
                                                    m_rapor_lts.T6 = "";
                                                    m_rapor_lts.T7 = "";
                                                    m_rapor_lts.T8 = "";
                                                    m_rapor_lts.T9 = "";
                                                    m_rapor_lts.T10 = "";
                                                    m_rapor_lts.T11 = "";
                                                    m_rapor_lts.T12 = "";
                                                    m_rapor_lts.T13 = "";
                                                    m_rapor_lts.T14 = "";
                                                    m_rapor_lts.T15 = "";
                                                    m_rapor_lts.HD = s_jumlah_hadir + "/" + s_jumlah_hadir_max;
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
                                                    m_rapor_lts.Terlambat = s_terlambat;
                                                    m_rapor_lts.TanggalRapor = s_tanggal;
                                                    m_rapor_lts.WaliKelas = s_walikelas;
                                                    m_rapor_lts.UrutanMapel = m_desain.Urutan;
                                                    m_rapor_lts.TTDGuru = img_ttd_guru;

                                                    RaporLTSDeskripsi m_rapor_lts_des = new RaporLTSDeskripsi();
                                                    m_rapor_lts_des.Rel_Siswa = m_siswa.Kode.ToString();
                                                    m_rapor_lts_des.TahunAjaran = tahun_ajaran;
                                                    m_rapor_lts_des.Semester = semester;
                                                    m_rapor_lts_des.Rel_Mapel = m_desain.Rel_Mapel;
                                                    m_rapor_lts_des.Nama = m_siswa.Nama;
                                                    m_rapor_lts_des.NamaMapel = m_desain.NamaMapelRapor;
                                                    m_rapor_lts_des.NomorMapel = m_desain.Nomor;
                                                    m_rapor_lts_des.KodeKelompokMapel = "";
                                                    m_rapor_lts_des.KelompokMapel = "";
                                                    m_rapor_lts_des.UrutanMapel = m_desain.Urutan;

                                                    m_rapor_lts_des.PoinTCol1_1 = "";
                                                    m_rapor_lts_des.PoinTCol1_2 = "";
                                                    m_rapor_lts_des.PoinTCol1_3 = "";
                                                    m_rapor_lts_des.PoinTCol1_4 = "";
                                                    m_rapor_lts_des.PoinTCol1_5 = "";
                                                    m_rapor_lts_des.PoinTCol1_6 = "";

                                                    m_rapor_lts_des.PoinTCol2_1 = "";
                                                    m_rapor_lts_des.PoinTCol2_2 = "";
                                                    m_rapor_lts_des.PoinTCol2_3 = "";
                                                    m_rapor_lts_des.PoinTCol2_4 = "";
                                                    m_rapor_lts_des.PoinTCol2_5 = "";
                                                    m_rapor_lts_des.PoinTCol2_6 = "";

                                                    m_rapor_lts_des.DesPoinTCol1_1 = "";
                                                    m_rapor_lts_des.DesPoinTCol1_2 = "";
                                                    m_rapor_lts_des.DesPoinTCol1_3 = "";
                                                    m_rapor_lts_des.DesPoinTCol1_4 = "";
                                                    m_rapor_lts_des.DesPoinTCol1_5 = "";
                                                    m_rapor_lts_des.DesPoinTCol1_6 = "";

                                                    m_rapor_lts_des.DesPoinTCol2_1 = "";
                                                    m_rapor_lts_des.DesPoinTCol2_2 = "";
                                                    m_rapor_lts_des.DesPoinTCol2_3 = "";
                                                    m_rapor_lts_des.DesPoinTCol2_4 = "";
                                                    m_rapor_lts_des.DesPoinTCol2_5 = "";
                                                    m_rapor_lts_des.DesPoinTCol2_6 = "";

                                                    //tugas
                                                    for (int i = 1; i <= 6; i++)
                                                    {
                                                        var lst = lst_nilai_siswa_det.FindAll(m =>
                                                                (
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "TUGAS" ||
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                )
                                                                && m.UrutanKP == i
                                                            );
                                                        if (lst.Count == 1)
                                                        {
                                                            if (
                                                                    lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                    lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                                )
                                                            {
                                                                decimal d_nilai =
                                                                    (
                                                                        lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                        ? Libs.GetStringToDecimal(lst[0].PB)
                                                                        : Libs.GetStringToDecimal(lst[0].Nilai)
                                                                    );

                                                                switch (id_tagihan)
                                                                {
                                                                    case 1:
                                                                        m_rapor_lts.T1 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T1.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_1 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_1 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 2:
                                                                        m_rapor_lts.T2 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T2.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_2 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_2 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 3:
                                                                        m_rapor_lts.T3 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T3.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_3 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_3 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 4:
                                                                        m_rapor_lts.T4 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T4.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_4 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_4 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 5:
                                                                        m_rapor_lts.T5 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T5.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_5 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_5 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 6:
                                                                        m_rapor_lts.T6 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T6.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_6 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_6 = lst[0].DeskripsiKP;
                                                                        break;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //UH
                                                    for (int i = 1; i <= 6; i++)
                                                    {
                                                        var lst = lst_nilai_siswa_det.FindAll(m =>
                                                            (
                                                                Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "UH" ||
                                                                Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PTS") >= 0
                                                            )
                                                            && m.UrutanKP == i);
                                                        if (lst.Count == 1)
                                                        {
                                                            if (
                                                                    lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                    lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                                )
                                                            {
                                                                switch (id_tagihan)
                                                                {
                                                                    case 1:
                                                                        m_rapor_lts.T1 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T1.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_1 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_1 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 2:
                                                                        m_rapor_lts.T2 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T2.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_2 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_2 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 3:
                                                                        m_rapor_lts.T3 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T3.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_3 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_3 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 4:
                                                                        m_rapor_lts.T4 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T4.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_4 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_4 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 5:
                                                                        m_rapor_lts.T5 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T5.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_5 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_5 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 6:
                                                                        m_rapor_lts.T6 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T6.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol1_6 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol1_6 = lst[0].DeskripsiKP;
                                                                        break;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //praktik
                                                    id_tagihan = 1;
                                                    for (int i = 1; i <= 6; i++)
                                                    {
                                                        var lst = lst_nilai_siswa_det.FindAll(m =>
                                                                (
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "PRAKTIK" ||
                                                                    Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "KETERAMPILAN"
                                                                )
                                                                && m.UrutanKP == i
                                                            );
                                                        if (lst.Count == 1)
                                                        {
                                                            if (
                                                                    lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                    lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                                )
                                                            {
                                                                switch (id_tagihan)
                                                                {
                                                                    case 1:
                                                                        m_rapor_lts.T7 =
                                                                            (
                                                                                lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                ? lst[0].PB
                                                                                : lst[0].Nilai
                                                                            );
                                                                        if (m_rapor_lts.T7.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol2_1 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol2_1 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 2:
                                                                        m_rapor_lts.T8 =
                                                                           (
                                                                               lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                               ? lst[0].PB
                                                                               : lst[0].Nilai
                                                                           );
                                                                        if (m_rapor_lts.T8.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol2_2 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol2_2 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 3:
                                                                        m_rapor_lts.T9 =
                                                                           (
                                                                               lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                               ? lst[0].PB
                                                                               : lst[0].Nilai
                                                                           );
                                                                        if (m_rapor_lts.T9.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol2_3 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol2_3 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 4:
                                                                        m_rapor_lts.T10 =
                                                                           (
                                                                               lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                               ? lst[0].PB
                                                                               : lst[0].Nilai
                                                                           );
                                                                        if (m_rapor_lts.T10.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol2_4 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol2_4 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 5:
                                                                        m_rapor_lts.T11 =
                                                                           (
                                                                               lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                               ? lst[0].PB
                                                                               : lst[0].Nilai
                                                                           );
                                                                        if (m_rapor_lts.T11.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol2_5 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol2_5 = lst[0].DeskripsiKP;
                                                                        break;
                                                                    case 6:
                                                                        m_rapor_lts.T12 =
                                                                           (
                                                                               lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                               ? lst[0].PB
                                                                               : lst[0].Nilai
                                                                           );
                                                                        if (m_rapor_lts.T12.Trim() != "")
                                                                        {
                                                                            m_rapor_lts_des.PoinTCol2_6 = id_tagihan.ToString(); id_tagihan++;
                                                                        }
                                                                        m_rapor_lts_des.DesPoinTCol2_6 = lst[0].DeskripsiKP;
                                                                        break;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    lst_hasil_lts.Add(m_rapor_lts);
                                                    lst_hasil_deksripsi.Add(m_rapor_lts_des);
                                                }
                                            }
                                        }

                                    }
                                    //end nilai akademik
                                }

                            }

                        }

                    }
                }

                GetRaporLTS = lst_hasil_lts;
                GetRaporLTSDeskripsi = lst_hasil_deksripsi;
            }
        }
    

        public static void NilaiRapor_KTSP(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string halaman, string rel_siswa)
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

            List<KTSP_RaporCatatan> lst_nilai_rapor_catatan = DAO_Rapor_Semester.GetCatatan(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiCatatan = Libs.ToDataTable(lst_nilai_rapor_catatan);

            List<KTSP_RaporNilai> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KTSP(tahun_ajaran, semester, rel_kelas_det, halaman, rel_siswa, "");
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

            rpt_doc.Load(HttpContext.Current.Server.MapPath("~/Application_Reports/SMP/RaporKTSP-OK.rpt"));
            rpt_doc.SetDataSource(dtNilai);
            if (dtNilai.Rows.Count > 0)
            {
                rpt_doc.OpenSubreport("RaporDeskripsi").SetDataSource(dtNilaiDeskripsi);
                rpt_doc.OpenSubreport("RaporEkskul").SetDataSource(dtNilaiEkskul);
                rpt_doc.OpenSubreport("RaporKetidakhadiran").SetDataSource(dtNilaiKetidakhadiran);
                rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtNilaiVolunteer);
                rpt_doc.OpenSubreport("RaporCatatanWalas").SetDataSource(dtNilaiCatatan);
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

        public static void NilaiUraian_KTSP(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string halaman, string rel_siswa)
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

            List<KTSP_RaporNilai> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KTSP(tahun_ajaran, semester, rel_kelas_det, halaman, rel_siswa, "");
            DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

            List<KTSP_RaporEkskul> lst_nilai_deskripsi_ekskul = DAO_Rapor_Semester.GetNilaiEkskul_Entity(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiEkskul = Libs.ToDataTable(lst_nilai_deskripsi_ekskul);

            List<KTSP_RaporKetidakhadiran> lst_nilai_kepribadian = DAO_Rapor_Semester.GetNilaiKetidakhadiran_Entity(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiKetidakhadiran = Libs.ToDataTable(lst_nilai_kepribadian);

            List<KTSP_RaporVolunteer> lst_nilai_volunteer = DAO_Rapor_Semester.GetVolunteer(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiVolunteer = Libs.ToDataTable(lst_nilai_volunteer);

            List<KTSP_RaporCatatan> lst_nilai_rapor_catatan = DAO_Rapor_Semester.GetCatatan(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiCatatan = Libs.ToDataTable(lst_nilai_rapor_catatan);

            ReportDocument rpt_doc = new ReportDocument();
            rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());

            rpt_doc.Load(HttpContext.Current.Server.MapPath("~/Application_Reports/SMP/RaporKTSP2.rpt"));
            rpt_doc.SetDataSource(dtNilai);
            if (dtNilai.Rows.Count > 0)
            {
                //rpt_doc.OpenSubreport("RaporDeskripsi").SetDataSource(dtNilaiDeskripsi);
                rpt_doc.OpenSubreport("RaporEkstrakurikuler").SetDataSource(dtNilaiEkskul);
                rpt_doc.OpenSubreport("RaporKetidakhadiran").SetDataSource(dtNilaiKetidakhadiran);
                rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtNilaiVolunteer);
                rpt_doc.OpenSubreport("RaporCatatanWalas").SetDataSource(dtNilaiCatatan);
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

        public static void NilaiRapor_KURTILAS(HttpResponse http_response, string tahun_ajaran, string semester, string rel_kelas_det, string halaman, string rel_siswa)
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
            List<RaporLTSCapaianKedisiplinan> ListRaporLTSCapaianKedisiplinan = new List<RaporLTSCapaianKedisiplinan>();

            List<KTSP_RaporCatatan> lst_nilai_rapor_catatan = DAO_Rapor_Semester.GetCatatan(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiCatatan = Libs.ToDataTable(lst_nilai_rapor_catatan);

            List<KTSP_RaporKetidakhadiran> lst_nilai_kepribadian = DAO_Rapor_Semester.GetNilaiKetidakhadiran_Entity(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiKetidakhadiran = Libs.ToDataTable(lst_nilai_kepribadian);

            List<KURTILAS_RaporNilai> lst_nilai_rapor = DAO_Rapor_Semester.GetNilaiRapor_KURTILAS(tahun_ajaran, semester, rel_kelas_det, ref ListRaporLTSCapaianKedisiplinan, halaman, rel_siswa, "");
            DataTable dtNilai = Libs.ToDataTable(lst_nilai_rapor);

            List<KTSP_RaporEkskul> lst_nilai_deskripsi_ekskul = DAO_Rapor_Semester.GetNilaiEkskul_Entity(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiEkskul = Libs.ToDataTable(lst_nilai_deskripsi_ekskul);

            List<KTSP_RaporVolunteer> lst_nilai_volunteer = DAO_Rapor_Semester.GetVolunteer(tahun_ajaran, semester, rel_kelas_det, rel_siswa);
            DataTable dtNilaiVolunteer = Libs.ToDataTable(lst_nilai_volunteer);
            
            ReportDocument rpt_doc = new ReportDocument();
            rpt_doc = ReportFactory.GetReport(rpt_doc.GetType());

            rpt_doc.Load(HttpContext.Current.Server.MapPath("~/Application_Reports/SMP/RaporKURTILAS.rpt"));
            rpt_doc.SetDataSource(dtNilai);
            if (dtNilai.Rows.Count > 0)
            {
                rpt_doc.OpenSubreport("RaporEkstrakurikuler").SetDataSource(dtNilaiEkskul);
                rpt_doc.OpenSubreport("RaporKetidakhadiran").SetDataSource(dtNilaiKetidakhadiran);
                rpt_doc.OpenSubreport("RaporVolunteer").SetDataSource(dtNilaiVolunteer);
                rpt_doc.OpenSubreport("RaporCatatanWalas").SetDataSource(dtNilaiCatatan);
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

        public static string GetHTMLRaporDapodikPerMapel(
                HttpResponse http_response,
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_mapel
            )
        {
            string html = "";
            Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
            Rapor_StrukturNilai m_struktur = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                            tahun_ajaran, semester, DAO_KelasDet.GetByID_Entity(rel_kelasdet).Rel_Kelas.ToString(), rel_mapel
                        ).FirstOrDefault();

            if (m_mapel != null && m_struktur != null)
            {
                if (m_mapel.Nama != null && m_struktur.TahunAjaran != null)
                {
                    html = "<tr>" +
                                "<td colspan=\"2\" style=\"font-weight: bold; color: red; text-decoration: underline;\">" +
                                    "Mohon diperhatikan" +
                                "</td>" +
                                "<td style=\"width: 200px;\">" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td style=\"width: 500px;\">" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td style=\"width: 50px;\">" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td colspan=\"2\">" +
                                    "1. Dilarang merubah format excel yang telah disediakan" +
                                "</td>" +
                                "<td colspan=\"6\">" +
                                    "4. Untuk nilai rapor KKM Pengetahuan dan KKM Keterampilan wajib terisi" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td style=\"width: 50px;\">" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td colspan=\"2\">" +
                                    "2. Dilarang merubah setiap token yang telah ada" +
                                "</td>" +
                                "<td colspan=\"6\">" +
                                    "5. Untuk nilai SIKAP/SPIRITUAL dan SOSIAL harap diisi hanya pada mata evaluasi <span style=\"font-weight: bold;\">Bahasa Indonesia</span>" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td style=\"width: 50px;\">" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td colspan=\"3\">" +
                                    "3. Dilarang merubah No, Nama Siswa, NIS dan NISN yang telah disediakan" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td style=\"width: 50px;\">" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td colspan=\"2\" style=\"width: 500px;\">" +
                                    "<h2>Format Excel Import Nilai RAPOR</h2>" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td colspan=\"2\">" +
                                    "Aplikasi Dapodik Ditjen Dikdasmen" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td colspan=\"2\">" +
                                    "Token Mata Evaluasi" +
                                "</td>" +
                                "<td style=\"background-color: black;\">" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td style=\"background-color: black;\">" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td style=\"background-color: black;\">" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td colspan=\"2\">" +
                                    "Mata Evaluasi" +
                                "</td>" +
                                "<td colspan=\"2\">" +
                                    ": " +
                                    m_mapel.Nama +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td colspan=\"2\">" +
                                    "Tingkat Pendidikan" +
                                "</td>" +
                                "<td colspan=\"2\">" +
                                    ": " + DAO_Kelas.GetByID_Entity(DAO_KelasDet.GetByID_Entity(rel_kelasdet).Rel_Kelas.ToString()).Nama +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td colspan=\"2\">" +
                                    "Rombongan Belajar" +
                                "</td>" +
                                "<td colspan=\"2\">" +
                                    ": " + DAO_KelasDet.GetByID_Entity(rel_kelasdet).Nama +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td colspan=\"2\" style=\"font-weight: bold;\">" +
                                    "KKM Pengetahuan" +
                                "</td>" +
                                "<td colspan=\"2\">" +
                                    ": " + m_struktur.KKM.ToString() +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td colspan=\"2\">" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td colspan=\"2\">" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td colspan=\"2\">" +
                                    "Rapor Ke-" +
                                "</td>" +
                                "<td colspan=\"2\">" +
                                    ": " +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td colspan=\"2\">" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td colspan=\"2\">" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td style=\"text-align: center;\">" +
                                    "Sikap" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td rowspan=\"2\" style=\"vertical-align: middle; text-align: center; font-weight: bold; border-style: solid; border-width: 1px;\">" +
                                    "No" +
                                "</td>" +
                                "<td rowspan=\"2\" style=\"vertical-align: middle; text-align: center; font-weight: bold; border-style: solid; border-width: 1px;\">" +
                                    "Nama Siswa" +
                                "</td>" +
                                "<td rowspan=\"2\" style=\"vertical-align: middle; text-align: center; font-weight: bold; border-style: solid; border-width: 1px;\">" +
                                    "NIS" +
                                "</td>" +
                                "<td rowspan=\"2\" style=\"vertical-align: middle; text-align: center; font-weight: bold; border-style: solid; border-width: 1px;\">" +
                                    "NISN" +
                                "</td>" +
                                "<td style=\"vertical-align: middle; text-align: center; font-weight: bold; border-style: solid; border-width: 1px;\">" +
                                    "Pengetahuan" +
                                "</td>" +
                                "<td style=\"vertical-align: middle; text-align: center; font-weight: bold; border-style: solid; border-width: 1px;\">" +
                                    "Sikap Spiritual" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<td style=\"vertical-align: middle; text-align: center; font-weight: bold; border-style: solid; border-width: 1px;\">" +
                                    "Nilai" +
                                "</td>" +
                                "<td style=\"vertical-align: middle; text-align: center; font-weight: bold; border-style: solid; border-width: 1px;\">" +
                                    "Predikat" +
                                "</td>" +
                                "<td>" +
                                    "&nbsp;" +
                                "</td>" +
                           "</tr>";

                    string html_lst_nilai_siswa = "";
                    Sekolah m_sekolah = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMP).FirstOrDefault();
                    List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                                        m_sekolah.Kode.ToString(),
                                        rel_kelasdet,
                                        tahun_ajaran,
                                        semester
                                    );
                    int nomor = 1;
                    Rapor_Nilai m_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                            tahun_ajaran, semester, rel_kelasdet, rel_mapel
                        ).FirstOrDefault();

                    if (m_nilai != null)
                    {
                        if (m_nilai.TahunAjaran != null)
                        {

                            foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa.OrderBy(m => m.Nama))
                            {
                                Rapor_NilaiSiswa m_nilaisiswa = DAO_Rapor_NilaiSiswa.GetAllByHeaderBySiswa_Entity(m_nilai.Kode.ToString(), m_siswa.Kode.ToString()).FirstOrDefault();
                                if (m_nilaisiswa != null)
                                {
                                    if (m_nilaisiswa.Rel_Siswa != null)
                                    {
                                        html_lst_nilai_siswa +=
                                            "<tr>" +
                                                "<td style=\"vertical-align: middle; text-align: center; border-style: solid; border-width: 1px;\">" +
                                                    nomor.ToString() +
                                                "</td>" +
                                                "<td style=\"vertical-align: middle; text-align: left; border-style: solid; border-width: 1px;\">" +
                                                    m_siswa.Nama.ToUpper() +
                                                "</td>" +
                                                "<td style=\"vertical-align: middle; text-align: center; border-style: solid; border-width: 1px;\">" +
                                                    m_siswa.NISSekolah.ToUpper() +
                                                "</td>" +
                                                "<td style=\"vertical-align: middle; text-align: center; border-style: solid; border-width: 1px; width: 200px;\">" +
                                                    "&nbsp;" +
                                                    m_siswa.NISN.ToUpper() +
                                                "</td>" +
                                                "<td style=\"vertical-align: middle; text-align: left; border-style: solid; border-width: 1px; text-align: right;\">" +
                                                    m_nilaisiswa.Rapor +
                                                "</td>" +
                                                "<td style=\"vertical-align: middle; text-align: center; border-style: solid; border-width: 1px;\">" +
                                                    DAO_Rapor_NilaiSiswa.GetPredikatNilai(m_nilaisiswa.Rapor) +
                                                "</td>" +
                                                "<td>" +
                                                    "&nbsp;" +
                                                "</td>" +
                                           "</tr>";
                                        nomor++;

                                    }
                                }
                            }

                        }
                    }
                    
                    html += html_lst_nilai_siswa;
                }
            }

            return "<table style=\"margin: 0px;\">" +
                        html +
                   "</table>";
        }
    }
}