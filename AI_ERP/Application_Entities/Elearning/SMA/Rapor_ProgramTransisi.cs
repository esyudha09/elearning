using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SMA
{
    public class Rapor_ProgramTransisi
    {
        public Guid Kode { set; get; }
        public string TahunAjaran { set; get; }
        public string Semester { set; get; }
        public string Rel_KelasDet { set; get; }
        public string Rel_Siswa { set; get; }
        public string LayananSosial { set; get; }
        public string LayananSosial_JumlahJam { set; get; }
        public string LayananSosial_Keterangan { set; get; }
        public string Kewirausahaan { set; get; }
        public string Kewirausahaan_JumlahJam { set; get; }
        public string Kewirausahaan_Keterangan { set; get; }
        public string Internship { set; get; }
        public string Internship_JumlahJam { set; get; }
        public string Internship_Keterangan { set; get; }

    }
}