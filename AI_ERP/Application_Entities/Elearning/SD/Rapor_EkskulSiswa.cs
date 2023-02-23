using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
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

        public string LTS_CK_KEHADIRAN { get; set; }
        public string LTS_CK_KETEPATAN_WKT { get; set; }
        public string LTS_CK_PENGGUNAAN_SRGM { get; set; }
        public string LTS_CK_PENGGUNAAN_KMR { get; set; }

        public string SM_CK_KEHADIRAN { get; set; }
        public string SM_CK_KETEPATAN_WKT { get; set; }
        public string SM_CK_PENGGUNAAN_SRGM { get; set; }
        public string SM_CK_PENGGUNAAN_KMR { get; set; }
    }
}