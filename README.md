# SubastaYa

API backend para una plataforma de subastas online, desarrollada con **ASP.NET Core 8**, **Entity Framework Core** y **SQL Server**.

El sistema permite gestionar usuarios, billeteras, categorías, subastas y pujas. También incorpora un **ledger de transacciones**, **auditoría**, cierre automático de subastas mediante un worker y notificaciones en tiempo real utilizando **SignalR**.

---

## Tecnologías

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core 8
* SQL Server
* Swagger / OpenAPI
* SignalR
* BCrypt
* C#
* Arquitectura por capas

---

## Arquitectura

El proyecto está dividido en cuatro capas principales:

```text
SubastaYa
│
├── SubastaYa.API
├── SubastaYa.Application
├── SubastaYa.Domain
└── SubastaYa.Infrastructure
```

### API

Responsable de exponer los endpoints HTTP, configurar el pipeline de ASP.NET Core, Swagger, CORS y SignalR.

Incluye:

* Controllers
* Middleware
* Configuración de Dependency Injection
* Database Seeder
* SignalR Hub

### Application

Contiene la lógica de aplicación mediante handlers y casos de uso.

Incluye funcionalidades relacionadas con:

* Usuarios
* Autenticación
* Billeteras
* Subastas
* Pujas
* Categorías
* Transactions Ledger
* Auditorías
* Worker de cierre de subastas

### Domain

Contiene las entidades y reglas propias del dominio.

Entidades principales:

* `User`
* `Wallet`
* `Auction`
* `Bid`
* `Category`
* `TransactionLedger`
* `AuditLog`

También contiene constantes y excepciones de dominio.

### Infrastructure

Contiene la persistencia y acceso a datos.

Incluye:

* `AppDbContext`
* Entity Framework Core
* Configuraciones de entidades
* Repositories
* `UnitOfWork`
* SQL Server

---

## Funcionalidades

### Usuarios

Permite:

* Crear usuarios
* Consultar usuarios
* Consultar subastas creadas por un usuario
* Consultar pujas realizadas por un usuario

Cada usuario puede tener una billetera asociada.

---

### Autenticación

Endpoint disponible para iniciar sesión:

```http
POST /api/auth/login
```

La contraseña se almacena utilizando hashing mediante BCrypt.

---

### Billetera

Cada usuario posee una billetera con:

* Saldo total
* Saldo retenido
* Saldo disponible

El saldo disponible se calcula como:

```text
AvailableBalance = TotalBalance - HeldBalance
```

Endpoints:

```http
GET /api/users/{userId}/wallet/balance
POST /api/users/{userId}/wallet/deposit
```

---

### Categorías

Permite crear y consultar categorías.

Endpoints:

```http
POST /api/categories
GET /api/categories
```

---

### Subastas

Permite:

* Crear subastas
* Consultar una subasta
* Consultar subastas paginadas

Endpoints:

```http
POST /api/auctions
GET /api/auctions/{id}
GET /api/auctions
```

Una subasta contiene información como:

* Vendedor
* Categoría
* Título
* Descripción
* Imagen
* Precio base
* Precio actual
* Incremento mínimo
* Fecha de inicio
* Fecha de finalización
* Estado

---

### Pujas

Las pujas se realizan sobre una subasta específica.

Endpoints:

```http
POST /api/auctions/{auctionId}/bids
GET /api/auctions/{auctionId}/bids
```

El sistema valida, entre otras condiciones:

* Que la subasta exista.
* Que esté activa.
* Que no haya finalizado.
* Que el vendedor no puje sobre su propia subasta.
* Que exista el comprador.
* Que el monto de la puja respete el incremento mínimo.
* Que el comprador tenga saldo disponible suficiente.

### Escrow

Cuando un usuario realiza una puja, el monto correspondiente se retiene en su billetera.

Si otro usuario supera la puja anterior, el monto retenido del usuario anterior es liberado.

Este movimiento queda registrado en el ledger de transacciones.

---

## Anti-sniping

El sistema incorpora una protección contra pujas realizadas cerca del final de una subasta.

Si una puja se realiza cuando quedan **60 segundos o menos**, la fecha de finalización de la subasta se extiende **2 minutos**.

La extensión queda registrada mediante un `AuditLog`.

---

## Transaction Ledger

El sistema mantiene un registro de los movimientos financieros relacionados con las billeteras.

Actualmente contempla operaciones como:

