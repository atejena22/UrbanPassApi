using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace inTouchApi.Models
{
    public class Roles
    {

        string sConn = ConfigurationManager.AppSettings("connectionStrings:SQLConn");

        public Role getRole(int roleID)
        {

            Role objResponse = new Role();

            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" SELECT  * from intouch.roles where roleID =" + roleID);

                // Open connection
                using (MySqlConnection c = new MySqlConnection(sConn))
                {
                    c.Open();
                    // Create new DataAdapter
                    using (MySqlDataAdapter a = new MySqlDataAdapter(sb.ToString(), c))
                    {
                        // Use DataAdapter to fill DataTable
                        DataTable t = new DataTable();
                        a.Fill(t);
                        if (t.Rows.Count > 0)
                        {
                            foreach (DataRow r in t.Rows)
                            {
                                objResponse.roleID = SharedFunctions.ValidateInteger(r["roleID"]);
                                objResponse.roleName = SharedFunctions.ValidateString(r["roleName"]);
                            }
                        }
                    }
                    c.Close();
                }
            }
            catch (Exception ex)
            {
                objResponse.hasError = true;
                objResponse.errorDesc = ex.Message.ToString();
                objResponse.errorInter = ex.Message.ToString();
                objResponse.errorNum = 201;
            }

            return objResponse;

        }

        public RoleList getRoles(int roleID)
        {

            RoleList objResponse = new RoleList();

            List<Role> ObjRoles = new List<Role>();

            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" SELECT  * from intouch.users Roles roleID =" + roleID);

                // Open connection
                using (MySqlConnection c = new MySqlConnection(sConn))
                {
                    c.Open();
                    // Create new DataAdapter
                    using (MySqlDataAdapter a = new MySqlDataAdapter(sb.ToString(), c))
                    {
                        // Use DataAdapter to fill DataTable
                        DataTable t = new DataTable();
                        a.Fill(t);
                        if (t.Rows.Count > 0)
                        {
                            foreach (DataRow r in t.Rows)
                            {
                                Role ObjRole = new Role();
                                ObjRole.roleID = SharedFunctions.ValidateInteger(r["roleID"]);
                                ObjRole.roleName = SharedFunctions.ValidateString(r["roleName"]);
                                ObjRoles.Add(ObjRole);
                            }
                            objResponse.rolesList = ObjRoles;
                        } 
                      
                    }
                    c.Close();
                }
            }
            catch (Exception ex)
            {
                objResponse.hasError = true;
                objResponse.errorDesc = ex.Message.ToString();
                objResponse.errorInter = ex.Message.ToString();
                objResponse.errorNum = 201;
            }

            return objResponse;

        }


    }

    public class RoleList : ErrorInfo
    {
        public List<Role> rolesList { get; set; }

        public RoleList()
        {
        }
    }

    public class Role : ErrorInfo
    {
        public int roleID { get; set; }
        public string roleName { get; set; }

        public Role()
        {
            roleID = 0;
            roleName = "";

        }
    }
}
