using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.KB
{
    public static class PosisiUpload
    {
        public const string GURU = "GURU";
        public const string PIMSEK = "PIMSEK";
        public const string CLOSED = "CLOSED";
    }

    public class Rapor_UploadHistKelas
    {
        public Guid Kode { get; set; }
        public DateTime Tanggal { get; set; }
        public string PosisiUpload { get; set; }
        public string TahunAjaran { get; set; }
        public string Semester { get; set; }
        public string Rel_KelasDet { get; set; }
        public string JenisRapor { get; set; }
    }

    public class Rapor_UploadHistSiswa : Rapor_UploadHistKelas
    {
        public string Rel_Siswa { get; set; }
    }    
}