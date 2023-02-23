using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.TK
{
    public class Rapor_Design
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public Guid Rel_Kelas { get; set; }
        public bool IsLocked { get; set; }
        public string TipeRapor { get; set; }
    }
}