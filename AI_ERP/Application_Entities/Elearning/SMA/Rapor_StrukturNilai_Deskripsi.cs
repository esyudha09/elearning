using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_StrukturNilai_Deskripsi
    {
        public Guid Kode { set; get; }
        public string TahunAjaran{ set; get; }
        public string Semester { set; get; }
        public string Rel_KelasDet { set; get; }
        public string Rel_StrukturNilai { set; get; }
        public string Rel_StrukturNilai_AP { set; get; }
        public string Rel_StrukturNilai_KD { set; get; }
        public string Rel_StrukturNilai_KP { set; get; }
        public string Deskripsi { set; get; }
    }
}