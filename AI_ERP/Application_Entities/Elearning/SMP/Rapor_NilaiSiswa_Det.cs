using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public class Rapor_NilaiSiswa_Det
    {
        public Guid Kode { get; set; }
        public Guid Rel_Rapor_NilaiSiswa { get; set; }
        public string Rel_Siswa { get; set; }
        public string Rel_Rapor_StrukturNilai_AP { get; set; }
        public string Rel_Rapor_StrukturNilai_KD { get; set; }
        public string Rel_Rapor_StrukturNilai_KP { get; set; }
        public string Nilai { get; set; }
        public string PB { get; set; }
    }

    public class Rapor_NilaiSiswa_Det_Extend : Rapor_NilaiSiswa_Det
    {
        public string Rel_KelasDet { get; set; }
        public string Rel_Mapel { get; set; }
        public string Rel_Rapor_AspekPenilaian { get; set; }
        public string Rel_Rapor_KompetensiDasar { get; set; }
        public string Rel_Rapor_KomponenPenilaian { get; set; }
        public string NamaKD { get; set; }
        public string MateriKP { get; set; }
        public string DeskripsiKP { get; set; }
        public int UrutanAP { get; set; }
        public int UrutanKD { get; set; }
        public int UrutanKP { get; set; }
        public string LTS_HD { get; set; }
        public string LTS_MAX_HD { get; set; }
    }
}