using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD.Reports
{
    public class KTSP_RaporEkskul
    {
        public string Rel_Siswa { get; set; }
        public string IDSiswa { get; set; }
        public string Kegiatan { get; set; }
        public string Materi { get; set; }
        public int UrutKD { get; set; }
        public string Nilai { get; set; }
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