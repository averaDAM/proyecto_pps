import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedVideojuegos } from '../models/videojuego.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class VideojuegosService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiBaseUrl; // '/api' en dev/proxy o detrás de NGINX

  list(opts: { genero?: string; nombre?: string; numeroPagina?: number; tamanioPagina?: number }): Observable<PagedVideojuegos> {
    let params = new HttpParams()
      .set('numeroPagina', String(opts.numeroPagina ?? 1))
      .set('tamanioPagina', String(opts.tamanioPagina ?? 5));

    if (opts.genero) params = params.set('genero', opts.genero);
    if (opts.nombre) params = params.set('nombre', opts.nombre);

    return this.http.get<PagedVideojuegos>(`${this.baseUrl}/videojuegos`, { params });
  }
}
