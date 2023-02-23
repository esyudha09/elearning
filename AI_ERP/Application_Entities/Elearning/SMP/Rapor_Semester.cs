using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public class Rapor_Semester
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_KelasDet { get; set; }
        public bool IsLocked { get; set; }
        public string GuruKelas { get; set; }
        public string Kurikulum { get; set; }
        public DateTime Tanggal { get; set; }
    }
}