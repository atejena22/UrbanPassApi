using inTouchApi.Model;
using inTouchApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace inTouchApi.Controllers
{

    [Route("v1/[controller]")]
    [ApiController]

    public class HomeController : Controller
    {

        private readonly intouchContext _context;

        public HomeController(intouchContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Route("login")]
        public ActionResult<UserLogin> login(LoginRequest objIn)
        {
            Login objLogin = new Login();
            return Ok(objLogin.getLogin(objIn));
        }

        [HttpPost]
        [Route("validateToken")]
        public ActionResult<UserLogin> validateToken(tokenRequest objTokenRequest)
        {
            Login objLogin = new Login();
            return Ok(objLogin.getValidateToken(objTokenRequest));

        }

        [HttpPost]
        [Route("getGuestsByUser")]
        public ActionResult<GuestList> getGuests(tokenRequest objTokenRequest)
        {
            Login objLogin = new Login();
            tokenResponse objToken = objLogin.validateToken(objTokenRequest);

            Guests objGuest = new Guests();
            return Ok(objGuest.getGuests(objToken.userId));
        }

        [HttpPost]
        [Route("getGuestsAccessByUser")]
        public ActionResult<GuestAccessAllList> getGuestsAccess(tokenRequest objTokenRequest)
        {
            Login objLogin = new Login();
            tokenResponse objToken = objLogin.validateToken(objTokenRequest);

            GuestAccess objGuest = new GuestAccess();
            return Ok(objGuest.getAllAccess(objToken.userId));
        }



        [HttpPost]
        [Route("getUsers")]
        public ActionResult<UserList> getUsers(tokenRequest objTokenRequest)
        {
            Login objLogin = new Login();
            tokenResponse objToken = objLogin.validateToken(objTokenRequest);

            Users objUsers = new Users();
            return Ok(objUsers.getUsers(objToken.userId));
        }


        [HttpPost]
        [Route("getQR")]
        public ActionResult<UserList> getQR(tokenRequest objTokenRequest)
        {
            Login objLogin = new Login();
            tokenResponse objToken = objLogin.validateToken(objTokenRequest);

            QRS objQRS = new QRS();
            return Ok(objQRS.getQR(objToken.userId));
        }


       /* [HttpGet]
        [Route("getQRGuestacces")]
        public ActionResult<UserList> getQRGuestacces(tokenRequest objTokenRequest)
        {
            Login objLogin = new Login();
            tokenResponse objToken = objLogin.validateToken(objTokenRequest);

            QRS objQRS = new QRS();
            return Ok(objQRS.getQR(objToken.userId));
        }*/

        /*  [HttpPost]
          [Route("createUserAccess")]
          public ActionResult<simpleString> createUserAccess(tokenRequest objTokenRequest)
          {
              Login objLogin = new Login();
              tokenResponse objToken = objLogin.validateToken(objTokenRequest);
              GuestAccess objQRS = new GuestAccess();
              return Ok(objQRS.createUserQR(objToken.userId));
          }
        */

        [HttpPost]
        [Route("createGuests")]
        public ActionResult<IEnumerable> createGuests(Model.Guest crearvisita)
        {
            Model.Guest guest = new Model.Guest();
            guest.UserId = crearvisita.UserId;
            guest.UrbanizationId = crearvisita.UrbanizationId;
            guest.HouseId = crearvisita.HouseId;
            guest.Name = crearvisita.Name;
            guest.Identification = crearvisita.Identification;
            guest.Plate = crearvisita.Plate;
            guest.Delivery= crearvisita.Delivery;
            guest.Restaurant = crearvisita.Restaurant;
            guest.PhoneNumber= crearvisita.PhoneNumber;

            _context.Guests.Add(guest);
            _context.SaveChanges();
            // dynamic vari = _context.Guestacces

            //Login objLogin = new Login();
            //tokenResponse objToken = objLogin.validateToken(objTokenRequest);
            GuestAccess objQRS = new GuestAccess();
            //return Ok(objQRS.createUserQR(objToken.userId));
            return Ok(objQRS.createUserQR(guest));

        }









    }
}
