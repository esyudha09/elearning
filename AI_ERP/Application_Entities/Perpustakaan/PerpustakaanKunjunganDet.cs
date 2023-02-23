using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Perpustakaan
{
    public class PerpustakaanKunjunganDet
    {
        public Guid Kode { get; set; }
        public Guid Rel_PerpustakaanKunjungan { get; set; }
        public string JamKe { get; set; }
        public string Waktu { get; set; }
    }
}