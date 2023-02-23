using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class PegawaiRiwayatKesehatanMCU
    {
        public Guid Kode { get; set; }
        public string Rel_Pegawai { get; set; }
        public DateTime Tanggal { get; set; }
        public string Kesimpulan { get; set; }
        public string Saran { get; set; }
        public string Keterangan { get; set; }
        public int Urutan { get; set; }
    }
}