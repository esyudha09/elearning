using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public static class Reports
    {
        public class AbsensiSiswa
        {
            public string TahunAjaran { set; get; }
            public string Semester { set; get; }
            public string Rel_Unit { set; get; }
            public string Unit { set; get; }
            public string Rel_Guru { set; get; }
            public string Guru { set; get; }
            public string Rel_Mapel { set; get; }
            public string Mapel { set; get; }
            public string Rel_KelasDet { set; get; }
            public string KelasDet { set; get; }
            public string Rel_Siswa { set; get; }
            public string NIS { set; get; }
            public string Nama { set; get; }
            public DateTime Tanggal { set; get; }
            public int UrutanJenjang { set; get; }
            public int UrutanLevel { set; get; }
            public int UrutanKelas { set; get; }
            public string Kehadiran { set; get; }
            public string Sakit { set; get; }
            public string Izin { set; get; }
            public string Alpa { set; get; }
            public string Is_Cat01 { set; get; }
            public string Is_Cat02 { set; get; }
            public string Is_Cat03 { set; get; }
            public string Is_Cat04 { set; get; }
            public string Is_Cat05 { set; get; }
            public string Is_Cat06 { set; get; }
            public string Is_Cat07 { set; get; }
            public string Is_Cat08 { set; get; }
            public string Is_Cat09 { set; get; }
            public string Is_Cat10 { set; get; }
            public string Is_Sakit_Keterangan { set; get; }
            public string Is_Izin_Keterangan { set; get; }
            public string Is_Alpa_Keterangan { set; get; }
            public string Is_Cat01_Keterangan { set; get; }
            public string Is_Cat02_Keterangan { set; get; }
            public string Is_Cat03_Keterangan { set; get; }
            public string Is_Cat04_Keterangan { set; get; }
            public string Is_Cat05_Keterangan { set; get; }
            public string Is_Cat06_Keterangan { set; get; }
            public string Is_Cat07_Keterangan { set; get; }
            public string Is_Cat08_Keterangan { set; get; }
            public string Is_Cat09_Keterangan { set; get; }
            public string Is_Cat10_Keterangan { set; get; }
        }
    }
}