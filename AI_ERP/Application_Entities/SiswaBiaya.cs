using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class SiswaBiaya
    {
        public Guid Kode { get; set; }
        public string TahunAjaran { get; set; }
        public string Rel_Sekolah { get; set; }
        public string Rel_Siswa { get; set; }
        public string Rel_Kelas { get; set; }
        public string Rel_KelasDet { get; set; }
        public Guid Rel_ItemBiaya { get; set; }
        public int PeriodeTagih { get; set; }
        public decimal Jumlah { get; set; }
        public decimal Denda { get; set; }
        public string Keterangan { get; set; }
        public bool IsBebas { get; set; }
        public bool IsBebasDenda { get; set; }
        public int Urut { get; set; }
    }
}