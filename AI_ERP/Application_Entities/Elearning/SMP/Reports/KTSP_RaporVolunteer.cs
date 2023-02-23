using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP.Reports
{
    public class KTSP_RaporVolunteer
    {
        public string IDSiswa { get; set; }
        public string Kegiatan { get; set; }
        public DateTime Tanggal { get; set; }
        public string JumlahJam { get; set; }
        public DateTime TanggalKegiatan { get; set; }
        public string Keterangan { get; set; }
    }
}