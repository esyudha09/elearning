using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning
{
    public class SiswaAbsenRekapDet
    {
        public Guid Kode { set; get; }
        public string Rel_SiswaAbsenRekap { set; get; }
        public string Rel_Siswa { set; get; }
        public string Hadir { set; get; }
        public string JumlahPertemuan { set; get; }
        public string Sakit { set; get; }
        public string Izin { set; get; }
        public string Alpa { set; get; }
        public string Terlambat { set; get; }

    }
}