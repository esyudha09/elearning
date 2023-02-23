using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_NilaiSiswa_KTSP_Det
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_NilaiSiswa_KTSP { get; set; }
        public string Rel_Siswa { get; set; }
        public string Rel_Rapor_StrukturNilai_KTSP_KD { get; set; }
        public string Rel_Rapor_StrukturNilai_KTSP_KP { get; set; }
        public string Nilai { get; set; }
    }

    public class Rapor_NilaiSiswa_KTSP_Det_Lengkap: Rapor_NilaiSiswa_KTSP_Det
    {
        public string Rel_Mapel { get; set; }
        public string Rel_KelasDet { get; set; }
    }
}