using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_Pengaturan
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string KepalaSekolah { get; set; }
        public string KurikulumRaporLevel10 { get; set; }
        public string KurikulumRaporLevel11 { get; set; }
        public string KurikulumRaporLevel12 { get; set; }
        public string TemplateEmailLTS { get; set; }
        public string TemplateEmailRapor { get; set; }
        public DateTime TanggalBukaLinkRapor { get; set; }
        public DateTime TanggalBukaLinkLTS { get; set; }
    }
}