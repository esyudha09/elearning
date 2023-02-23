using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class SiswaSaudara
    {
        public Guid Rel_Siswa { set; get; }
        public int Urut { set; get; }
        public string Hubungan { set; get; }
        public string Nama { set; get; }
        public string JenisKelamin { set; get; }
        public string Umur { set; get; }
        public string Sekolah { set; get; }
        public bool IsSaudaraKandung { set; get; }
        public string Keterangan { set; get; }
        public string KeteranganLain { set; get; }
    }
}