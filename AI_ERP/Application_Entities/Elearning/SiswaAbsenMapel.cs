using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning
{
    public class SiswaAbsenMapel
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public Guid Rel_Sekolah { get; set; }
        public Guid Rel_KelasDet { get; set; }
        public string Rel_Guru { get; set; }
        public string Rel_Mapel { get; set; }
        public DateTime Tanggal { get; set; }
        public string Rel_Siswa { get; set; }
        public string JamAwal { get; set; }
        public string JamAkhir { get; set; }
        public string Absen { get; set; }
        public string Keterangan { get; set; }
        public Guid Rel_Linimasa { get; set; }
        public string Kejadian { get; set; }
        public string ButirSikap { get; set; }
        public string ButirSikapLain { get; set; }
        public string Sikap { get; set; }
        public string TindakLanjut { get; set; }
        public string RTG_UNIT { get; set; }
        public string RTG_LEVEL { get; set; }
        public string RTG_SEMESTER { get; set; }
        public string RTG_KELAS { get; set; }
        public string RTG_SUBKELAS { get; set; }


        public string Is_Hadir { get; set; }
        public DateTime Is_Hadir_Time { get; set; }
        public string Is_Sakit { get; set; }
        public DateTime Is_Sakit_Time { get; set; }
        public string Is_Izin { get; set; }
        public DateTime Is_Izin_Time { get; set; }
        public string Is_Alpa { get; set; }
        public DateTime Is_Alpa_Time { get; set; }
        public string Is_Cat01 { get; set; }
        public DateTime Is_Cat01_Time { get; set; }
        public string Is_Cat02 { get; set; }
        public DateTime Is_Cat02_Time { get; set; }
        public string Is_Cat03 { get; set; }
        public DateTime Is_Cat03_Time { get; set; }
        public string Is_Cat04 { get; set; }
        public DateTime Is_Cat04_Time { get; set; }
        public string Is_Cat05 { get; set; }
        public DateTime Is_Cat05_Time { get; set; }
        public string Is_Cat06 { get; set; }
        public DateTime Is_Cat06_Time { get; set; }
        public string Is_Cat07 { get; set; }
        public DateTime Is_Cat07_Time { get; set; }
        public string Is_Cat08 { get; set; }
        public DateTime Is_Cat08_Time { get; set; }
        public string Is_Cat09 { get; set; }
        public DateTime Is_Cat09_Time { get; set; }
        public string Is_Cat10 { get; set; }
        public DateTime Is_Cat10_Time { get; set; }
        public string Is_Cat11 { get; set; }
        public DateTime Is_Cat11_Time { get; set; }
        public string Is_Cat12 { get; set; }
        public DateTime Is_Cat12_Time { get; set; }
        public string Is_Cat13 { get; set; }
        public DateTime Is_Cat13_Time { get; set; }
        public string Is_Cat14 { get; set; }
        public DateTime Is_Cat14_Time { get; set; }
        public string Is_Cat15 { get; set; }
        public DateTime Is_Cat15_Time { get; set; }
        public string Is_Cat16 { get; set; }
        public DateTime Is_Cat16_Time { get; set; }
        public string Is_Cat17 { get; set; }
        public DateTime Is_Cat17_Time { get; set; }
        public string Is_Cat18 { get; set; }
        public DateTime Is_Cat18_Time { get; set; }
        public string Is_Cat19 { get; set; }
        public DateTime Is_Cat19_Time { get; set; }
        public string Is_Cat20 { get; set; }
        public DateTime Is_Cat20_Time { get; set; }

        public string Is_Sakit_Keterangan { get; set; }
        public string Is_Izin_Keterangan { get; set; }
        public string Is_Alpa_Keterangan { get; set; }

        public string Is_Cat01_Keterangan { get; set; }
        public string Is_Cat02_Keterangan { get; set; }
        public string Is_Cat03_Keterangan { get; set; }
        public string Is_Cat04_Keterangan { get; set; }
        public string Is_Cat05_Keterangan { get; set; }
        public string Is_Cat06_Keterangan { get; set; }
        public string Is_Cat07_Keterangan { get; set; }
        public string Is_Cat08_Keterangan { get; set; }
        public string Is_Cat09_Keterangan { get; set; }
        public string Is_Cat10_Keterangan { get; set; }
        public string Is_Cat11_Keterangan { get; set; }
        public string Is_Cat12_Keterangan { get; set; }
        public string Is_Cat13_Keterangan { get; set; }
        public string Is_Cat14_Keterangan { get; set; }
        public string Is_Cat15_Keterangan { get; set; }
        public string Is_Cat16_Keterangan { get; set; }
        public string Is_Cat17_Keterangan { get; set; }
        public string Is_Cat18_Keterangan { get; set; }
        public string Is_Cat19_Keterangan { get; set; }
        public string Is_Cat20_Keterangan { get; set; }
    }

    public class SiswaAbsenMapelRekap
    {
        public string Hadir { get; set; }
        public string Ditugaskan { get; set; }
        public string Terlambat { get; set; }
        public string Sakit { get; set; }
        public string Izin { get; set; }
        public string TanpaKeterangan { get; set; }
    }

    public class SiswaAbsenMapelByJadwal
    {
        public string Rel_MapelJadwal { get; set; }
        public DateTime TanggalAbsen { get; set; }
        public DateTime DariJam { get; set; }
        public DateTime SampaiJam { get; set; }
        public DateTime DariJam_Asli { get; set; }
        public DateTime SampaiJam_Asli { get; set; }
        public string Keterangan { get; set; }

    }
}