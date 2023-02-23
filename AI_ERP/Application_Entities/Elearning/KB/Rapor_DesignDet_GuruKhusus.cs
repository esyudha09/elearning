using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.KB
{
    public class Rapor_DesignDet_GuruKhusus
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_DesignDet { get; set; }
        public string Rel_Guru { get; set; }
        public int Urut { get; set; }
    }
}