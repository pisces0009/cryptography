using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FPKALE_Cryptography
{
    class CLA
    {
        public bool authenticate_user(string ssn1, string key)
        {
            try
            {
                crypto c = new crypto();
                int ssn = Convert.ToInt32(c.Decrypt(ssn1, key, true));
               


                DataTable dt_auth = new DataTable();
                string connString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                string query = "select * from tbl_voter_master where ssn = "+ssn;

                SqlConnection conn = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                
                da.Fill(dt_auth);
                conn.Close();
                da.Dispose();
                


                if (dt_auth.Rows.Count > 0)
                {
                    TripleDESCryptoServiceProvider TDES = new TripleDESCryptoServiceProvider();
                    TDES.GenerateIV();
                    TDES.GenerateKey();


                    SqlParameter[] sp_validation_key = new SqlParameter[2];
                    sp_validation_key[0] = new SqlParameter("@ssn", ssn);
                    string validation = c.RandomString();
                    sp_validation_key[1] = new SqlParameter("@validation", validation);
                    
                    string query1 = "update tbl_voter_master set validation_key = '"+ validation + "' where ssn = " + ssn;

                    SqlConnection conn1 = new SqlConnection(connString);
                    SqlCommand cmd1 = new SqlCommand(query1, conn1);
                    conn1.Open();

                    
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    
                    da1.Fill(dt_auth);
                    conn1.Close();
                    da1.Dispose();
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
