using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
	public class MapelJadwalDet
	{
		public Guid Kode { set; get; }
		public string Rel_MapelJadwal { set; get; }
		public DateTime Tanggal { set; get; }
		public DateTime Pukul { set; get; }
		public int LamaMenit { set; get; }
		public int JamKe { set; get; }
		public string Rel_KelasDet { set; get; }
		public string Rel_Mapel { set; get; }
        public bool IsLibur { set; get; }
    }
}