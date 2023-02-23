using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class PegawaiPengalamanSharing
    {
        public Guid Kode { get; set; }
        public string Rel_Pegawai { get; set; }
        public string Tahun { get; set; }
        public string Topik { get; set; }
        public string Penyelenggara { get; set; }
        public string Kota { get; set; }
        public int Urutan { get; set; }
    }
}