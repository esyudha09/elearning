using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class _NilaiSiswa
    {
        public string TahunAjaran { get; set; }
        public string Kelas { get; set; }
        public string Semester { get; set; }
        public string Mapel { get; set; }
        public string NamaMapel { get; set; }
        public string Guru { get; set; }
        public string NamaGuru { get; set; }
        public string NISSekolah { get; set; }
        public string AspekNilai { get; set; }
        public string NamaAspek { get; set; }
        public string NamaSubAspek { get; set; }
        public string UrutNilai { get; set; }
        public decimal Nilai { get; set; }
    }
}