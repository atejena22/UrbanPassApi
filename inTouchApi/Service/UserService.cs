using ExcelDataReader;
using inTouchApi.Common;
using inTouchApi.Model;
using inTouchApi.Request;
using inTouchApi.Response;
using inTouchApi.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;

namespace inTouchApi.Service
{
    public class UserService: UserInterface
    {

        private readonly AppSettings _appSettings;
        private readonly intouchContext _context;
        public UserService(IOptions<AppSettings> appSettings, intouchContext context)
        {
            _appSettings = appSettings.Value;
            this._context = context;
        }

        public async Task<Respuesta> GetRol()
        {
            Respuesta resp = new Respuesta();
            try
            {
                var listUsuario = await _context.Roles.ToListAsync();

                resp.Exito = 1;
                resp.Mensaje = "Lista de Rol cargada con éxito";
                resp.Data = listUsuario;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al cargar la lista de Rol" + ex.Message;
                return resp;
            }
         }

        public async Task<Respuesta> PresntarUsuario()
        {
            Respuesta resp = new Respuesta();
            try
            {
                dynamic listUsuario = await _context.Users.Select(x => new
                {
                    x.UserId,
                    x.Image,
                    x.Role.RoleId,
                    x.Email,
                    x.UserName,
                    x.FirstName,
                    x.LastName,
                    x.Active,
                    x.activeFrom,
                    x.activeTo,
                    x.UserParentId,
                    x.UrbanizationId,
                    x.Role.RoleName,//añadido recientemente
                    x.lastLogin
                }).OrderByDescending(f=> f.UserId).ToListAsync();

                resp.Exito = 1;
                resp.Mensaje = "Lista de Usuario cargada con éxito";
                resp.Data = listUsuario;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al cargar la lista de usuario" + ex.Message;
                return resp;
            }
        }



        public async Task<Respuesta> PresEntarUsuarioActual(int id)
        {
            Respuesta resp = new Respuesta();
            try
            {
                dynamic listUsuario = await _context.Users.Where(x => x.UrbanizationId == id).ToListAsync();
               /* dynamic listUsuario = await (from h in _context.Houses
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
                                                 /*f.IdFactura,
                                                 Cajero = p.NombrePersona,
                                                 Cliente = p2.NombrePersona,
                                                 Cedula = p2.Cedula,
                                                 FechaEmision = f.FechaEmision,
                                                 TotalFactura = f.TotalFactura*
                                             }).Where(x=> x.UrbanizationId==id).ToListAsync();*/

                resp.Exito = 1;
                resp.Mensaje = "Lista de Usuario cargada con éxito";
                resp.Data = listUsuario;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al cargar la lista de usuario" + ex.Message;
                return resp;
            }
        }

        /*  
         SELECT 
                    us.userID, us.roleID, us.email,us.userName, us.firstName, us.lastName, us.active, 
                    us.activeFrom, us.activeTo, us.lastLogin, us.deleted, us.userParentID, us.image
                     FROM intouch.houses as h JOIN intouch.urbanizations as u
									ON u.urbanizationID=h.urbanizationID
                                    JOIN intouch.users as us ON us.userID=h.userID
                                    where u.urbanizationID=3
        */
        /*
          dynamic listFactura = await (from f in _context.TblFacturas
                                   join u in _context.TblUsuarios on f.IdUsuario equals u.IdUsuario
                                   join p in _context.TblPersonas on u.IdPersona equals p.IdPersona
                                   join p2 in _context.TblPersonas on f.IdPersona equals p2.IdPersona
                                   select new
                                   {
                                       f.IdFactura,
                                       Cajero=p.NombrePersona,
                                       Cliente = p2.NombrePersona,
                                       Cedula=p2.Cedula,
                                       FechaEmision=f.FechaEmision,
                                       TotalFactura=f.TotalFactura
                                   }).ToListAsync();
         */



