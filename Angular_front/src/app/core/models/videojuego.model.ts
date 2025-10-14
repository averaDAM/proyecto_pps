export interface Videojuego {
    id: number;
    nombre: string;
    desarrollador?: string | null;
    genero?: string | null;
    anioLanzamiento: number;
    horasJuego: number;
    puntuacion: number;
    fechaLanzamiento: string; 
    fechaFinSoporte: string;  
  }
  
  export interface PagedVideojuegos {
    videojuegos: Videojuego[];
    totalRegistros: number;
  }
  