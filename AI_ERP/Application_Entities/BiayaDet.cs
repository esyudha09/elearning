using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class BiayaDet
    {
        public Guid Kode { get; set; }
        public Guid Rel_Biaya { get; set; }
        public Guid Rel_ItemBiaya { get; set; }
        public decimal Jumlah { get; set; }
        public string Keterangan { get; set; }
        public bool IsSiswaDalam { get; set; }
        public bool IsSiswaLuar { get; set; }
        public bool IsLakiLaki { get; set; }
        public bool IsPerempuan { get; set; }
        public int UrutanBiaya { get; set; }
        public string Rel_COA_D { get; set; }
        public string Rel_COA_C { get; set; }
    }
}