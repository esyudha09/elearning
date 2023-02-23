using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_StrukturNilai_KTSP
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_Kelas { get; set; }
        public Guid Rel_Mapel { get; set; }
        public decimal KKM { get; set; }
        public string Deskripsi { get; set; }
        public bool IsNilaiAkhir { get; set; }
        public string DeskripsiSikapSosial { get; set; }
        public string DeskripsiSikapSpiritual { get; set; }
    }
}