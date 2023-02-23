using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Volunteer_Det
    {
        public Guid Kode { get; set; }
        public Guid Rel_Volunteer { get; set; }
        public string Rel_Siswa { get; set; }
        public string Kegiatan { get; set; }
        public DateTime Tanggal { get; set; }
        public string JumlahJam { get; set; }
        public string Keterangan { get; set; }
    }
}