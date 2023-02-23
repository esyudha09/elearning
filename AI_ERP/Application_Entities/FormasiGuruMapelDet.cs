using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class FormasiGuruMapelDet
    {
        public Guid Kode { get; set; }
        public Guid Rel_FormasiGuruMapel { get; set; }
        public string Rel_Guru { get; set; }
        public Guid Rel_KelasDet { get; set; }
        public bool IsSiswaPilihan { get; set; }
        public string Keterangan { get; set; }
        public int Urutan { get; set; }
    }
}