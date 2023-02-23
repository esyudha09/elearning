using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public class Rapor_StrukturNilai_AP
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_StrukturNilai { get; set; }
        public string Poin { get; set; }
        public Guid Rel_Rapor_AspekPenilaian { get; set; }
        public string JenisPerhitungan { get; set; }
        public decimal BobotRapor { get; set; }
        public int Urutan { get; set; }
    }
}