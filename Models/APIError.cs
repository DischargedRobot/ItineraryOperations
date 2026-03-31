using ItineraryOperations.Models;
using Swashbuckle.AspNetCore.Filters;

namespace ItineraryOperations.Models
{
    public class APIError
    {
        public string Message { get; set; }
        public APIError()
        {
        }
    }

public class APIError404Example : IExamplesProvider<APIError>
{
    public APIError GetExamples()
    {
        return new APIError
        {
            Message = "Объект не найден",
        };
    }
}

public class APIError400Example : IExamplesProvider<APIError>
{
    public APIError GetExamples()
    {
        return new APIError
        {
            Message = "Неверные данные запроса",
        };
    }
}

public class APIError500Example : IExamplesProvider<APIError>
{
    public APIError GetExamples()
    {
        return new APIError
        {
            Message = "Внутренняя ошибка сервера",
        };
    }
}
}


