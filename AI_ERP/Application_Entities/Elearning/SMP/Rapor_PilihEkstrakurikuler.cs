using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public class Rapor_PilihEkstrakurikuler
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_KelasDet { get; set; }
        public string Rel_Siswa { get; set; }
        public string Rel_Mapel1 { get; set; }
        public string Rel_Mapel2 { get; set; }
        public string Rel_Mapel3 { get; set; }
        public string Rel_Mapel4 { get; set; }
        public string Rel_Mapel5 { get; set; }
    }
}