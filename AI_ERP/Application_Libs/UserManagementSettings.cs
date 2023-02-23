using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AI_ERP.Application_Libs
{
    public static class UserManagementSettings
    {
        public static class JenisUser
        {
            public const string GURU = "8E5C4CCC-CFF0-4BFE-A05F-2B82F0CCEBDF";
            
            public const string TU_KB = "DD979DA2-3FB3-424C-9ABF-4B09DD6ECF53";
            public const string TU_TK = "0CFBE955-AF15-48C8-8227-6EB342B17FED";
            public const string TU_SD = "37D5F637-58E1-4833-9715-685C5C07CDFE";
            public const string TU_SMP = "26ABEC29-98D5-475C-84EE-7F7611C056A3";
            public const string TU_SMA = "7DE60254-C8E8-44AC-853F-BBC03EE93784";

            public const string PIMPINAN_KB = "AC2B407A-A446-4B16-AE4E-FD4BE2504C40";
            public const string PIMPINAN_TK = "6FFEB5A6-FCCF-4CB5-A3E3-F2133E5B55F0";
            public const string PIMPINAN_SD = "4922D3A6-B7DA-4CBF-8212-FB1625CDBFCD";
            public const string PIMPINAN_SMP = "5E6AEE37-80AB-49C4-A61C-A71C91E5B726";
            public const string PIMPINAN_SMA = "B44D869B-EC21-4E6C-9D89-61A01C4FA7C2";

            public class JenisUserEntity
            {
                public string Kode { get; set; }
                public string Nama { get; set; }
            }

            public static List<JenisUserEntity> GetListJenis()
            {
                List<JenisUserEntity> hasil = new List<JenisUserEntity>();
                hasil.Clear();

                hasil.Add(new JenisUserEntity { Kode = GURU, Nama = "Guru" });

                hasil.Add(new JenisUserEntity { Kode = TU_KB, Nama = "Tata Usaha (TU) KB" });
                hasil.Add(new JenisUserEntity { Kode = TU_TK, Nama = "Tata Usaha (TU) TK" });
                hasil.Add(new JenisUserEntity { Kode = TU_SD, Nama = "Tata Usaha (TU) SD" });
                hasil.Add(new JenisUserEntity { Kode = TU_SMP, Nama = "Tata Usaha (TU) SMP" });
                hasil.Add(new JenisUserEntity { Kode = TU_SMA, Nama = "Tata Usaha (TU) SMA" });

                hasil.Add(new JenisUserEntity { Kode = PIMPINAN_KB, Nama = "Pimpinan KB" });
                hasil.Add(new JenisUserEntity { Kode = PIMPINAN_TK, Nama = "Pimpinan TK" });
                hasil.Add(new JenisUserEntity { Kode = PIMPINAN_SD, Nama = "Pimpinan SD" });
                hasil.Add(new JenisUserEntity { Kode = PIMPINAN_SMP, Nama = "Pimpinan SMP" });
                hasil.Add(new JenisUserEntity { Kode = PIMPINAN_SMA, Nama = "Pimpinan SMA" });

                return hasil;
            }

            public static void ListUserToDropdown(DropDownList cbo)
            {
                cbo.Items.Clear();
                cbo.Items.Add("");
                foreach (var item in GetListJenis())
                {
                    cbo.Items.Add(new ListItem {
                        Value = item.Kode,
                        Text = item.Nama
                    });
                }
            }
        }
    }
}