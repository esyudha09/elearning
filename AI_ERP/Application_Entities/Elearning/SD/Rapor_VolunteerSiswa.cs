using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Rapor_VolunteerSiswa
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_Volunteer { get; set; }
        public string Rel_Siswa { get; set; }
        public string Rel_Rapor_Volunteer_Settings_Det { get; set; }
        public string Nilai { get; set; }
    }
}