* `Deposit`
* `Retention`
* `Release`
* `Pay`
* `Collection`

Endpoints:

```http
GET /api/transactions/{id}
GET /api/transactions/wallet/{walletId}
```

El historial de transacciones por billetera utiliza paginación.

---

## Auditoría

Las operaciones relevantes pueden generar registros de auditoría mediante `AuditLog`.

Cada registro puede contener:

* Entidad
* ID de entidad
* Acción
* Usuario
* Detalle en JSON
* Fecha de creación

Endpoints:

```http
GET /api/audits
GET /api/audits/{id}
```

También es posible filtrar las auditorías por usuario mediante los parámetros correspondientes del endpoint.

---

## Cierre automático de subastas

El proyecto cuenta con un worker encargado de procesar automáticamente las subastas cuyo tiempo de finalización ya pasó.

Cuando una subasta finaliza:

### Subasta con pujas

El sistema:

1. Determina la puja más alta.
2. Finaliza la subasta.
3. Descuenta el importe retenido de la billetera del comprador.
4. Registra el pago en el ledger.
5. Acredita el importe en la billetera del vendedor.
6. Registra la cobranza en el ledger.
7. Registra una auditoría.

### Subasta sin pujas

El sistema:

1. Marca la subasta como desierta.
2. Registra el evento en auditoría.

---

## SignalR

El backend utiliza SignalR para enviar actualizaciones de las subastas en tiempo real.

Hub:

```text
/hubs/auction
```

Se utilizan notificaciones para eventos como:

* Nueva puja
* Extensión de tiempo de una subasta

---

## Base de datos

El proyecto utiliza:

**SQL Server**

La conexión se configura mediante:

```text
ConnectionStrings
└── DefaultConnection
```

Ejemplo de configuración local:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=SubastaYaDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Se recomienda utilizar una configuración local propia y no publicar credenciales o información sensible en el repositorio.

---

## Entity Framework Core

El contexto principal es:

```text
AppDbContext
```

DbSets principales:

```text
Users
Categories
Bids
Wallets
Auctions
TransactionLedgers
AuditLogs
```

Las configuraciones de las entidades se encuentran separadas en:

```text
SubastaYa.Infrastructure
└── Persistence
    └── Configurations
```

Se utiliza:

```csharp
modelBuilder.ApplyConfigurationsFromAssembly(
    typeof(AppDbContext).Assembly);
```

para aplicar automáticamente las configuraciones.

---

## Unit of Work

El proyecto utiliza el patrón **Unit of Work** para coordinar operaciones sobre el contexto de Entity Framework.

Permite:

* Iniciar transacciones.
* Confirmar transacciones.
* Revertir transacciones.
* Guardar cambios.
* Limpiar el tracking del contexto.

Esto resulta especialmente importante para operaciones como las pujas y el cierre de subastas, donde se modifican múltiples entidades relacionadas.

---

## Concurrencia

Las entidades `Auction` y `Wallet` utilizan una propiedad `Version` configurada como `RowVersion`.

Esto permite utilizar el mecanismo de concurrencia optimista de Entity Framework Core para evitar inconsistencias cuando múltiples operaciones intentan modificar simultáneamente los mismos datos.

---

## Seeder

Al iniciar la aplicación se ejecuta el `DatabaseSeeder`.

El seeder:

1. Ejecuta las migraciones pendientes.
2. Comprueba si existen usuarios.
3. Si la base está vacía, crea datos iniciales.

Incluye usuarios de prueba, billeteras, categorías, subastas, pujas y movimientos del ledger.

### Usuarios de prueba

```text
Vendedor
Email: vendedor@test.com
Password: contra123

Comprador 1
Email: comprador1@test.com
Password: contra123

Comprador 2
Email: comprador2@test.com
Password: contra123

Usuario sin fondos
Email: sinfondos@test.com
Password: contra123
```

Estos usuarios son únicamente para desarrollo y pruebas locales.

---

## Swagger

En entorno de desarrollo, Swagger está habilitado para explorar y probar la API.

Una vez ejecutado el proyecto, se puede acceder a la interfaz de Swagger desde:

```text
/swagger
```

---

## CORS

El backend permite solicitudes desde los siguientes orígenes configurados para desarrollo:

```text
http://127.0.0.1:5500
http://127.0.0.1:5501
```

Esto permite conectar el backend con un frontend ejecutándose localmente mediante un servidor de desarrollo.

