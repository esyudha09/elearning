using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning
{
    public class SiswaAbsenReport
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_Sekolah { get; set; }
        public string Rel_KelasDet { get; set; }
        public string Rel_Guru { get; set; }

        public string Rel_Mapel { get; set; }
        public string Mapel { get; set; }
        public string JenisMapel { get; set; }

        public DateTime Tanggal { get; set; }
        public string Rel_Siswa { get; set; }
        public string NamaSiswa { get; set; }
        public string Rel_Linimasa { get; set; }
        public string Is_Hadir { get; set; }
        public string Is_Sakit { get; set; }
        public string Is_Sakit_Keterangan { get; set; }
        public string Is_Izin { get; set; }
        public string Is_Izin_Keterangan { get; set; }
        public string Is_Alpa { get; set; }
        public string Is_Alpa_Keterangan { get; set; }
        public string Is_Cat01 { get; set; }
        public string Is_Cat01_Keterangan { get; set; }
        public string Is_Cat02 { get; set; }
        public string Is_Cat02_Keterangan { get; set; }
        public string Is_Cat03 { get; set; }
        public string Is_Cat03_Keterangan { get; set; }
        public string Is_Cat04 { get; set; }
        public string Is_Cat04_Keterangan { get; set; }
        public string Is_Cat05 { get; set; }
        public string Is_Cat05_Keterangan { get; set; }
        public string Is_Cat06 { get; set; }
        public string Is_Cat06_Keterangan { get; set; }
        public string Is_Cat07 { get; set; }
        public string Is_Cat07_Keterangan { get; set; }
        public string Is_Cat08 { get; set; }
        public string Is_Cat08_Keterangan { get; set; }
        public string Is_Cat09 { get; set; }
        public string Is_Cat09_Keterangan { get; set; }
        public string Is_Cat10 { get; set; }
        public string Is_Cat10_Keterangan { get; set; }
    }
}