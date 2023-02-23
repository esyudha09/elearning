using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class MapelJadwalDetPukul
    {
        public Guid Kode { get; set; }
        public string Rel_MapelJadwalDet { get; set; }
        public DateTime DariJam { get; set; }
        public DateTime SampaiJam { get; set; }
    }
}