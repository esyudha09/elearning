using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_NilaiEkskulSiswa
    {
        public Guid Kode { get; set; }
        public string Rel_Rapor_NilaiEkskul { get; set; }
        public string Rel_Siswa { get; set; }
        public string Nilai { get; set; }
        public string LTS_HD { get; set; }
        public string Sakit { get; set; }
        public string Izin { get; set; }
        public string Alpa { get; set; }
    }
}