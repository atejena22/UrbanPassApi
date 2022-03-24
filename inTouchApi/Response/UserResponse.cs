using inTouchApi.Model;
using System;
using System.Collections.Generic;

namespace inTouchApi.Response
{
    public class UserResponse
    {
        //public string Email { get; set; }
        public string Token { get; set; }
        public int userID { get; set; }
        public int roleID { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        //public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        //public Boolean active { get; set; }
        public DateTime activeFrom { get; set; }
        public DateTime activeTo { get; set; }
        //public DateTime lastLogin { get; set; }
        //public Boolean deleted { get; set; }
        //public Boolean emailConfirmation { get; set; }
        public int userParentID { get; set; }
        public string image { get; set; }

        public int UrbanizationId { get; set; }

        // public int RoleId { get; set; }
        //public string RoleName { get; set; }
        //public string RoleDescription { get; set; }

        //public List<User> users { get; set; }
        public HouseList houses { get; set; }
       // public Role role { get; set; }



    }

    public class HouseList
    {
        public List<House> houseList { get; set; }

        public HouseList()
        {
            houseList = new List<House>();
        }

    }

}
