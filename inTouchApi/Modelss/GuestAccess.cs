using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace inTouchApi.Models
{
    
    public class GuestAccess
    {
        string sConn = ConfigurationManager.AppSettings("connectionStrings:SQLConn");

        string sImages = ConfigurationManager.AppSettings("appSettings:imagesPath");
        string sIcons = ConfigurationManager.AppSettings("appSettings:iconsPath");

        string simagesQRSURL = ConfigurationManager.AppSettings("appSettings:imagesQRSURL");

        public GuestAccessList getGuest(int userID, int guestID)
        {

            GuestAccessList objResponse = new GuestAccessList();

            List<GuestAccessItem> objList = new List<GuestAccessItem>();

            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" SELECT  * from intouch.guestacces where userID =" + userID + " and guestID = " + guestID);

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
                                GuestAccessItem objItem = new GuestAccessItem();
                                objItem.urbanizationID = SharedFunctions.ValidateInteger(r["urbanizationID"]);
                                objItem.guestID = SharedFunctions.ValidateInteger(r["guestID"]);
                                objItem.userID = SharedFunctions.ValidateInteger(r["userID"]);
                                objItem.guestAccessID = SharedFunctions.ValidateInteger(r["guestAccessID"]);
                                objItem.accessFrom = SharedFunctions.ValidateDatetime(r["accessFrom"]);
                                objItem.accessTo = SharedFunctions.ValidateDatetime(r["accessTo"]);
                                objItem.accessCode = SharedFunctions.ValidateString(r["code"]);
                                objItem.accessImage = SharedFunctions.ValidateString(r["image"]);
                                objList.Add(objItem);
                            }
                            objResponse.guestAccessList = objList;
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

        public GuestAccessAllList getAllAccess(int userID)
        {

            GuestAccessAllList objResponse = new GuestAccessAllList();

            List<GuestUserAccessItem> objList = new List<GuestUserAccessItem>();

            try
            {
                

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" SELECT  guestacces.guestAccessID, guestacces.guestID, guestacces.userID, guestacces.urbanizationID, guestacces.accessFrom, guestacces.accessTo, guestacces.accessCode, guestacces.accessImage, guests.name, ");
                sb.Append(" guests.image, guests.identification, guests.plate, guests.delivery, guests.restaurant, guests.phoneNumber, guests.email ");
                sb.Append(" FROM intouch.guestacces INNER JOIN  ");
                sb.Append(" guests ON guestacces.guestID = guests.guestID ");
                sb.Append(" WHERE(guestacces.userID = " + userID + ")");

                // Open connection
                using (SqlConnection c = new SqlConnection(sConn))
                {
                    c.Open();
                    // Create new DataAdapter
                    using (SqlDataAdapter a = new SqlDataAdapter(sb.ToString(), c))
                    {
                        // Use DataAdapter to fill DataTable 
                        DataTable t = new DataTable();
                        a.Fill(t);
                        if (t.Rows.Count > 0)
                        {
                            foreach (DataRow r in t.Rows)
                            {
                                GuestUserAccessItem objItem = new GuestUserAccessItem();
                                objItem.urbanizationID = SharedFunctions.ValidateInteger(r["urbanizationID"]);
                                objItem.guestID = SharedFunctions.ValidateInteger(r["guestID"]);
                                objItem.userID = SharedFunctions.ValidateInteger(r["userID"]);
                                objItem.guestAccessID = SharedFunctions.ValidateInteger(r["guestAccessID"]);
                                objItem.accessFrom = SharedFunctions.ValidateDatetime(r["accessFrom"]);
                                objItem.accessTo = SharedFunctions.ValidateDatetime(r["accessTo"]);
                                objItem.accessCode = SharedFunctions.ValidateString(r["accessCode"]); 
                                objItem.accessImage = SharedFunctions.ValidateString(r["accessImage"]);
                                objItem.delivery = SharedFunctions.ValidateBoolean(r["delivery"]);
                                objItem.restaurant = SharedFunctions.ValidateString(r["restaurant"]);
                                objItem.name = SharedFunctions.ValidateString(r["name"]);
                                objItem.phoneNumber = SharedFunctions.ValidateString(r["phoneNumber"]);
                                objItem.email = SharedFunctions.ValidateString(r["email"]); 
                                objItem.plate = SharedFunctions.ValidateString(r["plate"]);
                                objItem.image = SharedFunctions.ValidateString(r["image"]);
                                objItem.identification = SharedFunctions.ValidateString(r["identification"]);
                                objItem.icon   = sIcons + "qr50px.png";  
                                objList.Add(objItem);
                            }
                            objResponse.guestAccessList = objList;
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


        public simpleString createUserQR(Model.Guest  obje)
        {
            int userID = obje.UserId;
            simpleString objReturn = new simpleString();

            try
            {

                Users objUsers = new Users();
                UserLogin objUser = objUsers.getUser(obje.UserId);

                int urbanizationID = 0;
                if (objUser.houses.houseList.Count > 0) 
                {
                    urbanizationID =  objUser.houses.houseList[0].urbanizationID; 
                }

                DateTime accessFrom = DateTime.Now;
                DateTime accessTo = DateTime.Now.AddMinutes(10);
                int guestID = obje.GuestId;
                string accessCode = "";
                string accessImage = "";

                // create row
                simpleInteger objInsert = createAccess(guestID, userID, urbanizationID, accessFrom, accessTo, accessCode, accessImage);
                if (objInsert.hasError)
                {
                    objReturn.hasError = objInsert.hasError = true;
                    objReturn.errorDesc = objInsert.errorDesc;
                }

                if (objInsert.response > 0)
                {
                    // ok
                }
                else
                {
                    objReturn.hasError = objInsert.hasError = true;
                    objReturn.errorDesc = "error de registro de acceso";
                } 

                // create codes.
                accessCode = "QR-" + objInsert.response.ToString() + "-" + guestID.ToString() + "-" + userID.ToString() + "-" + urbanizationID.ToString();
                accessImage = accessCode + ".png";
                 
                // create QR
                QRS objQRS = new QRS();
                simpleString objQR = objQRS.getQR(accessCode, accessImage);
                if (objQR.hasError)
                {
                    objReturn.hasError = objQR.hasError = true;
                    objReturn.errorDesc = objQR.errorDesc;
                }

                // update row
                ErrorInfo objUpdateAccess = updateAccess(objInsert.response, userID, accessCode, accessImage);
                if (objUpdateAccess.hasError)
                {
                    objReturn.hasError = objUpdateAccess.hasError = true;
                    objReturn.errorDesc = objUpdateAccess.errorDesc;
                }
                 
                objReturn.response = simagesQRSURL +  accessImage;

            }
            catch (Exception ex)
            {
                objReturn.hasError = true;
                objReturn.errorDesc = ex.Message.ToString();
            }

            return objReturn;


        }

        public simpleInteger createAccess(int guestID, int userID, int urbanizationID, DateTime accessFrom, DateTime accessTo, string accessCode, string accessImage)
        {

            simpleInteger objResponse = new simpleInteger();

            try
            {
                //select @@identity as guestAccessID                                 //accessFrom.ToString()           ------------------------------------------------         accessTo.ToString()                       
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" insert into intouch.guestacces (guestID, userID, urbanizationID, accessFrom, accessTo, accessCode, accessImage) values (" + guestID + ", " + userID + "," + urbanizationID + ",'" +  accessFrom.ToString("yyyy-MM-dd H:mm:ss") + "', '" + accessTo.ToString("yyyy-MM-dd H:mm:ss") + "', '"  + accessCode + "', '" + accessImage + "');select @@identity as guestAccessID");

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
                                objResponse.response = SharedFunctions.ValidateInteger(r["guestAccessID"]);
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

        public ErrorInfo updateAccess(int guestAccessID, int userID, string accessCode, string accessImage)
        {

            ErrorInfo objResponse = new ErrorInfo();

            try 
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("UPDATE intouch.guestacces SET accessCode = '"+accessCode+"', accessImage = '"+accessImage+"' where  guestAccessID = "+guestAccessID+" and userID = "+userID );

                // Open connection
                using (MySqlConnection conn = new MySqlConnection(sConn))
                {
                    MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex) 
                    {
                        objResponse.hasError = true;
                        objResponse.errorDesc = ex.Message.ToString();
                    }
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


    public class GuestAccessList : ErrorInfo
    {
      public  List<GuestAccessItem> guestAccessList { get; set; }

    }

    public class GuestAccessAllList : ErrorInfo
    {
        public List<GuestUserAccessItem> guestAccessList { get; set; }

    }



    public class GuestUserAccessItem : GuestAccessItem
    {
        public string icon { get; set; }

        public string name { get; set; }
        public string image { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string identification { get; set; }
        public string plate { get; set; }
        public bool delivery { get; set; }
        public string restaurant { get; set; }

        public GuestUserAccessItem()
        {
            delivery = false;
            restaurant = "";
            name = "";
            phoneNumber = "";
            email = "";
            plate = "";
            image = "";
            identification = "";
            icon = "";
        }
    }

    public class GuestAccessItem
    {

        public int guestAccessID { get; set; }
        public int guestID { get; set; }
        public int userID { get; set; }
        public int urbanizationID { get; set; }
        public string roleName { get; set; }
        public DateTime accessFrom { get; set; }
        public DateTime accessTo { get; set; }
        public string accessCode { get; set; }
        public string accessImage { get; set; }
  

        public GuestAccessItem()
        {
            guestAccessID = 0;
            guestID = 0;
            urbanizationID = 0;
            userID = 0;
            accessCode = ""; 
            accessImage = "";
        }

    }

}
