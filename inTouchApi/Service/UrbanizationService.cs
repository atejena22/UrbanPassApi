using inTouchApi.Interface;
using inTouchApi.Model;
using inTouchApi.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace inTouchApi.Service
{
    public class UrbanizationService : UrbanizationInterface
    {
        private readonly intouchContext _context;
        public UrbanizationService(intouchContext context)
        {
            this._context = context;
        }


        public async Task<Respuesta> PresentarUrbanizacion()
        {
            Respuesta resp = new Respuesta();
            try
            {
                var listUsuario = await _context.Urbanizations.ToListAsync();
                resp.Exito = 1;
                resp.Mensaje = "Lista de Urbanización cargada con éxito";
                resp.Data = listUsuario;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al cargar la lista de Urbanización" + ex.Message;
                return resp;
            }
        }

        public async Task<Respuesta> PresentarUrbanizacionActual(int id)
        {
            Respuesta resp = new Respuesta();
            try
            {
                var listUsuario = await _context.Urbanizations.Where(x => x.UrbanizationId == id).FirstOrDefaultAsync();
                resp.Exito = 1;
                resp.Mensaje = "Lista de Urbanización Actual cargada con éxito";
                resp.Data = listUsuario;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al cargar la lista de Urbanización Actual" + ex.Message;
                return resp;
            }
        }



        public async Task<Respuesta> InsertarNuevaUrbanizacion([FromBody] Urbanization urbanization)
        {
            Respuesta resp = new Respuesta();
            try
            {
                Urbanization urban = new Urbanization();
                urban.Urbanization1 = urbanization.Urbanization1;
                urban.ContactNumber = urbanization.ContactNumber;
                urban.ContactEmail = urbanization.ContactEmail;
                urban.ContactName= urbanization.ContactName;
                urban.Active = urbanization.Active;
                urban.activeFrom = urbanization.activeFrom;
                urban.activeTo= urbanization.activeTo;
                urban.Ruc= urbanization.Ruc;
                urban.City= urbanization.City;
                urban.Country= urbanization.Country;
                urban.Image= urbanization.Image;

                _context.Urbanizations.Add(urban);
                await _context.SaveChangesAsync();

                resp.Exito = 1;
                resp.Mensaje = "Urbanización registrada con éxito";
                resp.Data = urban;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al registrar nueva Urbanización" + ex.Message;
                return resp;
            }
        }


        public async Task<Respuesta> ActualizarUrbanizacion([FromBody] Urbanization urbanization)
        {
            Respuesta resp = new Respuesta();
            try
            {
                var urbanizationActual = await _context.Urbanizations.Where(x => x.UrbanizationId == urbanization.UrbanizationId).FirstOrDefaultAsync();
                urbanizationActual.UrbanizationId=urbanization.UrbanizationId;
                urbanizationActual.Urbanization1 = urbanization.Urbanization1;
                urbanizationActual.ContactNumber = urbanization.ContactNumber;
                urbanizationActual.ContactEmail = urbanization.ContactEmail;
                urbanizationActual.ContactName = urbanization.ContactName;
                urbanizationActual.Active = urbanization.Active;
                urbanizationActual.activeFrom = urbanization.activeFrom;
                urbanizationActual.activeTo = urbanization.activeTo;
                urbanizationActual.Ruc = urbanization.Ruc;
                urbanizationActual.City = urbanization.City;
                urbanizationActual.Country = urbanization.Country;
                urbanizationActual.Image = urbanization.Image;
                _context.Entry(urbanizationActual).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                resp.Exito = 1;
                resp.Mensaje = "Urbanización actualizada con éxito";
                resp.Data = urbanizationActual;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Exito = 0;
                resp.Mensaje = "Error al actualizar Urbanización" + ex.Message;
                return resp;
            }
        }








    }
}
