using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Rapor_Pengaturan
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string KepalaSekolah { get; set; }
        public string KurikulumRaporLevel1 { get; set; }
        public string KurikulumRaporLevel2 { get; set; }
        public string KurikulumRaporLevel3 { get; set; }
        public string KurikulumRaporLevel4 { get; set; }
        public string KurikulumRaporLevel5 { get; set; }
        public string KurikulumRaporLevel6 { get; set; }
        public string TemplateEmailLTS { get; set; }
        public string TemplateEmailRapor { get; set; }
        public DateTime TanggalBukaLinkRapor { get; set; }
        public DateTime TanggalBukaLinkLTS { get; set; }
    }
}