        public async Task<Respuesta> GenerarUsuario([FromBody] User usuario)
        {
            Respuesta resp = new Respuesta();
            try
            {
                #region Generador de contraseña aletoriamente
                Random rdn = new Random();
                string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                int longitud = caracteres.Length;
                char letra;
                int longitudContrasenia = 10;
                string contraseniaAleatoria = string.Empty;
                for (int i = 0; i < longitudContrasenia; i++)
                {
                    letra = caracteres[rdn.Next(longitud)];
                    contraseniaAleatoria += letra.ToString();//esta es la contraseña que sse genera.
                }
                #endregion
              
                    //Respuesta resp = new Respuesta();
                    string spassword = Encrypt.GetSHA256(contraseniaAleatoria);
                // string spassword = Encrypt.GetSHA256(usuario.Password);
                User usu = new User();
                //usu.UserId = usuario.UserId;
                usu.RoleId = usuario.RoleId;
                usu.Deleted = false;
                usu.lastLogin = DateTime.Now;  
                usu.Email = usuario.Email;
                usu.UserName = usuario.UserName;
                usu.Password = spassword;
                usu.FirstName = usuario.FirstName;
                usu.LastName = usuario.LastName;
                usu.Active = usuario.Active;
                usu.activeFrom = usuario.activeFrom;
                usu.activeTo = usuario.activeTo;
                usu.UserParentId = usuario.UserId;
                usu.UrbanizationId= usuario.UrbanizationId;
               
                if (usuario.Image == "")
                {
                    usu.Image ="sinNombre.png";
                }
                else
                {
                    usu.Image = usuario.Image;
                }

                _context.Users.Add(usu);
                await _context.SaveChangesAsync();

                #region enviar password al correo
                string emailOrigen = "desarrollosistemas150@gmail.com";
                string emailDestino = usuario.Email;
                string passw = "0981203639";

                string body = "<p> Estimado Usuario está es su contraseña: <b>" + contraseniaAleatoria + "</b></p>" + "<p>Le sugerimos qué cambie su contraseña inmediatamente.</p>";
                string encabezado = "Hola " + usuario.LastName + " "+ usuario.FirstName;
                MailMessage mailMessage = new MailMessage(emailOrigen, emailDestino, encabezado,  body);
                mailMessage.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, passw);
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();
                #endregion


                resp.Exito = 1;
                resp.Mensaje = "Usuario Registrado con Éxito";
                resp.Data = usuario;
                return resp;

            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al insertar usuario" + ex.Message;
                return resp;
            }

        }


        public async Task<Respuesta> ActualizarPasswordUsuario([FromBody] User usuario)
        {
            Respuesta resp = new Respuesta();
            try
            {
                string spassword = Encrypt.GetSHA256(usuario.Password);
                var usu = await _context.Users.FindAsync(usuario.UserId);

                usu.UserId = usuario.UserId;
                usu.UserName = usuario.UserName;
                usu.Password = spassword;
                _context.Entry(usu).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();

                resp.Exito = 1;
                resp.Mensaje = "Contraseña actualizada con Éxito";
                resp.Data = usu;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al Actualizar contraseña" + ex.Message;
                return resp;
            }
     

        }

