using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD.Reports
{
    public class KTSP_RaporSikap
    {
        public string IDSiswa { get; set; }
        public string Aspek { get; set; }
        public decimal Nilai { get; set; }
        public string Predikat { get; set; }
    }
}