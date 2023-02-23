using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.KB
{
    public class Rapor_Arsip
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string JenisRapor { get; set; }
        public string KepalaSekolah { get; set; }
        public DateTime TanggalAwalAbsen { get; set; }
        public DateTime TanggalAkhirAbsen { get; set; }
        public DateTime TanggalClosing { get; set; }
        public DateTime TanggalRapor { get; set; }
        public string Keterangan { get; set; }
        public bool IsArsip { get; set; }
        public string TemplateEmailRapor { get; set; }
        public DateTime TanggalBukaLinkRapor { get; set; }
    }
}