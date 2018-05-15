using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPKALE_Cryptography
{
    class Program
    {
        static void Main(string[] args)
        {
            CLA cla = new CLA();
            crypto c = new crypto();
            Console.WriteLine("Enter Your SSN to validate. or Enter 0 for result. \n");

            String ssn = Console.ReadLine();
            if(ssn.Equals("0") == true)
            {
                CTF ctf = new CTF();
                ctf.showResult();
            }else
            {

            
            Console.WriteLine();
            if(cla.authenticate_user(c.Encrypt(ssn, ssn, true), ssn) == true)
            {
              
                DataTable dt_votechk = new DataTable();
                string connString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                string query = "select * from tbl_CTF_Directory where ssn = '" + ssn + "'";

                SqlConnection conn = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                
                da.Fill(dt_votechk);
                conn.Close();
                da.Dispose();

                if(dt_votechk.Rows.Count > 0)
                {
                    Console.WriteLine("already voted.");
                }
                else
                {
                    DataTable dt_voter = new DataTable();
                     connString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                     query = "select * from tbl_voter_master where ssn = '" + ssn + "'";

                     conn = new SqlConnection(connString);
                     cmd = new SqlCommand(query, conn);
                    conn.Open();
                    
                    
                     da = new SqlDataAdapter(cmd);
                    
                    da.Fill(dt_voter);
                    conn.Close();
                    da.Dispose();
                    string validation = c.Encrypt(dt_voter.Rows[0]["validation_key"].ToString(), ssn, true);
                    Console.WriteLine("***********************************************");
                    Console.WriteLine("here are voter's list.");
                  

                    DataTable dt_can = new DataTable();
                    string connString1 = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                    string query1 = "select id, c_name from tbl_candidate_master";

                    SqlConnection conn1 = new SqlConnection(connString1);
                    SqlCommand cmd1 = new SqlCommand(query1, conn1);
                    conn1.Open();

                    
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    
                    da1.Fill(dt_can);
                    conn1.Close();
                    da1.Dispose();

                    for(int i = 0; i < dt_can.Rows.Count; i++)
                    {
                        Console.WriteLine("press "+dt_can.Rows[i]["id"].ToString()+" for " + dt_can.Rows[i]["c_name"].ToString());
                    }
                    Console.WriteLine("***********************************************");
                    Console.Write("Enter yout vote:   ");
                    string vote = Console.ReadLine();
                        
                        
                        DataTable dt = new DataTable();
                   
                     connString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                     query = "insert into tbl_CTF_Directory (ssn, vote, validation) values(" + Convert.ToInt32(ssn) + ", '" + c.Encrypt(vote, ssn, true) + "', '" + c.Decrypt(validation, ssn, true) + "')";

                     conn = new SqlConnection(connString);
                     cmd = new SqlCommand(query, conn);
                    conn.Open();

                    
                     da = new SqlDataAdapter(cmd);
                   
                    da.Fill(dt);
                    conn.Close();
                    da.Dispose();


                    Console.WriteLine("vote is secured.");
                }
                

            }else
            {
                Console.WriteLine("invalid ssn.");
            }
            
            }
            Console.ReadLine();
        }

       
    }
}
