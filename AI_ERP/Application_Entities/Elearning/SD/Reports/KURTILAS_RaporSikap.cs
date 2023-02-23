using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities.Elearning.SD.Reports
{
    public class KURTILAS_RaporSikap
    {
        public string IDSiswa { get; set; }
        public string Aspek { get; set; }
        public string Indikator { get; set; }
        public decimal Kemampuan { get; set; }
    }
}