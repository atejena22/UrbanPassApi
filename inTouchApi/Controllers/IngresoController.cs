using inTouchApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace inTouchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngresoController : ControllerBase
    {
        private readonly intouchContext _context;

        public IngresoController(intouchContext context)
        {
            _context = context;
        }

        // GET: api/<HistoricoController>
        [HttpGet]
        public async Task<IEnumerable> GetIngresoRegistrados()
        {

            dynamic historial = await _context.Guestacces.Select(x => new
            {
                x.GuestAccessId,
                x.GuestId,
                x.UrbanizationId,
                x.UserId,
                x.Guest.Name,
                x.Guest.House.Mz,
                x.Guest.House.Villa,
                x.accessFrom,
                x.accessTo
              /*  x.UserId,
                x.HouseId,
                x.GuestId,
                x.UrbanizationId,
                x.Name,
                x.House.Mz,
                x.House.Villa,*/
               


            }).ToListAsync();

            return historial;
          
        }



    }
}
