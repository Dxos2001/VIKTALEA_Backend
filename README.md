# VIKTALEA Backend

API REST construida con ASP.NET Core 8 y Entity Framework Core para administrar el cat?logo de clientes de VIKTALEA. El proyecto expone operaciones CRUD, paginaci?n y filtros básicos, y se integra con una base de datos Oracle.

## Caracter?sticas principales

- ASP.NET Core Web API (`net8.0`) con Swagger para exploraci?n interactiva.
- Entity Framework Core 9 + Oracle EF Provider (`Oracle.EntityFrameworkCore`) para acceso a datos.
- Middleware centralizado de manejo de excepciones que devuelve respuestas JSON consistentes (`ApiResponse`).
- Servicio de dominio (`ClienteService`) que encapsula reglas de negocio y validaciones para clientes.
- Soporte de paginaci?n, filtrado por RUC y raz?n social, y fechas de auditor?a (`createdAt`, `updatedAt`).

## Requisitos previos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- Base de datos Oracle accesible (por ejemplo Oracle Database 21c/23c, Autonomous DB o XE)
- Herramientas de l?nea de comandos `dotnet-ef` (se restauran desde el proyecto al ejecutar `dotnet tool restore` si se usa manifest; alternativamente `dotnet tool install --global dotnet-ef`)

## Configuraci?n inicial

1. Restaurar dependencias y compilar:
   ```bash
   dotnet restore VIKTALEA_Backend/VIKTALEA_Backend.csproj
   dotnet build VIKTALEA_Backend/VIKTALEA_Backend.csproj
   ```
2. Definir la cadena de conexi?n `ConnectionStrings:OracleDbConnection`. Existen dos opciones:
   - **Secrets en desarrollo** (recomendado):
     ```bash
     cd VIKTALEA_Backend
     dotnet user-secrets init
     dotnet user-secrets set "ConnectionStrings:OracleDbConnection" "User Id=VIKTALEA;Password=********;Data Source=localhost:1521/XEPDB1"
     ```
   - **Archivo de configuraci?n**: editar `VIKTALEA_Backend/appsettings.json` y rellenar `ConnectionStrings.OracleDbConnection` (evitar comprometer secretos si se versiona el repositorio).

> Si se despliega en producci?n, tambi?n puede definirse la variable de entorno `ConnectionStrings_OracleDbConnection`.

## Migraciones y base de datos

- Crear la base de datos a partir de la migraci?n inicial:
  ```bash
  cd VIKTALEA_Backend
  dotnet ef database update --project VIKTALEA_Backend/VIKTALEA_Backend.csproj --startup-project VIKTALEA_Backend/VIKTALEA_Backend.csproj
  ```
- Para generar nuevas migraciones:
  ```bash
  dotnet ef migrations add NombreNuevaMigracion --project VIKTALEA_Backend/VIKTALEA_Backend.csproj --startup-project VIKTALEA_Backend/VIKTALEA_Backend.csproj
  ```

El `AppDbContextFactory` usa la cadena de conexi?n configurada para soportar comandos de dise?o (`dotnet ef`).

## Ejecuci?n local

```bash
cd VIKTALEA_Backend
dotnet watch run --project VIKTALEA_Backend/VIKTALEA_Backend.csproj
```

La API se publica por defecto en `https://localhost:7100` (ver perfiles en `Properties/launchSettings.json`) y la documentaci?n Swagger en `/swagger`.

## Endpoints principales

| M?todo | Ruta | Descripci?n |
| --- | --- | --- |
| GET | `/api/Cliente` | Lista clientes con paginaci?n (`page`, `pageSize`) y filtros opcionales por `ruc` y `razonSocial`. |
| GET | `/api/Cliente/{id}` | Obtiene un cliente por identificador. |
| POST | `/api/Cliente` | Crea un nuevo cliente. |
| PUT | `/api/Cliente/{id}` | Actualiza datos existentes, valida duplicidad de RUC. |
| DELETE | `/api/Cliente/{id}` | Elimina (hard delete) un cliente. |

### Esquema `Cliente`

```json
{
  "id": 1,
  "ruc": "12345678901",
  "razonSocial": "Empresa SAC",
  "telefonoContacto": "999-888-777",
  "correoContacto": "contacto@empresa.com",
  "direccion": "Av. Principal 123",
  "activate": true,
  "createdAt": "2025-04-10T20:24:00Z",
  "updatedAt": "2025-04-10T20:30:00Z"
}
```

Las respuestas siguen el contrato `ApiResponse<T>`:
```json
{
  "success": true,
  "traceId": "REQUEST_TRACE_ID",
  "data": {},
  "error": null,
  "pagination": {
    "totalRecords": 100,
    "page": 1,
    "pageSize": 10,
    "totalPages": 10
  }
}
```

En caso de error el middleware `ExceptionHandlingMiddleware` devuelve `success = false` y un objeto `error` con detalle.

## Estructura del proyecto

```
VIKTALEA_Backend/
??? VIKTALEA_Backend/          # C?digo fuente ASP.NET Core
?   ??? Controllers/           # Controladores API (ClienteController)
?   ??? Data/                  # Factory de DbContext para comandos dotnet ef
?   ??? Migrations/            # Migraciones EF Core
?   ??? Models/                # Entidades EF (`Clientes`, `AppDbContext`)
?   ??? Schemas/               # DTOs expuestos por la API
?   ??? Services/              # Servicios de dominio e interfaces
?   ??? Shared/                # Respuestas gen?ricas, paginaci?n y middleware
??? README.md
```

## Notas y pr?ximos pasos

- Revisar el archivo `AppDbContext.cs` para mover la cadena de conexi?n embebida a configuraci?n segura.
- Agregar pruebas automatizadas para servicios y controladores.
- Considerar un `soft delete` en lugar de eliminaci?n f?sica si se requiere historial.
