using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public class Rapor_Kepribadian
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_KelasDet { get; set; }
        public string Rel_Siswa { get; set; }
        public string Kelakuan { get; set; }
        public string Kerajinan { get; set; }
        public string Kerapihan { get; set; }
    }
}