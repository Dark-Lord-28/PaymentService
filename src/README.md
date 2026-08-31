# Sistema distribuido: e-commerce + paymentservice

Trabajo final integrador para la materia Backend (Optativa II) - 2026

Arquitectura y tecnologías:

El sistema está compuesto por dos proyectos .NET independientes basados en Clean Architecture:

MiApp WebAPI (e-commerce): Puerto `5004` — Maneja usuarios, autenticación (JWT), roles y ordenes.
PaymentService: Puerto `5005` — Microservicio independiente que procesa cobros y decisiones de pago.

Patrones e integraciones:

CQRS con MediatR: Implementacion de commands, queries y handlers en la capa Application.
Persistencia: EF Core con base de datos SQLite.
Comunicación HTTP: `IHttpClientFactory` (Typed Client) con reintentos y resiliencia mediante `try/catch` ante caídas del servicio externo.
Seguridad: Tokens JWT con restricción de roles mediante `[Authorize(Roles = "Admin")]`.

Regla de Negocio (PaymentService)
El microservicio de pagos evalua el monto de la orden segun la siguiente regla:
* Monto < $100.000: Pago aprobado (`Approved`), genera `TransactionId` (ej: `TX-A90749A9`) y la orden se guarda como `Paid`.
* Monto >= $100.000: Pago rechazado (`Rejected`) y la orden pasa a `PaymentRejected`.
* Servicio Caido / Timeout: La WebAPI atrapa la excepcion (`HttpRequestException`) y la orden pasa a `PaymentRejected` sin romper la ejecucion del servidor.

Como ejecutar el proyecto:

1. Levantar PaymentService:
```bash
cd PaymentService
dotnet run --project src/PaymentService.WebApi ("http://localhost:5005")

```
2. Levantar MiApp WebAPI:
```bash
cd MiApp
dotnet run --project src/MiApp.WebApi ("http://localhost:5004")

```

3. Credenciales y prueba de Endpoints:

Usuario Admin para pruebas:
Email: elias@gmail.com
Password: 12345

Flujo completo End-to-End en Swagger (http://localhost:5004/swagger):

Iniciar sesion en POST /api/Auth/login enviando las credenciales para obtener el token JWT.

Hacer clic en el botón "Authorize" e ingresar el token.

Probar la creación de ordenes en POST /api/Ordenes:

Monto $45.000 -> Devuelve estado Paid y un transactionId.

Monto $150.000 -> Devuelve estado PaymentRejected.

Con PaymentService detenido (Ctrl+C) -> Devuelve estado PaymentRejected mediante el manejo de resiliencia HTTP en el Handler.


