using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class _MutasiKantin
    {
        public string NoKasir { get; set; }
        public DateTime Tanggal { get; set; }
        public decimal SaldoAwal { get; set; }
        public decimal Total { get; set; }
        public decimal SaldoAkhir { get; set; }
        public string Keterangan { get; set; }
    }
}