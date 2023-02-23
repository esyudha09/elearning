using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public class FormasiEkskulGuru
    {
        public Guid Kode { get; set; }
        public string Rel_FormasiEkskul { get; set; }
        public string Rel_Guru { get; set; }
    }
}