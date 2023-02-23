using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class MapelJadwalDetPukulDet
    {
        public Guid Kode { get; set; }
        public string Rel_MapelJadwalDetPukul { get; set; }
        public string Rel_KelasDet { get; set; }
        public string Rel_Mapel { get; set; }
    }
}