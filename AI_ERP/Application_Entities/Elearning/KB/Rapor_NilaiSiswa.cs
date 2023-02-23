using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.KB
{
    public class Rapor_NilaiSiswa
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_Nilai { get; set; }
        public string Rel_Siswa { get; set; }
        public string BeratBadan { get; set; }
        public string TinggiBadan { get; set; }
        public string LingkarKepala { get; set; }
        public string Usia { get; set; }
        public bool IsPosted { get; set; }
        public bool IsLocked { get; set; }
    }
}