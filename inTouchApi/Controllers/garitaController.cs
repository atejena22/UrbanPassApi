using inTouchApi.Model;
using inTouchApi.Response;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace inTouchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class garitaController : ControllerBase
    {
        string sConn = inTouchApi.Models.ConfigurationManager.AppSettings("connectionStrings:SQLConn");

        private readonly intouchContext _context;

        public garitaController(intouchContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<Respuesta> GetGarita(int id)
        {
            //var lstGarita= _context.Guests.Select(x=> new
            //{
            //    x.GuestId,
            //    x.House.Mz,
            //    x.House.Villa,
            //    x.User.UserName,
            //    x.PhoneNumber,
            //    //x.Guestacces.ac
            //})
            Respuesta resp = new Respuesta();
            try
            {
                var lstGarita = _context.Guestacces.Select(g => new
                {
                    g.Guest.GuestId,
                    g.Guest.House.Mz,
                    g.Guest.House.Villa,
                    g.Guest.PhoneNumber,
                    g.User.LastName,
                    g.User.FirstName,
                    g.accessFrom,
                    g.accessTo,
                    g.Guest.UrbanizationId,
                    g.Guest.Restaurant
                }).Where(x=> x.UrbanizationId==id && x.accessTo>DateTime.Now).ToList();

                resp.Exito = 1;
                resp.Mensaje = "Lista garita cargada con exito";
                resp.Data = lstGarita;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "error de conexion"+ex.Message;
                return resp;
            }
           

        }

        [HttpGet]
        public async Task<Respuesta> GetGaritaGeneral()
        {
           
            Respuesta resp = new Respuesta();
            try
            {
                var lstGarita = _context.Guestacces.Select(g => new
                {
                    g.Guest.GuestId,
                    g.Guest.House.Mz,
                    g.Guest.House.Villa,
                    g.Guest.PhoneNumber,
                    g.User.LastName,
                    g.User.FirstName,
                    g.accessFrom,
                    g.accessTo,
                    g.Guest.UrbanizationId,
                    g.Guest.Restaurant
                }).OrderByDescending(f=> f.GuestId).ToList();

                resp.Exito = 1;
                resp.Mensaje = "Lista garita cargada con exito";
                resp.Data = lstGarita;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "error de conexion" + ex.Message;
                return resp;
            }


        }




        /*****************************************************  total de usuario, uso y domicilio, para admin general de impova **************************************************************************/

        [HttpGet("totaldonaAdmin")]
        public async Task<Respuesta> GettotaldonaAdmin()
        {
            Respuesta resp = new Respuesta();
           // int[]=[3];

            var totalHouse = _context.Houses.Count();

            var TotaldeUsuario = _context.Users.Count();

            var totalGuestAcces = _context.Guestacces.Count();

            int[] total = new int[3] { totalHouse, TotaldeUsuario, totalGuestAcces };
            resp.Data = total;


            return resp;
        }

        /*****************************************************  total de usuario, uso y domicilio, para role admin de urbanizacion **************************************************************************/
        [HttpGet("totaldona")]
        public async Task<Respuesta> GettotalDona(int id)
        {
            Respuesta resp = new Respuesta();

            dynamic TotaldeUsuario = (from h in _context.Houses
                                      join u in _context.Urbanizations on h.UrbanizationId equals u.UrbanizationId
                                      join us in _context.Users on h.UserId equals us.UserId
                                      select new
                                      {
                                          us.UserId,
                                          us.RoleId,
                                          us.Email,
                                          us.UserName,
                                          us.FirstName,
                                          us.LastName,
                                          us.Active,
                                          us.activeFrom,
                                          us.activeTo,
                                          us.lastLogin,
                                          us.Deleted,
                                          us.UserParentId,
                                          us.Image,
                                          u.UrbanizationId

                                      }).Where(x => x.UrbanizationId == id).Count();
            //Where(x => x.UrbanizationId == id).ToListAsync()
            var totalHouse = _context.Houses.Where(x => x.UrbanizationId == id).Count();

            var totalGuestAcces = _context.Guestacces.Where(x => x.UrbanizationId == id).Count();

            int[] total = new int[3] { totalHouse, TotaldeUsuario, totalGuestAcces };
            resp.Data = total;

            return resp;
        }






        [HttpGet("masAccesUser")]
        public async Task<Respuesta> GetPrimerosUsuariosAcces(int id)
        {
            Respuesta resp = new Respuesta();
            List<PersonaAccesMax> lista = new List<PersonaAccesMax>();
            try
            {
                using (MySqlConnection c = new MySqlConnection(sConn))
                {
                    c.Open();
                    string query = "SELECT us.userID,us.email, us.firstName, us.lastName,h.mz,h.villa,u.urbanizationID, us.lastLogin, us.image, COUNT(guestA.guestAccessID) as counte FROM intouch.houses as h JOIN intouch.urbanizations as u ON u.urbanizationID = h.urbanizationID JOIN intouch.users as us ON us.userID = h.userID JOIN intouch.guestacces as guestA ON guestA.userID = us.userID  where u.urbanizationID = " + id+"     group by guestA.userID LIMIT 10";
                    MySqlCommand cmd = new MySqlCommand(query, c);
                    MySqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        PersonaAccesMax pers = new PersonaAccesMax();
                        pers.UserId = reader.GetInt32(0);
                        pers.email = reader.GetString(1);
                        pers.firstName = reader.GetString(2);
                        pers.lastName = reader.GetString(3);
                        pers.mz = reader.GetString(4);
                        pers.villa = reader.GetString(5);
                        pers.urbanizationID = reader.GetInt32(6);
                        pers.lastLogin = reader.GetDateTime(7);
                        pers.image = reader.GetString(8);
                        pers.counte = reader.GetInt32(9);
                        lista.Add(pers);
                    }

                }
                resp.Exito = 1;
                resp.Mensaje = "Exito al cargar los 5 primeros usuarios con mas accesos";
                resp.Data = lista;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al cargar la lista de los primeros 5 usuarios";
                return resp;
            }
 
        }




        [HttpGet("masAccesUserAdmin")]
        public async Task<Respuesta> GetPrimerosUsuariosAccesAdmin()
        {
            Respuesta resp = new Respuesta();
            List<PersonaAccesMax> lista = new List<PersonaAccesMax>();
            try
            {
                using (MySqlConnection c = new MySqlConnection(sConn))
                {
                    c.Open();
                    string query = "SELECT us.userID,us.email, us.firstName, us.lastName,h.mz,h.villa,u.urbanizationID, us.lastLogin, us.image, COUNT(guestA.guestAccessID) as counte FROM intouch.houses as h JOIN intouch.urbanizations as u ON u.urbanizationID = h.urbanizationID JOIN intouch.users as us ON us.userID = h.userID JOIN intouch.guestacces as guestA ON guestA.userID = us.userID  group by guestA.userID LIMIT 5";
                    MySqlCommand cmd = new MySqlCommand(query, c);
                    MySqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        PersonaAccesMax pers = new PersonaAccesMax();
                        pers.UserId = reader.GetInt32(0);
                        pers.email = reader.GetString(1);
                        pers.firstName = reader.GetString(2);
                        pers.lastName = reader.GetString(3);
                        pers.mz = reader.GetString(4);
                        pers.villa = reader.GetString(5);
                        pers.urbanizationID = reader.GetInt32(6);
                        pers.lastLogin = reader.GetDateTime(7);
                        pers.image = reader.GetString(8);
                        pers.counte = reader.GetInt32(9);
                        lista.Add(pers);
                    }

                }
                resp.Exito = 1;
                resp.Mensaje = "Exito al cargar los 5 primeros usuarios con mas accesos";
                resp.Data = lista;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al cargar la lista de los primeros 5 usuarios";
                return resp;
            }

        }












    }
}









public class PersonaAccesMax{
    /*
     us.userID,us.email, us.firstName, us.lastName,h.mz,h.villa,u.urbanizationID" +
                    " us.lastLogin, us.image, COUNT(guestA.guestAccessID) as counte" 
     */
    public int UserId { get; set; }
    public int urbanizationID { get; set; }
    public string  email { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string mz { get; set; }
    public string villa { get; set; }

    public DateTime lastLogin { get; set; }
    public string image { get; set; }
    public int counte { get; set; }

    public PersonaAccesMax()
    {

    }

    public PersonaAccesMax(int UserId, int urbanizationID, string email, string firstName, string lastName, string mz, string villa, DateTime lastLogin, string image, int counte)
    {
        this.UserId = UserId;
        this. urbanizationID = urbanizationID;  
        this.email = email;
        this.firstName = firstName;
        this.lastName=lastName;
        this.mz = mz;
        this.villa = villa;
        this.lastLogin = lastLogin;
        this.image = image;
        this.counte = counte;
    }


}