using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning
{
    public class IsiNilai_Log
    {
        public Guid Kode { get; set; }
        public DateTime Tanggal { get; set; }
        public string Rel_ProsesRapor { get; set; }
        public string Rel_Sekolah { get; set; }
        public string Rel_Guru { get; set; }
        public string Rel_Mapel { get; set; }
        public string Rel_KelasDet { get; set; }
        public string Alasan { get; set; }
        public string Keterangan { get; set; }
        public bool IsClosed { get; set; }
        public string UserIDOpened { get; set; }
        public string UserIDClosed { get; set; }
        public DateTime ClosedDate { get; set; }
    }
}