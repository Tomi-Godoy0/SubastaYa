# Guía de Configuración y Ejecución del Proyecto

Esta guía detalla los pasos necesarios para instalar, configurar y ejecutar la solución completa **SubastaYa (Backend .NET + Frontend Web)** en una computadora nueva.

## 1. Requisitos Previos

### Antes de comenzar, asegúrese de tener instaladas las siguientes herramientas:

* **SDK de .NET 8.0**: Necesario para compilar y ejecutar el backend.
* **Entity Framework Core**: Para el mapeo objeto-relacional y la gestión de la base de datos.
* **SQL Server Express**: Para la persistencia de datos.
* **Node.js**: Requerido para utilizar el servidor de desarrollo del frontend mediante `http-server`.
* **Navegador Web**: Chrome, Edge, Firefox u otro navegador moderno.

### Instalación de Herramientas Globales

Es necesario ejecutar estos comandos para habilitar las funciones de base de datos y el servidor del frontend.

* **Instalar EF Core CLI**: Permite gestionar las migraciones de la base de datos desde la terminal.

```bash
dotnet tool install --global dotnet-ef
```

* **Instalar http-server**: Servidor ligero para levantar el frontend.

```bash
npm install -g http-server
```

> Si alguna de estas herramientas ya está instalada, no es necesario volver a instalarla.

---

## 2. Configuración Inicial

### Clonar y Preparar el Repositorio

1. Descargue o clone el repositorio en una carpeta local.

2. Abra una terminal en la raíz del proyecto.

3. Verifique que la solución tenga una estructura similar a:

```text
SubastaYa/
├── SubastaYa.sln
├── SubastaYa.API/
├── SubastaYa.Application/
├── SubastaYa.Domain/
├── SubastaYa.Infrastructure/
└── Frontend/
```

4. Restaure las dependencias del proyecto:

```bash
dotnet restore
```

5. Compile la solución para verificar que no existan errores:

```bash
dotnet build
```

---

## 3. Base de Datos

El proyecto utiliza **SQL Server Express** y **Entity Framework Core 8**.

### Configuración de la conexión

Verifique la cadena de conexión ubicada en:

```text
SubastaYa.API/appsettings.json
```

La configuración utilizada actualmente es:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=SubastaYaDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Si la instancia de SQL Server del equipo tiene otro nombre, modifique el valor de `Server`.

Por ejemplo:

```text
Server=localhost\SQLEXPRESS
```

o, dependiendo de la instalación:

```text
Server=.\SQLEXPRESS
```

### Crear o actualizar la base de datos

Desde la raíz de la solución puede ejecutar:

```bash
dotnet ef database update --project SubastaYa.Infrastructure --startup-project SubastaYa.API
```

Este comando aplicará las migraciones existentes sobre la base de datos.

Si todavía no existe ninguna migración, puede crear la primera con:

```bash
dotnet ef migrations add InitialCreate --project SubastaYa.Infrastructure --startup-project SubastaYa.API
```

y posteriormente:

```bash
dotnet ef database update --project SubastaYa.Infrastructure --startup-project SubastaYa.API
```

### Seeder

Al iniciar el backend, el proyecto ejecuta automáticamente `DatabaseSeeder`.

El Seeder:

1. Aplica las migraciones pendientes.
2. Comprueba si existen usuarios.
3. Si la base de datos está vacía, crea datos iniciales.
4. Crea usuarios de prueba.
5. Crea billeteras.
6. Crea categorías.
7. Crea subastas.
8. Crea pujas.
9. Crea movimientos del Transaction Ledger.

Por lo tanto, no es necesario cargar manualmente los datos iniciales para comenzar a probar el sistema.

---

## 4. Ejecución del Backend

Desde la raíz del proyecto puede ejecutar:

```bash
dotnet run --project SubastaYa.API
```

El backend iniciará la API y realizará las configuraciones necesarias.

En entorno de desarrollo también estará disponible Swagger para probar los endpoints.

La documentación de Swagger estará disponible en:

```text
/swagger
```

---

## 5. Ejecución del Frontend

El frontend puede ejecutarse utilizando `http-server`.

Abra una terminal en la carpeta donde se encuentra el frontend y ejecute:

```bash
http-server -p 5500
```

El frontend quedará disponible normalmente en:

```text
http://localhost:5500
```

También puede utilizarse:

```text
http://127.0.0.1:5500
```

El backend tiene configurados estos orígenes dentro de su política CORS:

```text
http://127.0.0.1:5500
http://127.0.0.1:5501
```

Por lo tanto, es importante utilizar uno de estos orígenes cuando se pruebe la aplicación desde el navegador.

---

## 6. Ejecución mediante Script (.bat)

Si el repositorio contiene un archivo `Startproject.bat` en la raíz, puede ejecutarse mediante doble clic.

El script puede utilizarse para automatizar el inicio del entorno de desarrollo.

La ejecución esperada es:

```text
1. Iniciar el servidor de .NET.
2. Levantar el servidor del frontend.
3. Abrir el navegador en la dirección del proyecto.
```

Si el proyecto no contiene actualmente el archivo `Startproject.bat`, estos pasos pueden realizarse manualmente siguiendo las secciones anteriores.

