using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.KB
{
    public class Rapor_NilaiSiswa_Det
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_NilaiSiswa { get; set; }
        public string Rel_Siswa { get; set; }
        public string Rel_Rapor_DesignDet { get; set; }
        public string Rel_Rapor_Kriteria { get; set; }
        public string Deskripsi { get; set; }
    }
}