using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class BukaSemester
    {
        public Guid Kode { get; set; }
        public string Rel_Sekolah { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public DateTime Tanggal { get; set; }
        public string UserID { get; set; }
    }
}