using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD.Reports
{
    public class KTSP_RaporVolunteer
    {
        public string IDSiswa { get; set; }
        public string Kelas { get; set; }
        public string Kegiatan { get; set; }
        public string Durasi { get; set; }
        public decimal AkumulasiWaktu { get; set; }
    }
}