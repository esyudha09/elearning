using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Rapor_StrukturNilai_AP
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_StrukturNilai { get; set; }
        public string Poin { get; set; }
        public Guid Rel_Rapor_AspekPenilaian { get; set; }
        public string JenisPerhitungan { get; set; }
        public decimal BobotRapor { get; set; }
        public bool IsAdaPAT_UKK { get; set; }
        public decimal Bobot_PAT_UKK { get; set; }
        public decimal Bobot_Non_PAT_UKK { get; set; }
        public int Urutan { get; set; }
    }
}