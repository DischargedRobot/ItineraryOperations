using ItineraryOperations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ItineraryOperations.Lib
{
    public class AuthFilter : IAsyncActionFilter
    {
        private readonly PostgresContext _context;
        private readonly ILogger<AuthFilter> _logger;

        public AuthFilter(PostgresContext context, ILogger<AuthFilter> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Пропускаем методы помеченные AllowAnonymous.
            var endpoint = context.HttpContext.GetEndpoint();
            var hasAllowAnonymous = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>() != null;
            if (hasAllowAnonymous)
            {
                await next();
                return;
            }

            try
            {
                var sessionCookie = context.HttpContext.Request.Cookies["SessionId"];
                _logger.LogWarning("AuthFilter: path={Path}, SessionId cookie={Cookie}", 
                    context.HttpContext.Request.Path, sessionCookie ?? "ОТСУТСТВУЕТ");

                var result = await CheckSessionFunctions.CheckAndRefreshSession(context.HttpContext.Request, context.HttpContext.Response, _context);

                _logger.LogWarning("AuthFilter: IsValid={IsValid}, Error={Error}", result.IsValid, result.ErrorMessage ?? "—");

                if (!result.IsValid)
                {
                    context.Result = new UnauthorizedObjectResult(new Models.APIError { Message = result.ErrorMessage ?? "Ошибка аутентификации" });
                    return;
                }

                // кладём пользователя в HttpContext.Items для дальнейшего использования в действиях
                context.HttpContext.Items["CurrentUser"] = result.User;

                await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AuthFilter error");
                context.Result = new StatusCodeResult(500);
            }
        }
    }
}
