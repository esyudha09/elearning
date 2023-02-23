using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class MapelEkskulAjar
    {
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_Mapel { get; set; }
        public string Rel_Kelas { get; set; }
        public string Rel_KelasDet { get; set; }
        public int UrutanLevel { get; set; }
    }
}