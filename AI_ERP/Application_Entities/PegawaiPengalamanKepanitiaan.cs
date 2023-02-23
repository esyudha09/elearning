using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class PegawaiPengalamanKepanitiaan
    {
        public Guid Kode { get; set; }
        public string Rel_Pegawai { get; set; }
        public string Tahun { get; set; }
        public string Kegiatan { get; set; }
        public string Jabatan { get; set; }
        public string NoSuratTugas { get; set; }
        public string Keterangan { get; set; }
        public int Urutan { get; set; }
    }
}