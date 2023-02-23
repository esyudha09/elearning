using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class LinimasaKelas
    {
        public Guid Kode { get; set; }
        public DateTime Tanggal { get; set; }
        public string Jenis { get; set; }
        public string TahunAjaran { get; set; }
        public string Rel_KelasDet { get; set; }
        public string Keterangan { get; set; }
        public DateTime TanggalUpdate { get; set; }
        public string RTG_UNIT { get; set; }
        public string RTG_LEVEL { get; set; }
        public string RTG_SEMESTER { get; set; }
        public string RTG_KELAS { get; set; }
        public string RTG_SUBKELAS { get; set; }
        public string ATTRIB_01 { get; set; }
        public string ATTRIB_02 { get; set; }
        public string ATTRIB_03 { get; set; }
        public string ATTRIB_04 { get; set; }
        public string ATTRIB_05 { get; set; }
        public string ATTRIB_06 { get; set; }
        public string ATTRIB_07 { get; set; }
        public string ATTRIB_08 { get; set; }
        public string ATTRIB_09 { get; set; }
        public string ATTRIB_10 { get; set; }
        public string ACT { get; set; }
        public string ACT_KETERANGAN { get; set; }
    }
}