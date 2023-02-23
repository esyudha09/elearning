using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP.Reports
{
    public class KURTILAS_NilaiSikap
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
        public decimal SikapSpiritual { get; set; }
        public decimal SikapSosial { get; set; }
        public DateTime TanggalRapor { get; set; }
        public string WaliKelas { get; set; }
        public string NIP { get; set; }
        public decimal Nilai { get; set; }
    }
}