using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using inTouchApi.Model;

namespace inTouchApi.Models
{
    public class Users : ErrorInfo
    {

        string sConn = ConfigurationManager.AppSettings("connectionStrings:SQLConn");

        string sImages = ConfigurationManager.AppSettings("appSettings:imagesPath");
        string sIcons = ConfigurationManager.AppSettings("appSettings:iconsPath");
        string sImagesPerfil = ConfigurationManager.AppSettings("appSettings:imagesPerfil");

        public User getUser(LoginRequest inObj)
        {

            User objUser = new User();

            try
            { 

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" SELECT  * from intouch.users where userName ='" + inObj.userName + "' and password = '" + inObj.password + "'  ");

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
                                objUser.userID = SharedFunctions.ValidateInteger(r["userID"]);
                                objUser.roleID = SharedFunctions.ValidateInteger(r["roleID"]);
                                objUser.email = SharedFunctions.ValidateString(r["email"]);
                                objUser.password = SharedFunctions.ValidateString(r["password"]);
                                objUser.firstName = SharedFunctions.ValidateString(r["firstName"]);
                                objUser.lastName = SharedFunctions.ValidateString(r["lastName"]);
                                objUser.active = SharedFunctions.ValidateBoolean(r["active"]);
                                objUser.deleted = SharedFunctions.ValidateBoolean(r["deleted"]);
                                objUser.emailConfirmation = SharedFunctions.ValidateBoolean(r["emailConfirmation"]);
                                objUser.activeFrom = SharedFunctions.ValidateDatetime(r["activeFrom"]);
                                objUser.activeTo = SharedFunctions.ValidateDatetime(r["activeTo"]);
                                objUser.lastLogin = SharedFunctions.ValidateDatetime(r["lastLogin"]);
                               
                            }

                        }
                        else
                        {
                            objUser.hasError = true;
                            objUser.errorDesc = "Usuario o clave no válida";
                            objUser.errorInter = "Usuario o clave no válida";
                            objUser.errorNum = 302;
                        }
                    }
                    c.Close();
                }
            }
            catch (Exception ex)
            {
                objUser.hasError = true;
                objUser.errorDesc = ex.Message.ToString();
                objUser.errorInter = ex.Message.ToString();
                objUser.errorNum = 301;
            }
            /***************************************************** AÑADIDO 21/03/2022 ************************************************************************************/
            if (objUser.roleID==3)
            {
                return objUser;
            }
            else
            {
                objUser.hasError = true;
                objUser.errorDesc = "Su cuenta no tiene acceso para acceder a la App Móvil";
                return objUser;
            }
            /***************************************************** AÑADIDO 21/03/2022 ************************************************************************************/

        }

        public UserLogin getUser(int userID, string token = "") 
        {
            UserLogin objUser = new UserLogin();
            objUser.token = token;
            /*****************************************************************************************************************/
            #region Actualiza el ultimo acceso de Usuario
            System.Text.StringBuilder sbd = new System.Text.StringBuilder();
            sbd.Append("UPDATE intouch.users SET lastLogin = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where  userID = " + userID);

            // Open connection
            using (MySqlConnection conn = new MySqlConnection(sConn))
            {
                MySqlCommand cmd = new MySqlCommand(sbd.ToString(), conn);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch
                {
                }
            }
            #endregion
            /*****************************************************************************************************************/

            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" SELECT  * from intouch.users where userID =" + userID);

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
                                objUser.userID = SharedFunctions.ValidateInteger(r["userID"]);
                                objUser.roleID = SharedFunctions.ValidateInteger(r["roleID"]);
                                objUser.email = SharedFunctions.ValidateString(r["email"]);
                                objUser.userName = SharedFunctions.ValidateString(r["userName"]);
                                objUser.password = SharedFunctions.ValidateString(r["password"]);
                                objUser.firstName = SharedFunctions.ValidateString(r["firstName"]);
                                objUser.lastName = SharedFunctions.ValidateString(r["lastName"]);
                                objUser.active = SharedFunctions.ValidateBoolean(r["active"]);
                                objUser.deleted = SharedFunctions.ValidateBoolean(r["deleted"]);
                                objUser.emailConfirmation = SharedFunctions.ValidateBoolean(r["emailConfirmation"]);
                                objUser.activeFrom = SharedFunctions.ValidateDatetime(r["activeFrom"]);
                                objUser.activeTo = SharedFunctions.ValidateDatetime(r["activeTo"]);
                                objUser.lastLogin = SharedFunctions.ValidateDatetime(r["lastLogin"]);
                                objUser.userParentID = SharedFunctions.ValidateInteger(r["userParentID"]);


                                // if (SharedFunctions.ValidateString(r["image"]) == "")
                               //  {
                                  //   objUser.image = sIcons + "user100px.png";
                               //  }
                               //  else
                               //  {
                                 // objUser.image = sImages + SharedFunctions.ValidateString(r["image"]);
                                objUser.image = sImages + SharedFunctions.ValidateString(r["image"]);
                               // }

                                Houses objHouses = new Houses();
                                objUser.houses = objHouses.getHousesByUser(objUser.userID);

                                if (objUser.roleID > 0)
                                {
                                    Roles objRoles = new Roles();
                                    objUser.role = objRoles.getRole(objUser.roleID);
                                }
                            }
                        }                       
                    }
                    c.Close();
                }
            }
            catch (Exception ex)
            {
                objUser.hasError = true;
                objUser.errorDesc = ex.Message.ToString();
                objUser.errorInter = ex.Message.ToString();
                objUser.errorNum = 304;
            }

            return objUser;

        }

        public List<User> getUsers(int userID)
        {
            /*inTouchApi.Model.User usu = new inTouchApi.Model.User();
            var usuario = _context.Users.Where(x => x.UserId == userID).FirstOrDefault();
            usuario.lastLogin = DateTime.Now;
            _context.Entry(usuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();*/


            List<User> users = new List<User>();
           
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(" SELECT  * from intouch.users where userID =" + userID);

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
                            users.Add(loaddata(r));
                        }
                    }
                }
                c.Close();
            }

            return users;

        }



        public List<User> getParents(int userParentID)
        {

            List<User> users = new List<User>();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(" SELECT  * from intouch.users where userParentID = " + userParentID);

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
                            users.Add(loaddata(r));
                        }
                    }
                }
                c.Close();
            }

            return users;
        }


        public User loaddata(DataRow r) 
        {

            User objUser = new User();
            objUser.userID = SharedFunctions.ValidateInteger(r["userID"]);
            objUser.roleID = SharedFunctions.ValidateInteger(r["roleID"]);
            objUser.email = SharedFunctions.ValidateString(r["email"]);
            objUser.userName = SharedFunctions.ValidateString(r["userName"]);
            objUser.password = SharedFunctions.ValidateString(r["password"]);
            objUser.firstName = SharedFunctions.ValidateString(r["firstName"]);
            objUser.lastName = SharedFunctions.ValidateString(r["lastName"]);
            objUser.active = SharedFunctions.ValidateBoolean(r["active"]);
            objUser.deleted = SharedFunctions.ValidateBoolean(r["deleted"]);
            objUser.emailConfirmation = SharedFunctions.ValidateBoolean(r["emailConfirmation"]);
            objUser.activeFrom = SharedFunctions.ValidateDatetime(r["activeFrom"]);
            objUser.activeTo = SharedFunctions.ValidateDatetime(r["activeTo"]);
            objUser.lastLogin = SharedFunctions.ValidateDatetime(r["lastLogin"]);
            objUser.userParentID = SharedFunctions.ValidateInteger(r["userParentID"]);

            //if (SharedFunctions.ValidateString(r["image"]) == "") 
            //{
            //    objUser.image = sIcons + "user50px.png"; 
            //} 
            //else
            //{
            //                objUser.image = sImages + SharedFunctions.ValidateString(r["image"]);
            objUser.image = SharedFunctions.ValidateString(r["image"]);
            //}

            if (objUser.roleID > 0)
            {
                Roles objRoles = new Roles();
                objUser.role = objRoles.getRole(objUser.roleID);
            }
            
            objUser.users = getParents(objUser.userID);

            return objUser;

        }

    }


    public class UserList : ErrorInfo
    {
        public List<User> users { get; set; }

        public UserList()
        {
            users = new List<User>();
        }
    }

    public class UserLogin : User
    {
        public string token { get; set; }
        public UserLogin()
        {
            token = "";
        }
            
    }

    public class User : ErrorInfo
    {       
        public int userID { get; set; }
        public int roleID { get; set; }
      
        public string email { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public Boolean active { get; set; }
        public DateTime activeFrom { get; set; }
        public DateTime activeTo { get; set; }
        public DateTime lastLogin { get; set; }
        public Boolean deleted { get; set; }
        public Boolean emailConfirmation { get; set; }
        public int userParentID { get; set; }
        public string image { get; set; }
        public List<User> users { get; set; }
        public HouseList houses { get; set; }  
        public Role role { get; set; } 
        public User()
        {
            userID = 0;
            roleID = 0;
            email = "";
            password = "";
            firstName = "";
            lastName = "";
            active = false;
            deleted = false;
            emailConfirmation = false;
            userParentID = 0;
            image = "";
        }

    }

}
