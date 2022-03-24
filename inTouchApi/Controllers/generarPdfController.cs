using inTouchApi.Response;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace inTouchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class generarPdfController : ControllerBase
    {
        // GET: api/<generarPdfController>
        [HttpGet]
        public async Task<Respuesta> Get()
        {
            Respuesta resp = new Respuesta();
            try
            {
                Document doc = new Document(PageSize.LETTER);
                doc.AddTitle("Mi primer PDF");
                doc.AddCreator("Roberto Torres");
                //Le agregamos un segundo párrafo
                PdfWriter writer = PdfWriter.GetInstance(doc,
                                            new FileStream(@"D:\pruueba.pdf", FileMode.Create));
                // Indicamos donde vamos a guardar el documento
                doc.Open();
                // Creamos un titulo personalizado con tamaño de fuente 18 y color Azul
                Paragraph title = new Paragraph();
                title.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, BaseColor.BLUE);
                title.Add("Crear una tabla en PDF con iTextSharp");
                doc.Add(title);
                // Agregamos un parrafo vacio como separacion.
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph("Este es mi primer PDF"));
                doc.Add(new Paragraph("Segundo párrafo"));
                doc.Add(new Paragraph(""));


                PdfPTable table = new PdfPTable(3);
                PdfPCell cell = new PdfPCell(new Phrase("Header spanning 3 columns"));
                cell.Colspan = 3;
                cell.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
                table.AddCell("Col 1 Row 1");
                table.AddCell("Col 2 Row 1");
                table.AddCell("Col 3 Row 1");
                table.AddCell("Col 1 Row 2");
                table.AddCell("Col 2 Row 2");
                table.AddCell("Col 3 Row 2");
                doc.Add(table);
                //doc.Add(table);

                doc.Close();

                //Cerramos el documento
                // doc.Close();

                // Abrimos el archivo
                //doc.Open();

                resp.Exito = 1;
                resp.Mensaje = "bien hecho";

                return resp;
            }
            catch (Exception ex)
            {

                resp.Exito = 0;
                resp.Mensaje = "error"+ ex.Message.ToString();
                return resp;
            }
           
            
        }

        
    }
}
