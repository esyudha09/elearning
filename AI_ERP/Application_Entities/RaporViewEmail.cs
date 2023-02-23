using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class RaporViewEmail
    {
        public Guid Kode { get; set; }
        public string Rel_Email { get; set; }
        public string Rel_Siswa { get; set; }
        public DateTime Tanggal { get; set; }
        public string URL { get; set; }
    }
}