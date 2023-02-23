using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class PembelajaranJudul
    {
        public Guid Kode { get; set; }
        public Guid Rel_Pembelajaran { get; set; }
        public string Judul { get; set; }
        public string Keterangan { get; set; }
        public int Urutan { get; set; }
    }
}