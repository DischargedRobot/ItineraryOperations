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
    }
}
