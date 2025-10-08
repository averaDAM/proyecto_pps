using ApiVideojuegos.Contexto;
using ApiVideojuegos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiVideojuegos.Servicios
{
    public class GestionVideojuegosBBDD : IGestionVideojuegos
    {
        private readonly ContextoApi contextoApi;

        public GestionVideojuegosBBDD(ContextoApi contextoApi)
        {
            this.contextoApi = contextoApi;
        }

        public void ActualizarVideojuego(Videojuego videojuego)
        {
            contextoApi.Videojuegos.Update(videojuego);
        }

        public void Agregar(Videojuego videojuego)
        {
            contextoApi.Videojuegos.Add(videojuego);
        }

        public void EliminarVideojuego(Videojuego videojuego)
        {
            contextoApi.Videojuegos.Remove(videojuego);
        }

        public async Task<bool> GuardarCambios()
        {
            return await contextoApi.SaveChangesAsync() > 0;
        }

        public async Task<Videojuego?> ObtenerPorId(int id)
        {
            return await contextoApi.Videojuegos.FirstOrDefaultAsync(videojuego => videojuego.Id == id);
        }

        public async Task<List<Videojuego>> ObtenerTodos()
        {
            return await contextoApi.Videojuegos.ToListAsync();
        }

        public async Task<(List<Videojuego> videojuegos, int totalRegistros)> ObtenerTodosFiltrado(string? genero, string? nombre, int numeroPagina, int tamañoPagina)
        {
            var consulta = contextoApi.Videojuegos.AsQueryable();

            if (!string.IsNullOrEmpty(genero))
            {
                genero = genero.Trim();
                consulta = consulta.Where(videojuego => videojuego.Genero == genero);
            }

            if (!string.IsNullOrEmpty(nombre))
            {
                nombre = nombre.Trim();
                consulta = consulta.Where(videojuego => videojuego.Nombre.Contains(nombre));
            }

            var totalRegistros = await consulta.CountAsync();

            var videojuegos = await consulta
                .OrderBy(videojuego => videojuego.Nombre)
                .Skip((numeroPagina - 1) * tamañoPagina)
                .Take(tamañoPagina)
                .ToListAsync();

            return (videojuegos, totalRegistros);
        }
    }
}
