using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.KB
{
    public class Rapor_Arsip_Unlock
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_Arsip { get; set; }
        public DateTime Tanggal { get; set; }
        public string Rel_KelasDet { get; set; }
        public string Rel_Mapel { get; set; }
        public string Rel_Guru { get; set; }
        public string Alasan { get; set; }
        public bool IsClosed { get; set; }
    }
}