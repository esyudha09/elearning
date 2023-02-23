using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.TK
{
    public class Rapor_NilaiSiswa
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_Nilai { get; set; }
        public string Rel_Siswa { get; set; }
        public string BeratBadan { get; set; }
        public string TinggiBadan { get; set; }
        public string LingkarKepala { get; set; }
        public bool IsPosted { get; set; }
        public bool IsLocked { get; set; }

        public string LTS_CK_KEHADIRAN { get; set; }
        public string LTS_CK_KETEPATAN_WKT { get; set; }
        public string LTS_CK_PENGGUNAAN_SRGM { get; set; }
        public string LTS_CK_PENGGUNAAN_KMR { get; set; }

        public string SM_CK_KEHADIRAN { get; set; }
        public string SM_CK_KETEPATAN_WKT { get; set; }
        public string SM_CK_PENGGUNAAN_SRGM { get; set; }
        public string SM_CK_PENGGUNAAN_KMR { get; set; }
    }

    public class RaporEkskulSiswa {
        public string Mapel { get; set; }
        public string LTS_CK_KEHADIRAN { get; set; }
    }
}