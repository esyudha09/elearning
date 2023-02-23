using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD.Reports
{
    public class RaporLTSCapaianKedisiplinan
    {
        public string Rel_Siswa { get; set; }
        public string KodeKelompokMapel { get; set; }
        public string KelompokMapel { get; set; }
        public string NomorMapel { get; set; }
        public int UrutanMapel { get; set; }
        public string Rel_Mapel { get; set; }
        public string NamaMapel { get; set; }
        public string Kehadiran { get; set; }
        public string KetepatanWaktu { get; set; }
        public string PenggunaanSeragam { get; set; }
        public string PenggunaanKamera { get; set; }
    }
}