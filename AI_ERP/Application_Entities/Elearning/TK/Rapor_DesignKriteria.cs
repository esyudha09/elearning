using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.TK
{
    public class Rapor_DesignKriteria
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_Design { get; set; }
        public Guid Rel_Rapor_Kriteria { get; set; }
        public int Urut { get; set; }
        public string NamaKriteria { get; set; }
        public string Alias { get; set; }
    }
}