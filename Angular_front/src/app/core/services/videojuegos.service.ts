import { HttpClient, HttpParams } from '@angular/common/http';
import { isPlatformServer } from '@angular/common';
import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedVideojuegos } from '../models/videojuego.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class VideojuegosService {
  private http = inject(HttpClient);
  private platformId = inject(PLATFORM_ID);
  private baseUrl = environment.apiBaseUrl; // '/api' en dev/proxy o detrás de NGINX

  list(opts: { genero?: string; nombre?: string; numeroPagina?: number; tamanioPagina?: number }): Observable<PagedVideojuegos> {
    let params = new HttpParams()
      .set('numeroPagina', String(opts.numeroPagina ?? 1))
      .set('tamanioPagina', String(opts.tamanioPagina ?? 5));

    if (opts.genero) params = params.set('genero', opts.genero);
    if (opts.nombre) params = params.set('nombre', opts.nombre);

    const baseUrl = isPlatformServer(this.platformId) ? 'http://nginx/api' : environment.apiBaseUrl;
    return this.http.get<PagedVideojuegos>(`${baseUrl}/videojuegos`, { params });
  }
}
