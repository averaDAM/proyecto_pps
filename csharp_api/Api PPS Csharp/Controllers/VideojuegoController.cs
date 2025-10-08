using Microsoft.AspNetCore.Mvc;
using ApiVideojuegos.Entidades;
using ApiVideojuegos.Servicios;

namespace ApiVideojuegos.Controllers
{
    // CRUD de videojuegos
    // Create, Read, Update, Delete
    // Crear, Leer, Actualizar, Borrar

    [Route("api/videojuegos")]
    [ApiController]
    public class VideojuegoController : ControllerBase
    {
        private readonly IGestionVideojuegos gestionVideojuegos;
        private readonly ILogger<VideojuegoController> logger;

        public VideojuegoController(IGestionVideojuegos gestionVideojuegos, ILogger<VideojuegoController> logger)
        {
            this.gestionVideojuegos = gestionVideojuegos;
            this.logger = logger;
        }

        const int TamañoPagina = 5;

        // Obtener todos los videojuegos
        [HttpGet]
        public async Task<ActionResult<List<Videojuego>>> Get([FromQuery] string? genero,
                                 [FromQuery] string? nombre,
                                 [FromQuery] int numeroPagina = 1,
                                 [FromQuery] int tamañoPagina = TamañoPagina)
        {
            logger.LogInformation("Obteniendo todos los videojuegos con filtros - Género: {Genero}, Nombre: {Nombre}", genero, nombre);

            var (videojuegos, totalRegistros) = await gestionVideojuegos.ObtenerTodosFiltrado(genero, nombre, numeroPagina, tamañoPagina);

            logger.LogInformation("Obteniendo todos los videojuegos. Número de videojuegos: {TotalRegistros}", totalRegistros);
            return Ok(new { videojuegos, totalRegistros });
        }

        // Obtener un videojuego por id
        [HttpGet("{id}")]
        public async Task<ActionResult<Videojuego>> Get([FromRoute] int id)
        {
            logger.LogInformation("Obteniendo videojuego con id {Id}", id);

            var videojuego = await gestionVideojuegos.ObtenerPorId(id);
            if (videojuego == null)
            {
                logger.LogError("No se ha encontrado el videojuego con id {Id}", id);
                return NotFound();
            }

            return Ok(videojuego);
        }

        // Alta de videojuego
        [HttpPost]
        public async Task<ActionResult<Videojuego>> Post([FromBody] Videojuego videojuego)
        {
            logger.LogInformation("Creando nuevo videojuego: {Nombre}", videojuego.Nombre);

            var modeloValido = TryValidateModel(videojuego);

            if (!modeloValido)
            {
                logger.LogWarning("Modelo de videojuego no válido para: {Nombre}", videojuego.Nombre);
                return BadRequest(ModelState);
            }

            gestionVideojuegos.Agregar(videojuego);
            await gestionVideojuegos.GuardarCambios();

            logger.LogInformation("Videojuego creado exitosamente con id {Id}: {Nombre}", videojuego.Id, videojuego.Nombre);
            return CreatedAtAction(nameof(Get), new { id = videojuego.Id }, videojuego);
        }

        // Actualizar videojuego
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] Videojuego videojuego)
        {
            logger.LogInformation("Actualizando videojuego con id {Id}", id);

            var videojuegoActual = await gestionVideojuegos.ObtenerPorId(id);
            if (videojuegoActual == null)
            {
                logger.LogError("No se encontró el videojuego con id {Id} para actualizar", id);
                return NotFound();
            }

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

            logger.LogInformation("Videojuego actualizado exitosamente: {Nombre}", videojuegoActual.Nombre);
            return NoContent();
        }

        // Baja de videojuego
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            logger.LogInformation("Eliminando videojuego con id {Id}", id);

            var videojuego = await gestionVideojuegos.ObtenerPorId(id);
            if (videojuego == null)
            {
                logger.LogError("No se encontró el videojuego con id {Id} para eliminar", id);
                return NotFound();
            }

            gestionVideojuegos.EliminarVideojuego(videojuego);
            await gestionVideojuegos.GuardarCambios();

            logger.LogInformation("Videojuego eliminado exitosamente: {Nombre}", videojuego.Nombre);
            return Ok();
        }
    }
}
