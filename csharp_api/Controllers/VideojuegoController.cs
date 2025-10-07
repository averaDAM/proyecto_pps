using Microsoft.AspNetCore.Mvc;
using ApiVideojuegos.Entidades;
using ApiVideojuegos.Servicios;

namespace ApiVideojuegos.Controllers
{
    [Route("api/videojuegos")]
    [ApiController]
    public class VideojuegoController : ControllerBase
    {
        private readonly IGestionVideojuegos gestionVideojuegos;

        public VideojuegoController(IGestionVideojuegos gestionVideojuegos)
        {
            this.gestionVideojuegos = gestionVideojuegos;
        }

        const int TamañoPagina = 5;

        // Obtener todos los videojuegos
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string? genero,
                                 [FromQuery] string? nombre,
                                 [FromQuery] int numeroPagina = 1,
                                 [FromQuery] int tamañoPagina = TamañoPagina)
        {
            var (videojuegos, totalRegistros) = await gestionVideojuegos.ObtenerTodosFiltrado(genero, nombre, numeroPagina, tamañoPagina);
            return Ok(new { videojuegos, totalRegistros });
        }

        // Obtener un videojuego por id
        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            var videojuego = await gestionVideojuegos.ObtenerPorId(id);
            if (videojuego == null)
                return NotFound();
            return Ok(videojuego);
        }

        // Alta de videojuego
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Videojuego videojuego)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            gestionVideojuegos.Agregar(videojuego);
            await gestionVideojuegos.GuardarCambios();
            return CreatedAtAction(nameof(Get), new { id = videojuego.Id }, videojuego);
        }

        // Actualizar videojuego
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] Videojuego videojuego)
        {
            var videojuegoActual = await gestionVideojuegos.ObtenerPorId(id);
            if (videojuegoActual == null)
                return NotFound();

            videojuegoActual.Nombre = videojuego.Nombre;
            videojuegoActual.Desarrollador = videojuego.Desarrollador;
            videojuegoActual.Genero = videojuego.Genero;
            videojuegoActual.AñoLanzamiento = videojuego.AñoLanzamiento;
            videojuegoActual.HorasJuego = videojuego.HorasJuego;
            videojuegoActual.Puntuacion = videojuego.Puntuacion;
            videojuegoActual.FechaLanzamiento = videojuego.FechaLanzamiento;
            videojuegoActual.FechaFinSoporte = videojuego.FechaFinSoporte;

            gestionVideojuegos.ActualizarVideojuego(videojuegoActual);
            await gestionVideojuegos.GuardarCambios();
            return NoContent();
        }

        // Baja de videojuego
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var videojuego = await gestionVideojuegos.ObtenerPorId(id);
            if (videojuego == null)
                return NotFound();

            gestionVideojuegos.EliminarVideojuego(videojuego);
            await gestionVideojuegos.GuardarCambios();
            return Ok();
        }
    }
}
