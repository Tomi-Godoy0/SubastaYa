using Microsoft.EntityFrameworkCore;
using SubastaYa.Domain.Exceptions;
using System.Net;

namespace SubastaYa.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger; //Registra los errores tecnicos de nuestra aplicacion (es decír de errores de producción) y limpia la información que se le muestra al usuario final.

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next(ctx);

            } catch (Exception ex)
            {
                _logger.LogError(ex ,"Ocurrió un error interno no controlado");
                await HandleExceptionAsync(ctx, ex);

            }
        }

        private static Task HandleExceptionAsync(HttpContext ctx, Exception ex)
        {
            ctx.Response.ContentType = "application/json";

            //Vamos a mapear códigos de excepciones a códigos HTTP
            int statusCode = ex switch
            {
                //Bad Request
                ArgumentNullException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status400BadRequest,
                // Not Found
                KeyNotFoundException => StatusCodes.Status404NotFound,
                NotFoundException => StatusCodes.Status404NotFound,
                // Unauthorized
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                // Conflict
                ConflictException => StatusCodes.Status409Conflict,
                DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
                //Insufficient
                InsufficientFundsException => StatusCodes.Status422UnprocessableEntity,
                //Fallback para excepción de dominio no mapeada
                DomainException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            string message = statusCode == StatusCodes.Status500InternalServerError ? "Ocurrió un error interno en el servidor." : ex.Message;

            ctx.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Message = message
            };

            var json = System.Text.Json.JsonSerializer.Serialize(response);
            return ctx.Response.WriteAsync(json);
        }
    }
}
