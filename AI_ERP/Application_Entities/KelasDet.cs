using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class KelasDet
    {
        public Guid Kode { get; set; }
        public Guid Rel_Kelas { get; set; }
        public string Nama { get; set; }
        public int UrutanKelas { get; set; }
        public string Keterangan { get; set; }
        public bool IsKelasJurusan { get; set; }
        public bool IsKelasSosialisasi { get; set; }
        public bool IsAktif { get; set; }
    }
}