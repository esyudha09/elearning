using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class UnitPimpinan
    {
        public Guid Kode { get; set; }
        public Guid Rel_Unit { get; set; }
        public string Rel_Pegawai { get; set; }
        public int UrutanLevel { get; set; }
        public Guid Rel_Jabatan { get; set; }
    }
}