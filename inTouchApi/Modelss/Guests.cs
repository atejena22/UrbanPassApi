using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace inTouchApi.Models
{
    public class Guests
    {

        string sConn = ConfigurationManager.AppSettings("connectionStrings:SQLConn");

        public GuestList getGuests(int userID)
        {

            GuestList objResponse = new GuestList();

            List<Guest> objList = new List<Guest>();

            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" SELECT  * from intouch.guests where userID =" + userID);

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
                                Guest objItem = new Guest();
                                objItem.urbanizationID = SharedFunctions.ValidateInteger(r["urbanizationID"]);
                                objItem.guestID = SharedFunctions.ValidateInteger(r["guestID"]);
                                objItem.userID = SharedFunctions.ValidateInteger(r["userID"]);
                                objItem.name = SharedFunctions.ValidateString(r["name"]);
                                objItem.phoneNumber = SharedFunctions.ValidateString(r["phoneNumber"]);
                                objItem.image = SharedFunctions.ValidateString(r["image"]);
                                objItem.identification = SharedFunctions.ValidateString(r["identification"]);
                                objItem.plate = SharedFunctions.ValidateString(r["plate"]);
                                objItem.delivery = SharedFunctions.ValidateBoolean(r["delivery"]);
                                objItem.restaurant = SharedFunctions.ValidateString(r["restaurant"]);
                                objList.Add(objItem); 
                            }
                            objResponse.guestList = objList;
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

    
        public Guest getGuest(int guestID, int userID)
        {

            Guest objResponse = new Guest();

            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" SELECT  * from intouch.guests where guestID =" + guestID + " and userID = "+ userID);

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
                                objResponse.urbanizationID = SharedFunctions.ValidateInteger(r["urbanizationID"]);
                                objResponse.guestID = SharedFunctions.ValidateInteger(r["guestID"]);
                                objResponse.userID = SharedFunctions.ValidateInteger(r["userID"]);
                                objResponse.name = SharedFunctions.ValidateString(r["name"]);
                                objResponse.email = SharedFunctions.ValidateString(r["email"]);
                                objResponse.phoneNumber = SharedFunctions.ValidateString(r["phoneNumber"]);
                                objResponse.image = SharedFunctions.ValidateString(r["image"]);
                                objResponse.identification = SharedFunctions.ValidateString(r["identification"]);
                                objResponse.plate = SharedFunctions.ValidateString(r["plate"]);
                                objResponse.delivery = SharedFunctions.ValidateBoolean(r["delivery"]);
                                objResponse.restaurant  = SharedFunctions.ValidateString(r["restaurant"]);
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


    }



    public class GuestList : ErrorInfo
    {
      public  List<Guest> guestList { get; set; }
    }


    public class Guest : ErrorInfo
    {

        public int guestID { get; set; }
        public int userID { get; set; }
        public int urbanizationID { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string identification { get; set; }
        public string plate { get; set; }
        public bool delivery { get; set; }
        public string restaurant { get; set; }

        public Guest()
        {
            delivery = false;
            restaurant = "";
            guestID = 0;
            userID = 0;
            urbanizationID = 0;
            name = "";
            phoneNumber = "";
            email = "";
        }
    }
    




}
