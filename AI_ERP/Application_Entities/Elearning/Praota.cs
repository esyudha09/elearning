using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning
{
    public class Praota
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Rel_Sekolah { get; set; }
        public string Rel_Kelas { get; set; }
        public string Rel_Guru { get; set; }
        public string Rel_Mapel { get; set; }
        public bool IsPublished { get; set; }        
    }
}