using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Drawing;
using System.Web.UI;
using System.Threading;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_Entities.Elearning.SMA.Reports;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.Application_Libs
{
    public static class Reports_SMA
    {
        public class NilaiRaporKURTILAS
        {
            public string IDSiswa { get; set; }
            public string Nama { get; set; }
            public string NIS { get; set; }
            public string NISN { get; set; }
            public string Kelas { get; set; }
            public string Semester { get; set; }
            public string TahunAjaran { get; set; }
            public string NamaSekolah { get; set; }
            public string Peminatan { get; set; }
            public string KKM { get; set; }
            public string PoinKelompokNilai { get; set; }
            public int UrutanKelompokNilai { get; set; }
            public string KelompokNilai { get; set; }
            public string PoinKelompokMapel { get; set; }
            public int UrutanKelompokMapel { get; set; }
            public string KelompokMapel { get; set; }
            public string NomorMapel { get; set; }
            public string Rel_Mapel { get; set; }
            public string Mapel { get; set; }
            public Decimal Nilai { get; set; }
            public string Predikat { get; set; }
            public string Deskripsi { get; set; }
            public string Sakit { get; set; }
            public string Izin { get; set; }
            public string Alpa { get; set; }
            public string CatatanWaliKelas { get; set; }
            public string WaliKelas { get; set; }
            public string KepalaSekolah { get; set; }
            public string TanggalRapor { get; set; }
            public string Halaman { get; set; }
            public string PredikatSikapSpiritual { get; set; }
            public string PredikatSikapSosial { get; set; }
            public byte[] TTDGuru { get; set; }
            public byte[] TTDKepsek { get; set; }
            public byte[] QRCode { get; set; }
            public string LS_JumlahJam { get; set; }
            public string LS_Deskripsi { get; set; }
            public string KW_JumlahJam { get; set; }
            public string KW_Deskripsi { get; set; }
            public string IN_JumlahJam { get; set; }
            public string IN_Deskripsi { get; set; }
            public string NaikKelas { get; set; }

        }

        public class NilaiRaporEkskul
        {
            public string IDSiswa { get; set; }
            public string JenisKegiatan { get; set; }
            public string Predikat { get; set; }
            public string Deskripsi { get; set; }
            public string Sakit { get; set; }
            public string Izin { get; set; }
            public string Alpa { get; set; }
            public int Urutan { get; set; }
        }

        public class NilaiRaporVolunteer
        {
            public string IDSiswa { get; set; }
            public string Kegiatan { get; set; }
            public DateTime Tanggal { get; set; }
            public string JumlahJam { get; set; }
            public string Keterangan { get; set; }
            public int Urutan { get; set; }
        }

        public class NilaiRaporSikap
        {
            public string IDSiswa { get; set; }
            public string DeskripsiSikapSpiritual { get; set; }
            public string NilaiSikapSpiritual { get; set; }
            public string DeskripsiSikapSosial { get; set; }
            public string NilaiSikapSosial { get; set; }
        }

        public class NilaiLTS
        {
            public int UrutanTagihan { get; set; }
            public string Nilai { get; set; }
            public string DeskripsiLTS { get; set; }
        }

        public static Sekolah GetUnitSekolah()
        {
            Sekolah m_unit = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMA).FirstOrDefault();
            return m_unit;
        }

        public class RaporSemester
        {
            public string TahunAjaran;
            public string Semester;
            public string Rel_KelasDet;
            public string LokasiTTD;
            public string Rel_Siswa;
            public string WaliKelas;

            public List<NilaiRaporKURTILAS> GetRaporSemester;
            public List<NilaiRaporEkskul> GetRaporEkskul;
            public List<NilaiRaporVolunteer> GetRaporVolunteer;
            public List<Rapor_CapaianKedisiplinan> GetRaporCapaianKedisiplinan;
            public List<NilaiRaporSikap> GetRaporSikap;

            public class NilaiMapelKurtilas
            {
                public string Rel_Rapor_StrukturNilai { get; set; }
                public string Rel_Rapor_StrukturNilai_KURTILAS_AP { get; set; }
                public string Rel_Rapor_StrukturNilai_KURTILAS_KD { get; set; }
                public string JenisKD { get; set; }
                public string Rel_Rapor_StrukturNilai_KURTILAS_KP { get; set; }
                public string Deskripsi_KD { get; set; }
            }

            public class NilaiHarian: NilaiMapelKurtilas
            {
                public string Nilai { get; set; }
            }

            public class MapelLedger
            {
                public List<Rapor_Desain_Det> MapelRapor { get; set; }
                public int Urutan { get; set; }
            }

            public class NilaiPerMapel {
                public string Rel_Mapel { get; set; }
                public decimal KKM { get; set; }
                public decimal Nilai_Pengetahuan { get; set; }
                public decimal Nilai_Keterampilan { get; set; }
            }

            public class NilaiLedgerPerSiswa
            {
                public string Rel_Siswa { get; set; }
                public decimal Nilai { get; set; }
            }

            public string GetHTMLLedger()
            {
                string rel_siswa = "";
                string hasil = "";
                string s_html = "";

                string css_cell_headers = "border-style: solid; border-width: 1px; border-color: black; text-align: center; font-weight: normal; padding: 3px;";
                string css_cell_body = "border-style: solid; border-width: 1px; border-color: black; padding: 1px; padding-left: 5px; padding-right: 5px;";

                string html_row_header = "<td rowspan=\"3\" style=\"" + css_cell_headers + " font-size: 8pt; width: 20px; \">" +
                                            "NO" +
                                         "</td>" +
                                         "<td rowspan=\"3\" style=\"" + css_cell_headers + " font-size: 8pt; width: 50px; \">" +
                                            "NIS" +
                                         "</td>" +
                                         "<td rowspan=\"3\" style=\"" + css_cell_headers + " font-size: 8pt; \">" +
                                            "NAMA" +
                                         "</td>" +
                                         "<td rowspan=\"3\" style=\"" + css_cell_headers + " font-size: 8pt; \">" +
                                            "L/P" +
                                         "</td>";

                List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                        DAO_Sekolah.GetKodeSekolah(Libs.UnitSekolah.SMA).ToString(),
                        this.Rel_KelasDet,
                        this.TahunAjaran,
                        this.Semester
                    );

                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(this.Rel_KelasDet);
                List<Rapor_StrukturNilai_KURTILAS> lst_sn = DAO_Rapor_StrukturNilai_KURTILAS.GetAllByTABySMByKelas_Entity(
                        this.TahunAjaran,
                        this.Semester,
                        m_kelas_det.Rel_Kelas.ToString()
                    );

                List<Rapor_Desain> lst_rapor_desain = DAO_Rapor_Desain.GetByTABySM_Entity(this.TahunAjaran, this.Semester);
                List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByTABySMByJenisRapor_Entity(this.TahunAjaran, this.Semester, DAO_Rapor_Desain.JenisRapor.Semester);

                string nama_kelas = m_kelas_det.Nama + "-";
                string[] arr_nama_kelas = nama_kelas.Split(new string[] { "-" }, StringSplitOptions.None);
                string nama_kelas_ok = "";
                int id_str = 0;

                foreach (string item_nama_kelas in arr_nama_kelas)
                {
                    if (id_str == 2)
                    {
                        break;
                    }
                    nama_kelas_ok += (nama_kelas_ok.Trim() != "" ? "-" : "") + item_nama_kelas;
                    id_str++;
                }

                Rapor_Desain m_rapor_desain = lst_rapor_desain.
                    FindAll(
                        m0 => m0.Rel_Kelas.Trim().ToUpper() == nama_kelas_ok.Trim().ToUpper() &&
                              m0.JenisRapor == DAO_Rapor_Desain.JenisRapor.Semester
                    ).FirstOrDefault();

                //list cols header                
                string html_row_mapel = "";
                string html_row_jenis_nilai = "";
                string html_caption_nilai = "";
                string html_row_body = "";
                string html_footer_rata_rata = "";
                string html_footer_kkm = "";

                string key_jumlah_pengetahuan_per_siswa = "siswa_jumlah_pengetahuan_";
                string key_rata_pengetahuan_per_siswa = "siswa_rata_pengetahuan_";
                string key_jumlah_keterampilan_per_siswa = "siswa_jumlah_keterampilan_";
                string key_rata_keterampilan_per_siswa = "siswa_rata_keterampilan_";
                string key_jumlah_total_per_siswa = "siswa_jumlah_total_";
                string key_rata_total_per_siswa = "siswa_rata_total_";

                int i_jumlah_col_mapel = 4;

                foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa)
                {
                    rel_siswa += m_siswa.Kode.ToString() + ";";
                }
                List<NilaiRaporKURTILAS> lst_nilai_rapor = get_RaporSemester(this.TahunAjaran, this.Semester, this.Rel_KelasDet, rel_siswa, true);
                if (m_rapor_desain != null)
                {
                    if (m_rapor_desain.TahunAjaran != null)
                    {
                        List<Rapor_Desain_Det> lst_rapor_desain_det_ = lst_rapor_desain_det.FindAll(
                                m0 => m0.Rel_Rapor_Desain.ToString().ToUpper().Trim() == m_rapor_desain.Kode.ToString().ToUpper().Trim()
                            );

                        List<Rapor_Desain_Det> lst_mapel = new List<Rapor_Desain_Det>();
                        List<MapelLedger> lst_mapel_ledger = new List<MapelLedger>();
                        lst_mapel_ledger.Clear();
                        int nomor_mapel = 0;
                        int urutan = 0;
                        var lst_list_mapel_rapor = lst_rapor_desain_det_.FindAll(
                                m0 => m0.Rel_Mapel.Trim() != ""
                            );
                        lst_mapel.Clear();

                        lst_list_mapel_rapor.Add(new Rapor_Desain_Det
                        {
                            Kode = new Guid(Constantas.GUID_NOL),
                            Rel_Mapel = Constantas.GUID_NOL,
                            NamaMapelRapor = Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN
                        });
                        lst_list_mapel_rapor.Add(new Rapor_Desain_Det
                        {
                            Kode = new Guid(Constantas.GUID_NOL),
                            Rel_Mapel = Constantas.GUID_NOL,
                            NamaMapelRapor = Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN
                        });
                        lst_list_mapel_rapor.Add(new Rapor_Desain_Det
                        {
                            Kode = new Guid(Constantas.GUID_NOL),
                            Rel_Mapel = Constantas.GUID_NOL,
                            NamaMapelRapor = Libs.JenisKomponenNilaiKURTILAS.SMA.TOTAL
                        });
                        foreach (Rapor_Desain_Det item_desain_det in lst_list_mapel_rapor.ToList())
                        {
                            nomor_mapel++;
                            lst_mapel.Add(item_desain_det);

                            if (nomor_mapel % i_jumlah_col_mapel == 0 || nomor_mapel == lst_list_mapel_rapor.Count)
                            {
                                List<Rapor_Desain_Det> lst_item_mapel = lst_mapel;
                                lst_mapel_ledger.Add(new MapelLedger
                                {
                                    MapelRapor = lst_item_mapel.ToList(),
                                    Urutan = urutan
                                });
                                urutan++;
                                lst_mapel.Clear();
                            }
                        }
                        
                        int id_mapel_ledger = 0;
                        List<NilaiLedgerPerSiswa> lst_nilai_pengetahuan_per_siswa = new List<NilaiLedgerPerSiswa>();
                        List<NilaiLedgerPerSiswa> lst_nilai_keterampilan_per_siswa = new List<NilaiLedgerPerSiswa>();
                        foreach (MapelLedger item_mapel_ledger in lst_mapel_ledger)
                        {
                            s_html += "<div style=\"font-size: 10pt; width: 100%;\">" +
                                         "DAFTAR NILAI RAPOR SISWA (LEGGER)<br />" +
                                         "SMA ISLAM AL-IZHAR PONDOK LABU<br />" +
                                         "SEMESTER " + Semester + " TAHUN " + TahunAjaran + "<br />" +
                                         "<label style=\"float: left;\">" +
                                            "KELAS : " + DAO_KelasDet.GetByID_Entity(m_kelas_det.Kode.ToString()).Nama.Trim().ToUpper() +
                                         "</label>" +
                                         "<label style=\"float: right;\">" +
                                            "Wali Kelas : " +
                                            this.WaliKelas +
                                         "</label>" +
                                         "<br />";

                            int nomor = 1;
                            string html_table_header = "";

                            html_row_body = "";
                            html_row_mapel = "";
                            html_row_jenis_nilai = "";
                            html_caption_nilai = "";
                            html_footer_rata_rata = "";
                            html_footer_kkm = "";

                            List<NilaiPerMapel> lst_nilai_per_mapel = new List<NilaiPerMapel>();
                            lst_nilai_per_mapel.Clear();

                            decimal jumlah_pengetahuan_per_siswa = 0;
                            decimal rata_rata_pengetahuan_per_siswa = 0;
                            decimal jumlah_keterampilan_per_siswa = 0;
                            decimal rata_rata_keterampilan_per_siswa = 0;
                            foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa)
                            {
                                string html_row_body_per_siswa = "";
                                jumlah_pengetahuan_per_siswa = 0;
                                rata_rata_pengetahuan_per_siswa = 0;
                                jumlah_keterampilan_per_siswa = 0;
                                rata_rata_keterampilan_per_siswa = 0;
                                if (nomor == 1)
                                {
                                    foreach (Rapor_Desain_Det item_desain_det in item_mapel_ledger.MapelRapor)
                                    {
                                        if (item_desain_det.Rel_Mapel.Trim() != "" && item_desain_det.Rel_Mapel.ToString() != Constantas.GUID_NOL)
                                        {
                                            html_row_mapel += "<td colspan=\"6\" style=\"" + css_cell_headers + " font-size: 8pt; \">" +
                                                                (
                                                                    item_desain_det.Alias.Trim() != ""
                                                                    ? item_desain_det.Alias
                                                                    : item_desain_det.NamaMapelRapor.Trim().ToUpper()
                                                                ) +
                                                              "</td>";

                                            html_row_jenis_nilai += "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8pt; \">" +
                                                                            "KI-1 :<br />S.Spiritual" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8pt; \">" +
                                                                            "KI-2 :<br />S.Sosial" +
                                                                    "</td>" +
                                                                    "<td colspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8pt; \">" +
                                                                            "KI-3 :<br />Pengetahuan" +
                                                                    "</td>" +
                                                                    "<td colspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8pt; \">" +
                                                                            "KI-4 :<br />Keterampilan" +
                                                                    "</td>";

                                            html_caption_nilai += "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8pt; \">" +
                                                                            "Predikat" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8pt; \">" +
                                                                            "Predikat" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8pt; \">" +
                                                                            "Nilai" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8pt; \">" +
                                                                            "Predikat" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8pt; \">" +
                                                                            "Nilai" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8pt; \">" +
                                                                            "Predikat" +
                                                                    "</td>";
                                        }
                                        else if (item_desain_det.Rel_Mapel.Trim() != "" && item_desain_det.Rel_Mapel.ToString() == Constantas.GUID_NOL)
                                        {
                                            if (item_desain_det.NamaMapelRapor == Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN)
                                            {
                                                html_row_mapel += "<td rowspan=\"2\" colspan=\"2\" style=\"" + css_cell_headers + " font-size: 8pt; \">" +
                                                                    "KI - 3<br />" +
                                                                    "Pengetahuan" +
                                                                  "</td>";
                                            }
                                            else if (item_desain_det.NamaMapelRapor == Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN)
                                            {
                                                html_row_mapel += "<td rowspan=\"2\" colspan=\"2\" style=\"" + css_cell_headers + " font-size: 8pt; \">" +
                                                                    "KI - 4<br />" +
                                                                    "Keterampilan" +
                                                                  "</td>";
                                            }
                                            else if (item_desain_det.NamaMapelRapor == Libs.JenisKomponenNilaiKURTILAS.SMA.TOTAL)
                                            {
                                                html_row_mapel += "<td rowspan=\"2\" colspan=\"2\" style=\"" + css_cell_headers + " font-size: 8pt; \">" +
                                                                    "RATA-RATA NILAI<br />" +
                                                                    "RAPOR" +
                                                                  "</td>";
                                            }

                                            html_caption_nilai += "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8pt; \">" +
                                                                        "JUMLAH" +
                                                                  "</td>" +
                                                                  "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8pt; \">" +
                                                                        "RATA2" +
                                                                  "</td>";
                                        }
                                    }

                                    html_table_header += 
                                                      "<tr>" +
                                                            html_row_header +
                                                            html_row_mapel +
                                                      "</tr>" +
                                                      "<tr>" +
                                                            html_row_jenis_nilai +
                                                      "</tr>" +
                                                      "<tr>" +
                                                            html_caption_nilai +
                                                      "</tr>";
                                }

                                html_row_body_per_siswa +=
                                        "<td style=\"" + css_cell_headers + " font-size: 8pt; \">" +
                                            nomor.ToString() +
                                        "</td>" +
                                        "<td style=\"" + css_cell_body + " font-size: 8pt; text-align: center;\">" +
                                            m_siswa.NISSekolah +
                                        "</td>" +
                                        "<td style=\"" + css_cell_body + " white-space: nowrap; font-size: 8pt; \">" +
                                            m_siswa.Nama.ToString().ToUpper().Trim() +
                                        "</td>" +
                                        "<td style=\"" + css_cell_body + " font-size: 8pt; text-align: center; \">" +
                                            m_siswa.JenisKelamin +
                                        "</td>";

                                foreach (Rapor_Desain_Det item_desain_det in item_mapel_ledger.MapelRapor)
                                {
                                    if (item_desain_det.Rel_Mapel != Constantas.GUID_NOL)
                                    {
                                        string nilai_pengetahuan = "";
                                        string nilai_keterampilan = "";
                                        string predikat_pengetahuan = "";
                                        string predikat_keterampilan = "";

                                        var m_nilai_pengetahuan = lst_nilai_rapor.FindAll(
                                                m0 =>   m0.Rel_Mapel.Trim().ToUpper() == item_desain_det.Rel_Mapel.Trim().ToUpper() &&
                                                        m0.KelompokNilai == Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN &&
                                                        m0.IDSiswa.Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper()
                                            ).FirstOrDefault();                                        
                                        var m_nilai_keterampilan = lst_nilai_rapor.FindAll(
                                                m0 =>   m0.Rel_Mapel.Trim().ToUpper() == item_desain_det.Rel_Mapel.Trim().ToUpper() &&
                                                        m0.KelompokNilai == Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN &&
                                                        m0.IDSiswa.Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper()
                                            ).FirstOrDefault();

                                        string s_sikap_spiritual = "";
                                        string s_sikap_sosial = "";
                                        decimal d_kkm_pengetahuan = 0;
                                        decimal d_kkm_keterampilan = 0;

                                        if (m_nilai_pengetahuan != null)
                                        {
                                            if (m_nilai_pengetahuan.Nama != null)
                                            {
                                                nilai_pengetahuan = Math.Round(
                                                                        m_nilai_pengetahuan.Nilai,
                                                                        Constantas.PEMBULATAN_DESIMAL_NILAI_SMA,
                                                                        MidpointRounding.AwayFromZero
                                                                    ).ToString();
                                                lst_nilai_pengetahuan_per_siswa.Add(
                                                        new NilaiLedgerPerSiswa {
                                                            Rel_Siswa = m_siswa.Kode.ToString().ToUpper().Trim(),
                                                            Nilai = Libs.GetStringToDecimal(nilai_pengetahuan)
                                                        }
                                                    );
                                                predikat_pengetahuan = m_nilai_pengetahuan.Predikat;
                                                s_sikap_spiritual = m_nilai_pengetahuan.PredikatSikapSpiritual;
                                                s_sikap_sosial = m_nilai_pengetahuan.PredikatSikapSosial;
                                                d_kkm_pengetahuan = Libs.GetStringToDecimal(m_nilai_pengetahuan.KKM);
                                            }
                                        }
                                        if (m_nilai_keterampilan != null)
                                        {
                                            if (m_nilai_keterampilan.Nama != null)
                                            {
                                                nilai_keterampilan = Math.Round(
                                                                        m_nilai_keterampilan.Nilai,
                                                                        Constantas.PEMBULATAN_DESIMAL_NILAI_SMA,
                                                                        MidpointRounding.AwayFromZero
                                                                    ).ToString();
                                                lst_nilai_keterampilan_per_siswa.Add(
                                                        new NilaiLedgerPerSiswa
                                                        {
                                                            Rel_Siswa = m_siswa.Kode.ToString().ToUpper().Trim(),
                                                            Nilai = Libs.GetStringToDecimal(nilai_keterampilan)
                                                        }
                                                    );
                                                predikat_keterampilan = m_nilai_keterampilan.Predikat;
                                                s_sikap_spiritual = m_nilai_keterampilan.PredikatSikapSpiritual;
                                                s_sikap_sosial = m_nilai_keterampilan.PredikatSikapSosial;
                                                d_kkm_keterampilan = Libs.GetStringToDecimal(m_nilai_keterampilan.KKM);
                                            }
                                        }

                                        if (item_desain_det.JenisMapel == Libs.JENIS_MAPEL.LINTAS_MINAT ||
                                            item_desain_det.JenisMapel == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN ||
                                            item_desain_det.JenisMapel == Libs.JENIS_MAPEL.PILIHAN
                                            )
                                        {
                                            if (
                                                    Libs.GetStringToDecimal(nilai_pengetahuan) > 0 ||
                                                    Libs.GetStringToDecimal(nilai_keterampilan) > 0
                                               )
                                            {
                                                lst_nilai_per_mapel.Add(new NilaiPerMapel
                                                {
                                                    Rel_Mapel = item_desain_det.Rel_Mapel,
                                                    Nilai_Pengetahuan = Libs.GetStringToDecimal(nilai_pengetahuan),
                                                    Nilai_Keterampilan = Libs.GetStringToDecimal(nilai_keterampilan)
                                                });
                                            }
                                        }
                                        else
                                        {
                                            lst_nilai_per_mapel.Add(new NilaiPerMapel
                                            {
                                                Rel_Mapel = item_desain_det.Rel_Mapel,
                                                Nilai_Pengetahuan = Libs.GetStringToDecimal(nilai_pengetahuan),
                                                Nilai_Keterampilan = Libs.GetStringToDecimal(nilai_keterampilan)
                                            });
                                        }
                                        
                                        html_row_body_per_siswa +=
                                                            "<td style=\"" + css_cell_body + " width: 55px; padding-left: 1px; padding-right: 1px; font-size: 6pt; text-align: center; \">" +
                                                                    s_sikap_spiritual +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_body + " width: 55px; padding-left: 1px; padding-right: 1px; font-size: 6pt; text-align: center; \">" +
                                                                    s_sikap_sosial +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_body + (Libs.GetStringToDecimal(nilai_pengetahuan) < d_kkm_pengetahuan ? " color: red; " : "") + " width: 30px; font-size: 8pt; text-align: right; \">" +
                                                                    nilai_pengetahuan +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_body + " width: 30px; font-size: 8pt; text-align: center; \">" +
                                                                    predikat_pengetahuan +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_body + (Libs.GetStringToDecimal(nilai_keterampilan) < d_kkm_keterampilan ? " color: red; " : "") + " width: 30px; font-size: 8pt; text-align: right; \">" +
                                                                    nilai_keterampilan +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_body + " width: 30px; font-size: 8pt; text-align: center; \">" +
                                                                    predikat_keterampilan +
                                                            "</td>";
                                    }
                                    else
                                    {
                                        switch (item_desain_det.NamaMapelRapor)
                                        {
                                            case Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN:
                                                html_row_body_per_siswa +=
                                                            "<td style=\"" + css_cell_body + " width: 30px; font-size: 8pt; text-align: right; \">" +
                                                                key_jumlah_pengetahuan_per_siswa + m_siswa.Kode.ToString().ToUpper().Trim() + //jumlah per siswa
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_body + " width: 30px; font-size: 8pt; text-align: right; \">" +
                                                                key_rata_pengetahuan_per_siswa + m_siswa.Kode.ToString().ToUpper().Trim() + //rata2 per siswa
                                                            "</td>";
                                                break;
                                            case Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN:
                                                html_row_body_per_siswa +=
                                                            "<td style=\"" + css_cell_body + " width: 30px; font-size: 8pt; text-align: right; \">" +
                                                                key_jumlah_keterampilan_per_siswa + m_siswa.Kode.ToString().ToUpper().Trim() + //jumlah per siswa
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_body + " width: 30px; font-size: 8pt; text-align: right; \">" +
                                                                key_rata_keterampilan_per_siswa + m_siswa.Kode.ToString().ToUpper().Trim() + //rata2 per siswa
                                                            "</td>";
                                                break;
                                            case Libs.JenisKomponenNilaiKURTILAS.SMA.TOTAL:
                                                html_row_body_per_siswa +=
                                                            "<td style=\"" + css_cell_body + " width: 30px; font-size: 8pt; text-align: right; \">" +
                                                                key_jumlah_total_per_siswa + m_siswa.Kode.ToString().ToUpper().Trim() + //jumlah per siswa
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_body + " width: 30px; font-size: 8pt; text-align: right; \">" +
                                                                key_rata_total_per_siswa + m_siswa.Kode.ToString().ToUpper().Trim() + //rata2 per siswa
                                                            "</td>";
                                                break;
                                            default:
                                                html_row_body_per_siswa +=
                                                            "<td style=\"" + css_cell_body + " width: 30px; font-size: 8pt; \">" +
                                                                "&nbsp;" + //jumlah per siswa
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_body + " width: 30px; font-size: 8pt; \">" +
                                                                "&nbsp;" + //rata2 per siswa
                                                            "</td>";
                                                break;
                                        }
                                    }
                                }

                                jumlah_pengetahuan_per_siswa =
                                    (
                                        lst_nilai_pengetahuan_per_siswa.Count > 0
                                        ? lst_nilai_pengetahuan_per_siswa.FindAll(
                                                m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                                            ).Select(
                                                m0 => m0.Nilai
                                            ).Sum()
                                        : 0
                                    );
                                rata_rata_pengetahuan_per_siswa =
                                    (
                                        lst_nilai_pengetahuan_per_siswa.Count > 0
                                        ? lst_nilai_pengetahuan_per_siswa.FindAll(
                                                m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                                            ).Select(
                                                m0 => m0.Nilai
                                            ).Average()
                                        : 0
                                    );
                                jumlah_keterampilan_per_siswa =
                                                    (
                                        lst_nilai_keterampilan_per_siswa.Count > 0
                                        ? lst_nilai_keterampilan_per_siswa.FindAll(
                                                m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                                            ).Select(
                                                m0 => m0.Nilai
                                            ).Sum()
                                        : 0
                                    );
                                rata_rata_keterampilan_per_siswa =
                                    (
                                        lst_nilai_keterampilan_per_siswa.Count > 0
                                        ? lst_nilai_keterampilan_per_siswa.FindAll(
                                                m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                                            ).Select(
                                                m0 => m0.Nilai
                                            ).Average()
                                        : 0
                                    );

                                html_row_body_per_siswa = html_row_body_per_siswa.Replace(
                                        key_jumlah_pengetahuan_per_siswa + m_siswa.Kode.ToString().ToUpper().Trim(), 
                                        Math.Round(jumlah_pengetahuan_per_siswa, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA, MidpointRounding.AwayFromZero).ToString()
                                    );
                                html_row_body_per_siswa = html_row_body_per_siswa.Replace(
                                        key_rata_pengetahuan_per_siswa + m_siswa.Kode.ToString().ToUpper().Trim(),
                                        Math.Round(rata_rata_pengetahuan_per_siswa, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA, MidpointRounding.AwayFromZero).ToString()
                                    );

                                html_row_body_per_siswa = html_row_body_per_siswa.Replace(
                                        key_jumlah_keterampilan_per_siswa + m_siswa.Kode.ToString().ToUpper().Trim(),
                                        Math.Round(jumlah_keterampilan_per_siswa, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA, MidpointRounding.AwayFromZero).ToString()
                                    );
                                html_row_body_per_siswa = html_row_body_per_siswa.Replace(
                                        key_rata_keterampilan_per_siswa + m_siswa.Kode.ToString().ToUpper().Trim(),
                                        Math.Round(rata_rata_keterampilan_per_siswa, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA, MidpointRounding.AwayFromZero).ToString()
                                    );

                                html_row_body_per_siswa = html_row_body_per_siswa.Replace(
                                        key_jumlah_total_per_siswa + m_siswa.Kode.ToString().ToUpper().Trim(),
                                        Math.Round(jumlah_pengetahuan_per_siswa + jumlah_keterampilan_per_siswa, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA, MidpointRounding.AwayFromZero).ToString()
                                    );
                                html_row_body_per_siswa = html_row_body_per_siswa.Replace(
                                        key_rata_total_per_siswa + m_siswa.Kode.ToString().ToUpper().Trim(),
                                        Math.Round((rata_rata_pengetahuan_per_siswa + rata_rata_keterampilan_per_siswa) / 2, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA, MidpointRounding.AwayFromZero).ToString()
                                    );

                                html_row_body += "<tr>" +
                                                    html_row_body_per_siswa +
                                                 "</tr>";

                                nomor++;
                            }

                            html_footer_rata_rata = 
                                              "<td colspan=\"3\" style=\"" + css_cell_body + " width: 30px; font-size: 8pt; text-align: right; \">" +
                                                "Rata-Rata" +
                                              "</td>" +
                                              "<td style=\"" + css_cell_body + " width: 30px; font-size: 8pt; text-align: center; \">" +
                                                "&nbsp;" +
                                              "</td>";
                            html_footer_kkm =
                                              "<td colspan=\"3\" style=\"" + css_cell_body + " width: 30px; font-size: 8pt; text-align: right; \">" +
                                                "KKM" +
                                              "</td>" +
                                              "<td style=\"" + css_cell_body + " width: 30px; font-size: 8pt; text-align: center; \">" +
                                                "&nbsp;" +
                                              "</td>";

                            foreach (Rapor_Desain_Det item_desain_det in item_mapel_ledger.MapelRapor)
                            {
                                Rapor_StrukturNilai_KURTILAS m_sn = lst_sn.FindAll(
                                    m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == item_desain_det.Rel_Mapel.ToString().ToUpper().Trim()
                                ).FirstOrDefault();

                                if (m_sn != null)
                                {
                                    decimal rata_rata_pengetahuan =
                                        (
                                            lst_nilai_per_mapel.Count > 0
                                            ? (
                                                lst_nilai_per_mapel.FindAll(
                                                    m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == item_desain_det.Rel_Mapel.ToString().ToUpper().Trim() &&
                                                            m0.Rel_Mapel != Constantas.GUID_NOL &&
                                                            m0.Nilai_Pengetahuan.ToString().Trim() != ""
                                                ).Count > 0
                                                ? Math.Round(
                                                        lst_nilai_per_mapel.FindAll(
                                                                m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == item_desain_det.Rel_Mapel.ToString().ToUpper().Trim() &&
                                                                      m0.Rel_Mapel != Constantas.GUID_NOL &&
                                                                      m0.Nilai_Pengetahuan.ToString().Trim() != ""
                                                            ).Select(m0 => m0.Nilai_Pengetahuan).Average()
                                                    , 0, MidpointRounding.AwayFromZero)
                                                : 0
                                              )
                                            : 0
                                        );

                                    decimal rata_rata_keterampilan =
                                        (
                                            lst_nilai_per_mapel.Count > 0
                                            ? (
                                                lst_nilai_per_mapel.FindAll(
                                                    m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == item_desain_det.Rel_Mapel.ToString().ToUpper().Trim() &&
                                                            m0.Rel_Mapel != Constantas.GUID_NOL &&
                                                            m0.Nilai_Keterampilan.ToString().Trim() != ""
                                                ).Count > 0
                                                ? Math.Round(
                                                        lst_nilai_per_mapel.FindAll(
                                                                m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == item_desain_det.Rel_Mapel.ToString().ToUpper().Trim() &&
                                                                      m0.Rel_Mapel != Constantas.GUID_NOL &&
                                                                      m0.Nilai_Keterampilan.ToString().Trim() != ""
                                                            ).Select(m0 => m0.Nilai_Keterampilan).Average()
                                                    , 0, MidpointRounding.AwayFromZero)
                                                : 0
                                              )
                                            : 0
                                        );

                                    html_footer_rata_rata +=
                                                       "<td style=\"" + css_cell_body + " font-size: 8pt; text-align: right; \">" +
                                                            "&nbsp;" +
                                                       "</td>" +
                                                       "<td style=\"" + css_cell_body + " font-size: 8pt; text-align: right; \">" +
                                                            "&nbsp;" +
                                                       "</td>" +
                                                       "<td style=\"" + css_cell_body + " font-size: 8pt; text-align: right; \">" +
                                                            Math.Round(rata_rata_pengetahuan, 0, MidpointRounding.AwayFromZero).ToString() +
                                                       "</td>" +
                                                       "<td style=\"" + css_cell_body + " font-size: 8pt; text-align: right; \">" +
                                                            "&nbsp;" +
                                                       "</td>" +
                                                       "<td style=\"" + css_cell_body + " font-size: 8pt; text-align: right; \">" +
                                                            Math.Round(rata_rata_keterampilan, 0, MidpointRounding.AwayFromZero).ToString() +
                                                       "</td>" +
                                                       "<td style=\"" + css_cell_body + " font-size: 8pt; text-align: right; \">" +
                                                            "&nbsp;" +
                                                       "</td>";

                                    html_footer_kkm += "<td colspan=\"6\" style=\"" + css_cell_body + " font-size: 8pt; text-align: right; \">" +
                                                            Math.Round(m_sn.KKM, 0, MidpointRounding.AwayFromZero).ToString() +
                                                       "</td>";
                                }
                                else
                                {
                                    html_footer_rata_rata +=
                                                       "<td style=\"" + css_cell_body + " font-size: 8pt; text-align: right; \">" +
                                                            "&nbsp;" + //kkm
                                                       "</td>" +
                                                       "<td style=\"" + css_cell_body + " font-size: 8pt; text-align: right; \">" +
                                                            "&nbsp;" + //rata2
                                                       "</td>";
                                }
                            }

                            if (item_mapel_ledger.MapelRapor.FindAll(
                                    m0 => m0.Rel_Mapel == Constantas.GUID_NOL
                                ).Count > 0)
                            {
                                html_footer_kkm += "<td colspan=\"" +
                                                        (
                                                            lst_list_mapel_rapor.FindAll(
                                                                m0 => m0.Rel_Mapel == Constantas.GUID_NOL
                                                            ).Count * 2
                                                        ).ToString() +
                                                   "\" " +
                                                   "style=\"" + css_cell_body + " font-size: 8pt; text-align: right; \">" +
                                                        "&nbsp;" +
                                                   "</td>";
                            }

                            html_footer_kkm = "<tr>" +
                                                    html_footer_kkm +
                                              "<tr>";
                            html_footer_rata_rata = "<tr>" +
                                                        html_footer_rata_rata +
                                                    "<tr>";

                            id_mapel_ledger++;
                            s_html += "<table style=\"border-collapse: collapse; width: 100%;\">" +
                                            html_table_header +
                                            html_row_body +                                            
                                            html_footer_rata_rata +
                                            html_footer_kkm +
                                      "</table>";
                            s_html += "</div>" +
                                      "<br />" +
                                      "<br />" +
                                      "<div id=\"content\">" +
                                          "<label style=\"float: left; font-size: 7pt;\">Tabel interval berdasarkan KKM : < KKM = D; 70 - 79 = C; 80 - 89 = B; 90 - 100 = A</label>" +                                          
                                          "<br />" +
                                          "<label style=\"font-size: 6pt;\">" +
                                            "Halaman " + id_mapel_ledger.ToString() +
                                          "</label>" +
                                          "<label style=\"float: right; font-size: 7pt;\">,&nbsp;Update tanggal " + Libs.GetTanggalIndonesiaFromDate(DateTime.Now, true) + "</label>" +
                                          "<br /><br />" +
                                      "</div>" +
                                      (
                                        id_mapel_ledger < lst_mapel_ledger.Count
                                        ? "<div class=\"pagebreak\"></div>"
                                        : ""
                                      );
                        }
                    }

                    hasil = s_html;                    
                }

                return hasil;
            }

            public List<NilaiMapelKurtilas> get_RaporMapel(
                    Rapor_StrukturNilai_KURTILAS m_sn_kurtilas,
                    List<Rapor_NilaiSiswa_KURTILAS_Det_Lengkap> lst_nilai_det,
                    List<Rapor_StrukturNilai_KURTILAS_AP> lst_sn_ap,
                    List<Rapor_StrukturNilai_KURTILAS_KD> lst_sn_kd,
                    List<Rapor_StrukturNilai_KURTILAS_KP> lst_sn_kp,
                    List<Rapor_AspekPenilaian> lst_ap,
                    List<Rapor_KompetensiDasar> lst_kd,
                    List<Rapor_KomponenPenilaian> lst_kp,
                    string s_kelas_formasi
                )
            {
                List<NilaiMapelKurtilas> lst_hasil = new List<NilaiMapelKurtilas>();

                int id_tagihan = 0;
                List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian_ = lst_sn_ap.FindAll(
                        m0 => m0.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() == m_sn_kurtilas.Kode.ToString().Trim().ToUpper()
                    );
                //load kurtilas ap
                foreach (Rapor_StrukturNilai_KURTILAS_AP m_sn_ap in lst_aspek_penilaian_)
                {
                    if (m_sn_ap != null)
                    {
                        if (m_sn_ap.JenisPerhitungan != null)
                        {
                            Rapor_AspekPenilaian m_ap =
                                lst_ap.FindAll(
                                    m0 => m0.Kode.ToString().Trim().ToUpper() == m_sn_ap.Rel_Rapor_AspekPenilaian.ToString().Trim().ToUpper()
                                ).FirstOrDefault();

                            if (m_ap != null)
                            {
                                if (m_ap.Nama != null)
                                {
                                    //load kurtilas kd
                                    int id_kd = 1;
                                    List<Rapor_StrukturNilai_KURTILAS_KD> lst_kompetensi_dasar_ =
                                        lst_sn_kd.FindAll(m0 => m0.Rel_Rapor_StrukturNilai_AP.ToString().Trim().ToUpper() == m_sn_ap.Kode.ToString().Trim().ToUpper());

                                    foreach (Rapor_StrukturNilai_KURTILAS_KD m_sn_kd in lst_kompetensi_dasar_)
                                    {
                                        if (m_sn_kd != null)
                                        {
                                            if (m_sn_kd.JenisPerhitungan != null)
                                            {
                                                Rapor_KompetensiDasar m_kd =
                                                    lst_kd.FindAll(
                                                        m0 => m0.Kode.ToString().Trim().ToUpper() == m_sn_kd.Rel_Rapor_KompetensiDasar.ToString().Trim().ToUpper()
                                                    ).FirstOrDefault();

                                                if (m_kd != null)
                                                {
                                                    if (m_kd.Nama != null)
                                                    {

                                                        //load kurtilas kp
                                                        List<Rapor_StrukturNilai_KURTILAS_KP> lst_komponen_penilaian_ =
                                                            lst_sn_kp.FindAll(m0 => m0.Rel_Rapor_StrukturNilai_KD.ToString().Trim().ToUpper() == m_sn_kd.Kode.ToString().Trim().ToUpper());

                                                        foreach (Rapor_StrukturNilai_KURTILAS_KP m_sn_kp in lst_komponen_penilaian_)
                                                        {
                                                            Rapor_KomponenPenilaian m_kp =
                                                                lst_kp.FindAll(
                                                                    m0 => m0.Kode.ToString().Trim().ToUpper() == m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString().Trim().ToUpper()
                                                                ).FirstOrDefault();

                                                            if (m_kp != null)
                                                            {
                                                                if (m_kp.Nama != null)
                                                                {
                                                                    if (s_kelas_formasi.Trim() != "")
                                                                    {
                                                                        if (lst_nilai_det.FindAll(
                                                                            m0 => m0.Rel_KelasDet.ToString().ToUpper().Trim() == s_kelas_formasi.ToString().ToUpper().Trim() &&
                                                                                  m0.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() == m_sn_ap.Kode.ToString().ToUpper() &&
                                                                                  m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() == m_sn_kd.Kode.ToString().ToUpper() &&
                                                                                  m0.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().ToUpper() == m_sn_kp.Kode.ToString().ToUpper() &&
                                                                                  m0.Nilai.Trim() != ""
                                                                        ).Count > 0)
                                                                        {
                                                                            id_tagihan++;

                                                                            lst_hasil.Add(new NilaiMapelKurtilas
                                                                            {
                                                                                Rel_Rapor_StrukturNilai = m_sn_kurtilas.Kode.ToString().ToUpper(),
                                                                                Rel_Rapor_StrukturNilai_KURTILAS_AP = m_sn_ap.Kode.ToString().ToUpper(),
                                                                                Rel_Rapor_StrukturNilai_KURTILAS_KD = m_sn_kd.Kode.ToString().ToUpper(),
                                                                                JenisKD = Libs.GetHTMLSimpleText(m_kd.Nama),
                                                                                Rel_Rapor_StrukturNilai_KURTILAS_KP = m_sn_kp.Kode.ToString().ToUpper(),
                                                                                Deskripsi_KD = m_sn_kd.DeskripsiRapor
                                                                            });
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (lst_nilai_det.FindAll(
                                                                            m0 => m0.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() == m_sn_ap.Kode.ToString().ToUpper() &&
                                                                                  m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() == m_sn_kd.Kode.ToString().ToUpper() &&
                                                                                  m0.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().ToUpper() == m_sn_kp.Kode.ToString().ToUpper() &&
                                                                                  m0.Nilai.Trim() != ""
                                                                        ).Count > 0)
                                                                        {
                                                                            id_tagihan++;

                                                                            lst_hasil.Add(new NilaiMapelKurtilas
                                                                            {
                                                                                Rel_Rapor_StrukturNilai = m_sn_kurtilas.Kode.ToString().ToUpper(),
                                                                                Rel_Rapor_StrukturNilai_KURTILAS_AP = m_sn_ap.Kode.ToString().ToUpper(),
                                                                                Rel_Rapor_StrukturNilai_KURTILAS_KD = m_sn_kd.Kode.ToString().ToUpper(),
                                                                                JenisKD = Libs.GetHTMLSimpleText(m_kd.Nama),
                                                                                Rel_Rapor_StrukturNilai_KURTILAS_KP = m_sn_kp.Kode.ToString().ToUpper(),
                                                                                Deskripsi_KD = m_sn_kd.DeskripsiRapor
                                                                            });
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                    }
                                                }

                                                id_kd++;
                                            }
                                        }
                                    }
                                    //end load kurtilas kd

                                }
                            }

                        }
                    }
                }
                //end load kurtilas ap

                return lst_hasil;
            }

            class NomorMapel
            {
                public string Nomor { get; set; }
                public string Mapel { get; set; }
            }

            public List<NilaiRaporKURTILAS> get_RaporSemester(
                    string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa, bool is_ledger, int halaman = 0, string s_lokasi_ttd = "", bool show_qrcode = true
                )
            {

                System.Drawing.Image img = null;
                string s_loc = s_lokasi_ttd;
                if (File.Exists(s_loc) && s_loc.Trim() != "")
                {
                    img = System.Drawing.Image.FromFile(s_loc);
                }
                byte[] img_ttd_guru = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));

                List<NilaiRaporKURTILAS> lst_nilai_rapor_ = new List<NilaiRaporKURTILAS>();
                lst_nilai_rapor_.Clear();

                this.TahunAjaran = tahun_ajaran;
                this.Semester = semester;
                this.Rel_KelasDet = rel_kelas_det;
                this.Rel_Siswa = rel_siswa;

                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);

                List<NomorMapel> lst_nomor_mapel = new List<NomorMapel>();

                List<Rapor_NilaiSiswa_KURTILAS_Det_Lengkap> lst_nilai_det =
                    DAO_Rapor_NilaiSiswa_KURTILAS_Det.GetAllByTABySMByKelasDet_LENGKAP_Entity(tahun_ajaran, semester, rel_kelas_det);

                List<DAO_Rapor_NilaiSikapSiswa.Rapor_NilaiSikapSiswa_Lengkap> lst_nilai_sikap =
                    DAO_Rapor_NilaiSikapSiswa.GetByTABySMByMapelByKelasDet_Entity(
                            tahun_ajaran, semester, rel_kelas_det
                        );

                List<Rapor_CapaianKedisiplinan> lst_hasil_capaian_kedisiplinan = new List<Rapor_CapaianKedisiplinan>();

                List<Rapor_StrukturNilai_KURTILAS_AP> lst_sn_aspek_penilaian =
                    DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByTABySMByKelas_Entity(tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString());

                List<Rapor_StrukturNilai_KURTILAS_KD> lst_sn_kompetensi_dasar =
                    DAO_Rapor_StrukturNilai_KURTILAS_KD.GetAllByTABySMByKelas_Entity(tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString());

                List<Rapor_StrukturNilai_KURTILAS_KP> lst_sn_komponen_penilaian =
                    DAO_Rapor_StrukturNilai_KURTILAS_KP.GetAllByTABySMByKelas_Entity(tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString());

                List<Rapor_AspekPenilaian> lst_ap =
                    DAO_Rapor_AspekPenilaian.GetAll_Entity();

                List<Rapor_KompetensiDasar> lst_kd =
                    DAO_Rapor_KompetensiDasar.GetAll_Entity();

                List<Rapor_KomponenPenilaian> lst_kp =
                    DAO_Rapor_KomponenPenilaian.GetAll_Entity();

                List<FormasiGuruKelas> lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                        GetUnitSekolah().Kode.ToString(), tahun_ajaran, semester
                    ).FindAll(m => m.Rel_KelasDet.ToString().ToUpper() == rel_kelas_det.Trim().ToUpper());

                List<Rapor_CatatanSiswa> lst_catatan_siswa = DAO_Rapor_CatatanSiswa.GetAllByTABySMByKelasDet_Entity(
                        tahun_ajaran, semester, rel_kelas_det
                    );

                List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByTABySMByJenisRapor_Entity(tahun_ajaran, semester, DAO_Rapor_Desain.JenisRapor.Semester);

                List<SiswaAbsenRapor> lst_siswa_absen_rapor = new List<SiswaAbsenRapor>();
                lst_siswa_absen_rapor = DAO_SiswaAbsenRapor.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det);

                List<Rapor_Desain> lst_rapor_desain = DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran, semester);
                List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(GetUnitSekolah().Kode.ToString());

                List<DAO_Siswa.SiswaByFormasiMapel> lst_siswa_lintas_minat = DAO_FormasiGuruMapelDetSiswa.GetSiswaByTABySMBy_Entity(
                                        tahun_ajaran,
                                        semester
                                    ).FindAll(
                                        m0 => m0.Rel_Kelas.ToString().Trim().ToUpper() == m_kelas_det.Rel_Kelas.ToString().Trim().ToUpper()
                                    );

                List<DAO_Siswa.SiswaByFormasiMapel> lst_siswa_non_lintas_minat = DAO_FormasiGuruMapel.GetSiswaByTABySMByKelas_Entity(
                                        tahun_ajaran,
                                        semester,
                                        (
                                            m_kelas_det != null
                                            ? (
                                                m_kelas_det.Nama != null
                                                ? m_kelas_det.Rel_Kelas.ToString()
                                                : ""
                                              )
                                            : ""
                                        )
                                    );

                List<Rapor_Nilai> lst_nilai_rapor = DAO_Rapor_Nilai.GetAllByTABySMByKelasByKurikulum_Entity(
                        tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString(), Libs.JenisKurikulum.SMA.KURTILAS
                    );

                List<Rapor_StrukturNilai_Deskripsi> lst_deskripsi = DAO_Rapor_StrukturNilai_Deskripsi.GetAllByTABySMByKelas_Entity(
                            tahun_ajaran,
                            semester,
                            m_kelas_det.Rel_Kelas.ToString()
                        );

                List<Rapor_StrukturNilai_KURTILAS_Predikat> lst_predikat = DAO_Rapor_StrukturNilai_KURTILAS_Predikat.GetAllByTABySM_Entity(
                        tahun_ajaran, semester
                    );

                Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                    m0 => m0.TahunAjaran == tahun_ajaran &&
                          m0.Semester == semester &&
                          m0.JenisRapor == DAO_Rapor_Desain.JenisRapor.Semester
                ).FirstOrDefault();

                string s_tanggal_rapor = "";

                if (m_rapor_arsip != null)
                {
                    if (m_rapor_arsip.TahunAjaran != null)
                    {
                        s_tanggal_rapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);
                    }
                }

                string s_walikelas = "";
                this.WaliKelas = "";
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
                                        this.WaliKelas = s_walikelas;
                                    }
                                }
                            }
                        }
                    }
                }

                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        List<Rapor_StrukturNilai_KURTILAS> lst_sn_kurtilas = DAO_Rapor_StrukturNilai_KURTILAS.GetAllByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString()
                                    );

                        List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                            GetUnitSekolah().Kode.ToString(), rel_kelas_det, tahun_ajaran, semester);
                        if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m0 => (rel_siswa + ";").Trim().ToUpper().IndexOf(m0.Kode.ToString().ToUpper() + ";") >= 0).ToList();

                        List<DAO_SiswaAbsen.AbsenRekapRaporSiswa> lst_absen_ = new List<DAO_SiswaAbsen.AbsenRekapRaporSiswa>();
                        lst_absen_ = DAO_SiswaAbsen.GetRekapAbsenRaporByPeriode_Entity(
                                (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                            );

                        List<Rapor_ProgramTransisi> lst_program_transisi = DAO_Rapor_ProgramTransisi.GetByTABySMByKelasDet(
                                tahun_ajaran, semester, m_kelas_det.Kode.ToString()
                            );

                        List<Rapor_NilaiSiswa_KURTILAS> lst_nilaisiswa =
                                    DAO_Rapor_NilaiSiswa_KURTILAS.GetAllByTABySMByKelasDet_ForReport_Entity(tahun_ajaran, semester, rel_kelas_det);

                        List<KelasDet> lst_kelas_jurusan = 
                            DAO_KelasDet.GetAll_Entity();

                        int urutan = 0;
                        int nomor_mapel = 0;
                        foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa)
                        {
                            bool is_naik_kelas = true;
                            KenaikanKelas kenaikan_kelas = DAO_KenaikanKelas.GetByTAByKelasBySiswa_Entity(tahun_ajaran, rel_kelas_det, m_siswa.Kode.ToString());
                            if (kenaikan_kelas != null)
                            {
                                if (kenaikan_kelas.TahunAjaran != null)
                                {
                                    is_naik_kelas = kenaikan_kelas.IsNaik;
                                }
                            }

                            Rapor_ProgramTransisi m_program_transisi = lst_program_transisi.FindAll(
                                    m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                                ).FirstOrDefault();

                            string s_kelas = DAO_KelasDet.GetByID_Entity(rel_kelas_det).Nama;
                            string s_info_qr = "";
                            s_info_qr = "NIS = " + m_siswa.NISSekolah + ", " +
                                        "Nama = " + m_siswa.Nama.Trim().ToUpper() + ", " +
                                        "Unit = SMA, " +
                                        "Tahun Pelajaran & Semester = " + tahun_ajaran + " & " + semester + ", " +
                                        "Kelas = " + s_kelas;
                            byte[] qr_code =
                                (show_qrcode
                                    ? (byte[])(new ImageConverter()).ConvertTo(QRCodeGenerator.GetQRCode(s_info_qr, 20), typeof(byte[]))
                                    : null
                                );

                            var lst_nilai_sikap_ = lst_nilai_sikap.FindAll(
                                    m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                                );
                            var lst_siswa_non_lintas_minat_ = lst_siswa_non_lintas_minat.FindAll(
                                m0 => (
                                            (
                                                m0.Rel_KelasDet.ToString().Trim().ToUpper() == m_kelas_det.Kode.ToString().Trim().ToUpper() &&
                                                m0.JenisKelas == "WAL"
                                            ) ||
                                            (
                                                m0.Rel_KelasDetJurusan.ToString().Trim().ToUpper() == m_siswa.Rel_KelasDetJurusan.ToString().Trim().ToUpper() &&
                                                m0.JenisKelas == "JUR"
                                            ) ||
                                            (
                                                m0.Rel_KelasDetSosialisasi.ToString().Trim().ToUpper() == m_siswa.Rel_KelasDetSosialisasi.ToString().Trim().ToUpper() &&
                                                m0.JenisKelas == "SOS"
                                            )
                                        )
                                        &&
                                        m0.Kode.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper()
                            );

                            List<Rapor_NilaiSiswa_KURTILAS_Det_Lengkap> lst_nilai_det_per_siswa = lst_nilai_det.FindAll(
                                    m2 => m2.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                                );
                            urutan++;
                            
                            Rapor_CatatanSiswa m_catatan = lst_catatan_siswa.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()).FirstOrDefault();
                            string s_catatan_rapor_siswa = "";
                            if (m_catatan != null)
                            {
                                if (m_catatan.TahunAjaran != null) s_catatan_rapor_siswa = m_catatan.Catatan;
                            }

                            string nama_kelas = m_kelas_det.Nama + "-";
                            string rel_kelas_jurusan = m_siswa.Rel_KelasDetJurusan;
                            string rel_kelas_sosialisasi = m_siswa.Rel_KelasDetSosialisasi;
                            if (rel_kelas_jurusan.Trim() != "")
                            {
                                KelasDet m_kelas_jurusan = lst_kelas_jurusan
                                    .FindAll(m0 => m0.Kode.ToString().Trim().ToUpper() == rel_kelas_jurusan.Trim().ToUpper()).FirstOrDefault();
                                if (m_kelas_jurusan != null)
                                {
                                    if (m_kelas_jurusan.Nama != null)
                                    {
                                        nama_kelas = m_kelas_jurusan.Nama + "-";
                                    }
                                }
                            }

                            string[] arr_nama_kelas = nama_kelas.Split(new string[] { "-" }, StringSplitOptions.None);
                            string nama_kelas_ok = "";
                            int id_str = 0;

                            foreach (string item_nama_kelas in arr_nama_kelas)
                            {
                                if (id_str == 2)
                                {
                                    break;
                                }
                                nama_kelas_ok += (nama_kelas_ok.Trim() != "" ? "-" : "") + item_nama_kelas;
                                id_str++;
                            }

                            Rapor_Desain m_rapor_desain = lst_rapor_desain.
                                FindAll(m0 => m0.Rel_Kelas.Trim().ToUpper() == nama_kelas_ok.Trim().ToUpper() && m0.JenisRapor == DAO_Rapor_Desain.JenisRapor.Semester).FirstOrDefault();

                            bool ada_absen = false;
                            string s_sakit = "0";
                            string s_izin = "0";
                            string s_alpa = "0";
                            ada_absen = (lst_siswa_absen_rapor.FindAll(m0 => m0.Sakit > 0 || m0.Izin > 0 || m0.Alpa > 0).Count > 0 ? true : false);
                            SiswaAbsenRapor m_absen_rapor = lst_siswa_absen_rapor.FindAll(ms => ms.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()).FirstOrDefault();
                            if (m_absen_rapor != null)
                            {
                                if (m_absen_rapor.TahunAjaran != null)
                                {
                                    s_sakit = m_absen_rapor.Sakit.ToString();
                                    s_izin = m_absen_rapor.Izin.ToString();
                                    s_alpa = m_absen_rapor.Alpa.ToString();
                                }
                            }
                            if (!ada_absen)
                            {
                                if (m_rapor_arsip != null)
                                {
                                    if (m_rapor_arsip.TahunAjaran != null)
                                    {
                                        List<DAO_SiswaAbsen.AbsenRekapRaporSiswa> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRaporSiswa>();
                                        lst_absen = lst_absen_.FindAll(m0 => m0.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper());
                                        if (lst_absen.FindAll(m0 => m0.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)).Count() == 1)
                                        {
                                            s_sakit = lst_absen.FindAll(m0 => m0.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)).FirstOrDefault().Jumlah.ToString();
                                        }
                                        if (lst_absen.FindAll(m0 => m0.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)).Count() == 1)
                                        {
                                            s_izin = lst_absen.FindAll(m0 => m0.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)).FirstOrDefault().Jumlah.ToString();
                                        }
                                        if (lst_absen.FindAll(m0 => m0.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)).Count() == 1)
                                        {
                                            s_alpa = lst_absen.FindAll(m0 => m0.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)).FirstOrDefault().Jumlah.ToString();
                                        }
                                    }
                                }
                            }

                            if (m_rapor_desain != null)
                            {
                                if (m_rapor_desain.TahunAjaran != null)
                                {
                                    string poin = "";
                                    List<Rapor_Desain_Det> lst_rapor_desain_det_ = lst_rapor_desain_det.FindAll(
                                            m0 => m0.Rel_Rapor_Desain.ToString().ToUpper().Trim() == m_rapor_desain.Kode.ToString().ToUpper().Trim()
                                        );
                                    if (is_ledger)
                                    {
                                        lst_rapor_desain_det.FindAll(
                                            m0 => m0.Rel_Rapor_Desain.ToString().ToUpper().Trim() == m_rapor_desain.Kode.ToString().ToUpper().Trim() &&
                                                  m0.Rel_Mapel.Trim() != ""
                                        );
                                    }

                                    int id_lintas_minat_pengetahuan = 0;
                                    int id_lintas_minat_keterampilan = 0;
                                    int i_urutan_kelompok_mapel = 0;
                                    string s_kelompok_mapel = "";

                                    string s_peminatan = "";
                                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("ipa") >= 0)
                                    {
                                        s_peminatan = "MIPA";
                                    }
                                    else if (m_kelas_det.Nama.Trim().ToLower().IndexOf("ips") >= 0)
                                    {
                                        s_peminatan = "IPS";
                                    }
                                    else if (
                                            m_kelas_det.Nama.Trim().ToLower().IndexOf("bahasa") >= 0 ||
                                            m_kelas_det.Nama.Trim().ToLower().IndexOf("ibb") >= 0
                                        )
                                    {
                                        s_peminatan = "IBB";
                                    }

                                    int urut_mapel = 0;
                                    foreach (Rapor_Desain_Det item_rapor_desain_det in lst_rapor_desain_det_)
                                    {
                                        string s_deskripsi_pengetahuan = "";
                                        string s_deskripsi_keterampilan = "";

                                        if (
                                                item_rapor_desain_det.NamaMapelRapor.ToUpper().Trim() == "SEJARAH INDONESIA" &&
                                                m_siswa.Nama.ToUpper().IndexOf("HARYASTY") >= 0
                                            )
                                        {
                                            string a = "";
                                        }

                                        if (item_rapor_desain_det.Poin.Trim() != "" && item_rapor_desain_det.Nomor.Trim() == "")
                                        {
                                            s_kelompok_mapel = item_rapor_desain_det.NamaMapelRapor;
                                            i_urutan_kelompok_mapel = item_rapor_desain_det.Urutan;
                                        }

                                        List<NilaiRaporKURTILAS> lst_sn_lts = new List<NilaiRaporKURTILAS>();
                                        lst_sn_lts.Clear();

                                        //list siswa
                                        List<DAO_Siswa.SiswaByFormasiMapel> lst_siswa_mapel = new List<DAO_Siswa.SiswaByFormasiMapel>();
                                        lst_siswa_mapel.Clear();
                                        if (item_rapor_desain_det.JenisMapel == Libs.JENIS_MAPEL.LINTAS_MINAT)
                                        {
                                            lst_siswa_mapel = lst_siswa_lintas_minat.FindAll(
                                                    m0 => (
                                                            m0.Rel_KelasDet.ToString().Trim().ToUpper() == m_kelas_det.Kode.ToString().Trim().ToUpper() &&
                                                            m0.JenisKelas == "WAL"
                                                          ) ||
                                                          (
                                                            m0.Rel_KelasDetJurusan.ToString().Trim().ToUpper() == m_siswa.Rel_KelasDetJurusan.ToString().Trim().ToUpper() &&
                                                            m0.JenisKelas == "JUR"
                                                          ) ||
                                                          (
                                                            m0.Rel_KelasDetSosialisasi.ToString().Trim().ToUpper() == m_siswa.Rel_KelasDetSosialisasi.ToString().Trim().ToUpper() &&
                                                            m0.JenisKelas == "SOS"
                                                          ) &&
                                                          m0.Rel_Kelas.ToString().Trim().ToUpper() == m_kelas_det.Rel_Kelas.ToString().Trim().ToUpper() &&
                                                          m0.Rel_Mapel.ToString().Trim().ToUpper() == item_rapor_desain_det.Rel_Mapel.ToString().Trim().ToUpper()
                                                );
                                        }
                                        else
                                        {
                                            lst_siswa_mapel = lst_siswa_non_lintas_minat_.FindAll(
                                                    m0 => m0.Rel_Mapel.ToString().Trim().ToUpper() == item_rapor_desain_det.Rel_Mapel.ToString().Trim().ToUpper()
                                                );
                                        }

                                        DAO_Siswa.SiswaByFormasiMapel m_formasi_kelas = (
                                                lst_siswa_mapel != null
                                                ? lst_siswa_mapel.FirstOrDefault()
                                                : null
                                            );
                                        string s_kelas_formasi = "";
                                        if (m_formasi_kelas != null)
                                        {
                                            if (m_formasi_kelas.JenisKelas == "WAL")
                                            {
                                                s_kelas_formasi = m_formasi_kelas.Rel_KelasDet;
                                            }
                                            else if (m_formasi_kelas.JenisKelas == "JUR")
                                            {
                                                s_kelas_formasi = m_formasi_kelas.Rel_KelasDetJurusan;
                                            }
                                            else if (m_formasi_kelas.JenisKelas == "SOS")
                                            {
                                                s_kelas_formasi = m_formasi_kelas.Rel_KelasDetSosialisasi;
                                            }
                                        }

                                        if (item_rapor_desain_det.Poin.Trim() != "")
                                        {
                                            poin = item_rapor_desain_det.Poin.Trim();
                                        }

                                        NilaiRaporKURTILAS m = new NilaiRaporKURTILAS();

                                        List<NilaiHarian> lst_nilai_harian = new List<NilaiHarian>();
                                        lst_nilai_harian.Clear();

                                        Rapor_NilaiSiswa_KURTILAS_Det_Lengkap m_nilai_siswa = new Rapor_NilaiSiswa_KURTILAS_Det_Lengkap();
                                        
                                        List<NilaiMapelKurtilas> lst_rapor_mapel = new List<NilaiMapelKurtilas>();
                                        lst_rapor_mapel.Clear();

                                        if (item_rapor_desain_det.Rel_Mapel.Trim() != "")
                                        {
                                            Rapor_StrukturNilai_KURTILAS m_sn_kurtilas = lst_sn_kurtilas.FindAll(
                                                    m0 => m0.TahunAjaran == tahun_ajaran &&
                                                          m0.Semester == semester &&
                                                          m0.Rel_Mapel.ToString().Trim().ToUpper() == item_rapor_desain_det.Rel_Mapel.Trim().Trim().ToUpper()
                                                ).FirstOrDefault();

                                            if (m_sn_kurtilas != null)
                                            {
                                                if (m_sn_kurtilas.TahunAjaran != null)
                                                {
                                                    List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian_ = lst_sn_aspek_penilaian.FindAll(
                                                                m0 => m0.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() == m_sn_kurtilas.Kode.ToString().Trim().ToUpper()
                                                            );
                                                    lst_rapor_mapel = get_RaporMapel(
                                                            m_sn_kurtilas,
                                                            lst_nilai_det_per_siswa,
                                                            lst_sn_aspek_penilaian,
                                                            lst_sn_kompetensi_dasar,
                                                            lst_sn_komponen_penilaian,
                                                            lst_ap,
                                                            lst_kd,
                                                            lst_kp,
                                                            s_kelas_formasi
                                                        );

                                                    string s_deskripsi_predikat = "";

                                                    //nilai pengetahuan
                                                    decimal nilai_pengetahuan = 0;
                                                    List<decimal> lst_nilai_pengetahuan = new List<decimal>(); lst_nilai_pengetahuan.Clear();
                                                    List<NilaiMapelKurtilas> lst_rapor_pengetahuan = lst_rapor_mapel.FindAll(
                                                            m0 => m0.JenisKD.IndexOf(Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN) >= 0).ToList();
                                                    foreach (string item_rapor_pengetahuan in lst_rapor_pengetahuan.Select(m0 => m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString()).Distinct())
                                                    {
                                                        var m_sn_kd = lst_sn_kompetensi_dasar.FindAll(
                                                                m1 => m1.Kode.ToString().ToUpper().Trim() == item_rapor_pengetahuan.ToUpper().Trim()
                                                            ).FirstOrDefault();
                                                        decimal nilai_kd = 0;

                                                        if (m_sn_kd != null)
                                                        {
                                                            if (m_sn_kd.JenisPerhitungan != null)
                                                            {
                                                                var lst_nilai_det_ = lst_nilai_det_per_siswa.FindAll(
                                                                        m2 => m2.Rel_Mapel.ToString().ToUpper().Trim() == item_rapor_desain_det.Rel_Mapel.ToString().ToUpper().Trim() &&
                                                                              m2.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper().Trim() == m_sn_kd.Rel_Rapor_StrukturNilai_AP.ToString().ToUpper().Trim() &&
                                                                              m2.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper().Trim() == item_rapor_pengetahuan.ToUpper().Trim()
                                                                    ).FindAll(m0 => m0.Nilai.Trim() != "");

                                                                //tambahkan validasi cek ke kd struktur nilainya
                                                                bool b_ada_kp = false;
                                                                foreach (var item_nilai_det_ in lst_nilai_det_)
                                                                {
                                                                    if (lst_sn_komponen_penilaian.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item_nilai_det_.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToUpper().Trim()).Count > 0)
                                                                    {
                                                                        m_nilai_siswa = item_nilai_det_;
                                                                        b_ada_kp = true;
                                                                        break;
                                                                    }
                                                                }
                                                                //end tambahkan validasi cek ke kd struktur nilainya

                                                                if (!b_ada_kp)
                                                                {
                                                                    m_nilai_siswa = lst_nilai_det.FindAll(
                                                                           m0 => m0.Rel_Siswa.ToString().ToUpper() == m_siswa.Kode.ToString().ToUpper() &&
                                                                                 m0.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() == m_sn_kd.Rel_Rapor_StrukturNilai_AP.ToString().ToUpper() &&
                                                                                 m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() == m_sn_kd.Kode.ToString().ToUpper()
                                                                       ).FirstOrDefault();
                                                                }

                                                                if (m_sn_kd.IsKomponenRapor)
                                                                {
                                                                    nilai_kd = 0;
                                                                    if (lst_nilai_det_.Count > 0)
                                                                    {
                                                                        if (m_sn_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                        {
                                                                            nilai_kd = lst_nilai_det_.FindAll(
                                                                                            m2 => m2.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper().Trim() == item_rapor_pengetahuan.ToUpper().Trim()
                                                                                       ).Select(
                                                                                            m2 => Math.Round((Libs.GetStringToDecimal(m2.Nilai) * (m2.BobotKD / 100)), 2, MidpointRounding.AwayFromZero)
                                                                                       ).Sum();
                                                                        }
                                                                        else
                                                                        {
                                                                            nilai_kd = lst_nilai_det_.FindAll(
                                                                                            m2 => m2.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper().Trim() == item_rapor_pengetahuan.ToUpper().Trim()
                                                                                       ).Select(
                                                                                            m2 => Libs.GetStringToDecimal(m2.Nilai)
                                                                                       ).Average();
                                                                        }
                                                                    }

                                                                    s_deskripsi_predikat = "";
                                                                    foreach (var item_predikat in lst_predikat.FindAll(
                                                                            m0 => m0.Rel_Rapor_StrukturNilai.ToString().ToUpper().Trim() ==
                                                                                  m_sn_kurtilas.Kode.ToString().ToUpper().Trim()
                                                                        ))
                                                                    {
                                                                        if (nilai_kd >= item_predikat.Minimal && nilai_kd <= item_predikat.Maksimal)
                                                                        {
                                                                            s_deskripsi_predikat = item_predikat.Deskripsi;
                                                                            break;
                                                                        }
                                                                    }

                                                                    if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2020)
                                                                    {
                                                                        s_deskripsi_pengetahuan += Libs.GetHTMLSimpleText(
                                                                                                       (s_deskripsi_pengetahuan.Trim() != "" ? "; " : "") +
                                                                                                       m_sn_kd.DeskripsiRapor 
                                                                                                   );
                                                                    }
                                                                    else
                                                                    {
                                                                        s_deskripsi_pengetahuan += Libs.GetHTMLSimpleText(
                                                                                                       (s_deskripsi_pengetahuan.Trim() != "" ? "; " : "") +
                                                                                                       m_sn_kd.DeskripsiRapor + ", " +
                                                                                                       s_deskripsi_predikat +
                                                                                                       "&nbsp;" +
                                                                                                       "(" +
                                                                                                            Math.Round(
                                                                                                                nilai_kd,
                                                                                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SMA,
                                                                                                                MidpointRounding.AwayFromZero
                                                                                                            ).ToString() +
                                                                                                       ")"
                                                                                                   );
                                                                    }
                                                                    lst_nilai_pengetahuan.Add(
                                                                        Math.Round(
                                                                                nilai_kd,
                                                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SMA,
                                                                                MidpointRounding.AwayFromZero
                                                                            )
                                                                    );
                                                                }
                                                                else
                                                                {
                                                                    if (lst_nilai_det_.Count == 0)
                                                                    {
                                                                        lst_nilai_pengetahuan.Add(0);
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var item_nilai_det in lst_nilai_det_)
                                                                        {
                                                                            if (item_nilai_det.IsKP_KomponeneRapor)
                                                                            {
                                                                                s_deskripsi_predikat = "";
                                                                                foreach (var item_predikat in lst_predikat.FindAll(
                                                                                        m0 => m0.Rel_Rapor_StrukturNilai.ToString().ToUpper().Trim() ==
                                                                                              m_sn_kurtilas.Kode.ToString().ToUpper().Trim()
                                                                                    ))
                                                                                {
                                                                                    if (Libs.GetStringToDecimal(item_nilai_det.Nilai) >= item_predikat.Minimal && Libs.GetStringToDecimal(item_nilai_det.Nilai) <= item_predikat.Maksimal)
                                                                                    {
                                                                                        s_deskripsi_predikat = item_predikat.Deskripsi;
                                                                                        break;
                                                                                    }
                                                                                }

                                                                                if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2020)
                                                                                {
                                                                                    s_deskripsi_pengetahuan += Libs.GetHTMLSimpleText(
                                                                                                                   (s_deskripsi_pengetahuan.Trim() != "" ? "; " : "") +
                                                                                                                   m_sn_kd.DeskripsiRapor
                                                                                                                );
                                                                                }
                                                                                else
                                                                                {
                                                                                    s_deskripsi_pengetahuan += Libs.GetHTMLSimpleText(
                                                                                                               (s_deskripsi_pengetahuan.Trim() != "" ? "; " : "") +
                                                                                                               m_sn_kd.DeskripsiRapor + ", " +
                                                                                                               s_deskripsi_predikat +
                                                                                                               "&nbsp;" +
                                                                                                               "(" +
                                                                                                                    Math.Round(
                                                                                                                        Libs.GetStringToDecimal(item_nilai_det.Nilai),
                                                                                                                        Constantas.PEMBULATAN_DESIMAL_NILAI_SMA,
                                                                                                                        MidpointRounding.AwayFromZero
                                                                                                                    ).ToString() +
                                                                                                               ")"
                                                                                                            );
                                                                                }
                                                                                lst_nilai_pengetahuan.Add(
                                                                                        Libs.GetStringToDecimal(item_nilai_det.Nilai)
                                                                                    );
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    nilai_pengetahuan = (lst_nilai_pengetahuan.Count > 0 ? lst_nilai_pengetahuan.Average() : 0);

                                                    //nilai keterampilan
                                                    decimal nilai_keterampilan = 0;
                                                    List<decimal> lst_nilai_keterampilan = new List<decimal>(); lst_nilai_keterampilan.Clear();
                                                    List<NilaiMapelKurtilas> lst_rapor_keterampilan = lst_rapor_mapel.FindAll(
                                                            m0 => m0.JenisKD.IndexOf(Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN) >= 0).ToList();
                                                    foreach (string item_rapor_keterampilan in lst_rapor_keterampilan.Select(m0 => m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString()).Distinct())
                                                    {
                                                        var m_sn_kd = lst_sn_kompetensi_dasar.FindAll(
                                                                m1 => m1.Kode.ToString().ToUpper().Trim() == item_rapor_keterampilan.ToUpper().Trim()
                                                            ).FirstOrDefault();
                                                        decimal nilai_kd = 0;

                                                        if (m_sn_kd != null)
                                                        {
                                                            if (m_sn_kd.JenisPerhitungan != null)
                                                            {
                                                                var lst_nilai_det_ = lst_nilai_det_per_siswa.FindAll(
                                                                        m2 => m2.Rel_Mapel.ToString().ToUpper().Trim() == item_rapor_desain_det.Rel_Mapel.ToString().ToUpper().Trim() &&
                                                                              m2.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper().Trim() == m_sn_kd.Rel_Rapor_StrukturNilai_AP.ToString().ToUpper().Trim() &&
                                                                              m2.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper().Trim() == item_rapor_keterampilan.ToUpper().Trim()

                                                                    ).FindAll(m0 => m0.Nilai.Trim() != ""); ;

                                                                m_nilai_siswa = lst_nilai_det.FindAll(
                                                                       m0 => m0.Rel_Siswa.ToString().ToUpper() == m_siswa.Kode.ToString().ToUpper() &&
                                                                             m0.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() == m_sn_kd.Rel_Rapor_StrukturNilai_AP.ToString().ToUpper() &&
                                                                             m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() == m_sn_kd.Kode.ToString().ToUpper()
                                                                   ).FirstOrDefault();

                                                                if (m_sn_kd.IsKomponenRapor)
                                                                {
                                                                    nilai_kd = 0;
                                                                    if (lst_nilai_det_.Count > 0)
                                                                    {
                                                                        if (m_sn_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                        {
                                                                            nilai_kd = lst_nilai_det_.FindAll(
                                                                                        m2 => m2.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper().Trim() == item_rapor_keterampilan.ToUpper().Trim()
                                                                                    ).Select(
                                                                                        m2 => Libs.GetStringToDecimal(m2.Nilai) * (m2.BobotKD / 100)
                                                                                    ).Sum();
                                                                        }
                                                                        else
                                                                        {
                                                                            nilai_kd = lst_nilai_det_.FindAll(
                                                                                        m2 => m2.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper().Trim() == item_rapor_keterampilan.ToUpper().Trim()
                                                                                    ).Select(
                                                                                        m2 => Libs.GetStringToDecimal(m2.Nilai)
                                                                                    ).Average();
                                                                        }
                                                                    }

                                                                    s_deskripsi_predikat = "";
                                                                    foreach (var item_predikat in lst_predikat.FindAll(
                                                                            m0 => m0.Rel_Rapor_StrukturNilai.ToString().ToUpper().Trim() ==
                                                                                  m_sn_kurtilas.Kode.ToString().ToUpper().Trim()
                                                                        ))
                                                                    {
                                                                        if (nilai_kd >= item_predikat.Minimal && nilai_kd <= item_predikat.Maksimal)
                                                                        {
                                                                            s_deskripsi_predikat = item_predikat.Deskripsi;
                                                                            break;
                                                                        }
                                                                    }
                                                                    if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2020)
                                                                    {
                                                                        s_deskripsi_keterampilan += Libs.GetHTMLSimpleText(
                                                                                                        (s_deskripsi_keterampilan.Trim() != "" ? "; " : "") +
                                                                                                        m_sn_kd.DeskripsiRapor 
                                                                                                    );
                                                                    }
                                                                    else
                                                                    {
                                                                        s_deskripsi_keterampilan += Libs.GetHTMLSimpleText(
                                                                                                    (s_deskripsi_keterampilan.Trim() != "" ? "; " : "") +
                                                                                                    m_sn_kd.DeskripsiRapor + ", " +
                                                                                                    s_deskripsi_predikat +
                                                                                                    "&nbsp;" +
                                                                                                    "(" +
                                                                                                        Math.Round(
                                                                                                            nilai_kd,
                                                                                                            Constantas.PEMBULATAN_DESIMAL_NILAI_SMA,
                                                                                                            MidpointRounding.AwayFromZero
                                                                                                        ).ToString() +
                                                                                                    ")"
                                                                                                );
                                                                    }
                                                                    lst_nilai_keterampilan.Add(
                                                                        Math.Round(
                                                                                nilai_kd,
                                                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SMA,
                                                                                MidpointRounding.AwayFromZero
                                                                            )
                                                                    );
                                                                }
                                                                else
                                                                {
                                                                    if (lst_nilai_det_.Count == 0)
                                                                    {
                                                                        lst_nilai_keterampilan.Add(0);
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var item_nilai_det in lst_nilai_det_)
                                                                        {
                                                                            if (item_nilai_det.IsKP_KomponeneRapor)
                                                                            {
                                                                                s_deskripsi_predikat = "";
                                                                                foreach (var item_predikat in lst_predikat.FindAll(
                                                                                        m0 => m0.Rel_Rapor_StrukturNilai.ToString().ToUpper().Trim() ==
                                                                                              m_sn_kurtilas.Kode.ToString().ToUpper().Trim()
                                                                                    ))
                                                                                {
                                                                                    if (nilai_kd >= item_predikat.Minimal && nilai_kd <= item_predikat.Maksimal)
                                                                                    {
                                                                                        s_deskripsi_predikat = item_nilai_det.Nilai;
                                                                                        break;
                                                                                    }
                                                                                }

                                                                                if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2020)
                                                                                {
                                                                                    s_deskripsi_keterampilan += Libs.GetHTMLSimpleText(
                                                                                                                (s_deskripsi_keterampilan.Trim() != "" ? "; " : "") +
                                                                                                                m_sn_kd.DeskripsiRapor 
                                                                                                            );
                                                                                }
                                                                                else
                                                                                {
                                                                                    s_deskripsi_keterampilan += Libs.GetHTMLSimpleText(
                                                                                                                (s_deskripsi_keterampilan.Trim() != "" ? "; " : "") +
                                                                                                                m_sn_kd.DeskripsiRapor + ", " +
                                                                                                                s_deskripsi_predikat +
                                                                                                                "&nbsp;" +
                                                                                                                "(" +
                                                                                                                    Math.Round(
                                                                                                                        Libs.GetStringToDecimal(item_nilai_det.Nilai),
                                                                                                                        Constantas.PEMBULATAN_DESIMAL_NILAI_SMA,
                                                                                                                        MidpointRounding.AwayFromZero
                                                                                                                    ).ToString() +
                                                                                                                ")"
                                                                                                            );
                                                                                }
                                                                                lst_nilai_keterampilan.Add(
                                                                                        Libs.GetStringToDecimal(item_nilai_det.Nilai)
                                                                                    );
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    nilai_keterampilan = Math.Round(
                                                                            (lst_nilai_keterampilan.Count > 0 ? lst_nilai_keterampilan.Average() : 0),
                                                                            Constantas.PEMBULATAN_DESIMAL_NILAI_SMA, MidpointRounding.AwayFromZero
                                                                         );

                                                    //nilai UAS
                                                    decimal nilai_uas = 0;
                                                    List<decimal> lst_nilai_uas = new List<decimal>(); lst_nilai_uas.Clear();
                                                    List<NilaiMapelKurtilas> lst_rapor_uas = lst_rapor_mapel.FindAll(
                                                            m0 => m0.JenisKD.IndexOf(Libs.JenisKomponenNilaiKURTILAS.SMA.UAS) >= 0).ToList();
                                                    foreach (string item_rapor_uas in lst_rapor_uas.Select(m0 => m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString()).Distinct())
                                                    {
                                                        var m_sn_kd = lst_sn_kompetensi_dasar.FindAll(
                                                                m1 => m1.Kode.ToString().ToUpper().Trim() == item_rapor_uas.ToUpper().Trim()
                                                            ).FirstOrDefault();
                                                        decimal nilai_kd = 0;

                                                        if (m_sn_kd != null)
                                                        {
                                                            if (m_sn_kd.JenisPerhitungan != null)
                                                            {
                                                                var lst_nilai_det_ = lst_nilai_det_per_siswa.FindAll(
                                                                        m2 => m2.Rel_Mapel.ToString().ToUpper().Trim() == item_rapor_desain_det.Rel_Mapel.ToString().ToUpper().Trim() &&
                                                                              m2.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper().Trim() == m_sn_kd.Rel_Rapor_StrukturNilai_AP.ToString().ToUpper().Trim() &&
                                                                              m2.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper().Trim() == item_rapor_uas.ToUpper().Trim()

                                                                    );

                                                                if (m_sn_kd.IsKomponenRapor)
                                                                {
                                                                    nilai_kd = 0;
                                                                    if (m_sn_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                    {
                                                                        if (lst_nilai_det_.Count > 0)
                                                                        {
                                                                            nilai_kd = lst_nilai_det_.FindAll(
                                                                                        m2 => m2.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper().Trim() == item_rapor_uas.ToUpper().Trim()
                                                                                    ).Select(
                                                                                        m2 => Libs.GetStringToDecimal(m2.Nilai) * (m2.BobotKD / 100)
                                                                                    ).Sum();
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (lst_nilai_det_.Count > 0)
                                                                        {
                                                                            nilai_kd = lst_nilai_det_.FindAll(
                                                                                    m2 => m2.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper().Trim() == item_rapor_uas.ToUpper().Trim()
                                                                                ).Select(
                                                                                    m2 => Libs.GetStringToDecimal(m2.Nilai)
                                                                                ).Average();
                                                                        }
                                                                    }

                                                                    lst_nilai_uas.Add(
                                                                        nilai_kd
                                                                    );
                                                                }
                                                                else
                                                                {
                                                                    if (lst_nilai_det_.Count == 0)
                                                                    {
                                                                        lst_nilai_uas.Add(0);
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var item_nilai_det in lst_nilai_det_)
                                                                        {
                                                                            if (item_nilai_det.IsKP_KomponeneRapor)
                                                                            {
                                                                                lst_nilai_uas.Add(
                                                                                        Libs.GetStringToDecimal(item_nilai_det.Nilai)
                                                                                    );
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    nilai_uas = (lst_nilai_uas.Count > 0 ? lst_nilai_uas.Average() : 0);
                                                    nilai_pengetahuan = Math.Round(
                                                                            (nilai_pengetahuan * (m_sn_kurtilas.BobotRaporPengetahuan / 100)) +
                                                                            (nilai_uas * (m_sn_kurtilas.BobotRaporUAS / 100)),
                                                                        Constantas.PEMBULATAN_DESIMAL_NILAI_SMA, MidpointRounding.AwayFromZero);

                                                    var m_nilai_sikap = lst_nilai_sikap_.FindAll(
                                                            m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == item_rapor_desain_det.Rel_Mapel.ToString().ToUpper().Trim()
                                                        ).FirstOrDefault();

                                                    //nilai rapor pengetahuan
                                                    bool b_tampil = (
                                                                        (
                                                                            item_rapor_desain_det.JenisMapel == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN ||
                                                                            item_rapor_desain_det.JenisMapel == Libs.JENIS_MAPEL.PILIHAN ||
                                                                            item_rapor_desain_det.JenisMapel == Libs.JENIS_MAPEL.LINTAS_MINAT
                                                                        ) && nilai_pengetahuan == 0
                                                                        ? false
                                                                        : true
                                                                    );
                                                    if (b_tampil)
                                                    {
                                                        m = new NilaiRaporKURTILAS();
                                                        m.IDSiswa = m_siswa.Kode.ToString();
                                                        m.Nama = m_siswa.Nama.Trim().ToUpper();
                                                        m.NIS = m_siswa.NISSekolah;
                                                        m.NISN = m_siswa.NISN;
                                                        m.Kelas = m_kelas_det.Nama;
                                                        m.Semester = semester;
                                                        m.TahunAjaran = tahun_ajaran;
                                                        m.Peminatan = s_peminatan;
                                                        m.UrutanKelompokMapel = i_urutan_kelompok_mapel;
                                                        m.KelompokMapel = s_kelompok_mapel;
                                                        m.KKM = Math.Round(m_sn_kurtilas.KKM).ToString();
                                                        m.UrutanKelompokNilai = 1;
                                                        m.PoinKelompokNilai = "B.";
                                                        m.KelompokNilai = Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN;

                                                        if (
                                                            item_rapor_desain_det.JenisMapel == Libs.JENIS_MAPEL.LINTAS_MINAT &&
                                                            (
                                                                nilai_pengetahuan > 0
                                                            )
                                                        )
                                                        {
                                                            id_lintas_minat_pengetahuan++;
                                                            m.NomorMapel = id_lintas_minat_pengetahuan.ToString();
                                                        }
                                                        else
                                                        {
                                                            m.NomorMapel = item_rapor_desain_det.Nomor;
                                                        }

                                                        lst_nomor_mapel.Add(new NomorMapel {
                                                            Nomor = m.NomorMapel,
                                                            Mapel = item_rapor_desain_det.NamaMapelRapor
                                                        });

                                                        //predikat pengetahuan
                                                        string s_predikat_pengetahuan = "";
                                                        foreach (var item_predikat in lst_predikat.FindAll(
                                                                m0 => m0.Rel_Rapor_StrukturNilai.ToString().ToUpper().Trim() ==
                                                                      m_sn_kurtilas.Kode.ToString().ToUpper().Trim()
                                                            ))
                                                        {
                                                            if (nilai_pengetahuan >= item_predikat.Minimal && nilai_pengetahuan <= item_predikat.Maksimal)
                                                            {
                                                                s_predikat_pengetahuan = item_predikat.Predikat;
                                                                break;
                                                            }
                                                        }

                                                        m.PredikatSikapSosial = "";
                                                        m.PredikatSikapSpiritual = "";
                                                        if (m_nilai_sikap != null)
                                                        {
                                                            m.PredikatSikapSosial = m_nilai_sikap.PredikatSikapSosial;
                                                            m.PredikatSikapSpiritual = m_nilai_sikap.PredikatSikapSpiritual;
                                                        }
                                                        m.Rel_Mapel = item_rapor_desain_det.Rel_Mapel;
                                                        m.Mapel = item_rapor_desain_det.NamaMapelRapor;
                                                        m.Nilai = nilai_pengetahuan;
                                                        m.Predikat = s_predikat_pengetahuan;
                                                        m.Deskripsi = s_deskripsi_pengetahuan;
                                                        m.Sakit = s_sakit;
                                                        m.Izin = s_izin;
                                                        m.Alpa = s_alpa;
                                                        m.CatatanWaliKelas = s_catatan_rapor_siswa;
                                                        m.WaliKelas = s_walikelas;
                                                        m.KepalaSekolah = m_rapor_arsip.KepalaSekolah;
                                                        m.TanggalRapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);
                                                        m.Halaman = halaman.ToString();
                                                        m.TTDGuru = img_ttd_guru;
                                                        m.QRCode = qr_code;

                                                        //nilai program transisi
                                                        m.LS_JumlahJam = "";
                                                        m.LS_Deskripsi = "";
                                                        m.KW_JumlahJam = "";
                                                        m.KW_Deskripsi = "";
                                                        m.IN_JumlahJam = "";
                                                        m.IN_Deskripsi = "";
                                                        if (m_program_transisi != null)
                                                        {
                                                            if (m_program_transisi.Rel_Siswa != null)
                                                            {
                                                                m.LS_JumlahJam = m_program_transisi.LayananSosial_JumlahJam;
                                                                m.LS_Deskripsi = m_program_transisi.LayananSosial_Keterangan;
                                                                m.KW_JumlahJam = m_program_transisi.Kewirausahaan_JumlahJam;
                                                                m.KW_Deskripsi = m_program_transisi.Kewirausahaan_Keterangan;
                                                                m.IN_JumlahJam = m_program_transisi.Internship_JumlahJam;
                                                                m.IN_Deskripsi = m_program_transisi.Internship_Keterangan;
                                                            }
                                                        }
                                                        //end nilai program transisi
                                                        m.NaikKelas = (is_naik_kelas ? "1" : "0");

                                                        lst_nilai_rapor_.Add(m);
                                                    }

                                                    //nilai rapor keterampilan
                                                    b_tampil = (
                                                                (
                                                                    item_rapor_desain_det.JenisMapel == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN ||
                                                                    item_rapor_desain_det.JenisMapel == Libs.JENIS_MAPEL.PILIHAN ||
                                                                    item_rapor_desain_det.JenisMapel == Libs.JENIS_MAPEL.LINTAS_MINAT
                                                                ) && nilai_keterampilan == 0
                                                                ? false
                                                                : true
                                                            );
                                                    if (b_tampil)
                                                    {
                                                        m = new NilaiRaporKURTILAS();
                                                        m.IDSiswa = m_siswa.Kode.ToString();
                                                        m.Nama = m_siswa.Nama.Trim().ToUpper();
                                                        m.NIS = m_siswa.NISSekolah;
                                                        m.NISN = m_siswa.NISN;
                                                        m.Kelas = m_kelas_det.Nama;
                                                        m.Semester = semester;
                                                        m.TahunAjaran = tahun_ajaran;
                                                        m.Peminatan = s_peminatan;
                                                        m.UrutanKelompokMapel = i_urutan_kelompok_mapel;
                                                        m.KelompokMapel = s_kelompok_mapel;
                                                        m.KKM = Math.Round(m_sn_kurtilas.KKM).ToString();
                                                        m.UrutanKelompokNilai = 2;
                                                        m.PoinKelompokNilai = "C.";
                                                        m.KelompokNilai = Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN;

                                                        if (
                                                            item_rapor_desain_det.JenisMapel == Libs.JENIS_MAPEL.LINTAS_MINAT &&
                                                            (
                                                                nilai_keterampilan > 0
                                                            )
                                                        )
                                                        {
                                                            id_lintas_minat_keterampilan++;
                                                            m.NomorMapel = id_lintas_minat_keterampilan.ToString();
                                                        }
                                                        else
                                                        {
                                                            m.NomorMapel = item_rapor_desain_det.Nomor;
                                                        }

                                                        //predikat keterampilan
                                                        string s_predikat_keterampilan = "";
                                                        foreach (var item_predikat in lst_predikat.FindAll(
                                                                m0 => m0.Rel_Rapor_StrukturNilai.ToString().ToUpper().Trim() ==
                                                                      m_sn_kurtilas.Kode.ToString().ToUpper().Trim()
                                                            ))
                                                        {
                                                            if (nilai_keterampilan >= item_predikat.Minimal && nilai_keterampilan <= item_predikat.Maksimal)
                                                            {
                                                                s_predikat_keterampilan = item_predikat.Predikat;
                                                            }
                                                        }

                                                        m.PredikatSikapSosial = "";
                                                        m.PredikatSikapSpiritual = "";
                                                        if (m_nilai_sikap != null)
                                                        {
                                                            m.PredikatSikapSosial = m_nilai_sikap.PredikatSikapSosial;
                                                            m.PredikatSikapSpiritual = m_nilai_sikap.PredikatSikapSpiritual;
                                                        }

                                                        m.Rel_Mapel = item_rapor_desain_det.Rel_Mapel;
                                                        m.Mapel = item_rapor_desain_det.NamaMapelRapor;
                                                        m.Nilai = nilai_keterampilan;
                                                        m.Predikat = s_predikat_keterampilan;
                                                        m.Deskripsi = s_deskripsi_keterampilan;
                                                        m.Sakit = s_sakit;
                                                        m.Izin = s_izin;
                                                        m.Alpa = s_alpa;
                                                        m.CatatanWaliKelas = s_catatan_rapor_siswa;
                                                        m.WaliKelas = s_walikelas;
                                                        m.KepalaSekolah = m_rapor_arsip.KepalaSekolah;
                                                        m.TanggalRapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);
                                                        m.Halaman = halaman.ToString();
                                                        m.TTDGuru = img_ttd_guru;
                                                        m.QRCode = qr_code;

                                                        //nilai program transisi
                                                        m.LS_JumlahJam = "";
                                                        m.LS_Deskripsi = "";
                                                        m.KW_JumlahJam = "";
                                                        m.KW_Deskripsi = "";
                                                        m.IN_JumlahJam = "";
                                                        m.IN_Deskripsi = "";
                                                        if (m_program_transisi != null)
                                                        {
                                                            if (m_program_transisi.Rel_Siswa != null)
                                                            {
                                                                m.LS_JumlahJam = m_program_transisi.LayananSosial_JumlahJam;
                                                                m.LS_Deskripsi = m_program_transisi.LayananSosial_Keterangan;
                                                                m.KW_JumlahJam = m_program_transisi.Kewirausahaan_JumlahJam;
                                                                m.KW_Deskripsi = m_program_transisi.Kewirausahaan_Keterangan;
                                                                m.IN_JumlahJam = m_program_transisi.Internship_JumlahJam;
                                                                m.IN_Deskripsi = m_program_transisi.Internship_Keterangan;
                                                            }
                                                        }
                                                        //end nilai program transisi
                                                        m.NaikKelas = (is_naik_kelas ? "1" : "0");

                                                        lst_nilai_rapor_.Add(m);
                                                    }
                                                }
                                            }
                                        }

                                        Mapel m_mapel = lst_mapel.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item_rapor_desain_det.Rel_Mapel.ToUpper().Trim()).FirstOrDefault();
                                        bool b_tampil_kedisiplinan = true;

                                        //capaian kedisiplinan
                                        Rapor_NilaiSiswa_KURTILAS m_nilaisiswa = lst_nilaisiswa.FindAll(
                                                    m0 => m0.Kode.ToString().ToUpper().Trim() == m_nilai_siswa.Rel_Rapor_NilaiSiswa_KURTILAS.ToString().ToUpper().Trim()
                                                ).FirstOrDefault();

                                        List<Rapor_NilaiSiswa_KURTILAS> lst_nilaisiswa_ = lst_nilaisiswa.FindAll(
                                                    m0 => m0.Kode.ToString().ToUpper().Trim() == m_nilai_siswa.Rel_Rapor_NilaiSiswa_KURTILAS.ToString().ToUpper().Trim()
                                                );

                                        bool ada_nilai_kedisiplinan = false;
                                        bool ada_nilai = false;
                                        Rapor_CapaianKedisiplinan m_rapor_capaian_kedisiplinan = new Rapor_CapaianKedisiplinan();

                                        if (item_rapor_desain_det.Poin.Trim() != "")
                                        {
                                            poin = item_rapor_desain_det.Poin.Trim();
                                        }
                                        if (m_nilaisiswa != null)
                                        {
                                            if (m_nilaisiswa.Rel_Siswa != null)
                                            {
                                                if (m_nilai_siswa.Nilai.Trim() != "") ada_nilai = true;
                                                ada_nilai_kedisiplinan = true;
                                                urut_mapel++;
                                                if (item_rapor_desain_det.Poin.Trim() == "") nomor_mapel++;

                                                string s_nomor_mapel_cp = "";
                                                var m_nomor_mapel = lst_nomor_mapel.FindAll(m0 => m0.Mapel == item_rapor_desain_det.NamaMapelRapor).FirstOrDefault();
                                                if (m_nomor_mapel != null)
                                                {
                                                    if (m_nomor_mapel.Mapel != null) s_nomor_mapel_cp = m_nomor_mapel.Nomor;
                                                }

                                                m_rapor_capaian_kedisiplinan.Rel_Siswa = m_nilaisiswa.Rel_Siswa;
                                                m_rapor_capaian_kedisiplinan.KodeKelompokMapel = poin;
                                                m_rapor_capaian_kedisiplinan.KelompokMapel = item_rapor_desain_det.NamaMapelRapor;
                                                m_rapor_capaian_kedisiplinan.NomorMapel = s_nomor_mapel_cp;
                                                m_rapor_capaian_kedisiplinan.Rel_Mapel = item_rapor_desain_det.Rel_Mapel;
                                                m_rapor_capaian_kedisiplinan.NamaMapel = (item_rapor_desain_det.Poin.Trim() != "" ? item_rapor_desain_det.Poin + " " : "") + item_rapor_desain_det.NamaMapelRapor;
                                                //if (tahun_ajaran == "2020/2021" && semester == "2" && m_kelas_det.Nama.Substring(0, 4) == "XII-")
                                                if (tahun_ajaran == "2020/2021" && semester == "2")
                                                {
                                                    m_rapor_capaian_kedisiplinan.Kehadiran = m_nilaisiswa.LTS_CK_KEHADIRAN;
                                                    m_rapor_capaian_kedisiplinan.KetepatanWaktu = m_nilaisiswa.LTS_CK_KETEPATAN_WKT;
                                                    m_rapor_capaian_kedisiplinan.PenggunaanSeragam = m_nilaisiswa.LTS_CK_PENGGUNAAN_SRGM;
                                                    m_rapor_capaian_kedisiplinan.PenggunaanKamera = m_nilaisiswa.LTS_CK_PENGGUNAAN_KMR;
                                                }
                                                else
                                                {
                                                    m_rapor_capaian_kedisiplinan.Kehadiran = m_nilaisiswa.SM_CK_KEHADIRAN;
                                                    m_rapor_capaian_kedisiplinan.KetepatanWaktu = m_nilaisiswa.SM_CK_KETEPATAN_WKT;
                                                    m_rapor_capaian_kedisiplinan.PenggunaanSeragam = m_nilaisiswa.SM_CK_PENGGUNAAN_SRGM;
                                                    m_rapor_capaian_kedisiplinan.PenggunaanKamera = m_nilaisiswa.SM_CK_PENGGUNAAN_KMR;
                                                }
                                                m_rapor_capaian_kedisiplinan.UrutanMapel = urut_mapel;
                                            }
                                        }
                                        if (!ada_nilai_kedisiplinan)
                                        {
                                            ada_nilai_kedisiplinan = true;

                                            urut_mapel++;
                                            if (item_rapor_desain_det.Poin.Trim() == "") nomor_mapel++;

                                            m_rapor_capaian_kedisiplinan.Rel_Siswa = m_siswa.Kode.ToString();
                                            m_rapor_capaian_kedisiplinan.KodeKelompokMapel = poin;
                                            m_rapor_capaian_kedisiplinan.KelompokMapel = item_rapor_desain_det.NamaMapelRapor;
                                            m_rapor_capaian_kedisiplinan.NomorMapel = "";
                                            m_rapor_capaian_kedisiplinan.Rel_Mapel = item_rapor_desain_det.Rel_Mapel;
                                            m_rapor_capaian_kedisiplinan.NamaMapel = (item_rapor_desain_det.Poin.Trim() != "" ? item_rapor_desain_det.Poin + " " : "") +
                                                                                         (
                                                                                            item_rapor_desain_det.Rel_Mapel.Trim() == ""
                                                                                            ? item_rapor_desain_det.NamaMapelRapor.Replace(":", "") + ":"
                                                                                            : item_rapor_desain_det.NamaMapelRapor
                                                                                         );
                                            m_rapor_capaian_kedisiplinan.Kehadiran = "";
                                            m_rapor_capaian_kedisiplinan.KetepatanWaktu = "";
                                            m_rapor_capaian_kedisiplinan.PenggunaanSeragam = "";
                                            m_rapor_capaian_kedisiplinan.PenggunaanKamera = "";
                                            m_rapor_capaian_kedisiplinan.UrutanMapel = urut_mapel;
                                        }
                                        //end capaian kedisiplinan

                                        if (m_mapel != null)
                                        {
                                            if (m_mapel.Nama != null)
                                            {
                                                if (m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN && !ada_nilai)
                                                {
                                                    b_tampil_kedisiplinan = false;
                                                }
                                                else if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT && ada_nilai)
                                                {
                                                    b_tampil_kedisiplinan = true;
                                                }
                                                else if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT && !ada_nilai)
                                                {
                                                    b_tampil_kedisiplinan = false;
                                                }
                                            }
                                        }
                                        if (b_tampil_kedisiplinan)
                                        {
                                            lst_hasil_capaian_kedisiplinan.Add(m_rapor_capaian_kedisiplinan);
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                GetRaporCapaianKedisiplinan = lst_hasil_capaian_kedisiplinan;
                return lst_nilai_rapor_;
            }

            public RaporSemester(
                    string tahun_ajaran, string semester, string rel_kelas_det
                )
            {
                this.TahunAjaran = tahun_ajaran;
                this.Semester = semester;
                this.Rel_KelasDet = rel_kelas_det;
            }

            public RaporSemester(
                    string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa, int halaman = 0, string s_lokasi_ttd = "", bool show_qrcode = true
                )
            {
                this.TahunAjaran = tahun_ajaran;
                this.Semester = semester;
                this.Rel_KelasDet = rel_kelas_det;
                this.Rel_Siswa = rel_siswa;
                this.LokasiTTD = s_lokasi_ttd;

                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);

                List<FormasiGuruKelas> lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                        GetUnitSekolah().Kode.ToString(), tahun_ajaran, semester
                    ).FindAll(m => m.Rel_KelasDet.ToString().ToUpper() == rel_kelas_det.Trim().ToUpper());

                Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                    m0 => m0.TahunAjaran == tahun_ajaran &&
                          m0.Semester == semester &&
                          m0.JenisRapor == DAO_Rapor_Desain.JenisRapor.Semester
                ).FirstOrDefault();

                List<Rapor_NilaiSikap_For_Rapor> lst_sikap_det = DAO_Rapor_NilaiSikap.GetNilaiByTABySMByKelas_Entity(
                       tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString()
                   );

                List<DAO_FormasiEkskulDet.NilaiEkskul> lst_ekskul_det = DAO_FormasiEkskulDet.GetNilaiEkskulByTABySMByKelas_Entity(
                       tahun_ajaran, semester, DAO_KelasDet.GetByID_Entity(rel_kelas_det).Rel_Kelas.ToString()
                   );

                List<Volunteer_Det> lst_volunteer = DAO_Volunteer_Det.GetByTABySMByKelasDet_Entity(
                       tahun_ajaran, semester, rel_kelas_det
                   );

                List<NilaiRaporEkskul> lst_nilai_rapor_ekskul = new List<NilaiRaporEkskul>();
                List<NilaiRaporVolunteer> lst_rapor_volunteer = new List<NilaiRaporVolunteer>();
                List<NilaiRaporSikap> lst_rapor_sikap = new List<NilaiRaporSikap>();

                string s_tanggal_rapor = "";

                if (m_rapor_arsip != null)
                {
                    if (m_rapor_arsip.TahunAjaran != null)
                    {
                        s_tanggal_rapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);
                    }
                }

                string s_walikelas = "";
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
                                    }
                                }
                            }
                        }
                    }
                }

                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        List<Rapor_StrukturNilai_KURTILAS> lst_sn_kurtilas = DAO_Rapor_StrukturNilai_KURTILAS.GetAllByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString()
                                    );

                        List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                            GetUnitSekolah().Kode.ToString(), rel_kelas_det, tahun_ajaran, semester);
                        if (rel_siswa.Trim() != "")
                        {
                            lst_siswa = lst_siswa.FindAll(m0 => (rel_siswa + ";").Trim().ToUpper().IndexOf(m0.Kode.ToString().ToUpper() + ";") >= 0).ToList();
                            foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa)
                            {
                                //sikap
                                List<Rapor_NilaiSikap_For_Rapor> lst_sikap_by_siswa =
                                    lst_sikap_det.FindAll(
                                            m0 => m0.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().ToUpper().Trim()
                                        );
                                foreach (var item in lst_sikap_by_siswa)
                                {
                                    lst_rapor_sikap.Add(new NilaiRaporSikap
                                    {
                                        IDSiswa = m_siswa.Kode.ToString(),
                                        NilaiSikapSpiritual = item.SikapSpiritual,
                                        NilaiSikapSosial = item.SikapSosial,
                                        DeskripsiSikapSpiritual = Libs.GetHTMLSimpleText(item.DeskripsiSikapSpiritual),
                                        DeskripsiSikapSosial = Libs.GetHTMLSimpleText(item.DeskripsiSikapSosial)
                                    });
                                }

                                //ekskul
                                List<DAO_FormasiEkskulDet.NilaiEkskul> lst_ekskul_det_by_siswa =
                                    lst_ekskul_det.FindAll(
                                            m0 => m0.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().ToUpper().Trim()
                                        );
                                foreach (var item in lst_ekskul_det_by_siswa)
                                {
                                    lst_nilai_rapor_ekskul.Add(new NilaiRaporEkskul
                                    {
                                        IDSiswa = m_siswa.Kode.ToString(),
                                        JenisKegiatan = item.Mapel,
                                        Deskripsi = Libs.GetHTMLSimpleText(item.Deskripsi),
                                        Predikat = item.Nilai,
                                        Sakit = item.Sakit,
                                        Izin = item.Izin,
                                        Alpa = item.Alpa,
                                        Urutan = item.Urutan
                                    });
                                }

                                //volunteer
                                List<Volunteer_Det> lst_rapor_volunteer_by_siswa =
                                    lst_volunteer.FindAll(
                                            m0 => m0.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().ToUpper().Trim()
                                        );
                                foreach (var item in lst_rapor_volunteer_by_siswa)
                                {
                                    lst_rapor_volunteer.Add(new NilaiRaporVolunteer
                                    {
                                        IDSiswa = m_siswa.Kode.ToString(),
                                        JumlahJam = item.JumlahJam,
                                        Kegiatan = Libs.GetHTMLSimpleText(item.Kegiatan),
                                        Keterangan = Libs.GetHTMLSimpleText(item.Keterangan),
                                        Tanggal = item.Tanggal,
                                        Urutan = 0
                                    });
                                }
                            }

                            GetRaporSemester = get_RaporSemester(
                                    tahun_ajaran, semester, rel_kelas_det, rel_siswa, false, halaman, s_lokasi_ttd, show_qrcode
                                );
                            GetRaporEkskul = lst_nilai_rapor_ekskul;
                            GetRaporVolunteer = lst_rapor_volunteer;
                            GetRaporSikap = lst_rapor_sikap;
                        }
                    }
                }
            }
        }

        public class LTS
        {
            public string TahunAjaran;
            public string Semester;
            public string Rel_KelasDet;
            public string Rel_Siswa;
            public string GetHTML;
            public string LokasiTTD;

            public List<RaporLTS> GetRaporLTS;
            public List<RaporLTSDeskripsi> GetRaporLTSDeskripsi;
            public List<RaporLTSCapaianKedisiplinan> GetRaporLTSCapaianKedisiplinan;

            public class NilaiLTSMapel
            {
                public string Rel_Rapor_StrukturNilai { get; set; }
                public string Rel_Rapor_StrukturNilai_KURTILAS_AP { get; set; }
                public string Rel_Rapor_StrukturNilai_KURTILAS_KD { get; set; }
                public string Rel_Rapor_StrukturNilai_KURTILAS_KP { get; set; }
                public string DeskripsiLTS { get; set; }
                public string NamaKP { get; set; }
                public int UrutanTagihan { get; set; }
            }

            public LTS(string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa, string s_lokasi_ttd, bool show_qrcode = true)
            {
                if (tahun_ajaran.IndexOf("/") < 0) tahun_ajaran = RandomLibs.GetParseTahunAjaran(tahun_ajaran);
                System.Drawing.Image img = null;
                string s_loc = s_lokasi_ttd;
                if (File.Exists(s_loc) && s_loc.Trim() != "")
                {
                    img = System.Drawing.Image.FromFile(s_loc);
                }
                byte[] img_ttd_guru = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));

                this.TahunAjaran = tahun_ajaran;
                this.Semester = semester;
                this.Rel_KelasDet = rel_kelas_det;
                this.Rel_Siswa = rel_siswa;

                List<KelasDet> lst_kelasdet = DAO_KelasDet.GetBySekolah_Entity(GetUnitSekolah().Kode.ToString());
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);

                List<RaporLTS> lst_hasil_lts = new List<RaporLTS>();
                List<RaporLTSDeskripsi> lst_hasil_deksripsi = new List<RaporLTSDeskripsi>();
                List<RaporLTSCapaianKedisiplinan> lst_hasil_capaian_kedisiplinan = new List<RaporLTSCapaianKedisiplinan>();

                List<Rapor_NilaiSiswa_KURTILAS_Det_Lengkap> lst_nilai_det =
                    DAO_Rapor_NilaiSiswa_KURTILAS_Det.GetAllByTABySMByKelasDet_LENGKAP_Entity(tahun_ajaran, semester, rel_kelas_det);

                List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian =
                    DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByTABySM_Entity(tahun_ajaran, semester);

                List<Rapor_StrukturNilai_KURTILAS_KD> lst_kompetensi_dasar =
                    DAO_Rapor_StrukturNilai_KURTILAS_KD.GetAllByTABySM_Entity(tahun_ajaran, semester);

                List<Rapor_StrukturNilai_KURTILAS_KP> lst_komponen_penilaian =
                    DAO_Rapor_StrukturNilai_KURTILAS_KP.GetAllByTABySM_Entity(tahun_ajaran, semester);

                List<Rapor_AspekPenilaian> lst_ap =
                    DAO_Rapor_AspekPenilaian.GetAll_Entity();

                List<Rapor_KompetensiDasar> lst_kd =
                    DAO_Rapor_KompetensiDasar.GetAll_Entity();

                List<Rapor_KomponenPenilaian> lst_kp =
                    DAO_Rapor_KomponenPenilaian.GetAll_Entity();

                List<FormasiGuruKelas> lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                        GetUnitSekolah().Kode.ToString(), tahun_ajaran, semester
                    ).FindAll(m => m.Rel_KelasDet.ToString().ToUpper() == rel_kelas_det.Trim().ToUpper());

                List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByTABySMByJenisRapor_Entity(tahun_ajaran, semester, DAO_Rapor_Desain.JenisRapor.LTS);

                List<SiswaAbsenLTS> lst_siswa_absen_lts = new List<SiswaAbsenLTS>();
                lst_siswa_absen_lts = DAO_SiswaAbsenLTS.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det);

                List<Rapor_Desain> lst_rapor_desain = DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran, semester);
                List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(GetUnitSekolah().Kode.ToString());

                List<DAO_Siswa.SiswaByFormasiMapel> lst_siswa_lintas_minat = DAO_FormasiGuruMapelDetSiswa.GetSiswaByTABySMBy_Entity(
                                        tahun_ajaran,
                                        semester
                                    );

                List<DAO_Siswa.SiswaByFormasiMapel> lst_siswa_non_lintas_minat = DAO_FormasiGuruMapel.GetSiswaByTABySMByKelas_Entity(
                                        tahun_ajaran,
                                        semester,
                                        (
                                            m_kelas_det != null
                                            ? (
                                                m_kelas_det.Nama != null
                                                ? m_kelas_det.Rel_Kelas.ToString()
                                                : ""
                                              )
                                            : ""
                                        )
                                    );

                List<Rapor_Nilai> lst_nilai_rapor = DAO_Rapor_Nilai.GetAllByTABySMByKelasByKurikulum_Entity(
                        tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString(), Libs.JenisKurikulum.SMA.KURTILAS
                    );

                List<Rapor_StrukturNilai_Deskripsi> lst_deskripsi = DAO_Rapor_StrukturNilai_Deskripsi.GetAllByTABySMByKelas_Entity(
                            tahun_ajaran,
                            semester,
                            m_kelas_det.Rel_Kelas.ToString()
                        );

                Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                    m0 => m0.TahunAjaran == tahun_ajaran &&
                          m0.Semester == semester &&
                          m0.JenisRapor == DAO_Rapor_Desain.JenisRapor.LTS
                ).FirstOrDefault();

                string s_walikelas = "";
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
                                    }
                                }
                            }
                        }
                    }
                }

                List<DAO_SiswaAbsenMapel.AbsenMapel> lst_absen_mapel_all = DAO_SiswaAbsenMapel.GetAllByTABySMByPeriodeBySekolah_Entity(
                                                tahun_ajaran, semester,
                                                (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                                (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue),
                                                GetUnitSekolah().Kode.ToString()
                                            );

                string s_html = "";
                string s_html_deskripsi = "";
                string s_tanggal_rapor = "";

                List<DAO_SiswaAbsen.AbsenRekapRaporSiswa> lst_absen_ = new List<DAO_SiswaAbsen.AbsenRekapRaporSiswa>();
                if (m_rapor_arsip != null)
                {
                    if (m_rapor_arsip.TahunAjaran != null)
                    {
                        s_tanggal_rapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);

                        lst_absen_ = DAO_SiswaAbsen.GetRekapAbsenRaporByPeriode_Entity(
                                (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                            );
                    }
                }

                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        List<Rapor_StrukturNilai_KURTILAS> lst_sn_kurtilas = DAO_Rapor_StrukturNilai_KURTILAS.GetAllByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString()
                                    );

                        List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                            GetUnitSekolah().Kode.ToString(), rel_kelas_det, tahun_ajaran, semester);
                        //if (rel_siswa.Trim() != "")
                        //{

                        //}

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
                        if (rel_siswa.Trim() != "")
                        {
                            lst_siswa = lst_siswa.FindAll(m0 => (rel_siswa + ";").Trim().ToUpper().IndexOf(m0.Kode.ToString().ToUpper() + ";") >= 0).ToList();
                        }

                        List<Rapor_NilaiSiswa_KURTILAS> lst_nilai_siswa = DAO_Rapor_NilaiSiswa_KURTILAS.GetAllByTABySMByKelasDet_Entity(
                                tahun_ajaran, semester, rel_kelas_det
                            );

                        List<Rapor_NilaiSiswa_KURTILAS> lst_nilaisiswa =
                                    DAO_Rapor_NilaiSiswa_KURTILAS.GetAllByTABySMByKelasDet_ForReport_Entity(tahun_ajaran, semester, rel_kelas_det);

                        int nomor_mapel = 0;
                        int urut_mapel = 0;
                        foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa)
                        {
                            string s_info_qr = "";
                            s_info_qr = "NIS = " + m_siswa.NISSekolah + ", " +
                                        "Nama = " + m_siswa.Nama.Trim().ToUpper() + ", " +
                                        "Unit = SMA, " +
                                        "Tahun Pelajaran & Semester = " + tahun_ajaran + " & " + semester + ", " +
                                        "Kelas = " + m_kelas_det.Nama;
                            byte[] qr_code =
                                (show_qrcode
                                    ? (byte[])(new ImageConverter()).ConvertTo(QRCodeGenerator.GetQRCode(s_info_qr, 20), typeof(byte[]))
                                    : null
                                );

                            List<DAO_FormasiEkskulDet.AbsenEkskulLTS> lst_ekskul_det_ = DAO_FormasiEkskulDet.GetLTSEkskulAbsenByTABySMBySiswa_Entity(
                                tahun_ajaran, semester, m_siswa.Kode.ToString()
                            );

                            //ekskul LTS
                            int id_ekskul = 1;
                            List<DAO_FormasiEkskulDet.AbsenEkskulLTS> lst_ekskul_det = lst_ekskul_det_.FindAll(
                                   m0 => m0.TahunAjaran == tahun_ajaran &&
                                         m0.Semester == semester &&
                                         m0.Rel_Siswa == m_siswa.Kode.ToString()
                               );
                            //end ekskul LTS

                            s_html += "<h4 style=\"margin: 0 auto; display: table; margin-top: 20px;\">DAFTAR NILAI (RAPOR) TENGAH SEMESTER - K13</h4>" +
                                      "<h4 style=\"margin: 0 auto; display: table;\">SMA ISLAM AL-IZHAR PONDOK LABU</h4>" +
                                      "<hr />";

                            s_html += "<table style=\"border-style: none; width: 100%; font-size: small;\">" +
                                        "<tr>" +
                                            "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">NIS</td>" +
                                            "<td style=\"text-align: center; padding: 5px; padding-top: 3px; padding-bottom: 3px;\">:</td>" +
                                            "<td colspan=\"4\" style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">" + m_siswa.NISSekolah + "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">Nama Siswa</td>" +
                                            "<td style=\"text-align: center; padding: 5px; padding-top: 3px; padding-bottom: 3px;\">:</td>" +
                                            "<td style=\"font-weight: bold; padding: 5px; padding-top: 3px; padding-bottom: 3px; width: 50%;\">" + m_siswa.Nama.Trim().ToUpper() + "</td>" +
                                            "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">Semester</td>" +
                                            "<td style=\"text-align: center; padding: 5px; padding-top: 3px; padding-bottom: 3px;\">:</td>" +
                                            "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">" +
                                                (semester == "1" ? "1 (satu)" : "2 (dua)") +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">Kelas</td>" +
                                            "<td style=\"text-align: center; padding: 5px; padding-top: 3px; padding-bottom: 3px;\">:</td>" +
                                            "<td style=\"font-weight: bold; padding: 5px; padding-top: 3px; padding-bottom: 3px;\">" + m_kelas_det.Nama + "</td>" +
                                            "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">Tahun Pelajaran</td>" +
                                            "<td style=\"text-align: center; padding: 5px; padding-top: 3px; padding-bottom: 3px;\">:</td>" +
                                            "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">" +
                                                tahun_ajaran +
                                            "</td>" +
                                        "</tr>" +
                                      "</table>";

                            s_html += "<table style=\"border-style: solid; border-width: 1px; font-size: small; width: 100%; margin-top: 15px;\">" +
                                        "<tr>" +
                                            "<td rowspan=\"2\" style=\"width: 40px; vertical-align: middle; padding: 3px; border-style: solid; border-width: 0.5px;\">No.</td>" +
                                            "<td rowspan=\"2\" style=\"vertical-align: middle; padding: 3px; border-style: solid; border-width: 0.5px;\">Mata Pelajaran</td>" +
                                            "<td rowspan=\"2\" style=\"vertical-align: middle; padding: 3px; border-style: solid; border-width: 0.5px;\">KKM</td>" +
                                            "<td colspan=\"12\" style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">PR/Latihan/Tugas/Kuis/Ulangan Harian/Lain-lain *)</td>" +
                                            "<td colspan=\"4\" style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">Perilaku Belajar</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T1</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T2</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T3</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T4</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T5</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T6</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T7</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T8</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T9</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T10</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T11</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T12</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">HD</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">LK</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">RJ</td>" +
                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">RPKB</td>" +
                                        "</tr>";

                            string nama_kelas = m_kelas_det.Nama + "-";
                            string rel_kelas_jurusan = m_siswa.Rel_KelasDetJurusan;
                            string rel_kelas_sosialisasi = m_siswa.Rel_KelasDetSosialisasi;
                            if (rel_kelas_jurusan.Trim() != "")
                            {
                                //KelasDet m_kelas_jurusan = DAO_KelasDet.GetByID_Entity(rel_kelas_jurusan);
                                KelasDet m_kelas_jurusan = lst_kelasdet.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == rel_kelas_jurusan.ToUpper().Trim()).FirstOrDefault();
                                if (m_kelas_jurusan != null)
                                {
                                    if (m_kelas_jurusan.Nama != null)
                                    {
                                        nama_kelas = m_kelas_jurusan.Nama + "-";
                                    }
                                }
                            }

                            string[] arr_nama_kelas = nama_kelas.Split(new string[] { "-" }, StringSplitOptions.None);
                            string nama_kelas_ok = "";
                            int id_str = 0;

                            foreach (string item_nama_kelas in arr_nama_kelas)
                            {
                                if (id_str == 2)
                                {
                                    break;
                                }
                                nama_kelas_ok += (nama_kelas_ok.Trim() != "" ? "-" : "") + item_nama_kelas;
                                id_str++;
                            }

                            Rapor_Desain m_rapor_desain = lst_rapor_desain.
                                FindAll(m0 => m0.Rel_Kelas.Trim().ToUpper() == nama_kelas_ok.Trim().ToUpper() && m0.JenisRapor == DAO_Rapor_Desain.JenisRapor.LTS).FirstOrDefault();

                            bool ada_absen = false;
                            string s_sakit = "0";
                            string s_izin = "0";
                            string s_alpa = "0";
                            ada_absen = (lst_siswa_absen_lts.FindAll(m0 => m0.Sakit > 0 || m0.Izin > 0 || m0.Alpa > 0).Count > 0 ? true : false);
                            SiswaAbsenLTS m_absen_lts = lst_siswa_absen_lts.FindAll(ms => ms.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()).FirstOrDefault();
                            if (m_absen_lts != null)
                            {
                                if (m_absen_lts.TahunAjaran != null)
                                {
                                    s_sakit = m_absen_lts.Sakit.ToString();
                                    s_izin = m_absen_lts.Izin.ToString();
                                    s_alpa = m_absen_lts.Alpa.ToString();
                                }
                            }

                            if (!ada_absen)
                            {
                                if (m_rapor_arsip != null)
                                {
                                    if (m_rapor_arsip.TahunAjaran != null)
                                    {
                                        List<DAO_SiswaAbsen.AbsenRekapRaporSiswa> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRaporSiswa>();
                                        lst_absen = lst_absen_.FindAll(m0 => m0.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper());
                                        foreach (var absen in lst_absen)
                                        {
                                            if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                            if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                            if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                        }
                                        if (tahun_ajaran == "2020/2021")
                                        {
                                            if (lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper() == rel_siswa.ToUpper().Trim()).Count > 0)
                                            {
                                                SiswaAbsenRekapDet m_rekap_absen_siswa = lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper() == rel_siswa.ToUpper().Trim()).FirstOrDefault();
                                                if (m_rekap_absen_siswa != null)
                                                {
                                                    if (m_rekap_absen_siswa.Rel_Siswa != null)
                                                    {
                                                        s_sakit = m_rekap_absen_siswa.Sakit;
                                                        s_izin = m_rekap_absen_siswa.Izin;
                                                        s_alpa = m_rekap_absen_siswa.Alpa;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (Libs.GetStringToDecimal(tahun_ajaran.Replace("/", "")) >= 20212022)
                            {
                                //absen lts
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
                                }
                                //end absen lts
                            }

                            if (m_rapor_desain != null)
                            {
                                if (m_rapor_desain.TahunAjaran != null)
                                {
                                    string poin = "";
                                    List<Rapor_Desain_Det> lst_rapor_desain_det_ = lst_rapor_desain_det.FindAll(
                                            m0 => m0.Rel_Rapor_Desain.ToString().ToUpper().Trim() == m_rapor_desain.Kode.ToString().ToUpper().Trim()
                                        );

                                    int id_lintas_minat = 0;
                                    foreach (Rapor_Desain_Det item_rapor_desain_det in lst_rapor_desain_det_)
                                    {
                                        List<NilaiLTSMapel> lst_sn_lts = new List<NilaiLTSMapel>();
                                        lst_sn_lts.Clear();

                                        //list siswa
                                        List<DAO_Siswa.SiswaByFormasiMapel> lst_siswa_mapel = new List<DAO_Siswa.SiswaByFormasiMapel>();
                                        lst_siswa_mapel.Clear();
                                        if (item_rapor_desain_det.JenisMapel == Libs.JENIS_MAPEL.LINTAS_MINAT)
                                        {
                                            lst_siswa_mapel = lst_siswa_lintas_minat.FindAll(
                                                    m0 => (
                                                            m0.Rel_KelasDet.ToString().Trim().ToUpper() == m_kelas_det.Kode.ToString().Trim().ToUpper() &&
                                                            m0.JenisKelas == "WAL"
                                                          ) ||
                                                          (
                                                            m0.Rel_KelasDetJurusan.ToString().Trim().ToUpper() == m_siswa.Rel_KelasDetJurusan.ToString().Trim().ToUpper() &&
                                                            m0.JenisKelas == "JUR"
                                                          ) ||
                                                          (
                                                            m0.Rel_KelasDetSosialisasi.ToString().Trim().ToUpper() == m_siswa.Rel_KelasDetSosialisasi.ToString().Trim().ToUpper() &&
                                                            m0.JenisKelas == "SOS"
                                                          ) &&
                                                          m0.Rel_Kelas.ToString().Trim().ToUpper() == m_kelas_det.Rel_Kelas.ToString().Trim().ToUpper() &&
                                                          m0.Rel_Mapel.ToString().Trim().ToUpper() == item_rapor_desain_det.Rel_Mapel.ToString().Trim().ToUpper()
                                                );
                                        }
                                        else
                                        {
                                            lst_siswa_mapel = lst_siswa_non_lintas_minat.FindAll(
                                                    m0 => (
                                                                (
                                                                    m0.Rel_KelasDet.ToString().Trim().ToUpper() == m_kelas_det.Kode.ToString().Trim().ToUpper() &&
                                                                    m0.JenisKelas == "WAL"
                                                                ) ||
                                                                (
                                                                    m0.Rel_KelasDetJurusan.ToString().Trim().ToUpper() == m_siswa.Rel_KelasDetJurusan.ToString().Trim().ToUpper() &&
                                                                    m0.JenisKelas == "JUR"
                                                                ) ||
                                                                (
                                                                    m0.Rel_KelasDetSosialisasi.ToString().Trim().ToUpper() == m_siswa.Rel_KelasDetSosialisasi.ToString().Trim().ToUpper() &&
                                                                    m0.JenisKelas == "SOS"
                                                                )
                                                          )
                                                          &&
                                                          m0.Kode.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                          m0.Rel_Mapel.ToString().Trim().ToUpper() == item_rapor_desain_det.Rel_Mapel.ToString().Trim().ToUpper()
                                                );
                                        }

                                        DAO_Siswa.SiswaByFormasiMapel m_formasi_kelas = (
                                                lst_siswa_mapel != null
                                                ? lst_siswa_mapel.FirstOrDefault()
                                                : null
                                            );
                                        string s_kelas_formasi = "";
                                        if (m_formasi_kelas != null)
                                        {
                                            if (m_formasi_kelas.JenisKelas == "WAL")
                                            {
                                                s_kelas_formasi = m_formasi_kelas.Rel_KelasDet;
                                            }
                                            else if (m_formasi_kelas.JenisKelas == "JUR")
                                            {
                                                s_kelas_formasi = m_formasi_kelas.Rel_KelasDetJurusan;
                                            }
                                            else if (m_formasi_kelas.JenisKelas == "SOS")
                                            {
                                                s_kelas_formasi = m_formasi_kelas.Rel_KelasDetSosialisasi;
                                            }
                                        }

                                        List<DAO_SiswaAbsenMapel.AbsenMapel> lst_absen_mapel = new List<DAO_SiswaAbsenMapel.AbsenMapel>();
                                        if (s_kelas_formasi.Trim() != "")
                                        {
                                            //lst_absen_mapel = DAO_SiswaAbsenMapel.GetAllByTABySMByPeriodeByKelas_Entity(
                                            //    tahun_ajaran, semester,
                                            //    (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                            //    (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue),
                                            //    s_kelas_formasi
                                            //);
                                            lst_absen_mapel = lst_absen_mapel_all.FindAll(m0 => m0.Rel_KelasDet.ToUpper().Trim() == s_kelas_formasi.ToUpper().Trim());
                                        }

                                        if (item_rapor_desain_det.Poin.Trim() != "")
                                        {
                                            poin = item_rapor_desain_det.Poin.Trim();
                                        }

                                        int id_tagihan = 0;

                                        RaporLTS m = new RaporLTS();
                                        RaporLTSDeskripsi m_des = new RaporLTSDeskripsi();

                                        List<NilaiLTS> lst_nilai_lts = new List<NilaiLTS>();
                                        lst_nilai_lts.Clear();

                                        int id_tagihan_pengetahuan = 0;
                                        List<NilaiLTS> lst_nilai_lts_pengetahuan = new List<NilaiLTS>();
                                        lst_nilai_lts_pengetahuan.Clear();

                                        int id_tagihan_keterampilan = 0;
                                        List<NilaiLTS> lst_nilai_lts_keterampilan = new List<NilaiLTS>();
                                        lst_nilai_lts_keterampilan.Clear();

                                        Rapor_NilaiSiswa_KURTILAS_Det_Lengkap m_nilai_siswa = new Rapor_NilaiSiswa_KURTILAS_Det_Lengkap();
                                        Rapor_NilaiSiswa_KURTILAS_Det_Lengkap m_nilai_siswa_ok = new Rapor_NilaiSiswa_KURTILAS_Det_Lengkap();

                                        //nilai LTS
                                        m.TahunAjaran = tahun_ajaran;
                                        m.Semester = semester;
                                        m.Kelas = m_kelas_det.Nama;
                                        m.Rel_Siswa = m_siswa.Kode.ToString();
                                        m.NIS = m_siswa.NISSekolah;
                                        m.NISN = m_siswa.NISN;
                                        m.Nama = m_siswa.Nama.Trim().ToUpper();
                                        m.KodeKelompokMapel = poin;
                                        m.KelompokMapel = item_rapor_desain_det.NamaMapelRapor;
                                        m.Rel_Mapel = item_rapor_desain_det.Rel_Mapel;
                                        m.NamaMapel = item_rapor_desain_det.NamaMapelRapor;
                                        m.KKM = "";
                                        m.TTDGuru = img_ttd_guru;
                                        m.QRCode = qr_code;

                                        //deskripsi LTS
                                        m_des.Rel_Siswa = m_siswa.Kode.ToString();
                                        m_des.Nama = m_siswa.Nama.Trim().ToUpper();
                                        m_des.TahunAjaran = tahun_ajaran;
                                        m_des.Semester = semester;
                                        m_des.KodeKelompokMapel = poin;
                                        m_des.KelompokMapel = item_rapor_desain_det.NamaMapelRapor;
                                        m_des.Rel_Mapel = item_rapor_desain_det.Rel_Mapel;
                                        m_des.NamaMapel = item_rapor_desain_det.NamaMapelRapor;

                                        List<NilaiLTSMapel> lst_lts_mapel = new List<NilaiLTSMapel>();
                                        lst_lts_mapel.Clear();

                                        if (item_rapor_desain_det.Rel_Mapel.Trim() != "")
                                        {
                                            Rapor_StrukturNilai_KURTILAS m_sn_kurtilas = lst_sn_kurtilas.FindAll(
                                                    m0 => m0.TahunAjaran == tahun_ajaran &&
                                                          m0.Semester == semester &&
                                                          m0.Rel_Mapel.ToString().Trim().ToUpper() == item_rapor_desain_det.Rel_Mapel.Trim().Trim().ToUpper()
                                                ).FirstOrDefault();

                                            string s_nama_kd = "";

                                            if (m_sn_kurtilas != null)
                                            {
                                                if (m_sn_kurtilas.TahunAjaran != null)
                                                {
                                                    id_tagihan = 0;
                                                    List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian_ = lst_aspek_penilaian.FindAll(
                                                                m0 => m0.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() == m_sn_kurtilas.Kode.ToString().Trim().ToUpper()
                                                            );
                                                    //load kurtilas ap
                                                    foreach (Rapor_StrukturNilai_KURTILAS_AP m_sn_ap in lst_aspek_penilaian_)
                                                    {
                                                        if (m_sn_ap != null)
                                                        {
                                                            if (m_sn_ap.JenisPerhitungan != null)
                                                            {
                                                                Rapor_AspekPenilaian m_ap =
                                                                    lst_ap.FindAll(
                                                                        m0 => m0.Kode.ToString().Trim().ToUpper() == m_sn_ap.Rel_Rapor_AspekPenilaian.ToString().Trim().ToUpper()
                                                                    ).FirstOrDefault();

                                                                if (m_ap != null)
                                                                {
                                                                    if (m_ap.Nama != null)
                                                                    {
                                                                        //load kurtilas kd
                                                                        int id_kd = 1;
                                                                        List<Rapor_StrukturNilai_KURTILAS_KD> lst_kompetensi_dasar_ =
                                                                            lst_kompetensi_dasar.FindAll(m0 => m0.Rel_Rapor_StrukturNilai_AP.ToString().Trim().ToUpper() == m_sn_ap.Kode.ToString().Trim().ToUpper());
                                                                        foreach (Rapor_StrukturNilai_KURTILAS_KD m_sn_kd in lst_kompetensi_dasar_)
                                                                        {
                                                                            if (m_sn_kd != null)
                                                                            {
                                                                                if (m_sn_kd.JenisPerhitungan != null)
                                                                                {
                                                                                    Rapor_KompetensiDasar m_kd =
                                                                                        lst_kd.FindAll(
                                                                                            m0 => m0.Kode.ToString().Trim().ToUpper() == m_sn_kd.Rel_Rapor_KompetensiDasar.ToString().Trim().ToUpper()
                                                                                        ).FirstOrDefault();

                                                                                    if (m_kd != null)
                                                                                    {
                                                                                        if (m_kd.Nama != null)
                                                                                        {

                                                                                            //load kurtilas kp
                                                                                            List<Rapor_StrukturNilai_KURTILAS_KP> lst_komponen_penilaian_ =
                                                                                                lst_komponen_penilaian.FindAll(m0 => m0.Rel_Rapor_StrukturNilai_KD.ToString().Trim().ToUpper() == m_sn_kd.Kode.ToString().Trim().ToUpper());
                                                                                            foreach (Rapor_StrukturNilai_KURTILAS_KP m_sn_kp in lst_komponen_penilaian_)
                                                                                            {
                                                                                                Rapor_KomponenPenilaian m_kp =
                                                                                                    lst_kp.FindAll(
                                                                                                        m0 => m0.Kode.ToString().Trim().ToUpper() == m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString().Trim().ToUpper()
                                                                                                    ).FirstOrDefault();

                                                                                                if (m_kp != null)
                                                                                                {
                                                                                                    if (m_kp.Nama != null)
                                                                                                    {
                                                                                                        if (s_kelas_formasi.Trim() != "")
                                                                                                        {
                                                                                                            if (lst_nilai_det.FindAll(
                                                                                                                m0 => m0.Rel_KelasDet.ToString().ToUpper().Trim() == s_kelas_formasi.ToString().ToUpper().Trim() &&
                                                                                                                      m0.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() == m_sn_ap.Kode.ToString().ToUpper() &&
                                                                                                                      m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() == m_sn_kd.Kode.ToString().ToUpper() &&
                                                                                                                      m0.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().ToUpper() == m_sn_kp.Kode.ToString().ToUpper() &&
                                                                                                                      m0.Nilai.Trim() != ""
                                                                                                            ).Count > 0)
                                                                                                            {
                                                                                                                s_nama_kd = "";
                                                                                                                if (m_kd.Nama.ToUpper().Trim().IndexOf(Constantas.JENIS_KD_PENGETAHUAN) >= 0)
                                                                                                                {
                                                                                                                    s_nama_kd = Constantas.JENIS_KD_PENGETAHUAN;
                                                                                                                    id_tagihan_pengetahuan++;
                                                                                                                    id_tagihan = id_tagihan_pengetahuan;
                                                                                                                } else if (m_kd.Nama.ToUpper().Trim().IndexOf(Constantas.JENIS_KD_KETERAMPILAN) >= 0)
                                                                                                                {
                                                                                                                    s_nama_kd = Constantas.JENIS_KD_KETERAMPILAN;
                                                                                                                    id_tagihan_keterampilan++;
                                                                                                                    id_tagihan = id_tagihan_keterampilan;
                                                                                                                }

                                                                                                                lst_lts_mapel.Add(new NilaiLTSMapel
                                                                                                                {
                                                                                                                    Rel_Rapor_StrukturNilai = m_sn_kurtilas.Kode.ToString().ToUpper(),
                                                                                                                    Rel_Rapor_StrukturNilai_KURTILAS_AP = m_sn_ap.Kode.ToString().ToUpper(),
                                                                                                                    Rel_Rapor_StrukturNilai_KURTILAS_KD = m_sn_kd.Kode.ToString().ToUpper(),
                                                                                                                    Rel_Rapor_StrukturNilai_KURTILAS_KP = m_sn_kp.Kode.ToString().ToUpper(),
                                                                                                                    DeskripsiLTS = "",
                                                                                                                    UrutanTagihan = id_tagihan,
                                                                                                                    NamaKP = s_nama_kd
                                                                                                                });
                                                                                                            }
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            if (lst_nilai_det.FindAll(
                                                                                                                m0 => m0.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() == m_sn_ap.Kode.ToString().ToUpper() &&
                                                                                                                      m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() == m_sn_kd.Kode.ToString().ToUpper() &&
                                                                                                                      m0.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().ToUpper() == m_sn_kp.Kode.ToString().ToUpper() &&
                                                                                                                      m0.Nilai.Trim() != ""
                                                                                                            ).Count > 0)
                                                                                                            {
                                                                                                                s_nama_kd = "";
                                                                                                                if (m_kd.Nama.ToUpper().Trim().IndexOf(Constantas.JENIS_KD_PENGETAHUAN) >= 0)
                                                                                                                {
                                                                                                                    s_nama_kd = Constantas.JENIS_KD_PENGETAHUAN;
                                                                                                                    id_tagihan_pengetahuan++;
                                                                                                                    id_tagihan = id_tagihan_pengetahuan;
                                                                                                                }
                                                                                                                else if (m_kd.Nama.ToUpper().Trim().IndexOf(Constantas.JENIS_KD_KETERAMPILAN) >= 0)
                                                                                                                {
                                                                                                                    s_nama_kd = Constantas.JENIS_KD_KETERAMPILAN;
                                                                                                                    id_tagihan_keterampilan++;
                                                                                                                    id_tagihan = id_tagihan_keterampilan;
                                                                                                                }

                                                                                                                lst_lts_mapel.Add(new NilaiLTSMapel
                                                                                                                {
                                                                                                                    Rel_Rapor_StrukturNilai = m_sn_kurtilas.Kode.ToString().ToUpper(),
                                                                                                                    Rel_Rapor_StrukturNilai_KURTILAS_AP = m_sn_ap.Kode.ToString().ToUpper(),
                                                                                                                    Rel_Rapor_StrukturNilai_KURTILAS_KD = m_sn_kd.Kode.ToString().ToUpper(),
                                                                                                                    Rel_Rapor_StrukturNilai_KURTILAS_KP = m_sn_kp.Kode.ToString().ToUpper(),
                                                                                                                    DeskripsiLTS = "",
                                                                                                                    UrutanTagihan = id_tagihan,
                                                                                                                    NamaKP = s_nama_kd
                                                                                                                });
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }

                                                                                        }
                                                                                    }

                                                                                    id_kd++;
                                                                                }
                                                                            }
                                                                        }
                                                                        //end load kurtilas kd

                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                    //end load kurtilas ap

                                                    m.KKM = Math.Round(m_sn_kurtilas.KKM).ToString();

                                                }
                                            }
                                        }

                                        //nilai LTS
                                        m.T1 = "";
                                        m.T2 = "";
                                        m.T3 = "";
                                        m.T4 = "";
                                        m.T5 = "";
                                        m.T6 = "";
                                        m.T7 = "";
                                        m.T8 = "";
                                        m.T9 = "";
                                        m.T10 = "";
                                        m.T11 = "";
                                        m.T12 = "";
                                        m.T13 = "";
                                        m.T14 = "";
                                        m.T15 = "";

                                        m.T1_Deskripsi = "";
                                        m.T2_Deskripsi = "";
                                        m.T3_Deskripsi = "";
                                        m.T4_Deskripsi = "";
                                        m.T5_Deskripsi = "";
                                        m.T6_Deskripsi = "";
                                        m.T7_Deskripsi = "";
                                        m.T8_Deskripsi = "";
                                        m.T9_Deskripsi = "";
                                        m.T10_Deskripsi = "";
                                        m.T11_Deskripsi = "";
                                        m.T12_Deskripsi = "";
                                        m.T13_Deskripsi = "";
                                        m.T14_Deskripsi = "";
                                        m.T15_Deskripsi = "";

                                        m.HD = "";
                                        m.LK = "";
                                        m.RJ = "";
                                        m.RPKB = "";
                                        m.NamaEkskul1 = "";
                                        m.HadirEkskul1 = "";
                                        m.NamaEkskul2 = "";
                                        m.HadirEkskul2 = "";
                                        m.NamaEkskul3 = "";
                                        m.HadirEkskul3 = "";
                                        m.Sakit = s_sakit;
                                        m.Izin = s_izin;
                                        m.Alpa = s_alpa;
                                        m.TanggalRapor = s_tanggal_rapor;

                                        id_ekskul = 1;
                                        foreach (DAO_FormasiEkskulDet.AbsenEkskulLTS item_ekskul_det in lst_ekskul_det)
                                        {
                                            if (id_ekskul == 1)
                                            {
                                                m.NamaEkskul1 = item_ekskul_det.Mapel;
                                                m.HadirEkskul1 = (item_ekskul_det.LTS_HD.Trim() == "" ? "0" : item_ekskul_det.LTS_HD);
                                            }
                                            else if (id_ekskul == 2)
                                            {
                                                m.NamaEkskul2 = item_ekskul_det.Mapel;
                                                m.HadirEkskul2 = (item_ekskul_det.LTS_HD.Trim() == "" ? "0" : item_ekskul_det.LTS_HD);
                                            }
                                            else if (id_ekskul == 3)
                                            {
                                                m.NamaEkskul3 = item_ekskul_det.Mapel;
                                                m.HadirEkskul3 = (item_ekskul_det.LTS_HD.Trim() == "" ? "0" : item_ekskul_det.LTS_HD);
                                            }
                                            id_ekskul++;
                                        }

                                        //deskripsi LTS
                                        m_des.PoinTCol1_1 = "";
                                        m_des.PoinTCol1_2 = "";
                                        m_des.PoinTCol1_3 = "";
                                        m_des.PoinTCol1_4 = "";
                                        m_des.PoinTCol1_5 = "";
                                        m_des.PoinTCol1_6 = "";
                                        m_des.DesPoinTCol1_1 = "";
                                        m_des.DesPoinTCol1_2 = "";
                                        m_des.DesPoinTCol1_3 = "";
                                        m_des.DesPoinTCol1_4 = "";
                                        m_des.DesPoinTCol1_5 = "";
                                        m_des.DesPoinTCol1_6 = "";
                                        m_des.PoinTCol2_1 = "";
                                        m_des.PoinTCol2_2 = "";
                                        m_des.PoinTCol2_3 = "";
                                        m_des.PoinTCol2_4 = "";
                                        m_des.PoinTCol2_5 = "";
                                        m_des.PoinTCol2_6 = "";
                                        m_des.DesPoinTCol2_1 = "";
                                        m_des.DesPoinTCol2_2 = "";
                                        m_des.DesPoinTCol2_3 = "";
                                        m_des.DesPoinTCol2_4 = "";
                                        m_des.DesPoinTCol2_5 = "";
                                        m_des.DesPoinTCol2_6 = "";

                                        lst_nilai_lts.Clear();
                                        lst_nilai_lts_pengetahuan.Clear();
                                        lst_nilai_lts_keterampilan.Clear();

                                        m_nilai_siswa = new Rapor_NilaiSiswa_KURTILAS_Det_Lengkap();
                                        
                                        bool ada_nilai = false;
                                        //pengetahuan
                                        foreach (NilaiLTSMapel item_lts_mapel in lst_lts_mapel.FindAll(m0 => m0.NamaKP == Constantas.JENIS_KD_PENGETAHUAN).OrderBy(m0 => m0.UrutanTagihan))
                                        {
                                            m_nilai_siswa = lst_nilai_det.FindAll(
                                                    m0 => m0.Rel_Siswa.ToString().ToUpper() == m_siswa.Kode.ToString().ToUpper() &&
                                                          m0.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() &&
                                                          m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() &&
                                                          m0.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().ToUpper()
                                                ).FirstOrDefault();

                                            string s_deksripsi_lts = "";
                                            if (m_nilai_siswa == null)
                                            {
                                                m_nilai_siswa = new Rapor_NilaiSiswa_KURTILAS_Det_Lengkap();
                                                //m_nilai_siswa.Nilai = "BL";
                                                m_nilai_siswa.Nilai = "";

                                                Rapor_StrukturNilai_Deskripsi m_sn_deskripsi = lst_deskripsi.FindAll(
                                                        m0 => m0.Rel_KelasDet.ToString().Trim().ToUpper() == s_kelas_formasi.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai.Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai_AP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai_KD.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai_KP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().Trim().ToUpper()
                                                    ).FirstOrDefault();

                                                if (m_sn_deskripsi != null)
                                                {
                                                    if (m_sn_deskripsi.TahunAjaran != null)
                                                    {
                                                        s_deksripsi_lts = m_sn_deskripsi.Deskripsi;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (m_nilai_siswa.Rel_Siswa != null)
                                                {
                                                    Rapor_StrukturNilai_Deskripsi m_sn_deskripsi = lst_deskripsi.FindAll(
                                                        m0 => m0.Rel_KelasDet.ToString().Trim().ToUpper() == m_nilai_siswa.Rel_KelasDet.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai.Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai_AP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai_KD.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai_KP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().Trim().ToUpper()
                                                    ).FirstOrDefault();

                                                    if (m_sn_deskripsi == null)
                                                    {
                                                        m_sn_deskripsi = lst_deskripsi.FindAll(
                                                            m0 => m0.Rel_KelasDet.ToString().Trim().ToUpper() == rel_kelas_det.ToString().Trim().ToUpper() &&
                                                                  m0.Rel_StrukturNilai.Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() &&
                                                                  m0.Rel_StrukturNilai_AP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().Trim().ToUpper() &&
                                                                  m0.Rel_StrukturNilai_KD.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().Trim().ToUpper() &&
                                                                  m0.Rel_StrukturNilai_KP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().Trim().ToUpper()
                                                        ).FirstOrDefault();
                                                    }

                                                    if (m_sn_deskripsi != null)
                                                    {
                                                        if (m_sn_deskripsi.TahunAjaran != null)
                                                        {
                                                            s_deksripsi_lts = m_sn_deskripsi.Deskripsi;
                                                        }
                                                    }

                                                    m_nilai_siswa_ok = m_nilai_siswa;
                                                    if (m_nilai_siswa.Nilai.Trim() != "") ada_nilai = true;
                                                }
                                            }

                                            lst_nilai_lts_pengetahuan.Add(new NilaiLTS
                                            {
                                                Nilai = m_nilai_siswa.Nilai,
                                                DeskripsiLTS = s_deksripsi_lts,
                                                UrutanTagihan = item_lts_mapel.UrutanTagihan
                                            });
                                        }

                                        //keterampilan
                                        foreach (NilaiLTSMapel item_lts_mapel in lst_lts_mapel.FindAll(m0 => m0.NamaKP == Constantas.JENIS_KD_KETERAMPILAN).OrderBy(m0 => m0.UrutanTagihan))
                                        {
                                            m_nilai_siswa = lst_nilai_det.FindAll(
                                                    m0 => m0.Rel_Siswa.ToString().ToUpper() == m_siswa.Kode.ToString().ToUpper() &&
                                                          m0.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() &&
                                                          m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() &&
                                                          m0.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().ToUpper()
                                                ).FirstOrDefault();

                                            string s_deksripsi_lts = "";
                                            if (m_nilai_siswa == null)
                                            {
                                                m_nilai_siswa = new Rapor_NilaiSiswa_KURTILAS_Det_Lengkap();
                                                //m_nilai_siswa.Nilai = "BL";
                                                m_nilai_siswa.Nilai = "";

                                                Rapor_StrukturNilai_Deskripsi m_sn_deskripsi = lst_deskripsi.FindAll(
                                                        m0 => m0.Rel_KelasDet.ToString().Trim().ToUpper() == s_kelas_formasi.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai.Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai_AP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai_KD.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai_KP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().Trim().ToUpper()
                                                    ).FirstOrDefault();

                                                if (m_sn_deskripsi != null)
                                                {
                                                    if (m_sn_deskripsi.TahunAjaran != null)
                                                    {
                                                        s_deksripsi_lts = m_sn_deskripsi.Deskripsi;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (m_nilai_siswa.Rel_Siswa != null)
                                                {
                                                    Rapor_StrukturNilai_Deskripsi m_sn_deskripsi = lst_deskripsi.FindAll(
                                                        m0 => m0.Rel_KelasDet.ToString().Trim().ToUpper() == m_nilai_siswa.Rel_KelasDet.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai.Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai_AP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai_KD.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().Trim().ToUpper() &&
                                                              m0.Rel_StrukturNilai_KP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().Trim().ToUpper()
                                                    ).FirstOrDefault();

                                                    if (m_sn_deskripsi == null)
                                                    {
                                                        m_sn_deskripsi = lst_deskripsi.FindAll(
                                                            m0 => m0.Rel_KelasDet.ToString().Trim().ToUpper() == rel_kelas_det.ToString().Trim().ToUpper() &&
                                                                  m0.Rel_StrukturNilai.Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() &&
                                                                  m0.Rel_StrukturNilai_AP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().Trim().ToUpper() &&
                                                                  m0.Rel_StrukturNilai_KD.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().Trim().ToUpper() &&
                                                                  m0.Rel_StrukturNilai_KP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().Trim().ToUpper()
                                                        ).FirstOrDefault();
                                                    }

                                                    if (m_sn_deskripsi != null)
                                                    {
                                                        if (m_sn_deskripsi.TahunAjaran != null)
                                                        {
                                                            s_deksripsi_lts = m_sn_deskripsi.Deskripsi;
                                                        }
                                                    }

                                                    m_nilai_siswa_ok = m_nilai_siswa;
                                                    if (m_nilai_siswa.Nilai.Trim() != "") ada_nilai = true;
                                                }
                                            }

                                            lst_nilai_lts_keterampilan.Add(new NilaiLTS
                                            {
                                                Nilai = m_nilai_siswa.Nilai,
                                                DeskripsiLTS = s_deksripsi_lts,
                                                UrutanTagihan = item_lts_mapel.UrutanTagihan
                                            });
                                        }

                                        Rapor_NilaiSiswa_KURTILAS m_nilai = new Rapor_NilaiSiswa_KURTILAS();
                                        if (m_nilai_siswa_ok != null)
                                        {
                                            if (m_nilai_siswa_ok.Rel_Siswa != null)
                                            {
                                                //Rapor_NilaiSiswa_KURTILAS m_nilai = DAO_Rapor_NilaiSiswa_KURTILAS.GetByID_Entity(m_nilai_siswa_ok.Rel_Rapor_NilaiSiswa_KURTILAS.ToString());
                                                m_nilai = lst_nilai_siswa.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == m_nilai_siswa_ok.Rel_Rapor_NilaiSiswa_KURTILAS.ToString().ToUpper().Trim()).FirstOrDefault();

                                                if (m_nilai != null)
                                                {
                                                    if (m_nilai.Rel_Siswa != null)
                                                    {
                                                        //absen
                                                        string s_jumlah_hadir = "0";
                                                        string s_jumlah_hadir_max = "0";
                                                        DAO_SiswaAbsenMapel.AbsenMapel m_absen_mapel = lst_absen_mapel.FindAll(
                                                                m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == item_rapor_desain_det.Rel_Mapel.ToString().ToUpper().Trim() &&
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

                                                        if (
                                                                (m_nilai.LTS_HD.Trim() != "" && m_nilai.LTS_MAX_HD.Trim() != "")
                                                           //||
                                                           //tahun_ajaran == "2020/2021"
                                                           )
                                                        {
                                                            m.HD = (m_nilai.LTS_HD.Trim() == "" ? "-" : m_nilai.LTS_HD.Trim()) +
                                                                   "/" +
                                                                   (m_nilai.LTS_MAX_HD.Trim() == "" ? "-" : m_nilai.LTS_MAX_HD.Trim());
                                                        }
                                                        else
                                                        {
                                                            m.HD = s_jumlah_hadir +
                                                                   "/" +
                                                                   s_jumlah_hadir_max;
                                                        }
                                                        m.LK = m_nilai.LTS_LK;
                                                        m.RJ = m_nilai.LTS_RJ;
                                                        m.RPKB = m_nilai.LTS_RPKB;
                                                    }
                                                }
                                            }
                                        }

                                        //read pengetahuan
                                        foreach (var item in lst_nilai_lts_pengetahuan)
                                        {
                                            if (item.UrutanTagihan == 1)
                                            {
                                                m.T1 = item.Nilai.ToUpper().Trim();
                                                m.T1_Deskripsi = item.DeskripsiLTS;
                                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + item.UrutanTagihan.ToString() + ":";
                                                m_des.DesPoinTCol1_1 = item.DeskripsiLTS;
                                            }
                                            else if (item.UrutanTagihan == 2)
                                            {
                                                m.T2 = item.Nilai.ToUpper().Trim();
                                                m.T2_Deskripsi = item.DeskripsiLTS;
                                                m_des.PoinTCol1_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + item.UrutanTagihan.ToString() + ":";
                                                m_des.DesPoinTCol1_2 = item.DeskripsiLTS;
                                            }
                                            else if (item.UrutanTagihan == 3)
                                            {
                                                m.T3 = item.Nilai.ToUpper().Trim();
                                                m.T3_Deskripsi = item.DeskripsiLTS;
                                                m_des.PoinTCol1_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + item.UrutanTagihan.ToString() + ":";
                                                m_des.DesPoinTCol1_3 = item.DeskripsiLTS;
                                            }
                                            else if (item.UrutanTagihan == 4)
                                            {
                                                m.T4 = item.Nilai.ToUpper().Trim();
                                                m.T4_Deskripsi = item.DeskripsiLTS;
                                                m_des.PoinTCol1_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + item.UrutanTagihan.ToString() + ":";
                                                m_des.DesPoinTCol1_4 = item.DeskripsiLTS;
                                            }
                                            else if (item.UrutanTagihan == 5)
                                            {
                                                m.T5 = item.Nilai.ToUpper().Trim();
                                                m.T5_Deskripsi = item.DeskripsiLTS;
                                                m_des.PoinTCol1_5 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + item.UrutanTagihan.ToString() + ":";
                                                m_des.DesPoinTCol1_5 = item.DeskripsiLTS;
                                            }
                                            else if (item.UrutanTagihan == 6)
                                            {
                                                m.T6 = item.Nilai.ToUpper().Trim();
                                                m.T6_Deskripsi = item.DeskripsiLTS;
                                                m_des.PoinTCol1_6 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + item.UrutanTagihan.ToString() + ":";
                                                m_des.DesPoinTCol1_6 = item.DeskripsiLTS;
                                            }
                                        }

                                        //read keterampilan
                                        foreach (var item in lst_nilai_lts_keterampilan)
                                        {
                                            if (item.UrutanTagihan == 1)
                                            {
                                                m.T7 = item.Nilai.ToUpper().Trim();
                                                m.T7_Deskripsi = item.DeskripsiLTS;
                                                m_des.PoinTCol2_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + item.UrutanTagihan.ToString() + ":";
                                                m_des.DesPoinTCol2_1 = item.DeskripsiLTS;
                                            }
                                            else if (item.UrutanTagihan == 2)
                                            {
                                                m.T8 = item.Nilai.ToUpper().Trim();
                                                m.T8_Deskripsi = item.DeskripsiLTS;
                                                m_des.PoinTCol2_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + item.UrutanTagihan.ToString() + ":";
                                                m_des.DesPoinTCol2_2 = item.DeskripsiLTS;
                                            }
                                            else if (item.UrutanTagihan == 3)
                                            {
                                                m.T9 = item.Nilai.ToUpper().Trim();
                                                m.T9_Deskripsi = item.DeskripsiLTS;
                                                m_des.PoinTCol2_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + item.UrutanTagihan.ToString() + ":";
                                                m_des.DesPoinTCol2_3 = item.DeskripsiLTS;
                                            }
                                            else if (item.UrutanTagihan == 4)
                                            {
                                                m.T10 = item.Nilai.ToUpper().Trim();
                                                m.T10_Deskripsi = item.DeskripsiLTS;
                                                m_des.PoinTCol2_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + item.UrutanTagihan.ToString() + ":";
                                                m_des.DesPoinTCol2_4 = item.DeskripsiLTS;
                                            }
                                            else if (item.UrutanTagihan == 5)
                                            {
                                                m.T11 = item.Nilai.ToUpper().Trim();
                                                m.T11_Deskripsi = item.DeskripsiLTS;
                                                m_des.PoinTCol2_5 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + item.UrutanTagihan.ToString() + ":";
                                                m_des.DesPoinTCol2_5 = item.DeskripsiLTS;
                                            }
                                            else if (item.UrutanTagihan == 6)
                                            {
                                                m.T12 = item.Nilai.ToUpper().Trim();
                                                m.T12_Deskripsi = item.DeskripsiLTS;
                                                m_des.PoinTCol2_6 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + item.UrutanTagihan.ToString() + ":";
                                                m_des.DesPoinTCol2_6 = item.DeskripsiLTS;
                                            }
                                        }

                                        m.WaliKelas = s_walikelas;

                                        Mapel m_mapel = lst_mapel.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item_rapor_desain_det.Rel_Mapel.ToUpper().Trim()).FirstOrDefault();
                                        bool b_tampil = true;
                                        string s_nomor_mapel = item_rapor_desain_det.Nomor;

                                        m.NomorMapel = s_nomor_mapel;
                                        m_des.NomorMapel = s_nomor_mapel;

                                        //capaian kedisiplinan
                                        Rapor_NilaiSiswa_KURTILAS m_nilaisiswa = lst_nilaisiswa.FindAll(
                                                    m0 => m0.Kode.ToString().ToUpper().Trim() == m_nilai.Kode.ToString().ToUpper().Trim()
                                                ).FirstOrDefault();
                                        bool ada_nilai_kedisiplinan = false;
                                        RaporLTSCapaianKedisiplinan m_rapor_lts_capaian_kedisiplinan = new RaporLTSCapaianKedisiplinan();

                                        if (item_rapor_desain_det.Poin.Trim() != "")
                                        {
                                            poin = item_rapor_desain_det.Poin.Trim();
                                        }
                                        if (m_nilaisiswa != null)
                                        {
                                            if (m_nilaisiswa.Rel_Siswa != null)
                                            {
                                                ada_nilai_kedisiplinan = true;
                                                urut_mapel++;
                                                if (item_rapor_desain_det.Poin.Trim() == "") nomor_mapel++;

                                                m_rapor_lts_capaian_kedisiplinan.Rel_Siswa = m_nilaisiswa.Rel_Siswa;
                                                m_rapor_lts_capaian_kedisiplinan.KodeKelompokMapel = poin;
                                                m_rapor_lts_capaian_kedisiplinan.KelompokMapel = item_rapor_desain_det.NamaMapelRapor;
                                                m_rapor_lts_capaian_kedisiplinan.NomorMapel = s_nomor_mapel;
                                                m_rapor_lts_capaian_kedisiplinan.Rel_Mapel = item_rapor_desain_det.Rel_Mapel;
                                                m_rapor_lts_capaian_kedisiplinan.NamaMapel = (item_rapor_desain_det.Poin.Trim() != "" ? item_rapor_desain_det.Poin + " " : "") + item_rapor_desain_det.NamaMapelRapor;
                                                m_rapor_lts_capaian_kedisiplinan.Kehadiran = m_nilaisiswa.LTS_CK_KEHADIRAN;
                                                m_rapor_lts_capaian_kedisiplinan.KetepatanWaktu = m_nilaisiswa.LTS_CK_KETEPATAN_WKT;
                                                m_rapor_lts_capaian_kedisiplinan.PenggunaanSeragam = m_nilaisiswa.LTS_CK_PENGGUNAAN_SRGM;
                                                m_rapor_lts_capaian_kedisiplinan.PenggunaanKamera = m_nilaisiswa.LTS_CK_PENGGUNAAN_KMR;
                                                m_rapor_lts_capaian_kedisiplinan.UrutanMapel = urut_mapel;
                                            }
                                        }
                                        if (!ada_nilai_kedisiplinan)
                                        {
                                            ada_nilai_kedisiplinan = true;

                                            urut_mapel++;
                                            if (item_rapor_desain_det.Poin.Trim() == "") nomor_mapel++;

                                            m_rapor_lts_capaian_kedisiplinan.Rel_Siswa = m_siswa.Kode.ToString();
                                            m_rapor_lts_capaian_kedisiplinan.KodeKelompokMapel = poin;
                                            m_rapor_lts_capaian_kedisiplinan.KelompokMapel = item_rapor_desain_det.NamaMapelRapor;
                                            m_rapor_lts_capaian_kedisiplinan.NomorMapel = s_nomor_mapel;
                                            m_rapor_lts_capaian_kedisiplinan.Rel_Mapel = item_rapor_desain_det.Rel_Mapel;
                                            m_rapor_lts_capaian_kedisiplinan.NamaMapel = (item_rapor_desain_det.Poin.Trim() != "" ? item_rapor_desain_det.Poin + " " : "") +
                                                                                         (
                                                                                            item_rapor_desain_det.Rel_Mapel.Trim() == ""
                                                                                            ? item_rapor_desain_det.NamaMapelRapor.Replace(":", "") + ":"
                                                                                            : item_rapor_desain_det.NamaMapelRapor
                                                                                         );
                                            m_rapor_lts_capaian_kedisiplinan.Kehadiran = "";
                                            m_rapor_lts_capaian_kedisiplinan.KetepatanWaktu = "";
                                            m_rapor_lts_capaian_kedisiplinan.PenggunaanSeragam = "";
                                            m_rapor_lts_capaian_kedisiplinan.PenggunaanKamera = "";
                                            m_rapor_lts_capaian_kedisiplinan.UrutanMapel = urut_mapel;
                                        }
                                        //end capaian kedisiplinan

                                        if (m.KodeKelompokMapel.Trim() != "" && m.NomorMapel.ToString().Trim() == "")
                                        {
                                            s_html += "<tr>" +
                                                            "<td style=\"width: 40px; vertical-align: middle; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.KodeKelompokMapel + "</td>" +
                                                            "<td colspan=\"18\" style=\"vertical-align: middle; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.NamaMapel + "</td>" +
                                                          "</tr>";

                                            s_html_deskripsi +=
                                                      "<tr>" +
                                                        "<td style=\"width: 40px; vertical-align: middle; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.KodeKelompokMapel + "</td>" +
                                                        "<td colspan=\"5\" style=\"vertical-align: middle; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.NamaMapel + "</td>" +
                                                      "</tr>";
                                        }
                                        else
                                        {
                                            if (m_mapel != null)
                                            {
                                                if (m_mapel.Nama != null)
                                                {
                                                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN && !ada_nilai)
                                                    {
                                                        b_tampil = false;
                                                    }
                                                    else if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT && ada_nilai)
                                                    {
                                                        b_tampil = true;
                                                        id_lintas_minat++;
                                                        s_nomor_mapel = id_lintas_minat.ToString();
                                                        m.NomorMapel = s_nomor_mapel;
                                                        m_des.NomorMapel = s_nomor_mapel;
                                                        m_rapor_lts_capaian_kedisiplinan.NomorMapel = s_nomor_mapel;
                                                    }
                                                    else if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT && !ada_nilai)
                                                    {
                                                        b_tampil = false;
                                                    }
                                                }
                                            }

                                            if (b_tampil)
                                            {
                                                s_html += "<tr>" +
                                                            "<td style=\"width: 40px; vertical-align: middle; text-align: right; padding: 3px; border-style: solid; border-width: 0.5px;\">" + s_nomor_mapel + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.NamaMapel + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;\">" + m.KKM + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T1) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T1_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T1_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T1 + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T2) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T2_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T2_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T2 + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T3) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T3_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T3_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T3 + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T4) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T4_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T4_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T4 + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T5) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T5_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T5_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T5 + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T6) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T6_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T6_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T6 + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T7) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T7_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T7_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T7 + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T8) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T8_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T8_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T8 + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T9) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T9_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T9_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T9 + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T10) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T10_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T10_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T10 + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T11) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T11_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T11_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T11 + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T12) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T12_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T12_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T12 + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;\">" + m.HD + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;\">" + m.LK + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;\">" + m.RJ + "</td>" +
                                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;\">" + m.RPKB + "</td>" +
                                                        "</tr>";

                                                string s_html_deskripsi_mapel = "";
                                                int rows_span = 0;
                                                int i_count = (lst_nilai_lts.Count > 12 ? 12 : lst_nilai_lts.Count);
                                                if (i_count > 1)
                                                {
                                                    if (i_count % 2 == 0)
                                                    {
                                                        rows_span = Convert.ToInt16(Math.Round(Convert.ToDecimal(i_count / 2), 0));
                                                    }
                                                    else
                                                    {
                                                        rows_span = Convert.ToInt16(Convert.ToDecimal((i_count + 1) / 2));
                                                    }

                                                    if (rows_span >= 1)
                                                    {
                                                        for (int i = 1; i <= rows_span; i++)
                                                        {
                                                            int id_kol_2 = ((i * 2) + (rows_span - i));
                                                            s_html_deskripsi_mapel += "<tr>" +
                                                                                          (
                                                                                            i == 1
                                                                                            ? "<td " + (rows_span > 1 ? "rowspan=\"" + rows_span + "\"" : "") + " style=\"width: 40px; vertical-align: top; text-align: right; padding: 3px; border-style: solid; border-width: 0.5px;\">" + s_nomor_mapel + "</td>" +
                                                                                              "<td " + (rows_span > 1 ? "rowspan=\"" + rows_span + "\"" : "") + " style=\"vertical-align: top; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.NamaMapel + "</td>"
                                                                                            : ""
                                                                                          ) +
                                                                                          "<td style=\"vertical-align: top; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black; width: 30px; border-right-style: none;\">" +
                                                                                            (
                                                                                                i_count >= (rows_span)
                                                                                                ? (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + i.ToString()
                                                                                                : "&nbsp;"
                                                                                            ) +
                                                                                          "</td>" +
                                                                                          "<td style=\"vertical-align: top; text-align: justify; padding: 3px; padding-right: 7px; border-style: solid; border-width: 0.5px; border-color: black; border-left-style: none; width: 30%;\">" +
                                                                                            (
                                                                                                i_count >= (rows_span)
                                                                                                ? Libs.GetHTMLNoParagraphDiAwal(lst_nilai_lts[i - 1].DeskripsiLTS)
                                                                                                : "&nbsp;"
                                                                                            ) +
                                                                                          "</td>";

                                                            s_html_deskripsi_mapel += "<td style=\"vertical-align: top; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black; width: 30px; border-right-style: none;\">" +
                                                                                                (
                                                                                                    i_count > id_kol_2 - 1
                                                                                                    ? (
                                                                                                        (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_kol_2.ToString()
                                                                                                      )
                                                                                                    : "&nbsp;"
                                                                                                ) +
                                                                                              "</td>" +
                                                                                              "<td style=\"vertical-align: top; text-align: justify; padding: 3px; padding-right: 7px; border-style: solid; border-width: 0.5px; border-color: black; border-left-style: none; width: 30%;\">" +
                                                                                                (
                                                                                                    i_count > id_kol_2 - 1
                                                                                                    ? (
                                                                                                        Libs.GetHTMLNoParagraphDiAwal(lst_nilai_lts[id_kol_2 - 1].DeskripsiLTS)
                                                                                                      )
                                                                                                    : "&nbsp;"
                                                                                                ) +
                                                                                              "</td>" +
                                                                                            "</tr>";
                                                        }

                                                        s_html_deskripsi += s_html_deskripsi_mapel;
                                                    }
                                                }
                                                else
                                                {
                                                    s_html_deskripsi_mapel =
                                                        "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black; border-right-style: none;\">" + "&nbsp;" + "</td>" +
                                                        "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black; border-left-style: none; width: 30%;\">" + "&nbsp;" + "</td>" +
                                                        "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black; border-right-style: none;\">" + "&nbsp;" + "</td>" +
                                                        "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black; border-left-style: none; width: 30%;\">" + "&nbsp;" + "</td>";

                                                    s_html_deskripsi +=
                                                        "<tr>" +
                                                            "<td " + (i_count > 1 ? "rowspan=\"" + rows_span + "\"" : "") + " style=\"width: 40px; vertical-align: top; text-align: right; padding: 3px; border-style: solid; border-width: 0.5px;\">" + s_nomor_mapel + "</td>" +
                                                            "<td " + (i_count > 1 ? "rowspan=\"" + rows_span + "\"" : "") + " style=\"vertical-align: top; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.NamaMapel + "</td>" +
                                                            s_html_deskripsi_mapel +
                                                        "</tr>";
                                                }
                                            }
                                        }

                                        if (b_tampil)
                                        {
                                            lst_hasil_lts.Add(m);
                                            lst_hasil_deksripsi.Add(m_des);
                                            lst_hasil_capaian_kedisiplinan.Add(m_rapor_lts_capaian_kedisiplinan);
                                        }

                                    }

                                    s_html += "</table>";

                                    s_html += "<table style=\"width: 100%; font-size: small; margin-top: 20px;\">" +
                                                "<tr>" +
                                                    "<td rowspan=\"7\" style=\"width: 30px; padding: 3px; vertical-align: top;\">" +
                                                        "Ketr." +
                                                    "</td>" +
                                                    "<td style=\"text-align: right; padding: 3px; width: 60px;\">" +
                                                        "KKM" +
                                                    "</td>" +
                                                    "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
                                                        ":" +
                                                    "</td>" +
                                                    "<td style=\"padding: 3px;\">" +
                                                        "Kriteria Ketuntasan Minimal" +
                                                    "</td>" +
                                                    "<td colspan=\"2\" style=\"border-style: solid; border-width: 0.5px; text-align: center; padding-top: 3px; padding-bottom: 3px;\">" +
                                                        "Eksktrakurikuler yang diikuti" +
                                                    "</td>" +
                                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                        "&nbsp;" +
                                                    "</td>" +
                                                    "<td colspan=\"2\" style=\"border-style: solid; border-width: 0.5px; text-align: center; padding-top: 3px; padding-bottom: 3px;\">" +
                                                        "Absensi Siswa" +
                                                    "</td>" +
                                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                        "&nbsp;" +
                                                    "</td>" +
                                                    "<td rowspan=\"7\" style=\"border-style: none; border-width: 0px; text-align: left; padding-top: 3px; padding-bottom: 3px; width: 200px; vertical-align: top;\">" +
                                                        "Jakarta, " +
                                                        (
                                                            m_rapor_arsip != null
                                                            ? (
                                                                    m_rapor_arsip.TanggalRapor != DateTime.MinValue
                                                                    ? Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false)
                                                                    : ""
                                                              )
                                                            : ""
                                                        ) +
                                                        "<br />" +
                                                        "Wali Kelas," +
                                                        "<br />" +
                                                        "<br />" +
                                                        "<br />" +
                                                        "<br />" +
                                                        "<br />" +
                                                        "<label style=\"font-weight: bold; text-decoration: underline;\">" +
                                                        s_walikelas +
                                                        "</label>" +
                                                    "</td>" +
                                                "</tr>";

                                    s_html +=
                                            "<tr>" +
                                                "<td style=\"text-align: right; padding: 3px; width: 60px;\">" +
                                                    "HD" +
                                                "</td>" +
                                                "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 3px;\">" +
                                                    "Kehadiran (jumlah kehadiran siswa" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: center; padding: 3px; width: 150px;\">" +
                                                    "Nama Ekskul" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: center; padding: 3px; width: 100px;\">" +
                                                    "Jumlah Hadir" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: center; padding: 3px; width: 150px;\">" +
                                                    "Ketidakhadiran" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: center; padding: 3px; width: 100px;\">" +
                                                    "Jumlah" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                            "</tr>";

                                    s_html +=
                                            "<tr>" +
                                                "<td style=\"text-align: right; padding: 3px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"padding: 3px; width: 280px;\">" +
                                                    "berbanding dengan total pertemuan)" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: left; padding: 3px;\">" +
                                                    (
                                                        lst_ekskul_det.Count >= 1
                                                        ? lst_ekskul_det[0].Mapel
                                                        : ""
                                                    ) +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
                                                    (
                                                        lst_ekskul_det.Count >= 1
                                                        ? lst_ekskul_det[0].LTS_HD
                                                        : ""
                                                    ) +
                                                    "&nbsp;&nbsp;kali" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: left; padding: 3px;\">" +
                                                    "Sakit" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
                                                    s_sakit +
                                                    "&nbsp;&nbsp;hari" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                            "</tr>";

                                    s_html +=
                                            "<tr>" +
                                                "<td style=\"text-align: right; padding: 3px; width: 60px;\">" +
                                                    "LK" +
                                                "</td>" +
                                                "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 3px;\">" +
                                                    "Kelakuan" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: left; padding: 3px;\">" +
                                                    (
                                                        lst_ekskul_det.Count >= 2
                                                        ? lst_ekskul_det[1].Mapel
                                                        : ""
                                                    ) +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
                                                    (
                                                        lst_ekskul_det.Count >= 2
                                                        ? lst_ekskul_det[1].LTS_HD
                                                        : ""
                                                    ) +
                                                    "&nbsp;&nbsp;kali" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: left; padding: 3px;\">" +
                                                    "Izin" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
                                                    s_izin +
                                                    "&nbsp;&nbsp;hari" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                            "</tr>";

                                    s_html +=
                                            "<tr>" +
                                                "<td style=\"text-align: right; padding: 3px; width: 60px;\">" +
                                                    "RJ" +
                                                "</td>" +
                                                "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 3px;\">" +
                                                    "Kerajinan" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: left; padding: 3px;\">" +
                                                    (
                                                        lst_ekskul_det.Count >= 3
                                                        ? lst_ekskul_det[2].Mapel
                                                        : ""
                                                    ) +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
                                                    (
                                                        lst_ekskul_det.Count >= 3
                                                        ? lst_ekskul_det[2].LTS_HD
                                                        : ""
                                                    ) +
                                                    "&nbsp;&nbsp;kali" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: left; padding: 3px;\">" +
                                                    "Tanpa Keterangan" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
                                                    s_alpa +
                                                    "&nbsp;&nbsp;hari" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                            "</tr>";

                                    s_html +=
                                            "<tr>" +
                                                "<td style=\"text-align: right; padding: 3px; width: 60px;\">" +
                                                    "RPKB" +
                                                "</td>" +
                                                "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 3px;\">" +
                                                    "Kerapian dan Kebersihan" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
                                                    "Jumlah" +
                                                "</td>" +
                                                "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
                                                    (
                                                        Libs.GetStringToDecimal(s_sakit) +
                                                        Libs.GetStringToDecimal(s_izin) +
                                                        Libs.GetStringToDecimal(s_alpa)
                                                    ).ToString() +
                                                    "&nbsp;&nbsp;hari" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                            "</tr>";

                                    s_html +=
                                            "<tr>" +
                                                "<td style=\"text-align: right; padding: 3px; width: 60px;\">" +
                                                    "BL" +
                                                "</td>" +
                                                "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 3px;\">" +
                                                    "Belum" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: center; padding: 3px; width: 150px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: center; padding: 3px; width: 100px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: center; padding: 3px; width: 150px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: center; padding: 3px; width: 100px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                            "</tr>" +

                                          "</table>" +
                                          "<hr />";

                                    s_html += "<table style=\"width: 100%;\">" +
                                                "<tr>" +
                                                    "<td style=\"width: 50%; text-align: left;\">" +
                                                        "Keterangan Tagihan (LTS " + semester + " - TP " + tahun_ajaran + ") *)" +
                                                    "</td>" +
                                                    "<td style=\"width: 50%; text-align: right;\">" +
                                                        m_siswa.Nama.Trim().ToUpper() +
                                                    "</td>" +
                                                "<tr>" +
                                              "</table>" +
                                              "<table style=\"width: 100%; font-size: small;\">" +
                                                s_html_deskripsi +
                                              "</table>";
                                }
                            }

                        }

                        GetHTML = s_html;
                        GetRaporLTS = lst_hasil_lts;
                        GetRaporLTSDeskripsi = lst_hasil_deksripsi;
                        GetRaporLTSCapaianKedisiplinan = lst_hasil_capaian_kedisiplinan;
                    }
                }
            }

            //public LTS(string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa, string s_lokasi_ttd)
            //{
            //    if (tahun_ajaran.IndexOf("/") < 0) tahun_ajaran = RandomLibs.GetParseTahunAjaran(tahun_ajaran);
            //    System.Drawing.Image img = null;
            //    string s_loc = s_lokasi_ttd;
            //    if (File.Exists(s_loc) && s_loc.Trim() != "")
            //    {
            //        img = System.Drawing.Image.FromFile(s_loc);
            //    }
            //    byte[] img_ttd_guru = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));

            //    this.TahunAjaran = tahun_ajaran;
            //    this.Semester = semester;
            //    this.Rel_KelasDet = rel_kelas_det;
            //    this.Rel_Siswa = rel_siswa;

            //    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);

            //    List<RaporLTS> lst_hasil_lts = new List<RaporLTS>();
            //    List<RaporLTSDeskripsi> lst_hasil_deksripsi = new List<RaporLTSDeskripsi>();

            //    List<Rapor_NilaiSiswa_KURTILAS_Det_Lengkap> lst_nilai_det =
            //        DAO_Rapor_NilaiSiswa_KURTILAS_Det.GetAllByTABySMByKelasDet_LENGKAP_Entity(tahun_ajaran, semester, rel_kelas_det);

            //    List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian =
            //        DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByTABySM_Entity(tahun_ajaran, semester);

            //    List<Rapor_StrukturNilai_KURTILAS_KD> lst_kompetensi_dasar =
            //        DAO_Rapor_StrukturNilai_KURTILAS_KD.GetAllByTABySM_Entity(tahun_ajaran, semester);

            //    List<Rapor_StrukturNilai_KURTILAS_KP> lst_komponen_penilaian =
            //        DAO_Rapor_StrukturNilai_KURTILAS_KP.GetAllByTABySM_Entity(tahun_ajaran, semester);

            //    List<Rapor_AspekPenilaian> lst_ap =
            //        DAO_Rapor_AspekPenilaian.GetAll_Entity();

            //    List<Rapor_KompetensiDasar> lst_kd =
            //        DAO_Rapor_KompetensiDasar.GetAll_Entity();

            //    List<Rapor_KomponenPenilaian> lst_kp =
            //        DAO_Rapor_KomponenPenilaian.GetAll_Entity();

            //    List<FormasiGuruKelas> lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
            //            GetUnitSekolah().Kode.ToString(), tahun_ajaran, semester
            //        ).FindAll(m => m.Rel_KelasDet.ToString().ToUpper() == rel_kelas_det.Trim().ToUpper());

            //    List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByTABySMByJenisRapor_Entity(tahun_ajaran, semester, DAO_Rapor_Desain.JenisRapor.LTS);

            //    List<SiswaAbsenLTS> lst_siswa_absen_lts = new List<SiswaAbsenLTS>();
            //    lst_siswa_absen_lts = DAO_SiswaAbsenLTS.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det);

            //    List<Rapor_Desain> lst_rapor_desain = DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran, semester);
            //    List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(GetUnitSekolah().Kode.ToString());

            //    List<DAO_Siswa.SiswaByFormasiMapel> lst_siswa_lintas_minat = DAO_FormasiGuruMapelDetSiswa.GetSiswaByTABySMBy_Entity(
            //                            tahun_ajaran,
            //                            semester
            //                        );

            //    List<DAO_Siswa.SiswaByFormasiMapel> lst_siswa_non_lintas_minat = DAO_FormasiGuruMapel.GetSiswaByTABySMByKelas_Entity(
            //                            tahun_ajaran,
            //                            semester,
            //                            (
            //                                m_kelas_det != null
            //                                ? (
            //                                    m_kelas_det.Nama != null
            //                                    ? m_kelas_det.Rel_Kelas.ToString()
            //                                    : ""
            //                                  )
            //                                : ""
            //                            )
            //                        );

            //    List<Rapor_Nilai> lst_nilai_rapor = DAO_Rapor_Nilai.GetAllByTABySMByKelasByKurikulum_Entity(
            //            tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString(), Libs.JenisKurikulum.SMA.KURTILAS
            //        );

            //    List<Rapor_StrukturNilai_Deskripsi> lst_deskripsi = DAO_Rapor_StrukturNilai_Deskripsi.GetAllByTABySMByKelas_Entity(
            //                tahun_ajaran,
            //                semester,
            //                m_kelas_det.Rel_Kelas.ToString()
            //            );

            //    Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
            //        m0 => m0.TahunAjaran == tahun_ajaran &&
            //              m0.Semester == semester &&
            //              m0.JenisRapor == DAO_Rapor_Desain.JenisRapor.LTS
            //    ).FirstOrDefault();

            //    string s_html = "";
            //    string s_html_deskripsi = "";
            //    string s_tanggal_rapor = "";

            //    List<DAO_SiswaAbsen.AbsenRekapRaporSiswa> lst_absen_ = new List<DAO_SiswaAbsen.AbsenRekapRaporSiswa>();
            //    if (m_rapor_arsip != null)
            //    {
            //        if (m_rapor_arsip.TahunAjaran != null)
            //        {
            //            s_tanggal_rapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);

            //            lst_absen_ = DAO_SiswaAbsen.GetRekapAbsenRaporByPeriode_Entity(
            //                    (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
            //                    (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
            //                );
            //        }
            //    }

            //    if (m_kelas_det != null)
            //    {
            //        if (m_kelas_det.Nama != null)
            //        {
            //            List<Rapor_StrukturNilai_KURTILAS> lst_sn_kurtilas = DAO_Rapor_StrukturNilai_KURTILAS.GetAllByTABySMByKelas_Entity(
            //                            tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString()
            //                        );

            //            List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
            //                GetUnitSekolah().Kode.ToString(), rel_kelas_det, tahun_ajaran, semester);
            //            //if (rel_siswa.Trim() != "")
            //            //{

            //            //}

            //            //rekap absensi walas
            //            List<SiswaAbsenRekap> lst_absen_rekap = DAO_SiswaAbsenRekap.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FindAll(m0 => m0.Rel_Mapel.Trim() == "" && m0.Jenis.ToString().ToUpper() == TipeRapor.LTS.ToUpper().Trim());
            //            List<SiswaAbsenRekapDet> lst_rekap_det = new List<SiswaAbsenRekapDet>();
            //            if (lst_absen_rekap.Count == 1)
            //            {
            //                SiswaAbsenRekap m_rekap_absensi = lst_absen_rekap.FirstOrDefault();
            //                if (m_rekap_absensi != null)
            //                {
            //                    if (m_rekap_absensi.TahunAjaran != null)
            //                    {
            //                        lst_rekap_det = DAO_SiswaAbsenRekapDet.GetAllByHeader_Entity(m_rekap_absensi.Kode.ToString());
            //                    }
            //                }
            //            }
            //            //end rekap absensi walas

            //            lst_siswa = lst_siswa.FindAll(m0 => (rel_siswa + ";").Trim().ToUpper().IndexOf(m0.Kode.ToString().ToUpper() + ";") >= 0).ToList();
            //            foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa)
            //            {
            //                List<DAO_FormasiEkskulDet.AbsenEkskulLTS> lst_ekskul_det_ = DAO_FormasiEkskulDet.GetLTSEkskulAbsenByTABySMBySiswa_Entity(
            //                    tahun_ajaran, semester, m_siswa.Kode.ToString()
            //                );

            //                //ekskul LTS
            //                int id_ekskul = 1;
            //                List<DAO_FormasiEkskulDet.AbsenEkskulLTS> lst_ekskul_det = lst_ekskul_det_.FindAll(
            //                       m0 => m0.TahunAjaran == tahun_ajaran &&
            //                             m0.Semester == semester &&
            //                             m0.Rel_Siswa == m_siswa.Kode.ToString()
            //                   );
            //                //end ekskul LTS

            //                s_html += "<h4 style=\"margin: 0 auto; display: table; margin-top: 20px;\">DAFTAR NILAI (RAPOR) TENGAH SEMESTER - K13</h4>" +
            //                          "<h4 style=\"margin: 0 auto; display: table;\">SMA ISLAM AL-IZHAR PONDOK LABU</h4>" +
            //                          "<hr />";

            //                s_html += "<table style=\"border-style: none; width: 100%; font-size: small;\">" +
            //                            "<tr>" +
            //                                "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">NIS</td>" +
            //                                "<td style=\"text-align: center; padding: 5px; padding-top: 3px; padding-bottom: 3px;\">:</td>" +
            //                                "<td colspan=\"4\" style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">" + m_siswa.NISSekolah + "</td>" +
            //                            "</tr>" +
            //                            "<tr>" +
            //                                "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">Nama Siswa</td>" +
            //                                "<td style=\"text-align: center; padding: 5px; padding-top: 3px; padding-bottom: 3px;\">:</td>" +
            //                                "<td style=\"font-weight: bold; padding: 5px; padding-top: 3px; padding-bottom: 3px; width: 50%;\">" + m_siswa.Nama.Trim().ToUpper() + "</td>" +
            //                                "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">Semester</td>" +
            //                                "<td style=\"text-align: center; padding: 5px; padding-top: 3px; padding-bottom: 3px;\">:</td>" +
            //                                "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">" +
            //                                    (semester == "1" ? "1 (satu)" : "2 (dua)") +
            //                                "</td>" +
            //                            "</tr>" +
            //                            "<tr>" +
            //                                "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">Kelas</td>" +
            //                                "<td style=\"text-align: center; padding: 5px; padding-top: 3px; padding-bottom: 3px;\">:</td>" +
            //                                "<td style=\"font-weight: bold; padding: 5px; padding-top: 3px; padding-bottom: 3px;\">" + m_kelas_det.Nama + "</td>" +
            //                                "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">Tahun Pelajaran</td>" +
            //                                "<td style=\"text-align: center; padding: 5px; padding-top: 3px; padding-bottom: 3px;\">:</td>" +
            //                                "<td style=\"padding: 5px; padding-top: 3px; padding-bottom: 3px;\">" +
            //                                    tahun_ajaran +
            //                                "</td>" +
            //                            "</tr>" +
            //                          "</table>";

            //                s_html += "<table style=\"border-style: solid; border-width: 1px; font-size: small; width: 100%; margin-top: 15px;\">" +
            //                            "<tr>" +
            //                                "<td rowspan=\"2\" style=\"width: 40px; vertical-align: middle; padding: 3px; border-style: solid; border-width: 0.5px;\">No.</td>" +
            //                                "<td rowspan=\"2\" style=\"vertical-align: middle; padding: 3px; border-style: solid; border-width: 0.5px;\">Mata Pelajaran</td>" +
            //                                "<td rowspan=\"2\" style=\"vertical-align: middle; padding: 3px; border-style: solid; border-width: 0.5px;\">KKM</td>" +
            //                                "<td colspan=\"12\" style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">PR/Latihan/Tugas/Kuis/Ulangan Harian/Lain-lain *)</td>" +
            //                                "<td colspan=\"4\" style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">Perilaku Belajar</td>" +
            //                            "</tr>" +
            //                            "<tr>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T1</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T2</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T3</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T4</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T5</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T6</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T7</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T8</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T9</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T10</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T11</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">T12</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">HD</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">LK</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">RJ</td>" +
            //                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px;\">RPKB</td>" +
            //                            "</tr>";

            //                string nama_kelas = m_kelas_det.Nama + "-";
            //                string rel_kelas_jurusan = m_siswa.Rel_KelasDetJurusan;
            //                string rel_kelas_sosialisasi = m_siswa.Rel_KelasDetSosialisasi;
            //                if (rel_kelas_jurusan.Trim() != "")
            //                {
            //                    KelasDet m_kelas_jurusan = DAO_KelasDet.GetByID_Entity(rel_kelas_jurusan);
            //                    if (m_kelas_jurusan != null)
            //                    {
            //                        if (m_kelas_jurusan.Nama != null)
            //                        {
            //                            nama_kelas = m_kelas_jurusan.Nama + "-";
            //                        }
            //                    }
            //                }

            //                string[] arr_nama_kelas = nama_kelas.Split(new string[] { "-" }, StringSplitOptions.None);
            //                string nama_kelas_ok = "";
            //                int id_str = 0;

            //                foreach (string item_nama_kelas in arr_nama_kelas)
            //                {
            //                    if (id_str == 2)
            //                    {
            //                        break;
            //                    }
            //                    nama_kelas_ok += (nama_kelas_ok.Trim() != "" ? "-" : "") + item_nama_kelas;
            //                    id_str++;
            //                }

            //                Rapor_Desain m_rapor_desain = lst_rapor_desain.
            //                    FindAll(m0 => m0.Rel_Kelas.Trim().ToUpper() == nama_kelas_ok.Trim().ToUpper() && m0.JenisRapor == DAO_Rapor_Desain.JenisRapor.LTS).FirstOrDefault();

            //                bool ada_absen = false;
            //                string s_sakit = "0";
            //                string s_izin = "0";
            //                string s_alpa = "0";
            //                ada_absen = (lst_siswa_absen_lts.FindAll(m0 => m0.Sakit > 0 || m0.Izin > 0 || m0.Alpa > 0).Count > 0 ? true : false);
            //                SiswaAbsenLTS m_absen_lts = lst_siswa_absen_lts.FindAll(ms => ms.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()).FirstOrDefault();
            //                if (m_absen_lts != null)
            //                {
            //                    if (m_absen_lts.TahunAjaran != null)
            //                    {
            //                        s_sakit = m_absen_lts.Sakit.ToString();
            //                        s_izin = m_absen_lts.Izin.ToString();
            //                        s_alpa = m_absen_lts.Alpa.ToString();
            //                    }
            //                }

            //                if (!ada_absen)
            //                {
            //                    if (m_rapor_arsip != null)
            //                    {
            //                        if (m_rapor_arsip.TahunAjaran != null)
            //                        {
            //                            List<DAO_SiswaAbsen.AbsenRekapRaporSiswa> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRaporSiswa>();
            //                            lst_absen = lst_absen_.FindAll(m0 => m0.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper());
            //                            foreach (var absen in lst_absen)
            //                            {
            //                                if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
            //                                if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
            //                                if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
            //                            }
            //                            if (tahun_ajaran == "2020/2021")
            //                            {
            //                                if (lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper() == rel_siswa.ToUpper().Trim()).Count > 0)
            //                                {
            //                                    SiswaAbsenRekapDet m_rekap_absen_siswa = lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper() == rel_siswa.ToUpper().Trim()).FirstOrDefault();
            //                                    if (m_rekap_absen_siswa != null)
            //                                    {
            //                                        if (m_rekap_absen_siswa.Rel_Siswa != null)
            //                                        {
            //                                            s_sakit = m_rekap_absen_siswa.Sakit;
            //                                            s_izin = m_rekap_absen_siswa.Izin;
            //                                            s_alpa = m_rekap_absen_siswa.Alpa;
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }

            //                if (m_rapor_desain != null)
            //                {
            //                    if (m_rapor_desain.TahunAjaran != null)
            //                    {
            //                        string poin = "";
            //                        List<Rapor_Desain_Det> lst_rapor_desain_det_ = lst_rapor_desain_det.FindAll(
            //                                m0 => m0.Rel_Rapor_Desain.ToString().ToUpper().Trim() == m_rapor_desain.Kode.ToString().ToUpper().Trim()
            //                            );

            //                        string s_walikelas = "";
            //                        if (lst_formasi_guru_kelas != null)
            //                        {
            //                            if (lst_formasi_guru_kelas.Count > 0)
            //                            {
            //                                FormasiGuruKelas m_guru_kelas = lst_formasi_guru_kelas.FirstOrDefault();
            //                                if (m_guru_kelas != null)
            //                                {
            //                                    if (m_guru_kelas.TahunAjaran != null)
            //                                    {
            //                                        Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m_guru_kelas.Rel_GuruKelas);
            //                                        if (m_pegawai != null)
            //                                        {
            //                                            if (m_pegawai.Nama != null)
            //                                            {
            //                                                s_walikelas = m_pegawai.Nama;
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }

            //                        int id_lintas_minat = 0;
            //                        foreach (Rapor_Desain_Det item_rapor_desain_det in lst_rapor_desain_det_)
            //                        {
            //                            List<NilaiLTSMapel> lst_sn_lts = new List<NilaiLTSMapel>();
            //                            lst_sn_lts.Clear();

            //                            //list siswa
            //                            List<DAO_Siswa.SiswaByFormasiMapel> lst_siswa_mapel = new List<DAO_Siswa.SiswaByFormasiMapel>();
            //                            lst_siswa_mapel.Clear();
            //                            if (item_rapor_desain_det.JenisMapel == Libs.JENIS_MAPEL.LINTAS_MINAT)
            //                            {
            //                                lst_siswa_mapel = lst_siswa_lintas_minat.FindAll(
            //                                        m0 => (
            //                                                m0.Rel_KelasDet.ToString().Trim().ToUpper() == m_kelas_det.Kode.ToString().Trim().ToUpper() &&
            //                                                m0.JenisKelas == "WAL"
            //                                              ) ||
            //                                              (
            //                                                m0.Rel_KelasDetJurusan.ToString().Trim().ToUpper() == m_siswa.Rel_KelasDetJurusan.ToString().Trim().ToUpper() &&
            //                                                m0.JenisKelas == "JUR"
            //                                              ) ||
            //                                              (
            //                                                m0.Rel_KelasDetSosialisasi.ToString().Trim().ToUpper() == m_siswa.Rel_KelasDetSosialisasi.ToString().Trim().ToUpper() &&
            //                                                m0.JenisKelas == "SOS"
            //                                              ) &&
            //                                              m0.Rel_Kelas.ToString().Trim().ToUpper() == m_kelas_det.Rel_Kelas.ToString().Trim().ToUpper() &&
            //                                              m0.Rel_Mapel.ToString().Trim().ToUpper() == item_rapor_desain_det.Rel_Mapel.ToString().Trim().ToUpper()
            //                                    );
            //                            }
            //                            else
            //                            {
            //                                lst_siswa_mapel = lst_siswa_non_lintas_minat.FindAll(
            //                                        m0 => (
            //                                                    (
            //                                                        m0.Rel_KelasDet.ToString().Trim().ToUpper() == m_kelas_det.Kode.ToString().Trim().ToUpper() &&
            //                                                        m0.JenisKelas == "WAL"
            //                                                    ) ||
            //                                                    (
            //                                                        m0.Rel_KelasDetJurusan.ToString().Trim().ToUpper() == m_siswa.Rel_KelasDetJurusan.ToString().Trim().ToUpper() &&
            //                                                        m0.JenisKelas == "JUR"
            //                                                    ) ||
            //                                                    (
            //                                                        m0.Rel_KelasDetSosialisasi.ToString().Trim().ToUpper() == m_siswa.Rel_KelasDetSosialisasi.ToString().Trim().ToUpper() &&
            //                                                        m0.JenisKelas == "SOS"
            //                                                    )
            //                                              )
            //                                              &&
            //                                              m0.Kode.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
            //                                              m0.Rel_Mapel.ToString().Trim().ToUpper() == item_rapor_desain_det.Rel_Mapel.ToString().Trim().ToUpper()
            //                                    );
            //                            }

            //                            DAO_Siswa.SiswaByFormasiMapel m_formasi_kelas = (
            //                                    lst_siswa_mapel != null
            //                                    ? lst_siswa_mapel.FirstOrDefault()
            //                                    : null
            //                                );
            //                            string s_kelas_formasi = "";
            //                            if (m_formasi_kelas != null)
            //                            {
            //                                if (m_formasi_kelas.JenisKelas == "WAL")
            //                                {
            //                                    s_kelas_formasi = m_formasi_kelas.Rel_KelasDet;
            //                                }
            //                                else if (m_formasi_kelas.JenisKelas == "JUR")
            //                                {
            //                                    s_kelas_formasi = m_formasi_kelas.Rel_KelasDetJurusan;
            //                                }
            //                                else if (m_formasi_kelas.JenisKelas == "SOS")
            //                                {
            //                                    s_kelas_formasi = m_formasi_kelas.Rel_KelasDetSosialisasi;
            //                                }
            //                            }

            //                            List<DAO_SiswaAbsenMapel.AbsenMapel> lst_absen_mapel = new List<DAO_SiswaAbsenMapel.AbsenMapel>();
            //                            if (s_kelas_formasi.Trim() != "")
            //                            {
            //                                lst_absen_mapel = DAO_SiswaAbsenMapel.GetAllByTABySMByPeriodeByKelas_Entity(
            //                                    tahun_ajaran, semester,
            //                                    (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
            //                                    (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue),
            //                                    s_kelas_formasi
            //                                );
            //                            }

            //                            if (item_rapor_desain_det.Poin.Trim() != "")
            //                            {
            //                                poin = item_rapor_desain_det.Poin.Trim();
            //                            }

            //                            int id_tagihan = 0;

            //                            RaporLTS m = new RaporLTS();
            //                            RaporLTSDeskripsi m_des = new RaporLTSDeskripsi();

            //                            List<NilaiLTS> lst_nilai_lts = new List<NilaiLTS>();
            //                            lst_nilai_lts.Clear();

            //                            Rapor_NilaiSiswa_KURTILAS_Det_Lengkap m_nilai_siswa = new Rapor_NilaiSiswa_KURTILAS_Det_Lengkap();
            //                            Rapor_NilaiSiswa_KURTILAS_Det_Lengkap m_nilai_siswa_ok = new Rapor_NilaiSiswa_KURTILAS_Det_Lengkap();

            //                            //nilai LTS
            //                            m.TahunAjaran = tahun_ajaran;
            //                            m.Semester = semester;
            //                            m.Kelas = m_kelas_det.Nama;
            //                            m.Rel_Siswa = m_siswa.Kode.ToString();
            //                            m.NIS = m_siswa.NISSekolah;
            //                            m.Nama = m_siswa.Nama.Trim().ToUpper();
            //                            m.KodeKelompokMapel = poin;
            //                            m.KelompokMapel = item_rapor_desain_det.NamaMapelRapor;
            //                            m.Rel_Mapel = item_rapor_desain_det.Rel_Mapel;
            //                            m.NamaMapel = item_rapor_desain_det.NamaMapelRapor;
            //                            m.KKM = "";
            //                            m.TTDGuru = img_ttd_guru;

            //                            //deskripsi LTS
            //                            m_des.Rel_Siswa = m_siswa.Kode.ToString();
            //                            m_des.Nama = m_siswa.Nama.Trim().ToUpper();
            //                            m_des.TahunAjaran = tahun_ajaran;
            //                            m_des.Semester = semester;
            //                            m_des.KodeKelompokMapel = poin;
            //                            m_des.KelompokMapel = item_rapor_desain_det.NamaMapelRapor;
            //                            m_des.Rel_Mapel = item_rapor_desain_det.Rel_Mapel;
            //                            m_des.NamaMapel = item_rapor_desain_det.NamaMapelRapor;

            //                            List<NilaiLTSMapel> lst_lts_mapel = new List<NilaiLTSMapel>();
            //                            lst_lts_mapel.Clear();

            //                            if (item_rapor_desain_det.Rel_Mapel.Trim() != "")
            //                            {
            //                                Rapor_StrukturNilai_KURTILAS m_sn_kurtilas = lst_sn_kurtilas.FindAll(
            //                                        m0 => m0.TahunAjaran == tahun_ajaran &&
            //                                              m0.Semester == semester &&
            //                                              m0.Rel_Mapel.ToString().Trim().ToUpper() == item_rapor_desain_det.Rel_Mapel.Trim().Trim().ToUpper()
            //                                    ).FirstOrDefault();

            //                                if (m_sn_kurtilas != null)
            //                                {
            //                                    if (m_sn_kurtilas.TahunAjaran != null)
            //                                    {
            //                                        id_tagihan = 0;
            //                                        List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian_ = lst_aspek_penilaian.FindAll(
            //                                                    m0 => m0.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() == m_sn_kurtilas.Kode.ToString().Trim().ToUpper()
            //                                                );
            //                                        //load kurtilas ap
            //                                        foreach (Rapor_StrukturNilai_KURTILAS_AP m_sn_ap in lst_aspek_penilaian_)
            //                                        {
            //                                            if (m_sn_ap != null)
            //                                            {
            //                                                if (m_sn_ap.JenisPerhitungan != null)
            //                                                {
            //                                                    Rapor_AspekPenilaian m_ap =
            //                                                        lst_ap.FindAll(
            //                                                            m0 => m0.Kode.ToString().Trim().ToUpper() == m_sn_ap.Rel_Rapor_AspekPenilaian.ToString().Trim().ToUpper()
            //                                                        ).FirstOrDefault();

            //                                                    if (m_ap != null)
            //                                                    {
            //                                                        if (m_ap.Nama != null)
            //                                                        {
            //                                                            //load kurtilas kd
            //                                                            int id_kd = 1;
            //                                                            List<Rapor_StrukturNilai_KURTILAS_KD> lst_kompetensi_dasar_ =
            //                                                                lst_kompetensi_dasar.FindAll(m0 => m0.Rel_Rapor_StrukturNilai_AP.ToString().Trim().ToUpper() == m_sn_ap.Kode.ToString().Trim().ToUpper());
            //                                                            foreach (Rapor_StrukturNilai_KURTILAS_KD m_sn_kd in lst_kompetensi_dasar_)
            //                                                            {
            //                                                                if (m_sn_kd != null)
            //                                                                {
            //                                                                    if (m_sn_kd.JenisPerhitungan != null)
            //                                                                    {
            //                                                                        Rapor_KompetensiDasar m_kd =
            //                                                                            lst_kd.FindAll(
            //                                                                                m0 => m0.Kode.ToString().Trim().ToUpper() == m_sn_kd.Rel_Rapor_KompetensiDasar.ToString().Trim().ToUpper()
            //                                                                            ).FirstOrDefault();

            //                                                                        if (m_kd != null)
            //                                                                        {
            //                                                                            if (m_kd.Nama != null)
            //                                                                            {

            //                                                                                //load kurtilas kp
            //                                                                                List<Rapor_StrukturNilai_KURTILAS_KP> lst_komponen_penilaian_ =
            //                                                                                    lst_komponen_penilaian.FindAll(m0 => m0.Rel_Rapor_StrukturNilai_KD.ToString().Trim().ToUpper() == m_sn_kd.Kode.ToString().Trim().ToUpper());
            //                                                                                foreach (Rapor_StrukturNilai_KURTILAS_KP m_sn_kp in lst_komponen_penilaian_)
            //                                                                                {
            //                                                                                    Rapor_KomponenPenilaian m_kp =
            //                                                                                        lst_kp.FindAll(
            //                                                                                            m0 => m0.Kode.ToString().Trim().ToUpper() == m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString().Trim().ToUpper()
            //                                                                                        ).FirstOrDefault();

            //                                                                                    if (m_kp != null)
            //                                                                                    {
            //                                                                                        if (m_kp.Nama != null)
            //                                                                                        {
            //                                                                                            if (s_kelas_formasi.Trim() != "")
            //                                                                                            {
            //                                                                                                if (lst_nilai_det.FindAll(
            //                                                                                                    m0 => m0.Rel_KelasDet.ToString().ToUpper().Trim() == s_kelas_formasi.ToString().ToUpper().Trim() &&
            //                                                                                                          m0.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() == m_sn_ap.Kode.ToString().ToUpper() &&
            //                                                                                                          m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() == m_sn_kd.Kode.ToString().ToUpper() &&
            //                                                                                                          m0.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().ToUpper() == m_sn_kp.Kode.ToString().ToUpper() &&
            //                                                                                                          m0.Nilai.Trim() != ""
            //                                                                                                ).Count > 0)
            //                                                                                                {
            //                                                                                                    id_tagihan++;

            //                                                                                                    lst_lts_mapel.Add(new NilaiLTSMapel
            //                                                                                                    {
            //                                                                                                        Rel_Rapor_StrukturNilai = m_sn_kurtilas.Kode.ToString().ToUpper(),
            //                                                                                                        Rel_Rapor_StrukturNilai_KURTILAS_AP = m_sn_ap.Kode.ToString().ToUpper(),
            //                                                                                                        Rel_Rapor_StrukturNilai_KURTILAS_KD = m_sn_kd.Kode.ToString().ToUpper(),
            //                                                                                                        Rel_Rapor_StrukturNilai_KURTILAS_KP = m_sn_kp.Kode.ToString().ToUpper(),
            //                                                                                                        DeskripsiLTS = "",
            //                                                                                                        UrutanTagihan = id_tagihan
            //                                                                                                    });
            //                                                                                                }
            //                                                                                            }
            //                                                                                            else
            //                                                                                            {
            //                                                                                                if (lst_nilai_det.FindAll(
            //                                                                                                    m0 => m0.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() == m_sn_ap.Kode.ToString().ToUpper() &&
            //                                                                                                          m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() == m_sn_kd.Kode.ToString().ToUpper() &&
            //                                                                                                          m0.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().ToUpper() == m_sn_kp.Kode.ToString().ToUpper() &&
            //                                                                                                          m0.Nilai.Trim() != ""
            //                                                                                                ).Count > 0)
            //                                                                                                {
            //                                                                                                    id_tagihan++;

            //                                                                                                    lst_lts_mapel.Add(new NilaiLTSMapel
            //                                                                                                    {
            //                                                                                                        Rel_Rapor_StrukturNilai = m_sn_kurtilas.Kode.ToString().ToUpper(),
            //                                                                                                        Rel_Rapor_StrukturNilai_KURTILAS_AP = m_sn_ap.Kode.ToString().ToUpper(),
            //                                                                                                        Rel_Rapor_StrukturNilai_KURTILAS_KD = m_sn_kd.Kode.ToString().ToUpper(),
            //                                                                                                        Rel_Rapor_StrukturNilai_KURTILAS_KP = m_sn_kp.Kode.ToString().ToUpper(),
            //                                                                                                        DeskripsiLTS = "",
            //                                                                                                        UrutanTagihan = id_tagihan
            //                                                                                                    });
            //                                                                                                }
            //                                                                                            }
            //                                                                                        }
            //                                                                                    }
            //                                                                                }

            //                                                                            }
            //                                                                        }

            //                                                                        id_kd++;
            //                                                                    }
            //                                                                }
            //                                                            }
            //                                                            //end load kurtilas kd

            //                                                        }
            //                                                    }

            //                                                }
            //                                            }
            //                                        }
            //                                        //end load kurtilas ap

            //                                        m.KKM = Math.Round(m_sn_kurtilas.KKM).ToString();

            //                                    }
            //                                }
            //                            }

            //                            //nilai LTS
            //                            m.T1 = "";
            //                            m.T2 = "";
            //                            m.T3 = "";
            //                            m.T4 = "";
            //                            m.T5 = "";
            //                            m.T6 = "";
            //                            m.T7 = "";
            //                            m.T8 = "";
            //                            m.T9 = "";
            //                            m.T10 = "";
            //                            m.T11 = "";
            //                            m.T12 = "";
            //                            m.T13 = "";
            //                            m.T14 = "";
            //                            m.T15 = "";

            //                            m.T1_Deskripsi = "";
            //                            m.T2_Deskripsi = "";
            //                            m.T3_Deskripsi = "";
            //                            m.T4_Deskripsi = "";
            //                            m.T5_Deskripsi = "";
            //                            m.T6_Deskripsi = "";
            //                            m.T7_Deskripsi = "";
            //                            m.T8_Deskripsi = "";
            //                            m.T9_Deskripsi = "";
            //                            m.T10_Deskripsi = "";
            //                            m.T11_Deskripsi = "";
            //                            m.T12_Deskripsi = "";
            //                            m.T13_Deskripsi = "";
            //                            m.T14_Deskripsi = "";
            //                            m.T15_Deskripsi = "";

            //                            m.HD = "";
            //                            m.LK = "";
            //                            m.RJ = "";
            //                            m.RPKB = "";
            //                            m.NamaEkskul1 = "";
            //                            m.HadirEkskul1 = "";
            //                            m.NamaEkskul2 = "";
            //                            m.HadirEkskul2 = "";
            //                            m.NamaEkskul3 = "";
            //                            m.HadirEkskul3 = "";
            //                            m.Sakit = s_sakit;
            //                            m.Izin = s_izin;
            //                            m.Alpa = s_alpa;
            //                            m.TanggalRapor = s_tanggal_rapor;

            //                            id_ekskul = 1;
            //                            foreach (DAO_FormasiEkskulDet.AbsenEkskulLTS item_ekskul_det in lst_ekskul_det)
            //                            {
            //                                if (id_ekskul == 1)
            //                                {
            //                                    m.NamaEkskul1 = item_ekskul_det.Mapel;
            //                                    m.HadirEkskul1 = (item_ekskul_det.LTS_HD.Trim() == "" ? "0" : item_ekskul_det.LTS_HD);
            //                                }
            //                                else if (id_ekskul == 2)
            //                                {
            //                                    m.NamaEkskul2 = item_ekskul_det.Mapel;
            //                                    m.HadirEkskul2 = (item_ekskul_det.LTS_HD.Trim() == "" ? "0" : item_ekskul_det.LTS_HD);
            //                                }
            //                                else if (id_ekskul == 3)
            //                                {
            //                                    m.NamaEkskul3 = item_ekskul_det.Mapel;
            //                                    m.HadirEkskul3 = (item_ekskul_det.LTS_HD.Trim() == "" ? "0" : item_ekskul_det.LTS_HD);
            //                                }
            //                                id_ekskul++;
            //                            }

            //                            //deskripsi LTS
            //                            m_des.PoinTCol1_1 = "";
            //                            m_des.PoinTCol1_2 = "";
            //                            m_des.PoinTCol1_3 = "";
            //                            m_des.PoinTCol1_4 = "";
            //                            m_des.PoinTCol1_5 = "";
            //                            m_des.PoinTCol1_6 = "";
            //                            m_des.DesPoinTCol1_1 = "";
            //                            m_des.DesPoinTCol1_2 = "";
            //                            m_des.DesPoinTCol1_3 = "";
            //                            m_des.DesPoinTCol1_4 = "";
            //                            m_des.DesPoinTCol1_5 = "";
            //                            m_des.DesPoinTCol1_6 = "";
            //                            m_des.PoinTCol2_1 = "";
            //                            m_des.PoinTCol2_2 = "";
            //                            m_des.PoinTCol2_3 = "";
            //                            m_des.PoinTCol2_4 = "";
            //                            m_des.PoinTCol2_5 = "";
            //                            m_des.PoinTCol2_6 = "";
            //                            m_des.DesPoinTCol2_1 = "";
            //                            m_des.DesPoinTCol2_2 = "";
            //                            m_des.DesPoinTCol2_3 = "";
            //                            m_des.DesPoinTCol2_4 = "";
            //                            m_des.DesPoinTCol2_5 = "";
            //                            m_des.DesPoinTCol2_6 = "";

            //                            lst_nilai_lts.Clear();
            //                            m_nilai_siswa = new Rapor_NilaiSiswa_KURTILAS_Det_Lengkap();

            //                            bool ada_nilai = false;
            //                            foreach (NilaiLTSMapel item_lts_mapel in lst_lts_mapel)
            //                            {
            //                                m_nilai_siswa = lst_nilai_det.FindAll(
            //                                        m0 => m0.Rel_Siswa.ToString().ToUpper() == m_siswa.Kode.ToString().ToUpper() &&
            //                                              m0.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().ToUpper() &&
            //                                              m0.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().ToUpper() &&
            //                                              m0.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().ToUpper()
            //                                    ).FirstOrDefault();

            //                                string s_deksripsi_lts = "";
            //                                if (m_nilai_siswa == null)
            //                                {
            //                                    m_nilai_siswa = new Rapor_NilaiSiswa_KURTILAS_Det_Lengkap();
            //                                    //m_nilai_siswa.Nilai = "BL";
            //                                    m_nilai_siswa.Nilai = "";

            //                                    Rapor_StrukturNilai_Deskripsi m_sn_deskripsi = lst_deskripsi.FindAll(
            //                                            m0 => m0.Rel_KelasDet.ToString().Trim().ToUpper() == s_kelas_formasi.ToString().Trim().ToUpper() &&
            //                                                  m0.Rel_StrukturNilai.Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() &&
            //                                                  m0.Rel_StrukturNilai_AP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().Trim().ToUpper() &&
            //                                                  m0.Rel_StrukturNilai_KD.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().Trim().ToUpper() &&
            //                                                  m0.Rel_StrukturNilai_KP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().Trim().ToUpper()
            //                                        ).FirstOrDefault();

            //                                    if (m_sn_deskripsi != null)
            //                                    {
            //                                        if (m_sn_deskripsi.TahunAjaran != null)
            //                                        {
            //                                            s_deksripsi_lts = m_sn_deskripsi.Deskripsi;
            //                                        }
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    if (m_nilai_siswa.Rel_Siswa != null)
            //                                    {
            //                                        Rapor_StrukturNilai_Deskripsi m_sn_deskripsi = lst_deskripsi.FindAll(
            //                                            m0 => m0.Rel_KelasDet.ToString().Trim().ToUpper() == m_nilai_siswa.Rel_KelasDet.ToString().Trim().ToUpper() &&
            //                                                  m0.Rel_StrukturNilai.Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() &&
            //                                                  m0.Rel_StrukturNilai_AP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().Trim().ToUpper() &&
            //                                                  m0.Rel_StrukturNilai_KD.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().Trim().ToUpper() &&
            //                                                  m0.Rel_StrukturNilai_KP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().Trim().ToUpper()
            //                                        ).FirstOrDefault();

            //                                        if (m_sn_deskripsi == null)
            //                                        {
            //                                            m_sn_deskripsi = lst_deskripsi.FindAll(
            //                                                m0 => m0.Rel_KelasDet.ToString().Trim().ToUpper() == rel_kelas_det.ToString().Trim().ToUpper() &&
            //                                                      m0.Rel_StrukturNilai.Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai.ToString().Trim().ToUpper() &&
            //                                                      m0.Rel_StrukturNilai_AP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_AP.ToString().Trim().ToUpper() &&
            //                                                      m0.Rel_StrukturNilai_KD.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KD.ToString().Trim().ToUpper() &&
            //                                                      m0.Rel_StrukturNilai_KP.ToString().Trim().ToUpper() == item_lts_mapel.Rel_Rapor_StrukturNilai_KURTILAS_KP.ToString().Trim().ToUpper()
            //                                            ).FirstOrDefault();
            //                                        }

            //                                        if (m_sn_deskripsi != null)
            //                                        {
            //                                            if (m_sn_deskripsi.TahunAjaran != null)
            //                                            {
            //                                                s_deksripsi_lts = m_sn_deskripsi.Deskripsi;
            //                                            }
            //                                        }

            //                                        m_nilai_siswa_ok = m_nilai_siswa;
            //                                        if (m_nilai_siswa.Nilai.Trim() != "") ada_nilai = true;
            //                                    }
            //                                }

            //                                lst_nilai_lts.Add(new NilaiLTS
            //                                {
            //                                    Nilai = m_nilai_siswa.Nilai,
            //                                    DeskripsiLTS = s_deksripsi_lts,
            //                                    UrutanTagihan = item_lts_mapel.UrutanTagihan
            //                                });
            //                            }

            //                            if (m_nilai_siswa_ok != null)
            //                            {
            //                                if (m_nilai_siswa_ok.Rel_Siswa != null)
            //                                {
            //                                    Rapor_NilaiSiswa_KURTILAS m_nilai = DAO_Rapor_NilaiSiswa_KURTILAS.GetByID_Entity(m_nilai_siswa_ok.Rel_Rapor_NilaiSiswa_KURTILAS.ToString());
            //                                    if (m_nilai != null)
            //                                    {
            //                                        if (m_nilai.Rel_Siswa != null)
            //                                        {
            //                                            //absen
            //                                            string s_jumlah_hadir = "0";
            //                                            string s_jumlah_hadir_max = "0";
            //                                            DAO_SiswaAbsenMapel.AbsenMapel m_absen_mapel = lst_absen_mapel.FindAll(
            //                                                    m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == item_rapor_desain_det.Rel_Mapel.ToString().ToUpper().Trim() &&
            //                                                          m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
            //                                                ).FirstOrDefault();
            //                                            if (m_absen_mapel != null)
            //                                            {
            //                                                if (m_absen_mapel.Rel_Siswa != null)
            //                                                {
            //                                                    s_jumlah_hadir = m_absen_mapel.JumlahHadir;
            //                                                    s_jumlah_hadir_max = m_absen_mapel.JumlahHadirMax;
            //                                                }
            //                                            }

            //                                            if (
            //                                                    (m_nilai.LTS_HD.Trim() != "" && m_nilai.LTS_MAX_HD.Trim() != "") 
            //                                                    //||
            //                                                    //tahun_ajaran == "2020/2021"
            //                                               )
            //                                            {
            //                                                m.HD = (m_nilai.LTS_HD.Trim() == "" ? "-" : m_nilai.LTS_HD.Trim()) +
            //                                                       "/" +
            //                                                       (m_nilai.LTS_MAX_HD.Trim() == "" ? "-" : m_nilai.LTS_MAX_HD.Trim());
            //                                            }
            //                                            else
            //                                            {
            //                                                m.HD = s_jumlah_hadir +
            //                                                       "/" +
            //                                                       s_jumlah_hadir_max;
            //                                            }
            //                                            m.LK = m_nilai.LTS_LK;
            //                                            m.RJ = m_nilai.LTS_RJ;
            //                                            m.RPKB = m_nilai.LTS_RPKB;
            //                                        }
            //                                    }
            //                                }
            //                            }

            //                            id_tagihan = 0;
            //                            if (lst_nilai_lts.Count == 1)
            //                            {
            //                                id_tagihan++;
            //                                m.T1 = lst_nilai_lts[0].Nilai.ToUpper().Trim();
            //                                m.T1_Deskripsi = lst_nilai_lts[0].DeskripsiLTS;
            //                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_1 = lst_nilai_lts[0].DeskripsiLTS;
            //                            }
            //                            else if (lst_nilai_lts.Count == 2)
            //                            {
            //                                id_tagihan++;
            //                                m.T1 = lst_nilai_lts[0].Nilai.ToUpper().Trim();
            //                                m.T1_Deskripsi = lst_nilai_lts[0].DeskripsiLTS;
            //                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_1 = lst_nilai_lts[0].DeskripsiLTS;

            //                                if (!(Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020))
            //                                {
            //                                    id_tagihan++;
            //                                }
            //                                else
            //                                {
            //                                    id_tagihan = 1;
            //                                }
            //                                m.T2 = lst_nilai_lts[1].Nilai.ToUpper().Trim();
            //                                m.T2_Deskripsi = lst_nilai_lts[1].DeskripsiLTS;
            //                                m_des.PoinTCol2_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_1 = lst_nilai_lts[1].DeskripsiLTS;
            //                            }
            //                            else if (lst_nilai_lts.Count == 3)
            //                            {
            //                                id_tagihan++;
            //                                m.T1 = lst_nilai_lts[0].Nilai.ToUpper().Trim();
            //                                m.T1_Deskripsi = lst_nilai_lts[0].DeskripsiLTS;
            //                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_1 = lst_nilai_lts[0].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T2 = lst_nilai_lts[1].Nilai.ToUpper().Trim();
            //                                m.T2_Deskripsi = lst_nilai_lts[1].DeskripsiLTS;
            //                                m_des.PoinTCol1_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_2 = lst_nilai_lts[1].DeskripsiLTS;

            //                                if (!(Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020))
            //                                {
            //                                    id_tagihan++;
            //                                }
            //                                else
            //                                {
            //                                    id_tagihan = 1;
            //                                }
            //                                m.T3 = lst_nilai_lts[2].Nilai.ToUpper().Trim();
            //                                m.T3_Deskripsi = lst_nilai_lts[2].DeskripsiLTS;
            //                                m_des.PoinTCol2_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_1 = lst_nilai_lts[2].DeskripsiLTS;
            //                            }
            //                            else if (lst_nilai_lts.Count == 4)
            //                            {
            //                                id_tagihan++;
            //                                m.T1 = lst_nilai_lts[0].Nilai.ToUpper().Trim();
            //                                m.T1_Deskripsi = lst_nilai_lts[0].DeskripsiLTS;
            //                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_1 = lst_nilai_lts[0].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T2 = lst_nilai_lts[1].Nilai.ToUpper().Trim();
            //                                m.T2_Deskripsi = lst_nilai_lts[1].DeskripsiLTS;
            //                                m_des.PoinTCol1_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_2 = lst_nilai_lts[1].DeskripsiLTS;

            //                                if (!(Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020))
            //                                {
            //                                    id_tagihan++;
            //                                }
            //                                else
            //                                {
            //                                    id_tagihan = 1;
            //                                }
            //                                m.T3 = lst_nilai_lts[2].Nilai.ToUpper().Trim();
            //                                m.T3_Deskripsi = lst_nilai_lts[2].DeskripsiLTS;
            //                                m_des.PoinTCol2_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_1 = lst_nilai_lts[2].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T4 = lst_nilai_lts[3].Nilai.ToUpper().Trim();
            //                                m.T4_Deskripsi = lst_nilai_lts[3].DeskripsiLTS;
            //                                m_des.PoinTCol2_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_2 = lst_nilai_lts[3].DeskripsiLTS;
            //                            }
            //                            else if (lst_nilai_lts.Count == 5)
            //                            {
            //                                id_tagihan++;
            //                                m.T1 = lst_nilai_lts[0].Nilai.ToUpper().Trim();
            //                                m.T1_Deskripsi = lst_nilai_lts[0].DeskripsiLTS;
            //                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_1 = lst_nilai_lts[0].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T2 = lst_nilai_lts[1].Nilai.ToUpper().Trim();
            //                                m.T2_Deskripsi = lst_nilai_lts[1].DeskripsiLTS;
            //                                m_des.PoinTCol1_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_2 = lst_nilai_lts[1].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T3 = lst_nilai_lts[2].Nilai.ToUpper().Trim();
            //                                m.T3_Deskripsi = lst_nilai_lts[2].DeskripsiLTS;
            //                                m_des.PoinTCol1_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_3 = lst_nilai_lts[2].DeskripsiLTS;

            //                                if (!(Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020))
            //                                {
            //                                    id_tagihan++;
            //                                }
            //                                else
            //                                {
            //                                    id_tagihan = 1;
            //                                }
            //                                m.T4 = lst_nilai_lts[3].Nilai.ToUpper().Trim();
            //                                m.T4_Deskripsi = lst_nilai_lts[3].DeskripsiLTS;
            //                                m_des.PoinTCol2_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_1 = lst_nilai_lts[3].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T5 = lst_nilai_lts[4].Nilai.ToUpper().Trim();
            //                                m.T5_Deskripsi = lst_nilai_lts[4].DeskripsiLTS;
            //                                m_des.PoinTCol2_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_2 = lst_nilai_lts[4].DeskripsiLTS;
            //                            }
            //                            else if (lst_nilai_lts.Count == 6)
            //                            {
            //                                id_tagihan++;
            //                                m.T1 = lst_nilai_lts[0].Nilai.ToUpper().Trim();
            //                                m.T1_Deskripsi = lst_nilai_lts[0].DeskripsiLTS;
            //                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_1 = lst_nilai_lts[0].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T2 = lst_nilai_lts[1].Nilai.ToUpper().Trim();
            //                                m.T2_Deskripsi = lst_nilai_lts[1].DeskripsiLTS;
            //                                m_des.PoinTCol1_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_2 = lst_nilai_lts[1].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T3 = lst_nilai_lts[2].Nilai.ToUpper().Trim();
            //                                m.T3_Deskripsi = lst_nilai_lts[2].DeskripsiLTS;
            //                                m_des.PoinTCol1_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_3 = lst_nilai_lts[2].DeskripsiLTS;

            //                                if (!(Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020))
            //                                {
            //                                    id_tagihan++;
            //                                }
            //                                else
            //                                {
            //                                    id_tagihan = 1;
            //                                }
            //                                m.T4 = lst_nilai_lts[3].Nilai.ToUpper().Trim();
            //                                m.T4_Deskripsi = lst_nilai_lts[3].DeskripsiLTS;
            //                                m_des.PoinTCol2_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_1 = lst_nilai_lts[3].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T5 = lst_nilai_lts[4].Nilai.ToUpper().Trim();
            //                                m.T5_Deskripsi = lst_nilai_lts[4].DeskripsiLTS;
            //                                m_des.PoinTCol2_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_2 = lst_nilai_lts[4].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T6 = lst_nilai_lts[5].Nilai.ToUpper().Trim();
            //                                m.T6_Deskripsi = lst_nilai_lts[5].DeskripsiLTS;
            //                                m_des.PoinTCol2_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_3 = lst_nilai_lts[5].DeskripsiLTS;
            //                            }
            //                            else if (lst_nilai_lts.Count == 7)
            //                            {
            //                                id_tagihan++;
            //                                m.T1 = lst_nilai_lts[0].Nilai.ToUpper().Trim();
            //                                m.T1_Deskripsi = lst_nilai_lts[0].DeskripsiLTS;
            //                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_1 = lst_nilai_lts[0].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T2 = lst_nilai_lts[1].Nilai.ToUpper().Trim();
            //                                m.T2_Deskripsi = lst_nilai_lts[1].DeskripsiLTS;
            //                                m_des.PoinTCol1_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_2 = lst_nilai_lts[1].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T3 = lst_nilai_lts[2].Nilai.ToUpper().Trim();
            //                                m.T3_Deskripsi = lst_nilai_lts[2].DeskripsiLTS;
            //                                m_des.PoinTCol1_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_3 = lst_nilai_lts[2].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T4 = lst_nilai_lts[3].Nilai.ToUpper().Trim();
            //                                m.T4_Deskripsi = lst_nilai_lts[3].DeskripsiLTS;
            //                                m_des.PoinTCol1_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_4 = lst_nilai_lts[3].DeskripsiLTS;

            //                                if (!(Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020))
            //                                {
            //                                    id_tagihan++;
            //                                }
            //                                else
            //                                {
            //                                    id_tagihan = 1;
            //                                }
            //                                m.T5 = lst_nilai_lts[4].Nilai.ToUpper().Trim();
            //                                m.T5_Deskripsi = lst_nilai_lts[4].DeskripsiLTS;
            //                                m_des.PoinTCol2_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_1 = lst_nilai_lts[4].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T6 = lst_nilai_lts[5].Nilai.ToUpper().Trim();
            //                                m.T6_Deskripsi = lst_nilai_lts[5].DeskripsiLTS;
            //                                m_des.PoinTCol2_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_2 = lst_nilai_lts[5].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T7 = lst_nilai_lts[6].Nilai.ToUpper().Trim();
            //                                m.T7_Deskripsi = lst_nilai_lts[6].DeskripsiLTS;
            //                                m_des.PoinTCol2_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_3 = lst_nilai_lts[6].DeskripsiLTS;
            //                            }
            //                            else if (lst_nilai_lts.Count == 8)
            //                            {
            //                                id_tagihan++;
            //                                m.T1 = lst_nilai_lts[0].Nilai.ToUpper().Trim();
            //                                m.T1_Deskripsi = lst_nilai_lts[0].DeskripsiLTS;
            //                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_1 = lst_nilai_lts[0].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T2 = lst_nilai_lts[1].Nilai.ToUpper().Trim();
            //                                m.T2_Deskripsi = lst_nilai_lts[1].DeskripsiLTS;
            //                                m_des.PoinTCol1_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_2 = lst_nilai_lts[1].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T3 = lst_nilai_lts[2].Nilai.ToUpper().Trim();
            //                                m.T3_Deskripsi = lst_nilai_lts[2].DeskripsiLTS;
            //                                m_des.PoinTCol1_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_3 = lst_nilai_lts[2].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T4 = lst_nilai_lts[3].Nilai.ToUpper().Trim();
            //                                m.T4_Deskripsi = lst_nilai_lts[3].DeskripsiLTS;
            //                                m_des.PoinTCol1_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_4 = lst_nilai_lts[3].DeskripsiLTS;

            //                                if (!(Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020))
            //                                {
            //                                    id_tagihan++;
            //                                }
            //                                else
            //                                {
            //                                    id_tagihan = 1;
            //                                }
            //                                m.T5 = lst_nilai_lts[4].Nilai.ToUpper().Trim();
            //                                m.T5_Deskripsi = lst_nilai_lts[4].DeskripsiLTS;
            //                                m_des.PoinTCol2_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_1 = lst_nilai_lts[4].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T6 = lst_nilai_lts[5].Nilai.ToUpper().Trim();
            //                                m.T6_Deskripsi = lst_nilai_lts[5].DeskripsiLTS;
            //                                m_des.PoinTCol2_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_2 = lst_nilai_lts[5].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T7 = lst_nilai_lts[6].Nilai.ToUpper().Trim();
            //                                m.T7_Deskripsi = lst_nilai_lts[6].DeskripsiLTS;
            //                                m_des.PoinTCol2_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_3 = lst_nilai_lts[6].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T8 = lst_nilai_lts[7].Nilai.ToUpper().Trim();
            //                                m.T8_Deskripsi = lst_nilai_lts[7].DeskripsiLTS;
            //                                m_des.PoinTCol2_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_4 = lst_nilai_lts[7].DeskripsiLTS;
            //                            }
            //                            else if (lst_nilai_lts.Count == 9)
            //                            {
            //                                id_tagihan++;
            //                                m.T1 = lst_nilai_lts[0].Nilai.ToUpper().Trim();
            //                                m.T1_Deskripsi = lst_nilai_lts[0].DeskripsiLTS;
            //                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_1 = lst_nilai_lts[0].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T2 = lst_nilai_lts[1].Nilai.ToUpper().Trim();
            //                                m.T2_Deskripsi = lst_nilai_lts[1].DeskripsiLTS;
            //                                m_des.PoinTCol1_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_2 = lst_nilai_lts[1].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T3 = lst_nilai_lts[2].Nilai.ToUpper().Trim();
            //                                m.T3_Deskripsi = lst_nilai_lts[2].DeskripsiLTS;
            //                                m_des.PoinTCol1_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_3 = lst_nilai_lts[2].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T4 = lst_nilai_lts[3].Nilai.ToUpper().Trim();
            //                                m.T4_Deskripsi = lst_nilai_lts[3].DeskripsiLTS;
            //                                m_des.PoinTCol1_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_4 = lst_nilai_lts[3].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T5 = lst_nilai_lts[4].Nilai.ToUpper().Trim();
            //                                m.T5_Deskripsi = lst_nilai_lts[4].DeskripsiLTS;
            //                                m_des.PoinTCol1_5 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_5 = lst_nilai_lts[4].DeskripsiLTS;

            //                                if (!(Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020))
            //                                {
            //                                    id_tagihan++;
            //                                }
            //                                else
            //                                {
            //                                    id_tagihan = 1;
            //                                }
            //                                m.T6 = lst_nilai_lts[5].Nilai.ToUpper().Trim();
            //                                m.T6_Deskripsi = lst_nilai_lts[5].DeskripsiLTS;
            //                                m_des.PoinTCol2_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_1 = lst_nilai_lts[5].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T7 = lst_nilai_lts[6].Nilai.ToUpper().Trim();
            //                                m.T7_Deskripsi = lst_nilai_lts[6].DeskripsiLTS;
            //                                m_des.PoinTCol2_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_2 = lst_nilai_lts[6].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T8 = lst_nilai_lts[7].Nilai.ToUpper().Trim();
            //                                m.T8_Deskripsi = lst_nilai_lts[7].DeskripsiLTS;
            //                                m_des.PoinTCol2_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_3 = lst_nilai_lts[7].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T9 = lst_nilai_lts[8].Nilai.ToUpper().Trim();
            //                                m.T9_Deskripsi = lst_nilai_lts[8].DeskripsiLTS;
            //                                m_des.PoinTCol2_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_4 = lst_nilai_lts[8].DeskripsiLTS;
            //                            }
            //                            else if (lst_nilai_lts.Count == 10)
            //                            {
            //                                id_tagihan++;
            //                                m.T1 = lst_nilai_lts[0].Nilai.ToUpper().Trim();
            //                                m.T1_Deskripsi = lst_nilai_lts[0].DeskripsiLTS;
            //                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_1 = lst_nilai_lts[0].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T2 = lst_nilai_lts[1].Nilai.ToUpper().Trim();
            //                                m.T2_Deskripsi = lst_nilai_lts[1].DeskripsiLTS;
            //                                m_des.PoinTCol1_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_2 = lst_nilai_lts[1].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T3 = lst_nilai_lts[2].Nilai.ToUpper().Trim();
            //                                m.T3_Deskripsi = lst_nilai_lts[2].DeskripsiLTS;
            //                                m_des.PoinTCol1_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_3 = lst_nilai_lts[2].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T4 = lst_nilai_lts[3].Nilai.ToUpper().Trim();
            //                                m.T4_Deskripsi = lst_nilai_lts[3].DeskripsiLTS;
            //                                m_des.PoinTCol1_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_4 = lst_nilai_lts[3].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T5 = lst_nilai_lts[4].Nilai.ToUpper().Trim();
            //                                m.T5_Deskripsi = lst_nilai_lts[4].DeskripsiLTS;
            //                                m_des.PoinTCol1_5 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_5 = lst_nilai_lts[4].DeskripsiLTS;

            //                                if (!(Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020))
            //                                {
            //                                    id_tagihan++;
            //                                }
            //                                else
            //                                {
            //                                    id_tagihan = 1;
            //                                }
            //                                m.T6 = lst_nilai_lts[5].Nilai.ToUpper().Trim();
            //                                m.T6_Deskripsi = lst_nilai_lts[5].DeskripsiLTS;
            //                                m_des.PoinTCol2_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_1 = lst_nilai_lts[5].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T7 = lst_nilai_lts[6].Nilai.ToUpper().Trim();
            //                                m.T7_Deskripsi = lst_nilai_lts[6].DeskripsiLTS;
            //                                m_des.PoinTCol2_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_2 = lst_nilai_lts[6].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T8 = lst_nilai_lts[7].Nilai.ToUpper().Trim();
            //                                m.T8_Deskripsi = lst_nilai_lts[7].DeskripsiLTS;
            //                                m_des.PoinTCol2_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_3 = lst_nilai_lts[7].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T9 = lst_nilai_lts[8].Nilai.ToUpper().Trim();
            //                                m.T9_Deskripsi = lst_nilai_lts[8].DeskripsiLTS;
            //                                m_des.PoinTCol2_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_4 = lst_nilai_lts[8].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T10 = lst_nilai_lts[9].Nilai.ToUpper().Trim();
            //                                m.T10_Deskripsi = lst_nilai_lts[9].DeskripsiLTS;
            //                                m_des.PoinTCol2_5 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_5 = lst_nilai_lts[9].DeskripsiLTS;
            //                            }
            //                            else if (lst_nilai_lts.Count == 11)
            //                            {
            //                                id_tagihan++;
            //                                m.T1 = lst_nilai_lts[0].Nilai.ToUpper().Trim();
            //                                m.T1_Deskripsi = lst_nilai_lts[0].DeskripsiLTS;
            //                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_1 = lst_nilai_lts[0].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T2 = lst_nilai_lts[1].Nilai.ToUpper().Trim();
            //                                m.T2_Deskripsi = lst_nilai_lts[1].DeskripsiLTS;
            //                                m_des.PoinTCol1_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_2 = lst_nilai_lts[1].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T3 = lst_nilai_lts[2].Nilai.ToUpper().Trim();
            //                                m.T3_Deskripsi = lst_nilai_lts[2].DeskripsiLTS;
            //                                m_des.PoinTCol1_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_3 = lst_nilai_lts[2].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T4 = lst_nilai_lts[3].Nilai.ToUpper().Trim();
            //                                m.T4_Deskripsi = lst_nilai_lts[3].DeskripsiLTS;
            //                                m_des.PoinTCol1_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_4 = lst_nilai_lts[3].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T5 = lst_nilai_lts[4].Nilai.ToUpper().Trim();
            //                                m.T5_Deskripsi = lst_nilai_lts[4].DeskripsiLTS;
            //                                m_des.PoinTCol1_5 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_5 = lst_nilai_lts[4].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T6 = lst_nilai_lts[5].Nilai.ToUpper().Trim();
            //                                m.T6_Deskripsi = lst_nilai_lts[5].DeskripsiLTS;
            //                                m_des.PoinTCol1_6 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_6 = lst_nilai_lts[5].DeskripsiLTS;

            //                                if (!(Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020))
            //                                {
            //                                    id_tagihan++;
            //                                }
            //                                else
            //                                {
            //                                    id_tagihan = 1;
            //                                }
            //                                m.T7 = lst_nilai_lts[6].Nilai.ToUpper().Trim();
            //                                m.T7_Deskripsi = lst_nilai_lts[6].DeskripsiLTS;
            //                                m_des.PoinTCol2_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_1 = lst_nilai_lts[6].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T8 = lst_nilai_lts[7].Nilai.ToUpper().Trim();
            //                                m.T8_Deskripsi = lst_nilai_lts[7].DeskripsiLTS;
            //                                m_des.PoinTCol2_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_2 = lst_nilai_lts[7].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T9 = lst_nilai_lts[8].Nilai.ToUpper().Trim();
            //                                m.T9_Deskripsi = lst_nilai_lts[8].DeskripsiLTS;
            //                                m_des.PoinTCol2_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_3 = lst_nilai_lts[8].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T10 = lst_nilai_lts[9].Nilai.ToUpper().Trim();
            //                                m.T10_Deskripsi = lst_nilai_lts[9].DeskripsiLTS;
            //                                m_des.PoinTCol2_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_4 = lst_nilai_lts[9].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T11 = lst_nilai_lts[10].Nilai.ToUpper().Trim();
            //                                m.T11_Deskripsi = lst_nilai_lts[10].DeskripsiLTS;
            //                                m_des.PoinTCol2_5 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_5 = lst_nilai_lts[10].DeskripsiLTS;
            //                            }
            //                            else if (lst_nilai_lts.Count >= 12)
            //                            {
            //                                id_tagihan++;
            //                                m.T1 = lst_nilai_lts[0].Nilai.ToUpper().Trim();
            //                                m.T1_Deskripsi = lst_nilai_lts[0].DeskripsiLTS;
            //                                m_des.PoinTCol1_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_1 = lst_nilai_lts[0].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T2 = lst_nilai_lts[1].Nilai.ToUpper().Trim();
            //                                m.T2_Deskripsi = lst_nilai_lts[1].DeskripsiLTS;
            //                                m_des.PoinTCol1_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_2 = lst_nilai_lts[1].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T3 = lst_nilai_lts[2].Nilai.ToUpper().Trim();
            //                                m.T3_Deskripsi = lst_nilai_lts[2].DeskripsiLTS;
            //                                m_des.PoinTCol1_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_3 = lst_nilai_lts[2].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T4 = lst_nilai_lts[3].Nilai.ToUpper().Trim();
            //                                m.T4_Deskripsi = lst_nilai_lts[3].DeskripsiLTS;
            //                                m_des.PoinTCol1_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_4 = lst_nilai_lts[3].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T5 = lst_nilai_lts[4].Nilai.ToUpper().Trim();
            //                                m.T5_Deskripsi = lst_nilai_lts[4].DeskripsiLTS;
            //                                m_des.PoinTCol1_5 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_5 = lst_nilai_lts[4].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T6 = lst_nilai_lts[5].Nilai.ToUpper().Trim();
            //                                m.T6_Deskripsi = lst_nilai_lts[5].DeskripsiLTS;
            //                                m_des.PoinTCol1_6 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol1_6 = lst_nilai_lts[5].DeskripsiLTS;

            //                                if (!(Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020))
            //                                {
            //                                    id_tagihan++;
            //                                }
            //                                else
            //                                {
            //                                    id_tagihan = 1;
            //                                }
            //                                m.T7 = lst_nilai_lts[6].Nilai.ToUpper().Trim();
            //                                m.T7_Deskripsi = lst_nilai_lts[6].DeskripsiLTS;
            //                                m_des.PoinTCol2_1 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_1 = lst_nilai_lts[6].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T8 = lst_nilai_lts[7].Nilai.ToUpper().Trim();
            //                                m.T8_Deskripsi = lst_nilai_lts[7].DeskripsiLTS;
            //                                m_des.PoinTCol2_2 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_2 = lst_nilai_lts[7].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T9 = lst_nilai_lts[8].Nilai.ToUpper().Trim();
            //                                m.T9_Deskripsi = lst_nilai_lts[8].DeskripsiLTS;
            //                                m_des.PoinTCol2_3 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_3 = lst_nilai_lts[8].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T10 = lst_nilai_lts[9].Nilai.ToUpper().Trim();
            //                                m.T10_Deskripsi = lst_nilai_lts[9].DeskripsiLTS;
            //                                m_des.PoinTCol2_4 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_4 = lst_nilai_lts[9].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T11 = lst_nilai_lts[10].Nilai.ToUpper().Trim();
            //                                m.T11_Deskripsi = lst_nilai_lts[10].DeskripsiLTS;
            //                                m_des.PoinTCol2_5 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_5 = lst_nilai_lts[10].DeskripsiLTS;

            //                                id_tagihan++;
            //                                m.T12 = lst_nilai_lts[11].Nilai.ToUpper().Trim();
            //                                m.T12_Deskripsi = lst_nilai_lts[11].DeskripsiLTS;
            //                                m_des.PoinTCol2_6 = (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_tagihan.ToString() + ":";
            //                                m_des.DesPoinTCol2_6 = lst_nilai_lts[11].DeskripsiLTS;
            //                            }

            //                            m.WaliKelas = s_walikelas;

            //                            Mapel m_mapel = lst_mapel.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item_rapor_desain_det.Rel_Mapel.ToUpper().Trim()).FirstOrDefault();
            //                            bool b_tampil = true;
            //                            string s_nomor_mapel = item_rapor_desain_det.Nomor;

            //                            m.NomorMapel = s_nomor_mapel;
            //                            m_des.NomorMapel = s_nomor_mapel;

            //                            if (m.KodeKelompokMapel.Trim() != "" && m.NomorMapel.ToString().Trim() == "")
            //                            {
            //                                s_html += "<tr>" +
            //                                                "<td style=\"width: 40px; vertical-align: middle; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.KodeKelompokMapel + "</td>" +
            //                                                "<td colspan=\"18\" style=\"vertical-align: middle; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.NamaMapel + "</td>" +
            //                                              "</tr>";

            //                                s_html_deskripsi +=
            //                                          "<tr>" +
            //                                            "<td style=\"width: 40px; vertical-align: middle; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.KodeKelompokMapel + "</td>" +
            //                                            "<td colspan=\"5\" style=\"vertical-align: middle; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.NamaMapel + "</td>" +
            //                                          "</tr>";
            //                            }
            //                            else
            //                            {
            //                                if (m_mapel != null)
            //                                {
            //                                    if (m_mapel.Nama != null)
            //                                    {
            //                                        if (m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN && !ada_nilai)
            //                                        {
            //                                            b_tampil = false;
            //                                        }
            //                                        else if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT && ada_nilai)
            //                                        {
            //                                            b_tampil = true;
            //                                            id_lintas_minat++;
            //                                            s_nomor_mapel = id_lintas_minat.ToString();
            //                                            m.NomorMapel = s_nomor_mapel;
            //                                            m_des.NomorMapel = s_nomor_mapel;
            //                                        }
            //                                        else if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT && !ada_nilai)
            //                                        {
            //                                            b_tampil = false;
            //                                        }
            //                                    }
            //                                }

            //                                if (b_tampil)
            //                                {
            //                                    s_html += "<tr>" +
            //                                                "<td style=\"width: 40px; vertical-align: middle; text-align: right; padding: 3px; border-style: solid; border-width: 0.5px;\">" + s_nomor_mapel + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.NamaMapel + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;\">" + m.KKM + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T1) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T1_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T1_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T1 + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T2) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T2_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T2_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T2 + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T3) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T3_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T3_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T3 + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T4) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T4_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T4_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T4 + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T5) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T5_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T5_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T5 + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T6) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T6_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T6_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T6 + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T7) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T7_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T7_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T7 + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T8) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T8_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T8_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T8 + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T9) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T9_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T9_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T9 + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T10) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T10_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T10_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T10 + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T11) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T11_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T11_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T11 + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;" + (Libs.GetStringToDecimal(m.T12) < Libs.GetStringToDecimal(m.KKM) ? " color: red;" : "") + "\" " + (Libs.GetHTMLNoParagraphDiAwal(m.T12_Deskripsi).Trim() != "" ? " data-tooltip=\"" + Libs.GetHTMLNoParagraphDiAwal(m.T12_Deskripsi.Replace("\"", "&rdquo;")) + "\" " : "") + ">" + m.T12 + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;\">" + m.HD + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;\">" + m.LK + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;\">" + m.RJ + "</td>" +
            //                                                "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black;\">" + m.RPKB + "</td>" +
            //                                            "</tr>";

            //                                    string s_html_deskripsi_mapel = "";
            //                                    int rows_span = 0;
            //                                    int i_count = (lst_nilai_lts.Count > 12 ? 12 : lst_nilai_lts.Count);
            //                                    if (i_count > 1)
            //                                    {
            //                                        if (i_count % 2 == 0)
            //                                        {
            //                                            rows_span = Convert.ToInt16(Math.Round(Convert.ToDecimal(i_count / 2), 0));
            //                                        }
            //                                        else
            //                                        {
            //                                            rows_span = Convert.ToInt16(Convert.ToDecimal((i_count + 1) / 2));
            //                                        }

            //                                        if (rows_span >= 1)
            //                                        {
            //                                            for (int i = 1; i <= rows_span; i++)
            //                                            {
            //                                                int id_kol_2 = ((i * 2) + (rows_span - i));
            //                                                s_html_deskripsi_mapel += "<tr>" +
            //                                                                              (
            //                                                                                i == 1
            //                                                                                ? "<td " + (rows_span > 1 ? "rowspan=\"" + rows_span + "\"" : "") + " style=\"width: 40px; vertical-align: top; text-align: right; padding: 3px; border-style: solid; border-width: 0.5px;\">" + s_nomor_mapel + "</td>" +
            //                                                                                  "<td " + (rows_span > 1 ? "rowspan=\"" + rows_span + "\"" : "") + " style=\"vertical-align: top; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.NamaMapel + "</td>"
            //                                                                                : ""
            //                                                                              ) +
            //                                                                              "<td style=\"vertical-align: top; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black; width: 30px; border-right-style: none;\">" +
            //                                                                                (
            //                                                                                    i_count >= (rows_span)
            //                                                                                    ? (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + i.ToString()
            //                                                                                    : "&nbsp;"
            //                                                                                ) +
            //                                                                              "</td>" +
            //                                                                              "<td style=\"vertical-align: top; text-align: justify; padding: 3px; padding-right: 7px; border-style: solid; border-width: 0.5px; border-color: black; border-left-style: none; width: 30%;\">" +
            //                                                                                (
            //                                                                                    i_count >= (rows_span)
            //                                                                                    ? Libs.GetHTMLNoParagraphDiAwal(lst_nilai_lts[i - 1].DeskripsiLTS)
            //                                                                                    : "&nbsp;"
            //                                                                                ) +
            //                                                                              "</td>";

            //                                                s_html_deskripsi_mapel += "<td style=\"vertical-align: top; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black; width: 30px; border-right-style: none;\">" +
            //                                                                                    (
            //                                                                                        i_count > id_kol_2 - 1
            //                                                                                        ? (
            //                                                                                            (Libs.GetStringToDouble(tahun_ajaran.Replace("/", "")) <= 20192020 ? "T" : "") + id_kol_2.ToString()
            //                                                                                          )
            //                                                                                        : "&nbsp;"
            //                                                                                    ) +
            //                                                                                  "</td>" +
            //                                                                                  "<td style=\"vertical-align: top; text-align: justify; padding: 3px; padding-right: 7px; border-style: solid; border-width: 0.5px; border-color: black; border-left-style: none; width: 30%;\">" +
            //                                                                                    (
            //                                                                                        i_count > id_kol_2 - 1
            //                                                                                        ? (
            //                                                                                            Libs.GetHTMLNoParagraphDiAwal(lst_nilai_lts[id_kol_2 - 1].DeskripsiLTS)
            //                                                                                          )
            //                                                                                        : "&nbsp;"
            //                                                                                    ) +
            //                                                                                  "</td>" +
            //                                                                                "</tr>";
            //                                            }

            //                                            s_html_deskripsi += s_html_deskripsi_mapel;
            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        s_html_deskripsi_mapel =
            //                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black; border-right-style: none;\">" + "&nbsp;" + "</td>" +
            //                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black; border-left-style: none; width: 30%;\">" + "&nbsp;" + "</td>" +
            //                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black; border-right-style: none;\">" + "&nbsp;" + "</td>" +
            //                                            "<td style=\"vertical-align: middle; text-align: center; padding: 3px; border-style: solid; border-width: 0.5px; border-color: black; border-left-style: none; width: 30%;\">" + "&nbsp;" + "</td>";

            //                                        s_html_deskripsi +=
            //                                            "<tr>" +
            //                                                "<td " + (i_count > 1 ? "rowspan=\"" + rows_span + "\"" : "") + " style=\"width: 40px; vertical-align: top; text-align: right; padding: 3px; border-style: solid; border-width: 0.5px;\">" + s_nomor_mapel + "</td>" +
            //                                                "<td " + (i_count > 1 ? "rowspan=\"" + rows_span + "\"" : "") + " style=\"vertical-align: top; text-align: left; padding: 3px; border-style: solid; border-width: 0.5px;\">" + m.NamaMapel + "</td>" +
            //                                                s_html_deskripsi_mapel +
            //                                            "</tr>";
            //                                    }
            //                                }
            //                            }

            //                            if (b_tampil)
            //                            {
            //                                lst_hasil_lts.Add(m);
            //                                lst_hasil_deksripsi.Add(m_des);
            //                            }

            //                        }

            //                        s_html += "</table>";

            //                        s_html += "<table style=\"width: 100%; font-size: small; margin-top: 20px;\">" +
            //                                    "<tr>" +
            //                                        "<td rowspan=\"7\" style=\"width: 30px; padding: 3px; vertical-align: top;\">" +
            //                                            "Ketr." +
            //                                        "</td>" +
            //                                        "<td style=\"text-align: right; padding: 3px; width: 60px;\">" +
            //                                            "KKM" +
            //                                        "</td>" +
            //                                        "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
            //                                            ":" +
            //                                        "</td>" +
            //                                        "<td style=\"padding: 3px;\">" +
            //                                            "Kriteria Ketuntasan Minimal" +
            //                                        "</td>" +
            //                                        "<td colspan=\"2\" style=\"border-style: solid; border-width: 0.5px; text-align: center; padding-top: 3px; padding-bottom: 3px;\">" +
            //                                            "Eksktrakurikuler yang diikuti" +
            //                                        "</td>" +
            //                                        "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                            "&nbsp;" +
            //                                        "</td>" +
            //                                        "<td colspan=\"2\" style=\"border-style: solid; border-width: 0.5px; text-align: center; padding-top: 3px; padding-bottom: 3px;\">" +
            //                                            "Absensi Siswa" +
            //                                        "</td>" +
            //                                        "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                            "&nbsp;" +
            //                                        "</td>" +
            //                                        "<td rowspan=\"7\" style=\"border-style: none; border-width: 0px; text-align: left; padding-top: 3px; padding-bottom: 3px; width: 200px; vertical-align: top;\">" +
            //                                            "Jakarta, " +
            //                                            (
            //                                                m_rapor_arsip != null
            //                                                ? (
            //                                                        m_rapor_arsip.TanggalRapor != DateTime.MinValue
            //                                                        ? Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false)
            //                                                        : ""
            //                                                  )
            //                                                : ""
            //                                            ) +
            //                                            "<br />" +
            //                                            "Wali Kelas," +
            //                                            "<br />" +
            //                                            "<br />" +
            //                                            "<br />" +
            //                                            "<br />" +
            //                                            "<br />" +
            //                                            "<label style=\"font-weight: bold; text-decoration: underline;\">" +
            //                                            s_walikelas +
            //                                            "</label>" +
            //                                        "</td>" +
            //                                    "</tr>";

            //                        s_html +=
            //                                "<tr>" +
            //                                    "<td style=\"text-align: right; padding: 3px; width: 60px;\">" +
            //                                        "HD" +
            //                                    "</td>" +
            //                                    "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
            //                                        ":" +
            //                                    "</td>" +
            //                                    "<td style=\"padding: 3px;\">" +
            //                                        "Kehadiran (jumlah kehadiran siswa" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: center; padding: 3px; width: 150px;\">" +
            //                                        "Nama Ekskul" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: center; padding: 3px; width: 100px;\">" +
            //                                        "Jumlah Hadir" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: center; padding: 3px; width: 150px;\">" +
            //                                        "Ketidakhadiran" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: center; padding: 3px; width: 100px;\">" +
            //                                        "Jumlah" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                "</tr>";

            //                        s_html +=
            //                                "<tr>" +
            //                                    "<td style=\"text-align: right; padding: 3px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"padding: 3px; width: 280px;\">" +
            //                                        "berbanding dengan total pertemuan)" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: left; padding: 3px;\">" +
            //                                        (
            //                                            lst_ekskul_det.Count >= 1
            //                                            ? lst_ekskul_det[0].Mapel
            //                                            : ""
            //                                        ) +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
            //                                        (
            //                                            lst_ekskul_det.Count >= 1
            //                                            ? lst_ekskul_det[0].LTS_HD
            //                                            : ""
            //                                        ) +
            //                                        "&nbsp;&nbsp;kali" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: left; padding: 3px;\">" +
            //                                        "Sakit" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
            //                                        s_sakit +
            //                                        "&nbsp;&nbsp;hari" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                "</tr>";

            //                        s_html +=
            //                                "<tr>" +
            //                                    "<td style=\"text-align: right; padding: 3px; width: 60px;\">" +
            //                                        "LK" +
            //                                    "</td>" +
            //                                    "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
            //                                        ":" +
            //                                    "</td>" +
            //                                    "<td style=\"padding: 3px;\">" +
            //                                        "Kelakuan" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: left; padding: 3px;\">" +
            //                                        (
            //                                            lst_ekskul_det.Count >= 2
            //                                            ? lst_ekskul_det[1].Mapel
            //                                            : ""
            //                                        ) +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
            //                                        (
            //                                            lst_ekskul_det.Count >= 2
            //                                            ? lst_ekskul_det[1].LTS_HD
            //                                            : ""
            //                                        ) +
            //                                        "&nbsp;&nbsp;kali" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: left; padding: 3px;\">" +
            //                                        "Izin" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
            //                                        s_izin +
            //                                        "&nbsp;&nbsp;hari" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                "</tr>";

            //                        s_html +=
            //                                "<tr>" +
            //                                    "<td style=\"text-align: right; padding: 3px; width: 60px;\">" +
            //                                        "RJ" +
            //                                    "</td>" +
            //                                    "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
            //                                        ":" +
            //                                    "</td>" +
            //                                    "<td style=\"padding: 3px;\">" +
            //                                        "Kerajinan" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: left; padding: 3px;\">" +
            //                                        (
            //                                            lst_ekskul_det.Count >= 3
            //                                            ? lst_ekskul_det[2].Mapel
            //                                            : ""
            //                                        ) +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
            //                                        (
            //                                            lst_ekskul_det.Count >= 3
            //                                            ? lst_ekskul_det[2].LTS_HD
            //                                            : ""
            //                                        ) +
            //                                        "&nbsp;&nbsp;kali" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: left; padding: 3px;\">" +
            //                                        "Tanpa Keterangan" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
            //                                        s_alpa +
            //                                        "&nbsp;&nbsp;hari" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                "</tr>";

            //                        s_html +=
            //                                "<tr>" +
            //                                    "<td style=\"text-align: right; padding: 3px; width: 60px;\">" +
            //                                        "RPKB" +
            //                                    "</td>" +
            //                                    "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
            //                                        ":" +
            //                                    "</td>" +
            //                                    "<td style=\"padding: 3px;\">" +
            //                                        "Kerapian dan Kebersihan" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
            //                                        "Jumlah" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: solid; border-width: 0.5px; text-align: right; padding: 3px;\">" +
            //                                        (
            //                                            Libs.GetStringToDecimal(s_sakit) +
            //                                            Libs.GetStringToDecimal(s_izin) +
            //                                            Libs.GetStringToDecimal(s_alpa)
            //                                        ).ToString() +
            //                                        "&nbsp;&nbsp;hari" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                "</tr>";

            //                        s_html +=
            //                                "<tr>" +
            //                                    "<td style=\"text-align: right; padding: 3px; width: 60px;\">" +
            //                                        "BL" +
            //                                    "</td>" +
            //                                    "<td style=\"width: 15px; padding: 3px; text-align: center;\">" +
            //                                        ":" +
            //                                    "</td>" +
            //                                    "<td style=\"padding: 3px;\">" +
            //                                        "Belum" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: center; padding: 3px; width: 150px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: center; padding: 3px; width: 100px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: center; padding: 3px; width: 150px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: center; padding: 3px; width: 100px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                    "<td style=\"border-style: none; border-width: 0.5px; text-align: left; padding: 3px; width: 15px;\">" +
            //                                        "&nbsp;" +
            //                                    "</td>" +
            //                                "</tr>" +

            //                              "</table>" +
            //                              "<hr />";

            //                        s_html += "<table style=\"width: 100%;\">" +
            //                                    "<tr>" +
            //                                        "<td style=\"width: 50%; text-align: left;\">" +
            //                                            "Keterangan Tagihan (LTS " + semester + " - TP " + tahun_ajaran + ") *)" +
            //                                        "</td>" +
            //                                        "<td style=\"width: 50%; text-align: right;\">" +
            //                                            m_siswa.Nama.Trim().ToUpper() +
            //                                        "</td>" +
            //                                    "<tr>" +
            //                                  "</table>" +
            //                                  "<table style=\"width: 100%; font-size: small;\">" +
            //                                    s_html_deskripsi +
            //                                  "</table>";
            //                    }
            //                }

            //            }

            //            GetHTML = s_html;
            //            GetRaporLTS = lst_hasil_lts;
            //            GetRaporLTSDeskripsi = lst_hasil_deksripsi;
            //        }
            //    }
            //}
        }
    }
}