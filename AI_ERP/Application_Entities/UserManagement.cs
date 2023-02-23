using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Entities
{
    public class UserManagement
    {
        public Guid Kode { get; set; }
        public string UserID { get; set; }
        public bool IsActive { get; set; }
        public string JenisUser { get; set; }
    }
}