using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public class SiswaMapelPilihan_Det
    {
        public Guid Kode { get; set; }
        public Guid Rel_SiswaMapelPilihan { get; set; }
        public string Rel_Siswa { get; set; }
    }
}