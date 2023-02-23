using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class FormasiGuruKelas
    {
        public Guid Kode { get; set; }
        public Guid Rel_Sekolah { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_GuruKelas { get; set; }
        public string Rel_GuruPendamping { get; set; }
        public string Rel_KelasDet { get; set; }
    }

    public class FormasiGuruKelas_ByGuru
    {
        public Guid Rel_Sekolah { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_KelasDet { get; set; }
        public string KelasDet { get; set; }
        public string Mapel { get; set; }
        public string KodeMapel { get; set; }
        public double Urutan { get; set; }
    }
}