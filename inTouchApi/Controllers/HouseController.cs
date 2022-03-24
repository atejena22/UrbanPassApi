using ExcelDataReader;
using inTouchApi.Model;
using inTouchApi.Response;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace inTouchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {

        //[Obsolete]
        //public static IHostingEnvironment _environment;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;


        private readonly IWebHostEnvironment _environment;

        private readonly intouchContext _context;

        [Obsolete]
        public HouseController(intouchContext context, IWebHostEnvironment environment, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _environment = environment;
            _hostingEnvironment = hostingEnvironment;
        }


        // [Obsolete]
        [HttpPost("excel")]
        public async Task<Respuesta> UploadFileExcel()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Respuesta resp = new Respuesta();
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);

                        IExcelDataReader reader = null;

                        if (file.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (file.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        //else
                        //{
                        //    message = "This file format is not supported";
                        //}
                        DataSet excelRecords = reader.AsDataSet();
                        reader.Close();

                        var finalRecords = excelRecords.Tables[0];
                        for (int i = 0; i < finalRecords.Rows.Count; i++)
                        {
                            Model.User objUser = new Model.User();
                            objUser.RoleId= Convert.ToInt32(finalRecords.Rows[i][0]);
                            objUser.Email= finalRecords.Rows[i][1].ToString();
                            objUser.UserName = finalRecords.Rows[i][2].ToString();
                            objUser.Password= finalRecords.Rows[i][3].ToString();
                            objUser.FirstName= finalRecords.Rows[i][4].ToString();
                            objUser.LastName= finalRecords.Rows[i][5].ToString();
                            objUser.Active= Convert.ToBoolean(finalRecords.Rows[i][6]);
                            objUser.activeTo= (DateTime)finalRecords.Rows[i][7];
                            objUser.activeFrom= (DateTime)finalRecords.Rows[i][8];
                            objUser.lastLogin = (DateTime)finalRecords.Rows[i][9];
                            objUser.Deleted= Convert.ToBoolean(finalRecords.Rows[i][10]);
                            objUser.EmailConfirmation = Convert.ToBoolean(finalRecords.Rows[i][11]);
                            objUser.UserParentId = Convert.ToInt32(finalRecords.Rows[i][12]);
                            objUser.Image= finalRecords.Rows [i][13].ToString();

                            _context.Users.Add(objUser);
                        }
                        await _context.SaveChangesAsync();

                    }
              
                }
                resp.Mensaje= ("Upload Successful.");
                return resp;
            }
            catch (System.Exception ex)
            {
                resp.Mensaje= ("Upload Failed: " + ex.Message);
                return resp;
            }
        }



        [HttpPost("UploadFile")]
        public async Task<Respuesta> UploadFile()
        {
            Respuesta resp = new Respuesta();
            try
            {
                var file = Request.Form.Files[0];
                string fName = DateTime.Now.ToString("yymmssfff") + file.FileName;
                string path = Path.Combine("D:\\inetpub\\urbanPassApi\\wwwroot\\images\\", fName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                resp.Exito = 1;
                resp.Mensaje = fName;

                return resp;
                // return file.FileName;  // file.FileName; //$" successfully uploaded to the Server"
            }
            catch (Exception ex)
            {
                resp.Exito = 1;
                resp.Mensaje = ex.Message;
                return resp;
            }
        }



        /*[HttpGet("correo")]
        public async Task<Respuesta> EnviarCorreo()
        {
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

            Respuesta resp = new Respuesta();
            try
            {
                string emailOrigen = "desarrollosistemas150@gmail.com";
                string emailDestino = "richard.suarezjacome@gmail.com";
                string passw = "0981203639";

                string body = "<p> Estimado Usuario está es su contraseña <b>" + contraseniaAleatoria + "</b></p>" + "<p>Le sugerimos qué cambie su contraseña inmediatamente.</p>";

                MailMessage mailMessage = new MailMessage(emailOrigen, emailDestino, "Hola Estimado", body);
                mailMessage.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, passw);
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();

                resp.Exito = 1;
                resp.Mensaje = "Correo electronico enviado con éxito";
              //  resp.Data = mailMessage;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error de conexión" + ex.Message;
                return resp;
            }
        }*/


        [HttpGet]
        public async Task<IEnumerable> Get()
        {
            // return await _context.Houses.ToListAsync();
            /*
             "houseId": 1,
            "urbanizationId": 1,
            "userId": 1,
            "mz": "10",
            "villa": "7",
            "phoneNumber": "0979699620",
            "notes": "Frente Al club",
            "fullAddress": "Vergeles",
            "urbanization": null,
            "user": null,
            "guests": []
             */
            dynamic listDomicilio = await _context.Houses.Select(x => new
            {
                x.HouseId,
                x.UrbanizationId,
                x.UserId,
                x.Mz,
                x.Villa,
                x.PhoneNumber,
                x.Notes,
                x.FullAddress,
                x.Urbanization.Urbanization1,
                x.User.UserName
            }).ToListAsync();
            return listDomicilio;
        }



        [HttpGet("UserUrban/{id}")]
        public async Task<Respuesta> GetUserUrban(int id)
        {
            /*dynamic listDomicilio = await _context.Houses.Select(x => new
            {
                x.UrbanizationId,
                x.UserId,
                x.User.UserName
            }).Where(c=> c.UrbanizationId==id).ToListAsync();
            return listDomicilio;*/
            Respuesta resp = new Respuesta();

            /*   dynamic listUsuario = await (from h in _context.Houses
                                            join u in _context.Urbanizations on h.UrbanizationId equals u.UrbanizationId
                                            join us in _context.Users on h.UserId equals us.UserId
                                            select new
                                            {
                                                us.UserId,
                                                us.UserName,
                                                u.UrbanizationId
                                            }).Where(x => x.UrbanizationId == id).ToListAsync();*/
            dynamic listUsuario = await _context.Users.Select(x => new
            {
                x.UserId,
                x.UserName,
                x.UrbanizationId
            }).Where(x => x.UrbanizationId == id).ToListAsync();

             resp.Exito = 1;
            resp.Mensaje = "cargado con exito";
            resp.Data = listUsuario;

            return resp;
        }



        [HttpGet("urban")]
        public async Task<IEnumerable> GetUrbanizationHouse()
        {
            return await _context.Urbanizations.ToListAsync();
        }

        [HttpGet("user")]
        public async Task<IEnumerable> GetUserHouse()
        {
            return await _context.Users.ToListAsync();
        }




        [HttpGet("{id}")]
        public async Task<IEnumerable> GetNotification(int id)
        {
            dynamic listDomicilio = await _context.Houses.Select(x => new
            {
                x.HouseId,
                x.UrbanizationId,
                x.UserId,
                x.Mz,
                x.Villa,
                x.PhoneNumber,
                x.Notes,
                x.FullAddress,
                x.Urbanization.Urbanization1,
                x.User.UserName
            }).Where(x => x.UrbanizationId == id).ToListAsync();
            return listDomicilio;
        }


        [HttpPost]
        public async Task<Respuesta> Post([FromBody] House house)
        {
            Respuesta resp = new Respuesta();
            try
            {
                House hou= new House();
                hou.UrbanizationId = house.UrbanizationId;
                hou.UserId= house.UserId;
                hou.Mz=house.Mz;
                hou.Villa=house.Villa;
                hou.PhoneNumber=house.PhoneNumber;
                hou.Notes=house.Notes;
                hou.FullAddress=house.FullAddress;
                _context.Houses.Add(hou);
                await _context.SaveChangesAsync();


                resp.Exito = 1;
                resp.Mensaje = "Domicilio Registrado con éxito";
                resp.Data = hou;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error de conexión" + ex.Message;
                return resp;
            }

        }


        [HttpPut]
        public async Task<Respuesta>PutHouse([FromBody] House house)
        {
            Respuesta resp = new Respuesta();
            try
            {
                var hous = await _context.Houses.Where(x => x.HouseId == house.HouseId).FirstOrDefaultAsync();
                hous.HouseId = house.HouseId;
                hous.UrbanizationId=house.UrbanizationId;
                hous.UserId=house.UserId;
                hous.Mz=house.Mz;
                hous.Villa=house.Villa;
                hous.PhoneNumber=house.PhoneNumber;
                hous.Notes=house.Notes;
                hous.FullAddress=house.FullAddress;
                _context.Entry(hous).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                resp.Exito = 1;
                resp.Mensaje = "Domicilio actualizado con éxito";
                resp.Data = hous;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error de conexión" + ex.Message;
                return resp;
            }
        }










       
    }
}
