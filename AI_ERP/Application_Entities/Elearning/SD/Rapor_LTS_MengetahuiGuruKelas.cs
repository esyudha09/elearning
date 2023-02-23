using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Rapor_LTS_MengetahuiGuruKelas
    {
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_KelasDet { get; set; }
        public string NamaGuru { get; set; }
        public DateTime Tanggal { get; set; }
    }
}