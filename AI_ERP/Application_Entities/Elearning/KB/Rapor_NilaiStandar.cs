using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.KB
{
    public class Rapor_NilaiStandar
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_Design { get; set; }
        public Guid Rel_KelasDet { get; set; }
        public Guid Rel_Rapor_Kriteria { get; set; }
        public string Rel_Mapel { get; set; }
    }
}