---

## Requisitos

Para ejecutar el proyecto localmente se necesita:

* .NET 8 SDK
* SQL Server / SQL Server Express
* Visual Studio, Rider o VS Code
* Git

---

## Ejecución del proyecto

Clonar el repositorio:

```bash
git clone <URL_DEL_REPOSITORIO>
```

Ingresar al proyecto:

```bash
cd SubastaYa
```

Restaurar dependencias:

```bash
dotnet restore
```

Compilar:

```bash
dotnet build
```

Ejecutar:

```bash
dotnet run --project SubastaYa.API
```

Al iniciar la API, el `DatabaseSeeder` ejecutará las migraciones pendientes y, si la base de datos no contiene usuarios, cargará los datos iniciales.

---

## Migraciones

Las migraciones de Entity Framework Core se pueden administrar utilizando:

```bash
dotnet ef migrations add NombreMigracion --project SubastaYa.Infrastructure --startup-project SubastaYa.API
```

Para aplicar las migraciones:

```bash
dotnet ef database update --project SubastaYa.Infrastructure --startup-project SubastaYa.API
```

---

## Endpoints principales

| Método | Endpoint                              | Descripción                |
| ------ | ------------------------------------- | -------------------------- |
| POST   | `/api/auth/login`                     | Iniciar sesión             |
| POST   | `/api/users`                          | Crear usuario              |
| GET    | `/api/users/{id}`                     | Obtener usuario            |
| GET    | `/api/users/{sellerId}/auctions`      | Subastas de un usuario     |
| GET    | `/api/users/{buyerId}/bids`           | Pujas de un usuario        |
| GET    | `/api/users/{userId}/wallet/balance`  | Consultar saldo            |
| POST   | `/api/users/{userId}/wallet/deposit`  | Depositar saldo            |
| POST   | `/api/categories`                     | Crear categoría            |
| GET    | `/api/categories`                     | Obtener categorías         |
| POST   | `/api/auctions`                       | Crear subasta              |
| GET    | `/api/auctions/{id}`                  | Obtener subasta            |
| GET    | `/api/auctions`                       | Obtener subastas           |
| POST   | `/api/auctions/{auctionId}/bids`      | Crear puja                 |
| GET    | `/api/auctions/{auctionId}/bids`      | Historial de pujas         |
| GET    | `/api/transactions/{id}`              | Obtener transacción        |
| GET    | `/api/transactions/wallet/{walletId}` | Historial de transacciones |
| GET    | `/api/audits/{id}`                    | Obtener auditoría          |
| GET    | `/api/audits`                         | Obtener auditorías         |

---

## Estados de una subasta

El dominio contempla estados para representar el ciclo de vida de una subasta, incluyendo:

```text
Scheduled
Active
Finished
Deserted
```

Las transiciones son gestionadas por los casos de uso correspondientes y por el worker encargado del procesamiento automático.

---

## Estructura resumida

```text
SubastaYa
│
├── SubastaYa.API
│   ├── Controllers
│   ├── Middleware
│   ├── DatabaseSeeder
│   └── Program.cs
│
├── SubastaYa.Application
│   ├── Common
│   ├── Interfaces
│   └── UseCases
│       ├── Auctions
│       ├── Audits
│       ├── Bids
│       ├── Categories
│       ├── LedgerTransactions
│       ├── Users
│       ├── Wallets
│       └── Worker
│
├── SubastaYa.Domain
│   ├── Constants
│   ├── Entities
│   └── Exceptions
│
└── SubastaYa.Infrastructure
    └── Persistence
        ├── Configurations
        ├── Repositories
        ├── AppDbContext.cs
        └── UnitOfWork.cs
```

---

## Estado del proyecto

El backend actualmente cuenta con:

* Gestión de usuarios.
* Autenticación.
* Gestión de billeteras.
* Depósitos.
* Gestión de categorías.
* Creación y consulta de subastas.
* Sistema de pujas.
* Retención y liberación de fondos.
* Transaction Ledger.
* Auditoría.
* Cierre automático de subastas.
* Protección anti-sniping.
* Notificaciones en tiempo real mediante SignalR.
* Persistencia mediante Entity Framework Core.
* SQL Server.
* Migraciones.
* Seeder de datos.
* Swagger.
* CORS.
* Unit of Work.
* Control de concurrencia mediante RowVersion.
