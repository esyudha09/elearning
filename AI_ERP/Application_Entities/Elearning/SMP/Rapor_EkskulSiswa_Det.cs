using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public class Rapor_EkskulSiswa_Det
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_EkskulSiswa { get; set; }
        public string Rel_Siswa { get; set; }
        public string Rel_Rapor_StrukturNilai_AP { get; set; }
        public string Rel_Rapor_StrukturNilai_KD { get; set; }
        public string Rel_Rapor_StrukturNilai_KP { get; set; }
        public string Nilai { get; set; }
    }
}