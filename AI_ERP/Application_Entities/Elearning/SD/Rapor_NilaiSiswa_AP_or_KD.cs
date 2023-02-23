using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Rapor_NilaiSiswa_AP_or_KD
    {
        public Guid Kode { get; set; }
        public string Rel_Rapor_Nilai { get; set; }
        public string Rel_StrukturNilai_AP { get; set; }
        public string Rel_StrukturNilai_KD { get; set; }
        public string Rel_Siswa { get; set; }
        public string Nilai { get; set; }
    }
}