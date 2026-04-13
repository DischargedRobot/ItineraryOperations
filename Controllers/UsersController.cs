using ItineraryOperations.Lib;
using ItineraryOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ItineraryOperations.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly PostgresContext _context;

        private readonly ILogger<ExecutorsController> _logger;


        public UsersController(ILogger<ExecutorsController> logger, PostgresContext context)
        {
            _context = context;
            _logger = logger;
        }
        private void SetSessionCookie(UserSession newSession, HttpResponse Response)
        {
            CheckSessionFunctions.SetSessionCookie(newSession, Response);
        }

        public class AuthorizationRequest
        {
            public required string Login { get; set; }
            public required string Password { get; set; }
        }

        [HttpPost("auth")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<UsersDto>> Authorization(
            [FromBody] AuthorizationRequest request
        )
        {
            try
            {
                Users? user = _context.Users.FirstOrDefault(user => user.Login == request.Login);

                if (user == null || request.Password != user.Password)
                {
                    Console.WriteLine($"{user} is user for login: {request.Login}, password: {request.Password}");
                    return NotFound(new APIError { Message = "Пользователя с таким логином и паролем не существует" });
                }

                UserSession? session = await _context.UserSessions
                    .FirstOrDefaultAsync(session => session.UserId == user.ID);

                if (session != null)
                {
                    _context.UserSessions.Remove(session);
                }

                UserSession newSession = new UserSession(user);

                await _context.UserSessions.AddAsync(newSession);
                await _context.SaveChangesAsync();

                UserSession? newSessionNew = await _context.UserSessions.FirstOrDefaultAsync(session => session.Id == newSession.Id);
                
                if (newSessionNew == null )
                {
                    return StatusCode(500, "Внутренняя ошибка сервера");
                }

                SetSessionCookie(newSessionNew, Response);
                Console.WriteLine("setSessionIdCookies", Convert.ToString(Response.Cookies));

                return Ok(new UsersDto(user));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while authorization");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpPost("logout")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<UsersDto>> Logout(
        )
        {
            try
            {
                var coockieSession = Request.Cookies["SessionId"]?.ToString();
                Response.Cookies.Delete("SessionId");

                if (int.TryParse(coockieSession, out int sessionId))
                {
                    var sessionToRemove = new UserSession { Id = Convert.ToInt32(sessionId) };
                    _context.UserSessions.Remove(sessionToRemove);
                    await _context.SaveChangesAsync();

                }

                return Ok(new { message = "Выход выполнен успешно" });
                
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while logout");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpGet("me")]
        public async Task<ActionResult<UsersDto>> Me()
        {
            try
            {
                var result = await CheckSessionFunctions.CheckAndRefreshSession(Request, Response, _context);

                if (!result.IsValid)
                {
                    return Unauthorized(new APIError { Message = result.ErrorMessage ?? "Ошибка аутентификации" });
                }

                if (result.User == null)
                {
                    return NotFound(new APIError { Message = "Пользователь не найден" });
                }

                return Ok(new UsersDto(result.User));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting current user by session");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpPost("refresh")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<UsersDto>> Refresh()
        {
            try
            {
                var result = await CheckSessionFunctions.CheckAndRefreshSession(Request, Response, _context);

                if (!result.IsValid)
                {
                    return Unauthorized(new APIError { Message = result.ErrorMessage ?? "Ошибка аутентификации" });
                }

                if (result.User == null)
                {
                    return NotFound(new APIError { Message = "Пользователь не найден" });
                }

                return Ok(new UsersDto(result.User));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while refreshing session");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }

    
}