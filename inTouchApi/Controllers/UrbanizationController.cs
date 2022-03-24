using inTouchApi.Interface;
using inTouchApi.Model;
using inTouchApi.Response;
using inTouchApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inTouchApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UrbanizationController : ControllerBase
    {
        private UrbanizationInterface _Urbanizacionicontext;
        public UrbanizationController(UrbanizationInterface Urbanizacionicontext)
        {
            _Urbanizacionicontext = Urbanizacionicontext;
        }

        /***************************************** -LISTAR TODAS LAS URBANIZACIONES PARA ADMIN- ****************************************************************************/
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            Respuesta urbanizationResponse = await _Urbanizacionicontext.PresentarUrbanizacion();
            if (urbanizationResponse.Exito == 1)
            {
                return Ok(urbanizationResponse);
            }
            else
            {
                return Ok(urbanizationResponse.Mensaje);
            }
        }

        /***************************************** -LISTAR URBANIZACION ACTUAL- ****************************************************************************/
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUrban(int id)
        {
            // return await _context..Where(x => x.UrbanizationId == id).ToListAsync();
            Respuesta urbanizationResponse = await _Urbanizacionicontext.PresentarUrbanizacionActual(id);
            if (urbanizationResponse.Exito == 1)
            {
                return Ok(urbanizationResponse);
            }
            else
            {
                return Ok(urbanizationResponse.Mensaje);
            }
        }



        /***************************************** -REGISTRAR NUEVA URBANIZACION- ****************************************************************************/
        [HttpPost]
        public async Task<ActionResult> PostUrbanization([FromBody] Urbanization urbanization)
        {
            Respuesta urbanizationResponse = await _Urbanizacionicontext.InsertarNuevaUrbanizacion(urbanization);
            if (urbanizationResponse.Exito==1)
            {
                return Ok(urbanizationResponse);
            }
            else
            {
                return Ok(urbanizationResponse.Mensaje);
            }
        }


        /***************************************** -ACTUALIZAR URBANIZACION- ****************************************************************************/
        [HttpPut]
        public async Task<ActionResult> PutUrbanization([FromBody] Urbanization urbanization)
        {
            Respuesta urbanizationResponse = await _Urbanizacionicontext.ActualizarUrbanizacion(urbanization);
            if (urbanizationResponse.Exito == 1)
            {
                return Ok(urbanizationResponse);
            }
            else
            {
                return Ok(urbanizationResponse.Mensaje);
            }
        }









    }
}
