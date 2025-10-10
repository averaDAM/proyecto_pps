using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Api_PPS;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var corsPolicyName = "AllowAngular";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Cambiar a servicios de videojuegos
builder.Services.AddScoped<IGestionVideojuegos, GestionVideojuegosBBDD>();

// Agregar BBDD (SQLite)
builder.Services.AddDbContext<ContextoApi>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ApiVideojuegos"));
});

var app = builder.Build();

// Rellenar la tabla de videojuegos al arrancar la aplicación con datos simulados
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContextoApi>();
    db.Database.Migrate();

    if (!db.Videojuegos.Any())
    {
        try
        {
            var videojuegosData = new List<(string nombre, string desarrollador, string genero, int anio, int horas, double puntuacion)>
            {
                 ("The Legend of Zelda: Breath of the Wild", "Nintendo", "Aventura", 2017, 100, 9.7),
                 ("The Witcher 3: Wild Hunt", "CD Projekt", "RPG", 2015, 120, 9.6),
                 ("Red Dead Redemption 2", "Rockstar Games", "Accion", 2018, 80, 9.8),
                 ("Super Mario Odyssey", "Nintendo", "Plataformas", 2017, 50, 9.5),
                 ("God of War", "Santa Monica Studio", "Accion", 2018, 40, 9.4),
                 ("Minecraft", "Mojang", "Sandbox", 2011, 500, 9.0),
                 ("Dark Souls", "FromSoftware", "RPG", 2011, 70, 9.2),
                 ("Grand Theft Auto V", "Rockstar Games", "Accion", 2013, 60, 9.5),
                 ("Hollow Knight", "Team Cherry", "Metroidvania", 2017, 40, 9.3),
                 ("Celeste", "Matt Makes Games", "Plataformas", 2018, 15, 9.2),
                 ("Persona 5", "Atlus", "RPG", 2016, 100, 9.6),
                 ("Sekiro: Shadows Die Twice", "FromSoftware", "Accion", 2019, 50, 9.1),
                 ("The Last of Us Part II", "Naughty Dog", "Aventura", 2020, 30, 9.3),
                 ("DOOM Eternal", "id Software", "Shooter", 2020, 25, 9.0),
                 ("Animal Crossing: New Horizons", "Nintendo", "Simulacion", 2020, 200, 8.9),
                 ("Cyberpunk 2077", "CD Projekt", "RPG", 2020, 60, 7.5),
                 ("Overwatch", "Blizzard", "Shooter", 2016, 300, 8.8),
                 ("Fortnite", "Epic Games", "Battle Royale", 2017, 400, 8.0),
                 ("League of Legends", "Riot Games", "MOBA", 2009, 1000, 8.5),
                 ("Counter-Strike: Global Offensive", "Valve", "Shooter", 2012, 800, 8.7)
            };

            var videojuegos = new List<Videojuego>();
            var random = new Random();

            foreach (var (nombre, desarrollador, genero, anio, horas, puntuacion) in videojuegosData)
            {
                var fechaLanzamiento = new DateTime(anio, random.Next(1, 13), random.Next(1, 28));
                var fechaFinSoporte = fechaLanzamiento.AddYears(random.Next(2, 8));

                var videojuego = new Videojuego
                {
                    Nombre = nombre,
                    Desarrollador = desarrollador,
                    Genero = genero,
                    AnioLanzamiento = anio,
                    HorasJuego = horas,
                    Puntuacion = puntuacion,
                    FechaLanzamiento = fechaLanzamiento,
                    FechaFinSoporte = fechaFinSoporte
                };

                videojuegos.Add(videojuego);
            }

            db.Videojuegos.AddRange(videojuegos);
            await db.SaveChangesAsync();
            Console.WriteLine($"Se han agregado {videojuegos.Count} videojuegos a la base de datos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error general obteniendo videojuegos: {ex.Message}");
            var videojuegosRespaldo = new List<Videojuego>
            {
                new() { Nombre = "The Legend of Zelda: Ocarina of Time", Desarrollador = "Nintendo", Genero = "Aventura", AnioLanzamiento = 1998, HorasJuego = 40, Puntuacion = 9.8, FechaLanzamiento = new DateTime(1998, 11, 21), FechaFinSoporte = new DateTime(2005, 11, 21) },
                new() { Nombre = "Super Mario Bros.", Desarrollador = "Nintendo", Genero = "Plataformas", AnioLanzamiento = 1985, HorasJuego = 10, Puntuacion = 9.4, FechaLanzamiento = new DateTime(1985, 9, 13), FechaFinSoporte = new DateTime(1992, 9, 13) }
            };
            db.Videojuegos.AddRange(videojuegosRespaldo);
            await db.SaveChangesAsync();
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseAuthorization();
app.MapControllers();
app.Run();