using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_StrukturNilai_KTSP_KD
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_StrukturNilai { get; set; }
        public string Poin { get; set; }
        public Guid Rel_Rapor_KompetensiDasar { get; set; }
        public decimal BobotRaporPPK { get; set; }
        public decimal BobotRaporP { get; set; }
        public string JenisPerhitungan { get; set; }
        public int Urutan { get; set; }
    }
}