using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Rapor_Ekskul
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_Kelas { get; set; }
        public string Rel_Kelas2 { get; set; }
        public string Rel_Kelas3 { get; set; }
        public string Rel_Kelas4 { get; set; }
        public string Rel_Kelas5 { get; set; }
        public string Rel_Kelas6 { get; set; }
        public string Kurikulum { get; set; }
        public string Rel_Mapel { get; set; }
    }
}