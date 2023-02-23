using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class Kelas
    {
        public Guid Kode { get; set; }
        public Guid Rel_Sekolah { get; set; }
        public string Nama { get; set; }
        public int UrutanLevel { get; set; }
        public string Keterangan { get; set; }
        public bool IsAktif { get; set; }
    }
}