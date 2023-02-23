using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class PembelajaranJudulMateri
    {
        public Guid Kode { get; set; }
        public Guid Rel_PembelajaranJudul { get; set; }
        public int Urutan { get; set; }
        public string Materi { get; set; }
    }
}