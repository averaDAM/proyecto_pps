<<<<<<< HEAD
# Proyecto PPS - Docker

## Servicios

- NGINX en `:80` (reverse proxy)
- Angular SSR (`frontend`) en red interna
- API .NET (`api`) en red interna
- PostgreSQL + Exporter
- Prometheus + Grafana

## Puesta en marcha

1) Define las variables de entorno en un `.env` junto a `docker-compose.yml`:

```
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
```

2) Construye y levanta los servicios:

```
docker compose up -d --build
```

3) Acceso

- Frontend: `http://localhost/app/`
- API (proxied): `http://localhost/api/` (por ejemplo `http://localhost/api/api/videojuegos`)
- Grafana: `http://localhost/grafana/`

Notas:

- El API usa la cadena `ConnectionStrings__ApiVideojuegos` y desactiva la redirección HTTPS en contenedor via `DISABLE_HTTPS_REDIRECT=true`.
- CORS permitido por defecto: `http://localhost,http://localhost:4200`. Ajusta `CORS_ALLOWED_ORIGINS` si necesitas más orígenes.
=======
# MUltiple Services - Project

This repository contains a list of multiple services, 
>>>>>>> main
