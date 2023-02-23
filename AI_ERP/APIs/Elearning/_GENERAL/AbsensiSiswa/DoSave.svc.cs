using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.APIs.Elearning._GENERAL.AbsensiSiswa
{
    public class DoSave : IDoSave
    {
        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] Do(
                string sw,
                string a,
                string k,
                string t,
                string s,
                string kd,
                string tgl,
                string lm,
                string m,
                string jk,
                string kj,
                string bs,
                string bsl,
                string skp,
                string tl,
                string act_ket,
                string ssid
            )
        {
            List<string> hasil = new List<string>();

            string siswa = sw;
            string absen = a;
            string keterangan = k;
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(t);
            string semester = s;
            string rel_kelasdet = kd;
            string tanggal = tgl;
            string linimasa = lm;
            string rel_mapel = m;
            string jam_ke = jk;
            string kejadian = kj;
            string butir_sikap = bs;
            string butir_sikap_lain = bsl;
            string sikap = (skp == "plus" ? "+" : skp);
            string tindak_lanjut = tl;
            string keterangan_act = act_ket;
            string s_ssid = Libs.Decryptdata(ssid);

            bool ada_linimasa = false;

            //parse absen
            string Is_Hadir = "";
            DateTime Is_Hadir_Time = DateTime.MinValue;
            string Is_Sakit = "";
            DateTime Is_Sakit_Time = DateTime.MinValue;
            string Is_Izin = "";
            DateTime Is_Izin_Time = DateTime.MinValue;
            string Is_Alpa = "";
            DateTime Is_Alpa_Time = DateTime.MinValue;

            string Is_Cat01 = "";
            DateTime Is_Cat01_Time = DateTime.MinValue;
            string Is_Cat02 = "";
            DateTime Is_Cat02_Time = DateTime.MinValue;
            string Is_Cat03 = "";
            DateTime Is_Cat03_Time = DateTime.MinValue;
            string Is_Cat04 = "";
            DateTime Is_Cat04_Time = DateTime.MinValue;
            string Is_Cat05 = "";
            DateTime Is_Cat05_Time = DateTime.MinValue;
            string Is_Cat06 = "";
            DateTime Is_Cat06_Time = DateTime.MinValue;
            string Is_Cat07 = "";
            DateTime Is_Cat07_Time = DateTime.MinValue;
            string Is_Cat08 = "";
            DateTime Is_Cat08_Time = DateTime.MinValue;
            string Is_Cat09 = "";
            DateTime Is_Cat09_Time = DateTime.MinValue;
            string Is_Cat10 = "";
            DateTime Is_Cat10_Time = DateTime.MinValue;
            string Is_Cat11 = "";
            DateTime Is_Cat11_Time = DateTime.MinValue;
            string Is_Cat12 = "";
            DateTime Is_Cat12_Time = DateTime.MinValue;
            string Is_Cat13 = "";
            DateTime Is_Cat13_Time = DateTime.MinValue;
            string Is_Cat14 = "";
            DateTime Is_Cat14_Time = DateTime.MinValue;
            string Is_Cat15 = "";
            DateTime Is_Cat15_Time = DateTime.MinValue;
            string Is_Cat16 = "";
            DateTime Is_Cat16_Time = DateTime.MinValue;
            string Is_Cat17 = "";
            DateTime Is_Cat17_Time = DateTime.MinValue;
            string Is_Cat18 = "";
            DateTime Is_Cat18_Time = DateTime.MinValue;
            string Is_Cat19 = "";
            DateTime Is_Cat19_Time = DateTime.MinValue;
            string Is_Cat20 = "";
            DateTime Is_Cat20_Time = DateTime.MinValue;

            string Is_Sakit_Keterangan = "";
            string Is_Izin_Keterangan = "";
            string Is_Alpa_Keterangan = "";

            string Is_Cat01_Keterangan = "";
            string Is_Cat02_Keterangan = "";
            string Is_Cat03_Keterangan = "";
            string Is_Cat04_Keterangan = "";
            string Is_Cat05_Keterangan = "";
            string Is_Cat06_Keterangan = "";
            string Is_Cat07_Keterangan = "";
            string Is_Cat08_Keterangan = "";
            string Is_Cat09_Keterangan = "";
            string Is_Cat10_Keterangan = "";
            string Is_Cat11_Keterangan = "";
            string Is_Cat12_Keterangan = "";
            string Is_Cat13_Keterangan = "";
            string Is_Cat14_Keterangan = "";
            string Is_Cat15_Keterangan = "";
            string Is_Cat16_Keterangan = "";
            string Is_Cat17_Keterangan = "";
            string Is_Cat18_Keterangan = "";
            string Is_Cat19_Keterangan = "";
            string Is_Cat20_Keterangan = "";

            string[] arr_absen = absen.Split(new string[] { ";" }, StringSplitOptions.None);
            absen = " ";
            int id_cat = 0;
            foreach (var item_absen in arr_absen)
            {
                string[] arr_item_absen = item_absen.Split(new string[] { "~" }, StringSplitOptions.None);
                if (arr_item_absen.Length == 5)
                {
                    string s_kode_siswa = arr_item_absen[0]; //kode siswa
                    string s_inisial = arr_item_absen[1]; //inisial
                    string s_is_checked = arr_item_absen[2]; //checked value
                    string s_update_time = (arr_item_absen[3].Trim() == "" ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : arr_item_absen[3]); //updated time
                    string s_keterangan = arr_item_absen[4]; //keterangan kedisiplinan & absen

                    if (
                        s_inisial.Trim().ToUpper() == "HDR" ||
                        s_inisial.Trim().ToUpper() == "SKT" ||
                        s_inisial.Trim().ToUpper() == "IZN" ||
                        s_inisial.Trim().ToUpper() == "APA"
                      )
                    {
                        switch (s_inisial.Trim().ToUpper())
                        {
                            case "HDR":
                                Is_Hadir = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Hadir_Time = Convert.ToDateTime(s_update_time);
                                break;
                            case "SKT":
                                Is_Sakit = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Sakit_Time = Convert.ToDateTime(s_update_time);
                                Is_Sakit_Keterangan = s_keterangan;
                                break;
                            case "IZN":
                                Is_Izin = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Izin_Time = Convert.ToDateTime(s_update_time);
                                Is_Izin_Keterangan = s_keterangan;
                                break;
                            case "APA":
                                Is_Alpa = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Alpa_Time = Convert.ToDateTime(s_update_time);
                                Is_Alpa_Keterangan = s_keterangan;
                                break;
                        }
                    }
                    else
                    {
                        if (Is_Hadir != "HDR")
                        {
                            s_is_checked = "1";
                            s_keterangan = "";
                        }

                        id_cat++;
                        switch (id_cat)
                        {
                            case 1:
                                Is_Cat01 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat01_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat01_Keterangan = s_keterangan;
                                break;
                            case 2:
                                Is_Cat02 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat02_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat02_Keterangan = s_keterangan;
                                break;
                            case 3:
                                Is_Cat03 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat03_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat03_Keterangan = s_keterangan;
                                break;
                            case 4:
                                Is_Cat04 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat04_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat04_Keterangan = s_keterangan;
                                break;
                            case 5:
                                Is_Cat05 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat05_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat05_Keterangan = s_keterangan;
                                break;
                            case 6:
                                Is_Cat06 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat06_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat06_Keterangan = s_keterangan;
                                break;
                            case 7:
                                Is_Cat07 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat07_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat07_Keterangan = s_keterangan;
                                break;
                            case 8:
                                Is_Cat08 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat08_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat08_Keterangan = s_keterangan;
                                break;
                            case 9:
                                Is_Cat09 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat09_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat09_Keterangan = s_keterangan;
                                break;
                            case 10:
                                Is_Cat10 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat10_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat10_Keterangan = s_keterangan;
                                break;
                            case 11:
                                Is_Cat11 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat11_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat11_Keterangan = s_keterangan;
                                break;
                            case 12:
                                Is_Cat12 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat12_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat12_Keterangan = s_keterangan;
                                break;
                            case 13:
                                Is_Cat13 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat13_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat13_Keterangan = s_keterangan;
                                break;
                            case 14:
                                Is_Cat14 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat14_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat14_Keterangan = s_keterangan;
                                break;
                            case 15:
                                Is_Cat15 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat15_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat15_Keterangan = s_keterangan;
                                break;
                            case 16:
                                Is_Cat16 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat16_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat16_Keterangan = s_keterangan;
                                break;
                            case 17:
                                Is_Cat17 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat17_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat17_Keterangan = s_keterangan;
                                break;
                            case 18:
                                Is_Cat18 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat18_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat18_Keterangan = s_keterangan;
                                break;
                            case 19:
                                Is_Cat19 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat19_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat19_Keterangan = s_keterangan;
                                break;
                            case 20:
                                Is_Cat20 = (s_is_checked == "1" ? s_inisial.Trim().ToUpper() : "__" + s_inisial.Trim().ToUpper());
                                Is_Cat20_Time = Convert.ToDateTime(s_update_time);
                                Is_Cat20_Keterangan = s_keterangan;
                                break;
                        }
                    }
                }
            }

            if (Is_Hadir == "HDR")
            {
                Is_Sakit = "__SKT";
                Is_Sakit_Time = DateTime.MinValue;
                Is_Sakit_Keterangan = "";

                Is_Izin = "__IZN";
                Is_Izin_Time = DateTime.MinValue;
                Is_Izin_Keterangan = "";

                Is_Alpa = "__APA";
                Is_Alpa_Time = DateTime.MinValue;
                Is_Alpa_Keterangan = "";
            }
            //end parse absen

            KelasDet m_kelasdet = DAO_KelasDet.GetByID_Entity(rel_kelasdet);
            if (m_kelasdet != null)
            {
                if (m_kelasdet.Nama != null)
                {
                    ERoutingData m_routing_data = RoutingData.GetRoutingByKelasDet(rel_kelasdet);
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelasdet.Rel_Kelas.ToString());

                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            DateTime dt_tanggal = Libs.GetDateFromTanggalIndonesiaStr(tanggal);
                            LinimasaKelas m_linimasa = DAO_LinimasaKelas.GetByID_Entity(linimasa);
                            ada_linimasa = false;

                            Guid kode_absen = Guid.NewGuid();
                            bool ada_absen = false;
                            if (rel_mapel.Trim() == "")
                            {
                                //get lini masa && insert jika belum ada
                                if (
                                    m_linimasa != null
                                )
                                {
                                    if (m_linimasa.Jenis != null)
                                    {
                                        DAO_LinimasaKelas.Update(new LinimasaKelas
                                        {
                                            Kode = new Guid(linimasa),
                                            Jenis = Libs.JENIS_LINIMASA.ABSEN_SISWA_HARIAN,
                                            Rel_KelasDet = rel_kelasdet,
                                            TahunAjaran = tahun_ajaran,
                                            Keterangan = Libs.GetTanggalIndonesiaFromDate(dt_tanggal, false),
                                            ACT_KETERANGAN = act_ket
                                        }, s_ssid);

                                        ada_linimasa = true;
                                    }
                                }

                                if (!ada_linimasa)
                                {
                                    DAO_LinimasaKelas.Insert(new LinimasaKelas
                                    {
                                        Kode = new Guid(linimasa),
                                        Jenis = Libs.JENIS_LINIMASA.ABSEN_SISWA_HARIAN,
                                        Tanggal = DateTime.Now,
                                        Rel_KelasDet = rel_kelasdet,
                                        TahunAjaran = tahun_ajaran,
                                        Keterangan = Libs.GetTanggalIndonesiaFromDate(dt_tanggal, false),
                                        ACT = GetValidateAbsen(
                                                Libs.GetTanggalIndonesiaFromDate(
                                                      new DateTime(dt_tanggal.Year, dt_tanggal.Month, dt_tanggal.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
                                                    , false), 
                                                rel_mapel, rel_kelasdet, s_ssid
                                              ),
                                        ACT_KETERANGAN = act_ket,
                                        RTG_UNIT = m_routing_data.Unit,
                                        RTG_LEVEL = m_routing_data.Level,
                                        RTG_KELAS = m_routing_data.Kelas,
                                        RTG_SEMESTER = Libs.GetSemesterByTanggal(dt_tanggal).ToString(),
                                        RTG_SUBKELAS = m_routing_data.SubKelas
                                    }, s_ssid);
                                }
                                //end get lini masa && insert jika belum ada

                                //data siswa absen
                                SiswaAbsen m_absen = DAO_SiswaAbsen.GetAllBySekolahByKelasDetBySiswaByTanggal_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelasdet,
                                        siswa,
                                        dt_tanggal
                                    ).FirstOrDefault();

                                if (m_absen != null)
                                {
                                    if (m_absen.Absen != null)
                                    {
                                        kode_absen = m_absen.Kode;
                                        DAO_SiswaAbsen.Update(
                                            new SiswaAbsen
                                            {
                                                Kode = kode_absen,
                                                TahunAjaran = tahun_ajaran,
                                                Semester = Libs.GetSemesterByTanggal(dt_tanggal).ToString(),
                                                Rel_Sekolah = m_kelas.Rel_Sekolah,
                                                Rel_KelasDet = new Guid(rel_kelasdet),
                                                Tanggal = dt_tanggal,
                                                Rel_Siswa = siswa,
                                                Absen = absen.Substring(0, 1).ToUpper(),
                                                Keterangan = keterangan,
                                                Rel_Guru = s_ssid,
                                                Rel_Linimasa = new Guid(linimasa),
                                                Kejadian = kejadian,
                                                ButirSikap = butir_sikap,
                                                ButirSikapLain = butir_sikap_lain,
                                                Sikap = sikap,
                                                TindakLanjut = tindak_lanjut,

                                                Is_Hadir = Is_Hadir,
                                                Is_Hadir_Time = Is_Hadir_Time,
                                                Is_Sakit = Is_Sakit,
                                                Is_Sakit_Time = Is_Sakit_Time,
                                                Is_Izin = Is_Izin,
                                                Is_Izin_Time = Is_Izin_Time,
                                                Is_Alpa = Is_Alpa,
                                                Is_Alpa_Time = Is_Alpa_Time,
                                                Is_Cat01 = Is_Cat01,
                                                Is_Cat01_Time = Is_Cat01_Time,
                                                Is_Cat02 = Is_Cat02,
                                                Is_Cat02_Time = Is_Cat02_Time,
                                                Is_Cat03 = Is_Cat03,
                                                Is_Cat03_Time = Is_Cat03_Time,
                                                Is_Cat04 = Is_Cat04,
                                                Is_Cat04_Time = Is_Cat04_Time,
                                                Is_Cat05 = Is_Cat05,
                                                Is_Cat05_Time = Is_Cat05_Time,
                                                Is_Cat06 = Is_Cat06,
                                                Is_Cat06_Time = Is_Cat06_Time,
                                                Is_Cat07 = Is_Cat07,
                                                Is_Cat07_Time = Is_Cat07_Time,
                                                Is_Cat08 = Is_Cat08,
                                                Is_Cat08_Time = Is_Cat08_Time,
                                                Is_Cat09 = Is_Cat09,
                                                Is_Cat09_Time = Is_Cat09_Time,
                                                Is_Cat10 = Is_Cat10,
                                                Is_Cat10_Time = Is_Cat10_Time,
                                                Is_Cat11 = Is_Cat11,
                                                Is_Cat11_Time = Is_Cat11_Time,
                                                Is_Cat12 = Is_Cat12,
                                                Is_Cat12_Time = Is_Cat12_Time,
                                                Is_Cat13 = Is_Cat13,
                                                Is_Cat13_Time = Is_Cat13_Time,
                                                Is_Cat14 = Is_Cat14,
                                                Is_Cat14_Time = Is_Cat14_Time,
                                                Is_Cat15 = Is_Cat15,
                                                Is_Cat15_Time = Is_Cat15_Time,
                                                Is_Cat16 = Is_Cat16,
                                                Is_Cat16_Time = Is_Cat16_Time,
                                                Is_Cat17 = Is_Cat17,
                                                Is_Cat17_Time = Is_Cat17_Time,
                                                Is_Cat18 = Is_Cat18,
                                                Is_Cat18_Time = Is_Cat18_Time,
                                                Is_Cat19 = Is_Cat19,
                                                Is_Cat19_Time = Is_Cat19_Time,
                                                Is_Cat20 = Is_Cat20,
                                                Is_Cat20_Time = Is_Cat20_Time,

                                                Is_Sakit_Keterangan = Is_Sakit_Keterangan,
                                                Is_Izin_Keterangan = Is_Izin_Keterangan,
                                                Is_Alpa_Keterangan = Is_Alpa_Keterangan,

                                                Is_Cat01_Keterangan = Is_Cat01_Keterangan,
                                                Is_Cat02_Keterangan = Is_Cat02_Keterangan,
                                                Is_Cat03_Keterangan = Is_Cat03_Keterangan,
                                                Is_Cat04_Keterangan = Is_Cat04_Keterangan,
                                                Is_Cat05_Keterangan = Is_Cat05_Keterangan,
                                                Is_Cat06_Keterangan = Is_Cat06_Keterangan,
                                                Is_Cat07_Keterangan = Is_Cat07_Keterangan,
                                                Is_Cat08_Keterangan = Is_Cat08_Keterangan,
                                                Is_Cat09_Keterangan = Is_Cat09_Keterangan,
                                                Is_Cat10_Keterangan = Is_Cat10_Keterangan,
                                                Is_Cat11_Keterangan = Is_Cat11_Keterangan,
                                                Is_Cat12_Keterangan = Is_Cat12_Keterangan,
                                                Is_Cat13_Keterangan = Is_Cat13_Keterangan,
                                                Is_Cat14_Keterangan = Is_Cat14_Keterangan,
                                                Is_Cat15_Keterangan = Is_Cat15_Keterangan,
                                                Is_Cat16_Keterangan = Is_Cat16_Keterangan,
                                                Is_Cat17_Keterangan = Is_Cat17_Keterangan,
                                                Is_Cat18_Keterangan = Is_Cat18_Keterangan,
                                                Is_Cat19_Keterangan = Is_Cat19_Keterangan,
                                                Is_Cat20_Keterangan = Is_Cat20_Keterangan
                                            }, s_ssid
                                        );

                                        ada_absen = true;
                                    }
                                }
                                if (!ada_absen)
                                {
                                    DAO_SiswaAbsen.Insert(
                                            new SiswaAbsen
                                            {
                                                Kode = kode_absen,
                                                TahunAjaran = tahun_ajaran,
                                                Semester = Libs.GetSemesterByTanggal(dt_tanggal).ToString(),
                                                Rel_Sekolah = m_kelas.Rel_Sekolah,
                                                Rel_KelasDet = new Guid(rel_kelasdet),
                                                Tanggal = dt_tanggal,
                                                Rel_Siswa = siswa,
                                                Absen = absen.Substring(0, 1).ToUpper(),
                                                Keterangan = keterangan,
                                                Rel_Guru = s_ssid,
                                                Rel_Linimasa = new Guid(linimasa),
                                                Kejadian = kejadian,
                                                ButirSikap = butir_sikap,
                                                ButirSikapLain = butir_sikap_lain,
                                                Sikap = sikap,
                                                TindakLanjut = tindak_lanjut,

                                                Is_Hadir = Is_Hadir,
                                                Is_Hadir_Time = Is_Hadir_Time,
                                                Is_Sakit = Is_Sakit,
                                                Is_Sakit_Time = Is_Sakit_Time,
                                                Is_Izin = Is_Izin,
                                                Is_Izin_Time = Is_Izin_Time,
                                                Is_Alpa = Is_Alpa,
                                                Is_Alpa_Time = Is_Alpa_Time,
                                                Is_Cat01 = Is_Cat01,
                                                Is_Cat01_Time = Is_Cat01_Time,
                                                Is_Cat02 = Is_Cat02,
                                                Is_Cat02_Time = Is_Cat02_Time,
                                                Is_Cat03 = Is_Cat03,
                                                Is_Cat03_Time = Is_Cat03_Time,
                                                Is_Cat04 = Is_Cat04,
                                                Is_Cat04_Time = Is_Cat04_Time,
                                                Is_Cat05 = Is_Cat05,
                                                Is_Cat05_Time = Is_Cat05_Time,
                                                Is_Cat06 = Is_Cat06,
                                                Is_Cat06_Time = Is_Cat06_Time,
                                                Is_Cat07 = Is_Cat07,
                                                Is_Cat07_Time = Is_Cat07_Time,
                                                Is_Cat08 = Is_Cat08,
                                                Is_Cat08_Time = Is_Cat08_Time,
                                                Is_Cat09 = Is_Cat09,
                                                Is_Cat09_Time = Is_Cat09_Time,
                                                Is_Cat10 = Is_Cat10,
                                                Is_Cat10_Time = Is_Cat10_Time,
                                                Is_Cat11 = Is_Cat11,
                                                Is_Cat11_Time = Is_Cat11_Time,
                                                Is_Cat12 = Is_Cat12,
                                                Is_Cat12_Time = Is_Cat12_Time,
                                                Is_Cat13 = Is_Cat13,
                                                Is_Cat13_Time = Is_Cat13_Time,
                                                Is_Cat14 = Is_Cat14,
                                                Is_Cat14_Time = Is_Cat14_Time,
                                                Is_Cat15 = Is_Cat15,
                                                Is_Cat15_Time = Is_Cat15_Time,
                                                Is_Cat16 = Is_Cat16,
                                                Is_Cat16_Time = Is_Cat16_Time,
                                                Is_Cat17 = Is_Cat17,
                                                Is_Cat17_Time = Is_Cat17_Time,
                                                Is_Cat18 = Is_Cat18,
                                                Is_Cat18_Time = Is_Cat18_Time,
                                                Is_Cat19 = Is_Cat19,
                                                Is_Cat19_Time = Is_Cat19_Time,
                                                Is_Cat20 = Is_Cat20,
                                                Is_Cat20_Time = Is_Cat20_Time,
                                                
                                                Is_Sakit_Keterangan = Is_Sakit_Keterangan,
                                                Is_Izin_Keterangan = Is_Izin_Keterangan,
                                                Is_Alpa_Keterangan = Is_Alpa_Keterangan,

                                                Is_Cat01_Keterangan = Is_Cat01_Keterangan,
                                                Is_Cat02_Keterangan = Is_Cat02_Keterangan,
                                                Is_Cat03_Keterangan = Is_Cat03_Keterangan,
                                                Is_Cat04_Keterangan = Is_Cat04_Keterangan,
                                                Is_Cat05_Keterangan = Is_Cat05_Keterangan,
                                                Is_Cat06_Keterangan = Is_Cat06_Keterangan,
                                                Is_Cat07_Keterangan = Is_Cat07_Keterangan,
                                                Is_Cat08_Keterangan = Is_Cat08_Keterangan,
                                                Is_Cat09_Keterangan = Is_Cat09_Keterangan,
                                                Is_Cat10_Keterangan = Is_Cat10_Keterangan,
                                                Is_Cat11_Keterangan = Is_Cat11_Keterangan,
                                                Is_Cat12_Keterangan = Is_Cat12_Keterangan,
                                                Is_Cat13_Keterangan = Is_Cat13_Keterangan,
                                                Is_Cat14_Keterangan = Is_Cat14_Keterangan,
                                                Is_Cat15_Keterangan = Is_Cat15_Keterangan,
                                                Is_Cat16_Keterangan = Is_Cat16_Keterangan,
                                                Is_Cat17_Keterangan = Is_Cat17_Keterangan,
                                                Is_Cat18_Keterangan = Is_Cat18_Keterangan,
                                                Is_Cat19_Keterangan = Is_Cat19_Keterangan,
                                                Is_Cat20_Keterangan = Is_Cat20_Keterangan
                                            }, s_ssid
                                        );
                                }
                            }
                            else
                            {
                                //get lini masa && insert jika belum ada
                                if (
                                    m_linimasa != null
                                )
                                {
                                    if (m_linimasa.Jenis != null)
                                    {
                                        DAO_LinimasaKelas.Update(new LinimasaKelas
                                        {
                                            Kode = new Guid(linimasa),
                                            Jenis = Libs.JENIS_LINIMASA.ABSEN_SISWA_MAPEL,
                                            Rel_KelasDet = rel_kelasdet,
                                            TahunAjaran = tahun_ajaran,
                                            Keterangan = Libs.GetTanggalIndonesiaFromDate(dt_tanggal, false),
                                            ACT_KETERANGAN = act_ket
                                        }, s_ssid);

                                        ada_linimasa = true;
                                    }
                                }

                                if (!ada_linimasa)
                                {
                                    DAO_LinimasaKelas.Insert(new LinimasaKelas
                                    {
                                        Kode = new Guid(linimasa),
                                        Jenis = Libs.JENIS_LINIMASA.ABSEN_SISWA_MAPEL,
                                        Tanggal = DateTime.Now,
                                        Rel_KelasDet = rel_kelasdet,
                                        TahunAjaran = tahun_ajaran,
                                        Keterangan = Libs.GetTanggalIndonesiaFromDate(dt_tanggal, false),
                                        ACT = GetValidateAbsen(
                                                Libs.GetTanggalIndonesiaFromDate(
                                                      new DateTime(dt_tanggal.Year, dt_tanggal.Month, dt_tanggal.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
                                                    , false),
                                                rel_mapel, rel_kelasdet, s_ssid
                                              ),
                                        ACT_KETERANGAN = act_ket,
                                        RTG_UNIT = m_routing_data.Unit,
                                        RTG_LEVEL = m_routing_data.Level,
                                        RTG_KELAS = m_routing_data.Kelas,
                                        RTG_SEMESTER = Libs.GetSemesterByTanggal(dt_tanggal).ToString(),
                                        RTG_SUBKELAS = m_routing_data.SubKelas
                                    }, s_ssid);
                                }
                                //end get lini masa && insert jika belum ada

                                //data siswa absen
                                SiswaAbsenMapel m_absen = DAO_SiswaAbsenMapel.GetAllBySekolahByKelasDetBySiswaByMapelByTanggal_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelasdet,
                                        siswa,
                                        rel_mapel,
                                        dt_tanggal
                                    ).FirstOrDefault();
                                
                                string[] arr_jam = jam_ke.Split(new string[] { "-" }, StringSplitOptions.None);
                                string jam_awal = "";
                                string jam_akhir = "";
                                if (arr_jam.Length == 2)
                                {
                                    jam_awal = arr_jam[0];
                                    jam_akhir = arr_jam[1];
                                }
                                else
                                {
                                    jam_awal = jam_ke.Trim();
                                    jam_akhir = jam_ke.Trim();
                                }

                                if (m_absen != null)
                                {
                                    if (m_absen.Absen != null)
                                    {
                                        kode_absen = m_absen.Kode;
                                        DAO_SiswaAbsenMapel.Update(
                                            new SiswaAbsenMapel
                                            {
                                                Kode = kode_absen,
                                                TahunAjaran = tahun_ajaran,
                                                Semester = Libs.GetSemesterByTanggal(dt_tanggal).ToString(),
                                                Rel_Sekolah = m_kelas.Rel_Sekolah,
                                                Rel_KelasDet = new Guid(rel_kelasdet),
                                                Tanggal = dt_tanggal,
                                                Rel_Siswa = siswa,
                                                Absen = absen.Substring(0, 1).ToUpper(),
                                                Keterangan = keterangan,
                                                Rel_Guru = s_ssid,
                                                Rel_Linimasa = new Guid(linimasa),
                                                Rel_Mapel = rel_mapel,
                                                JamAwal = jam_awal,
                                                JamAkhir = jam_akhir,
                                                Kejadian = kejadian,
                                                ButirSikap = butir_sikap,
                                                ButirSikapLain = butir_sikap_lain,
                                                Sikap = sikap,
                                                TindakLanjut = tindak_lanjut,

                                                Is_Hadir = Is_Hadir,
                                                Is_Hadir_Time = Is_Hadir_Time,
                                                Is_Sakit = Is_Sakit,
                                                Is_Sakit_Time = Is_Sakit_Time,
                                                Is_Izin = Is_Izin,
                                                Is_Izin_Time = Is_Izin_Time,
                                                Is_Alpa = Is_Alpa,
                                                Is_Alpa_Time = Is_Alpa_Time,
                                                Is_Cat01 = Is_Cat01,
                                                Is_Cat01_Time = Is_Cat01_Time,
                                                Is_Cat02 = Is_Cat02,
                                                Is_Cat02_Time = Is_Cat02_Time,
                                                Is_Cat03 = Is_Cat03,
                                                Is_Cat03_Time = Is_Cat03_Time,
                                                Is_Cat04 = Is_Cat04,
                                                Is_Cat04_Time = Is_Cat04_Time,
                                                Is_Cat05 = Is_Cat05,
                                                Is_Cat05_Time = Is_Cat05_Time,
                                                Is_Cat06 = Is_Cat06,
                                                Is_Cat06_Time = Is_Cat06_Time,
                                                Is_Cat07 = Is_Cat07,
                                                Is_Cat07_Time = Is_Cat07_Time,
                                                Is_Cat08 = Is_Cat08,
                                                Is_Cat08_Time = Is_Cat08_Time,
                                                Is_Cat09 = Is_Cat09,
                                                Is_Cat09_Time = Is_Cat09_Time,
                                                Is_Cat10 = Is_Cat10,
                                                Is_Cat10_Time = Is_Cat10_Time,
                                                Is_Cat11 = Is_Cat11,
                                                Is_Cat11_Time = Is_Cat11_Time,
                                                Is_Cat12 = Is_Cat12,
                                                Is_Cat12_Time = Is_Cat12_Time,
                                                Is_Cat13 = Is_Cat13,
                                                Is_Cat13_Time = Is_Cat13_Time,
                                                Is_Cat14 = Is_Cat14,
                                                Is_Cat14_Time = Is_Cat14_Time,
                                                Is_Cat15 = Is_Cat15,
                                                Is_Cat15_Time = Is_Cat15_Time,
                                                Is_Cat16 = Is_Cat16,
                                                Is_Cat16_Time = Is_Cat16_Time,
                                                Is_Cat17 = Is_Cat17,
                                                Is_Cat17_Time = Is_Cat17_Time,
                                                Is_Cat18 = Is_Cat18,
                                                Is_Cat18_Time = Is_Cat18_Time,
                                                Is_Cat19 = Is_Cat19,
                                                Is_Cat19_Time = Is_Cat19_Time,
                                                Is_Cat20 = Is_Cat20,
                                                Is_Cat20_Time = Is_Cat20_Time,

                                                Is_Sakit_Keterangan = Is_Sakit_Keterangan,
                                                Is_Izin_Keterangan = Is_Izin_Keterangan,
                                                Is_Alpa_Keterangan = Is_Alpa_Keterangan,

                                                Is_Cat01_Keterangan = Is_Cat01_Keterangan,
                                                Is_Cat02_Keterangan = Is_Cat02_Keterangan,
                                                Is_Cat03_Keterangan = Is_Cat03_Keterangan,
                                                Is_Cat04_Keterangan = Is_Cat04_Keterangan,
                                                Is_Cat05_Keterangan = Is_Cat05_Keterangan,
                                                Is_Cat06_Keterangan = Is_Cat06_Keterangan,
                                                Is_Cat07_Keterangan = Is_Cat07_Keterangan,
                                                Is_Cat08_Keterangan = Is_Cat08_Keterangan,
                                                Is_Cat09_Keterangan = Is_Cat09_Keterangan,
                                                Is_Cat10_Keterangan = Is_Cat10_Keterangan,
                                                Is_Cat11_Keterangan = Is_Cat11_Keterangan,
                                                Is_Cat12_Keterangan = Is_Cat12_Keterangan,
                                                Is_Cat13_Keterangan = Is_Cat13_Keterangan,
                                                Is_Cat14_Keterangan = Is_Cat14_Keterangan,
                                                Is_Cat15_Keterangan = Is_Cat15_Keterangan,
                                                Is_Cat16_Keterangan = Is_Cat16_Keterangan,
                                                Is_Cat17_Keterangan = Is_Cat17_Keterangan,
                                                Is_Cat18_Keterangan = Is_Cat18_Keterangan,
                                                Is_Cat19_Keterangan = Is_Cat19_Keterangan,
                                                Is_Cat20_Keterangan = Is_Cat20_Keterangan
                                            }, s_ssid
                                        );

                                        ada_absen = true;
                                    }
                                }
                                if (!ada_absen)
                                {
                                    DAO_SiswaAbsenMapel.Insert(
                                            new SiswaAbsenMapel
                                            {
                                                Kode = kode_absen,
                                                TahunAjaran = tahun_ajaran,
                                                Semester = Libs.GetSemesterByTanggal(dt_tanggal).ToString(),
                                                Rel_Sekolah = m_kelas.Rel_Sekolah,
                                                Rel_KelasDet = new Guid(rel_kelasdet),
                                                Tanggal = dt_tanggal,
                                                Rel_Siswa = siswa,
                                                Absen = absen.Substring(0, 1).ToUpper(),
                                                Keterangan = keterangan,
                                                Rel_Guru = s_ssid,
                                                Rel_Linimasa = new Guid(linimasa),
                                                Rel_Mapel = rel_mapel,
                                                JamAwal = jam_awal,
                                                JamAkhir = jam_akhir,
                                                Kejadian = kejadian,
                                                ButirSikap = butir_sikap,
                                                ButirSikapLain = butir_sikap_lain,
                                                Sikap = sikap,
                                                TindakLanjut = tindak_lanjut,

                                                Is_Hadir = Is_Hadir,
                                                Is_Hadir_Time = Is_Hadir_Time,
                                                Is_Sakit = Is_Sakit,
                                                Is_Sakit_Time = Is_Sakit_Time,
                                                Is_Izin = Is_Izin,
                                                Is_Izin_Time = Is_Izin_Time,
                                                Is_Alpa = Is_Alpa,
                                                Is_Alpa_Time = Is_Alpa_Time,
                                                Is_Cat01 = Is_Cat01,
                                                Is_Cat01_Time = Is_Cat01_Time,
                                                Is_Cat02 = Is_Cat02,
                                                Is_Cat02_Time = Is_Cat02_Time,
                                                Is_Cat03 = Is_Cat03,
                                                Is_Cat03_Time = Is_Cat03_Time,
                                                Is_Cat04 = Is_Cat04,
                                                Is_Cat04_Time = Is_Cat04_Time,
                                                Is_Cat05 = Is_Cat05,
                                                Is_Cat05_Time = Is_Cat05_Time,
                                                Is_Cat06 = Is_Cat06,
                                                Is_Cat06_Time = Is_Cat06_Time,
                                                Is_Cat07 = Is_Cat07,
                                                Is_Cat07_Time = Is_Cat07_Time,
                                                Is_Cat08 = Is_Cat08,
                                                Is_Cat08_Time = Is_Cat08_Time,
                                                Is_Cat09 = Is_Cat09,
                                                Is_Cat09_Time = Is_Cat09_Time,
                                                Is_Cat10 = Is_Cat10,
                                                Is_Cat10_Time = Is_Cat10_Time,
                                                Is_Cat11 = Is_Cat11,
                                                Is_Cat11_Time = Is_Cat11_Time,
                                                Is_Cat12 = Is_Cat12,
                                                Is_Cat12_Time = Is_Cat12_Time,
                                                Is_Cat13 = Is_Cat13,
                                                Is_Cat13_Time = Is_Cat13_Time,
                                                Is_Cat14 = Is_Cat14,
                                                Is_Cat14_Time = Is_Cat14_Time,
                                                Is_Cat15 = Is_Cat15,
                                                Is_Cat15_Time = Is_Cat15_Time,
                                                Is_Cat16 = Is_Cat16,
                                                Is_Cat16_Time = Is_Cat16_Time,
                                                Is_Cat17 = Is_Cat17,
                                                Is_Cat17_Time = Is_Cat17_Time,
                                                Is_Cat18 = Is_Cat18,
                                                Is_Cat18_Time = Is_Cat18_Time,
                                                Is_Cat19 = Is_Cat19,
                                                Is_Cat19_Time = Is_Cat19_Time,
                                                Is_Cat20 = Is_Cat20,
                                                Is_Cat20_Time = Is_Cat20_Time,

                                                Is_Sakit_Keterangan = Is_Sakit_Keterangan,
                                                Is_Izin_Keterangan = Is_Izin_Keterangan,
                                                Is_Alpa_Keterangan = Is_Alpa_Keterangan,

                                                Is_Cat01_Keterangan = Is_Cat01_Keterangan,
                                                Is_Cat02_Keterangan = Is_Cat02_Keterangan,
                                                Is_Cat03_Keterangan = Is_Cat03_Keterangan,
                                                Is_Cat04_Keterangan = Is_Cat04_Keterangan,
                                                Is_Cat05_Keterangan = Is_Cat05_Keterangan,
                                                Is_Cat06_Keterangan = Is_Cat06_Keterangan,
                                                Is_Cat07_Keterangan = Is_Cat07_Keterangan,
                                                Is_Cat08_Keterangan = Is_Cat08_Keterangan,
                                                Is_Cat09_Keterangan = Is_Cat09_Keterangan,
                                                Is_Cat10_Keterangan = Is_Cat10_Keterangan,
                                                Is_Cat11_Keterangan = Is_Cat11_Keterangan,
                                                Is_Cat12_Keterangan = Is_Cat12_Keterangan,
                                                Is_Cat13_Keterangan = Is_Cat13_Keterangan,
                                                Is_Cat14_Keterangan = Is_Cat14_Keterangan,
                                                Is_Cat15_Keterangan = Is_Cat15_Keterangan,
                                                Is_Cat16_Keterangan = Is_Cat16_Keterangan,
                                                Is_Cat17_Keterangan = Is_Cat17_Keterangan,
                                                Is_Cat18_Keterangan = Is_Cat18_Keterangan,
                                                Is_Cat19_Keterangan = Is_Cat19_Keterangan,
                                                Is_Cat20_Keterangan = Is_Cat20_Keterangan
                                            }, s_ssid
                                        );
                                }
                            }
                            //end data siswa absen
                        }
                    }
                }
            }

            hasil.Add("1");
            return hasil.ToArray();
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] Do_V1(
                string sw,
                string a,
                string k,
                string t,
                string s,
                string kd,
                string tgl,
                string lm,
                string m,
                string jk,
                string kj,
                string bs,
                string bsl,
                string skp,
                string tl,
                string act_ket,
                string ssid
            )
        {
            List<string> hasil = new List<string>();

            string siswa = sw;
            string absen = a;
            string keterangan = k;
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(t);
            string semester = s;
            string rel_kelasdet = kd;
            string tanggal = tgl;
            string linimasa = lm;
            string rel_mapel = m;
            string jam_ke = jk;
            string kejadian = kj;
            string butir_sikap = bs;
            string butir_sikap_lain = bsl;
            string sikap = (skp == "plus" ? "+" : skp);
            string tindak_lanjut = tl;
            string keterangan_act = act_ket;
            string s_ssid = Libs.Decryptdata(ssid);

            bool ada_linimasa = false;

            //parse absen
            string Is_Hadir = "";
            DateTime Is_Hadir_Time = DateTime.MinValue;
            string Is_Sakit = "";
            DateTime Is_Sakit_Time = DateTime.MinValue;
            string Is_Izin = "";
            DateTime Is_Izin_Time = DateTime.MinValue;
            string Is_Alpa = "";
            DateTime Is_Alpa_Time = DateTime.MinValue;

            string Is_Cat01 = "";
            DateTime Is_Cat01_Time = DateTime.MinValue;
            string Is_Cat02 = "";
            DateTime Is_Cat02_Time = DateTime.MinValue;
            string Is_Cat03 = "";
            DateTime Is_Cat03_Time = DateTime.MinValue;
            string Is_Cat04 = "";
            DateTime Is_Cat04_Time = DateTime.MinValue;
            string Is_Cat05 = "";
            DateTime Is_Cat05_Time = DateTime.MinValue;
            string Is_Cat06 = "";
            DateTime Is_Cat06_Time = DateTime.MinValue;
            string Is_Cat07 = "";
            DateTime Is_Cat07_Time = DateTime.MinValue;
            string Is_Cat08 = "";
            DateTime Is_Cat08_Time = DateTime.MinValue;
            string Is_Cat09 = "";
            DateTime Is_Cat09_Time = DateTime.MinValue;
            string Is_Cat10 = "";
            DateTime Is_Cat10_Time = DateTime.MinValue;
            string Is_Cat11 = "";
            DateTime Is_Cat11_Time = DateTime.MinValue;
            string Is_Cat12 = "";
            DateTime Is_Cat12_Time = DateTime.MinValue;
            string Is_Cat13 = "";
            DateTime Is_Cat13_Time = DateTime.MinValue;
            string Is_Cat14 = "";
            DateTime Is_Cat14_Time = DateTime.MinValue;
            string Is_Cat15 = "";
            DateTime Is_Cat15_Time = DateTime.MinValue;
            string Is_Cat16 = "";
            DateTime Is_Cat16_Time = DateTime.MinValue;
            string Is_Cat17 = "";
            DateTime Is_Cat17_Time = DateTime.MinValue;
            string Is_Cat18 = "";
            DateTime Is_Cat18_Time = DateTime.MinValue;
            string Is_Cat19 = "";
            DateTime Is_Cat19_Time = DateTime.MinValue;
            string Is_Cat20 = "";
            DateTime Is_Cat20_Time = DateTime.MinValue;

            KelasDet m_kelasdet = DAO_KelasDet.GetByID_Entity(rel_kelasdet);
            if (m_kelasdet != null)
            {
                if (m_kelasdet.Nama != null)
                {
                    ERoutingData m_routing_data = RoutingData.GetRoutingByKelasDet(rel_kelasdet);
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelasdet.Rel_Kelas.ToString());

                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            DateTime dt_tanggal = Libs.GetDateFromTanggalIndonesiaStr(tanggal);
                            LinimasaKelas m_linimasa = DAO_LinimasaKelas.GetByID_Entity(linimasa);
                            ada_linimasa = false;

                            Guid kode_absen = Guid.NewGuid();
                            bool ada_absen = false;
                            if (rel_mapel.Trim() == "")
                            {
                                //get lini masa && insert jika belum ada
                                if (
                                    m_linimasa != null
                                )
                                {
                                    if (m_linimasa.Jenis != null)
                                    {
                                        DAO_LinimasaKelas.Update(new LinimasaKelas
                                        {
                                            Kode = new Guid(linimasa),
                                            Jenis = Libs.JENIS_LINIMASA.ABSEN_SISWA_HARIAN,
                                            Rel_KelasDet = rel_kelasdet,
                                            TahunAjaran = tahun_ajaran,
                                            Keterangan = Libs.GetTanggalIndonesiaFromDate(dt_tanggal, false),
                                            ACT_KETERANGAN = act_ket
                                        }, s_ssid);

                                        ada_linimasa = true;
                                    }
                                }

                                if (!ada_linimasa)
                                {
                                    DAO_LinimasaKelas.Insert(new LinimasaKelas
                                    {
                                        Kode = new Guid(linimasa),
                                        Jenis = Libs.JENIS_LINIMASA.ABSEN_SISWA_HARIAN,
                                        Tanggal = DateTime.Now,
                                        Rel_KelasDet = rel_kelasdet,
                                        TahunAjaran = tahun_ajaran,
                                        Keterangan = Libs.GetTanggalIndonesiaFromDate(dt_tanggal, false),
                                        ACT = GetValidateAbsen(
                                                Libs.GetTanggalIndonesiaFromDate(
                                                      new DateTime(dt_tanggal.Year, dt_tanggal.Month, dt_tanggal.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
                                                    , false),
                                                rel_mapel, rel_kelasdet, s_ssid
                                              ),
                                        ACT_KETERANGAN = act_ket,
                                        RTG_UNIT = m_routing_data.Unit,
                                        RTG_LEVEL = m_routing_data.Level,
                                        RTG_KELAS = m_routing_data.Kelas,
                                        RTG_SEMESTER = Libs.GetSemesterByTanggal(dt_tanggal).ToString(),
                                        RTG_SUBKELAS = m_routing_data.SubKelas
                                    }, s_ssid);
                                }
                                //end get lini masa && insert jika belum ada

                                //data siswa absen
                                SiswaAbsen m_absen = DAO_SiswaAbsen.GetAllBySekolahByKelasDetBySiswaByTanggal_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelasdet,
                                        siswa,
                                        dt_tanggal
                                    ).FirstOrDefault();

                                if (m_absen != null)
                                {
                                    if (m_absen.Absen != null)
                                    {
                                        kode_absen = m_absen.Kode;
                                        DAO_SiswaAbsen.Update(
                                            new SiswaAbsen
                                            {
                                                Kode = kode_absen,
                                                TahunAjaran = tahun_ajaran,
                                                Semester = Libs.GetSemesterByTanggal(dt_tanggal).ToString(),
                                                Rel_Sekolah = m_kelas.Rel_Sekolah,
                                                Rel_KelasDet = new Guid(rel_kelasdet),
                                                Tanggal = dt_tanggal,
                                                Rel_Siswa = siswa,
                                                Absen = absen.Substring(0, 1).ToUpper(),
                                                Keterangan = keterangan,
                                                Rel_Guru = s_ssid,
                                                Rel_Linimasa = new Guid(linimasa),
                                                Kejadian = kejadian,
                                                ButirSikap = butir_sikap,
                                                ButirSikapLain = butir_sikap_lain,
                                                Sikap = sikap,
                                                TindakLanjut = tindak_lanjut,
                                                
                                                Is_Hadir = Is_Hadir,
                                                Is_Hadir_Time = Is_Hadir_Time,
                                                Is_Sakit = Is_Sakit,
                                                Is_Sakit_Time = Is_Sakit_Time,
                                                Is_Izin = Is_Izin,
                                                Is_Izin_Time = Is_Izin_Time,
                                                Is_Alpa = Is_Alpa,
                                                Is_Alpa_Time = Is_Alpa_Time,
                                                Is_Cat01 = Is_Cat01,
                                                Is_Cat01_Time = Is_Cat01_Time,
                                                Is_Cat02 = Is_Cat02,
                                                Is_Cat02_Time = Is_Cat02_Time,
                                                Is_Cat03 = Is_Cat03,
                                                Is_Cat03_Time = Is_Cat03_Time,
                                                Is_Cat04 = Is_Cat04,
                                                Is_Cat04_Time = Is_Cat04_Time,
                                                Is_Cat05 = Is_Cat05,
                                                Is_Cat05_Time = Is_Cat05_Time,
                                                Is_Cat06 = Is_Cat06,
                                                Is_Cat06_Time = Is_Cat06_Time,
                                                Is_Cat07 = Is_Cat07,
                                                Is_Cat07_Time = Is_Cat07_Time,
                                                Is_Cat08 = Is_Cat08,
                                                Is_Cat08_Time = Is_Cat08_Time,
                                                Is_Cat09 = Is_Cat09,
                                                Is_Cat09_Time = Is_Cat09_Time,
                                                Is_Cat10 = Is_Cat10,
                                                Is_Cat10_Time = Is_Cat10_Time,
                                                Is_Cat11 = Is_Cat11,
                                                Is_Cat11_Time = Is_Cat11_Time,
                                                Is_Cat12 = Is_Cat12,
                                                Is_Cat12_Time = Is_Cat12_Time,
                                                Is_Cat13 = Is_Cat13,
                                                Is_Cat13_Time = Is_Cat13_Time,
                                                Is_Cat14 = Is_Cat14,
                                                Is_Cat14_Time = Is_Cat14_Time,
                                                Is_Cat15 = Is_Cat15,
                                                Is_Cat15_Time = Is_Cat15_Time,
                                                Is_Cat16 = Is_Cat16,
                                                Is_Cat16_Time = Is_Cat16_Time,
                                                Is_Cat17 = Is_Cat17,
                                                Is_Cat17_Time = Is_Cat17_Time,
                                                Is_Cat18 = Is_Cat18,
                                                Is_Cat18_Time = Is_Cat18_Time,
                                                Is_Cat19 = Is_Cat19,
                                                Is_Cat19_Time = Is_Cat19_Time,
                                                Is_Cat20 = Is_Cat20,
                                                Is_Cat20_Time = Is_Cat20_Time
                                            }, s_ssid
                                        );

                                        ada_absen = true;
                                    }
                                }
                                if (!ada_absen)
                                {
                                    DAO_SiswaAbsen.Insert(
                                            new SiswaAbsen
                                            {
                                                Kode = kode_absen,
                                                TahunAjaran = tahun_ajaran,
                                                Semester = Libs.GetSemesterByTanggal(dt_tanggal).ToString(),
                                                Rel_Sekolah = m_kelas.Rel_Sekolah,
                                                Rel_KelasDet = new Guid(rel_kelasdet),
                                                Tanggal = dt_tanggal,
                                                Rel_Siswa = siswa,
                                                Absen = absen.Substring(0, 1).ToUpper(),
                                                Keterangan = keterangan,
                                                Rel_Guru = s_ssid,
                                                Rel_Linimasa = new Guid(linimasa),
                                                Kejadian = kejadian,
                                                ButirSikap = butir_sikap,
                                                ButirSikapLain = butir_sikap_lain,
                                                Sikap = sikap,
                                                TindakLanjut = tindak_lanjut,


                                                Is_Hadir = Is_Hadir,
                                                Is_Hadir_Time = Is_Hadir_Time,
                                                Is_Sakit = Is_Sakit,
                                                Is_Sakit_Time = Is_Sakit_Time,
                                                Is_Izin = Is_Izin,
                                                Is_Izin_Time = Is_Izin_Time,
                                                Is_Alpa = Is_Alpa,
                                                Is_Alpa_Time = Is_Alpa_Time,
                                                Is_Cat01 = Is_Cat01,
                                                Is_Cat01_Time = Is_Cat01_Time,
                                                Is_Cat02 = Is_Cat02,
                                                Is_Cat02_Time = Is_Cat02_Time,
                                                Is_Cat03 = Is_Cat03,
                                                Is_Cat03_Time = Is_Cat03_Time,
                                                Is_Cat04 = Is_Cat04,
                                                Is_Cat04_Time = Is_Cat04_Time,
                                                Is_Cat05 = Is_Cat05,
                                                Is_Cat05_Time = Is_Cat05_Time,
                                                Is_Cat06 = Is_Cat06,
                                                Is_Cat06_Time = Is_Cat06_Time,
                                                Is_Cat07 = Is_Cat07,
                                                Is_Cat07_Time = Is_Cat07_Time,
                                                Is_Cat08 = Is_Cat08,
                                                Is_Cat08_Time = Is_Cat08_Time,
                                                Is_Cat09 = Is_Cat09,
                                                Is_Cat09_Time = Is_Cat09_Time,
                                                Is_Cat10 = Is_Cat10,
                                                Is_Cat10_Time = Is_Cat10_Time,
                                                Is_Cat11 = Is_Cat11,
                                                Is_Cat11_Time = Is_Cat11_Time,
                                                Is_Cat12 = Is_Cat12,
                                                Is_Cat12_Time = Is_Cat12_Time,
                                                Is_Cat13 = Is_Cat13,
                                                Is_Cat13_Time = Is_Cat13_Time,
                                                Is_Cat14 = Is_Cat14,
                                                Is_Cat14_Time = Is_Cat14_Time,
                                                Is_Cat15 = Is_Cat15,
                                                Is_Cat15_Time = Is_Cat15_Time,
                                                Is_Cat16 = Is_Cat16,
                                                Is_Cat16_Time = Is_Cat16_Time,
                                                Is_Cat17 = Is_Cat17,
                                                Is_Cat17_Time = Is_Cat17_Time,
                                                Is_Cat18 = Is_Cat18,
                                                Is_Cat18_Time = Is_Cat18_Time,
                                                Is_Cat19 = Is_Cat19,
                                                Is_Cat19_Time = Is_Cat19_Time,
                                                Is_Cat20 = Is_Cat20,
                                                Is_Cat20_Time = Is_Cat20_Time
                                            }, s_ssid
                                        );
                                }
                            }
                            else
                            {
                                //get lini masa && insert jika belum ada
                                if (
                                    m_linimasa != null
                                )
                                {
                                    if (m_linimasa.Jenis != null)
                                    {
                                        DAO_LinimasaKelas.Update(new LinimasaKelas
                                        {
                                            Kode = new Guid(linimasa),
                                            Jenis = Libs.JENIS_LINIMASA.ABSEN_SISWA_MAPEL,
                                            Rel_KelasDet = rel_kelasdet,
                                            TahunAjaran = tahun_ajaran,
                                            Keterangan = Libs.GetTanggalIndonesiaFromDate(dt_tanggal, false),
                                            ACT_KETERANGAN = act_ket
                                        }, s_ssid);

                                        ada_linimasa = true;
                                    }
                                }

                                if (!ada_linimasa)
                                {
                                    DAO_LinimasaKelas.Insert(new LinimasaKelas
                                    {
                                        Kode = new Guid(linimasa),
                                        Jenis = Libs.JENIS_LINIMASA.ABSEN_SISWA_MAPEL,
                                        Tanggal = DateTime.Now,
                                        Rel_KelasDet = rel_kelasdet,
                                        TahunAjaran = tahun_ajaran,
                                        Keterangan = Libs.GetTanggalIndonesiaFromDate(dt_tanggal, false),
                                        ACT = GetValidateAbsen(
                                                Libs.GetTanggalIndonesiaFromDate(
                                                      new DateTime(dt_tanggal.Year, dt_tanggal.Month, dt_tanggal.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
                                                    , false),
                                                rel_mapel, rel_kelasdet, s_ssid
                                              ),
                                        ACT_KETERANGAN = act_ket,
                                        RTG_UNIT = m_routing_data.Unit,
                                        RTG_LEVEL = m_routing_data.Level,
                                        RTG_KELAS = m_routing_data.Kelas,
                                        RTG_SEMESTER = Libs.GetSemesterByTanggal(dt_tanggal).ToString(),
                                        RTG_SUBKELAS = m_routing_data.SubKelas
                                    }, s_ssid);
                                }
                                //end get lini masa && insert jika belum ada

                                //data siswa absen
                                SiswaAbsenMapel m_absen = DAO_SiswaAbsenMapel.GetAllBySekolahByKelasDetBySiswaByMapelByTanggal_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelasdet,
                                        siswa,
                                        rel_mapel,
                                        dt_tanggal
                                    ).FirstOrDefault();

                                string[] arr_jam = jam_ke.Split(new string[] { "-" }, StringSplitOptions.None);
                                string jam_awal = "";
                                string jam_akhir = "";
                                if (arr_jam.Length == 2)
                                {
                                    jam_awal = arr_jam[0];
                                    jam_akhir = arr_jam[1];
                                }

                                if (m_absen != null)
                                {
                                    if (m_absen.Absen != null)
                                    {
                                        kode_absen = m_absen.Kode;
                                        DAO_SiswaAbsenMapel.Update(
                                            new SiswaAbsenMapel
                                            {
                                                Kode = kode_absen,
                                                TahunAjaran = tahun_ajaran,
                                                Semester = Libs.GetSemesterByTanggal(dt_tanggal).ToString(),
                                                Rel_Sekolah = m_kelas.Rel_Sekolah,
                                                Rel_KelasDet = new Guid(rel_kelasdet),
                                                Tanggal = dt_tanggal,
                                                Rel_Siswa = siswa,
                                                Absen = absen.Substring(0, 1).ToUpper(),
                                                Keterangan = keterangan,
                                                Rel_Guru = s_ssid,
                                                Rel_Linimasa = new Guid(linimasa),
                                                Rel_Mapel = rel_mapel,
                                                JamAwal = jam_awal,
                                                JamAkhir = jam_akhir,
                                                Kejadian = kejadian,
                                                ButirSikap = butir_sikap,
                                                ButirSikapLain = butir_sikap_lain,
                                                Sikap = sikap,
                                                TindakLanjut = tindak_lanjut,


                                                Is_Hadir = Is_Hadir,
                                                Is_Hadir_Time = Is_Hadir_Time,
                                                Is_Sakit = Is_Sakit,
                                                Is_Sakit_Time = Is_Sakit_Time,
                                                Is_Izin = Is_Izin,
                                                Is_Izin_Time = Is_Izin_Time,
                                                Is_Alpa = Is_Alpa,
                                                Is_Alpa_Time = Is_Alpa_Time,
                                                Is_Cat01 = Is_Cat01,
                                                Is_Cat01_Time = Is_Cat01_Time,
                                                Is_Cat02 = Is_Cat02,
                                                Is_Cat02_Time = Is_Cat02_Time,
                                                Is_Cat03 = Is_Cat03,
                                                Is_Cat03_Time = Is_Cat03_Time,
                                                Is_Cat04 = Is_Cat04,
                                                Is_Cat04_Time = Is_Cat04_Time,
                                                Is_Cat05 = Is_Cat05,
                                                Is_Cat05_Time = Is_Cat05_Time,
                                                Is_Cat06 = Is_Cat06,
                                                Is_Cat06_Time = Is_Cat06_Time,
                                                Is_Cat07 = Is_Cat07,
                                                Is_Cat07_Time = Is_Cat07_Time,
                                                Is_Cat08 = Is_Cat08,
                                                Is_Cat08_Time = Is_Cat08_Time,
                                                Is_Cat09 = Is_Cat09,
                                                Is_Cat09_Time = Is_Cat09_Time,
                                                Is_Cat10 = Is_Cat10,
                                                Is_Cat10_Time = Is_Cat10_Time,
                                                Is_Cat11 = Is_Cat11,
                                                Is_Cat11_Time = Is_Cat11_Time,
                                                Is_Cat12 = Is_Cat12,
                                                Is_Cat12_Time = Is_Cat12_Time,
                                                Is_Cat13 = Is_Cat13,
                                                Is_Cat13_Time = Is_Cat13_Time,
                                                Is_Cat14 = Is_Cat14,
                                                Is_Cat14_Time = Is_Cat14_Time,
                                                Is_Cat15 = Is_Cat15,
                                                Is_Cat15_Time = Is_Cat15_Time,
                                                Is_Cat16 = Is_Cat16,
                                                Is_Cat16_Time = Is_Cat16_Time,
                                                Is_Cat17 = Is_Cat17,
                                                Is_Cat17_Time = Is_Cat17_Time,
                                                Is_Cat18 = Is_Cat18,
                                                Is_Cat18_Time = Is_Cat18_Time,
                                                Is_Cat19 = Is_Cat19,
                                                Is_Cat19_Time = Is_Cat19_Time,
                                                Is_Cat20 = Is_Cat20,
                                                Is_Cat20_Time = Is_Cat20_Time,

                                                Is_Cat01_Keterangan = "",
                                                Is_Cat02_Keterangan = "",
                                                Is_Cat03_Keterangan = "",
                                                Is_Cat04_Keterangan = "",
                                                Is_Cat05_Keterangan = "",
                                                Is_Cat06_Keterangan = "",
                                                Is_Cat07_Keterangan = "",
                                                Is_Cat08_Keterangan = "",
                                                Is_Cat09_Keterangan = "",
                                                Is_Cat10_Keterangan = "",
                                                Is_Cat11_Keterangan = "",
                                                Is_Cat12_Keterangan = "",
                                                Is_Cat13_Keterangan = "",
                                                Is_Cat14_Keterangan = "",
                                                Is_Cat15_Keterangan = "",
                                                Is_Cat16_Keterangan = "",
                                                Is_Cat17_Keterangan = "",
                                                Is_Cat18_Keterangan = "",
                                                Is_Cat19_Keterangan = "",
                                                Is_Cat20_Keterangan = ""
                                            }, s_ssid
                                        );

                                        ada_absen = true;
                                    }
                                }
                                if (!ada_absen)
                                {
                                    DAO_SiswaAbsenMapel.Insert(
                                            new SiswaAbsenMapel
                                            {
                                                Kode = kode_absen,
                                                TahunAjaran = tahun_ajaran,
                                                Semester = Libs.GetSemesterByTanggal(dt_tanggal).ToString(),
                                                Rel_Sekolah = m_kelas.Rel_Sekolah,
                                                Rel_KelasDet = new Guid(rel_kelasdet),
                                                Tanggal = dt_tanggal,
                                                Rel_Siswa = siswa,
                                                Absen = absen.Substring(0, 1).ToUpper(),
                                                Keterangan = keterangan,
                                                Rel_Guru = s_ssid,
                                                Rel_Linimasa = new Guid(linimasa),
                                                Rel_Mapel = rel_mapel,
                                                JamAwal = jam_awal,
                                                JamAkhir = jam_akhir,
                                                Kejadian = kejadian,
                                                ButirSikap = butir_sikap,
                                                ButirSikapLain = butir_sikap_lain,
                                                Sikap = sikap,
                                                TindakLanjut = tindak_lanjut,
                                                
                                                Is_Hadir = Is_Hadir,
                                                Is_Hadir_Time = Is_Hadir_Time,
                                                Is_Sakit = Is_Sakit,
                                                Is_Sakit_Time = Is_Sakit_Time,
                                                Is_Izin = Is_Izin,
                                                Is_Izin_Time = Is_Izin_Time,
                                                Is_Alpa = Is_Alpa,
                                                Is_Alpa_Time = Is_Alpa_Time,
                                                Is_Cat01 = Is_Cat01,
                                                Is_Cat01_Time = Is_Cat01_Time,
                                                Is_Cat02 = Is_Cat02,
                                                Is_Cat02_Time = Is_Cat02_Time,
                                                Is_Cat03 = Is_Cat03,
                                                Is_Cat03_Time = Is_Cat03_Time,
                                                Is_Cat04 = Is_Cat04,
                                                Is_Cat04_Time = Is_Cat04_Time,
                                                Is_Cat05 = Is_Cat05,
                                                Is_Cat05_Time = Is_Cat05_Time,
                                                Is_Cat06 = Is_Cat06,
                                                Is_Cat06_Time = Is_Cat06_Time,
                                                Is_Cat07 = Is_Cat07,
                                                Is_Cat07_Time = Is_Cat07_Time,
                                                Is_Cat08 = Is_Cat08,
                                                Is_Cat08_Time = Is_Cat08_Time,
                                                Is_Cat09 = Is_Cat09,
                                                Is_Cat09_Time = Is_Cat09_Time,
                                                Is_Cat10 = Is_Cat10,
                                                Is_Cat10_Time = Is_Cat10_Time,
                                                Is_Cat11 = Is_Cat11,
                                                Is_Cat11_Time = Is_Cat11_Time,
                                                Is_Cat12 = Is_Cat12,
                                                Is_Cat12_Time = Is_Cat12_Time,
                                                Is_Cat13 = Is_Cat13,
                                                Is_Cat13_Time = Is_Cat13_Time,
                                                Is_Cat14 = Is_Cat14,
                                                Is_Cat14_Time = Is_Cat14_Time,
                                                Is_Cat15 = Is_Cat15,
                                                Is_Cat15_Time = Is_Cat15_Time,
                                                Is_Cat16 = Is_Cat16,
                                                Is_Cat16_Time = Is_Cat16_Time,
                                                Is_Cat17 = Is_Cat17,
                                                Is_Cat17_Time = Is_Cat17_Time,
                                                Is_Cat18 = Is_Cat18,
                                                Is_Cat18_Time = Is_Cat18_Time,
                                                Is_Cat19 = Is_Cat19,
                                                Is_Cat19_Time = Is_Cat19_Time,
                                                Is_Cat20 = Is_Cat20,
                                                Is_Cat20_Time = Is_Cat20_Time,
                                                
                                                Is_Cat01_Keterangan = "",
                                                Is_Cat02_Keterangan = "",
                                                Is_Cat03_Keterangan = "",
                                                Is_Cat04_Keterangan = "",
                                                Is_Cat05_Keterangan = "",
                                                Is_Cat06_Keterangan = "",
                                                Is_Cat07_Keterangan = "",
                                                Is_Cat08_Keterangan = "",
                                                Is_Cat09_Keterangan = "",
                                                Is_Cat10_Keterangan = "",
                                                Is_Cat11_Keterangan = "",
                                                Is_Cat12_Keterangan = "",
                                                Is_Cat13_Keterangan = "",
                                                Is_Cat14_Keterangan = "",
                                                Is_Cat15_Keterangan = "",
                                                Is_Cat16_Keterangan = "",
                                                Is_Cat17_Keterangan = "",
                                                Is_Cat18_Keterangan = "",
                                                Is_Cat19_Keterangan = "",
                                                Is_Cat20_Keterangan = ""
                                            }, s_ssid
                                        );
                                }
                            }
                            //end data siswa absen
                        }
                    }
                }
            }

            hasil.Add("1");
            return hasil.ToArray();
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string GetValidateAbsen(string tgl, string m, string kd, string g)
        {
            try
            {
                string rel_kelasdet = kd;
                string tanggal = tgl;
                string rel_mapel = m;
                string rel_guru = g;

                DateTime dt_tanggal = Libs.GetDateFromTanggalIndonesiaStr(tanggal);
                dt_tanggal = new DateTime(dt_tanggal.Year, dt_tanggal.Month, dt_tanggal.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                List<SiswaAbsenMapelByJadwal> lst_jadwal = DAO_SiswaAbsenMapel.GetJadwalByAbsen_Entity(dt_tanggal, rel_mapel, rel_kelasdet, rel_guru);

                if (lst_jadwal.FindAll(m0 => m0.Keterangan == DAO_SiswaAbsenMapel.JADWAL_ABSEN.SESUAI_JADWAL).Count > 0)
                {
                    return DAO_SiswaAbsenMapel.JADWAL_ABSEN.SESUAI_JADWAL;
                }
                else if (lst_jadwal.FindAll(m0 => m0.Keterangan == DAO_SiswaAbsenMapel.JADWAL_ABSEN.DILUAR_JADWAL).Count > 0)
                {
                    return DAO_SiswaAbsenMapel.JADWAL_ABSEN.DILUAR_JADWAL;
                }
                else
                {
                    if (DAO_MapelJadwalDet.GetByTanggal_Entity(dt_tanggal).Count > 0)
                    {
                        return DAO_SiswaAbsenMapel.JADWAL_ABSEN.DILUAR_JADWAL;
                    }
                    else
                    {
                        return DAO_SiswaAbsenMapel.JADWAL_ABSEN.TIDAK_ADA_JADWAL;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
