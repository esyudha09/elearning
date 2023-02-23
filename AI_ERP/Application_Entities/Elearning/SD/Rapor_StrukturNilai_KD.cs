using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Rapor_StrukturNilai_KD
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_StrukturNilai_AP { get; set; }
        public string Poin { get; set; }
        public string JenisPerhitungan { get; set; }
        public Guid Rel_Rapor_KompetensiDasar { get; set; }
        public decimal BobotAP { get; set; }
        public int Urutan { get; set; }
        public string Deskripsi { get; set; }
    }
}