using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class Mapel
    {
        public Guid Kode { get; set; }        
        public string Nama { get; set; }
        public string Alias { get; set; }
        public string Jenis { get; set; }
        public string Keterangan { get; set; }
        public string Rel_Sekolah { get; set; }
    }
}