using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_NilaiSikap
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_Mapel { get; set; }
        public string Rel_KelasDet { get; set; }
    }

    public class Rapor_NilaiSikap_For_Rapor
    {
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_Kelas { get; set; }
        public string Rel_Siswa { get; set; }
        public string SikapSpiritual { get; set; }
        public string SikapSosial { get; set; }
        public string DeskripsiSikapSpiritual { get; set; }
        public string DeskripsiSikapSosial { get; set; }
    }
}
