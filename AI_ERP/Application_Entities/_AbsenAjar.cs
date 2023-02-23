using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class _AbsenAjar
    {
        public DateTime Tanggal { get; set; }
        public string TahunAjaran { get; set; }
        public string Kelas { get; set; }
        public string Semester { get; set; }
        public string SubSemester { get; set; }
        public string Guru { get; set; }
        public string NamaGuru { get; set; }
        public string Mapel { get; set; }
        public string NamaMapel { get; set; }
        public string Ruang { get; set; }
        public string JamMasuk { get; set; }
        public string JamKeluar { get; set; }
        public string Keterangan { get; set; }
        public string JamKe { get; set; }
        public string NISSekolah { get; set; }
        public bool IsSakit { get; set; }
        public bool IsIzin { get; set; }
        public bool IsAlfa { get; set; }
        public string KeteranganAbsen { get; set; }
    }
}