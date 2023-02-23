using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Perpustakaan
{
    public class PerpustakaanKunjunganRutinDet
    {
        public Guid Kode { get; set; }
        public Guid Rel_PerpustakaanKunjunganRutin { get; set; }
        public int Hari { get; set; }
        public string Waktu { get; set; }
        public Guid Rel_KelasDet { get; set; }
        public string Keterangan { get; set; }
    }
}