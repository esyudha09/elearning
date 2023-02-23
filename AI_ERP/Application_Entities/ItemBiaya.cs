using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class ItemBiaya
    {
        public Guid Kode { get; set; }
        public string Nama { get; set; }
        public string Keterangan { get; set; }
        public bool IsBulanan { get; set; }
        public bool IsBisaDicicil { get; set; }
        public bool IsTopUpSmartCard { get; set; }
        public bool IsBiayaTerbuka { get; set; }
        public string VA_MANDIRI { get; set; }
        public string VA_PERMATA { get; set; }
        public decimal JumlahDefault_MANDIRI { get; set; }
        public decimal JumlahDefault_PERMATA { get; set; }
        public string KodeTagihan_MANDIRI { get; set; }
        public string KodeTagihan_PERMATA { get; set; }
        public bool IsTagihVA_MANDIRI { get; set; }
        public bool IsTagihVA_PERMATA { get; set; }
    }
}