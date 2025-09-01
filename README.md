# 🎵 Proyecto AA2 - Aplicación de Gestión Musical

Una aplicación web completa para la gestión de música, artistas, álbumes y playlists.

## 📋 Descripción del Proyecto

Esta aplicación permite a los usuarios explorar, gestionar y organizar contenido musical de manera intuitiva. Cuenta con un sistema de roles que diferencia entre administradores y usuarios clientes, ofreciendo diferentes niveles de funcionalidad según el tipo de usuario.

## 🚀 Tecnologías Utilizadas

### Backend (.NET 8)
- **ASP.NET Core Web API** - Framework principal
- **Entity Framework Core** - ORM para manejo de datos
- **SQL Server** - Base de datos principal
- **JWT (JSON Web Tokens)** - Autenticación y autorización
- **Swagger/OpenAPI** - Documentación de API
- **Docker** - Containerización

### Frontend (Vue.js 3)
- **Vue 3 (Composition API)** - Framework de frontend
- **TypeScript** - Tipado estático
- **Vuetify 3** - Framework de componentes UI
- **Pinia** - Gestión de estado
- **Vue Router 4** - Enrutamiento
- **Axios** - Cliente HTTP
- **Vite** - Herramienta de build
- **Docker + Nginx** - Despliegue

### Infraestructura
- **Docker Compose** - Orquestación de contenedores
- **Nginx** - Servidor web para el frontend
- **SQL Server 2022** - Base de datos en contenedor

## 🎯 Funcionalidades Principales

### 👤 Sistema de Usuarios
- **Registro de usuarios** con validación de datos
- **Inicio de sesión** con autenticación JWT
- **Roles diferenciados**: Administrador y Cliente
- **Gestión de perfiles** de usuario

### 🎤 Gestión de Artistas
- **Visualización** de catálogo de artistas
- **Búsqueda y filtrado** por nombre y número de seguidores
- **Ordenamiento** ascendente/descendente
- **CRUD completo** (solo administradores):
  - Crear nuevos artistas
  - Editar información existente
  - Eliminar artistas
- **Información detallada**: biografía, imagen, seguidores

### 💿 Gestión de Álbumes
- **Exploración** de álbumes por artista
- **Visualización** de información detallada
- **Administración** (solo administradores):
  - Crear álbumes asociados a artistas
  - Editar información de álbumes
  - Gestión de imágenes y metadatos

### 🎵 Gestión de Canciones
- **Visualización** de tracks por álbum
- **Información detallada** de cada canción
- **Administración** (solo administradores):
  - Agregar nuevas canciones
  - Editar información existente
  - Eliminar tracks
  - Gestión de duración y metadatos

### 📝 Sistema de Playlists
- **Creación** de playlists personalizadas (usuarios autenticados)
- **Gestión** de contenido:
  - Agregar canciones a playlists
  - Eliminar canciones de playlists
  - Editar información de playlists
- **Filtrado** y búsqueda de playlists propias
- **Organización** personal de música favorita

### 🔍 Funciones de Búsqueda
- **Búsqueda global** por nombre de artista
- **Filtros avanzados**:
  - Por número de seguidores
  - Ordenamiento múltiple
  - Búsqueda en tiempo real

## 👥 Roles de Usuario

### 🔧 Administrador
- Acceso completo a todas las funcionalidades
- Gestión de artistas, álbumes y canciones
- Operaciones CRUD en toda la base de datos

### 👤 Cliente
- Navegación y exploración del catálogo
- Creación y gestión de playlists personales
- Búsqueda y filtrado de contenido

### 🌐 Usuario No Autenticado
- Exploración básica del catálogo
- Visualización de artistas y álbumes
- Acceso limitado sin funcionalidades de gestión

### Componentes Principales

#### Backend API
- **Controllers**: Manejo de endpoints REST
- **Services**: Lógica de negocio
- **Repositories**: Acceso a datos con Entity Framework
- **Models**: Entidades de dominio
- **Authentication**: Sistema JWT para seguridad

#### Frontend SPA
- **Views**: Páginas principales de la aplicación
- **Components**: Componentes reutilizables de UI
- **Stores**: Gestión de estado con Pinia
- **Services**: Comunicación con API
- **Router**: Navegación y protección de rutas

### Comandos de Despliegue

```bash
# Construir y ejecutar todos los servicios
docker-compose up --build

# Ejecutar en segundo plano
docker-compose up -d

# Ver logs de servicios
docker-compose logs -f

# Detener servicios
docker-compose down
```

## 📊 Modelo de Datos

### Entidades Principales

- **Users**: Información de usuarios y roles
- **Artists**: Catálogo de artistas musicales
- **Albums**: Álbumes asociados a artistas
- **Tracks**: Canciones individuales por álbum
- **Playlists**: Listas de reproducción de usuarios
- **PlaylistTrack**: Relación muchos-a-muchos entre playlists y tracks

## 🔧 Configuración de Desarrollo

### Requisitos Previos
- Docker Desktop
- Node.js 18+ (para desarrollo local del frontend)
- .NET 8 SDK (para desarrollo local del backend)

### Variables de Entorno

```env
# Base de datos
SA_PASSWORD=Admin1234!
ConnectionString="Server=db;Database=musicDb;User Id=sa;Password=Admin1234!;TrustServerCertificate=true;"

# JWT
JWT_SECRET=tu_clave_secreta_jwt
JWT_ISSUER=proyecto-aa2
JWT_AUDIENCE=proyecto-aa2-users
```

## 🌐 Endpoints de API

### Autenticación
- `POST /auth/login` - Inicio de sesión
- `POST /auth/register` - Registro de usuarios

### Artistas
- `GET /artists` - Listar artistas con filtros
- `POST /artists` - Crear artista (admin)
- `PUT /artists/{id}` - Actualizar artista (admin)
- `DELETE /artists/{id}` - Eliminar artista (admin)

### Álbumes
- `GET /artists/{id}/albums` - Álbumes por artista
- `POST /albums` - Crear álbum (admin)

### Canciones
- `GET /albums/{id}/tracks` - Canciones por álbum
- `POST /tracks` - Crear canción (admin)
- `PUT /tracks/{id}` - Actualizar canción (admin)
- `DELETE /tracks/{id}` - Eliminar canción (admin)

### Playlists
- `GET /playlists/user` - Playlists del usuario
- `POST /playlists` - Crear playlist
- `PUT /playlists/{id}` - Actualizar playlist
- `DELETE /playlists/{id}` - Eliminar playlist
- `POST /playlists/{playlistId}/add/{trackId}` - Agregar canción
- `DELETE /playlists/{playlistId}/remove/{trackId}` - Quitar canción

## 🚀 Acceso a la Aplicación

Una vez desplegada la aplicación:

- **Frontend**: http://localhost:9000
- **API**: http://localhost:7818
- **Swagger UI**: http://localhost:7818/swagger
- **Base de datos**: localhost:8187
