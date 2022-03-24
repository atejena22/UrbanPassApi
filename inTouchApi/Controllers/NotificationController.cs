using inTouchApi.Model;
using inTouchApi.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace inTouchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly intouchContext _context;

        public NotificationController(intouchContext context)
        {
            _context = context;
        }
        // GET: api/<NotificationController>
        [HttpGet]
        public async Task<IEnumerable> Get()
        {
            dynamic lstNotification = await _context.Notificationdetails.Select(x=> new
            {
                x.Id,
                x.Titulo,
                x.UrbanizationId,
                x.Notas,
                x.tiempo,
                x.FechaEstablecida
            }).ToListAsync();

            return lstNotification;
        }

        [HttpPost]
        public async Task<Notificationdetail> Post([FromBody] Notificationdetail noti)
        {
            try
            {
                Notificationdetail not = new Notificationdetail();
                not.Titulo = noti.Titulo;
                not.FechaEstablecida = noti.FechaEstablecida;
                not.Notas = noti.Notas;
                not.UrbanizationId = noti.UrbanizationId;
                not.tiempo= noti.tiempo;
                _context.Notificationdetails.Add(not);
                await _context.SaveChangesAsync();
                return not;
            }
            catch 
            {
                return null;
            }

        }


        [HttpPut]
        public async Task<Respuesta> Put([FromBody] Notificationdetail notifi)
        {
            Respuesta resp = new Respuesta();
            try
            {
                var notificacionEdit = await _context.Notificationdetails.Where(x=> x.Id==notifi.Id).FirstOrDefaultAsync();
                notificacionEdit.Id = notifi.Id;
                notificacionEdit.Titulo=notifi.Titulo;
                notificacionEdit.Notas = notifi.Notas;
                notificacionEdit.FechaEstablecida = notifi.FechaEstablecida;
                notificacionEdit.UrbanizationId=notifi.UrbanizationId;
                notificacionEdit.tiempo=notifi.tiempo;
                _context.Entry(notificacionEdit).State=EntityState.Modified;
                await _context.SaveChangesAsync();

                resp.Exito = 1;
                resp.Mensaje = "Registrado con éxito";
                resp.Data = notificacionEdit;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje="Ocurrio un error"+" "+ex.Message;
                return resp;
            }
        }




        [HttpGet("{id}")]
        public async Task<IEnumerable> GetNotification(int id)
        {
            return await _context.Notificationdetails.Where(x=> x.UrbanizationId== id).ToListAsync();

        }
        

    }
}
