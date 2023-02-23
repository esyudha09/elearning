using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class FormasiGuruMapelDetSiswa
    {
        public Guid Kode { get; set; }
        public Guid Rel_FormasiGuruMapel { get; set; }
        public string Rel_Siswa { get; set; }
        public int Urutan { get; set; }
    }
}