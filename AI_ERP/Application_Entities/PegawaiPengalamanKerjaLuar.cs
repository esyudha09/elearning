using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class PegawaiPengalamanKerjaLuar
    {
        public Guid Kode { get; set; }
        public string Rel_Pegawai { get; set; }
        public string NamaPerusahaan { get; set; }
        public string Dari { get; set; }
        public string Sampai { get; set; }
        public string Rel_Jabatan { get; set; }
        public string Jabatan { get; set; }
        public int Urutan { get; set; }
    }
}