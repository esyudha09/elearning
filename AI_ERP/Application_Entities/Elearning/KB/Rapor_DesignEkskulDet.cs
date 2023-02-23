using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.KB
{
    public class Rapor_DesignEkskulDet
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_DesignEkskul { get; set; }
        public string Poin { get; set; }
        public Guid Rel_KomponenRapor { get; set; }
        public JenisKomponenRapor JenisKomponen { get; set; }
        public string NamaKomponen { get; set; }
        public int Urut { get; set; }
    }
}