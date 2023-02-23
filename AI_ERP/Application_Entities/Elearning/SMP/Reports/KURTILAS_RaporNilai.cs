using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP.Reports
{
    public class KURTILAS_RaporNilai
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
        public string JenisNilai { get; set; }
        public string KelompokMapel { get; set; }
        public string NomorUrutMapel { get; set; }
        public string NamaMapel { get; set; }
        public decimal KKM { get; set; }
        public decimal Nilai { get; set; }
        public string Predikat { get; set; }
        public string Deskripsi { get; set; }
        public string Halaman { get; set; }
        public string WaliKelas { get; set; }
        public decimal NilaiSpiritual { get; set; }
        public decimal NilaiSosial { get; set; }
        public string NilaiSpiritualAkhir { get; set; }
        public string NilaiSosialAkhir { get; set; }
        public string DeskripsiSpiritual { get; set; }
        public string DeskripsiSosial { get; set; }
        public string TanggalRapor { get; set; }
        public string KepalaSekolah { get; set; }
        public string NaikKeKelas { get; set; }
        public string NaikKeKelasKeterangan { get; set; }
        public bool IsNaik { get; set; }
        public byte[] TTDGuru { get; set; }
        public byte[] TTDKepsek { get; set; }
        public byte[] QRCode { get; set; }
        public string LS_JumlahJam { get; set; }
        public string LS_Deskripsi { get; set; }
        public string KW_JumlahJam { get; set; }
        public string KW_Deskripsi { get; set; }
        public string IN_JumlahJam { get; set; }
        public string IN_Deskripsi { get; set; }
    }
}