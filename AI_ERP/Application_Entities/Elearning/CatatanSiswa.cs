using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning
{
    public class CatatanSiswa
    {
        public Guid Kode { get; set; }
        public string Rel_Siswa { get; set; }
        public string Rel_Kategori { get; set; }
        public DateTime Tanggal { get; set; }
        public string Catatan { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_KelasDet { get; set; }
        public string Rel_Mapel { get; set; }
        public string Rel_Guru { get; set; }
    }

    public class CatatanSiswaByGuru : CatatanSiswa
    {
        public string Siswa { get; set; }
        public string Kelas { get; set; }
    }

    public class CatatanSiswaKelasPeriode
    {
        public Guid Kode { get; set; }
        public string Kelas { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
    }
}