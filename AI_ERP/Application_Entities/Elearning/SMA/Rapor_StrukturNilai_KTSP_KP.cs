using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_StrukturNilai_KTSP_KP
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_StrukturNilai_KTSP_KD { get; set; }
        public Guid Rel_Rapor_KomponenPenilaian { get; set; }
        public decimal BobotNKD { get; set; }
        public string Jenis { get; set; }
        public int Urutan { get; set; }
    }
}