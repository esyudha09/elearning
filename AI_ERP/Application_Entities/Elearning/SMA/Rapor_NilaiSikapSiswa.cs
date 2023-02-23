using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_NilaiSikapSiswa
    {
        public Guid Kode { get; set; }
        public string Rel_Rapor_NilaiSikap { get; set; }
        public string Rel_Siswa { get; set; }
        public string SikapSpiritual { get; set; }
        public string SikapSosial { get; set; }
        public string DeskripsiSikapSpiritual { get; set; }
        public string DeskripsiSikapSosial { get; set; }
        public string SikapSpiritualAkhir { get; set; }
        public string SikapSosialAkhir { get; set; }
    }
}
