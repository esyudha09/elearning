using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP.Reports
{
    public class KTSP_DeskripsiRapor
    {
        public string IDSiswa { get; set; }
        public string NamaSekolah { get; set; }
        public string Alamat { get; set; }
        public string Nama { get; set; }
        public string NIS { get; set; }
        public string NISN { get; set; }
        public string Kelas { get; set; }
        public string Semester { get; set; }
        public string TahunPelajaran { get; set; }
        public int UrutanRapor { get; set; }
        public string NamaMataPelajaran { get; set; }
        public string Deskripsi { get; set; }
        public decimal Nilai { get; set; }
        public decimal KKM { get; set; }
    }
}