using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public class FormasiEkskul
    {
        public Guid Kode { set; get; }
        public string Rel_Mapel { set; get; }
        public string Rel_Guru { set; get; }
        public string TahunAjaran { set; get; }
        public string Semester { set; get; }
        public string Rel_Rapor_StrukturNilai { set; get; }
    }
}