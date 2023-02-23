using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class PesanGuruOrtu
    {
        public Guid Kode { get; set; }
        public DateTime Tanggal { get; set; }
        public string Rel_Pegawai { get; set; }
        public string Rel_Ortu { get; set; }
        public string Pesan { get; set; }
    }
}