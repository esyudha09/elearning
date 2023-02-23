using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class PegawaiRiwayatKesehatan
    {
        public Guid Kode { get; set; }
        public string Rel_Pegawai { get; set; }
        public DateTime DariTanggal { get; set; }
        public DateTime SampaiTanggal { get; set; }
        public bool IsIzin { get; set; }
        public string NamaPenyakit { get; set; }
        public string RSKlinik { get; set; }
        public string Dokter { get; set; }
        public string Keterangan { get; set; }
        public int Urutan { get; set; }
    }
}