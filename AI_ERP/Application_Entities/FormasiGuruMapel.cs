using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class FormasiGuruMapel
    {
        public Guid Kode { get; set; }
        public Guid Rel_Sekolah { get; set; }
        public string Rel_Mapel { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_Kelas { get; set; }
    }
}