---

## 7. Usuarios de Prueba

El Seeder crea usuarios para facilitar las pruebas del sistema.

### Vendedor

```text
Email: vendedor@test.com
Password: contra123
```

### Comprador 1

```text
Email: comprador1@test.com
Password: contra123
```

### Comprador 2

```text
Email: comprador2@test.com
Password: contra123
```

### Usuario sin fondos

```text
Email: sinfondos@test.com
Password: contra123
```

Estos usuarios están destinados exclusivamente al entorno de desarrollo y pruebas.

---

## 8. Funcionalidades Principales

El backend actualmente incluye las siguientes funcionalidades:

* Registro de usuarios.
* Login.
* Gestión de billeteras.
* Consulta de saldo.
* Depósitos.
* Gestión de categorías.
* Creación de subastas.
* Consulta de subastas.
* Sistema de pujas.
* Historial de pujas.
* Retención de fondos.
* Liberación de fondos cuando una puja es superada.
* Transaction Ledger.
* Auditoría de operaciones.
* Cierre automático de subastas.
* Finalización de subastas con ganador.
* Subastas desiertas.
* Extensión automática de tiempo mediante anti-sniping.
* Notificaciones en tiempo real mediante SignalR.
* Paginación de resultados.
* Control de concurrencia mediante `RowVersion`.

---

## 9. SignalR

El sistema utiliza **SignalR** para enviar actualizaciones de las subastas en tiempo real.

El Hub está disponible en:

```text
/hubs/auction
```

Se utiliza principalmente para notificar:

* Nuevas pujas.
* Extensiones de tiempo de una subasta.

---

## 10. Transaction Ledger

Los movimientos relacionados con las billeteras quedan registrados en un ledger.

Los tipos de movimientos utilizados actualmente son:

```text
Deposit
Retention
Release
Pay
Collection
```

Esto permite mantener un historial de las operaciones financieras relacionadas con las subastas y las billeteras.

---

## 11. Auditoría

El sistema registra determinadas operaciones mediante `AuditLog`.

Los registros pueden contener:

```text
Entidad
ID de entidad
Acción
Usuario
Detalle en formato JSON
Fecha de creación
```

Los endpoints disponibles son:

```text
GET /api/audits
GET /api/audits/{id}
```

Las auditorías se utilizan, entre otras operaciones, para registrar:

* Depósitos.
* Puja rechazada.
* Extensión de una subasta.
* Finalización de una subasta.
* Subasta desierta.

---

## 12. Solución de Problemas Comunes

### Error CORS

Si aparece un error relacionado con CORS, verifique que el frontend esté siendo ejecutado desde uno de los orígenes permitidos:

```text
http://127.0.0.1:5500
http://127.0.0.1:5501
```

También puede revisar la configuración de CORS en:

```text
SubastaYa.API/Program.cs
```

---

### ERR_CONNECTION_REFUSED

Verifique que el backend esté ejecutándose.

Ejecute:

```bash
dotnet run --project SubastaYa.API
```

También revise la consola para comprobar que no existan errores relacionados con:

* SQL Server.
* Entity Framework.
* Migraciones.
* Configuración de dependencias.
* Compilación.

---

### Error de conexión con SQL Server

Compruebe que:

1. SQL Server Express esté instalado.
2. El servicio de SQL Server esté iniciado.
3. La instancia se llame `SQLEXPRESS`.
4. La cadena de conexión de `appsettings.json` sea correcta.

La configuración utilizada actualmente es:

```text
Server=localhost\SQLEXPRESS;
Database=SubastaYaDB;
Trusted_Connection=True;
TrustServerCertificate=True;
```

---

### Puerto 5500 ocupado

Si aparece un error como:

```text
EADDRINUSE
```

significa que el puerto `5500` ya está siendo utilizado.

Puede cerrar el proceso que esté utilizando el puerto o ejecutar:

```bash
taskkill /F /IM node.exe
```

Luego vuelva a iniciar el frontend:

```bash
http-server -p 5500
```

---

### Error con Entity Framework

Compruebe que EF Core CLI esté instalado:

```bash
dotnet ef --version
```

Si el comando no existe, instálelo mediante:

```bash
dotnet tool install --global dotnet-ef
```

Luego pruebe nuevamente:

```bash
dotnet ef database update --project SubastaYa.Infrastructure --startup-project SubastaYa.API
```

---

## 13. Tecnologías Utilizadas

### Backend

* C#
* .NET 8.0
* ASP.NET Core Web API
* Entity Framework Core 8
* SQL Server Express
* BCrypt
* SignalR
* Swagger / OpenAPI

### Frontend

* HTML
* CSS
* JavaScript
* `http-server`

### Arquitectura

El backend se encuentra dividido en:

```text
SubastaYa.API
SubastaYa.Application
SubastaYa.Domain
SubastaYa.Infrastructure
```

Se utilizan conceptos y patrones como:

* Arquitectura por capas.
* Dependency Injection.
* Repository Pattern.
* Unit of Work.
* Entity Framework Core.
* Concurrencia optimista.
* Casos de uso mediante Handlers.
* Worker para procesamiento automático.
