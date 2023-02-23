using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_Nilai
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_Kelas { get; set; }
        public string Rel_KelasDet { get; set; }
        public string Kurikulum { get; set; }
        public string Rel_Mapel { get; set; }
    }
}