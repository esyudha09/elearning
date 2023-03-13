using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_StrukturNilai_KURTILAS_AP
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_StrukturNilai { get; set; }
        public string Poin { get; set; }
        public Guid Rel_Rapor_AspekPenilaian { get; set; }
        public string JenisPerhitungan { get; set; }
        public decimal BobotRapor { get; set; }
        public int Urutan { get; set; }
        public string KompetensiDasar { get; set; }
    }
}