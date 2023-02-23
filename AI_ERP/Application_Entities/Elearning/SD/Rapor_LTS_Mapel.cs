using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Rapor_LTS_Mapel
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_Mapel { get; set; }
        public string Rel_KelasDet { get; set; }
        public string Rel_Rapor_StrukturNilai_KD { get; set; }
        public int Urutan { get; set; }
    }

    public class Rapor_LTS_Mapel_Ext : Rapor_LTS_Mapel
    {
        public string Rel_Rapor_StrukturNilai_AP { get; set; }
        public string Rel_Rapor_StrukturNilai_KD { get; set; }
        public string Rel_Rapor_StrukturNilai_KP { get; set; }
    }
}