using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class PegawaiPendidikan
    {
        public Guid Kode { get; set; }
        public string Rel_Pegawai { get; set; }
        public string JenisPendidikan { get; set; }
        public string Rel_Lembaga { get; set; }
        public string Lembaga { get; set; }
        public string DariTahun { get; set; }
        public string SampaiTahun { get; set; }
        public string NilaiAkhir { get; set; }
        public string Rel_Jurusan { get; set; }
        public string Jurusan { get; set; }
        public string Keterangan { get; set; }
        public int Urutan { get; set; }
    }
}