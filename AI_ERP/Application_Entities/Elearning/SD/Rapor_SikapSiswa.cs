using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Rapor_SikapSiswa
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_Sikap { get; set; }
        public string Rel_Siswa { get; set; }
        public string Rapor { get; set; }
    }
}