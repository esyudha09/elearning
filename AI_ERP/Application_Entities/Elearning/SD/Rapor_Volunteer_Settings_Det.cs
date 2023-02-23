using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public class Rapor_Volunteer_Settings_Det
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_Volunteer_Settings { get; set; }
        public int Urutan { get; set; }
        public string Rel_Mapel { get; set; }
        public decimal Durasi { get; set; }
    }
}