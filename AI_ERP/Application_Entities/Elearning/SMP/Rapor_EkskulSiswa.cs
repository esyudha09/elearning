using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public class Rapor_EkskulSiswa
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_Ekskul { get; set; }
        public string Rel_Siswa { get; set; }
        public string Rapor { get; set; }
        public string Sakit { get; set; }
        public string Izin { get; set; }
        public string Alpa { get; set; }
    }
}