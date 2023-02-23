using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP.Reports
{
    public class KTSP_RaporNilai
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
        public string NamaMapel { get; set; }
        public decimal NilaiRapor { get; set; }
        public decimal KKM { get; set; }
        public string Predikat { get; set; }
        public int Urutan { get; set; }
        public int Halaman { get; set; }
        public string WaliKelas { get; set; }
        public string TinggalDiKelas { get; set; }
        public string TanggalRapor { get; set; }
        public string KepalaSekolah { get; set; }
        public string NaikKeKelas { get; set; }
        public string NaikKeKelasKeterangan { get; set; }
        public bool IsNaik { get; set; }
        public byte[] TTDGuru { get; set; }
        public byte[] TTDKepsek { get; set; }
    }
}