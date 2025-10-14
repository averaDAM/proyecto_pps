import { Routes } from '@angular/router';
import { VideojuegosListComponent } from './features/videojuegos/videojuegos-list.component';

export const routes: Routes = [
  { path: '', component: VideojuegosListComponent },
  { path: '**', redirectTo: '' }
];
