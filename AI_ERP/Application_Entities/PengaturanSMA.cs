using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class PengaturanSMA
    {
        public Guid Kode { get; set; }
        public string HeaderLogo { get; set; }
        public string HeaderKop { get; set; }
        public string HeaderAlamat { get; set; }
        public bool IsTestEmail { get; set; }
        public string TestEmail { get; set; }
        public string TeksLinkLTS { get; set; }
        public int ExpiredLinkLTSHari { get; set; }
        public int ExpiredLinkLTSJam { get; set; }
        public int ExpiredLinkLTSMenit { get; set; }
        public string TemplateHTMLLinkExpired { get; set; }
        public string JenisFileRapor { get; set; }
    }
}