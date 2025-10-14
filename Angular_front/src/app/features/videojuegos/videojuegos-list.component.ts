import { CommonModule } from '@angular/common';
import { Component, OnInit, computed, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { VideojuegosService } from '../../core/services/videojuegos.service';
import { Videojuego } from '../../core/models/videojuego.model';

@Component({
  selector: 'app-videojuegos-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './videojuegos-list.component.html',
  styleUrls: ['./videojuegos-list.component.css'],
})
export class VideojuegosListComponent implements OnInit {
  nombre = signal<string>('');
  genero = signal<string>('');
  pagina = signal<number>(1);
  readonly tamanioPagina = 5;

  cargando = signal<boolean>(false);
  error = signal<string | null>(null);
  videojuegos = signal<Videojuego[]>([]);
  total = signal<number>(0);
  paginasTotales = computed(() => Math.max(1, Math.ceil(this.total() / this.tamanioPagina)));

  constructor(private svc: VideojuegosService) {}

  ngOnInit(): void { this.cargar(); }

  cargar(): void {
    this.cargando.set(true);
    this.error.set(null);
    this.svc.list({
      nombre: this.nombre().trim() || undefined,
      genero: this.genero().trim() || undefined,
      numeroPagina: this.pagina(),
      tamanioPagina: this.tamanioPagina
    }).subscribe({
      next: r => { this.videojuegos.set(r.videojuegos ?? []); this.total.set(r.totalRegistros ?? 0); this.cargando.set(false); },
      error: err => { this.error.set(err?.message ?? 'Error desconocido'); this.cargando.set(false); }
    });
  }

  buscar(): void { this.pagina.set(1); this.cargar(); }
  limpiar(): void { this.nombre.set(''); this.genero.set(''); this.pagina.set(1); this.cargar(); }
  prev(): void { if (this.pagina() > 1) { this.pagina.update(p => p - 1); this.cargar(); } }
  next(): void { if (this.pagina() < this.paginasTotales()) { this.pagina.update(p => p + 1); this.cargar(); } }

  fFecha(iso: string): string {
    if (!iso) return '—';
    const d = new Date(iso); return isNaN(d.getTime()) ? '—' : d.toLocaleDateString();
  }
}
