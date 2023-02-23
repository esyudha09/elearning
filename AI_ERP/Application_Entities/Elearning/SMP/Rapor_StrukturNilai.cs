using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public class Rapor_StrukturNilai
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public Guid Rel_Kelas { get; set; }
        public Guid Rel_Mapel { get; set; }
        public decimal KKM { get; set; }
        public string JenisPerhitungan { get; set; }
        public string DeskripsiUmum { get; set; }
        public string DeskripsiPengetahuan { get; set; }
        public string DeskripsiKeterampilan { get; set; }
        public bool Is_PH_PTS_PAS { get; set; }
        public decimal BobotPH { get; set; }
        public decimal BobotPTS { get; set; }
        public decimal BobotPAS { get; set; }
        public string Rel_Kelas2 { get; set; }
        public string Rel_Kelas3 { get; set; }
        public bool IsNilaiAkhir { get; set; }
        public string DeskripsiSikapSpiritual { get; set; }
        public string DeskripsiSikapSosial { get; set; }
    }
}