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
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, // Защита от кросдомена Strict - запрещает крос, None позволяет
                Expires = newSession.Finished,  // Время истечения
                Path = "/",                // Для каких запроcов доступен 
            };

            Console.WriteLine("setSessionId", newSession.Id);
            Response.Cookies.Append("SessionId", newSession.Id.ToString(), cookieOptions);

        }

        public class AuthorizationRequest
        {
            public required string Login { get; set; }
            public required string Password { get; set; }
        }

        [HttpPost("auth")]
        public async Task<ActionResult<UsersDto>> Authorization(
            [FromBody] AuthorizationRequest request
        )
        {
            try
            {
                Users? user = _context.Users.FirstOrDefault(user => user.Login == request.Login);

                if (user == null || request.Password != user.Password)
                {
                    return Unauthorized(new APIError { Message = "Пользователя с таким логином и паролем не существует" });
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
                    return StatusCode(500, "Internal server error");
                }

                SetSessionCookie(newSessionNew, Response);
                Console.WriteLine("setSessionIdCookies", Convert.ToString(Response.Cookies));

                return Ok(new UsersDto(user));
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("logout")]
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

                return Ok(new { message = "Successfully logged out" });
                
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }

    
}