using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Perpustakaan
{
    public class PerpustakaanKunjunganRutin
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Rel_Sekolah { get; set; }
        public string Keterangan { get; set; }
        public bool IsSemester_1 { get; set; }
        public bool IsSemester_2 { get; set; }

    }
}