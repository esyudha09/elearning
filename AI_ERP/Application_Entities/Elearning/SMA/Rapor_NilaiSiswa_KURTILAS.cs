using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_NilaiSiswa_KURTILAS
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_Nilai { get; set; }
        public string Rel_Siswa { get; set; }
        public string Rapor_Pengetahuan { get; set; }
        public string Rapor_Keterampilan { get; set; }
        public string Predikat_Pengetahuan { get; set; }
        public string Predikat_Keterampilan { get; set; }
        public string LTS_HD { get; set; }
        public string LTS_MAX_HD { get; set; }
        public string LTS_S { get; set; }
        public string LTS_I { get; set; }
        public string LTS_A { get; set; }
        public string LTS_LK { get; set; }
        public string LTS_RJ { get; set; }
        public string LTS_RPKB { get; set; }
        public string SM_HD { get; set; }
        public string SM_MAX_HD { get; set; }
        public string SM_S { get; set; }
        public string SM_I { get; set; }
        public string SM_A { get; set; }
        public string SM_LK { get; set; }
        public string SM_RJ { get; set; }
        public string SM_RPKB { get; set; }

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