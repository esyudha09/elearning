using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning
{
    public class SiswaAbsenLTS
    {
        public Guid Kode { set; get; }
        public string TahunAjaran { set; get; }
        public string Semester { set; get; }
        public string Rel_KelasDet { set; get; }
        public string Rel_Siswa { set; get; }
        public int Sakit { set; get; }
        public int Izin { set; get; }
        public int Alpa { set; get; }

    }
}