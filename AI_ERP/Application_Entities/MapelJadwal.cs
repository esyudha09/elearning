using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
	public class MapelJadwal
	{
		public Guid Kode { set; get; }
        public string Rel_Kode { set; get; }
        public string Rel_Sekolah { set; get; }
		public string TahunAjaran { set; get; }
		public string Semester { set; get; }
		public string JenisPengaturan { set; get; }
		public DateTime PeriodeDariTanggal { set; get; }
		public DateTime PeriodeSampaiTanggal { set; get; }
		public DateTime CopyPeriodeDariTanggal { set; get; }
		public DateTime CopyPeriodeSampaiTanggal { set; get; }
		public DateTime CreatedDate { set; get; }
		public DateTime LastUpdated { set; get; }
		public string CreatedBy { set; get; }
		public string LastUpdatedBy { set; get; }
	}
}