using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD.Reports
{
    public class KURTILAS_RaporNilai
    {
        public string IDSiswa { get; set; }
        public string NIS { get; set; }
        public string NISN { get; set; }
        public string Nama { get; set; }
        public string Kelas { get; set; }
        public string Semester { get; set; }
        public string TahunAjaran { get; set; }
        public string NamaMapel { get; set; }
        public decimal KKM { get; set; }
        public decimal NilaiPengetahuan { get; set; }
        public string PredikatPengetahuan { get; set; }
        public decimal NilaiKeterampilan { get; set; }
        public string PredikatKeterampilan { get; set; }
        public decimal NilaiRataRataSM1SM2 { get; set; }
        public string PredikatRataRataSM1SM2 { get; set; }
        public string PengetahuanRataRataSM1SM2 { get; set; }
        public string KeterampilanRataRataSM1SM2 { get; set; }
        public string TanggalRapor { get; set; }
        public string GuruKelas { get; set; }
        public string KepalaSekolah { get; set; }
        public string JenisNilai { get; set; }
        public string Deskripsi { get; set; }
        public decimal Nilai { get; set; }
        public string Predikat { get; set; }
        public string NomorMapel { get; set; }
        public string StatusKenaikanKelas { get; set; }
        public string StatusKenaikanKelasKeKelas { get; set; }
        public byte[] TTDGuru { get; set; }
        public byte[] TTDKepsek { get; set; }
        public byte[] QRCode { get; set; }
        public List<RaporLTSCapaianKedisiplinan> HasilCapaianKedisiplinan { get; set; }
    }
}