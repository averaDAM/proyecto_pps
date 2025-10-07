using System.ComponentModel.DataAnnotations;

namespace ApiVideojuegos.Entidades
{
    // Sistemas de validaci�n utilizados en esta clase:
    // 1-Atributos de validaci�n de datos de DataAnnotations
    // 2-Interfaz IValidatableObject

    public class Videojuego : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [MinLength(1)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Desarrollador { get; set; }

        [MaxLength(50)]
        public string? Genero { get; set; }

        [Range(1970, 2030)] // Primeros videojuegos comerciales en los 70
        public int A�oLanzamiento { get; set; }

        [Range(1, 1000)] // Horas de juego estimadas
        public int HorasJuego { get; set; } //comentario

        [Range(0.0, 10.0)] // Puntuaci�n del 0 al 10
        public double Puntuacion { get; set; }

        public DateTime FechaLanzamiento { get; set; }
        public DateTime FechaFinSoporte { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validar que el desarrollador y el g�nero sean diferentes
            if (!string.IsNullOrEmpty(Desarrollador) && !string.IsNullOrEmpty(Genero) && Desarrollador.Equals(Genero, StringComparison.OrdinalIgnoreCase))
            {
                yield return new ValidationResult("El desarrollador y el g�nero deben ser diferentes", new[] { nameof(Desarrollador), nameof(Genero) });
            }

            // Validar que la fecha de lanzamiento no sea posterior a la fecha de fin de soporte
            if (FechaLanzamiento >= FechaFinSoporte)
            {
                yield return new ValidationResult("La fecha de lanzamiento no puede ser posterior a la fecha de fin de soporte", new[] { nameof(FechaLanzamiento), nameof(FechaFinSoporte) });
            }

            // Validar que el a�o de lanzamiento sea consistente con la fecha de lanzamiento
            if (A�oLanzamiento != FechaLanzamiento.Year)
            {
                yield return new ValidationResult("El a�o de lanzamiento debe coincidir con el a�o de la fecha de lanzamiento", new[] { nameof(A�oLanzamiento), nameof(FechaLanzamiento) });
            }
        }
    }
}
