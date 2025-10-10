using System.ComponentModel.DataAnnotations;

namespace Api_PPS
{
    // Sistemas de validación utilizados en esta clase:
    // 1-Atributos de validación de datos de DataAnnotations
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
    public int AnioLanzamiento { get; set; }

        [Range(1, 1000)] // Horas de juego estimadas
        public int HorasJuego { get; set; }

        [Range(0.0, 10.0)] // Puntuación del 0 al 10
        public double Puntuacion { get; set; }

        public DateTime FechaLanzamiento { get; set; }
        public DateTime FechaFinSoporte { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validar que el desarrollador y el género sean diferentes
            if (!string.IsNullOrEmpty(Desarrollador) && !string.IsNullOrEmpty(Genero) && Desarrollador.Equals(Genero, StringComparison.OrdinalIgnoreCase))
            {
                yield return new ValidationResult("El desarrollador y el género deben ser diferentes", new[] { nameof(Desarrollador), nameof(Genero) });
            }

            // Validar que la fecha de lanzamiento no sea posterior a la fecha de fin de soporte
            if (FechaLanzamiento >= FechaFinSoporte)
            {
                yield return new ValidationResult("La fecha de lanzamiento no puede ser posterior a la fecha de fin de soporte", new[] { nameof(FechaLanzamiento), nameof(FechaFinSoporte) });
            }

            // Validar que el año de lanzamiento sea consistente con la fecha de lanzamiento
            if (AnioLanzamiento != FechaLanzamiento.Year)
            {
                yield return new ValidationResult("El año de lanzamiento debe coincidir con el año de la fecha de lanzamiento", new[] { nameof(AnioLanzamiento), nameof(FechaLanzamiento) });
            }
        }
    }
}
