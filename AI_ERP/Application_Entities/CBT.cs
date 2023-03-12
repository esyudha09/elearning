using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class CBT_BankSoal
    {
        public Guid Kode { get; set; }
        public string Rel_Mapel { get; set; }
        public string Rel_Kelas { get; set; }
        public string Rel_Guru { get; set; }
        public string Soal { get; set; }
        public string Jenis { get; set; }
        public string JwbEssay { get; set; }
        public string Rel_JwbGanda { get; set; }

        public List<CBT_BankSoalJawabGanda> ListJwbGanda { get; set; }

    }

    public class CBT_BankSoalJawabGanda
    {
        public Guid Kode { get; set; }
        public string Jawaban { get; set; }
        public int Urut { get; set; }
    }

    public class CBT_RumahSoal
    {
        public Guid Kode { get; set; }
        public string Rel_Rapor_StrukturNilai_KP { get; set; }
        public string Rel_Mapel { get; set; }
        public string Rel_Kelas { get; set; }
        public string Rel_Guru { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Kurikulum { get; set; }
        public string Nama { get; set; }
        public string Deskripsi { get; set; }
        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }
        public int LimitTime { get; set; }
        public string LimitSatuan { get; set; }
        public string NamaKelas { get; set; }
        public string NamaMapel { get; set; }
    }

    public class CBT_DesignSoal
    {
        public Guid Kode { get; set; }
        public string Rel_RumahSoal { get; set; }
        public string Rel_BankSoal { get; set; }
        public string Rel_Jwb { get; set; }
        public string Jenis { get; set; }
        public int Skor { get; set; }
        public int Urut { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class CBT_DesignSoalJwb
    {
        public Guid Kode { get; set; }
        public string Rel_RumahSoal { get; set; }
        public string Rel_BankSoal { get; set; }
        public string Rel_Jwb { get; set; }
        public string Rel_Siswa { get; set; }
        public string JwbEssay { get; set; }
        public string Rel_JwbGanda { get; set; }
    }

    public class CBT_Jwb
    {
        public Guid Kode { get; set; }
        public string Rel_RumahSoal { get; set; }
        public string Rel_DesignSoal { get; set; }
        public string Rel_BankSoal { get; set; }
        public string Rel_Siswa { get; set; }
        public string Rel_JwbGanda { get; set; }
        public string JwbEssay { get; set; }
        public DateTime Tanggal_Buat { get; set; }

    }

    public class CBT_StatusSiswa
    {
        public Guid Kode { get; set; }
        public string Rel_RumahSoal { get; set; }
        public string Rel_Siswa { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}