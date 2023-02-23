using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.TK
{
    public class KenaikanKelas
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Rel_KelasDet { get; set; }
        public string Rel_Siswa { get; set; }
        public bool IsNaik { get; set; }
        public string KeteranganKenaikan { get; set; }
    }
}