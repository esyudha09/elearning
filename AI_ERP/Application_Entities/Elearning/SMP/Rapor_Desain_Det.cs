using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public class Rapor_Desain_Det
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_Desain { get; set; }
        public string Rel_Mapel { get; set; }
        public string NamaMapelRapor { get; set; }
        public string Nomor { get; set; }
        public string Poin { get; set; }
        public int Urutan { get; set; }
        public string Alias { get; set; }
    }
}