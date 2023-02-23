using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class FormasiGuruMapelDetSiswaDet
    {
        public Guid Kode { get; set; }
        public string Rel_FormasiGuruMapelDet { get; set; }
        public string Rel_Siswa { get; set; }
        public int Urutan { get; set; }
    }
}