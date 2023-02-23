using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_StrukturNilai_KURTILAS_Predikat
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_StrukturNilai { get; set; }
        public decimal Minimal { get; set; }
        public decimal Maksimal { get; set; }
        public string Predikat { get; set; }
        public int Urutan { get; set; }
        public string Deskripsi { get; set; }
    }
}