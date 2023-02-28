﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class CBT_BankSoal
    {
        public Guid Kode { get; set; }
        public string Rel_Mapel { get; set; }
        public string Rel_Guru { get; set; }
        public string Soal { get; set; }
        public string Jenis { get; set; }
        public string JwbEssay { get; set; }
        public string JwbGanda { get; set; }
        public string JwbGanda1 { get; set; }
        public string JwbGanda2 { get; set; }
        public string JwbGanda3 { get; set; }
        public string JwbGanda4 { get; set; }
        public string JwbGanda5 { get; set; }

    }

    public class CBT_RumahSoal
    {
        public Guid Kode { get; set; }
        public string Rel_Rapor_StrukturNilai_KP { get; set; }
        public string Rel_Mapel { get; set; }
        public string Rel_Guru { get; set; }
        public string Kurikulum { get; set; }
        public string Nama { get; set; }
        public string Deskripsi { get; set; }
        public DateTime? StartDatetime { get; set; }
        public DateTime? EndDatetime { get; set; }
        public int LimitTime { get; set; }
        public string LimitSatuan { get; set; }
    }

    public class CBT_DesignSoal
    {
        public Guid Kode { get; set; }
        public string Rel_RumahSoal { get; set; }
        public string Rel_BankSoal { get; set; }
        public string Jenis { get; set; }
        public int Skor { get; set; }
        public int Urut { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

}