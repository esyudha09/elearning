using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_Desain
    {
        public Guid Kode { set; get; }
        public string TahunAjaran { set; get; }
        public string Semester { set; get; }
        public string Rel_Kelas { set; get; }
        public string JenisRapor { set; get; }
    }
}