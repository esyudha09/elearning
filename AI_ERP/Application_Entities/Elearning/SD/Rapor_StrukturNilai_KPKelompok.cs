using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Rapor_StrukturNilai_KPKelompok
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_StrukturNilai { get; set; }
        public Guid Rel_Rapor_KomponenPenilaian { get; set; }
        public decimal Bobot { get; set; }
        public int Urutan { get; set; }
    }
}