using inTouchApi.Model;
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
    public class HistoricoController : ControllerBase
    {
        private readonly intouchContext _context;

        public HistoricoController(intouchContext context)
        {
            _context = context;
        }
        // GET: api/<HistoricoController>
        [HttpGet]
        public async Task<IEnumerable> GetFactura()
        {
            //  List<TblFactura> listFactura = await _context.TblFacturas.ToListAsync();
            /*  dynamic listFactura = await _context.TblFacturas.Include(x => x.IdUsuarioNavigation)
                                                                                 .Where(i => i.IdEstado == 1)
                                                                                 .Select(x => new
                                                                                 {
                                                                                     x.IdFactura,
                                                                                     x.IdUsuarioNavigation.NombreUsuario,
                                                                                     x.IdPersonaNavigation.Cedula,
                                                                                     x.FechaEmision,
                                                                                     x.TotalFactura,
                                                                                     x.TotalProducto
                                                                                 }).ToListAsync();*/
            dynamic historial = await _context.Guestacces.Select(x=> new
            {
                x.GuestAccessId,
                x.GuestId,
                x.UserId,
                x.UrbanizationId,
                x.accessFrom,
                x.accessTo,
                x.AccessCode,
                x.AccessImage,
                x.Guest.Name,
                ///x.accessFrom,
            }).ToListAsync();

            return historial;
        //    dynamic listFactura = await (from f in _context.Guestacces
        //                                 join u in _context.Guests on f.guestID equals u.guestID
        //                                 select new
        //                                 {
        //                                     //f.IdFactura,
        //                                     //Cajero = p.NombrePersona,
        //                                     //Cliente = p2.NombrePersona,
        //                                     //Cedula = p2.Cedula,
        //                                    // FechaEmision = f.FechaEmision,
        //                                     //TotalFactura = f.TotalFactura
        //                                 }).ToListAsync();
        //    return listFactura;
        //    // return await _context.TblCategoria.ToListAsync();
        }


        // GET api/<HistoricoController>/5
        /* [HttpGet("{id}")]
         public string Get(int id)
         {
             return "value";
         }

         // POST api/<HistoricoController>
         [HttpPost]
         public void Post([FromBody] string value)
         {
         }

         // PUT api/<HistoricoController>/5
         [HttpPut("{id}")]
         public void Put(int id, [FromBody] string value)
         {
         }

         // DELETE api/<HistoricoController>/5
         [HttpDelete("{id}")]
         public void Delete(int id)
         {
         }*/
    }
}
