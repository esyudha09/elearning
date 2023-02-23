using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_StrukturNilai_KURTILAS_Sikap
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_StrukturNilai { get; set; }
        public string Poin { get; set; }
        public Guid Rel_Rapor_KompetensiDasar { get; set; }
        public string Deskripsi { get; set; }
        public int Urutan { get; set; }
    }
}