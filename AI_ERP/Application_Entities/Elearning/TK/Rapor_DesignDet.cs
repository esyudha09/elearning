using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.TK
{
    public enum JenisKomponenRapor
    {
        KategoriPencapaian,
        SubKategoriPencapaian,
        PoinKategoriPencapaian,
        Rekomendasi,
        MapelEkskul
    }

    public class Rapor_DesignDet
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_Design { get; set; }
        public string Poin { get; set; }
        public Guid Rel_KomponenRapor { get; set; }
        public JenisKomponenRapor JenisKomponen { get; set; }
        public string NamaKomponen { get; set; }
        public bool IsNewPage { get; set; }
        public bool IsLockGuruKelas { get; set; }
        public int Urut { get; set; }
    }
}