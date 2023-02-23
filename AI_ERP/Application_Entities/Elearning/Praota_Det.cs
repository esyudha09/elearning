using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning
{
    public class Praota_Det
    {
        public Guid Kode { get; set; }
        public Guid Rel_Praota { get; set; }
        public string Semester { get; set; }
        public string MateriPokok { get; set; }
        public string DeskripsiMateriPokok { get; set; }
        public string AlokasiWaktu { get; set; }
        public string EstimasiWaktuDari { get; set; }
        public string EstimasiWaktuSampai { get; set; }
        public string Keterangan { get; set; }
        public string JenisFile { get; set; }
        public string URLEmbed { get; set; }
        public int Urutan { get; set; }
    }
}