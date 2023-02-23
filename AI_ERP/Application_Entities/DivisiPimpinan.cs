using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class DivisiPimpinan
    {
        public Guid Kode { get; set; }
        public Guid Rel_Divisi { get; set; }
        public string Rel_Pegawai { get; set; }
        public int UrutanLevel { get; set; }
        public Guid Rel_Jabatan { get; set; }
    }
}