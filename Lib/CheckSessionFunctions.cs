using ItineraryOperations.Models;
using Microsoft.EntityFrameworkCore;

namespace ItineraryOperations.Lib
{
    public class CheckSessionFunctions
    {
        public static async Task<bool> CheckSession(HttpRequest request, PostgresContext context)
        {
            Console.WriteLine(request.Cookies["SessionId"]?.ToString() + "  SessionId");

            if (request.Cookies["SessionId"] != null)
            {
                int sessionId = Convert.ToInt32(request.Cookies["SessionId"]);
                UserSession? sessionIdInServer = await context.UserSessions.FirstOrDefaultAsync(session => session.Id == sessionId);
                Console.WriteLine(sessionIdInServer + "  SsdfsdfdsfsdessionId");

                return sessionIdInServer != null;
            }
            else return false;
            
        }

        public class SessionCheckResult
        {
            public bool IsValid { get; set; }
            public Users? User { get; set; }
            public string? ErrorMessage { get; set; }
        }

        public static void SetSessionCookie(UserSession session, HttpResponse response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = session.Finished,
                Path = "/",
            };
            response.Cookies.Append("SessionId", session.Id.ToString(), cookieOptions);
        }

        public static async Task<SessionCheckResult> CheckAndRefreshSession(
            HttpRequest request,
            HttpResponse response,
            PostgresContext context)
        {
            var cookieSession = request.Cookies["SessionId"];

            if (!int.TryParse(cookieSession, out int sessionId))
            {
                return new SessionCheckResult
                {
                    IsValid = false,
                    ErrorMessage = "Сессия не найдена"
                };
            }

            UserSession? session = await context.UserSessions
                .FirstOrDefaultAsync(userSession => userSession.Id == sessionId);

            if (session == null)
            {
                response.Cookies.Delete("SessionId");
                return new SessionCheckResult
                {
                    IsValid = false,
                    ErrorMessage = "Сессия не найдена"
                };
            }

            if (session.Finished <= DateTime.UtcNow)
            {
                context.UserSessions.Remove(session);
                await context.SaveChangesAsync();
                response.Cookies.Delete("SessionId");
                return new SessionCheckResult
                {
                    IsValid = false,
                    ErrorMessage = "Сессия истекла"
                };
            }

            // Рефрешим сессию
            session.Finished = DateTime.UtcNow.AddMonths(1);
            context.UserSessions.Update(session);
            await context.SaveChangesAsync();
            SetSessionCookie(session, response);

            // Получаем пользователя
            Users? user = await context.Users.FirstOrDefaultAsync(u => u.ID == session.UserId);

            if (user == null)
            {
                return new SessionCheckResult
                {
                    IsValid = false,
                    ErrorMessage = "Пользователь не найден"
                };
            }

            return new SessionCheckResult
            {
                IsValid = true,
                User = user
            };
        }
    }
}
