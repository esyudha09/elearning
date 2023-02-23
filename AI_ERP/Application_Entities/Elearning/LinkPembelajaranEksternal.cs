using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning
{
    public class LinkPembelajaranEksternal
    {
        public Guid Kode { get; set; }
        public string Kategori { get; set; }
        public string Unit { get; set; }
        public string Nama { get; set; }
        public string Link { get; set; }
        public string RTG_UNIT { get; set; }
        public string RTG_LEVEL { get; set; }
        public string RTG_SEMESTER { get; set; }
        public string RTG_KELAS { get; set; }
        public string RTG_SUBKELAS { get; set; }
        public string Rel_Pegawai { get; set; }
        public DateTime TanggalBuat { get; set; }
        public DateTime TanggalUpdate { get; set; }
    }
}