        public async Task<Respuesta> ActualizarUsuarioTrueAdmin([FromBody] User usuario)
        {
            Respuesta resp = new Respuesta();
            try
            {
                var usu = await _context.Users.FindAsync(usuario.UserId);
                usu.UserId = usuario.UserId;
                usu.Active = usuario.Active;
                _context.Entry(usu).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();

                resp.Exito = 1;
                resp.Mensaje = "Exito al actualizar";
                resp.Data = usu;
                return resp;
            }catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error" + ex.Message;
                return resp;
            }
            

        }


        public async Task<Respuesta> PutUsuarioCompletoActualizar([FromBody] User usuario)
        {
            Respuesta resp = new Respuesta();
            try
            {
                //  string spassword = Encrypt.GetSHA256(usuario.Password);
                string spassword = Encrypt.GetSHA256("123");

                var usu = await _context.Users.FindAsync(usuario.UserId);
                usu.UserId = usuario.UserId;
                usu.RoleId = usuario.RoleId;
                usu.Email = usuario.Email;
                usu.UserName = usuario.UserName;
                usu.Password = spassword;
                usu.FirstName = usuario.FirstName;
                usu.LastName = usuario.LastName;
                usu.Active = usuario.Active;
                usu.activeFrom = usuario.activeFrom;
                usu.activeTo = usuario.activeTo;
                usu.UserParentId = usuario.UserId;
                usu.Image = usuario.Image;
                usu.UrbanizationId = usuario.UrbanizationId;
                _context.Entry(usu).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();

                resp.Exito = 1;
                resp.Mensaje = "Exito al actualizar el Usuario";
                resp.Data = usu;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al actualizar el Usuario" + ex.Message;
                return resp;
            }
          


        }


        //******************************************************** PARA AUTENTIFICARSE **********************************************************************************************************/
        public UserResponse Auth(AuthRequest model)
        {
            UserResponse userresponse = new UserResponse();
            // GeneralResponse resp = new GeneralResponse();

            using (var db = new intouchContext())
            {
                string spassword = Encrypt.GetSHA256(model.password);

                var usuari = db.Users.Where(u => u.UserName == model.userName && u.Password == spassword).FirstOrDefault();

                if (usuari == null)
                {
                    return null;
                }

                var persona = db.Users.Where(p => p.UserId == usuari.UserId).FirstOrDefault();
              //  var urbanization= db.Houses.Where(p => p.UserId==persona.UserId).FirstOrDefault();
                userresponse.Token = GetToken(usuari);
                userresponse.userName = persona.UserName;
                userresponse.userID = persona.UserId;
                userresponse.roleID = persona.RoleId;
                userresponse.email = persona.Email;
                userresponse.firstName = persona.FirstName;
                userresponse.lastName = persona.LastName;
                userresponse.activeFrom = persona.activeFrom;
                userresponse.activeTo = persona.activeTo;
                userresponse.userParentID = (int)persona.UserParentId;
                userresponse.image=persona.Image;

                userresponse.UrbanizationId = (int)persona.UrbanizationId; //urbanization.UrbanizationId;
                 
             }
            return userresponse;
        }

        private string GetToken(User usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var llave = Encoding.ASCII.GetBytes(_appSettings.secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.UserId.ToString()),
                        new Claim(ClaimTypes.Name,usuario.UserName)
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }








        public async Task<Respuesta> ResetearPasswordUserAdmin([FromBody] User usuario)
        {
            Respuesta resp = new Respuesta();
            try
            {
                #region Generador de contraseña aletoriamente
                Random rdn = new Random();
                string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                int longitud = caracteres.Length;
                char letra;
                int longitudContrasenia = 10;
                string contraseniaAleatoria = string.Empty;
                for (int i = 0; i < longitudContrasenia; i++)
                {
                    letra = caracteres[rdn.Next(longitud)];
                    contraseniaAleatoria += letra.ToString();//esta es la contraseña que sse genera.
                }
                #endregion

                string spassword = Encrypt.GetSHA256(contraseniaAleatoria);
                var usu = await _context.Users.FindAsync(usuario.UserId);

                usu.UserId = usuario.UserId;
                usu.Password = spassword;
                _context.Entry(usu).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();

                #region enviar correo contraseña actualizada
                string emailOrigen = "desarrollosistemas150@gmail.com";
                string emailDestino = usuario.Email;
                string passw = "0981203639";

                string encabezado = "Hola " + usuario.LastName +" "+ usuario.FirstName;
                string body = "<p> Estimado Usuario su contraseña ha sido restablecido: <b>" + contraseniaAleatoria + "</b></p>" + "<p>Le sugerimos qué cambie su contraseña inmediatamente.</p>";

                MailMessage mailMessage = new MailMessage(emailOrigen, emailDestino, encabezado, body);
                mailMessage.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, passw);
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();
                #endregion

                resp.Exito = 1;
                resp.Mensaje = "La contraseña se ha restablecido con éxito";
                resp.Data = usu;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al restablecer la contraseña" + ex.Message;
                return resp;
            }


        }










    }
}
