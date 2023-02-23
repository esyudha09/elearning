using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;

namespace AI_ERP.Application_DAOs
{
    public class DAO_UserOrtuDet
    {
        public const string SP_SELECT_BY_USER_ID = "UserOrtuDet_SELECT_BY_USER_ID";

        public static List<UserOrtuDet> SelectByUserID(string userid)
        {
            List<UserOrtuDet> hasil = new List<UserOrtuDet>();

            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand(SP_SELECT_BY_USER_ID, conn);
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@UserID", userid);
                
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    hasil.Add(new UserOrtuDet {
                        Rel_UserOrtu = new Guid(sdr["Rel_UserOrtu"].ToString()),
                        NIS = sdr["NIS"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }
    }
}