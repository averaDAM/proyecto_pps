using Api_PPS;

namespace Api_PPS
{
    public interface IGestionVideojuegos
    {
        Task<List<Videojuego>> ObtenerTodos();
        Task<(List<Videojuego> videojuegos, int totalRegistros)> ObtenerTodosFiltrado(string? genero, string? nombre, int numeroPagina, int tamañoPagina);
        Task<Videojuego?> ObtenerPorId(int id);

        Task<bool> GuardarCambios();

        void Agregar(Videojuego videojuego);

        void ActualizarVideojuego(Videojuego videojuego);

        // Método para eliminar videojuego
        void EliminarVideojuego(Videojuego videojuego);
    }
}