using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Rapor_StrukturNilai_KURTILAS_KP
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_StrukturNilai_KURTILAS_KD { get; set; }
        public Guid Rel_Rapor_KomponenPenilaian { get; set; }
        public decimal BobotNK { get; set; }
        public string Jenis { get; set; }
        public int Urutan { get; set; }
    }
}