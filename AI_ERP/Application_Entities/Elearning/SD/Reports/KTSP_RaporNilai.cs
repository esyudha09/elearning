using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD.Reports
{
    public class KTSP_RaporNilai
    {
        public string IDSiswa { get; set; }
        public string NIS { get; set; }
        public string NISN { get; set; }
        public string Nama { get; set; }
        public string Kelas { get; set; }
        public string Semester { get; set; }
        public string TahunAjaran { get; set; }
        public int UrutanMapelRapor { get; set; }
        public string NamaMapel { get; set; }
        public decimal KKM { get; set; }
        public decimal Nilai { get; set; }
        public string Tanggal { get; set; }
        public string GuruKelas { get; set; }
        public string KepalaSekolah { get; set; }
        public string NilaiRataRataSM1SM2 { get; set; }
        public string NaikKeKelas { get; set; }
        public string NaikKeKelasKeterangan { get; set; }
        public bool IsNaik { get; set; }
        public byte[] TTDKepsek { get; set; }
        public byte[] TTDGuru { get; set; }
    }
}